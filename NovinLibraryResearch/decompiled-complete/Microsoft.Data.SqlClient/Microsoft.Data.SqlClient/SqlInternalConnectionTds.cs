using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.Common;
using Microsoft.Data.ProviderBase;
using Microsoft.Identity.Client;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlInternalConnectionTds : SqlInternalConnection, IDisposable
{
	internal class SyncAsyncLock
	{
		private SemaphoreSlim semaphore = new SemaphoreSlim(1);

		internal bool CanBeReleasedFromAnyThread => semaphore.CurrentCount == 0;

		internal void Wait(bool canReleaseFromAnyThread)
		{
			Monitor.Enter(semaphore);
			if (canReleaseFromAnyThread || semaphore.CurrentCount == 0)
			{
				semaphore.Wait();
				if (canReleaseFromAnyThread)
				{
					Monitor.Exit(semaphore);
				}
				else
				{
					semaphore.Release();
				}
			}
		}

		internal void Wait(bool canReleaseFromAnyThread, int timeout, ref bool lockTaken)
		{
			lockTaken = false;
			bool lockTaken2 = false;
			try
			{
				Monitor.TryEnter(semaphore, timeout, ref lockTaken2);
				if (!lockTaken2)
				{
					return;
				}
				if (canReleaseFromAnyThread || semaphore.CurrentCount == 0)
				{
					if (semaphore.Wait(timeout))
					{
						if (canReleaseFromAnyThread)
						{
							Monitor.Exit(semaphore);
							lockTaken2 = false;
						}
						else
						{
							semaphore.Release();
						}
						lockTaken = true;
					}
				}
				else
				{
					lockTaken = true;
				}
			}
			finally
			{
				if (!lockTaken && lockTaken2)
				{
					Monitor.Exit(semaphore);
				}
			}
		}

		internal void Release()
		{
			if (semaphore.CurrentCount == 0)
			{
				semaphore.Release();
			}
			else
			{
				Monitor.Exit(semaphore);
			}
		}

		internal bool ThreadMayHaveLock()
		{
			if (!Monitor.IsEntered(semaphore))
			{
				return semaphore.CurrentCount == 0;
			}
			return true;
		}
	}

	internal const int _maxNumberOfRedirectRoute = 10;

	private readonly SqlConnectionPoolGroupProviderInfo _poolGroupProviderInfo;

	private TdsParser _parser;

	private SqlLoginAck _loginAck;

	private SqlCredential _credential;

	private FederatedAuthenticationFeatureExtensionData _fedAuthFeatureExtensionData;

	private bool _sessionRecoveryRequested;

	internal bool _sessionRecoveryAcknowledged;

	internal SessionData _currentSessionData;

	private SessionData _recoverySessionData;

	internal bool _fedAuthRequired;

	internal bool _federatedAuthenticationRequested;

	internal bool _federatedAuthenticationAcknowledged;

	internal bool _federatedAuthenticationInfoRequested;

	internal bool _federatedAuthenticationInfoReceived;

	private SqlFedAuthToken _fedAuthToken;

	internal byte[] _accessTokenInBytes;

	private readonly ActiveDirectoryAuthenticationTimeoutRetryHelper _activeDirectoryAuthTimeoutRetryHelper;

	private readonly SqlAuthenticationProviderManager _sqlAuthenticationProviderManager;

	private ServerCertificateValidationCallback _serverCallback;

	private ClientCertificateRetrievalCallback _clientCallback;

	private SqlClientOriginalNetworkAddressInfo _originalNetworkAddressInfo;

	internal bool _cleanSQLDNSCaching;

	private bool _serverSupportsDNSCaching;

	private bool _SQLDNSRetryEnabled;

	private bool DNSCachingBeforeRedirect;

	internal SQLDNSInfo pendingSQLDNSObject;

	internal byte _tceVersionSupported;

	private DbConnectionPool _dbConnectionPool;

	private DbConnectionPoolAuthenticationContext _newDbConnectionPoolAuthenticationContext;

	private DbConnectionPoolAuthenticationContextKey _dbConnectionPoolAuthenticationContextKey;

	private static readonly TimeSpan _dbAuthenticationContextLockedRefreshTimeSpan;

	private static readonly TimeSpan _dbAuthenticationContextUnLockedRefreshTimeSpan;

	private readonly TimeoutTimer _timeout;

	private static HashSet<int> transientErrors;

	private bool _fConnectionOpen;

	private bool _fResetConnection;

	private string _originalDatabase;

	private string _currentFailoverPartner;

	private string _originalLanguage;

	private string _currentLanguage;

	private int _currentPacketSize;

	private int _asyncCommandCount;

	private string _instanceName = string.Empty;

	private DbConnectionPoolIdentity _identity;

	internal SyncAsyncLock _parserLock = new SyncAsyncLock();

	private int _threadIdOwningParserLock = -1;

	private SqlConnectionTimeoutErrorInternal timeoutErrorInternal;

	internal Guid _clientConnectionId = Guid.Empty;

	private RoutingInfo _routingInfo;

	private Guid _originalClientConnectionId = Guid.Empty;

	private string _routingDestination;

	private int accessTokenExpirationBufferTime
	{
		get
		{
			if (base.ConnectionOptions.ConnectTimeout != 0 && base.ConnectionOptions.ConnectTimeout < 600)
			{
				return base.ConnectionOptions.ConnectTimeout;
			}
			return 600;
		}
	}

	internal bool IsSQLDNSCachingSupported
	{
		get
		{
			return _serverSupportsDNSCaching;
		}
		set
		{
			_serverSupportsDNSCaching = value;
		}
	}

	internal bool IsSQLDNSRetryEnabled
	{
		get
		{
			return _SQLDNSRetryEnabled;
		}
		set
		{
			_SQLDNSRetryEnabled = value;
		}
	}

	internal bool IsDNSCachingBeforeRedirectSupported
	{
		get
		{
			return DNSCachingBeforeRedirect;
		}
		set
		{
			DNSCachingBeforeRedirect = value;
		}
	}

	internal SessionData CurrentSessionData
	{
		get
		{
			if (_currentSessionData != null)
			{
				_currentSessionData._database = base.CurrentDatabase;
				_currentSessionData._language = _currentLanguage;
			}
			return _currentSessionData;
		}
	}

	internal SqlConnectionTimeoutErrorInternal TimeoutErrorInternal => timeoutErrorInternal;

	internal Guid ClientConnectionId => _clientConnectionId;

	internal Guid OriginalClientConnectionId => _originalClientConnectionId;

	internal string RoutingDestination => _routingDestination;

	internal RoutingInfo RoutingInfo => _routingInfo;

	internal override SqlInternalTransaction CurrentTransaction => _parser.CurrentTransaction;

	internal override SqlInternalTransaction AvailableInternalTransaction
	{
		get
		{
			if (!_parser._fResetConnection)
			{
				return CurrentTransaction;
			}
			return null;
		}
	}

	internal override SqlInternalTransaction PendingTransaction => _parser.PendingTransaction;

	internal DbConnectionPoolIdentity Identity => _identity;

	internal string InstanceName => _instanceName;

	internal override bool IsLockedForBulkCopy
	{
		get
		{
			if (!Parser.MARSOn)
			{
				return Parser._physicalStateObj.BcpLock;
			}
			return false;
		}
	}

	protected internal override bool IsNonPoolableTransactionRoot
	{
		get
		{
			if (IsTransactionRoot)
			{
				if (IsKatmaiOrNewer)
				{
					return base.Pool == null;
				}
				return true;
			}
			return false;
		}
	}

	internal override bool IsShiloh => _loginAck.isVersion8;

	internal override bool IsYukonOrNewer => _parser.IsYukonOrNewer;

	internal override bool IsKatmaiOrNewer => _parser.IsKatmaiOrNewer;

	internal int PacketSize => _currentPacketSize;

	internal TdsParser Parser => _parser;

	internal string ServerProvidedFailOverPartner => _currentFailoverPartner;

	internal SqlConnectionPoolGroupProviderInfo PoolGroupProviderInfo => _poolGroupProviderInfo;

	protected override bool ReadyToPrepareTransaction => FindLiveReader(null) == null;

	public override string ServerVersion => $"{_loginAck.majorVersion:00}.{(short)_loginAck.minorVersion:00}.{_loginAck.buildNum:0000}";

	public int ServerProcessId => Parser._physicalStateObj._spid;

	protected override bool UnbindOnTransactionCompletion => false;

	internal override bool IsAccessTokenExpired
	{
		get
		{
			if (_federatedAuthenticationInfoRequested)
			{
				return DateTime.FromFileTimeUtc(_fedAuthToken.expirationFileTime) < DateTime.UtcNow.AddSeconds(accessTokenExpirationBufferTime);
			}
			return false;
		}
	}

	internal bool IgnoreEnvChange => _routingInfo != null;

	internal bool ThreadHasParserLockForClose
	{
		get
		{
			return _threadIdOwningParserLock == Thread.CurrentThread.ManagedThreadId;
		}
		set
		{
			if (value)
			{
				_threadIdOwningParserLock = Thread.CurrentThread.ManagedThreadId;
			}
			else if (_threadIdOwningParserLock == Thread.CurrentThread.ManagedThreadId)
			{
				_threadIdOwningParserLock = -1;
			}
		}
	}

	static SqlInternalConnectionTds()
	{
		_dbAuthenticationContextLockedRefreshTimeSpan = new TimeSpan(0, 45, 0);
		_dbAuthenticationContextUnLockedRefreshTimeSpan = new TimeSpan(0, 10, 0);
		transientErrors = new HashSet<int>();
		populateTransientErrors();
	}

	internal SqlInternalConnectionTds(DbConnectionPoolIdentity identity, SqlConnectionString connectionOptions, SqlCredential credential, object providerInfo, string newPassword, SecureString newSecurePassword, bool redirectedUserInstance, SqlConnectionString userConnectionOptions = null, SessionData reconnectSessionData = null, ServerCertificateValidationCallback serverCallback = null, ClientCertificateRetrievalCallback clientCallback = null, DbConnectionPool pool = null, string accessToken = null, SqlClientOriginalNetworkAddressInfo originalNetworkAddressInfo = null, bool applyTransientFaultHandling = false)
		: base(connectionOptions)
	{
		_dbConnectionPool = pool;
		if (connectionOptions.ConnectRetryCount > 0)
		{
			_recoverySessionData = reconnectSessionData;
			if (reconnectSessionData == null)
			{
				_currentSessionData = new SessionData();
			}
			else
			{
				_currentSessionData = new SessionData(_recoverySessionData);
				_originalDatabase = _recoverySessionData._initialDatabase;
				_originalLanguage = _recoverySessionData._initialLanguage;
			}
		}
		if (connectionOptions.UserInstance && InOutOfProcHelper.InProc)
		{
			throw SQL.UserInstanceNotAvailableInProc();
		}
		if (accessToken != null)
		{
			_accessTokenInBytes = Encoding.Unicode.GetBytes(accessToken);
		}
		_activeDirectoryAuthTimeoutRetryHelper = new ActiveDirectoryAuthenticationTimeoutRetryHelper();
		_sqlAuthenticationProviderManager = SqlAuthenticationProviderManager.Instance;
		_serverCallback = serverCallback;
		_clientCallback = clientCallback;
		_originalNetworkAddressInfo = originalNetworkAddressInfo;
		_identity = identity;
		_poolGroupProviderInfo = (SqlConnectionPoolGroupProviderInfo)providerInfo;
		_fResetConnection = connectionOptions.ConnectionReset;
		if (_fResetConnection && _recoverySessionData == null)
		{
			_originalDatabase = connectionOptions.InitialCatalog;
			_originalLanguage = connectionOptions.CurrentLanguage;
		}
		timeoutErrorInternal = new SqlConnectionTimeoutErrorInternal();
		_credential = credential;
		_parserLock.Wait(canReleaseFromAnyThread: false);
		ThreadHasParserLockForClose = true;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			_timeout = TimeoutTimer.StartSecondsTimeout(connectionOptions.ConnectTimeout);
			int num = ((!applyTransientFaultHandling) ? 1 : (connectionOptions.ConnectRetryCount + 1));
			int num2 = connectionOptions.ConnectRetryInterval * 1000;
			for (int i = 0; i < num; i++)
			{
				try
				{
					OpenLoginEnlist(_timeout, connectionOptions, credential, newPassword, newSecurePassword, redirectedUserInstance);
				}
				catch (SqlException exc)
				{
					if (i + 1 == num || !applyTransientFaultHandling || _timeout.IsExpired || _timeout.MillisecondsRemaining < num2 || !IsTransientError(exc))
					{
						throw;
					}
					Thread.Sleep(num2);
					continue;
				}
				break;
			}
		}
		catch (OutOfMemoryException)
		{
			DoomThisConnection();
			throw;
		}
		catch (StackOverflowException)
		{
			DoomThisConnection();
			throw;
		}
		catch (ThreadAbortException)
		{
			DoomThisConnection();
			throw;
		}
		finally
		{
			ThreadHasParserLockForClose = false;
			_parserLock.Release();
		}
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.ctor|ADV> {0}, constructed new TDS internal connection", base.ObjectID);
	}

	private static void populateTransientErrors()
	{
		transientErrors.Add(4060);
		transientErrors.Add(10928);
		transientErrors.Add(10929);
		transientErrors.Add(40197);
		transientErrors.Add(40020);
		transientErrors.Add(40143);
		transientErrors.Add(40166);
		transientErrors.Add(40540);
		transientErrors.Add(40501);
		transientErrors.Add(40613);
	}

	private bool IsTransientError(SqlException exc)
	{
		if (exc == null)
		{
			return false;
		}
		foreach (SqlError error in exc.Errors)
		{
			if (transientErrors.Contains(error.Number))
			{
				UnDoomThisConnection();
				return true;
			}
		}
		return false;
	}

	protected override void ChangeDatabaseInternal(string database)
	{
		database = SqlConnection.FixupDatabaseTransactionName(database);
		Task task = _parser.TdsExecuteSQLBatch("use " + database, base.ConnectionOptions.ConnectTimeout, null, _parser._physicalStateObj, sync: true);
		_parser.Run(RunBehavior.UntilDone, null, null, null, _parser._physicalStateObj);
	}

	public override void Dispose()
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.Dispose|ADV> {0} disposing", base.ObjectID);
		try
		{
			Interlocked.Exchange(ref _parser, null)?.Disconnect();
		}
		finally
		{
			_loginAck = null;
			_fConnectionOpen = false;
		}
		base.Dispose();
	}

	internal override void ValidateConnectionForExecute(SqlCommand command)
	{
		TdsParser parser = _parser;
		if (parser == null || parser.State == TdsParserState.Broken || parser.State == TdsParserState.Closed)
		{
			throw ADP.ClosedConnectionError();
		}
		SqlDataReader sqlDataReader = null;
		if (parser.MARSOn)
		{
			if (command != null)
			{
				sqlDataReader = FindLiveReader(command);
			}
		}
		else
		{
			if (_asyncCommandCount > 0)
			{
				throw SQL.MARSUnsupportedOnConnection();
			}
			sqlDataReader = FindLiveReader(null);
		}
		if (sqlDataReader != null)
		{
			throw ADP.OpenReaderExists(parser.MARSOn);
		}
		if (!parser.MARSOn && parser._physicalStateObj._pendingData)
		{
			parser.DrainData(parser._physicalStateObj);
		}
		parser.RollbackOrphanedAPITransactions();
	}

	internal void CheckEnlistedTransactionBinding()
	{
		Transaction enlistedTransaction = base.EnlistedTransaction;
		if (!(enlistedTransaction != null))
		{
			return;
		}
		if (base.ConnectionOptions.TransactionBinding == SqlConnectionString.TransactionBindingEnum.ExplicitUnbind)
		{
			Transaction current = Transaction.Current;
			if (enlistedTransaction.TransactionInformation.Status != 0 || !enlistedTransaction.Equals(current))
			{
				throw ADP.TransactionConnectionMismatch();
			}
		}
		else if (enlistedTransaction.TransactionInformation.Status != 0)
		{
			if (!base.EnlistedTransactionDisposed)
			{
				throw ADP.TransactionCompletedButNotDisposed();
			}
			DetachTransaction(enlistedTransaction, isExplicitlyReleasing: true);
		}
	}

	internal override bool IsConnectionAlive(bool throwOnException)
	{
		bool flag = false;
		return _parser._physicalStateObj.IsConnectionAlive(throwOnException);
	}

	protected override void Activate(Transaction transaction)
	{
		FailoverPermissionDemand();
		if (null != transaction)
		{
			if (base.ConnectionOptions.Enlist)
			{
				Enlist(transaction);
			}
		}
		else
		{
			Enlist(null);
		}
	}

	protected override void InternalDeactivate()
	{
		if (_asyncCommandCount != 0)
		{
			DoomThisConnection();
		}
		if (!IsNonPoolableTransactionRoot && _parser != null)
		{
			_parser.Deactivate(base.IsConnectionDoomed);
			if (!base.IsConnectionDoomed)
			{
				ResetConnection();
			}
		}
	}

	private void ResetConnection()
	{
		if (!_fResetConnection)
		{
			return;
		}
		if (IsShiloh)
		{
			_parser.PrepareResetConnection(IsTransactionRoot && !IsNonPoolableTransactionRoot);
		}
		else if (!base.IsEnlistedInTransaction)
		{
			try
			{
				Task task = _parser.TdsExecuteSQLBatch("sp_reset_connection", 30, null, _parser._physicalStateObj, sync: true);
				_parser.Run(RunBehavior.UntilDone, null, null, null, _parser._physicalStateObj);
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					throw;
				}
				DoomThisConnection();
				ADP.TraceExceptionWithoutRethrow(e);
			}
		}
		base.CurrentDatabase = _originalDatabase;
		_currentLanguage = _originalLanguage;
	}

	internal void DecrementAsyncCount()
	{
		Interlocked.Decrement(ref _asyncCommandCount);
	}

	internal void IncrementAsyncCount()
	{
		Interlocked.Increment(ref _asyncCommandCount);
	}

	internal override void DisconnectTransaction(SqlInternalTransaction internalTransaction)
	{
		Parser?.DisconnectTransaction(internalTransaction);
	}

	internal void ExecuteTransaction(TransactionRequest transactionRequest, string name, System.Data.IsolationLevel iso)
	{
		ExecuteTransaction(transactionRequest, name, iso, null, isDelegateControlRequest: false);
	}

	internal override void ExecuteTransaction(TransactionRequest transactionRequest, string name, System.Data.IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest)
	{
		if (base.IsConnectionDoomed)
		{
			if (transactionRequest != TransactionRequest.Rollback && transactionRequest != TransactionRequest.IfRollback)
			{
				throw SQL.ConnectionDoomed();
			}
			return;
		}
		if ((transactionRequest == TransactionRequest.Commit || transactionRequest == TransactionRequest.Rollback || transactionRequest == TransactionRequest.IfRollback) && !Parser.MARSOn && Parser._physicalStateObj.BcpLock)
		{
			throw SQL.ConnectionLockedForBcpEvent();
		}
		string transactionName = ((name == null) ? string.Empty : name);
		if (!_parser.IsYukonOrNewer)
		{
			ExecuteTransactionPreYukon(transactionRequest, transactionName, iso, internalTransaction);
		}
		else
		{
			ExecuteTransactionYukon(transactionRequest, transactionName, iso, internalTransaction, isDelegateControlRequest);
		}
	}

	internal void ExecuteTransactionPreYukon(TransactionRequest transactionRequest, string transactionName, System.Data.IsolationLevel iso, SqlInternalTransaction internalTransaction)
	{
		StringBuilder stringBuilder = new StringBuilder();
		switch (iso)
		{
		case System.Data.IsolationLevel.ReadCommitted:
			stringBuilder.Append("SET TRANSACTION ISOLATION LEVEL READ COMMITTED");
			stringBuilder.Append(";");
			break;
		case System.Data.IsolationLevel.ReadUncommitted:
			stringBuilder.Append("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
			stringBuilder.Append(";");
			break;
		case System.Data.IsolationLevel.RepeatableRead:
			stringBuilder.Append("SET TRANSACTION ISOLATION LEVEL REPEATABLE READ");
			stringBuilder.Append(";");
			break;
		case System.Data.IsolationLevel.Serializable:
			stringBuilder.Append("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE");
			stringBuilder.Append(";");
			break;
		case System.Data.IsolationLevel.Snapshot:
			throw SQL.SnapshotNotSupported(System.Data.IsolationLevel.Snapshot);
		case System.Data.IsolationLevel.Chaos:
			throw SQL.NotSupportedIsolationLevel(iso);
		default:
			throw ADP.InvalidIsolationLevel(iso);
		case System.Data.IsolationLevel.Unspecified:
			break;
		}
		if (!ADP.IsEmpty(transactionName))
		{
			transactionName = " " + SqlConnection.FixupDatabaseTransactionName(transactionName);
		}
		switch (transactionRequest)
		{
		case TransactionRequest.Begin:
			stringBuilder.Append("BEGIN TRANSACTION");
			stringBuilder.Append(transactionName);
			break;
		case TransactionRequest.Commit:
			stringBuilder.Append("COMMIT TRANSACTION");
			stringBuilder.Append(transactionName);
			break;
		case TransactionRequest.Rollback:
			stringBuilder.Append("ROLLBACK TRANSACTION");
			stringBuilder.Append(transactionName);
			break;
		case TransactionRequest.IfRollback:
			stringBuilder.Append("IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION");
			stringBuilder.Append(transactionName);
			break;
		case TransactionRequest.Save:
			stringBuilder.Append("SAVE TRANSACTION");
			stringBuilder.Append(transactionName);
			break;
		}
		Task task = _parser.TdsExecuteSQLBatch(stringBuilder.ToString(), base.ConnectionOptions.ConnectTimeout, null, _parser._physicalStateObj, sync: true);
		_parser.Run(RunBehavior.UntilDone, null, null, null, _parser._physicalStateObj);
		if (transactionRequest == TransactionRequest.Begin)
		{
			_parser.CurrentTransaction = internalTransaction;
		}
	}

	internal void ExecuteTransactionYukon(TransactionRequest transactionRequest, string transactionName, System.Data.IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest)
	{
		TdsEnums.TransactionManagerRequestType request = TdsEnums.TransactionManagerRequestType.Begin;
		TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel = TdsEnums.TransactionManagerIsolationLevel.ReadCommitted;
		transactionManagerIsolationLevel = iso switch
		{
			System.Data.IsolationLevel.Unspecified => TdsEnums.TransactionManagerIsolationLevel.Unspecified, 
			System.Data.IsolationLevel.ReadCommitted => TdsEnums.TransactionManagerIsolationLevel.ReadCommitted, 
			System.Data.IsolationLevel.ReadUncommitted => TdsEnums.TransactionManagerIsolationLevel.ReadUncommitted, 
			System.Data.IsolationLevel.RepeatableRead => TdsEnums.TransactionManagerIsolationLevel.RepeatableRead, 
			System.Data.IsolationLevel.Serializable => TdsEnums.TransactionManagerIsolationLevel.Serializable, 
			System.Data.IsolationLevel.Snapshot => TdsEnums.TransactionManagerIsolationLevel.Snapshot, 
			System.Data.IsolationLevel.Chaos => throw SQL.NotSupportedIsolationLevel(iso), 
			_ => throw ADP.InvalidIsolationLevel(iso), 
		};
		TdsParserStateObject tdsParserStateObject = _parser._physicalStateObj;
		TdsParser parser = _parser;
		bool flag = false;
		bool releaseConnectionLock = false;
		if (!ThreadHasParserLockForClose)
		{
			_parserLock.Wait(canReleaseFromAnyThread: false);
			ThreadHasParserLockForClose = true;
			releaseConnectionLock = true;
		}
		try
		{
			switch (transactionRequest)
			{
			case TransactionRequest.Begin:
				request = TdsEnums.TransactionManagerRequestType.Begin;
				break;
			case TransactionRequest.Promote:
				request = TdsEnums.TransactionManagerRequestType.Promote;
				break;
			case TransactionRequest.Commit:
				request = TdsEnums.TransactionManagerRequestType.Commit;
				break;
			case TransactionRequest.Rollback:
			case TransactionRequest.IfRollback:
				request = TdsEnums.TransactionManagerRequestType.Rollback;
				break;
			case TransactionRequest.Save:
				request = TdsEnums.TransactionManagerRequestType.Save;
				break;
			}
			if ((internalTransaction?.RestoreBrokenConnection ?? false) && releaseConnectionLock)
			{
				Task task = internalTransaction.Parent.Connection.ValidateAndReconnect(delegate
				{
					ThreadHasParserLockForClose = false;
					_parserLock.Release();
					releaseConnectionLock = false;
				}, 0);
				if (task != null)
				{
					AsyncHelper.WaitForCompletion(task, 0);
					internalTransaction.ConnectionHasBeenRestored = true;
					return;
				}
			}
			if (internalTransaction != null && internalTransaction.IsDelegated)
			{
				if (_parser.MARSOn)
				{
					tdsParserStateObject = _parser.GetSession(this);
					flag = true;
				}
				if (internalTransaction.OpenResultsCount != 0)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.ExecuteTransactionYukon|DATA|CATCH> {0}, Connection is marked to be doomed when closed. Transaction ended with OpenResultsCount {1} > 0, MARSOn {2}", base.ObjectID, internalTransaction.OpenResultsCount, _parser.MARSOn);
					throw SQL.CannotCompleteDelegatedTransactionWithOpenResults(this, _parser.MARSOn);
				}
			}
			_parser.TdsExecuteTransactionManagerRequest(null, request, transactionName, transactionManagerIsolationLevel, base.ConnectionOptions.ConnectTimeout, internalTransaction, tdsParserStateObject, isDelegateControlRequest);
		}
		finally
		{
			if (flag)
			{
				parser.PutSession(tdsParserStateObject);
			}
			if (releaseConnectionLock)
			{
				ThreadHasParserLockForClose = false;
				_parserLock.Release();
			}
		}
	}

	internal override void DelegatedTransactionEnded()
	{
		base.DelegatedTransactionEnded();
	}

	protected override byte[] GetDTCAddress()
	{
		return _parser.GetDTCAddress(base.ConnectionOptions.ConnectTimeout, _parser.GetSession(this));
	}

	protected override void PropagateTransactionCookie(byte[] cookie)
	{
		_parser.PropagateDistributedTransaction(cookie, base.ConnectionOptions.ConnectTimeout, _parser._physicalStateObj);
	}

	private void CompleteLogin(bool enlistOK)
	{
		_parser.Run(RunBehavior.UntilDone, null, null, null, _parser._physicalStateObj);
		if (_routingInfo == null)
		{
			if (_federatedAuthenticationRequested && !_federatedAuthenticationAcknowledged)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.CompleteLogin|ERR> {0}, Server did not acknowledge the federated authentication request", base.ObjectID);
				throw SQL.ParsingError(ParsingErrorState.FedAuthNotAcknowledged);
			}
			if (_federatedAuthenticationInfoRequested && !_federatedAuthenticationInfoReceived)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.CompleteLogin|ERR> {0}, Server never sent the requested federated authentication info", base.ObjectID);
				throw SQL.ParsingError(ParsingErrorState.FedAuthInfoNotReceived);
			}
			if (!_sessionRecoveryAcknowledged)
			{
				_currentSessionData = null;
				if (_recoverySessionData != null)
				{
					throw SQL.CR_NoCRAckAtReconnection(this);
				}
			}
			if (_currentSessionData != null && _recoverySessionData == null)
			{
				_currentSessionData._initialDatabase = base.CurrentDatabase;
				_currentSessionData._initialCollation = _currentSessionData._collation;
				_currentSessionData._initialLanguage = _currentLanguage;
			}
			bool flag = (_parser.EncryptionOptions & EncryptionOptions.OPTIONS_MASK) == EncryptionOptions.ON;
			if (_recoverySessionData != null && _recoverySessionData._encrypted != flag)
			{
				throw SQL.CR_EncryptionChanged(this);
			}
			if (_currentSessionData != null)
			{
				_currentSessionData._encrypted = flag;
			}
			_recoverySessionData = null;
		}
		_parser._physicalStateObj.SniContext = SniContext.Snix_EnableMars;
		_parser.EnableMars();
		_fConnectionOpen = true;
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.CompleteLogin|ADV> Post-Login Phase: Server connection obtained.");
		if (enlistOK && base.ConnectionOptions.Enlist)
		{
			_parser._physicalStateObj.SniContext = SniContext.Snix_AutoEnlist;
			Transaction currentTransaction = ADP.GetCurrentTransaction();
			Enlist(currentTransaction);
		}
		_parser._physicalStateObj.SniContext = SniContext.Snix_Login;
	}

	private void Login(ServerInfo server, TimeoutTimer timeout, string newPassword, SecureString newSecurePassword)
	{
		SqlLogin sqlLogin = new SqlLogin();
		base.CurrentDatabase = server.ResolvedDatabaseName;
		_currentPacketSize = base.ConnectionOptions.PacketSize;
		_currentLanguage = base.ConnectionOptions.CurrentLanguage;
		int timeout2 = 0;
		if (!timeout.IsInfinite)
		{
			long num = timeout.MillisecondsRemaining / 1000;
			if (num == 0L && LocalAppContextSwitches.UseMinimumLoginTimeout)
			{
				num = 1L;
			}
			if (int.MaxValue > num)
			{
				timeout2 = (int)num;
			}
		}
		sqlLogin.authentication = base.ConnectionOptions.Authentication;
		sqlLogin.timeout = timeout2;
		sqlLogin.userInstance = base.ConnectionOptions.UserInstance;
		sqlLogin.hostName = base.ConnectionOptions.ObtainWorkstationId();
		sqlLogin.userName = base.ConnectionOptions.UserID;
		sqlLogin.password = base.ConnectionOptions.Password;
		sqlLogin.applicationName = base.ConnectionOptions.ApplicationName;
		sqlLogin.language = _currentLanguage;
		if (!sqlLogin.userInstance)
		{
			sqlLogin.database = base.CurrentDatabase;
			sqlLogin.attachDBFilename = base.ConnectionOptions.AttachDBFilename;
		}
		sqlLogin.serverName = server.UserServerName;
		sqlLogin.useReplication = base.ConnectionOptions.Replication;
		sqlLogin.useSSPI = base.ConnectionOptions.IntegratedSecurity || (base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated && !_fedAuthRequired);
		sqlLogin.packetSize = _currentPacketSize;
		sqlLogin.newPassword = newPassword;
		sqlLogin.readOnlyIntent = base.ConnectionOptions.ApplicationIntent == ApplicationIntent.ReadOnly;
		sqlLogin.credential = _credential;
		if (newSecurePassword != null)
		{
			sqlLogin.newSecurePassword = newSecurePassword;
		}
		TdsEnums.FeatureExtension featureExtension = TdsEnums.FeatureExtension.None;
		if (base.ConnectionOptions.ConnectRetryCount > 0)
		{
			featureExtension |= TdsEnums.FeatureExtension.SessionRecovery;
			_sessionRecoveryRequested = true;
		}
		if (base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryPassword || base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryInteractive || base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryServicePrincipal || base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow || base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryManagedIdentity || base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryMSI || base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryDefault || (base.ConnectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated && _fedAuthRequired))
		{
			featureExtension |= TdsEnums.FeatureExtension.FedAuth;
			_federatedAuthenticationInfoRequested = true;
			_fedAuthFeatureExtensionData = new FederatedAuthenticationFeatureExtensionData
			{
				libraryType = TdsEnums.FedAuthLibrary.MSAL,
				authentication = base.ConnectionOptions.Authentication,
				fedAuthRequiredPreLoginResponse = _fedAuthRequired
			};
		}
		if (_accessTokenInBytes != null)
		{
			featureExtension |= TdsEnums.FeatureExtension.FedAuth;
			_fedAuthFeatureExtensionData = new FederatedAuthenticationFeatureExtensionData
			{
				libraryType = TdsEnums.FedAuthLibrary.SecurityToken,
				fedAuthRequiredPreLoginResponse = _fedAuthRequired,
				accessToken = _accessTokenInBytes
			};
			_federatedAuthenticationRequested = true;
		}
		featureExtension |= TdsEnums.FeatureExtension.Tce | TdsEnums.FeatureExtension.GlobalTransactions | TdsEnums.FeatureExtension.DataClassification;
		featureExtension |= TdsEnums.FeatureExtension.UTF8Support;
		if (base.ConnectionOptions.ApplicationIntent == ApplicationIntent.ReadOnly)
		{
			featureExtension |= TdsEnums.FeatureExtension.AzureSQLSupport;
		}
		featureExtension |= TdsEnums.FeatureExtension.SQLDNSCaching;
		_parser.TdsLogin(sqlLogin, featureExtension, _recoverySessionData, _fedAuthFeatureExtensionData, _originalNetworkAddressInfo);
	}

	private void LoginFailure()
	{
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.LoginFailure|RES|CPOOL> {0}", base.ObjectID);
		if (_parser != null)
		{
			_parser.Disconnect();
		}
	}

	private void OpenLoginEnlist(TimeoutTimer timeout, SqlConnectionString connectionOptions, SqlCredential credential, string newPassword, SecureString newSecurePassword, bool redirectedUserInstance)
	{
		ServerInfo serverInfo = new ServerInfo(connectionOptions);
		bool flag;
		string failoverPartner;
		if (PoolGroupProviderInfo != null)
		{
			flag = PoolGroupProviderInfo.UseFailoverPartner;
			failoverPartner = PoolGroupProviderInfo.FailoverPartner;
		}
		else
		{
			flag = false;
			failoverPartner = base.ConnectionOptions.FailoverPartner;
		}
		timeoutErrorInternal.SetInternalSourceType(flag ? SqlConnectionInternalSourceType.Failover : SqlConnectionInternalSourceType.Principle);
		bool flag2 = !ADP.IsEmpty(failoverPartner);
		try
		{
			timeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.PreLoginBegin);
			if (flag2)
			{
				timeoutErrorInternal.SetFailoverScenario(useFailoverServer: true);
				LoginWithFailover(flag, serverInfo, failoverPartner, newPassword, newSecurePassword, redirectedUserInstance, connectionOptions, credential, timeout);
			}
			else
			{
				timeoutErrorInternal.SetFailoverScenario(useFailoverServer: false);
				LoginNoFailover(serverInfo, newPassword, newSecurePassword, redirectedUserInstance, connectionOptions, credential, timeout);
			}
			if (!base.IsAzureSQLConnection && base.ConnectionOptions.ApplicationIntent == ApplicationIntent.ReadOnly)
			{
				if (!string.IsNullOrEmpty(base.ConnectionOptions.FailoverPartner))
				{
					throw SQL.ROR_FailoverNotSupportedConnString();
				}
				if (ServerProvidedFailOverPartner != null)
				{
					throw SQL.ROR_FailoverNotSupportedServer(this);
				}
			}
			timeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.PostLogin);
		}
		catch (Exception e)
		{
			if (ADP.IsCatchableExceptionType(e))
			{
				LoginFailure();
			}
			throw;
		}
		timeoutErrorInternal.SetAllCompleteMarker();
	}

	private bool IsDoNotRetryConnectError(SqlException exc)
	{
		if (18456 != exc.Number && 18488 != exc.Number && 1346 != exc.Number)
		{
			return exc._doNotReconnect;
		}
		return true;
	}

	private void LoginNoFailover(ServerInfo serverInfo, string newPassword, SecureString newSecurePassword, bool redirectedUserInstance, SqlConnectionString connectionOptions, SqlCredential credential, TimeoutTimer timeout)
	{
		int num = 0;
		ServerInfo serverInfo2 = serverInfo;
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.LoginNoFailover|ADV> {0}, host={1}", base.ObjectID, serverInfo.UserServerName);
		int num2 = 100;
		ResolveExtendedServerName(serverInfo, !redirectedUserInstance, connectionOptions);
		bool flag = ShouldDisableTnir(connectionOptions);
		long num3 = 0L;
		bool flag2 = connectionOptions.MultiSubnetFailover || (connectionOptions.TransparentNetworkIPResolution && !flag);
		if (flag2)
		{
			float num4 = (connectionOptions.MultiSubnetFailover ? 0.08f : 0.125f);
			num3 = checked((!timeout.IsInfinite) ? ((long)(num4 * (float)timeout.MillisecondsRemaining)) : ((long)(num4 * 15000f)));
		}
		int num5 = 0;
		TimeoutTimer timeoutTimer = null;
		TimeoutTimer timeout2 = timeout;
		while (true)
		{
			bool flag3 = connectionOptions.TransparentNetworkIPResolution && !flag && num5 == 1;
			if (flag2)
			{
				int num6 = ++num5;
				if (connectionOptions.TransparentNetworkIPResolution)
				{
					num6 = 1 << num5 - 1;
				}
				long num7 = checked(num3 * num6);
				long millisecondsRemaining = timeout.MillisecondsRemaining;
				if (flag3)
				{
					num7 = Math.Max(500L, num7);
				}
				if (num7 > millisecondsRemaining)
				{
					num7 = millisecondsRemaining;
				}
				timeoutTimer = TimeoutTimer.StartMillisecondsTimeout(num7);
			}
			if (_parser != null)
			{
				_parser.Disconnect();
			}
			_parser = new TdsParser(base.ConnectionOptions.MARS, base.ConnectionOptions.Asynchronous);
			try
			{
				if (flag2)
				{
					timeout2 = timeoutTimer;
				}
				AttemptOneLogin(serverInfo, newPassword, newSecurePassword, !flag2, timeout2, withFailover: false, flag3, flag);
				if (connectionOptions.MultiSubnetFailover && ServerProvidedFailOverPartner != null)
				{
					throw SQL.MultiSubnetFailoverWithFailoverPartner(serverProvidedFailoverPartner: true, this);
				}
				if (_routingInfo != null)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.LoginNoFailover> Routed to {0}", serverInfo.ExtendedServerName);
					if (num > 10)
					{
						throw SQL.ROR_RecursiveRoutingNotSupported(this);
					}
					if (timeout.IsExpired)
					{
						throw SQL.ROR_TimeoutAfterRoutingInfo(this);
					}
					serverInfo = new ServerInfo(base.ConnectionOptions, _routingInfo, serverInfo.ResolvedServerName);
					timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.RoutingDestination);
					_originalClientConnectionId = _clientConnectionId;
					_routingDestination = serverInfo.UserServerName;
					_currentPacketSize = base.ConnectionOptions.PacketSize;
					_currentLanguage = (_originalLanguage = base.ConnectionOptions.CurrentLanguage);
					base.CurrentDatabase = (_originalDatabase = base.ConnectionOptions.InitialCatalog);
					_currentFailoverPartner = null;
					_instanceName = string.Empty;
					num++;
					continue;
				}
			}
			catch (SqlException ex)
			{
				if (AttemptRetryADAuthWithTimeoutError(ex, connectionOptions, timeout))
				{
					continue;
				}
				if (_parser == null || _parser.State != 0 || IsDoNotRetryConnectError(ex) || timeout.IsExpired)
				{
					throw;
				}
				if (timeout.MillisecondsRemaining <= num2)
				{
					throw;
				}
				goto IL_02ab;
			}
			break;
			IL_02ab:
			if (ServerProvidedFailOverPartner != null)
			{
				if (connectionOptions.MultiSubnetFailover)
				{
					throw SQL.MultiSubnetFailoverWithFailoverPartner(serverProvidedFailoverPartner: true, this);
				}
				timeoutErrorInternal.ResetAndRestartPhase();
				timeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.PreLoginBegin);
				timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.Failover);
				timeoutErrorInternal.SetFailoverScenario(useFailoverServer: true);
				LoginWithFailover(useFailoverHost: true, serverInfo, ServerProvidedFailOverPartner, newPassword, newSecurePassword, redirectedUserInstance, connectionOptions, credential, timeout);
				return;
			}
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.LoginNoFailover|ADV> {0}, sleeping {1}[milisec]", base.ObjectID, num2);
			Thread.Sleep(num2);
			num2 = ((num2 < 500) ? (num2 * 2) : 1000);
		}
		_activeDirectoryAuthTimeoutRetryHelper.State = ActiveDirectoryAuthenticationTimeoutRetryState.HasLoggedIn;
		if (PoolGroupProviderInfo != null)
		{
			PoolGroupProviderInfo.FailoverCheck(this, actualUseFailoverPartner: false, connectionOptions, ServerProvidedFailOverPartner);
		}
		base.CurrentDataSource = serverInfo2.UserServerName;
	}

	private bool ShouldDisableTnir(SqlConnectionString connectionOptions)
	{
		bool flag = ADP.IsAzureSqlServerEndpoint(connectionOptions.DataSource);
		bool flag2 = _accessTokenInBytes != null || connectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryPassword || connectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated || connectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryInteractive || connectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryServicePrincipal || connectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow || connectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryManagedIdentity || connectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryMSI || connectionOptions.Authentication == SqlAuthenticationMethod.ActiveDirectoryDefault;
		if (connectionOptions.Parsetable["transparent network ip resolution"] == null)
		{
			return flag || flag2;
		}
		return false;
	}

	private bool AttemptRetryADAuthWithTimeoutError(SqlException sqlex, SqlConnectionString connectionOptions, TimeoutTimer timeout)
	{
		if (!_activeDirectoryAuthTimeoutRetryHelper.CanRetryWithSqlException(sqlex))
		{
			return false;
		}
		timeout.Reset();
		_dbConnectionPoolAuthenticationContextKey = null;
		UnDoomThisConnection();
		_activeDirectoryAuthTimeoutRetryHelper.State = ActiveDirectoryAuthenticationTimeoutRetryState.Retrying;
		return true;
	}

	private void LoginWithFailover(bool useFailoverHost, ServerInfo primaryServerInfo, string failoverHost, string newPassword, SecureString newSecurePassword, bool redirectedUserInstance, SqlConnectionString connectionOptions, SqlCredential credential, TimeoutTimer timeout)
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.LoginWithFailover|ADV> {0}, useFailover={1}[bool], primary={2}, failover={3}", base.ObjectID, useFailoverHost, primaryServerInfo.UserServerName, failoverHost);
		int num = 100;
		string networkLibrary = base.ConnectionOptions.NetworkLibrary;
		ServerInfo serverInfo = new ServerInfo(connectionOptions, failoverHost);
		ResolveExtendedServerName(primaryServerInfo, !redirectedUserInstance, connectionOptions);
		if (ServerProvidedFailOverPartner == null)
		{
			ResolveExtendedServerName(serverInfo, !redirectedUserInstance && failoverHost != primaryServerInfo.UserServerName, connectionOptions);
		}
		long num2 = checked((!timeout.IsInfinite) ? ((long)(0.08f * (float)timeout.MillisecondsRemaining)) : ((long)(0.08f * (float)ADP.TimerFromSeconds(15))));
		bool flag = false;
		int num3 = 0;
		while (true)
		{
			TimeoutTimer timeout2;
			ServerInfo serverInfo2;
			checked
			{
				long num4 = num2 * (unchecked(num3 / 2) + 1);
				long millisecondsRemaining = timeout.MillisecondsRemaining;
				if (num4 > millisecondsRemaining)
				{
					num4 = millisecondsRemaining;
				}
				timeout2 = TimeoutTimer.StartMillisecondsTimeout(num4);
				if (_parser != null)
				{
					_parser.Disconnect();
				}
				_parser = new TdsParser(base.ConnectionOptions.MARS, base.ConnectionOptions.Asynchronous);
				if (useFailoverHost)
				{
					if (!flag)
					{
						FailoverPermissionDemand();
						flag = true;
					}
					if (ServerProvidedFailOverPartner != null && serverInfo.ResolvedServerName != ServerProvidedFailOverPartner)
					{
						SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.LoginWithFailover|ADV> {0}, new failover partner={1}", base.ObjectID, ServerProvidedFailOverPartner);
						serverInfo.SetDerivedNames(networkLibrary, ServerProvidedFailOverPartner);
					}
					serverInfo2 = serverInfo;
					timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.Failover);
				}
				else
				{
					serverInfo2 = primaryServerInfo;
					timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.Principle);
				}
			}
			try
			{
				AttemptOneLogin(serverInfo2, newPassword, newSecurePassword, ignoreSniOpenTimeout: false, timeout2, withFailover: true);
				int num5 = 0;
				while (_routingInfo != null)
				{
					if (num5 > 10)
					{
						throw SQL.ROR_RecursiveRoutingNotSupported(this);
					}
					num5++;
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.LoginWithFailover> Routed to {0}", _routingInfo.ServerName);
					if (_parser != null)
					{
						_parser.Disconnect();
					}
					_parser = new TdsParser(base.ConnectionOptions.MARS, base.ConnectionOptions.Asynchronous);
					serverInfo2 = new ServerInfo(base.ConnectionOptions, _routingInfo, serverInfo2.ResolvedServerName);
					timeoutErrorInternal.SetInternalSourceType(SqlConnectionInternalSourceType.RoutingDestination);
					_originalClientConnectionId = _clientConnectionId;
					_routingDestination = serverInfo2.UserServerName;
					_currentPacketSize = base.ConnectionOptions.PacketSize;
					_currentLanguage = (_originalLanguage = base.ConnectionOptions.CurrentLanguage);
					base.CurrentDatabase = (_originalDatabase = base.ConnectionOptions.InitialCatalog);
					_currentFailoverPartner = null;
					_instanceName = string.Empty;
					AttemptOneLogin(serverInfo2, newPassword, newSecurePassword, ignoreSniOpenTimeout: false, timeout2, withFailover: true);
				}
			}
			catch (SqlException ex)
			{
				if (AttemptRetryADAuthWithTimeoutError(ex, connectionOptions, timeout))
				{
					continue;
				}
				if (IsDoNotRetryConnectError(ex) || timeout.IsExpired)
				{
					throw;
				}
				if (!ADP.IsAzureSqlServerEndpoint(connectionOptions.DataSource) && base.IsConnectionDoomed)
				{
					throw;
				}
				if (1 == num3 % 2 && timeout.MillisecondsRemaining <= num)
				{
					throw;
				}
				goto IL_02f6;
			}
			break;
			IL_02f6:
			if (1 == num3 % 2)
			{
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.LoginWithFailover|ADV> {0}, sleeping {1}[milisec]", base.ObjectID, num);
				Thread.Sleep(num);
				num = ((num < 500) ? (num * 2) : 1000);
			}
			num3++;
			useFailoverHost = !useFailoverHost;
		}
		_activeDirectoryAuthTimeoutRetryHelper.State = ActiveDirectoryAuthenticationTimeoutRetryState.HasLoggedIn;
		if (useFailoverHost && ServerProvidedFailOverPartner == null)
		{
			throw SQL.InvalidPartnerConfiguration(failoverHost, base.CurrentDatabase);
		}
		if (PoolGroupProviderInfo != null)
		{
			PoolGroupProviderInfo.FailoverCheck(this, useFailoverHost, connectionOptions, ServerProvidedFailOverPartner);
		}
		base.CurrentDataSource = (useFailoverHost ? failoverHost : primaryServerInfo.UserServerName);
	}

	private void ResolveExtendedServerName(ServerInfo serverInfo, bool aliasLookup, SqlConnectionString options)
	{
		if (serverInfo.ExtendedServerName != null)
		{
			return;
		}
		string host = serverInfo.UserServerName;
		string protocol = serverInfo.UserProtocol;
		if (aliasLookup)
		{
			if (_currentSessionData != null && !string.IsNullOrEmpty(host))
			{
				if (_currentSessionData._resolvedAliases.TryGetValue(host, out var value))
				{
					host = value.Item1;
					protocol = value.Item2;
				}
				else
				{
					TdsParserStaticMethods.AliasRegistryLookup(ref host, ref protocol);
					_currentSessionData._resolvedAliases.Add(serverInfo.UserServerName, new Tuple<string, string>(host, protocol));
				}
			}
			else
			{
				TdsParserStaticMethods.AliasRegistryLookup(ref host, ref protocol);
			}
			if (options.EnforceLocalHost)
			{
				SqlConnectionString.VerifyLocalHostAndFixup(ref host, enforceLocalHost: true, fixup: true);
			}
		}
		serverInfo.SetDerivedNames(protocol, host);
	}

	private void AttemptOneLogin(ServerInfo serverInfo, string newPassword, SecureString newSecurePassword, bool ignoreSniOpenTimeout, TimeoutTimer timeout, bool withFailover = false, bool isFirstTransparentAttempt = true, bool disableTnir = false)
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.AttemptOneLogin|ADV> {0}, timout={1}[msec], server={2}", base.ObjectID, timeout.MillisecondsRemaining, serverInfo.ExtendedServerName);
		_routingInfo = null;
		_parser._physicalStateObj.SniContext = SniContext.Snix_Connect;
		_parser.Connect(serverInfo, this, ignoreSniOpenTimeout, timeout.LegacyTimerExpire, base.ConnectionOptions.Encrypt, base.ConnectionOptions.TrustServerCertificate, base.ConnectionOptions.IntegratedSecurity, withFailover, isFirstTransparentAttempt, base.ConnectionOptions.Authentication, base.ConnectionOptions.Certificate, _serverCallback, _clientCallback, _originalNetworkAddressInfo != null, disableTnir);
		timeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.ConsumePreLoginHandshake);
		timeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.LoginBegin);
		_parser._physicalStateObj.SniContext = SniContext.Snix_Login;
		Login(serverInfo, timeout, newPassword, newSecurePassword);
		timeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.ProcessConnectionAuth);
		timeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.PostLogin);
		CompleteLogin(!base.ConnectionOptions.Pooling);
		timeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.PostLogin);
	}

	internal void FailoverPermissionDemand()
	{
		if (PoolGroupProviderInfo != null)
		{
			PoolGroupProviderInfo.FailoverPermissionDemand();
		}
	}

	protected override object ObtainAdditionalLocksForClose()
	{
		bool flag = !ThreadHasParserLockForClose;
		if (flag)
		{
			_parserLock.Wait(canReleaseFromAnyThread: false);
			ThreadHasParserLockForClose = true;
		}
		return flag;
	}

	protected override void ReleaseAdditionalLocksForClose(object lockToken)
	{
		if ((bool)lockToken)
		{
			ThreadHasParserLockForClose = false;
			_parserLock.Release();
		}
	}

	internal bool GetSessionAndReconnectIfNeeded(SqlConnection parent, int timeout = 0)
	{
		if (ThreadHasParserLockForClose)
		{
			return false;
		}
		_parserLock.Wait(canReleaseFromAnyThread: false);
		ThreadHasParserLockForClose = true;
		bool releaseConnectionLock = true;
		try
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Task task = parent.ValidateAndReconnect(delegate
				{
					ThreadHasParserLockForClose = false;
					_parserLock.Release();
					releaseConnectionLock = false;
				}, timeout);
				if (task != null)
				{
					AsyncHelper.WaitForCompletion(task, timeout);
					return true;
				}
				return false;
			}
			catch (OutOfMemoryException)
			{
				DoomThisConnection();
				throw;
			}
			catch (StackOverflowException)
			{
				DoomThisConnection();
				throw;
			}
			catch (ThreadAbortException)
			{
				DoomThisConnection();
				throw;
			}
		}
		finally
		{
			if (releaseConnectionLock)
			{
				ThreadHasParserLockForClose = false;
				_parserLock.Release();
			}
		}
	}

	internal void BreakConnection()
	{
		SqlConnection connection = base.Connection;
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.BreakConnection|RES|CPOOL> {0}, Breaking connection.", base.ObjectID);
		DoomThisConnection();
		connection?.Close();
	}

	internal void OnEnvChange(SqlEnvChange rec)
	{
		switch (rec.type)
		{
		case 1:
			if (!_fConnectionOpen && _recoverySessionData == null)
			{
				_originalDatabase = rec.newValue;
			}
			base.CurrentDatabase = rec.newValue;
			break;
		case 2:
			if (!_fConnectionOpen && _recoverySessionData == null)
			{
				_originalLanguage = rec.newValue;
			}
			_currentLanguage = rec.newValue;
			break;
		case 4:
			_currentPacketSize = int.Parse(rec.newValue, CultureInfo.InvariantCulture);
			break;
		case 7:
			if (_currentSessionData != null)
			{
				_currentSessionData._collation = rec.newCollation;
			}
			break;
		case 13:
			_currentFailoverPartner = rec.newValue;
			break;
		case 15:
			base.PromotedDTCToken = rec.newBinValue;
			break;
		case 18:
			if (_currentSessionData != null)
			{
				_currentSessionData.Reset();
			}
			break;
		case 19:
			_instanceName = rec.newValue;
			break;
		case 20:
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnEnvChange|ADV> {0}, Received routing info", base.ObjectID);
			if (string.IsNullOrEmpty(rec.newRoutingInfo.ServerName) || rec.newRoutingInfo.Protocol != 0 || rec.newRoutingInfo.Port == 0)
			{
				throw SQL.ROR_InvalidRoutingInfo(this);
			}
			_routingInfo = rec.newRoutingInfo;
			break;
		case 3:
		case 5:
		case 6:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 14:
		case 16:
		case 17:
			break;
		}
	}

	internal void OnLoginAck(SqlLoginAck rec)
	{
		_loginAck = rec;
		if (_recoverySessionData != null && _recoverySessionData._tdsVersion != rec.tdsVersion)
		{
			throw SQL.CR_TDSVersionNotPreserved(this);
		}
		if (_currentSessionData != null)
		{
			_currentSessionData._tdsVersion = rec.tdsVersion;
		}
	}

	internal void OnFedAuthInfo(SqlFedAuthInfo fedAuthInfo)
	{
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFedAuthInfo> {0}, Generating federated authentication token", base.ObjectID);
		DbConnectionPoolAuthenticationContext value = null;
		bool flag = false;
		bool flag2 = false;
		if (_dbConnectionPool != null)
		{
			_dbConnectionPoolAuthenticationContextKey = new DbConnectionPoolAuthenticationContextKey(fedAuthInfo.stsurl, fedAuthInfo.spn);
			if (_dbConnectionPool.AuthenticationContexts.TryGetValue(_dbConnectionPoolAuthenticationContextKey, out value))
			{
				TimeSpan timeSpan = value.ExpirationTime.Subtract(DateTime.UtcNow);
				if (timeSpan <= _dbAuthenticationContextUnLockedRefreshTimeSpan)
				{
					if (SqlClientEventSource.Log.IsTraceEnabled())
					{
						SqlClientEventSource.Log.TraceEvent("<sc.SqlInternalConnectionTds.OnFedAuthInfo> {0}, The expiration time is less than 10 mins, so trying to get new access token regardless of if an other thread is also trying to update it.The expiration time is {1}. Current Time is {2}.", base.ObjectID, value.ExpirationTime.ToLongTimeString(), DateTime.UtcNow.ToLongTimeString());
					}
					flag = true;
				}
				else if (timeSpan <= _dbAuthenticationContextLockedRefreshTimeSpan)
				{
					if (SqlClientEventSource.Log.IsAdvancedTraceOn())
					{
						SqlClientEventSource.Log.AdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFedAuthInfo|ADV> {0}, The authentication context needs a refresh.The expiration time is {1}. Current Time is {2}.", base.ObjectID, value.ExpirationTime.ToLongTimeString(), DateTime.UtcNow.ToLongTimeString());
					}
					flag2 = TryGetFedAuthTokenLocked(fedAuthInfo, value, out _fedAuthToken);
					if (flag2)
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFedAuthInfo> {0}, The attempt to get a new access token succeeded under the locked mode.", base.ObjectID);
					}
				}
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFedAuthInfo> {0}, Found an authentication context in the cache that does not need a refresh at this time. Re-using the cached token.", base.ObjectID);
			}
		}
		if (value == null || flag)
		{
			_fedAuthToken = GetFedAuthToken(fedAuthInfo);
			if (_dbConnectionPool == null)
			{
			}
		}
		else if (!flag2)
		{
			_fedAuthToken = new SqlFedAuthToken();
			_fedAuthToken.accessToken = value.AccessToken;
			_fedAuthToken.expirationFileTime = value.ExpirationTime.ToFileTime();
		}
		_parser.SendFedAuthToken(_fedAuthToken);
	}

	internal bool TryGetFedAuthTokenLocked(SqlFedAuthInfo fedAuthInfo, DbConnectionPoolAuthenticationContext dbConnectionPoolAuthenticationContext, out SqlFedAuthToken fedAuthToken)
	{
		fedAuthToken = null;
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (dbConnectionPoolAuthenticationContext.LockToUpdate())
			{
				if (SqlClientEventSource.Log.IsTraceEnabled())
				{
					SqlClientEventSource.Log.TraceEvent("<sc.SqlInternalConnectionTds.TryGetFedAuthTokenLocked> {0}, Acquired the lock to update the authentication context.The expiration time is {1}. Current Time is {2}.", base.ObjectID, dbConnectionPoolAuthenticationContext.ExpirationTime.ToLongTimeString(), DateTime.UtcNow.ToLongTimeString());
				}
				flag = true;
			}
			else
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.TryGetFedAuthTokenLocked> {0}, Refreshing the context is already in progress by another thread.", base.ObjectID);
			}
			if (flag)
			{
				fedAuthToken = GetFedAuthToken(fedAuthInfo);
			}
		}
		finally
		{
			if (flag)
			{
				dbConnectionPoolAuthenticationContext.ReleaseLockToUpdate();
			}
		}
		return flag;
	}

	internal SqlFedAuthToken GetFedAuthToken(SqlFedAuthInfo fedAuthInfo)
	{
		int num = 100;
		int num2 = 0;
		SqlFedAuthToken sqlFedAuthToken = new SqlFedAuthToken();
		string text = null;
		SqlAuthenticationProvider provider = _sqlAuthenticationProviderManager.GetProvider(base.ConnectionOptions.Authentication);
		if (provider == null)
		{
			throw SQL.CannotFindAuthProvider(base.ConnectionOptions.Authentication.ToString());
		}
		while (num2 <= 1 && num <= _timeout.MillisecondsRemaining)
		{
			num2++;
			try
			{
				SqlAuthenticationParameters.Builder builder = new SqlAuthenticationParameters.Builder(base.ConnectionOptions.Authentication, fedAuthInfo.spn, fedAuthInfo.stsurl, base.ConnectionOptions.DataSource, base.ConnectionOptions.InitialCatalog).WithConnectionId(_clientConnectionId).WithConnectionTimeout(base.ConnectionOptions.ConnectTimeout);
				switch (base.ConnectionOptions.Authentication)
				{
				case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
					text = "NT Authority\\Anonymous Logon";
					if (_activeDirectoryAuthTimeoutRetryHelper.State == ActiveDirectoryAuthenticationTimeoutRetryState.Retrying)
					{
						sqlFedAuthToken = _activeDirectoryAuthTimeoutRetryHelper.CachedToken;
						break;
					}
					sqlFedAuthToken = provider.AcquireTokenAsync(builder).Result.ToSqlFedAuthToken();
					_activeDirectoryAuthTimeoutRetryHelper.CachedToken = sqlFedAuthToken;
					break;
				case SqlAuthenticationMethod.ActiveDirectoryInteractive:
				case SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow:
				case SqlAuthenticationMethod.ActiveDirectoryManagedIdentity:
				case SqlAuthenticationMethod.ActiveDirectoryMSI:
				case SqlAuthenticationMethod.ActiveDirectoryDefault:
					if (_activeDirectoryAuthTimeoutRetryHelper.State == ActiveDirectoryAuthenticationTimeoutRetryState.Retrying)
					{
						sqlFedAuthToken = _activeDirectoryAuthTimeoutRetryHelper.CachedToken;
						break;
					}
					builder.WithUserId(base.ConnectionOptions.UserID);
					sqlFedAuthToken = provider.AcquireTokenAsync(builder).Result.ToSqlFedAuthToken();
					_activeDirectoryAuthTimeoutRetryHelper.CachedToken = sqlFedAuthToken;
					break;
				case SqlAuthenticationMethod.ActiveDirectoryPassword:
				case SqlAuthenticationMethod.ActiveDirectoryServicePrincipal:
					if (_activeDirectoryAuthTimeoutRetryHelper.State == ActiveDirectoryAuthenticationTimeoutRetryState.Retrying)
					{
						sqlFedAuthToken = _activeDirectoryAuthTimeoutRetryHelper.CachedToken;
						break;
					}
					if (_credential != null)
					{
						text = _credential.UserId;
						builder.WithUserId(text).WithPassword(_credential.Password);
						sqlFedAuthToken = provider.AcquireTokenAsync(builder).Result.ToSqlFedAuthToken();
					}
					else
					{
						text = base.ConnectionOptions.UserID;
						builder.WithUserId(text).WithPassword(base.ConnectionOptions.Password);
						sqlFedAuthToken = provider.AcquireTokenAsync(builder).Result.ToSqlFedAuthToken();
					}
					_activeDirectoryAuthTimeoutRetryHelper.CachedToken = sqlFedAuthToken;
					break;
				default:
					throw SQL.UnsupportedAuthenticationSpecified(base.ConnectionOptions.Authentication);
				}
			}
			catch (MsalServiceException ex)
			{
				if (429 == ex.StatusCode)
				{
					RetryConditionHeaderValue retryAfter = ex.Headers.RetryAfter;
					if (retryAfter.Delta.HasValue)
					{
						num = retryAfter.Delta.Value.Milliseconds;
					}
					else if (retryAfter.Date.HasValue)
					{
						num = Convert.ToInt32(retryAfter.Date.Value.Offset.TotalMilliseconds);
					}
					if (num < _timeout.MillisecondsRemaining)
					{
						Thread.Sleep(num);
						continue;
					}
					break;
				}
				continue;
			}
			catch (MsalException ex2)
			{
				if ("unknown_error" != ex2.ErrorCode || _timeout.IsExpired || _timeout.MillisecondsRemaining <= num)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.GetFedAuthToken.MSALException error:> {0}", ex2.ErrorCode);
					SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
					sqlErrorCollection.Add(new SqlError(0, 0, 11, base.ConnectionOptions.DataSource, StringsHelper.GetString(Strings.SQL_MSALFailure, text, base.ConnectionOptions.Authentication.ToString("G")), "AcquireToken", 0));
					string @string = StringsHelper.GetString(Strings.SQL_MSALInnerException, ex2.ErrorCode);
					sqlErrorCollection.Add(new SqlError(0, 0, 11, base.ConnectionOptions.DataSource, @string, "AcquireToken", 0));
					if (!string.IsNullOrEmpty(ex2.Message))
					{
						sqlErrorCollection.Add(new SqlError(0, 0, 11, base.ConnectionOptions.DataSource, ex2.Message, "AcquireToken", 0));
					}
					SqlException ex3 = SqlException.CreateException(sqlErrorCollection, "", this);
					throw ex3;
				}
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.GetFedAuthToken|ADV> {0}, sleeping {1}[Milliseconds]", base.ObjectID, num);
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.GetFedAuthToken|ADV> {0}, remaining {1}[Milliseconds]", base.ObjectID, _timeout.MillisecondsRemaining);
				Thread.Sleep(num);
				num *= 2;
				continue;
			}
			break;
		}
		if (_dbConnectionPool != null)
		{
			DateTime expirationTime = DateTime.FromFileTimeUtc(sqlFedAuthToken.expirationFileTime);
			_newDbConnectionPoolAuthenticationContext = new DbConnectionPoolAuthenticationContext(sqlFedAuthToken.accessToken, expirationTime);
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.GetFedAuthToken> {0}, Finished generating federated authentication token.", base.ObjectID);
		return sqlFedAuthToken;
	}

	internal void OnFeatureExtAck(int featureId, byte[] data)
	{
		if (_routingInfo != null && 11 != featureId)
		{
			return;
		}
		switch (featureId)
		{
		case 1:
		{
			if (!_sessionRecoveryRequested)
			{
				throw SQL.ParsingErrorFeatureId(ParsingErrorState.UnrequestedFeatureAckReceived, featureId);
			}
			_sessionRecoveryAcknowledged = true;
			int num = 0;
			while (num < data.Length)
			{
				byte b3 = data[num];
				num++;
				byte b4 = data[num];
				num++;
				int num2;
				if (b4 == byte.MaxValue)
				{
					num2 = BitConverter.ToInt32(data, num);
					num += 4;
				}
				else
				{
					num2 = b4;
				}
				byte[] array = new byte[num2];
				Buffer.BlockCopy(data, num, array, 0, num2);
				num += num2;
				if (_recoverySessionData == null)
				{
					_currentSessionData._initialState[b3] = array;
					continue;
				}
				_currentSessionData._delta[b3] = new SessionStateRecord
				{
					_data = array,
					_dataLength = num2,
					_recoverable = true,
					_version = 0u
				};
				_currentSessionData._deltaDirty = true;
			}
			break;
		}
		case 2:
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ADV> {0}, Received feature extension acknowledgement for federated authentication", base.ObjectID);
			if (!_federatedAuthenticationRequested)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Did not request federated authentication", base.ObjectID);
				throw SQL.ParsingErrorFeatureId(ParsingErrorState.UnrequestedFeatureAckReceived, featureId);
			}
			TdsEnums.FedAuthLibrary libraryType = _fedAuthFeatureExtensionData.libraryType;
			if (libraryType - 1 <= TdsEnums.FedAuthLibrary.SecurityToken)
			{
				if (data.Length != 0)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Federated authentication feature extension ack for MSAL and Security Token includes extra data", base.ObjectID);
					throw SQL.ParsingError(ParsingErrorState.FedAuthFeatureAckContainsExtraData);
				}
				_federatedAuthenticationAcknowledged = true;
				if (_newDbConnectionPoolAuthenticationContext != null)
				{
					DbConnectionPoolAuthenticationContext dbConnectionPoolAuthenticationContext = _dbConnectionPool.AuthenticationContexts.AddOrUpdate(_dbConnectionPoolAuthenticationContextKey, _newDbConnectionPoolAuthenticationContext, (DbConnectionPoolAuthenticationContextKey key, DbConnectionPoolAuthenticationContext oldValue) => DbConnectionPoolAuthenticationContext.ChooseAuthenticationContextToUpdate(oldValue, _newDbConnectionPoolAuthenticationContext));
				}
				break;
			}
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Attempting to use unknown federated authentication library", base.ObjectID);
			throw SQL.ParsingErrorLibraryType(ParsingErrorState.FedAuthFeatureAckUnknownLibraryType, (int)_fedAuthFeatureExtensionData.libraryType);
		}
		case 4:
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ADV> {0}, Received feature extension acknowledgement for TCE", base.ObjectID);
			if (data.Length < 1)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Unknown version number for TCE", base.ObjectID);
				throw SQL.ParsingError(ParsingErrorState.TceUnknownVersion);
			}
			byte b5 = data[0];
			if (b5 == 0 || b5 > 3)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Invalid version number for TCE", base.ObjectID);
				throw SQL.ParsingErrorValue(ParsingErrorState.TceInvalidVersion, b5);
			}
			_tceVersionSupported = b5;
			_parser.IsColumnEncryptionSupported = true;
			_parser.TceVersionSupported = _tceVersionSupported;
			_parser.AreEnclaveRetriesSupported = _tceVersionSupported == 3;
			if (data.Length > 1)
			{
				_parser.EnclaveType = Encoding.Unicode.GetString(data, 2, data.Length - 2);
			}
			break;
		}
		case 5:
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ADV> {0}, Received feature extension acknowledgement for GlobalTransactions", base.ObjectID);
			if (data.Length < 1)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Unknown version number for GlobalTransactions", base.ObjectID);
				throw SQL.ParsingError(ParsingErrorState.CorruptedTdsStream);
			}
			base.IsGlobalTransaction = true;
			if (1 == data[0])
			{
				base.IsGlobalTransactionsEnabledForServer = true;
			}
			break;
		case 8:
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ADV> {0}, Received feature extension acknowledgement for AzureSQLSupport", base.ObjectID);
			if (data.Length < 1)
			{
				throw SQL.ParsingError(ParsingErrorState.CorruptedTdsStream);
			}
			base.IsAzureSQLConnection = true;
			if ((data[0] & 1) == 1 && SqlClientEventSource.Log.IsTraceEnabled())
			{
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ADV> {0}, FailoverPartner enabled with Readonly intent for AzureSQL DB", base.ObjectID);
			}
			break;
		case 9:
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ADV> {0}, Received feature extension acknowledgement for DATACLASSIFICATION", base.ObjectID);
			if (data.Length < 1)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Unknown token for DATACLASSIFICATION", base.ObjectID);
				throw SQL.ParsingError(ParsingErrorState.CorruptedTdsStream);
			}
			byte b = data[0];
			if (b == 0 || b > 2)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Invalid version number for DATACLASSIFICATION", base.ObjectID);
				throw SQL.ParsingErrorValue(ParsingErrorState.DataClassificationInvalidVersion, b);
			}
			if (data.Length != 2)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Unknown token for DATACLASSIFICATION", base.ObjectID);
				throw SQL.ParsingError(ParsingErrorState.CorruptedTdsStream);
			}
			byte b2 = data[1];
			_parser.DataClassificationVersion = ((b2 != 0) ? b : 0);
			break;
		}
		case 10:
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ADV> {0}, Received feature extension acknowledgement for UTF8 support", base.ObjectID);
			if (data.Length < 1)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Unknown value for UTF8 support", base.ObjectID);
				throw SQL.ParsingError(ParsingErrorState.CorruptedTdsStream);
			}
			break;
		case 11:
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ADV> {0}, Received feature extension acknowledgement for SQLDNSCACHING", base.ObjectID);
			if (data.Length < 1)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionTds.OnFeatureExtAck|ERR> {0}, Unknown token for SQLDNSCACHING", base.ObjectID);
				throw SQL.ParsingError(ParsingErrorState.CorruptedTdsStream);
			}
			if (1 == data[0])
			{
				IsSQLDNSCachingSupported = true;
				_cleanSQLDNSCaching = false;
				if (_routingInfo != null)
				{
					IsDNSCachingBeforeRedirectSupported = true;
				}
			}
			else
			{
				IsSQLDNSCachingSupported = false;
				_cleanSQLDNSCaching = true;
			}
			break;
		default:
			throw SQL.ParsingErrorFeatureId(ParsingErrorState.UnknownFeatureAck, featureId);
		}
	}

	internal override bool TryReplaceConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		return TryOpenConnectionInternal(outerConnection, connectionFactory, retry, userOptions);
	}
}
