using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.Common;
using Microsoft.Data.ProviderBase;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

[DefaultEvent("InfoMessage")]
[DesignerCategory("")]
public sealed class SqlConnection : DbConnection, ICloneable
{
	private class OpenAsyncRetry
	{
		private SqlConnection _parent;

		private TaskCompletionSource<DbConnectionInternal> _retry;

		private TaskCompletionSource<object> _result;

		private CancellationTokenRegistration _registration;

		public OpenAsyncRetry(SqlConnection parent, TaskCompletionSource<DbConnectionInternal> retry, TaskCompletionSource<object> result, CancellationTokenRegistration registration)
		{
			_parent = parent;
			_retry = retry;
			_result = result;
			_registration = registration;
		}

		internal void Retry(Task<DbConnectionInternal> retryTask)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.OpenAsyncRetry|Info> {0}", _parent.ObjectID);
			_registration.Dispose();
			try
			{
				SqlStatistics statistics = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					statistics = SqlStatistics.StartTimer(_parent.Statistics);
					if (retryTask.IsFaulted)
					{
						Exception innerException = retryTask.Exception.InnerException;
						_parent.CloseInnerConnection();
						_parent._currentCompletion = null;
						_result.SetException(retryTask.Exception.InnerException);
						return;
					}
					if (retryTask.IsCanceled)
					{
						_parent.CloseInnerConnection();
						_parent._currentCompletion = null;
						_result.SetCanceled();
						return;
					}
					bool flag;
					lock (_parent.InnerConnection)
					{
						flag = _parent.TryOpen(_retry);
					}
					if (flag)
					{
						_parent._currentCompletion = null;
						_result.SetResult(null);
					}
					else
					{
						_parent.CloseInnerConnection();
						_parent._currentCompletion = null;
						_result.SetException(ADP.ExceptionWithStackTrace(ADP.InternalError(ADP.InternalErrorCode.CompletedConnectReturnedPending)));
					}
				}
				finally
				{
					SqlStatistics.StopTimer(statistics);
				}
			}
			catch (Exception exception)
			{
				_parent.CloseInnerConnection();
				_parent._currentCompletion = null;
				_result.SetException(exception);
			}
		}
	}

	internal bool _suppressStateChangeForReconnection;

	private static readonly object EventInfoMessage = new object();

	private static Dictionary<string, SqlColumnEncryptionKeyStoreProvider> s_systemColumnEncryptionKeyStoreProviders = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>(3, StringComparer.OrdinalIgnoreCase)
	{
		{
			"MSSQL_CERTIFICATE_STORE",
			new SqlColumnEncryptionCertificateStoreProvider()
		},
		{
			"MSSQL_CNG_STORE",
			new SqlColumnEncryptionCngProvider()
		},
		{
			"MSSQL_CSP_PROVIDER",
			new SqlColumnEncryptionCspProvider()
		}
	};

	private static IReadOnlyDictionary<string, SqlColumnEncryptionKeyStoreProvider> s_globalCustomColumnEncryptionKeyStoreProviders;

	private IReadOnlyDictionary<string, SqlColumnEncryptionKeyStoreProvider> _customColumnEncryptionKeyStoreProviders;

	private static readonly object s_globalCustomColumnEncryptionKeyProvidersLock = new object();

	private static readonly ConcurrentDictionary<string, IList<string>> _ColumnEncryptionTrustedMasterKeyPaths = new ConcurrentDictionary<string, IList<string>>(4 * Environment.ProcessorCount, 1, StringComparer.OrdinalIgnoreCase);

	private static bool _ColumnEncryptionQueryMetadataCacheEnabled = true;

	private static TimeSpan _ColumnEncryptionKeyCacheTtl = TimeSpan.FromHours(2.0);

	private SqlDebugContext _sdc;

	private bool _AsyncCommandInProgress;

	internal SqlStatistics _statistics;

	private bool _collectstats;

	private bool _fireInfoMessageEventOnUserErrors;

	private Tuple<TaskCompletionSource<DbConnectionInternal>, Task> _currentCompletion;

	private SqlCredential _credential;

	private string _connectionString;

	private int _connectRetryCount;

	private string _accessToken;

	private object _reconnectLock = new object();

	internal Task _currentReconnectionTask;

	private Task _asyncWaitingForReconnection;

	private Guid _originalConnectionId = Guid.Empty;

	private CancellationTokenSource _reconnectionCancellationSource;

	internal SessionData _recoverySessionData;

	internal WindowsIdentity _lastIdentity;

	internal WindowsIdentity _impersonateIdentity;

	private int _reconnectCount;

	private ServerCertificateValidationCallback _serverCertificateValidationCallback;

	private ClientCertificateRetrievalCallback _clientCertificateRetrievalCallback;

	private SqlClientOriginalNetworkAddressInfo _originalNetworkAddressInfo;

	private SqlRetryLogicBaseProvider _retryLogicProvider;

	internal bool _applyTransientFaultHandling;

	private static readonly DbConnectionFactory _connectionFactory = SqlConnectionFactory.SingletonInstance;

	internal static readonly CodeAccessPermission ExecutePermission = CreateExecutePermission();

	private DbConnectionOptions _userConnectionOptions;

	private DbConnectionPoolGroup _poolGroup;

	private DbConnectionInternal _innerConnection;

	private int _closeCount;

	private static int _objectTypeCount;

	internal readonly int ObjectID = Interlocked.Increment(ref _objectTypeCount);

	internal bool ForceNewConnection { get; set; }

	internal bool HasColumnEncryptionKeyStoreProvidersRegistered
	{
		get
		{
			if (_customColumnEncryptionKeyStoreProviders != null)
			{
				return _customColumnEncryptionKeyStoreProviders.Count > 0;
			}
			return false;
		}
	}

	[DefaultValue(null)]
	[ResCategory("Data")]
	[ResDescription("Dictionary object containing SQL Server names and their trusted column master key paths.")]
	public static IDictionary<string, IList<string>> ColumnEncryptionTrustedMasterKeyPaths => _ColumnEncryptionTrustedMasterKeyPaths;

	[DefaultValue(null)]
	[ResCategory("Data")]
	[ResDescription("Defines whether query metadata caching is enabled.")]
	public static bool ColumnEncryptionQueryMetadataCacheEnabled
	{
		get
		{
			return _ColumnEncryptionQueryMetadataCacheEnabled;
		}
		set
		{
			_ColumnEncryptionQueryMetadataCacheEnabled = value;
		}
	}

	[DefaultValue(null)]
	[ResCategory("Data")]
	[ResDescription("Defines the time-to-live of entries in the column encryption key cache.")]
	public static TimeSpan ColumnEncryptionKeyCacheTtl
	{
		get
		{
			return _ColumnEncryptionKeyCacheTtl;
		}
		set
		{
			_ColumnEncryptionKeyCacheTtl = value;
		}
	}

	private static bool IsRetryEnabled => LocalAppContextSwitches.IsRetryEnabled;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SqlRetryLogicBaseProvider RetryLogicProvider
	{
		get
		{
			if (_retryLogicProvider == null)
			{
				_retryLogicProvider = SqlConfigurableRetryLogicManager.ConnectionProvider;
			}
			return _retryLogicProvider;
		}
		set
		{
			_retryLogicProvider = value;
		}
	}

	[DefaultValue(false)]
	[ResCategory("Data")]
	[ResDescription("Collect statistics for this connection.")]
	public bool StatisticsEnabled
	{
		get
		{
			return _collectstats;
		}
		set
		{
			if (IsContextConnection)
			{
				if (value)
				{
					throw SQL.NotAvailableOnContextConnection();
				}
				return;
			}
			if (value)
			{
				if (ConnectionState.Open == State)
				{
					if (_statistics == null)
					{
						_statistics = new SqlStatistics();
						_statistics._openTimestamp = ADP.TimerCurrent();
					}
					Parser.Statistics = _statistics;
				}
			}
			else if (_statistics != null && ConnectionState.Open == State)
			{
				TdsParser parser = Parser;
				parser.Statistics = null;
				_statistics._closeTimestamp = ADP.TimerCurrent();
			}
			_collectstats = value;
		}
	}

	internal bool AsyncCommandInProgress
	{
		get
		{
			return _AsyncCommandInProgress;
		}
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		set
		{
			_AsyncCommandInProgress = value;
		}
	}

	internal bool IsContextConnection
	{
		get
		{
			SqlConnectionString opt = (SqlConnectionString)ConnectionOptions;
			return UsesContextConnection(opt);
		}
	}

	internal bool IsColumnEncryptionSettingEnabled
	{
		get
		{
			SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
			if (sqlConnectionString == null)
			{
				return false;
			}
			return sqlConnectionString.ColumnEncryptionSetting == SqlConnectionColumnEncryptionSetting.Enabled;
		}
	}

	internal string EnclaveAttestationUrl
	{
		get
		{
			SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
			return sqlConnectionString.EnclaveAttestationUrl;
		}
	}

	internal SqlConnectionAttestationProtocol AttestationProtocol
	{
		get
		{
			SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
			return sqlConnectionString.AttestationProtocol;
		}
	}

	internal SqlConnectionIPAddressPreference iPAddressPreference => ((SqlConnectionString)ConnectionOptions).IPAddressPreference;

	internal SqlConnectionString.TransactionBindingEnum TransactionBinding => ((SqlConnectionString)ConnectionOptions).TransactionBinding;

	internal SqlConnectionString.TypeSystem TypeSystem => ((SqlConnectionString)ConnectionOptions).TypeSystemVersion;

	internal Version TypeSystemAssemblyVersion => ((SqlConnectionString)ConnectionOptions).TypeSystemAssemblyVersion;

	internal PoolBlockingPeriod PoolBlockingPeriod => ((SqlConnectionString)ConnectionOptions).PoolBlockingPeriod;

	internal int ConnectRetryInterval => ((SqlConnectionString)ConnectionOptions).ConnectRetryInterval;

	protected override DbProviderFactory DbProviderFactory => SqlClientFactory.Instance;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("Access token to use for authentication.")]
	public string AccessToken
	{
		get
		{
			string result = _accessToken;
			SqlConnectionString sqlConnectionString = (SqlConnectionString)UserConnectionOptions;
			if (InnerConnection.ShouldHidePassword && sqlConnectionString != null && !sqlConnectionString.PersistSecurityInfo)
			{
				result = null;
			}
			return result;
		}
		set
		{
			if (!InnerConnection.AllowSetConnectionString)
			{
				throw ADP.OpenConnectionPropertySet("AccessToken", InnerConnection.State);
			}
			if (value != null)
			{
				CheckAndThrowOnInvalidCombinationOfConnectionOptionAndAccessToken((SqlConnectionString)ConnectionOptions);
			}
			_accessToken = value;
			ConnectionString_Set(new SqlConnectionPoolKey(_connectionString, _credential, _accessToken, _serverCertificateValidationCallback, _clientCertificateRetrievalCallback, _originalNetworkAddressInfo));
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("Current connection timeout value, 'Connect Timeout=X' in the ConnectionString.")]
	public int CommandTimeout => ((SqlConnectionString)ConnectionOptions)?.CommandTimeout ?? 30;

	[DefaultValue("")]
	[RecommendedAsConfigurable(true)]
	[SettingsBindable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[ResCategory("Data")]
	[Editor("Microsoft.VSDesigner.Data.SQL.Design.SqlConnectionStringEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ResDescription("Information used to connect to a DataSource, such as 'Data Source=x;Initial Catalog=x;Integrated Security=SSPI'.")]
	public override string ConnectionString
	{
		get
		{
			return ConnectionString_Get();
		}
		set
		{
			if (_credential != null || _accessToken != null)
			{
				SqlConnectionString sqlConnectionString = new SqlConnectionString(value);
				if (_credential != null)
				{
					if (UsesActiveDirectoryIntegrated(sqlConnectionString))
					{
						throw SQL.SettingIntegratedWithCredential();
					}
					if (UsesActiveDirectoryInteractive(sqlConnectionString))
					{
						throw SQL.SettingInteractiveWithCredential();
					}
					if (UsesActiveDirectoryDeviceCodeFlow(sqlConnectionString))
					{
						throw SQL.SettingDeviceFlowWithCredential();
					}
					if (UsesActiveDirectoryManagedIdentity(sqlConnectionString))
					{
						throw SQL.SettingNonInteractiveWithCredential("Active Directory Managed Identity");
					}
					if (UsesActiveDirectoryMSI(sqlConnectionString))
					{
						throw SQL.SettingNonInteractiveWithCredential("Active Directory MSI");
					}
					if (UsesActiveDirectoryDefault(sqlConnectionString))
					{
						throw SQL.SettingNonInteractiveWithCredential("Active Directory Default");
					}
					CheckAndThrowOnInvalidCombinationOfConnectionStringAndSqlCredential(sqlConnectionString);
				}
				else if (_accessToken != null)
				{
					CheckAndThrowOnInvalidCombinationOfConnectionOptionAndAccessToken(sqlConnectionString);
				}
			}
			ConnectionString_Set(new SqlConnectionPoolKey(value, _credential, _accessToken, _serverCertificateValidationCallback, _clientCertificateRetrievalCallback, _originalNetworkAddressInfo));
			_connectionString = value;
			CacheConnectionStringProperties();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("Current connection timeout value, 'Connect Timeout=X' in the ConnectionString.")]
	public override int ConnectionTimeout => ((SqlConnectionString)ConnectionOptions)?.ConnectTimeout ?? 15;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("Current SQL Server database, 'Initial Catalog=X' in the connection string.")]
	public override string Database
	{
		get
		{
			if (!(InnerConnection is SqlInternalConnection { CurrentDatabase: var currentDatabase }))
			{
				SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
				return (sqlConnectionString != null) ? sqlConnectionString.InitialCatalog : "";
			}
			return currentDatabase;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal string SQLDNSCachingSupportedState
	{
		get
		{
			if (InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds)
			{
				return sqlInternalConnectionTds.IsSQLDNSCachingSupported ? "true" : "false";
			}
			return "innerConnection is null!";
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal string SQLDNSCachingSupportedStateBeforeRedirect
	{
		get
		{
			if (InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds)
			{
				return sqlInternalConnectionTds.IsDNSCachingBeforeRedirectSupported ? "true" : "false";
			}
			return "innerConnection is null!";
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("Current SqlServer that the connection is opened to, 'Data Source=X' in the connection string.")]
	public override string DataSource
	{
		get
		{
			if (!(InnerConnection is SqlInternalConnection { CurrentDataSource: var currentDataSource }))
			{
				SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
				return (sqlConnectionString != null) ? sqlConnectionString.DataSource : "";
			}
			return currentDataSource;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResCategory("Data")]
	[ResDescription("Network packet size, 'Packet Size=x' in the connection string.")]
	public int PacketSize
	{
		get
		{
			if (IsContextConnection)
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			if (!(InnerConnection is SqlInternalConnectionTds { PacketSize: var packetSize }))
			{
				return ((SqlConnectionString)ConnectionOptions)?.PacketSize ?? 8000;
			}
			return packetSize;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResCategory("Data")]
	[ResDescription("A guid to represent the physical connection.")]
	public Guid ClientConnectionId
	{
		get
		{
			if (InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds)
			{
				return sqlInternalConnectionTds.ClientConnectionId;
			}
			Task currentReconnectionTask = _currentReconnectionTask;
			DbConnectionClosedPreviouslyOpened dbConnectionClosedPreviouslyOpened = InnerConnection as DbConnectionClosedPreviouslyOpened;
			if ((currentReconnectionTask != null && !currentReconnectionTask.IsCompleted) || dbConnectionClosedPreviouslyOpened != null)
			{
				return _originalConnectionId;
			}
			return Guid.Empty;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("Version of the SQL Server accessed by the SqlConnection.")]
	public override string ServerVersion => GetOpenConnection().ServerVersion;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("Server Process Id (SPID) of the active connection.")]
	public int ServerProcessId
	{
		get
		{
			if (!(State.Equals(ConnectionState.Open) | State.Equals(ConnectionState.Executing) | State.Equals(ConnectionState.Fetching)))
			{
				return 0;
			}
			return GetOpenTdsConnection().ServerProcessId;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("The ConnectionState indicating whether the connection is open or closed.")]
	public override ConnectionState State
	{
		get
		{
			Task currentReconnectionTask = _currentReconnectionTask;
			if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
			{
				return ConnectionState.Open;
			}
			return InnerConnection.State;
		}
	}

	internal SqlStatistics Statistics => _statistics;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResCategory("Data")]
	[ResDescription("Workstation Id, 'Workstation ID=x' in the connection string.")]
	public string WorkstationId
	{
		get
		{
			if (IsContextConnection)
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			string text = ((SqlConnectionString)ConnectionOptions)?.WorkstationId;
			if (text == null)
			{
				text = Environment.MachineName;
			}
			return text;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("User Id and secure password to use for authentication.")]
	public SqlCredential Credential
	{
		get
		{
			SqlCredential result = _credential;
			SqlConnectionString sqlConnectionString = (SqlConnectionString)UserConnectionOptions;
			if (InnerConnection.ShouldHidePassword && sqlConnectionString != null && !sqlConnectionString.PersistSecurityInfo)
			{
				result = null;
			}
			return result;
		}
		set
		{
			if (!InnerConnection.AllowSetConnectionString)
			{
				throw ADP.OpenConnectionPropertySet("Credential", InnerConnection.State);
			}
			if (value != null)
			{
				SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
				if (UsesActiveDirectoryIntegrated(sqlConnectionString))
				{
					throw SQL.SettingCredentialWithIntegratedInvalid();
				}
				if (UsesActiveDirectoryInteractive(sqlConnectionString))
				{
					throw SQL.SettingCredentialWithInteractiveInvalid();
				}
				if (UsesActiveDirectoryDeviceCodeFlow(sqlConnectionString))
				{
					throw SQL.SettingCredentialWithDeviceFlowInvalid();
				}
				if (UsesActiveDirectoryManagedIdentity(sqlConnectionString))
				{
					throw SQL.SettingCredentialWithNonInteractiveInvalid("Active Directory Managed Identity");
				}
				if (UsesActiveDirectoryMSI(sqlConnectionString))
				{
					throw SQL.SettingCredentialWithNonInteractiveInvalid("Active Directory MSI");
				}
				if (UsesActiveDirectoryDefault(sqlConnectionString))
				{
					throw SQL.SettingCredentialWithNonInteractiveInvalid("Active Directory Default");
				}
				CheckAndThrowOnInvalidCombinationOfConnectionStringAndSqlCredential(sqlConnectionString);
				if (_accessToken != null)
				{
					throw ADP.InvalidMixedUsageOfCredentialAndAccessToken();
				}
			}
			_credential = value;
			ConnectionString_Set(new SqlConnectionPoolKey(_connectionString, _credential, _accessToken, _serverCertificateValidationCallback, _clientCertificateRetrievalCallback, _originalNetworkAddressInfo));
		}
	}

	public bool FireInfoMessageEventOnUserErrors
	{
		get
		{
			return _fireInfoMessageEventOnUserErrors;
		}
		set
		{
			_fireInfoMessageEventOnUserErrors = value;
		}
	}

	internal int ReconnectCount => _reconnectCount;

	internal bool HasLocalTransaction => GetOpenConnection().HasLocalTransaction;

	internal bool HasLocalTransactionFromAPI
	{
		get
		{
			Task currentReconnectionTask = _currentReconnectionTask;
			if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
			{
				return false;
			}
			return GetOpenConnection().HasLocalTransactionFromAPI;
		}
	}

	internal bool IsShiloh
	{
		get
		{
			if (_currentReconnectionTask != null)
			{
				return true;
			}
			return GetOpenConnection().IsShiloh;
		}
	}

	internal bool IsYukonOrNewer
	{
		get
		{
			if (_currentReconnectionTask != null)
			{
				return true;
			}
			return GetOpenConnection().IsYukonOrNewer;
		}
	}

	internal bool IsKatmaiOrNewer
	{
		get
		{
			if (_currentReconnectionTask != null)
			{
				return true;
			}
			return GetOpenConnection().IsKatmaiOrNewer;
		}
	}

	internal TdsParser Parser
	{
		get
		{
			if (!(GetOpenConnection() is SqlInternalConnectionTds sqlInternalConnectionTds))
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			return sqlInternalConnectionTds.Parser;
		}
	}

	internal bool Asynchronous => ((SqlConnectionString)ConnectionOptions)?.Asynchronous ?? false;

	internal int CloseCount => _closeCount;

	internal DbConnectionFactory ConnectionFactory => _connectionFactory;

	internal DbConnectionOptions ConnectionOptions => PoolGroup?.ConnectionOptions;

	internal DbConnectionInternal InnerConnection => _innerConnection;

	internal DbConnectionPoolGroup PoolGroup
	{
		get
		{
			return _poolGroup;
		}
		set
		{
			_poolGroup = value;
		}
	}

	internal DbConnectionOptions UserConnectionOptions => _userConnectionOptions;

	[ResCategory("InfoMessage")]
	[ResDescription("Event triggered when messages arrive from the DataSource.")]
	public event SqlInfoMessageEventHandler InfoMessage
	{
		add
		{
			base.Events.AddHandler(EventInfoMessage, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventInfoMessage, value);
		}
	}

	public static void RegisterColumnEncryptionKeyStoreProviders(IDictionary<string, SqlColumnEncryptionKeyStoreProvider> customProviders)
	{
		ValidateCustomProviders(customProviders);
		lock (s_globalCustomColumnEncryptionKeyProvidersLock)
		{
			if (s_globalCustomColumnEncryptionKeyStoreProviders != null)
			{
				throw SQL.CanOnlyCallOnce();
			}
			foreach (SqlColumnEncryptionKeyStoreProvider value in customProviders.Values)
			{
				value.ColumnEncryptionKeyCacheTtl = new TimeSpan(0L);
			}
			Dictionary<string, SqlColumnEncryptionKeyStoreProvider> dictionary = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>(customProviders, StringComparer.OrdinalIgnoreCase);
			s_globalCustomColumnEncryptionKeyStoreProviders = dictionary;
		}
	}

	public void RegisterColumnEncryptionKeyStoreProvidersOnConnection(IDictionary<string, SqlColumnEncryptionKeyStoreProvider> customProviders)
	{
		ValidateCustomProviders(customProviders);
		Dictionary<string, SqlColumnEncryptionKeyStoreProvider> customColumnEncryptionKeyStoreProviders = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>(customProviders, StringComparer.OrdinalIgnoreCase);
		_customColumnEncryptionKeyStoreProviders = customColumnEncryptionKeyStoreProviders;
	}

	private static void ValidateCustomProviders(IDictionary<string, SqlColumnEncryptionKeyStoreProvider> customProviders)
	{
		if (customProviders == null)
		{
			throw SQL.NullCustomKeyStoreProviderDictionary();
		}
		foreach (string key in customProviders.Keys)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw SQL.EmptyProviderName();
			}
			if (key.StartsWith("MSSQL_", StringComparison.InvariantCultureIgnoreCase))
			{
				throw SQL.InvalidCustomKeyStoreProviderName(key, "MSSQL_");
			}
			if (customProviders[key] == null)
			{
				throw SQL.NullProviderValue(key);
			}
		}
	}

	internal static bool TryGetSystemColumnEncryptionKeyStoreProvider(string keyStoreName, out SqlColumnEncryptionKeyStoreProvider provider)
	{
		return s_systemColumnEncryptionKeyStoreProviders.TryGetValue(keyStoreName, out provider);
	}

	internal bool TryGetColumnEncryptionKeyStoreProvider(string providerName, out SqlColumnEncryptionKeyStoreProvider columnKeyStoreProvider)
	{
		if (HasColumnEncryptionKeyStoreProvidersRegistered)
		{
			return _customColumnEncryptionKeyStoreProviders.TryGetValue(providerName, out columnKeyStoreProvider);
		}
		lock (s_globalCustomColumnEncryptionKeyProvidersLock)
		{
			if (s_globalCustomColumnEncryptionKeyStoreProviders == null)
			{
				columnKeyStoreProvider = null;
				return false;
			}
			return s_globalCustomColumnEncryptionKeyStoreProviders.TryGetValue(providerName, out columnKeyStoreProvider);
		}
	}

	internal static List<string> GetColumnEncryptionSystemKeyStoreProvidersNames()
	{
		return s_systemColumnEncryptionKeyStoreProviders.Keys.ToList();
	}

	internal List<string> GetColumnEncryptionCustomKeyStoreProvidersNames()
	{
		if (_customColumnEncryptionKeyStoreProviders != null && _customColumnEncryptionKeyStoreProviders.Count > 0)
		{
			return _customColumnEncryptionKeyStoreProviders.Keys.ToList();
		}
		if (s_globalCustomColumnEncryptionKeyStoreProviders != null)
		{
			return s_globalCustomColumnEncryptionKeyStoreProviders.Keys.ToList();
		}
		return new List<string>();
	}

	public SqlConnection(string connectionString)
		: this(connectionString, null)
	{
	}

	public SqlConnection(string connectionString, SqlCredential credential)
		: this()
	{
		ConnectionString = connectionString;
		if (credential != null)
		{
			SqlConnectionString opt = (SqlConnectionString)ConnectionOptions;
			if (UsesClearUserIdOrPassword(opt))
			{
				throw ADP.InvalidMixedArgumentOfSecureAndClearCredential();
			}
			if (UsesIntegratedSecurity(opt))
			{
				throw ADP.InvalidMixedArgumentOfSecureCredentialAndIntegratedSecurity();
			}
			if (UsesContextConnection(opt))
			{
				throw ADP.InvalidMixedArgumentOfSecureCredentialAndContextConnection();
			}
			if (UsesActiveDirectoryIntegrated(opt))
			{
				throw SQL.SettingCredentialWithIntegratedArgument();
			}
			if (UsesActiveDirectoryInteractive(opt))
			{
				throw SQL.SettingCredentialWithInteractiveArgument();
			}
			if (UsesActiveDirectoryDeviceCodeFlow(opt))
			{
				throw SQL.SettingCredentialWithDeviceFlowArgument();
			}
			if (UsesActiveDirectoryManagedIdentity(opt))
			{
				throw SQL.SettingCredentialWithNonInteractiveArgument("Active Directory Managed Identity");
			}
			if (UsesActiveDirectoryMSI(opt))
			{
				throw SQL.SettingCredentialWithNonInteractiveArgument("Active Directory MSI");
			}
			if (UsesActiveDirectoryDefault(opt))
			{
				throw SQL.SettingCredentialWithNonInteractiveArgument("Active Directory Default");
			}
			Credential = credential;
		}
	}

	private SqlConnection(SqlConnection connection)
	{
		GC.SuppressFinalize(this);
		CopyFrom(connection);
		_connectionString = connection._connectionString;
		if (connection._credential != null)
		{
			SecureString secureString = connection._credential.Password.Copy();
			secureString.MakeReadOnly();
			_credential = new SqlCredential(connection._credential.UserId, secureString);
		}
		_accessToken = connection._accessToken;
		_serverCertificateValidationCallback = connection._serverCertificateValidationCallback;
		_clientCertificateRetrievalCallback = connection._clientCertificateRetrievalCallback;
		_originalNetworkAddressInfo = connection._originalNetworkAddressInfo;
		CacheConnectionStringProperties();
	}

	private void CacheConnectionStringProperties()
	{
		if (ConnectionOptions is SqlConnectionString sqlConnectionString)
		{
			_connectRetryCount = sqlConnectionString.ConnectRetryCount;
			if (_connectRetryCount == 1 && ADP.IsAzureSqlServerEndpoint(sqlConnectionString.DataSource))
			{
				_connectRetryCount = 2;
			}
		}
	}

	private bool UsesContextConnection(SqlConnectionString opt)
	{
		return opt?.ContextConnection ?? false;
	}

	private bool UsesActiveDirectoryIntegrated(SqlConnectionString opt)
	{
		if (opt != null)
		{
			return opt.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated;
		}
		return false;
	}

	private bool UsesActiveDirectoryInteractive(SqlConnectionString opt)
	{
		if (opt != null)
		{
			return opt.Authentication == SqlAuthenticationMethod.ActiveDirectoryInteractive;
		}
		return false;
	}

	private bool UsesActiveDirectoryDeviceCodeFlow(SqlConnectionString opt)
	{
		if (opt != null)
		{
			return opt.Authentication == SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow;
		}
		return false;
	}

	private bool UsesActiveDirectoryManagedIdentity(SqlConnectionString opt)
	{
		if (opt != null)
		{
			return opt.Authentication == SqlAuthenticationMethod.ActiveDirectoryManagedIdentity;
		}
		return false;
	}

	private bool UsesActiveDirectoryMSI(SqlConnectionString opt)
	{
		if (opt != null)
		{
			return opt.Authentication == SqlAuthenticationMethod.ActiveDirectoryMSI;
		}
		return false;
	}

	private bool UsesActiveDirectoryDefault(SqlConnectionString opt)
	{
		if (opt != null)
		{
			return opt.Authentication == SqlAuthenticationMethod.ActiveDirectoryDefault;
		}
		return false;
	}

	private bool UsesAuthentication(SqlConnectionString opt)
	{
		if (opt != null)
		{
			return opt.Authentication != SqlAuthenticationMethod.NotSpecified;
		}
		return false;
	}

	private bool UsesIntegratedSecurity(SqlConnectionString opt)
	{
		return opt?.IntegratedSecurity ?? false;
	}

	private bool UsesClearUserIdOrPassword(SqlConnectionString opt)
	{
		bool result = false;
		if (opt != null)
		{
			result = !ADP.IsEmpty(opt.UserID) || !ADP.IsEmpty(opt.Password);
		}
		return result;
	}

	private bool UsesCertificate(SqlConnectionString opt)
	{
		return opt?.UsesCertificate ?? false;
	}

	private void CheckAndThrowOnInvalidCombinationOfConnectionStringAndSqlCredential(SqlConnectionString connectionOptions)
	{
		if (UsesClearUserIdOrPassword(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfSecureAndClearCredential();
		}
		if (UsesIntegratedSecurity(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfSecureCredentialAndIntegratedSecurity();
		}
		if (UsesContextConnection(connectionOptions))
		{
			throw ADP.InvalidMixedArgumentOfSecureCredentialAndContextConnection();
		}
	}

	private void CheckAndThrowOnInvalidCombinationOfConnectionOptionAndAccessToken(SqlConnectionString connectionOptions)
	{
		if (UsesClearUserIdOrPassword(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfAccessTokenAndUserIDPassword();
		}
		if (UsesIntegratedSecurity(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfAccessTokenAndIntegratedSecurity();
		}
		if (UsesContextConnection(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfAccessTokenAndContextConnection();
		}
		if (UsesAuthentication(connectionOptions))
		{
			throw ADP.InvalidMixedUsageOfAccessTokenAndAuthentication();
		}
		if (_credential != null)
		{
			throw ADP.InvalidMixedUsageOfAccessTokenAndCredential();
		}
	}

	public new SqlTransaction BeginTransaction()
	{
		return BeginTransaction(System.Data.IsolationLevel.Unspecified, null);
	}

	public new SqlTransaction BeginTransaction(System.Data.IsolationLevel iso)
	{
		return BeginTransaction(iso, null);
	}

	public SqlTransaction BeginTransaction(string transactionName)
	{
		return BeginTransaction(System.Data.IsolationLevel.Unspecified, transactionName);
	}

	protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<prov.SqlConnection.BeginDbTransaction|API> {0}, isolationLevel={1}", ObjectID, (int)isolationLevel);
		try
		{
			DbTransaction result = BeginTransaction(isolationLevel);
			GC.KeepAlive(this);
			return result;
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	public SqlTransaction BeginTransaction(System.Data.IsolationLevel iso, string transactionName)
	{
		WaitForPendingReconnection();
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlConnection.BeginTransaction|API> {0}, iso={1}, transactionName='{2}'", ObjectID, (int)iso, transactionName);
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			bool shouldReconnect = true;
			SqlTransaction sqlTransaction;
			do
			{
				sqlTransaction = GetOpenConnection().BeginSqlTransaction(iso, transactionName, shouldReconnect);
				shouldReconnect = false;
			}
			while (sqlTransaction.InternalTransaction.ConnectionHasBeenRestored);
			GC.KeepAlive(this);
			return sqlTransaction;
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
			SqlStatistics.StopTimer(statistics);
		}
	}

	public override void ChangeDatabase(string database)
	{
		SqlStatistics statistics = null;
		RepairInnerConnection();
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlConnection.ChangeDatabase|API|Correlation> ObjectID{0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(this);
			statistics = SqlStatistics.StartTimer(Statistics);
			InnerConnection.ChangeDatabase(database);
		}
		catch (OutOfMemoryException e)
		{
			Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public static void ClearAllPools()
	{
		((CodeAccessPermission)new SqlClientPermission(PermissionState.Unrestricted)).Demand();
		SqlConnectionFactory.SingletonInstance.ClearAllPools();
	}

	public static void ClearPool(SqlConnection connection)
	{
		ADP.CheckArgumentNull(connection, "connection");
		DbConnectionOptions userConnectionOptions = connection.UserConnectionOptions;
		if (userConnectionOptions != null)
		{
			userConnectionOptions.DemandPermission();
			if (connection.IsContextConnection)
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			SqlConnectionFactory.SingletonInstance.ClearPool(connection);
		}
	}

	object ICloneable.Clone()
	{
		return new SqlConnection(this);
	}

	private void CloseInnerConnection()
	{
		_originalConnectionId = ClientConnectionId;
		InnerConnection.CloseConnection(this, ConnectionFactory);
	}

	public override void Close()
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlConnection.Close|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlConnection.Close|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		try
		{
			SqlStatistics statistics = null;
			TdsParser target = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				target = SqlInternalConnection.GetBestEffortCleanupTarget(this);
				statistics = SqlStatistics.StartTimer(Statistics);
				Task currentReconnectionTask = _currentReconnectionTask;
				if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
				{
					_reconnectionCancellationSource?.Cancel();
					AsyncHelper.WaitForCompletion(currentReconnectionTask, 0, null, rethrowExceptions: false);
					if (State != ConnectionState.Open)
					{
						OnStateChange(DbConnectionInternal.StateChangeClosed);
					}
				}
				CancelOpenAndWait();
				CloseInnerConnection();
				GC.SuppressFinalize(this);
				if (Statistics != null)
				{
					_statistics._closeTimestamp = ADP.TimerCurrent();
				}
			}
			catch (OutOfMemoryException e)
			{
				Abort(e);
				throw;
			}
			catch (StackOverflowException e2)
			{
				Abort(e2);
				throw;
			}
			catch (ThreadAbortException e3)
			{
				Abort(e3);
				SqlInternalConnection.BestEffortCleanup(target);
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(statistics);
				if (_lastIdentity != null)
				{
					_lastIdentity.Dispose();
				}
			}
		}
		finally
		{
			SqlDebugContext sdc = _sdc;
			_sdc = null;
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
			sdc?.Dispose();
		}
	}

	public new SqlCommand CreateCommand()
	{
		return new SqlCommand(null, this);
	}

	private void DisposeMe(bool disposing)
	{
		_credential = null;
		_accessToken = null;
		if (!disposing && InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds && !sqlInternalConnectionTds.ConnectionOptions.Pooling)
		{
			TdsParser parser = sqlInternalConnectionTds.Parser;
			if (parser != null && parser._physicalStateObj != null)
			{
				parser._physicalStateObj.DecrementPendingCallbacks(release: false);
			}
		}
	}

	public void EnlistDistributedTransaction(ITransaction transaction)
	{
		if (IsContextConnection)
		{
			throw SQL.NotAvailableOnContextConnection();
		}
		EnlistDistributedTransactionHelper(transaction);
	}

	public override void Open()
	{
		Open(SqlConnectionOverrides.None);
	}

	private bool TryOpenWithRetry(TaskCompletionSource<DbConnectionInternal> retry, SqlConnectionOverrides overrides)
	{
		return RetryLogicProvider.Execute(this, () => TryOpen(retry, overrides));
	}

	public void Open(SqlConnectionOverrides overrides)
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlConnection.Open|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlConnection.Open|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		try
		{
			if (StatisticsEnabled)
			{
				if (_statistics == null)
				{
					_statistics = new SqlStatistics();
				}
				else
				{
					_statistics.ContinueOnNewConnection();
				}
			}
			SqlStatistics statistics = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				statistics = SqlStatistics.StartTimer(Statistics);
				if (!(IsRetryEnabled ? TryOpenWithRetry(null, overrides) : TryOpen(null, overrides)))
				{
					throw ADP.InternalError(ADP.InternalErrorCode.SynchronousConnectReturnedPending);
				}
			}
			finally
			{
				SqlStatistics.StopTimer(statistics);
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	internal void RegisterWaitingForReconnect(Task waitingTask)
	{
		if (!((SqlConnectionString)ConnectionOptions).MARS)
		{
			Interlocked.CompareExchange(ref _asyncWaitingForReconnection, waitingTask, null);
			if (_asyncWaitingForReconnection != waitingTask)
			{
				throw SQL.MARSUnsupportedOnConnection();
			}
		}
	}

	private async Task ReconnectAsync(int timeout)
	{
		_ = 1;
		try
		{
			long commandTimeoutExpiration = 0L;
			if (timeout > 0)
			{
				commandTimeoutExpiration = ADP.TimerCurrent() + ADP.TimerFromSeconds(timeout);
			}
			CancellationToken ctoken = (_reconnectionCancellationSource = new CancellationTokenSource()).Token;
			int retryCount = _connectRetryCount;
			for (int attempt = 0; attempt < retryCount; attempt++)
			{
				if (ctoken.IsCancellationRequested)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.ReconnectAsync|INFO> Original ClientConnectionID: {0} - reconnection cancelled.", _originalConnectionId);
					break;
				}
				try
				{
					_impersonateIdentity = _lastIdentity;
					try
					{
						ForceNewConnection = true;
						await OpenAsync(ctoken).ConfigureAwait(continueOnCapturedContext: false);
						_reconnectCount++;
					}
					finally
					{
						_impersonateIdentity = null;
						ForceNewConnection = false;
					}
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.ReconnectIfNeeded|INFO> Reconnection succeeded.  ClientConnectionID {0} -> {1}", _originalConnectionId, ClientConnectionId);
					break;
				}
				catch (SqlException ex)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.ReconnectAsyncINFO> Original ClientConnectionID {0} - reconnection attempt failed error {1}", _originalConnectionId, ex.Message);
					if (attempt == retryCount - 1)
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.ReconnectAsync|INFO> Original ClientConnectionID {0} - give up reconnection", _originalConnectionId);
						throw SQL.CR_AllAttemptsFailed(ex, _originalConnectionId);
					}
					if (timeout > 0 && ADP.TimerRemaining(commandTimeoutExpiration) < ADP.TimerFromSeconds(ConnectRetryInterval))
					{
						throw SQL.CR_NextAttemptWillExceedQueryTimeout(ex, _originalConnectionId);
					}
				}
				await Task.Delay(1000 * ConnectRetryInterval, ctoken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		finally
		{
			_recoverySessionData = null;
			_suppressStateChangeForReconnection = false;
		}
	}

	internal Task ValidateAndReconnect(Action beforeDisconnect, int timeout)
	{
		Task task = _currentReconnectionTask;
		while (task != null && task.IsCompleted)
		{
			Interlocked.CompareExchange(ref _currentReconnectionTask, null, task);
			task = _currentReconnectionTask;
		}
		if (task == null)
		{
			if (_connectRetryCount > 0)
			{
				SqlInternalConnectionTds openTdsConnection = GetOpenTdsConnection();
				if (openTdsConnection._sessionRecoveryAcknowledged)
				{
					TdsParserStateObject physicalStateObj = openTdsConnection.Parser._physicalStateObj;
					if (!physicalStateObj.ValidateSNIConnection())
					{
						if (openTdsConnection.Parser._sessionPool != null && openTdsConnection.Parser._sessionPool.ActiveSessionsCount > 0)
						{
							beforeDisconnect?.Invoke();
							OnError(SQL.CR_UnrecoverableClient(ClientConnectionId), breakConnection: true, null);
						}
						SessionData currentSessionData = openTdsConnection.CurrentSessionData;
						if (currentSessionData._unrecoverableStatesCount == 0)
						{
							bool flag = false;
							lock (_reconnectLock)
							{
								openTdsConnection.CheckEnlistedTransactionBinding();
								task = _currentReconnectionTask;
								if (task == null)
								{
									if (currentSessionData._unrecoverableStatesCount == 0)
									{
										_originalConnectionId = ClientConnectionId;
										SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.ReconnectIfNeeded|INFO> Connection ClientConnectionID {0} is invalid, reconnecting", _originalConnectionId);
										_recoverySessionData = currentSessionData;
										beforeDisconnect?.Invoke();
										try
										{
											_suppressStateChangeForReconnection = true;
											openTdsConnection.DoomThisConnection();
										}
										catch (SqlException)
										{
										}
										task = (_currentReconnectionTask = Task.Run(() => ReconnectAsync(timeout)));
									}
								}
								else
								{
									flag = true;
								}
							}
							if (flag)
							{
								beforeDisconnect?.Invoke();
							}
						}
						else
						{
							beforeDisconnect?.Invoke();
							OnError(SQL.CR_UnrecoverableServer(ClientConnectionId), breakConnection: true, null);
						}
					}
				}
			}
		}
		else
		{
			beforeDisconnect?.Invoke();
		}
		return task;
	}

	private void WaitForPendingReconnection()
	{
		Task currentReconnectionTask = _currentReconnectionTask;
		if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
		{
			AsyncHelper.WaitForCompletion(currentReconnectionTask, 0, null, rethrowExceptions: false);
		}
	}

	private void CancelOpenAndWait()
	{
		Tuple<TaskCompletionSource<DbConnectionInternal>, Task> currentCompletion = _currentCompletion;
		if (currentCompletion != null)
		{
			currentCompletion.Item1.TrySetCanceled();
			((IAsyncResult)currentCompletion.Item2).AsyncWaitHandle.WaitOne();
		}
	}

	private Task InternalOpenWithRetryAsync(CancellationToken cancellationToken)
	{
		return RetryLogicProvider.ExecuteAsync(this, () => InternalOpenAsync(cancellationToken), cancellationToken);
	}

	public override Task OpenAsync(CancellationToken cancellationToken)
	{
		if (!IsRetryEnabled)
		{
			return InternalOpenAsync(cancellationToken);
		}
		return InternalOpenWithRetryAsync(cancellationToken);
	}

	private Task InternalOpenAsync(CancellationToken cancellationToken)
	{
		long scopeId = SqlClientEventSource.Log.TryPoolerScopeEnterEvent("<sc.SqlConnection.OpenAsync|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlConnection.OpenAsync|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		try
		{
			if (StatisticsEnabled)
			{
				if (_statistics == null)
				{
					_statistics = new SqlStatistics();
				}
				else
				{
					_statistics.ContinueOnNewConnection();
				}
			}
			SqlStatistics statistics = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				statistics = SqlStatistics.StartTimer(Statistics);
				Transaction currentTransaction = ADP.GetCurrentTransaction();
				TaskCompletionSource<DbConnectionInternal> completion = new TaskCompletionSource<DbConnectionInternal>(currentTransaction);
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				if (cancellationToken.IsCancellationRequested)
				{
					taskCompletionSource.SetCanceled();
					return taskCompletionSource.Task;
				}
				if (IsContextConnection)
				{
					taskCompletionSource.SetException(ADP.ExceptionWithStackTrace(SQL.NotAvailableOnContextConnection()));
					return taskCompletionSource.Task;
				}
				bool flag;
				try
				{
					flag = TryOpen(completion);
				}
				catch (Exception exception)
				{
					taskCompletionSource.SetException(exception);
					return taskCompletionSource.Task;
				}
				if (flag)
				{
					taskCompletionSource.SetResult(null);
					return taskCompletionSource.Task;
				}
				CancellationTokenRegistration registration = default(CancellationTokenRegistration);
				if (cancellationToken.CanBeCanceled)
				{
					registration = cancellationToken.Register(delegate
					{
						completion.TrySetCanceled();
					});
				}
				OpenAsyncRetry @object = new OpenAsyncRetry(this, completion, taskCompletionSource, registration);
				_currentCompletion = new Tuple<TaskCompletionSource<DbConnectionInternal>, Task>(completion, taskCompletionSource.Task);
				completion.Task.ContinueWith(@object.Retry, TaskScheduler.Default);
				return taskCompletionSource.Task;
			}
			finally
			{
				SqlStatistics.StopTimer(statistics);
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryPoolerScopeLeaveEvent(scopeId);
		}
	}

	private bool TryOpen(TaskCompletionSource<DbConnectionInternal> retry, SqlConnectionOverrides overrides = SqlConnectionOverrides.None)
	{
		SqlConnectionString sqlConnectionString = (SqlConnectionString)ConnectionOptions;
		bool result = false;
		_applyTransientFaultHandling = !overrides.HasFlag(SqlConnectionOverrides.OpenWithoutRetry) && retry == null && sqlConnectionString != null && sqlConnectionString.ConnectRetryCount > 0;
		if (sqlConnectionString != null && (sqlConnectionString.Authentication == SqlAuthenticationMethod.SqlPassword || sqlConnectionString.Authentication == SqlAuthenticationMethod.ActiveDirectoryPassword || sqlConnectionString.Authentication == SqlAuthenticationMethod.ActiveDirectoryServicePrincipal) && (!sqlConnectionString.HasUserIdKeyword || !sqlConnectionString.HasPasswordKeyword) && _credential == null)
		{
			throw SQL.CredentialsNotProvided(sqlConnectionString.Authentication);
		}
		if (_impersonateIdentity != null)
		{
			WindowsIdentity currentWindowsIdentity = DbConnectionPoolIdentity.GetCurrentWindowsIdentity();
			try
			{
				if (_impersonateIdentity.User == currentWindowsIdentity.User)
				{
					result = TryOpenInner(retry);
				}
				else
				{
					WindowsImpersonationContext val = _impersonateIdentity.Impersonate();
					try
					{
						result = TryOpenInner(retry);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)currentWindowsIdentity)?.Dispose();
			}
		}
		else
		{
			if (UsesIntegratedSecurity(sqlConnectionString) || UsesCertificate(sqlConnectionString) || UsesActiveDirectoryIntegrated(sqlConnectionString))
			{
				_lastIdentity = DbConnectionPoolIdentity.GetCurrentWindowsIdentity();
			}
			else
			{
				_lastIdentity = null;
			}
			result = TryOpenInner(retry);
		}
		_applyTransientFaultHandling = retry == null && sqlConnectionString != null && sqlConnectionString.ConnectRetryCount > 0;
		return result;
	}

	private bool TryOpenInner(TaskCompletionSource<DbConnectionInternal> retry)
	{
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (ForceNewConnection)
			{
				if (!InnerConnection.TryReplaceConnection(this, ConnectionFactory, retry, UserConnectionOptions))
				{
					return false;
				}
			}
			else if (!InnerConnection.TryOpenConnection(this, ConnectionFactory, retry, UserConnectionOptions))
			{
				return false;
			}
			target = SqlInternalConnection.GetBestEffortCleanupTarget(this);
			if (!(InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds))
			{
				SqlInternalConnectionSmi sqlInternalConnectionSmi = InnerConnection as SqlInternalConnectionSmi;
				sqlInternalConnectionSmi.AutomaticEnlistment();
			}
			else
			{
				if (!sqlInternalConnectionTds.ConnectionOptions.Pooling)
				{
					GC.ReRegisterForFinalize(this);
				}
				if (StatisticsEnabled)
				{
					_statistics._openTimestamp = ADP.TimerCurrent();
					sqlInternalConnectionTds.Parser.Statistics = _statistics;
				}
				else
				{
					sqlInternalConnectionTds.Parser.Statistics = null;
					_statistics = null;
				}
				CompleteOpen();
			}
		}
		catch (OutOfMemoryException e)
		{
			Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		return true;
	}

	internal void ValidateConnectionForExecute(string method, SqlCommand command)
	{
		Task asyncWaitingForReconnection = _asyncWaitingForReconnection;
		if (asyncWaitingForReconnection != null)
		{
			if (!asyncWaitingForReconnection.IsCompleted)
			{
				throw SQL.MARSUnsupportedOnConnection();
			}
			Interlocked.CompareExchange(ref _asyncWaitingForReconnection, null, asyncWaitingForReconnection);
		}
		if (_currentReconnectionTask != null)
		{
			Task currentReconnectionTask = _currentReconnectionTask;
			if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
			{
				return;
			}
		}
		SqlInternalConnection openConnection = GetOpenConnection(method);
		openConnection.ValidateConnectionForExecute(command);
	}

	internal static string FixupDatabaseTransactionName(string name)
	{
		if (!ADP.IsEmpty(name))
		{
			return SqlServerEscapeHelper.EscapeIdentifier(name);
		}
		return name;
	}

	internal void OnError(SqlException exception, bool breakConnection, Action<Action> wrapCloseInAction)
	{
		if (breakConnection && ConnectionState.Open == State)
		{
			if (wrapCloseInAction != null)
			{
				int capturedCloseCount = _closeCount;
				Action obj = delegate
				{
					if (capturedCloseCount == _closeCount)
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.OnError|INFO> {0}, Connection broken.", ObjectID);
						Close();
					}
				};
				wrapCloseInAction(obj);
			}
			else
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.OnError|INFO> {0}, Connection broken.", ObjectID);
				Close();
			}
		}
		if (exception.Class >= 11)
		{
			throw exception;
		}
		OnInfoMessage(new SqlInfoMessageEventArgs(exception));
	}

	private void CompleteOpen()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (!GetOpenConnection().IsYukonOrNewer && Debugger.IsAttached)
		{
			bool flag = false;
			try
			{
				((CodeAccessPermission)new SecurityPermission(SecurityPermissionFlag.UnmanagedCode)).Demand();
				flag = true;
			}
			catch (SecurityException e)
			{
				ADP.TraceExceptionWithoutRethrow(e);
			}
			if (flag)
			{
				CheckSQLDebugOnConnect();
			}
		}
	}

	internal SqlInternalConnection GetOpenConnection()
	{
		if (!(InnerConnection is SqlInternalConnection result))
		{
			throw ADP.ClosedConnectionError();
		}
		return result;
	}

	internal SqlInternalConnection GetOpenConnection(string method)
	{
		DbConnectionInternal innerConnection = InnerConnection;
		if (!(innerConnection is SqlInternalConnection result))
		{
			throw ADP.OpenConnectionRequired(method, innerConnection.State);
		}
		return result;
	}

	internal SqlInternalConnectionTds GetOpenTdsConnection()
	{
		if (!(InnerConnection is SqlInternalConnectionTds result))
		{
			throw ADP.ClosedConnectionError();
		}
		return result;
	}

	internal SqlInternalConnectionTds GetOpenTdsConnection(string method)
	{
		if (!(InnerConnection is SqlInternalConnectionTds result))
		{
			throw ADP.OpenConnectionRequired(method, InnerConnection.State);
		}
		return result;
	}

	internal void OnInfoMessage(SqlInfoMessageEventArgs imevent)
	{
		OnInfoMessage(imevent, out var _);
	}

	internal void OnInfoMessage(SqlInfoMessageEventArgs imevent, out bool notified)
	{
		string args = ((imevent != null) ? imevent.Message : "");
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.OnInfoMessage|API|INFO> {0}, Message='{1}'", ObjectID, args);
		SqlInfoMessageEventHandler sqlInfoMessageEventHandler = (SqlInfoMessageEventHandler)base.Events[EventInfoMessage];
		if (sqlInfoMessageEventHandler != null)
		{
			notified = true;
			try
			{
				sqlInfoMessageEventHandler(this, imevent);
				return;
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(e))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(e);
				return;
			}
		}
		notified = false;
	}

	private void CheckSQLDebugOnConnect()
	{
		uint currentProcessId = (uint)Microsoft.Data.Common.SafeNativeMethods.GetCurrentProcessId();
		string text = ((!ADP.IsPlatformNT5) ? "SqlClientSSDebug" : "Global\\SqlClientSSDebug");
		text += currentProcessId.ToString(CultureInfo.InvariantCulture);
		IntPtr intPtr = NativeMethods.OpenFileMappingA(4, bInheritHandle: false, text);
		if (ADP.PtrZero != intPtr)
		{
			IntPtr intPtr2 = NativeMethods.MapViewOfFile(intPtr, 4, 0, 0, IntPtr.Zero);
			if (ADP.PtrZero != intPtr2)
			{
				SqlDebugContext sqlDebugContext = new SqlDebugContext();
				sqlDebugContext.hMemMap = intPtr;
				sqlDebugContext.pMemMap = intPtr2;
				sqlDebugContext.pid = currentProcessId;
				CheckSQLDebug(sqlDebugContext);
				_sdc = sqlDebugContext;
			}
		}
	}

	internal void CheckSQLDebug()
	{
		if (_sdc != null)
		{
			CheckSQLDebug(_sdc);
		}
	}

	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	private void CheckSQLDebug(SqlDebugContext sdc)
	{
		uint currentThreadId = (uint)AppDomain.GetCurrentThreadId();
		RefreshMemoryMappedData(sdc);
		if (!sdc.active && sdc.fOption)
		{
			sdc.active = true;
			sdc.tid = currentThreadId;
			try
			{
				IssueSQLDebug(1u, sdc.machineName, sdc.pid, sdc.dbgpid, sdc.sdiDllName, sdc.data);
				sdc.tid = 0u;
			}
			catch
			{
				sdc.active = false;
				throw;
			}
		}
		if (!sdc.active)
		{
			return;
		}
		if (!sdc.fOption)
		{
			sdc.Dispose();
			IssueSQLDebug(0u, null, 0u, 0u, null, null);
		}
		else if (sdc.tid != currentThreadId)
		{
			sdc.tid = currentThreadId;
			try
			{
				IssueSQLDebug(2u, null, sdc.pid, sdc.tid, null, null);
			}
			catch
			{
				sdc.tid = 0u;
				throw;
			}
		}
	}

	private void IssueSQLDebug(uint option, string machineName, uint pid, uint id, string sdiDllName, byte[] data)
	{
		if (!GetOpenConnection().IsYukonOrNewer)
		{
			SqlCommand sqlCommand = new SqlCommand("sp_sdidebug", this);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.VarChar, TdsEnums.SQLDEBUG_MODE_NAMES[option].Length);
			sqlParameter.Value = TdsEnums.SQLDEBUG_MODE_NAMES[option];
			sqlCommand.Parameters.Add(sqlParameter);
			if (option == 1)
			{
				sqlParameter = new SqlParameter(null, SqlDbType.VarChar, sdiDllName.Length);
				sqlParameter.Value = sdiDllName;
				sqlCommand.Parameters.Add(sqlParameter);
				sqlParameter = new SqlParameter(null, SqlDbType.VarChar, machineName.Length);
				sqlParameter.Value = machineName;
				sqlCommand.Parameters.Add(sqlParameter);
			}
			if (option != 0)
			{
				sqlParameter = new SqlParameter(null, SqlDbType.Int);
				sqlParameter.Value = pid;
				sqlCommand.Parameters.Add(sqlParameter);
				sqlParameter = new SqlParameter(null, SqlDbType.Int);
				sqlParameter.Value = id;
				sqlCommand.Parameters.Add(sqlParameter);
			}
			if (option == 1)
			{
				sqlParameter = new SqlParameter(null, SqlDbType.VarBinary, (data != null) ? data.Length : 0);
				sqlParameter.Value = data;
				sqlCommand.Parameters.Add(sqlParameter);
			}
			sqlCommand.ExecuteNonQuery();
		}
	}

	public static void ChangePassword(string connectionString, string newPassword)
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlConnection.ChangePassword|API>", "ChangePassword");
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlConnection.ChangePassword|API|Correlation> ActivityID {0}", ActivityCorrelator.Current);
		try
		{
			if (ADP.IsEmpty(connectionString))
			{
				throw SQL.ChangePasswordArgumentMissing("connectionString");
			}
			if (ADP.IsEmpty(newPassword))
			{
				throw SQL.ChangePasswordArgumentMissing("newPassword");
			}
			if (128 < newPassword.Length)
			{
				throw ADP.InvalidArgumentLength("newPassword", 128);
			}
			SqlConnectionPoolKey key = new SqlConnectionPoolKey(connectionString, null, null, null, null, null);
			SqlConnectionString sqlConnectionString = SqlConnectionFactory.FindSqlConnectionOptions(key);
			if (sqlConnectionString.IntegratedSecurity || sqlConnectionString.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated)
			{
				throw SQL.ChangePasswordConflictsWithSSPI();
			}
			if (!ADP.IsEmpty(sqlConnectionString.AttachDBFilename))
			{
				throw SQL.ChangePasswordUseOfUnallowedKey("attachdbfilename");
			}
			if (sqlConnectionString.ContextConnection)
			{
				throw SQL.ChangePasswordUseOfUnallowedKey("context connection");
			}
			PermissionSet permissionSet = sqlConnectionString.CreatePermissionSet();
			permissionSet.Demand();
			ChangePassword(connectionString, sqlConnectionString, null, newPassword, null);
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	public static void ChangePassword(string connectionString, SqlCredential credential, SecureString newSecurePassword)
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlConnection.ChangePassword|API>", "ChangePassword");
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlConnection.ChangePassword|API|Correlation> ActivityID {0}", ActivityCorrelator.Current);
		try
		{
			if (ADP.IsEmpty(connectionString))
			{
				throw SQL.ChangePasswordArgumentMissing("connectionString");
			}
			if (credential == null)
			{
				throw SQL.ChangePasswordArgumentMissing("credential");
			}
			if (newSecurePassword == null || newSecurePassword.Length == 0)
			{
				throw SQL.ChangePasswordArgumentMissing("newSecurePassword");
			}
			if (!newSecurePassword.IsReadOnly())
			{
				throw ADP.MustBeReadOnly("newSecurePassword");
			}
			if (128 < newSecurePassword.Length)
			{
				throw ADP.InvalidArgumentLength("newSecurePassword", 128);
			}
			SqlConnectionPoolKey key = new SqlConnectionPoolKey(connectionString, credential, null, null, null, null);
			SqlConnectionString sqlConnectionString = SqlConnectionFactory.FindSqlConnectionOptions(key);
			if (!ADP.IsEmpty(sqlConnectionString.UserID) || !ADP.IsEmpty(sqlConnectionString.Password))
			{
				throw ADP.InvalidMixedArgumentOfSecureAndClearCredential();
			}
			if (sqlConnectionString.IntegratedSecurity || sqlConnectionString.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated)
			{
				throw SQL.ChangePasswordConflictsWithSSPI();
			}
			if (!ADP.IsEmpty(sqlConnectionString.AttachDBFilename))
			{
				throw SQL.ChangePasswordUseOfUnallowedKey("attachdbfilename");
			}
			if (sqlConnectionString.ContextConnection)
			{
				throw SQL.ChangePasswordUseOfUnallowedKey("context connection");
			}
			PermissionSet permissionSet = sqlConnectionString.CreatePermissionSet();
			permissionSet.Demand();
			ChangePassword(connectionString, sqlConnectionString, credential, null, newSecurePassword);
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	private static void ChangePassword(string connectionString, SqlConnectionString connectionOptions, SqlCredential credential, string newPassword, SecureString newSecurePassword)
	{
		using (SqlInternalConnectionTds sqlInternalConnectionTds = new SqlInternalConnectionTds(null, connectionOptions, credential, null, newPassword, newSecurePassword, redirectedUserInstance: false))
		{
			if (!sqlInternalConnectionTds.IsYukonOrNewer)
			{
				throw SQL.ChangePasswordRequiresYukon();
			}
		}
		SqlConnectionPoolKey key = new SqlConnectionPoolKey(connectionString, credential, null, null, null, null);
		SqlConnectionFactory.SingletonInstance.ClearPool(key);
	}

	internal void RegisterForConnectionCloseNotification<T>(ref Task<T> outerTask, object value, int tag)
	{
		outerTask = outerTask.ContinueWith(delegate(Task<T> task)
		{
			RemoveWeakReference(value);
			return task;
		}, TaskScheduler.Default).Unwrap();
	}

	private static void RefreshMemoryMappedData(SqlDebugContext sdc)
	{
		MEMMAP mEMMAP = (MEMMAP)Marshal.PtrToStructure(sdc.pMemMap, typeof(MEMMAP));
		sdc.dbgpid = mEMMAP.dbgpid;
		sdc.fOption = mEMMAP.fOption == 1;
		Encoding encoding = Encoding.GetEncoding(1252);
		sdc.machineName = encoding.GetString(mEMMAP.rgbMachineName, 0, mEMMAP.rgbMachineName.Length);
		sdc.sdiDllName = encoding.GetString(mEMMAP.rgbDllName, 0, mEMMAP.rgbDllName.Length);
		sdc.data = mEMMAP.rgbData;
	}

	public void ResetStatistics()
	{
		if (IsContextConnection)
		{
			throw SQL.NotAvailableOnContextConnection();
		}
		if (Statistics != null)
		{
			Statistics.Reset();
			if (ConnectionState.Open == State)
			{
				_statistics._openTimestamp = ADP.TimerCurrent();
			}
		}
	}

	public IDictionary RetrieveStatistics()
	{
		if (IsContextConnection)
		{
			throw SQL.NotAvailableOnContextConnection();
		}
		if (Statistics != null)
		{
			UpdateStatistics();
			return Statistics.GetDictionary();
		}
		return new SqlStatistics().GetDictionary();
	}

	private void UpdateStatistics()
	{
		if (ConnectionState.Open == State)
		{
			_statistics._closeTimestamp = ADP.TimerCurrent();
		}
		Statistics.UpdateStatistics();
	}

	public IDictionary<string, object> RetrieveInternalInfo()
	{
		IDictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("SQLDNSCachingSupportedState", SQLDNSCachingSupportedState);
		dictionary.Add("SQLDNSCachingSupportedStateBeforeRedirect", SQLDNSCachingSupportedStateBeforeRedirect);
		return dictionary;
	}

	private Assembly ResolveTypeAssembly(AssemblyName asmRef, bool throwOnError)
	{
		if (string.Compare(asmRef.Name, "Microsoft.SqlServer.Types", StringComparison.OrdinalIgnoreCase) == 0)
		{
			if (asmRef.Version != TypeSystemAssemblyVersion && SqlClientEventSource.Log.IsTraceEnabled())
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnection.ResolveTypeAssembly> SQL CLR type version change: Server sent {0}, client will instantiate {1}", asmRef.Version, TypeSystemAssemblyVersion);
			}
			asmRef.Version = TypeSystemAssemblyVersion;
		}
		try
		{
			return Assembly.Load(asmRef);
		}
		catch (Exception e)
		{
			if (throwOnError || !ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			return null;
		}
	}

	internal void CheckGetExtendedUDTInfo(SqlMetaDataPriv metaData, bool fThrow)
	{
		if (metaData.udt?.Type == null)
		{
			metaData.udt.Type = Type.GetType(metaData.udt.AssemblyQualifiedName, (AssemblyName asmRef) => ResolveTypeAssembly(asmRef, fThrow), null, fThrow);
			if (fThrow && metaData.udt.Type == null)
			{
				throw SQL.UDTUnexpectedResult(metaData.udt.AssemblyQualifiedName);
			}
		}
	}

	internal object GetUdtValue(object value, SqlMetaDataPriv metaData, bool returnDBNull)
	{
		if (returnDBNull && ADP.IsNull(value))
		{
			return DBNull.Value;
		}
		object obj = null;
		if (ADP.IsNull(value))
		{
			Type type = metaData.udt?.Type;
			return type.InvokeMember("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, new object[0], CultureInfo.InvariantCulture);
		}
		MemoryStream s = new MemoryStream((byte[])value);
		return SerializationHelperSql9.Deserialize(s, metaData.udt?.Type);
	}

	internal byte[] GetBytes(object o)
	{
		Format format = Format.Native;
		int maxSize = 0;
		return GetBytes(o, out format, out maxSize);
	}

	internal byte[] GetBytes(object o, out Format format, out int maxSize)
	{
		SqlUdtInfo infoFromType = AssemblyCache.GetInfoFromType(o.GetType());
		maxSize = infoFromType.MaxByteSize;
		format = infoFromType.SerializationFormat;
		if (maxSize < -1 || maxSize >= 65535)
		{
			throw new InvalidOperationException(o.GetType()?.ToString() + ": invalid Size");
		}
		using MemoryStream memoryStream = new MemoryStream((maxSize >= 0) ? maxSize : 0);
		SerializationHelperSql9.Serialize(memoryStream, o);
		return memoryStream.ToArray();
	}

	public SqlConnection()
	{
		GC.SuppressFinalize(this);
		_innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
	}

	private void CopyFrom(SqlConnection connection)
	{
		ADP.CheckArgumentNull(connection, "connection");
		_userConnectionOptions = connection.UserConnectionOptions;
		_poolGroup = connection.PoolGroup;
		if (DbConnectionClosedNeverOpened.SingletonInstance == connection._innerConnection)
		{
			_innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
		}
		else
		{
			_innerConnection = DbConnectionClosedPreviouslyOpened.SingletonInstance;
		}
	}

	private string ConnectionString_Get()
	{
		SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionHelper.ConnectionString_Get|API> {0}", ObjectID);
		bool shouldHidePassword = InnerConnection.ShouldHidePassword;
		DbConnectionOptions userConnectionOptions = UserConnectionOptions;
		if (userConnectionOptions == null)
		{
			return "";
		}
		return userConnectionOptions.UsersConnectionString(shouldHidePassword);
	}

	private void ConnectionString_Set(string value)
	{
		DbConnectionPoolKey key = new DbConnectionPoolKey(value);
		ConnectionString_Set(key);
	}

	private void ConnectionString_Set(DbConnectionPoolKey key)
	{
		DbConnectionOptions userConnectionOptions = null;
		DbConnectionPoolGroup connectionPoolGroup = ConnectionFactory.GetConnectionPoolGroup(key, null, ref userConnectionOptions);
		DbConnectionInternal innerConnection = InnerConnection;
		bool flag = innerConnection.AllowSetConnectionString;
		if (flag)
		{
			flag = SetInnerConnectionFrom(DbConnectionClosedBusy.SingletonInstance, innerConnection);
			if (flag)
			{
				_userConnectionOptions = userConnectionOptions;
				_poolGroup = connectionPoolGroup;
				_innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
			}
		}
		if (!flag)
		{
			throw ADP.OpenConnectionPropertySet("ConnectionString", innerConnection.State);
		}
		if (SqlClientEventSource.Log.IsTraceEnabled())
		{
			SqlClientEventSource.Log.TraceEvent("<prov.DbConnectionHelper.ConnectionString_Set|API> {0}, '{1}'", ObjectID, userConnectionOptions.UsersConnectionStringForTrace(), "ConnectionString_Set");
		}
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal void Abort(Exception e)
	{
		DbConnectionInternal innerConnection = _innerConnection;
		if (ConnectionState.Open == innerConnection.State)
		{
			Interlocked.CompareExchange(ref _innerConnection, DbConnectionClosedPreviouslyOpened.SingletonInstance, innerConnection);
			innerConnection.DoomThisConnection();
		}
		if (e is OutOfMemoryException)
		{
			SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionHelper.Abort|RES|INFO|CPOOL> {0}, Aborting operation due to asynchronous exception: {'OutOfMemory'}", ObjectID);
		}
		else
		{
			SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionHelper.Abort|RES|INFO|CPOOL> {0}, Aborting operation due to asynchronous exception: {1}", ObjectID, e);
		}
	}

	internal void AddWeakReference(object value, int tag)
	{
		InnerConnection.AddWeakReference(value, tag);
	}

	protected override DbCommand CreateDbCommand()
	{
		DbCommand dbCommand = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<prov.DbConnectionHelper.CreateDbCommand|API> {0}", ObjectID);
		try
		{
			DbProviderFactory providerFactory = ConnectionFactory.ProviderFactory;
			dbCommand = providerFactory.CreateCommand();
			dbCommand.Connection = this;
			return dbCommand;
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	private static CodeAccessPermission CreateExecutePermission()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		DBDataPermission val = (DBDataPermission)SqlConnectionFactory.SingletonInstance.ProviderFactory.CreatePermission(PermissionState.None);
		val.Add(string.Empty, string.Empty, KeyRestrictionBehavior.AllowOnly);
		return (CodeAccessPermission)(object)val;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_userConnectionOptions = null;
			_poolGroup = null;
			Close();
		}
		DisposeMe(disposing);
		base.Dispose(disposing);
	}

	private void RepairInnerConnection()
	{
		WaitForPendingReconnection();
		if (_connectRetryCount != 0 && InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds)
		{
			sqlInternalConnectionTds.ValidateConnectionForExecute(null);
			sqlInternalConnectionTds.GetSessionAndReconnectIfNeeded(this);
		}
	}

	private void EnlistDistributedTransactionHelper(ITransaction transaction)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		PermissionSet permissionSet = new PermissionSet(PermissionState.None);
		permissionSet.AddPermission((IPermission?)ExecutePermission);
		permissionSet.AddPermission((IPermission?)new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
		permissionSet.Demand();
		SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionHelper.EnlistDistributedTransactionHelper|RES|TRAN> {0}, Connection enlisting in a transaction.", ObjectID);
		Transaction transaction2 = null;
		if (transaction != null)
		{
			transaction2 = TransactionInterop.GetTransactionFromDtcTransaction((IDtcTransaction)transaction);
		}
		RepairInnerConnection();
		InnerConnection.EnlistTransaction(transaction2);
		GC.KeepAlive(this);
	}

	public override void EnlistTransaction(Transaction transaction)
	{
		ExecutePermission.Demand();
		SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionHelper.EnlistTransaction|RES|TRAN> {0}, Connection enlisting in a transaction.", ObjectID);
		DbConnectionInternal innerConnection = InnerConnection;
		Transaction enlistedTransaction = innerConnection.EnlistedTransaction;
		if (enlistedTransaction != null)
		{
			if (enlistedTransaction.Equals(transaction))
			{
				return;
			}
			if (enlistedTransaction.TransactionInformation.Status == TransactionStatus.Active)
			{
				throw ADP.TransactionPresent();
			}
		}
		RepairInnerConnection();
		InnerConnection.EnlistTransaction(transaction);
		GC.KeepAlive(this);
	}

	private DbMetaDataFactory GetMetaDataFactory(DbConnectionInternal internalConnection)
	{
		return ConnectionFactory.GetMetaDataFactory(_poolGroup, internalConnection);
	}

	internal DbMetaDataFactory GetMetaDataFactoryInternal(DbConnectionInternal internalConnection)
	{
		return GetMetaDataFactory(internalConnection);
	}

	public override DataTable GetSchema()
	{
		return GetSchema(DbMetaDataCollectionNames.MetaDataCollections, null);
	}

	public override DataTable GetSchema(string collectionName)
	{
		return GetSchema(collectionName, null);
	}

	public override DataTable GetSchema(string collectionName, string[] restrictionValues)
	{
		ExecutePermission.Demand();
		return InnerConnection.GetSchema(ConnectionFactory, PoolGroup, this, collectionName, restrictionValues);
	}

	internal void NotifyWeakReference(int message)
	{
		InnerConnection.NotifyWeakReference(message);
	}

	internal void PermissionDemand()
	{
		DbConnectionOptions dbConnectionOptions = PoolGroup?.ConnectionOptions;
		if (dbConnectionOptions == null || dbConnectionOptions.IsEmpty)
		{
			throw ADP.NoConnectionString();
		}
		DbConnectionOptions userConnectionOptions = UserConnectionOptions;
		userConnectionOptions.DemandPermission();
	}

	internal void RemoveWeakReference(object value)
	{
		InnerConnection.RemoveWeakReference(value);
	}

	internal void SetInnerConnectionEvent(DbConnectionInternal to)
	{
		ConnectionState connectionState = _innerConnection.State & ConnectionState.Open;
		ConnectionState connectionState2 = to.State & ConnectionState.Open;
		if (connectionState != connectionState2 && connectionState2 == ConnectionState.Closed)
		{
			_closeCount++;
		}
		_innerConnection = to;
		if (connectionState == ConnectionState.Closed && ConnectionState.Open == connectionState2)
		{
			OnStateChange(DbConnectionInternal.StateChangeOpen);
		}
		else if (ConnectionState.Open == connectionState && connectionState2 == ConnectionState.Closed)
		{
			OnStateChange(DbConnectionInternal.StateChangeClosed);
		}
		else if (connectionState != connectionState2)
		{
			OnStateChange(new StateChangeEventArgs(connectionState, connectionState2));
		}
	}

	internal bool SetInnerConnectionFrom(DbConnectionInternal to, DbConnectionInternal from)
	{
		return from == Interlocked.CompareExchange(ref _innerConnection, to, from);
	}

	internal void SetInnerConnectionTo(DbConnectionInternal to)
	{
		_innerConnection = to;
	}

	[Conditional("DEBUG")]
	internal static void VerifyExecutePermission()
	{
		try
		{
			ExecutePermission.Demand();
		}
		catch (SecurityException)
		{
			throw;
		}
	}
}
