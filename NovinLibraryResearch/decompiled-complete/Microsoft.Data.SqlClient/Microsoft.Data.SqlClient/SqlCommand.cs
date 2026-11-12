using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Common;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

[DefaultEvent("RecordsAffected")]
[ToolboxItem(true)]
[Designer("Microsoft.VSDesigner.Data.VS.SqlCommandDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[DesignerCategory("")]
public sealed class SqlCommand : DbCommand, ICloneable
{
	private enum EXECTYPE
	{
		UNPREPARED,
		PREPAREPENDING,
		PREPARED
	}

	private sealed class CachedAsyncState
	{
		private int _cachedAsyncCloseCount = -1;

		private TaskCompletionSource<object> _cachedAsyncResult;

		private SqlConnection _cachedAsyncConnection;

		private SqlDataReader _cachedAsyncReader;

		private RunBehavior _cachedRunBehavior = RunBehavior.ReturnImmediately;

		private string _cachedSetOptions;

		private string _cachedEndMethod;

		internal SqlDataReader CachedAsyncReader => _cachedAsyncReader;

		internal RunBehavior CachedRunBehavior => _cachedRunBehavior;

		internal string CachedSetOptions => _cachedSetOptions;

		internal bool PendingAsyncOperation => _cachedAsyncResult != null;

		internal string EndMethodName => _cachedEndMethod;

		internal CachedAsyncState()
		{
		}

		internal bool IsActiveConnectionValid(SqlConnection activeConnection)
		{
			if (_cachedAsyncConnection == activeConnection)
			{
				return _cachedAsyncCloseCount == activeConnection.CloseCount;
			}
			return false;
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void ResetAsyncState()
		{
			_cachedAsyncCloseCount = -1;
			_cachedAsyncResult = null;
			if (_cachedAsyncConnection != null)
			{
				_cachedAsyncConnection.AsyncCommandInProgress = false;
				_cachedAsyncConnection = null;
			}
			_cachedAsyncReader = null;
			_cachedRunBehavior = RunBehavior.ReturnImmediately;
			_cachedSetOptions = null;
			_cachedEndMethod = null;
		}

		internal void SetActiveConnectionAndResult(TaskCompletionSource<object> completion, string endMethod, SqlConnection activeConnection)
		{
			TdsParser parser = activeConnection.Parser;
			if (parser == null || parser.State == TdsParserState.Closed || parser.State == TdsParserState.Broken)
			{
				throw ADP.ClosedConnectionError();
			}
			_cachedAsyncCloseCount = activeConnection.CloseCount;
			_cachedAsyncResult = completion;
			if (activeConnection != null && !parser.MARSOn && activeConnection.AsyncCommandInProgress)
			{
				throw SQL.MARSUnsupportedOnConnection();
			}
			_cachedAsyncConnection = activeConnection;
			_cachedAsyncConnection.AsyncCommandInProgress = true;
			_cachedEndMethod = endMethod;
		}

		internal void SetAsyncReaderState(SqlDataReader ds, RunBehavior runBehavior, string optionSettings)
		{
			_cachedAsyncReader = ds;
			_cachedRunBehavior = runBehavior;
			_cachedSetOptions = optionSettings;
		}
	}

	private sealed class CommandEventSink : SmiEventSink_Default
	{
		private SqlCommand _command;

		internal CommandEventSink(SqlCommand command)
		{
			_command = command;
		}

		internal override void StatementCompleted(int rowsAffected)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlCommand.CommandEventSink.StatementCompleted|ADV> {0}, rowsAffected={1}.", _command.ObjectID, rowsAffected);
			_command.InternalRecordsAffected = rowsAffected;
		}

		internal override void BatchCompleted()
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlCommand.CommandEventSink.BatchCompleted|ADV> {0}.", _command.ObjectID);
		}

		internal override void ParametersAvailable(SmiParameterMetaData[] metaData, ITypedGettersV3 parameterValues)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlCommand.CommandEventSink.ParametersAvailable|ADV> {0} metaData.Length={1}.", _command.ObjectID, metaData?.Length);
			if (SqlClientEventSource.Log.IsAdvancedTraceOn() && metaData != null)
			{
				for (int i = 0; i < metaData.Length; i++)
				{
					SqlClientEventSource.Log.AdvancedTraceEvent("<sc.SqlCommand.CommandEventSink.ParametersAvailable|ADV> {0}, metaData[{1}] is {2}{3}", _command.ObjectID, i, metaData[i].GetType(), metaData[i].TraceString());
				}
			}
			_command.OnParametersAvailableSmi(metaData, parameterValues);
		}

		internal override void ParameterAvailable(SmiParameterMetaData metaData, SmiTypedGetterSetter parameterValues, int ordinal)
		{
			if (SqlClientEventSource.Log.IsAdvancedTraceOn())
			{
				SqlClientEventSource.Log.AdvancedTraceEvent("<sc.SqlCommand.CommandEventSink.ParameterAvailable|ADV> {0}, metaData[{1}] is {2}{ 3}", _command.ObjectID, ordinal, metaData?.GetType(), metaData?.TraceString());
			}
			_command.OnParameterAvailableSmi(metaData, parameterValues, ordinal);
		}
	}

	private enum ProcParamsColIndex
	{
		ParameterName,
		ParameterType,
		DataType,
		ManagedDataType,
		CharacterMaximumLength,
		NumericPrecision,
		NumericScale,
		TypeCatalogName,
		TypeSchemaName,
		TypeName,
		XmlSchemaCollectionCatalogName,
		XmlSchemaCollectionSchemaName,
		XmlSchemaCollectionName,
		UdtTypeName,
		DateTimeScale
	}

	private static int _objectTypeCount;

	internal readonly int ObjectID = Interlocked.Increment(ref _objectTypeCount);

	private string _commandText;

	private CommandType _commandType;

	private int? _commandTimeout;

	private UpdateRowSource _updatedRowSource = UpdateRowSource.Both;

	private bool _designTimeInvisible;

	private bool _wasBatchModeColumnEncryptionSettingSetOnce;

	private SqlCommandColumnEncryptionSetting _columnEncryptionSetting;

	internal SqlDependency _sqlDep;

	private bool _inPrepare;

	private int _prepareHandle = -1;

	private bool _hiddenPrepare;

	private int _preparedConnectionCloseCount = -1;

	private int _preparedConnectionReconnectCount = -1;

	private SqlParameterCollection _parameters;

	private SqlConnection _activeConnection;

	private bool _dirty;

	private EXECTYPE _execType;

	private _SqlRPC[] _rpcArrayOf1;

	private _SqlRPC _rpcForEncryption;

	private _SqlMetaDataSet _cachedMetaData;

	internal EnclavePackage enclavePackage;

	private SqlEnclaveAttestationParameters enclaveAttestationParameters;

	private byte[] customData;

	private int customDataLength;

	private TaskCompletionSource<object> _reconnectionCompletionSource;

	internal ConcurrentDictionary<int, SqlTceCipherInfoEntry> keysToBeSentToEnclave;

	internal bool requiresEnclaveComputations;

	private IReadOnlyDictionary<string, SqlColumnEncryptionKeyStoreProvider> _customColumnEncryptionKeyStoreProviders;

	private CachedAsyncState _cachedAsyncState;

	internal int _rowsAffected = -1;

	private int _rowsAffectedBySpDescribeParameterEncryption = -1;

	private SqlNotificationRequest _notification;

	private bool _notificationAutoEnlist = true;

	private SqlTransaction _transaction;

	private StatementCompletedEventHandler _statementCompletedEventHandler;

	private TdsParserStateObject _stateObj;

	private volatile bool _pendingCancel;

	private bool _batchRPCMode;

	private List<_SqlRPC> _RPCList;

	private _SqlRPC[] _SqlRPCBatchArray;

	private _SqlRPC[] _sqlRPCParameterEncryptionReqArray;

	private List<SqlParameterCollection> _parameterCollectionList;

	private int _currentlyExecutingBatch;

	private SqlRetryLogicBaseProvider _retryLogicProvider;

	private int _currentlyExecutingDescribeParameterEncryptionRPC;

	private volatile bool _internalEndExecuteInitiated;

	private SmiContext _smiRequestContext;

	private CommandEventSink _smiEventSink;

	private SmiEventSink_DeferedProcessing _outParamEventSink;

	internal static readonly string[] PreKatmaiProcParamsNames = new string[15]
	{
		"PARAMETER_NAME", "PARAMETER_TYPE", "DATA_TYPE", null, "CHARACTER_MAXIMUM_LENGTH", "NUMERIC_PRECISION", "NUMERIC_SCALE", "UDT_CATALOG", "UDT_SCHEMA", "TYPE_NAME",
		"XML_CATALOGNAME", "XML_SCHEMANAME", "XML_SCHEMACOLLECTIONNAME", "UDT_NAME", null
	};

	internal static readonly string[] KatmaiProcParamsNames = new string[15]
	{
		"PARAMETER_NAME", "PARAMETER_TYPE", null, "MANAGED_DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH", "NUMERIC_PRECISION", "NUMERIC_SCALE", "TYPE_CATALOG_NAME", "TYPE_SCHEMA_NAME", "TYPE_NAME",
		"XML_CATALOGNAME", "XML_SCHEMANAME", "XML_SCHEMACOLLECTIONNAME", null, "SS_DATETIME_PRECISION"
	};

	internal bool InPrepare => _inPrepare;

	internal bool IsColumnEncryptionEnabled
	{
		get
		{
			if ((_columnEncryptionSetting == SqlCommandColumnEncryptionSetting.Enabled || (_columnEncryptionSetting == SqlCommandColumnEncryptionSetting.UseConnectionSetting && _activeConnection.IsColumnEncryptionSettingEnabled)) && _activeConnection.Parser != null)
			{
				return _activeConnection.Parser.IsColumnEncryptionSupported;
			}
			return false;
		}
	}

	internal bool ShouldUseEnclaveBasedWorkflow
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(_activeConnection.EnclaveAttestationUrl))
			{
				return IsColumnEncryptionEnabled;
			}
			return false;
		}
	}

	private bool ShouldCacheEncryptionMetadata
	{
		get
		{
			if (requiresEnclaveComputations)
			{
				return _activeConnection.Parser.AreEnclaveRetriesSupported;
			}
			return true;
		}
	}

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

	private CachedAsyncState cachedAsyncState
	{
		get
		{
			if (_cachedAsyncState == null)
			{
				_cachedAsyncState = new CachedAsyncState();
			}
			return _cachedAsyncState;
		}
	}

	internal bool IsDescribeParameterEncryptionRPCCurrentlyInProgress { get; private set; }

	internal bool CachingQueryMetadataPostponed { get; set; }

	private CommandEventSink EventSink
	{
		get
		{
			if (_smiEventSink == null)
			{
				_smiEventSink = new CommandEventSink(this);
			}
			_smiEventSink.Parent = InternalSmiConnection.CurrentEventSink;
			return _smiEventSink;
		}
	}

	private SmiEventSink_DeferedProcessing OutParamEventSink
	{
		get
		{
			if (_outParamEventSink == null)
			{
				_outParamEventSink = new SmiEventSink_DeferedProcessing(EventSink);
			}
			else
			{
				_outParamEventSink.Parent = EventSink;
			}
			return _outParamEventSink;
		}
	}

	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ResCategory("Data")]
	[ResDescription("Connection used by the command.")]
	public new SqlConnection Connection
	{
		get
		{
			return _activeConnection;
		}
		set
		{
			if (_activeConnection != value && _activeConnection != null && _cachedAsyncState != null && _cachedAsyncState.PendingAsyncOperation)
			{
				throw SQL.CannotModifyPropertyAsyncOperationInProgress("Connection");
			}
			if (_transaction != null && _transaction.Connection == null)
			{
				_transaction = null;
			}
			_smiRequestContext = null;
			if (IsPrepared && _activeConnection != value && _activeConnection != null)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Unprepare();
				}
				catch (OutOfMemoryException)
				{
					_activeConnection.InnerConnection.DoomThisConnection();
					throw;
				}
				catch (StackOverflowException)
				{
					_activeConnection.InnerConnection.DoomThisConnection();
					throw;
				}
				catch (ThreadAbortException)
				{
					_activeConnection.InnerConnection.DoomThisConnection();
					throw;
				}
				catch (Exception)
				{
				}
				finally
				{
					_prepareHandle = -1;
					_execType = EXECTYPE.UNPREPARED;
				}
			}
			_activeConnection = value;
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.set_Connection|API> {0}, {1}", ObjectID, value?.ObjectID);
		}
	}

	protected override DbConnection DbConnection
	{
		get
		{
			return Connection;
		}
		set
		{
			Connection = (SqlConnection)value;
		}
	}

	private SqlInternalConnectionSmi InternalSmiConnection => (SqlInternalConnectionSmi)_activeConnection.InnerConnection;

	private SqlInternalConnectionTds InternalTdsConnection => (SqlInternalConnectionTds)_activeConnection.InnerConnection;

	private bool IsShiloh
	{
		get
		{
			if (_activeConnection == null)
			{
				return false;
			}
			return _activeConnection.IsShiloh;
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
				_retryLogicProvider = SqlConfigurableRetryLogicManager.CommandProvider;
			}
			return _retryLogicProvider;
		}
		set
		{
			_retryLogicProvider = value;
		}
	}

	[DefaultValue(true)]
	[ResCategory("Notification")]
	[ResDescription("Automatic enlistment in notifications used by Microsoft SQL Server.")]
	public bool NotificationAutoEnlist
	{
		get
		{
			return _notificationAutoEnlist;
		}
		set
		{
			_notificationAutoEnlist = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResCategory("Notification")]
	[ResDescription("Notification values used by Microsoft SQL Server.")]
	public SqlNotificationRequest Notification
	{
		get
		{
			return _notification;
		}
		set
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.set_Notification|API> {0}", ObjectID);
			_sqlDep = null;
			_notification = value;
		}
	}

	internal SqlStatistics Statistics
	{
		get
		{
			if (_activeConnection != null && _activeConnection.StatisticsEnabled)
			{
				return _activeConnection.Statistics;
			}
			return null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResDescription("The transaction used by the command.")]
	public new SqlTransaction Transaction
	{
		get
		{
			if (_transaction != null && _transaction.Connection == null)
			{
				_transaction = null;
			}
			return _transaction;
		}
		set
		{
			if (_transaction != value && _activeConnection != null && cachedAsyncState.PendingAsyncOperation)
			{
				throw SQL.CannotModifyPropertyAsyncOperationInProgress("Transaction");
			}
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.set_Transaction|API> {0}", ObjectID);
			_transaction = value;
		}
	}

	protected override DbTransaction DbTransaction
	{
		get
		{
			return Transaction;
		}
		set
		{
			Transaction = (SqlTransaction)value;
		}
	}

	[DefaultValue("")]
	[Editor("Microsoft.VSDesigner.Data.SQL.Design.SqlCommandTextEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[RefreshProperties(RefreshProperties.All)]
	[ResCategory("Data")]
	[ResDescription("Command text to execute.")]
	public override string CommandText
	{
		get
		{
			string commandText = _commandText;
			if (commandText == null)
			{
				return ADP.StrEmpty;
			}
			return commandText;
		}
		set
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.set_CommandText|API> {0}, String Value = '{1}'", ObjectID, value);
			if (_commandText != value)
			{
				PropertyChanging();
				_commandText = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ResCategory("Data")]
	[ResDescription("Column encryption setting for the command. Overrides the connection level default.")]
	public SqlCommandColumnEncryptionSetting ColumnEncryptionSetting => _columnEncryptionSetting;

	[ResCategory("Data")]
	[ResDescription("Time to wait for command to execute.")]
	public override int CommandTimeout
	{
		get
		{
			return _commandTimeout ?? DefaultCommandTimeout;
		}
		set
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.set_CommandTimeout|API> {0}, {1}", ObjectID, value);
			if (value < 0)
			{
				throw ADP.InvalidCommandTimeout(value);
			}
			if (value != _commandTimeout)
			{
				PropertyChanging();
				_commandTimeout = value;
			}
		}
	}

	private int DefaultCommandTimeout => _activeConnection?.CommandTimeout ?? 30;

	[DefaultValue(CommandType.Text)]
	[RefreshProperties(RefreshProperties.All)]
	[ResCategory("Data")]
	[ResDescription("How to interpret the CommandText.")]
	public override CommandType CommandType
	{
		get
		{
			CommandType commandType = _commandType;
			if (commandType == (CommandType)0)
			{
				return CommandType.Text;
			}
			return commandType;
		}
		set
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.set_CommandType|API> {0}, {1}{2}", ObjectID, (int)value, _commandType);
			if (_commandType != value)
			{
				switch (value)
				{
				case CommandType.Text:
				case CommandType.StoredProcedure:
					PropertyChanging();
					_commandType = value;
					break;
				case CommandType.TableDirect:
					throw SQL.NotSupportedCommandType(value);
				default:
					throw ADP.InvalidCommandType(value);
				}
			}
		}
	}

	[DefaultValue(true)]
	[DesignOnly(true)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool DesignTimeVisible
	{
		get
		{
			return !_designTimeInvisible;
		}
		set
		{
			_designTimeInvisible = !value;
			TypeDescriptor.Refresh(this);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[ResCategory("Data")]
	[ResDescription("The parameters collection.")]
	public new SqlParameterCollection Parameters
	{
		get
		{
			if (_parameters == null)
			{
				_parameters = new SqlParameterCollection();
			}
			return _parameters;
		}
	}

	protected override DbParameterCollection DbParameterCollection => Parameters;

	[DefaultValue(UpdateRowSource.Both)]
	[ResCategory("Update")]
	[ResDescription("When used by a DataAdapter.Update, how command results are applied to the current DataRow.")]
	public override UpdateRowSource UpdatedRowSource
	{
		get
		{
			return _updatedRowSource;
		}
		set
		{
			if ((uint)value <= 3u)
			{
				_updatedRowSource = value;
				return;
			}
			throw ADP.InvalidUpdateRowSource(value);
		}
	}

	internal _SqlMetaDataSet MetaData => _cachedMetaData;

	internal TdsParserStateObject StateObject => _stateObj;

	private bool IsPrepared => _execType != EXECTYPE.UNPREPARED;

	private bool IsUserPrepared
	{
		get
		{
			if (IsPrepared && !_hiddenPrepare)
			{
				return !IsDirty;
			}
			return false;
		}
	}

	internal bool IsDirty
	{
		get
		{
			SqlConnection activeConnection = _activeConnection;
			if (IsPrepared)
			{
				if (!_dirty && (_parameters == null || !_parameters.IsDirty))
				{
					if (activeConnection != null)
					{
						if (activeConnection.CloseCount == _preparedConnectionCloseCount)
						{
							return activeConnection.ReconnectCount != _preparedConnectionReconnectCount;
						}
						return true;
					}
					return false;
				}
				return true;
			}
			return false;
		}
		set
		{
			_dirty = value && IsPrepared;
			if (_parameters != null)
			{
				_parameters.IsDirty = _dirty;
			}
			_cachedMetaData = null;
		}
	}

	internal int RowsAffectedByDescribeParameterEncryption
	{
		get
		{
			return _rowsAffectedBySpDescribeParameterEncryption;
		}
		set
		{
			if (-1 == _rowsAffectedBySpDescribeParameterEncryption)
			{
				_rowsAffectedBySpDescribeParameterEncryption = value;
			}
			else if (0 < value)
			{
				_rowsAffectedBySpDescribeParameterEncryption += value;
			}
		}
	}

	internal int InternalRecordsAffected
	{
		get
		{
			return _rowsAffected;
		}
		set
		{
			if (-1 == _rowsAffected)
			{
				_rowsAffected = value;
			}
			else if (0 < value)
			{
				_rowsAffected += value;
			}
		}
	}

	internal bool BatchRPCMode
	{
		get
		{
			return _batchRPCMode;
		}
		set
		{
			_batchRPCMode = value;
			if (!_batchRPCMode)
			{
				ClearBatchCommand();
				return;
			}
			if (_RPCList == null)
			{
				_RPCList = new List<_SqlRPC>();
			}
			if (_parameterCollectionList == null)
			{
				_parameterCollectionList = new List<SqlParameterCollection>();
			}
		}
	}

	[ResCategory("StatementCompleted")]
	[ResDescription("When records are affected by a given statement by the execution of the command.")]
	public event StatementCompletedEventHandler StatementCompleted
	{
		add
		{
			_statementCompletedEventHandler = (StatementCompletedEventHandler)Delegate.Combine(_statementCompletedEventHandler, value);
		}
		remove
		{
			_statementCompletedEventHandler = (StatementCompletedEventHandler)Delegate.Remove(_statementCompletedEventHandler, value);
		}
	}

	public SqlCommand()
	{
		GC.SuppressFinalize(this);
	}

	public SqlCommand(string cmdText)
		: this()
	{
		CommandText = cmdText;
	}

	public SqlCommand(string cmdText, SqlConnection connection)
		: this()
	{
		CommandText = cmdText;
		Connection = connection;
	}

	public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction)
		: this()
	{
		CommandText = cmdText;
		Connection = connection;
		Transaction = transaction;
	}

	public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction, SqlCommandColumnEncryptionSetting columnEncryptionSetting)
		: this()
	{
		CommandText = cmdText;
		Connection = connection;
		Transaction = transaction;
		_columnEncryptionSetting = columnEncryptionSetting;
	}

	private SqlCommand(SqlCommand from)
		: this()
	{
		CommandText = from.CommandText;
		CommandTimeout = from.CommandTimeout;
		CommandType = from.CommandType;
		Connection = from.Connection;
		DesignTimeVisible = from.DesignTimeVisible;
		Transaction = from.Transaction;
		UpdatedRowSource = from.UpdatedRowSource;
		_columnEncryptionSetting = from.ColumnEncryptionSetting;
		SqlParameterCollection parameters = Parameters;
		foreach (object parameter in from.Parameters)
		{
			parameters.Add((parameter is ICloneable) ? (parameter as ICloneable).Clone() : parameter);
		}
	}

	public void ResetCommandTimeout()
	{
		if (30 != CommandTimeout)
		{
			PropertyChanging();
			_commandTimeout = DefaultCommandTimeout;
		}
	}

	internal void CancelIgnoreFailure()
	{
		try
		{
			Cancel();
		}
		catch (Exception)
		{
		}
	}

	internal void OnStatementCompleted(int recordCount)
	{
		if (0 > recordCount)
		{
			return;
		}
		StatementCompletedEventHandler statementCompletedEventHandler = _statementCompletedEventHandler;
		if (statementCompletedEventHandler == null)
		{
			return;
		}
		try
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.OnStatementCompleted|INFO> {0}, recordCount={1}", ObjectID, recordCount);
			statementCompletedEventHandler(this, new StatementCompletedEventArgs(recordCount));
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableOrSecurityExceptionType(e))
			{
				throw;
			}
			ADP.TraceExceptionWithoutRethrow(e);
		}
	}

	private void PropertyChanging()
	{
		IsDirty = true;
	}

	public override void Prepare()
	{
		SqlConnection.ExecutePermission.Demand();
		_pendingCancel = false;
		if (_activeConnection != null && _activeConnection.IsContextConnection)
		{
			return;
		}
		SqlStatistics sqlStatistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlCommand.Prepare|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.Prepare|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		sqlStatistics = SqlStatistics.StartTimer(Statistics);
		if ((IsPrepared && !IsDirty) || CommandType == CommandType.StoredProcedure || (CommandType.Text == CommandType && GetParameterCount(_parameters) == 0))
		{
			if (Statistics != null)
			{
				Statistics.SafeIncrement(ref Statistics._prepares);
			}
			_hiddenPrepare = false;
		}
		else
		{
			ValidateCommand("Prepare", async: false);
			bool flag = true;
			TdsParser target = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
				GetStateObject();
				if (_parameters != null)
				{
					int count = _parameters.Count;
					for (int i = 0; i < count; i++)
					{
						_parameters[i].Prepare(this);
					}
				}
				InternalPrepare();
			}
			catch (OutOfMemoryException e)
			{
				flag = false;
				_activeConnection.Abort(e);
				throw;
			}
			catch (StackOverflowException e2)
			{
				flag = false;
				_activeConnection.Abort(e2);
				throw;
			}
			catch (ThreadAbortException e3)
			{
				flag = false;
				_activeConnection.Abort(e3);
				SqlInternalConnection.BestEffortCleanup(target);
				throw;
			}
			catch (Exception e4)
			{
				flag = ADP.IsCatchableExceptionType(e4);
				throw;
			}
			finally
			{
				if (flag)
				{
					_hiddenPrepare = false;
					ReliablePutStateObject();
				}
			}
		}
		SqlStatistics.StopTimer(sqlStatistics);
		SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
	}

	private void InternalPrepare()
	{
		if (IsDirty)
		{
			Unprepare();
			IsDirty = false;
		}
		_execType = EXECTYPE.PREPAREPENDING;
		_preparedConnectionCloseCount = _activeConnection.CloseCount;
		_preparedConnectionReconnectCount = _activeConnection.ReconnectCount;
		if (Statistics != null)
		{
			Statistics.SafeIncrement(ref Statistics._prepares);
		}
	}

	internal void Unprepare()
	{
		if (!_activeConnection.IsContextConnection)
		{
			_execType = EXECTYPE.PREPAREPENDING;
			if (_activeConnection.CloseCount != _preparedConnectionCloseCount || _activeConnection.ReconnectCount != _preparedConnectionReconnectCount)
			{
				_prepareHandle = -1;
			}
			_cachedMetaData = null;
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.Prepare|INFO> {0}, Command unprepared.", ObjectID);
		}
	}

	public override void Cancel()
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlCommand.Cancel | API > {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.Cancel|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		SqlStatistics statistics = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			TaskCompletionSource<object> reconnectionCompletionSource = _reconnectionCompletionSource;
			if ((reconnectionCompletionSource != null && reconnectionCompletionSource.TrySetCanceled()) || _activeConnection == null || !(_activeConnection.InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds))
			{
				return;
			}
			lock (sqlInternalConnectionTds)
			{
				if (sqlInternalConnectionTds != _activeConnection.InnerConnection as SqlInternalConnectionTds)
				{
					return;
				}
				TdsParser parser = sqlInternalConnectionTds.Parser;
				if (parser == null)
				{
					return;
				}
				TdsParser target = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
					if (!_pendingCancel)
					{
						_pendingCancel = true;
						TdsParserStateObject stateObj = _stateObj;
						if (stateObj != null)
						{
							stateObj.Cancel(ObjectID);
						}
						else
						{
							sqlInternalConnectionTds.FindLiveReader(this)?.Cancel(ObjectID);
						}
					}
				}
				catch (OutOfMemoryException e)
				{
					_activeConnection.Abort(e);
					throw;
				}
				catch (StackOverflowException e2)
				{
					_activeConnection.Abort(e2);
					throw;
				}
				catch (ThreadAbortException e3)
				{
					_activeConnection.Abort(e3);
					SqlInternalConnection.BestEffortCleanup(target);
					throw;
				}
			}
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	public new SqlParameter CreateParameter()
	{
		return new SqlParameter();
	}

	protected override DbParameter CreateDbParameter()
	{
		return CreateParameter();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_cachedMetaData = null;
		}
		base.Dispose(disposing);
	}

	private SqlDataReader RunExecuteReaderWithRetry(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, string method)
	{
		return RetryLogicProvider.Execute(this, () => RunExecuteReader(cmdBehavior, runBehavior, returnStream, method));
	}

	public override object ExecuteScalar()
	{
		SqlConnection.ExecutePermission.Demand();
		_pendingCancel = false;
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlCommand.ExecuteScalar|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteScalar|API|Correlation> ObjectID{0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		bool success = false;
		int? sqlExceptionNumber = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			WriteBeginExecuteEvent();
			SqlDataReader ds = (IsRetryEnabled ? RunExecuteReaderWithRetry(CommandBehavior.Default, RunBehavior.ReturnImmediately, returnStream: true, "ExecuteScalar") : RunExecuteReader(CommandBehavior.Default, RunBehavior.ReturnImmediately, returnStream: true, "ExecuteScalar"));
			object result = CompleteExecuteScalar(ds, returnSqlValue: false);
			success = true;
			return result;
		}
		catch (SqlException ex)
		{
			sqlExceptionNumber = ex.Number;
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
			WriteEndExecuteEvent(success, sqlExceptionNumber, synchronous: true);
		}
	}

	private object CompleteExecuteScalar(SqlDataReader ds, bool returnSqlValue)
	{
		object result = null;
		try
		{
			if (ds.Read() && ds.FieldCount > 0)
			{
				result = ((!returnSqlValue) ? ds.GetValue(0) : ds.GetSqlValue(0));
			}
		}
		finally
		{
			ds.Close();
		}
		return result;
	}

	private Task InternalExecuteNonQueryWithRetry(string methodName, bool sendToPipe, int timeout, out bool usedCache, bool asyncWrite, bool inRetry)
	{
		bool innerUsedCache = false;
		Task result = RetryLogicProvider.Execute(this, () => InternalExecuteNonQuery(null, methodName, sendToPipe, timeout, out innerUsedCache, asyncWrite, inRetry));
		usedCache = innerUsedCache;
		return result;
	}

	public override int ExecuteNonQuery()
	{
		SqlConnection.ExecutePermission.Demand();
		_pendingCancel = false;
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlCommand.ExecuteNonQuery|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteNonQuery|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		bool success = false;
		int? sqlExceptionNumber = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			WriteBeginExecuteEvent();
			bool usedCache;
			if (IsRetryEnabled)
			{
				InternalExecuteNonQueryWithRetry("ExecuteNonQuery", sendToPipe: false, CommandTimeout, out usedCache, asyncWrite: false, inRetry: false);
			}
			else
			{
				InternalExecuteNonQuery(null, "ExecuteNonQuery", sendToPipe: false, CommandTimeout, out usedCache);
			}
			success = true;
			return _rowsAffected;
		}
		catch (SqlException ex)
		{
			sqlExceptionNumber = ex.Number;
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
			WriteEndExecuteEvent(success, sqlExceptionNumber, synchronous: true);
		}
	}

	internal void ExecuteToPipe(SmiContext pipeContext)
	{
		SqlConnection.ExecutePermission.Demand();
		_pendingCancel = false;
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlCommand.ExecuteToPipe|INFO> {0}", ObjectID);
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			InternalExecuteNonQuery(null, "ExecuteNonQuery", sendToPipe: true, CommandTimeout, out var _);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteNonQuery()
	{
		return BeginExecuteNonQuery(null, null);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, object stateObject)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.BeginExecuteNonQuery|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		SqlConnection.ExecutePermission.Demand();
		return BeginExecuteNonQueryInternal(CommandBehavior.Default, callback, stateObject, 0, inRetry: false);
	}

	private IAsyncResult BeginExecuteNonQueryAsync(AsyncCallback callback, object stateObject)
	{
		return BeginExecuteNonQueryInternal(CommandBehavior.Default, callback, stateObject, CommandTimeout, inRetry: false, asyncWrite: true);
	}

	private IAsyncResult BeginExecuteNonQueryInternal(CommandBehavior behavior, AsyncCallback callback, object stateObject, int timeout, bool inRetry, bool asyncWrite = false)
	{
		TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>(stateObject);
		TaskCompletionSource<object> localCompletion = new TaskCompletionSource<object>(stateObject);
		if (!inRetry)
		{
			_pendingCancel = false;
			ValidateAsyncCommand();
		}
		SqlStatistics statistics = null;
		try
		{
			if (!inRetry)
			{
				statistics = SqlStatistics.StartTimer(Statistics);
				WriteBeginExecuteEvent();
			}
			bool usedCache;
			try
			{
				Task task = InternalExecuteNonQuery(localCompletion, "BeginExecuteNonQuery", sendToPipe: false, timeout, out usedCache, asyncWrite, inRetry);
				if (task != null)
				{
					AsyncHelper.ContinueTask(task, localCompletion, delegate
					{
						BeginExecuteNonQueryInternalReadStage(localCompletion);
					});
				}
				else
				{
					BeginExecuteNonQueryInternalReadStage(localCompletion);
				}
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(e))
				{
					throw;
				}
				ReliablePutStateObject();
				throw;
			}
			if (!TriggerInternalEndAndRetryIfNecessary(behavior, stateObject, timeout, "EndExecuteNonQuery", usedCache, inRetry, asyncWrite, taskCompletionSource, localCompletion, InternalEndExecuteNonQuery, BeginExecuteNonQueryInternal))
			{
				taskCompletionSource = localCompletion;
			}
			if (callback != null)
			{
				taskCompletionSource.Task.ContinueWith(delegate(Task<object> t)
				{
					callback(t);
				}, TaskScheduler.Default);
			}
			return taskCompletionSource.Task;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private void BeginExecuteNonQueryInternalReadStage(TaskCompletionSource<object> completion)
	{
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteNonQuery", _activeConnection);
			_stateObj.ReadSni(completion);
		}
		catch (OutOfMemoryException e)
		{
			_activeConnection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_activeConnection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		catch (Exception)
		{
			if (_cachedAsyncState != null)
			{
				_cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			throw;
		}
	}

	private void VerifyEndExecuteState(Task completionTask, string endMethod, bool fullCheckForColumnEncryption = false)
	{
		if (completionTask.IsCanceled)
		{
			if (_stateObj == null)
			{
				throw SQL.CR_ReconnectionCancelled();
			}
			_stateObj.Parser.State = TdsParserState.Broken;
			_stateObj.Parser.Connection.BreakConnection();
			_stateObj.Parser.ThrowExceptionAndWarning(_stateObj);
		}
		else if (completionTask.IsFaulted)
		{
			throw completionTask.Exception.InnerException;
		}
		if (IsColumnEncryptionEnabled && !fullCheckForColumnEncryption)
		{
			if (_activeConnection.State != ConnectionState.Open)
			{
				throw ADP.ClosedConnectionError();
			}
			return;
		}
		if (cachedAsyncState.EndMethodName == null)
		{
			throw ADP.MethodCalledTwice(endMethod);
		}
		if (endMethod != cachedAsyncState.EndMethodName)
		{
			throw ADP.MismatchedAsyncResult(cachedAsyncState.EndMethodName, endMethod);
		}
		if (_activeConnection.State != ConnectionState.Open || !cachedAsyncState.IsActiveConnectionValid(_activeConnection))
		{
			throw ADP.ClosedConnectionError();
		}
	}

	private void WaitForAsyncResults(IAsyncResult asyncResult, bool isInternal)
	{
		Task task = (Task)asyncResult;
		if (!asyncResult.IsCompleted)
		{
			asyncResult.AsyncWaitHandle.WaitOne();
		}
		if (_stateObj != null)
		{
			_stateObj._networkPacketTaskSource = null;
		}
		if (!isInternal && (!IsColumnEncryptionEnabled || !task.IsFaulted))
		{
			_activeConnection.GetOpenTdsConnection().DecrementAsyncCount();
		}
	}

	public int EndExecuteNonQuery(IAsyncResult asyncResult)
	{
		try
		{
			return EndExecuteNonQueryInternal(asyncResult);
		}
		finally
		{
			SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.EndExecuteNonQuery|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		}
	}

	private void ThrowIfReconnectionHasBeenCanceled()
	{
		if (_stateObj == null)
		{
			TaskCompletionSource<object> reconnectionCompletionSource = _reconnectionCompletionSource;
			if (reconnectionCompletionSource != null && reconnectionCompletionSource.Task.IsCanceled)
			{
				throw SQL.CR_ReconnectionCancelled();
			}
		}
	}

	private int EndExecuteNonQueryAsync(IAsyncResult asyncResult)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.EndExecuteNonQueryAsync|Info|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		Exception exception = ((Task)asyncResult).Exception;
		if (exception != null)
		{
			ReliablePutStateObject();
			throw exception.InnerException;
		}
		ThrowIfReconnectionHasBeenCanceled();
		if (!_internalEndExecuteInitiated)
		{
			lock (_stateObj)
			{
				return EndExecuteNonQueryInternal(asyncResult);
			}
		}
		return EndExecuteNonQueryInternal(asyncResult);
	}

	private int EndExecuteNonQueryInternal(IAsyncResult asyncResult)
	{
		SqlStatistics statistics = null;
		bool success = false;
		int? sqlExceptionNumber = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			int result = (int)InternalEndExecuteNonQuery(asyncResult, "EndExecuteNonQuery", isInternal: false);
			success = true;
			return result;
		}
		catch (SqlException ex)
		{
			sqlExceptionNumber = ex.Number;
			_cachedAsyncState?.ResetAsyncState();
			ReliablePutStateObject();
			throw;
		}
		catch (Exception e)
		{
			_cachedAsyncState?.ResetAsyncState();
			if (ADP.IsCatchableExceptionType(e))
			{
				ReliablePutStateObject();
			}
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			WriteEndExecuteEvent(success, sqlExceptionNumber, synchronous: false);
		}
	}

	private object InternalEndExecuteNonQuery(IAsyncResult asyncResult, string endMethod, bool isInternal)
	{
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			VerifyEndExecuteState((Task)asyncResult, endMethod);
			WaitForAsyncResults(asyncResult, isInternal);
			if (IsColumnEncryptionEnabled)
			{
				VerifyEndExecuteState((Task)asyncResult, endMethod, fullCheckForColumnEncryption: true);
			}
			bool flag = true;
			try
			{
				if (!isInternal)
				{
					NotifyDependency();
					if (_internalEndExecuteInitiated)
					{
						cachedAsyncState.ResetAsyncState();
						return _rowsAffected;
					}
				}
				CheckThrowSNIException();
				if (CommandType.Text == CommandType && GetParameterCount(_parameters) == 0)
				{
					try
					{
						if (!_stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, _stateObj, out var _))
						{
							throw SQL.SynchronousCallMayNotPend();
						}
					}
					finally
					{
						if (!isInternal)
						{
							cachedAsyncState.ResetAsyncState();
						}
					}
				}
				else
				{
					CompleteAsyncExecuteReader(isInternal)?.Close();
				}
			}
			catch (Exception e)
			{
				flag = ADP.IsCatchableExceptionType(e);
				throw;
			}
			finally
			{
				if (flag)
				{
					PutStateObject();
				}
			}
			return _rowsAffected;
		}
		catch (OutOfMemoryException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (StackOverflowException e3)
		{
			_activeConnection.Abort(e3);
			throw;
		}
		catch (ThreadAbortException e4)
		{
			_activeConnection.Abort(e4);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
	}

	private Task InternalExecuteNonQuery(TaskCompletionSource<object> completion, string methodName, bool sendToPipe, int timeout, out bool usedCache, bool asyncWrite = false, bool inRetry = false)
	{
		bool async = completion != null;
		usedCache = false;
		SqlStatistics statistics = Statistics;
		_rowsAffected = -1;
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			if (!inRetry)
			{
				ValidateCommand(methodName, async);
			}
			CheckNotificationStateAndAutoEnlist();
			Task task = null;
			if (_activeConnection.IsContextConnection)
			{
				statistics?.SafeIncrement(ref statistics._unpreparedExecs);
				RunExecuteNonQuerySmi(sendToPipe);
			}
			else if (!ShouldUseEnclaveBasedWorkflow && !BatchRPCMode && CommandType.Text == CommandType && GetParameterCount(_parameters) == 0)
			{
				if (statistics != null)
				{
					if (!IsDirty && IsPrepared)
					{
						statistics.SafeIncrement(ref statistics._preparedExecs);
					}
					else
					{
						statistics.SafeIncrement(ref statistics._unpreparedExecs);
					}
				}
				task = RunExecuteNonQueryTds(methodName, async, timeout, asyncWrite);
			}
			else
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.ExecuteNonQuery|INFO> {0}, Command executed as RPC.", ObjectID);
				SqlDataReader reader = RunExecuteReader(CommandBehavior.Default, RunBehavior.UntilDone, returnStream: false, methodName, completion, timeout, out task, out usedCache, asyncWrite, inRetry);
				if (reader != null)
				{
					if (task != null)
					{
						task = AsyncHelper.CreateContinuationTask(task, delegate
						{
							reader.Close();
						});
					}
					else
					{
						reader.Close();
					}
				}
			}
			return task;
		}
		catch (OutOfMemoryException e)
		{
			_activeConnection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_activeConnection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
	}

	public XmlReader ExecuteXmlReader()
	{
		SqlConnection.ExecutePermission.Demand();
		_pendingCancel = false;
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlCommand.ExecuteXmlReader|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteXmlReader|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		bool success = false;
		int? sqlExceptionNumber = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			WriteBeginExecuteEvent();
			SqlDataReader ds = (IsRetryEnabled ? RunExecuteReaderWithRetry(CommandBehavior.SequentialAccess, RunBehavior.ReturnImmediately, returnStream: true, "ExecuteXmlReader") : RunExecuteReader(CommandBehavior.SequentialAccess, RunBehavior.ReturnImmediately, returnStream: true, "ExecuteXmlReader"));
			XmlReader result = CompleteXmlReader(ds);
			success = true;
			return result;
		}
		catch (SqlException ex)
		{
			sqlExceptionNumber = ex.Number;
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
			WriteEndExecuteEvent(success, sqlExceptionNumber, synchronous: true);
		}
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteXmlReader()
	{
		return BeginExecuteXmlReader(null, null);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteXmlReader(AsyncCallback callback, object stateObject)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.BeginExecuteXmlReader|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		SqlConnection.ExecutePermission.Demand();
		return BeginExecuteXmlReaderInternal(CommandBehavior.SequentialAccess, callback, stateObject, 0, inRetry: false);
	}

	private IAsyncResult BeginExecuteXmlReaderAsync(AsyncCallback callback, object stateObject)
	{
		return BeginExecuteXmlReaderInternal(CommandBehavior.SequentialAccess, callback, stateObject, CommandTimeout, inRetry: false, asyncWrite: true);
	}

	private IAsyncResult BeginExecuteXmlReaderInternal(CommandBehavior behavior, AsyncCallback callback, object stateObject, int timeout, bool inRetry, bool asyncWrite = false)
	{
		TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>(stateObject);
		TaskCompletionSource<object> localCompletion = new TaskCompletionSource<object>(stateObject);
		if (!inRetry)
		{
			_pendingCancel = false;
			ValidateAsyncCommand();
		}
		SqlStatistics statistics = null;
		try
		{
			if (!inRetry)
			{
				statistics = SqlStatistics.StartTimer(Statistics);
				WriteBeginExecuteEvent();
			}
			Task task;
			bool usedCache;
			try
			{
				RunExecuteReader(behavior, RunBehavior.ReturnImmediately, returnStream: true, "BeginExecuteXmlReader", localCompletion, timeout, out task, out usedCache, asyncWrite, inRetry);
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(e))
				{
					throw;
				}
				ReliablePutStateObject();
				throw;
			}
			if (task != null)
			{
				AsyncHelper.ContinueTask(task, localCompletion, delegate
				{
					BeginExecuteXmlReaderInternalReadStage(localCompletion);
				});
			}
			else
			{
				BeginExecuteXmlReaderInternalReadStage(localCompletion);
			}
			if (!TriggerInternalEndAndRetryIfNecessary(behavior, stateObject, timeout, "EndExecuteXmlReader", usedCache, inRetry, asyncWrite, taskCompletionSource, localCompletion, InternalEndExecuteReader, BeginExecuteXmlReaderInternal))
			{
				taskCompletionSource = localCompletion;
			}
			if (callback != null)
			{
				taskCompletionSource.Task.ContinueWith(delegate(Task<object> t)
				{
					callback(t);
				}, TaskScheduler.Default);
			}
			return taskCompletionSource.Task;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private void BeginExecuteXmlReaderInternalReadStage(TaskCompletionSource<object> completion)
	{
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteXmlReader", _activeConnection);
			_stateObj.ReadSni(completion);
		}
		catch (OutOfMemoryException ex)
		{
			_activeConnection.Abort(ex);
			completion.TrySetException(ex);
			throw;
		}
		catch (StackOverflowException ex2)
		{
			_activeConnection.Abort(ex2);
			completion.TrySetException(ex2);
			throw;
		}
		catch (ThreadAbortException ex3)
		{
			_activeConnection.Abort(ex3);
			SqlInternalConnection.BestEffortCleanup(target);
			completion.TrySetException(ex3);
			throw;
		}
		catch (Exception exception)
		{
			if (_cachedAsyncState != null)
			{
				_cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			completion.TrySetException(exception);
		}
	}

	public XmlReader EndExecuteXmlReader(IAsyncResult asyncResult)
	{
		try
		{
			return EndExecuteXmlReaderInternal(asyncResult);
		}
		finally
		{
			SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.EndExecuteXmlReader|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		}
	}

	private XmlReader EndExecuteXmlReaderAsync(IAsyncResult asyncResult)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.EndExecuteXmlReaderAsync|Info|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		Exception exception = ((Task)asyncResult).Exception;
		if (exception != null)
		{
			ReliablePutStateObject();
			throw exception.InnerException;
		}
		ThrowIfReconnectionHasBeenCanceled();
		if (!_internalEndExecuteInitiated)
		{
			lock (_stateObj)
			{
				return EndExecuteXmlReaderInternal(asyncResult);
			}
		}
		return EndExecuteXmlReaderInternal(asyncResult);
	}

	private XmlReader EndExecuteXmlReaderInternal(IAsyncResult asyncResult)
	{
		bool success = false;
		int? sqlExceptionNumber = null;
		try
		{
			XmlReader result = CompleteXmlReader(InternalEndExecuteReader(asyncResult, "EndExecuteXmlReader", isInternal: false), async: true);
			success = true;
			return result;
		}
		catch (SqlException ex)
		{
			sqlExceptionNumber = ex.Number;
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			throw;
		}
		catch (Exception e)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			if (ADP.IsCatchableExceptionType(e))
			{
				ReliablePutStateObject();
			}
			throw;
		}
		finally
		{
			WriteEndExecuteEvent(success, sqlExceptionNumber, synchronous: false);
		}
	}

	private XmlReader CompleteXmlReader(SqlDataReader ds, bool async = false)
	{
		XmlReader xmlReader = null;
		SmiExtendedMetaData[] internalSmiMetaData = ds.GetInternalSmiMetaData();
		if (internalSmiMetaData != null && internalSmiMetaData.Length == 1 && (internalSmiMetaData[0].SqlDbType == SqlDbType.NText || internalSmiMetaData[0].SqlDbType == SqlDbType.NVarChar || internalSmiMetaData[0].SqlDbType == SqlDbType.Xml))
		{
			try
			{
				SqlStream sqlStream = new SqlStream(ds, addByteOrderMark: true, internalSmiMetaData[0].SqlDbType != SqlDbType.Xml);
				xmlReader = sqlStream.ToXmlReader(async);
			}
			catch (Exception e)
			{
				if (ADP.IsCatchableExceptionType(e))
				{
					ds.Close();
				}
				throw;
			}
		}
		if (xmlReader == null)
		{
			ds.Close();
			throw SQL.NonXmlResult();
		}
		return xmlReader;
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteReader()
	{
		return BeginExecuteReader(null, null, CommandBehavior.Default);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject)
	{
		return BeginExecuteReader(callback, stateObject, CommandBehavior.Default);
	}

	protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteDbDataReader|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		return ExecuteReader(behavior, "ExecuteReader");
	}

	private SqlDataReader ExecuteReaderWithRetry(CommandBehavior behavior, string method)
	{
		return RetryLogicProvider.Execute(this, () => ExecuteReader(behavior, method));
	}

	public new SqlDataReader ExecuteReader()
	{
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlCommand.ExecuteReader|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteReader|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			return IsRetryEnabled ? ExecuteReaderWithRetry(CommandBehavior.Default, "ExecuteReader") : ExecuteReader(CommandBehavior.Default, "ExecuteReader");
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	public new SqlDataReader ExecuteReader(CommandBehavior behavior)
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlCommand.ExecuteReader|API> {0}, behavior={1}", ObjectID, (int)behavior);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteReader|API|Correlation> ObjectID {0}, behavior={1}, ActivityID {2}", ObjectID, (int)behavior, ActivityCorrelator.Current);
		try
		{
			return IsRetryEnabled ? ExecuteReaderWithRetry(behavior, "ExecuteReader") : ExecuteReader(behavior, "ExecuteReader");
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteReader(CommandBehavior behavior)
	{
		return BeginExecuteReader(null, null, behavior);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject, CommandBehavior behavior)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.BeginExecuteReader|API|Correlation> ObjectID{0}, behavior={1}, ActivityID {2}", ObjectID, (int)behavior, ActivityCorrelator.Current);
		SqlConnection.ExecutePermission.Demand();
		return BeginExecuteReaderInternal(behavior, callback, stateObject, 0, inRetry: false);
	}

	internal SqlDataReader ExecuteReader(CommandBehavior behavior, string method)
	{
		SqlConnection.ExecutePermission.Demand();
		_pendingCancel = false;
		SqlStatistics statistics = null;
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		bool success = false;
		int? sqlExceptionNumber = null;
		try
		{
			WriteBeginExecuteEvent();
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			statistics = SqlStatistics.StartTimer(Statistics);
			SqlDataReader result = RunExecuteReader(behavior, RunBehavior.ReturnImmediately, returnStream: true, method);
			success = true;
			return result;
		}
		catch (SqlException ex)
		{
			sqlExceptionNumber = ex.Number;
			throw;
		}
		catch (OutOfMemoryException e)
		{
			_activeConnection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_activeConnection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			WriteEndExecuteEvent(success, sqlExceptionNumber, synchronous: true);
		}
	}

	public SqlDataReader EndExecuteReader(IAsyncResult asyncResult)
	{
		try
		{
			return EndExecuteReaderInternal(asyncResult);
		}
		finally
		{
			SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.EndExecuteReader|API|Correlation> ObjectID{0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		}
	}

	private SqlDataReader EndExecuteReaderAsync(IAsyncResult asyncResult)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.EndExecuteReaderAsync|Info|Correlation> ObjectID{0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		Exception exception = ((Task)asyncResult).Exception;
		if (exception != null)
		{
			ReliablePutStateObject();
			throw exception.InnerException;
		}
		ThrowIfReconnectionHasBeenCanceled();
		if (!_internalEndExecuteInitiated)
		{
			lock (_stateObj)
			{
				return EndExecuteReaderInternal(asyncResult);
			}
		}
		return EndExecuteReaderInternal(asyncResult);
	}

	private SqlDataReader EndExecuteReaderInternal(IAsyncResult asyncResult)
	{
		SqlStatistics statistics = null;
		bool success = false;
		int? sqlExceptionNumber = null;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			SqlDataReader result = InternalEndExecuteReader(asyncResult, "EndExecuteReader", isInternal: false);
			success = true;
			return result;
		}
		catch (SqlException ex)
		{
			sqlExceptionNumber = ex.Number;
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			throw;
		}
		catch (Exception e)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			if (ADP.IsCatchableExceptionType(e))
			{
				ReliablePutStateObject();
			}
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			WriteEndExecuteEvent(success, sqlExceptionNumber, synchronous: false);
		}
	}

	private IAsyncResult BeginExecuteReaderAsync(CommandBehavior behavior, AsyncCallback callback, object stateObject)
	{
		return BeginExecuteReaderInternal(behavior, callback, stateObject, CommandTimeout, inRetry: false, asyncWrite: true);
	}

	private IAsyncResult BeginExecuteReaderInternal(CommandBehavior behavior, AsyncCallback callback, object stateObject, int timeout, bool inRetry, bool asyncWrite = false)
	{
		TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>(stateObject);
		TaskCompletionSource<object> localCompletion = new TaskCompletionSource<object>(stateObject);
		if (!inRetry)
		{
			_pendingCancel = false;
		}
		SqlStatistics statistics = null;
		try
		{
			if (!inRetry)
			{
				statistics = SqlStatistics.StartTimer(Statistics);
				WriteBeginExecuteEvent();
				ValidateAsyncCommand();
			}
			Task task = null;
			bool usedCache;
			try
			{
				RunExecuteReader(behavior, RunBehavior.ReturnImmediately, returnStream: true, "BeginExecuteReader", localCompletion, timeout, out task, out usedCache, asyncWrite, inRetry);
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(e))
				{
					throw;
				}
				ReliablePutStateObject();
				throw;
			}
			if (task != null)
			{
				AsyncHelper.ContinueTask(task, localCompletion, delegate
				{
					BeginExecuteReaderInternalReadStage(localCompletion);
				});
			}
			else
			{
				BeginExecuteReaderInternalReadStage(localCompletion);
			}
			if (!TriggerInternalEndAndRetryIfNecessary(behavior, stateObject, timeout, "EndExecuteReader", usedCache, inRetry, asyncWrite, taskCompletionSource, localCompletion, InternalEndExecuteReader, BeginExecuteReaderInternal))
			{
				taskCompletionSource = localCompletion;
			}
			if (callback != null)
			{
				taskCompletionSource.Task.ContinueWith(delegate(Task<object> t)
				{
					callback(t);
				}, TaskScheduler.Default);
			}
			return taskCompletionSource.Task;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private bool TriggerInternalEndAndRetryIfNecessary(CommandBehavior behavior, object stateObject, int timeout, string endMethod, bool usedCache, bool inRetry, bool asyncWrite, TaskCompletionSource<object> globalCompletion, TaskCompletionSource<object> localCompletion, Func<IAsyncResult, string, bool, object> endFunc, Func<CommandBehavior, AsyncCallback, object, int, bool, bool, IAsyncResult> retryFunc)
	{
		if (IsColumnEncryptionEnabled && !inRetry && (usedCache || ShouldUseEnclaveBasedWorkflow))
		{
			long firstAttemptStart = ADP.TimerCurrent();
			localCompletion.Task.ContinueWith(delegate(Task<object> tsk)
			{
				if (tsk.IsFaulted)
				{
					globalCompletion.TrySetException(tsk.Exception.InnerException);
				}
				else
				{
					if (!tsk.IsCanceled)
					{
						try
						{
							_internalEndExecuteInitiated = true;
							lock (_stateObj)
							{
								endFunc(tsk, endMethod, arg3: true);
							}
							globalCompletion.TrySetResult(tsk.Result);
							return;
						}
						catch (Exception ex)
						{
							if (ADP.IsCatchableExceptionType(ex))
							{
								ReliablePutStateObject();
							}
							bool flag = ex is EnclaveDelegate.RetryableEnclaveQueryExecutionException;
							if (ex is SqlException)
							{
								SqlException ex2 = ex as SqlException;
								for (int i = 0; i < ex2.Errors.Count; i++)
								{
									if ((usedCache && ex2.Errors[i].Number == 33514) || (ShouldUseEnclaveBasedWorkflow && ex2.Errors[i].Number == 33195))
									{
										flag = true;
										break;
									}
								}
							}
							if (!flag)
							{
								if (_cachedAsyncState != null)
								{
									_cachedAsyncState.ResetAsyncState();
								}
								_activeConnection.GetOpenTdsConnection().DecrementAsyncCount();
								globalCompletion.TrySetException(ex);
								return;
							}
							SqlQueryMetadataCache.GetInstance().InvalidateCacheEntry(this);
							if (ShouldUseEnclaveBasedWorkflow && enclavePackage != null)
							{
								EnclaveSessionParameters enclaveSessionParameters = new EnclaveSessionParameters(_activeConnection.DataSource, _activeConnection.EnclaveAttestationUrl, _activeConnection.Database);
								EnclaveDelegate.Instance.InvalidateEnclaveSession(_activeConnection.AttestationProtocol, _activeConnection.Parser.EnclaveType, enclaveSessionParameters, enclavePackage.EnclaveSession);
							}
							try
							{
								_internalEndExecuteInitiated = false;
								Task<object> task = (Task<object>)retryFunc(behavior, null, stateObject, TdsParserStaticMethods.GetRemainingTimeout(timeout, firstAttemptStart), arg5: true, asyncWrite);
								task.ContinueWith(delegate(Task<object> retryTsk)
								{
									if (retryTsk.IsFaulted)
									{
										globalCompletion.TrySetException(retryTsk.Exception.InnerException);
									}
									else if (retryTsk.IsCanceled)
									{
										globalCompletion.TrySetCanceled();
									}
									else
									{
										globalCompletion.TrySetResult(retryTsk.Result);
									}
								}, TaskScheduler.Default);
								return;
							}
							catch (Exception exception)
							{
								globalCompletion.TrySetException(exception);
								return;
							}
						}
					}
					globalCompletion.TrySetCanceled();
				}
			}, TaskScheduler.Default);
			return true;
		}
		return false;
	}

	private void BeginExecuteReaderInternalReadStage(TaskCompletionSource<object> completion)
	{
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteReader", _activeConnection);
			_stateObj.ReadSni(completion);
		}
		catch (OutOfMemoryException ex)
		{
			_activeConnection.Abort(ex);
			completion.TrySetException(ex);
			throw;
		}
		catch (StackOverflowException ex2)
		{
			_activeConnection.Abort(ex2);
			completion.TrySetException(ex2);
			throw;
		}
		catch (ThreadAbortException ex3)
		{
			_activeConnection.Abort(ex3);
			SqlInternalConnection.BestEffortCleanup(target);
			completion.TrySetException(ex3);
			throw;
		}
		catch (Exception exception)
		{
			if (_cachedAsyncState != null)
			{
				_cachedAsyncState.ResetAsyncState();
			}
			ReliablePutStateObject();
			completion.TrySetException(exception);
		}
	}

	private SqlDataReader InternalEndExecuteReader(IAsyncResult asyncResult, string endMethod, bool isInternal)
	{
		VerifyEndExecuteState((Task)asyncResult, endMethod);
		WaitForAsyncResults(asyncResult, isInternal);
		if (IsColumnEncryptionEnabled)
		{
			VerifyEndExecuteState((Task)asyncResult, endMethod, fullCheckForColumnEncryption: true);
		}
		CheckThrowSNIException();
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			return CompleteAsyncExecuteReader(isInternal);
		}
		catch (OutOfMemoryException e)
		{
			_activeConnection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_activeConnection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
	}

	private Task<int> InternalExecuteNonQueryWithRetryAsync(CancellationToken cancellationToken)
	{
		return RetryLogicProvider.ExecuteAsync(this, () => InternalExecuteNonQueryAsync(cancellationToken), cancellationToken);
	}

	public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
	{
		if (!IsRetryEnabled)
		{
			return InternalExecuteNonQueryAsync(cancellationToken);
		}
		return InternalExecuteNonQueryWithRetryAsync(cancellationToken);
	}

	private Task<int> InternalExecuteNonQueryAsync(CancellationToken cancellationToken)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteNonQueryAsync|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		SqlConnection.ExecutePermission.Demand();
		TaskCompletionSource<int> source = new TaskCompletionSource<int>();
		CancellationTokenRegistration registration = default(CancellationTokenRegistration);
		if (cancellationToken.CanBeCanceled)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				source.SetCanceled();
				return source.Task;
			}
			registration = cancellationToken.Register(CancelIgnoreFailure);
		}
		Task<int> outterTask = source.Task;
		try
		{
			RegisterForConnectionCloseNotification(ref outterTask);
			Task<int>.Factory.FromAsync(BeginExecuteNonQueryAsync, EndExecuteNonQueryAsync, null).ContinueWith(delegate(Task<int> t)
			{
				registration.Dispose();
				if (t.IsFaulted)
				{
					Exception innerException = t.Exception.InnerException;
					source.SetException(innerException);
				}
				else if (t.IsCanceled)
				{
					source.SetCanceled();
				}
				else
				{
					source.SetResult(t.Result);
				}
			}, TaskScheduler.Default);
		}
		catch (Exception exception)
		{
			source.SetException(exception);
		}
		return outterTask;
	}

	protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
	{
		return ExecuteReaderAsync(behavior, cancellationToken).ContinueWith((Func<Task<SqlDataReader>, DbDataReader>)delegate(Task<SqlDataReader> result)
		{
			if (result.IsFaulted)
			{
				throw result.Exception.InnerException;
			}
			return result.Result;
		}, CancellationToken.None, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	private Task<SqlDataReader> InternalExecuteReaderWithRetryAsync(CommandBehavior behavior, CancellationToken cancellationToken)
	{
		return RetryLogicProvider.ExecuteAsync(this, () => InternalExecuteReaderAsync(behavior, cancellationToken), cancellationToken);
	}

	public new Task<SqlDataReader> ExecuteReaderAsync()
	{
		if (!IsRetryEnabled)
		{
			return InternalExecuteReaderAsync(CommandBehavior.Default, CancellationToken.None);
		}
		return InternalExecuteReaderWithRetryAsync(CommandBehavior.Default, CancellationToken.None);
	}

	public new Task<SqlDataReader> ExecuteReaderAsync(CommandBehavior behavior)
	{
		if (!IsRetryEnabled)
		{
			return InternalExecuteReaderAsync(behavior, CancellationToken.None);
		}
		return InternalExecuteReaderWithRetryAsync(behavior, CancellationToken.None);
	}

	public new Task<SqlDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
	{
		if (!IsRetryEnabled)
		{
			return InternalExecuteReaderAsync(CommandBehavior.Default, cancellationToken);
		}
		return InternalExecuteReaderWithRetryAsync(CommandBehavior.Default, cancellationToken);
	}

	public new Task<SqlDataReader> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
	{
		if (!IsRetryEnabled)
		{
			return InternalExecuteReaderAsync(behavior, cancellationToken);
		}
		return InternalExecuteReaderWithRetryAsync(behavior, cancellationToken);
	}

	private Task<SqlDataReader> InternalExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteReaderAsync|API|Correlation> ObjectID {0}, behavior={1}, ActivityID {2}", ObjectID, (int)behavior, ActivityCorrelator.Current);
		SqlConnection.ExecutePermission.Demand();
		TaskCompletionSource<SqlDataReader> source = new TaskCompletionSource<SqlDataReader>();
		CancellationTokenRegistration registration = default(CancellationTokenRegistration);
		if (cancellationToken.CanBeCanceled)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				source.SetCanceled();
				return source.Task;
			}
			registration = cancellationToken.Register(CancelIgnoreFailure);
		}
		Task<SqlDataReader> outterTask = source.Task;
		try
		{
			RegisterForConnectionCloseNotification(ref outterTask);
			Task<SqlDataReader>.Factory.FromAsync(BeginExecuteReaderAsync, EndExecuteReaderAsync, behavior, null).ContinueWith(delegate(Task<SqlDataReader> t)
			{
				registration.Dispose();
				if (t.IsFaulted)
				{
					Exception innerException = t.Exception.InnerException;
					source.SetException(innerException);
				}
				else if (t.IsCanceled)
				{
					source.SetCanceled();
				}
				else
				{
					source.SetResult(t.Result);
				}
			}, TaskScheduler.Default);
		}
		catch (Exception exception)
		{
			source.SetException(exception);
		}
		return outterTask;
	}

	private Task<object> InternalExecuteScalarWithRetryAsync(CancellationToken cancellationToken)
	{
		return RetryLogicProvider.ExecuteAsync(this, () => InternalExecuteScalarAsync(cancellationToken), cancellationToken);
	}

	public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
	{
		if (!IsRetryEnabled)
		{
			return InternalExecuteScalarAsync(cancellationToken);
		}
		return InternalExecuteScalarWithRetryAsync(cancellationToken);
	}

	private Task<object> InternalExecuteScalarAsync(CancellationToken cancellationToken)
	{
		return ExecuteReaderAsync(cancellationToken).ContinueWith(delegate(Task<SqlDataReader> executeTask)
		{
			TaskCompletionSource<object> source = new TaskCompletionSource<object>();
			if (executeTask.IsCanceled)
			{
				source.SetCanceled();
			}
			else if (executeTask.IsFaulted)
			{
				source.SetException(executeTask.Exception.InnerException);
			}
			else
			{
				SqlDataReader reader = executeTask.Result;
				reader.ReadAsync(cancellationToken).ContinueWith(delegate(Task<bool> readTask)
				{
					try
					{
						if (readTask.IsCanceled)
						{
							reader.Dispose();
							source.SetCanceled();
						}
						else if (readTask.IsFaulted)
						{
							reader.Dispose();
							source.SetException(readTask.Exception.InnerException);
						}
						else
						{
							Exception ex = null;
							object result = null;
							try
							{
								if (readTask.Result && reader.FieldCount > 0)
								{
									try
									{
										result = reader.GetValue(0);
									}
									catch (Exception ex2)
									{
										ex = ex2;
									}
								}
							}
							finally
							{
								reader.Dispose();
							}
							if (ex != null)
							{
								source.SetException(ex);
							}
							else
							{
								source.SetResult(result);
							}
						}
					}
					catch (Exception exception)
					{
						source.SetException(exception);
					}
				}, TaskScheduler.Default);
			}
			return source.Task;
		}, TaskScheduler.Default).Unwrap();
	}

	public Task<XmlReader> ExecuteXmlReaderAsync()
	{
		return ExecuteXmlReaderAsync(CancellationToken.None);
	}

	private Task<XmlReader> InternalExecuteXmlReaderWithRetryAsync(CancellationToken cancellationToken)
	{
		return RetryLogicProvider.ExecuteAsync(this, () => InternalExecuteXmlReaderAsync(cancellationToken), cancellationToken);
	}

	public Task<XmlReader> ExecuteXmlReaderAsync(CancellationToken cancellationToken)
	{
		if (!IsRetryEnabled)
		{
			return InternalExecuteXmlReaderAsync(cancellationToken);
		}
		return InternalExecuteXmlReaderWithRetryAsync(cancellationToken);
	}

	private Task<XmlReader> InternalExecuteXmlReaderAsync(CancellationToken cancellationToken)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlCommand.ExecuteXmlReaderAsync|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		SqlConnection.ExecutePermission.Demand();
		TaskCompletionSource<XmlReader> source = new TaskCompletionSource<XmlReader>();
		CancellationTokenRegistration registration = default(CancellationTokenRegistration);
		if (cancellationToken.CanBeCanceled)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				source.SetCanceled();
				return source.Task;
			}
			registration = cancellationToken.Register(CancelIgnoreFailure);
		}
		Task<XmlReader> outterTask = source.Task;
		try
		{
			RegisterForConnectionCloseNotification(ref outterTask);
			Task<XmlReader>.Factory.FromAsync(BeginExecuteXmlReaderAsync, EndExecuteXmlReaderAsync, null).ContinueWith(delegate(Task<XmlReader> t)
			{
				registration.Dispose();
				if (t.IsFaulted)
				{
					Exception innerException = t.Exception.InnerException;
					source.SetException(innerException);
				}
				else if (t.IsCanceled)
				{
					source.SetCanceled();
				}
				else
				{
					source.SetResult(t.Result);
				}
			}, TaskScheduler.Default);
		}
		catch (Exception exception)
		{
			source.SetException(exception);
		}
		return outterTask;
	}

	public void RegisterColumnEncryptionKeyStoreProvidersOnCommand(IDictionary<string, SqlColumnEncryptionKeyStoreProvider> customProviders)
	{
		ValidateCustomProviders(customProviders);
		Dictionary<string, SqlColumnEncryptionKeyStoreProvider> customColumnEncryptionKeyStoreProviders = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>(customProviders, StringComparer.OrdinalIgnoreCase);
		_customColumnEncryptionKeyStoreProviders = customColumnEncryptionKeyStoreProviders;
	}

	private void ValidateCustomProviders(IDictionary<string, SqlColumnEncryptionKeyStoreProvider> customProviders)
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

	internal bool TryGetColumnEncryptionKeyStoreProvider(string providerName, out SqlColumnEncryptionKeyStoreProvider columnKeyStoreProvider)
	{
		return _customColumnEncryptionKeyStoreProviders.TryGetValue(providerName, out columnKeyStoreProvider);
	}

	internal List<string> GetColumnEncryptionCustomKeyStoreProvidersNames()
	{
		return _customColumnEncryptionKeyStoreProviders.Keys.ToList();
	}

	private static string UnquoteProcedurePart(string part)
	{
		if (part != null && 2 <= part.Length && '[' == part[0] && ']' == part[part.Length - 1])
		{
			part = part.Substring(1, part.Length - 2);
			part = part.Replace("]]", "]");
		}
		return part;
	}

	private static string UnquoteProcedureName(string name, out object groupNumber)
	{
		groupNumber = null;
		string text = name;
		if (text != null)
		{
			if (char.IsDigit(text[text.Length - 1]))
			{
				int num = text.LastIndexOf(';');
				if (num != -1)
				{
					string s = text.Substring(num + 1);
					int result = 0;
					if (int.TryParse(s, out result))
					{
						groupNumber = result;
						text = text.Substring(0, num);
					}
				}
			}
			text = UnquoteProcedurePart(text);
		}
		return text;
	}

	internal void DeriveParameters()
	{
		switch (CommandType)
		{
		case CommandType.Text:
			throw ADP.DeriveParametersNotSupported(this);
		case CommandType.TableDirect:
			throw ADP.DeriveParametersNotSupported(this);
		default:
			throw ADP.InvalidCommandType(CommandType);
		case CommandType.StoredProcedure:
		{
			ValidateCommand("DeriveParameters", async: false);
			string[] array = MultipartIdentifier.ParseMultipartIdentifier(CommandText, "[\"", "]\"", Strings.SQL_SqlCommandCommandText, ThrowOnEmptyMultipartName: false);
			if (array[3] == null || ADP.IsEmpty(array[3]))
			{
				throw ADP.NoStoredProcedureExists(CommandText);
			}
			SqlCommand sqlCommand = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (!ADP.IsEmpty(array[0]))
			{
				SqlCommandSet.BuildStoredProcedureName(stringBuilder, array[0]);
				stringBuilder.Append(".");
			}
			if (ADP.IsEmpty(array[1]))
			{
				array[1] = Connection.Database;
			}
			SqlCommandSet.BuildStoredProcedureName(stringBuilder, array[1]);
			stringBuilder.Append(".");
			string[] array2;
			bool flag;
			if (Connection.IsKatmaiOrNewer)
			{
				stringBuilder.Append("[sys].[").Append("sp_procedure_params_100_managed").Append("]");
				array2 = KatmaiProcParamsNames;
				flag = true;
			}
			else
			{
				if (Connection.IsYukonOrNewer)
				{
					stringBuilder.Append("[sys].[").Append("sp_procedure_params_managed").Append("]");
				}
				else
				{
					stringBuilder.Append(".[").Append("sp_procedure_params_rowset").Append("]");
				}
				array2 = PreKatmaiProcParamsNames;
				flag = false;
			}
			sqlCommand = new SqlCommand(stringBuilder.ToString(), Connection, Transaction)
			{
				CommandType = CommandType.StoredProcedure
			};
			sqlCommand.Parameters.Add(new SqlParameter("@procedure_name", SqlDbType.NVarChar, 255));
			sqlCommand.Parameters[0].Value = UnquoteProcedureName(array[3], out var groupNumber);
			if (groupNumber != null)
			{
				SqlParameter sqlParameter = sqlCommand.Parameters.Add(new SqlParameter("@group_number", SqlDbType.Int));
				sqlParameter.Value = groupNumber;
			}
			if (!ADP.IsEmpty(array[2]))
			{
				SqlParameter sqlParameter2 = sqlCommand.Parameters.Add(new SqlParameter("@procedure_schema", SqlDbType.NVarChar, 255));
				sqlParameter2.Value = UnquoteProcedurePart(array[2]);
			}
			SqlDataReader sqlDataReader = null;
			List<SqlParameter> list = new List<SqlParameter>();
			bool flag2 = true;
			try
			{
				sqlDataReader = sqlCommand.ExecuteReader();
				SqlParameter sqlParameter3 = null;
				while (sqlDataReader.Read())
				{
					sqlParameter3 = new SqlParameter
					{
						ParameterName = (string)sqlDataReader[array2[0]]
					};
					if (flag)
					{
						sqlParameter3.SqlDbType = (SqlDbType)(short)sqlDataReader[array2[3]];
						switch (sqlParameter3.SqlDbType)
						{
						case SqlDbType.Image:
						case SqlDbType.Timestamp:
							sqlParameter3.SqlDbType = SqlDbType.VarBinary;
							break;
						case SqlDbType.NText:
							sqlParameter3.SqlDbType = SqlDbType.NVarChar;
							break;
						case SqlDbType.Text:
							sqlParameter3.SqlDbType = SqlDbType.VarChar;
							break;
						}
					}
					else
					{
						sqlParameter3.SqlDbType = MetaType.GetSqlDbTypeFromOleDbType((short)sqlDataReader[array2[2]], ADP.IsNull(sqlDataReader[array2[9]]) ? ADP.StrEmpty : ((string)sqlDataReader[array2[9]]));
					}
					if (sqlDataReader[array2[4]] is int num)
					{
						if (num == 0 && (sqlParameter3.SqlDbType == SqlDbType.NVarChar || sqlParameter3.SqlDbType == SqlDbType.VarBinary || sqlParameter3.SqlDbType == SqlDbType.VarChar))
						{
							num = -1;
						}
						sqlParameter3.Size = num;
					}
					sqlParameter3.Direction = ParameterDirectionFromOleDbDirection((short)sqlDataReader[array2[1]]);
					if (sqlParameter3.SqlDbType == SqlDbType.Decimal)
					{
						sqlParameter3.ScaleInternal = (byte)((uint)(short)sqlDataReader[array2[6]] & 0xFFu);
						sqlParameter3.PrecisionInternal = (byte)((uint)(short)sqlDataReader[array2[5]] & 0xFFu);
					}
					if (SqlDbType.Udt == sqlParameter3.SqlDbType)
					{
						string text = ((!flag) ? ((string)sqlDataReader[array2[13]]) : ((string)sqlDataReader[array2[9]]));
						sqlParameter3.UdtTypeName = sqlDataReader[array2[7]]?.ToString() + "." + sqlDataReader[array2[8]]?.ToString() + "." + text;
					}
					if (SqlDbType.Structured == sqlParameter3.SqlDbType)
					{
						sqlParameter3.TypeName = sqlDataReader[array2[7]]?.ToString() + "." + sqlDataReader[array2[8]]?.ToString() + "." + sqlDataReader[array2[9]];
						sqlParameter3.IsDerivedParameterTypeName = true;
					}
					if (SqlDbType.Xml == sqlParameter3.SqlDbType)
					{
						object obj = sqlDataReader[array2[10]];
						sqlParameter3.XmlSchemaCollectionDatabase = (ADP.IsNull(obj) ? string.Empty : ((string)obj));
						obj = sqlDataReader[array2[11]];
						sqlParameter3.XmlSchemaCollectionOwningSchema = (ADP.IsNull(obj) ? string.Empty : ((string)obj));
						obj = sqlDataReader[array2[12]];
						sqlParameter3.XmlSchemaCollectionName = (ADP.IsNull(obj) ? string.Empty : ((string)obj));
					}
					if (MetaType._IsVarTime(sqlParameter3.SqlDbType))
					{
						object obj2 = sqlDataReader[array2[14]];
						if (obj2 is int)
						{
							sqlParameter3.ScaleInternal = (byte)((uint)(int)obj2 & 0xFFu);
						}
					}
					list.Add(sqlParameter3);
				}
			}
			catch (Exception e)
			{
				flag2 = ADP.IsCatchableExceptionType(e);
				throw;
			}
			finally
			{
				if (flag2)
				{
					sqlDataReader?.Close();
					sqlCommand.Connection = null;
				}
			}
			if (list.Count == 0)
			{
				throw ADP.NoStoredProcedureExists(CommandText);
			}
			Parameters.Clear();
			{
				foreach (SqlParameter item in list)
				{
					_parameters.Add(item);
				}
				break;
			}
		}
		}
	}

	private ParameterDirection ParameterDirectionFromOleDbDirection(short oledbDirection)
	{
		return oledbDirection switch
		{
			2 => ParameterDirection.InputOutput, 
			3 => ParameterDirection.Output, 
			4 => ParameterDirection.ReturnValue, 
			_ => ParameterDirection.Input, 
		};
	}

	private void CheckNotificationStateAndAutoEnlist()
	{
		if (NotificationAutoEnlist && _activeConnection.IsYukonOrNewer)
		{
			string text = SqlNotificationContext();
			if (!ADP.IsEmpty(text))
			{
				SqlDependencyPerAppDomainDispatcher.SingletonInstance.LookupDependencyEntry(text)?.AddCommandDependency(this);
			}
		}
		if (Notification != null && _sqlDep != null)
		{
			if (_sqlDep.Options == null)
			{
				SqlDependency.IdentityUserNamePair identityUserNamePair = null;
				SqlInternalConnectionTds sqlInternalConnectionTds = _activeConnection.InnerConnection as SqlInternalConnectionTds;
				identityUserNamePair = ((sqlInternalConnectionTds.Identity == null) ? new SqlDependency.IdentityUserNamePair(null, sqlInternalConnectionTds.ConnectionOptions.UserID) : new SqlDependency.IdentityUserNamePair(sqlInternalConnectionTds.Identity, null));
				Notification.Options = SqlDependency.GetDefaultComposedOptions(_activeConnection.DataSource, InternalTdsConnection.ServerProvidedFailOverPartner, identityUserNamePair, _activeConnection.Database);
			}
			Notification.UserData = _sqlDep.ComputeHashAndAddToDispatcher(this);
			_sqlDep.AddToServerList(_activeConnection.DataSource);
		}
	}

	[SecurityPermission(SecurityAction.Assert, Infrastructure = true)]
	internal static string SqlNotificationContext()
	{
		return CallContext.GetData("MS.SqlDependencyCookie") as string;
	}

	private Task RunExecuteNonQueryTds(string methodName, bool async, int timeout, bool asyncWrite)
	{
		bool flag = true;
		try
		{
			Task task = _activeConnection.ValidateAndReconnect(null, timeout);
			if (task != null)
			{
				long reconnectionStart = ADP.TimerCurrent();
				if (async)
				{
					TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
					_activeConnection.RegisterWaitingForReconnect(completion.Task);
					_reconnectionCompletionSource = completion;
					CancellationTokenSource timeoutCTS = new CancellationTokenSource();
					AsyncHelper.SetTimeoutException(completion, timeout, SQL.CR_ReconnectTimeout, timeoutCTS.Token);
					AsyncHelper.ContinueTask(task, completion, delegate
					{
						if (!completion.Task.IsCompleted)
						{
							Interlocked.CompareExchange(ref _reconnectionCompletionSource, null, completion);
							timeoutCTS.Cancel();
							Task task3 = RunExecuteNonQueryTds(methodName, async, TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart), asyncWrite);
							if (task3 == null)
							{
								completion.SetResult(null);
							}
							else
							{
								AsyncHelper.ContinueTask(task3, completion, delegate
								{
									completion.SetResult(null);
								});
							}
						}
					}, null, null, null, null, _activeConnection);
					return completion.Task;
				}
				AsyncHelper.WaitForCompletion(task, timeout, delegate
				{
					throw SQL.CR_ReconnectTimeout();
				});
				timeout = TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart);
			}
			if (asyncWrite)
			{
				_activeConnection.AddWeakReference(this, 2);
			}
			GetStateObject();
			ResetEncryptionState();
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.ExecuteNonQuery|INFO> {0}, Command executed as SQLBATCH.", ObjectID);
			Task task2 = _stateObj.Parser.TdsExecuteSQLBatch(CommandText, timeout, Notification, _stateObj, sync: true);
			NotifyDependency();
			bool dataReady;
			if (async)
			{
				_activeConnection.GetOpenTdsConnection(methodName).IncrementAsyncCount();
			}
			else if (!_stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, _stateObj, out dataReady))
			{
				throw SQL.SynchronousCallMayNotPend();
			}
		}
		catch (Exception e)
		{
			flag = ADP.IsCatchableExceptionType(e);
			throw;
		}
		finally
		{
			if (flag && !async)
			{
				PutStateObject();
			}
		}
		return null;
	}

	private void RunExecuteNonQuerySmi(bool sendToPipe)
	{
		SqlInternalConnectionSmi internalSmiConnection = InternalSmiConnection;
		SmiRequestExecutor smiRequestExecutor = null;
		try
		{
			smiRequestExecutor = SetUpSmiRequest(internalSmiConnection);
			SmiExecuteType executeType = (sendToPipe ? SmiExecuteType.ToPipe : SmiExecuteType.NonQuery);
			SmiEventStream smiEventStream = null;
			bool flag = true;
			try
			{
				internalSmiConnection.GetCurrentTransactionPair(out var transactionId, out var transaction);
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlCommand.RunExecuteNonQuerySmi|ADV> {0}, innerConnection={1}, transactionId=0x{2}, cmdBehavior={3}.", ObjectID, internalSmiConnection.ObjectID, transactionId, 0);
				smiEventStream = ((SmiContextFactory.Instance.NegotiatedSmiVersion < 210) ? smiRequestExecutor.Execute(internalSmiConnection.SmiConnection, transactionId, CommandBehavior.Default, executeType) : smiRequestExecutor.Execute(internalSmiConnection.SmiConnection, transactionId, transaction, CommandBehavior.Default, executeType));
				while (smiEventStream.HasEvents)
				{
					smiEventStream.ProcessEvent(EventSink);
				}
			}
			catch (Exception e)
			{
				flag = ADP.IsCatchableExceptionType(e);
				throw;
			}
			finally
			{
				if (smiEventStream != null && flag)
				{
					smiEventStream.Close(EventSink);
				}
			}
			EventSink.ProcessMessagesAndThrow();
		}
		finally
		{
			if (smiRequestExecutor != null)
			{
				smiRequestExecutor.Close(EventSink);
				EventSink.ProcessMessagesAndThrow(ignoreNonFatalMessages: true);
			}
		}
	}

	private void ResetEncryptionState()
	{
		ClearDescribeParameterEncryptionRequests();
		_internalEndExecuteInitiated = false;
		CachingQueryMetadataPostponed = false;
		if (_parameters != null)
		{
			for (int i = 0; i < _parameters.Count; i++)
			{
				_parameters[i].CipherMetadata = null;
				_parameters[i].HasReceivedMetadata = false;
			}
		}
		keysToBeSentToEnclave?.Clear();
		enclavePackage = null;
		requiresEnclaveComputations = false;
		enclaveAttestationParameters = null;
		customData = null;
		customDataLength = 0;
	}

	private void PrepareTransparentEncryptionFinallyBlock(bool closeDataReader, bool clearDataStructures, bool decrementAsyncCount, bool wasDescribeParameterEncryptionNeeded, ReadOnlyDictionary<_SqlRPC, _SqlRPC> describeParameterEncryptionRpcOriginalRpcMap, SqlDataReader describeParameterEncryptionDataReader)
	{
		if (clearDataStructures)
		{
			ClearDescribeParameterEncryptionRequests();
			if (describeParameterEncryptionRpcOriginalRpcMap != null)
			{
				describeParameterEncryptionRpcOriginalRpcMap = null;
			}
		}
		if (decrementAsyncCount)
		{
			_activeConnection.GetOpenTdsConnection()?.DecrementAsyncCount();
		}
		if (closeDataReader)
		{
			describeParameterEncryptionDataReader?.Close();
		}
	}

	private void PrepareForTransparentEncryption(CommandBehavior cmdBehavior, bool returnStream, bool async, int timeout, TaskCompletionSource<object> completion, out Task returnTask, bool asyncWrite, out bool usedCache, bool inRetry)
	{
		Task task = null;
		bool describeParameterEncryptionNeeded = false;
		SqlDataReader describeParameterEncryptionDataReader = null;
		returnTask = null;
		usedCache = false;
		if (!BatchRPCMode && !inRetry && _parameters != null && _parameters.Count > 0 && SqlQueryMetadataCache.GetInstance().GetQueryMetadataIfExists(this))
		{
			usedCache = true;
			return;
		}
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		ReadOnlyDictionary<_SqlRPC, _SqlRPC> describeParameterEncryptionRpcOriginalRpcMap = null;
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			try
			{
				describeParameterEncryptionDataReader = TryFetchInputParameterEncryptionInfo(timeout, async, asyncWrite, out describeParameterEncryptionNeeded, out task, out describeParameterEncryptionRpcOriginalRpcMap);
				if (!describeParameterEncryptionNeeded)
				{
					return;
				}
				flag2 = async;
				if (task != null)
				{
					flag = false;
					returnTask = AsyncHelper.CreateContinuationTask(task, delegate
					{
						bool flag6 = true;
						bool flag7 = true;
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							CheckThrowSNIException();
							SqlInternalConnectionTds openTdsConnection2 = _activeConnection.GetOpenTdsConnection();
							if (openTdsConnection2 != null)
							{
								openTdsConnection2.DecrementAsyncCount();
								flag7 = false;
							}
							describeParameterEncryptionDataReader = CompleteAsyncExecuteReader(isInternal: false, forDescribeParameterEncryption: true);
							ReadDescribeEncryptionParameterResults(describeParameterEncryptionDataReader, describeParameterEncryptionRpcOriginalRpcMap);
						}
						catch (Exception e7)
						{
							flag6 = ADP.IsCatchableExceptionType(e7);
							throw;
						}
						finally
						{
							SqlCommand sqlCommand2 = this;
							bool closeDataReader3 = flag6;
							bool decrementAsyncCount3 = flag7;
							sqlCommand2.PrepareTransparentEncryptionFinallyBlock(closeDataReader3, flag6, decrementAsyncCount3, describeParameterEncryptionNeeded, describeParameterEncryptionRpcOriginalRpcMap, describeParameterEncryptionDataReader);
						}
					}, null, delegate(Exception exception)
					{
						if (_cachedAsyncState != null)
						{
							_cachedAsyncState.ResetAsyncState();
						}
						if (exception != null)
						{
							throw exception;
						}
					});
					flag2 = false;
				}
				else if (async)
				{
					flag = false;
					returnTask = Task.Run(delegate
					{
						bool flag4 = true;
						bool flag5 = true;
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							CheckThrowSNIException();
							SqlInternalConnectionTds openTdsConnection = _activeConnection.GetOpenTdsConnection();
							if (openTdsConnection != null)
							{
								openTdsConnection.DecrementAsyncCount();
								flag5 = false;
							}
							describeParameterEncryptionDataReader = CompleteAsyncExecuteReader(isInternal: false, forDescribeParameterEncryption: true);
							ReadDescribeEncryptionParameterResults(describeParameterEncryptionDataReader, describeParameterEncryptionRpcOriginalRpcMap);
						}
						catch (Exception e6)
						{
							flag4 = ADP.IsCatchableExceptionType(e6);
							throw;
						}
						finally
						{
							SqlCommand sqlCommand = this;
							bool closeDataReader2 = flag4;
							bool decrementAsyncCount2 = flag5;
							sqlCommand.PrepareTransparentEncryptionFinallyBlock(closeDataReader2, flag4, decrementAsyncCount2, describeParameterEncryptionNeeded, describeParameterEncryptionRpcOriginalRpcMap, describeParameterEncryptionDataReader);
						}
					});
					flag2 = false;
				}
				else
				{
					ReadDescribeEncryptionParameterResults(describeParameterEncryptionDataReader, describeParameterEncryptionRpcOriginalRpcMap);
				}
			}
			catch (Exception e)
			{
				flag = ADP.IsCatchableExceptionType(e);
				flag3 = true;
				throw;
			}
			finally
			{
				PrepareTransparentEncryptionFinallyBlock((flag && !async) || flag3, decrementAsyncCount: flag2 && flag3, clearDataStructures: (flag && !async) || flag3, wasDescribeParameterEncryptionNeeded: describeParameterEncryptionNeeded, describeParameterEncryptionRpcOriginalRpcMap: describeParameterEncryptionRpcOriginalRpcMap, describeParameterEncryptionDataReader: describeParameterEncryptionDataReader);
			}
		}
		catch (OutOfMemoryException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (StackOverflowException e3)
		{
			_activeConnection.Abort(e3);
			throw;
		}
		catch (ThreadAbortException e4)
		{
			_activeConnection.Abort(e4);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		catch (Exception e5)
		{
			if (cachedAsyncState != null)
			{
				cachedAsyncState.ResetAsyncState();
			}
			if (ADP.IsCatchableExceptionType(e5))
			{
				ReliablePutStateObject();
			}
			throw;
		}
	}

	private SqlDataReader TryFetchInputParameterEncryptionInfo(int timeout, bool async, bool asyncWrite, out bool inputParameterEncryptionNeeded, out Task task, out ReadOnlyDictionary<_SqlRPC, _SqlRPC> describeParameterEncryptionRpcOriginalRpcMap)
	{
		inputParameterEncryptionNeeded = false;
		task = null;
		describeParameterEncryptionRpcOriginalRpcMap = null;
		byte[] array = null;
		if (ShouldUseEnclaveBasedWorkflow)
		{
			SqlConnectionAttestationProtocol attestationProtocol = _activeConnection.AttestationProtocol;
			string enclaveType = _activeConnection.Parser.EnclaveType;
			EnclaveSessionParameters enclaveSessionParameters = new EnclaveSessionParameters(_activeConnection.DataSource, _activeConnection.EnclaveAttestationUrl, _activeConnection.Database);
			SqlEnclaveSession sqlEnclaveSession = null;
			EnclaveDelegate.Instance.GetEnclaveSession(attestationProtocol, enclaveType, enclaveSessionParameters, generateCustomData: true, out sqlEnclaveSession, out customData, out customDataLength);
			if (sqlEnclaveSession == null)
			{
				enclaveAttestationParameters = EnclaveDelegate.Instance.GetAttestationParameters(attestationProtocol, enclaveType, enclaveSessionParameters.AttestationUrl, customData, customDataLength);
				array = EnclaveDelegate.Instance.GetSerializedAttestationParameters(enclaveAttestationParameters, enclaveType);
			}
		}
		if (BatchRPCMode)
		{
			Dictionary<_SqlRPC, _SqlRPC> dictionary = new Dictionary<_SqlRPC, _SqlRPC>();
			for (int i = 0; i < _SqlRPCBatchArray.Length; i++)
			{
				if (_SqlRPCBatchArray[i].parameters.Length > 1)
				{
					_SqlRPCBatchArray[i].needsFetchParameterEncryptionMetadata = true;
					_SqlRPC describeParameterEncryptionRequest = new _SqlRPC();
					PrepareDescribeParameterEncryptionRequest(_SqlRPCBatchArray[i], ref describeParameterEncryptionRequest, (i == 0) ? array : null);
					dictionary.Add(describeParameterEncryptionRequest, _SqlRPCBatchArray[i]);
				}
			}
			describeParameterEncryptionRpcOriginalRpcMap = new ReadOnlyDictionary<_SqlRPC, _SqlRPC>(dictionary);
			if (describeParameterEncryptionRpcOriginalRpcMap.Count == 0)
			{
				return null;
			}
			inputParameterEncryptionNeeded = true;
			_sqlRPCParameterEncryptionReqArray = describeParameterEncryptionRpcOriginalRpcMap.Keys.ToArray();
		}
		else if (ShouldUseEnclaveBasedWorkflow || GetParameterCount(_parameters) != 0)
		{
			inputParameterEncryptionNeeded = true;
			_sqlRPCParameterEncryptionReqArray = new _SqlRPC[1];
			_SqlRPC rpc = null;
			GetRPCObject(GetParameterCount(_parameters), ref rpc);
			rpc.rpcName = CommandText;
			int num = 0;
			if (_parameters != null)
			{
				foreach (SqlParameter parameter in _parameters)
				{
					rpc.parameters[num++] = parameter;
				}
			}
			PrepareDescribeParameterEncryptionRequest(rpc, ref _sqlRPCParameterEncryptionReqArray[0], array);
		}
		if (inputParameterEncryptionNeeded)
		{
			IsDescribeParameterEncryptionRPCCurrentlyInProgress = true;
			return RunExecuteReaderTds(CommandBehavior.Default, RunBehavior.ReturnImmediately, returnStream: true, async, timeout, out task, asyncWrite, inRetry: false, null, describeParameterEncryptionRequest: true);
		}
		return null;
	}

	private SqlParameter GetSqlParameterWithQueryText(string queryText)
	{
		SqlParameter sqlParameter = new SqlParameter(null, (queryText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, queryText.Length);
		sqlParameter.Value = queryText;
		return sqlParameter;
	}

	private void PrepareDescribeParameterEncryptionRequest(_SqlRPC originalRpcRequest, ref _SqlRPC describeParameterEncryptionRequest, byte[] attestationParameters = null)
	{
		GetRPCObject((attestationParameters == null) ? 2 : 3, ref describeParameterEncryptionRequest, forSpDescribeParameterEncryption: true);
		describeParameterEncryptionRequest.rpcName = "sp_describe_parameter_encryption";
		SqlParameter sqlParameter;
		if (BatchRPCMode)
		{
			string queryText = (string)originalRpcRequest.parameters[0].Value;
			sqlParameter = GetSqlParameterWithQueryText(queryText);
		}
		else
		{
			string queryText = originalRpcRequest.rpcName;
			sqlParameter = ((CommandType != CommandType.StoredProcedure) ? GetSqlParameterWithQueryText(queryText) : BuildStoredProcedureStatementForColumnEncryption(queryText, originalRpcRequest.parameters));
		}
		describeParameterEncryptionRequest.parameters[0] = sqlParameter;
		string text = null;
		if (BatchRPCMode)
		{
			if (originalRpcRequest.parameters.Length > 1)
			{
				text = (string)originalRpcRequest.parameters[1].Value;
			}
		}
		else
		{
			SqlParameterCollection sqlParameterCollection = new SqlParameterCollection();
			if (_parameters != null)
			{
				for (int i = 0; i < _parameters.Count; i++)
				{
					SqlParameter sqlParameter2 = originalRpcRequest.parameters[i];
					SqlParameter sqlParameter3 = new SqlParameter(sqlParameter2.ParameterName, sqlParameter2.SqlDbType, sqlParameter2.Size, sqlParameter2.Direction, sqlParameter2.Precision, sqlParameter2.Scale, sqlParameter2.SourceColumn, sqlParameter2.SourceVersion, sqlParameter2.SourceColumnNullMapping, sqlParameter2.Value, sqlParameter2.XmlSchemaCollectionDatabase, sqlParameter2.XmlSchemaCollectionOwningSchema, sqlParameter2.XmlSchemaCollectionName);
					sqlParameter3.CompareInfo = sqlParameter2.CompareInfo;
					sqlParameter3.TypeName = sqlParameter2.TypeName;
					sqlParameter3.UdtTypeName = sqlParameter2.UdtTypeName;
					sqlParameter3.IsNullable = sqlParameter2.IsNullable;
					sqlParameter3.LocaleId = sqlParameter2.LocaleId;
					sqlParameter3.Offset = sqlParameter2.Offset;
					sqlParameterCollection.Add(sqlParameter3);
				}
			}
			TdsParser tdsParser = null;
			if (_activeConnection.Parser != null)
			{
				tdsParser = _activeConnection.Parser;
				if (tdsParser == null || tdsParser.State == TdsParserState.Broken || tdsParser.State == TdsParserState.Closed)
				{
					throw ADP.ClosedConnectionError();
				}
			}
			text = BuildParamList(tdsParser, sqlParameterCollection, includeReturnValue: true);
		}
		sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
		sqlParameter.Value = text;
		describeParameterEncryptionRequest.parameters[1] = sqlParameter;
		if (attestationParameters != null)
		{
			SqlParameter sqlParameter4 = new SqlParameter(null, SqlDbType.VarBinary)
			{
				Direction = ParameterDirection.Input,
				Size = attestationParameters.Length,
				Value = attestationParameters
			};
			describeParameterEncryptionRequest.parameters[2] = sqlParameter4;
		}
	}

	private void ReadDescribeEncryptionParameterResults(SqlDataReader ds, ReadOnlyDictionary<_SqlRPC, _SqlRPC> describeParameterEncryptionRpcOriginalRpcMap)
	{
		_SqlRPC sqlRPC = null;
		int num = -1;
		Dictionary<int, SqlTceCipherInfoEntry> dictionary = new Dictionary<int, SqlTceCipherInfoEntry>();
		int num2 = 0;
		while (!BatchRPCMode || num2 < _sqlRPCParameterEncryptionReqArray.Length)
		{
			bool flag = true;
			SqlTceCipherInfoEntry value;
			while (ds.Read())
			{
				num = ds.GetInt32(0);
				if (!dictionary.TryGetValue(num, out value))
				{
					value = new SqlTceCipherInfoEntry(num);
					dictionary.Add(num, value);
				}
				byte[] buffer = null;
				int num3 = (int)ds.GetBytes(5, 0L, buffer, 0, 0);
				buffer = new byte[num3];
				ds.GetBytes(5, 0L, buffer, 0, num3);
				byte[] array = new byte[8];
				ds.GetBytes(4, 0L, array, 0, array.Length);
				string @string = ds.GetString(6);
				string string2 = ds.GetString(7);
				value.Add(buffer, ds.GetInt32(1), ds.GetInt32(2), ds.GetInt32(3), array, string2, @string, ds.GetString(8));
				bool flag2 = false;
				if (_activeConnection.Parser.TceVersionSupported >= 2)
				{
					flag2 = ds.GetBoolean(9);
				}
				else
				{
					flag = false;
				}
				if (flag2)
				{
					if (string.IsNullOrWhiteSpace(Connection.EnclaveAttestationUrl))
					{
						throw SQL.NoAttestationUrlSpecifiedForEnclaveBasedQuerySpDescribe(_activeConnection.Parser.EnclaveType);
					}
					byte[] array2 = null;
					if (!ds.IsDBNull(10))
					{
						int num4 = (int)ds.GetBytes(10, 0L, array2, 0, 0);
						array2 = new byte[num4];
						ds.GetBytes(10, 0L, array2, 0, num4);
					}
					SqlSecurityUtility.VerifyColumnMasterKeySignature(@string, string2, flag2, array2, _activeConnection, this);
					int num5 = num;
					if (!dictionary.TryGetValue(num5, out var value2))
					{
						throw SQL.InvalidEncryptionKeyOrdinalEnclaveMetadata(num5, dictionary.Count);
					}
					if (keysToBeSentToEnclave == null)
					{
						keysToBeSentToEnclave = new ConcurrentDictionary<int, SqlTceCipherInfoEntry>();
						keysToBeSentToEnclave.TryAdd(num, value2);
					}
					else if (!keysToBeSentToEnclave.ContainsKey(num))
					{
						keysToBeSentToEnclave.TryAdd(num, value2);
					}
					requiresEnclaveComputations = true;
				}
			}
			if (!flag && !ds.NextResult())
			{
				throw SQL.UnexpectedDescribeParamFormatParameterMetadata();
			}
			int num6 = 0;
			int num7 = 0;
			if (BatchRPCMode)
			{
				sqlRPC = null;
				bool flag3 = describeParameterEncryptionRpcOriginalRpcMap.TryGetValue(_sqlRPCParameterEncryptionReqArray[num2++], out sqlRPC);
			}
			else
			{
				sqlRPC = _rpcArrayOf1[0];
			}
			num7 = (BatchRPCMode ? 2 : 0);
			if (!flag || ds.NextResult())
			{
				while (ds.Read())
				{
					string string3 = ds.GetString(1);
					for (num6 = num7; num6 < sqlRPC.parameters.Length && sqlRPC.parameters[num6] != null; num6++)
					{
						SqlParameter sqlParameter = sqlRPC.parameters[num6];
						if (sqlParameter.ParameterNameFixed.Equals(string3, StringComparison.Ordinal))
						{
							sqlParameter.HasReceivedMetadata = true;
							byte @byte = ds.GetByte(3);
							if (@byte == 0)
							{
								break;
							}
							byte byte2 = ds.GetByte(2);
							int @int = ds.GetInt32(4);
							byte byte3 = ds.GetByte(5);
							if (!dictionary.TryGetValue(@int, out value))
							{
								throw SQL.InvalidEncryptionKeyOrdinalParameterMetadata(@int, dictionary.Count);
							}
							sqlParameter.CipherMetadata = new SqlCipherMetadata(value, ushort.MaxValue, byte2, null, @byte, byte3);
							SqlSecurityUtility.DecryptSymmetricKey(sqlParameter.CipherMetadata, _activeConnection, this);
							sqlRPC.paramoptions[num6] |= 8;
							break;
						}
					}
				}
			}
			for (num6 = num7; num6 < sqlRPC.parameters.Length && sqlRPC.parameters[num6] != null; num6++)
			{
				if (!sqlRPC.parameters[num6].HasReceivedMetadata && sqlRPC.parameters[num6].Direction != ParameterDirection.ReturnValue)
				{
					throw SQL.ParamEncryptionMetadataMissing(sqlRPC.parameters[num6].ParameterName, sqlRPC.GetCommandTextOrRpcName());
				}
			}
			if (ShouldUseEnclaveBasedWorkflow && enclaveAttestationParameters != null && requiresEnclaveComputations)
			{
				if (!ds.NextResult())
				{
					throw SQL.UnexpectedDescribeParamFormatAttestationInfo(_activeConnection.Parser.EnclaveType);
				}
				bool flag4 = false;
				while (ds.Read())
				{
					if (flag4)
					{
						throw SQL.MultipleRowsReturnedForAttestationInfo();
					}
					int num8 = (int)ds.GetBytes(0, 0L, null, 0, 0);
					byte[] array3 = new byte[num8];
					ds.GetBytes(0, 0L, array3, 0, num8);
					SqlConnectionAttestationProtocol attestationProtocol = _activeConnection.AttestationProtocol;
					string enclaveType = _activeConnection.Parser.EnclaveType;
					EnclaveSessionParameters enclaveSessionParameters = new EnclaveSessionParameters(_activeConnection.DataSource, _activeConnection.EnclaveAttestationUrl, _activeConnection.Database);
					EnclaveDelegate.Instance.CreateEnclaveSession(attestationProtocol, enclaveType, enclaveSessionParameters, array3, enclaveAttestationParameters, customData, customDataLength);
					enclaveAttestationParameters = null;
					flag4 = true;
				}
				if (!flag4)
				{
					throw SQL.AttestationInfoNotReturnedFromSqlServer(_activeConnection.Parser.EnclaveType, _activeConnection.EnclaveAttestationUrl);
				}
			}
			sqlRPC.needsFetchParameterEncryptionMetadata = false;
			if (!ds.NextResult())
			{
				break;
			}
		}
		if (BatchRPCMode)
		{
			for (int i = 0; i < _SqlRPCBatchArray.Length; i++)
			{
				if (_SqlRPCBatchArray[i].needsFetchParameterEncryptionMetadata)
				{
					throw SQL.ProcEncryptionMetadataMissing(_SqlRPCBatchArray[i].rpcName);
				}
			}
		}
		if (!BatchRPCMode && ShouldCacheEncryptionMetadata && _parameters != null && _parameters.Count > 0)
		{
			SqlQueryMetadataCache.GetInstance().AddQueryMetadata(this, ignoreQueriesWithReturnValueParams: true);
		}
	}

	internal SqlDataReader RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, string method)
	{
		Task task;
		bool usedCache;
		return RunExecuteReader(cmdBehavior, runBehavior, returnStream, method, null, CommandTimeout, out task, out usedCache);
	}

	internal SqlDataReader RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, string method, TaskCompletionSource<object> completion, int timeout, out Task task, out bool usedCache, bool asyncWrite = false, bool inRetry = false)
	{
		bool flag = completion != null;
		usedCache = false;
		task = null;
		_rowsAffected = -1;
		_rowsAffectedBySpDescribeParameterEncryption = -1;
		if ((CommandBehavior.SingleRow & cmdBehavior) != 0)
		{
			cmdBehavior |= CommandBehavior.SingleResult;
		}
		if (!inRetry)
		{
			ValidateCommand(method, flag);
		}
		CheckNotificationStateAndAutoEnlist();
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			SqlStatistics statistics = Statistics;
			if (statistics != null)
			{
				if ((!IsDirty && IsPrepared && !_hiddenPrepare) || (IsPrepared && _execType == EXECTYPE.PREPAREPENDING))
				{
					statistics.SafeIncrement(ref statistics._preparedExecs);
				}
				else
				{
					statistics.SafeIncrement(ref statistics._unpreparedExecs);
				}
			}
			ResetEncryptionState();
			if (_activeConnection.IsContextConnection)
			{
				return RunExecuteReaderSmi(cmdBehavior, runBehavior, returnStream);
			}
			if (IsColumnEncryptionEnabled)
			{
				Task returnTask = null;
				PrepareForTransparentEncryption(cmdBehavior, returnStream, flag, timeout, completion, out returnTask, asyncWrite && flag, out usedCache, inRetry);
				long start = ADP.TimerCurrent();
				try
				{
					return RunExecuteReaderTdsWithTransparentParameterEncryption(cmdBehavior, runBehavior, returnStream, flag, timeout, out task, asyncWrite && flag, inRetry, null, describeParameterEncryptionRequest: false, returnTask);
				}
				catch (EnclaveDelegate.RetryableEnclaveQueryExecutionException)
				{
					if (inRetry || flag)
					{
						throw;
					}
					SqlQueryMetadataCache.GetInstance().InvalidateCacheEntry(this);
					if (ShouldUseEnclaveBasedWorkflow && enclavePackage != null)
					{
						EnclaveSessionParameters enclaveSessionParameters = new EnclaveSessionParameters(_activeConnection.DataSource, _activeConnection.EnclaveAttestationUrl, _activeConnection.Database);
						EnclaveDelegate.Instance.InvalidateEnclaveSession(_activeConnection.AttestationProtocol, _activeConnection.Parser.EnclaveType, enclaveSessionParameters, enclavePackage.EnclaveSession);
					}
					return RunExecuteReader(cmdBehavior, runBehavior, returnStream, method, null, TdsParserStaticMethods.GetRemainingTimeout(timeout, start), out task, out usedCache, flag, inRetry: true);
				}
				catch (SqlException ex2)
				{
					if (inRetry || flag || (!usedCache && !ShouldUseEnclaveBasedWorkflow))
					{
						throw;
					}
					bool flag2 = false;
					for (int i = 0; i < ex2.Errors.Count; i++)
					{
						if ((usedCache && ex2.Errors[i].Number == 33514) || (ShouldUseEnclaveBasedWorkflow && ex2.Errors[i].Number == 33195))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						throw;
					}
					SqlQueryMetadataCache.GetInstance().InvalidateCacheEntry(this);
					if (ShouldUseEnclaveBasedWorkflow && enclavePackage != null)
					{
						EnclaveSessionParameters enclaveSessionParameters2 = new EnclaveSessionParameters(_activeConnection.DataSource, _activeConnection.EnclaveAttestationUrl, _activeConnection.Database);
						EnclaveDelegate.Instance.InvalidateEnclaveSession(_activeConnection.AttestationProtocol, _activeConnection.Parser.EnclaveType, enclaveSessionParameters2, enclavePackage.EnclaveSession);
					}
					return RunExecuteReader(cmdBehavior, runBehavior, returnStream, method, null, TdsParserStaticMethods.GetRemainingTimeout(timeout, start), out task, out usedCache, flag, inRetry: true);
				}
			}
			return RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, flag, timeout, out task, asyncWrite && flag, inRetry);
		}
		catch (OutOfMemoryException e)
		{
			_activeConnection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_activeConnection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
	}

	private SqlDataReader RunExecuteReaderTdsWithTransparentParameterEncryption(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, bool async, int timeout, out Task task, bool asyncWrite, bool inRetry, SqlDataReader ds = null, bool describeParameterEncryptionRequest = false, Task describeParameterEncryptionTask = null)
	{
		if (ds == null && returnStream)
		{
			ds = new SqlDataReader(this, cmdBehavior);
		}
		if (describeParameterEncryptionTask != null)
		{
			long parameterEncryptionStart = ADP.TimerCurrent();
			TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
			AsyncHelper.ContinueTask(describeParameterEncryptionTask, completion, delegate
			{
				Task task2 = null;
				GenerateEnclavePackage();
				RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, async, TdsParserStaticMethods.GetRemainingTimeout(timeout, parameterEncryptionStart), out task2, asyncWrite, inRetry, ds);
				if (task2 == null)
				{
					completion.SetResult(null);
				}
				else
				{
					AsyncHelper.ContinueTask(task2, completion, delegate
					{
						completion.SetResult(null);
					});
				}
			}, null, delegate(Exception exception)
			{
				if (_cachedAsyncState != null)
				{
					_cachedAsyncState.ResetAsyncState();
				}
				if (exception != null)
				{
					throw exception;
				}
			}, delegate
			{
				if (_cachedAsyncState != null)
				{
					_cachedAsyncState.ResetAsyncState();
				}
			}, null, _activeConnection);
			task = completion.Task;
			return ds;
		}
		GenerateEnclavePackage();
		return RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, async, timeout, out task, asyncWrite, inRetry, ds);
	}

	private void GenerateEnclavePackage()
	{
		if (keysToBeSentToEnclave == null || keysToBeSentToEnclave.Count <= 0)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_activeConnection.EnclaveAttestationUrl))
		{
			throw SQL.NoAttestationUrlSpecifiedForEnclaveBasedQueryGeneratingEnclavePackage(_activeConnection.Parser.EnclaveType);
		}
		string enclaveType = _activeConnection.Parser.EnclaveType;
		if (string.IsNullOrWhiteSpace(enclaveType))
		{
			throw SQL.EnclaveTypeNullForEnclaveBasedQuery();
		}
		SqlConnectionAttestationProtocol attestationProtocol = _activeConnection.AttestationProtocol;
		if (attestationProtocol == SqlConnectionAttestationProtocol.NotSpecified)
		{
			throw SQL.AttestationProtocolNotSpecifiedForGeneratingEnclavePackage();
		}
		try
		{
			EnclaveSessionParameters enclaveSessionParameters = new EnclaveSessionParameters(_activeConnection.DataSource, _activeConnection.EnclaveAttestationUrl, _activeConnection.Database);
			enclavePackage = EnclaveDelegate.Instance.GenerateEnclavePackage(attestationProtocol, keysToBeSentToEnclave, CommandText, enclaveType, enclaveSessionParameters, _activeConnection, this);
		}
		catch (EnclaveDelegate.RetryableEnclaveQueryExecutionException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw SQL.ExceptionWhenGeneratingEnclavePackage(innerException);
		}
	}

	private SqlDataReader RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, bool async, int timeout, out Task task, bool asyncWrite, bool inRetry, SqlDataReader ds = null, bool describeParameterEncryptionRequest = false)
	{
		if (ds == null && returnStream)
		{
			ds = new SqlDataReader(this, cmdBehavior);
		}
		Task task2 = _activeConnection.ValidateAndReconnect(null, timeout);
		if (task2 != null)
		{
			long reconnectionStart = ADP.TimerCurrent();
			if (async)
			{
				TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
				_activeConnection.RegisterWaitingForReconnect(completion.Task);
				_reconnectionCompletionSource = completion;
				CancellationTokenSource timeoutCTS = new CancellationTokenSource();
				AsyncHelper.SetTimeoutException(completion, timeout, SQL.CR_ReconnectTimeout, timeoutCTS.Token);
				AsyncHelper.ContinueTask(task2, completion, delegate
				{
					if (!completion.Task.IsCompleted)
					{
						Interlocked.CompareExchange(ref _reconnectionCompletionSource, null, completion);
						timeoutCTS.Cancel();
						RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, async, TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart), out var task5, asyncWrite, inRetry, ds);
						if (task5 == null)
						{
							completion.SetResult(null);
						}
						else
						{
							AsyncHelper.ContinueTask(task5, completion, delegate
							{
								completion.SetResult(null);
							});
						}
					}
				}, null, null, null, null, _activeConnection);
				task = completion.Task;
				return ds;
			}
			AsyncHelper.WaitForCompletion(task2, timeout, delegate
			{
				throw SQL.CR_ReconnectTimeout();
			});
			timeout = TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart);
		}
		bool inSchema = (cmdBehavior & CommandBehavior.SchemaOnly) != 0;
		_SqlRPC rpc = null;
		task = null;
		string optionSettings = null;
		bool flag = true;
		bool flag2 = false;
		if (async && !inRetry)
		{
			_activeConnection.GetOpenTdsConnection().IncrementAsyncCount();
			flag2 = true;
		}
		try
		{
			if (asyncWrite)
			{
				_activeConnection.AddWeakReference(this, 2);
			}
			GetStateObject();
			Task task3 = null;
			if (describeParameterEncryptionRequest)
			{
				task3 = _stateObj.Parser.TdsExecuteRPC(this, _sqlRPCParameterEncryptionReqArray, timeout, inSchema, Notification, _stateObj, CommandType.StoredProcedure == CommandType, !asyncWrite);
			}
			else if (BatchRPCMode)
			{
				task3 = _stateObj.Parser.TdsExecuteRPC(this, _SqlRPCBatchArray, timeout, inSchema, Notification, _stateObj, CommandType.StoredProcedure == CommandType, !asyncWrite);
			}
			else if (CommandType.Text == CommandType && GetParameterCount(_parameters) == 0)
			{
				if (returnStream)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.ExecuteReader|INFO> {0}, Command executed as SQLBATCH.", ObjectID);
				}
				string text = GetCommandText(cmdBehavior) + GetResetOptionsString(cmdBehavior);
				if (requiresEnclaveComputations)
				{
					if (enclavePackage == null)
					{
						throw SQL.NullEnclavePackageForEnclaveBasedQuery(_activeConnection.Parser.EnclaveType, _activeConnection.EnclaveAttestationUrl);
					}
					task3 = _stateObj.Parser.TdsExecuteSQLBatch(text, timeout, Notification, _stateObj, !asyncWrite, callerHasConnectionLock: false, enclavePackage.EnclavePackageBytes);
				}
				else
				{
					task3 = _stateObj.Parser.TdsExecuteSQLBatch(text, timeout, Notification, _stateObj, !asyncWrite);
				}
			}
			else if (CommandType.Text == CommandType)
			{
				if (IsDirty)
				{
					if (_execType == EXECTYPE.PREPARED)
					{
						_hiddenPrepare = true;
					}
					Unprepare();
					IsDirty = false;
				}
				if (_execType == EXECTYPE.PREPARED)
				{
					rpc = BuildExecute(inSchema);
				}
				else if (_execType == EXECTYPE.PREPAREPENDING)
				{
					rpc = BuildPrepExec(cmdBehavior);
					_execType = EXECTYPE.PREPARED;
					_preparedConnectionCloseCount = _activeConnection.CloseCount;
					_preparedConnectionReconnectCount = _activeConnection.ReconnectCount;
					_inPrepare = true;
				}
				else
				{
					BuildExecuteSql(cmdBehavior, null, _parameters, ref rpc);
				}
				if (_activeConnection.IsShiloh)
				{
					rpc.options = 2;
				}
				if (returnStream)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.ExecuteReader|INFO> {0}, Command executed as RPC.", ObjectID);
				}
				task3 = _stateObj.Parser.TdsExecuteRPC(this, _rpcArrayOf1, timeout, inSchema, Notification, _stateObj, CommandType.StoredProcedure == CommandType, !asyncWrite);
			}
			else
			{
				BuildRPC(inSchema, _parameters, ref rpc);
				optionSettings = GetSetOptionsString(cmdBehavior);
				if (returnStream)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.ExecuteReader|INFO> {0}, Command executed as RPC.", ObjectID);
				}
				if (optionSettings != null)
				{
					Task task4 = _stateObj.Parser.TdsExecuteSQLBatch(optionSettings, timeout, Notification, _stateObj, sync: true);
					if (!_stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, _stateObj, out var _))
					{
						throw SQL.SynchronousCallMayNotPend();
					}
					optionSettings = GetResetOptionsString(cmdBehavior);
				}
				_activeConnection.CheckSQLDebug();
				task3 = _stateObj.Parser.TdsExecuteRPC(this, _rpcArrayOf1, timeout, inSchema, Notification, _stateObj, CommandType.StoredProcedure == CommandType, !asyncWrite);
			}
			if (async)
			{
				flag2 = false;
				if (task3 != null)
				{
					task = AsyncHelper.CreateContinuationTask(task3, delegate
					{
						_activeConnection.GetOpenTdsConnection();
						cachedAsyncState.SetAsyncReaderState(ds, runBehavior, optionSettings);
					}, null, delegate
					{
						_activeConnection.GetOpenTdsConnection().DecrementAsyncCount();
					});
				}
				else
				{
					cachedAsyncState.SetAsyncReaderState(ds, runBehavior, optionSettings);
				}
			}
			else
			{
				FinishExecuteReader(ds, runBehavior, optionSettings, isInternal: false, forDescribeParameterEncryption: false, !describeParameterEncryptionRequest);
			}
		}
		catch (Exception e)
		{
			flag = ADP.IsCatchableExceptionType(e);
			if (flag2 && _activeConnection.InnerConnection is SqlInternalConnectionTds sqlInternalConnectionTds)
			{
				sqlInternalConnectionTds.DecrementAsyncCount();
			}
			throw;
		}
		finally
		{
			if (flag && !async)
			{
				PutStateObject();
			}
		}
		return ds;
	}

	private SqlDataReader RunExecuteReaderSmi(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream)
	{
		SqlInternalConnectionSmi internalSmiConnection = InternalSmiConnection;
		SmiEventStream smiEventStream = null;
		SqlDataReader sqlDataReader = null;
		SmiRequestExecutor smiRequestExecutor = null;
		try
		{
			smiRequestExecutor = SetUpSmiRequest(internalSmiConnection);
			internalSmiConnection.GetCurrentTransactionPair(out var transactionId, out var transaction);
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlCommand.RunExecuteReaderSmi|ADV> {0}, innerConnection={1}, transactionId=0x{2}, commandBehavior={(int)cmdBehavior}.", ObjectID, internalSmiConnection.ObjectID, transactionId);
			smiEventStream = ((SmiContextFactory.Instance.NegotiatedSmiVersion < 210) ? smiRequestExecutor.Execute(internalSmiConnection.SmiConnection, transactionId, cmdBehavior, SmiExecuteType.Reader) : smiRequestExecutor.Execute(internalSmiConnection.SmiConnection, transactionId, transaction, cmdBehavior, SmiExecuteType.Reader));
			if ((runBehavior & RunBehavior.UntilDone) != 0)
			{
				while (smiEventStream.HasEvents)
				{
					smiEventStream.ProcessEvent(EventSink);
				}
				smiEventStream.Close(EventSink);
			}
			if (returnStream)
			{
				sqlDataReader = new SqlDataReaderSmi(smiEventStream, this, cmdBehavior, internalSmiConnection, EventSink, smiRequestExecutor);
				sqlDataReader.NextResult();
				_activeConnection.AddWeakReference(sqlDataReader, 1);
			}
			EventSink.ProcessMessagesAndThrow();
			return sqlDataReader;
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableOrSecurityExceptionType(e))
			{
				throw;
			}
			smiEventStream?.Close(EventSink);
			if (smiRequestExecutor != null)
			{
				smiRequestExecutor.Close(EventSink);
				EventSink.ProcessMessagesAndThrow(ignoreNonFatalMessages: true);
			}
			throw;
		}
	}

	private SqlDataReader CompleteAsyncExecuteReader(bool isInternal = false, bool forDescribeParameterEncryption = false)
	{
		SqlDataReader cachedAsyncReader = cachedAsyncState.CachedAsyncReader;
		bool flag = true;
		try
		{
			FinishExecuteReader(cachedAsyncReader, cachedAsyncState.CachedRunBehavior, cachedAsyncState.CachedSetOptions, isInternal, forDescribeParameterEncryption, !forDescribeParameterEncryption);
			return cachedAsyncReader;
		}
		catch (Exception e)
		{
			flag = ADP.IsCatchableExceptionType(e);
			throw;
		}
		finally
		{
			if (flag)
			{
				if (!isInternal)
				{
					cachedAsyncState.ResetAsyncState();
				}
				PutStateObject();
			}
		}
	}

	private void FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, string resetOptionsString, bool isInternal, bool forDescribeParameterEncryption, bool shouldCacheForAlwaysEncrypted = true)
	{
		if (!isInternal && !forDescribeParameterEncryption)
		{
			NotifyDependency();
			if (_internalEndExecuteInitiated)
			{
				return;
			}
		}
		if (runBehavior == RunBehavior.UntilDone)
		{
			try
			{
				if (!_stateObj.Parser.TryRun(RunBehavior.UntilDone, this, ds, null, _stateObj, out var _))
				{
					throw SQL.SynchronousCallMayNotPend();
				}
			}
			catch (Exception e)
			{
				if (ADP.IsCatchableExceptionType(e))
				{
					if (_inPrepare)
					{
						_inPrepare = false;
						IsDirty = true;
						_execType = EXECTYPE.PREPAREPENDING;
					}
					ds?.Close();
				}
				throw;
			}
		}
		if (ds == null)
		{
			return;
		}
		ds.Bind(_stateObj);
		_stateObj = null;
		ds.ResetOptionsString = resetOptionsString;
		_activeConnection.AddWeakReference(ds, 1);
		try
		{
			if (shouldCacheForAlwaysEncrypted)
			{
				_cachedMetaData = ds.MetaData;
			}
			else
			{
				_SqlMetaDataSet metaData = ds.MetaData;
			}
			ds.IsInitialized = true;
		}
		catch (Exception e2)
		{
			if (ADP.IsCatchableExceptionType(e2))
			{
				if (_inPrepare)
				{
					_inPrepare = false;
					IsDirty = true;
					_execType = EXECTYPE.PREPAREPENDING;
				}
				ds.Close();
			}
			throw;
		}
	}

	private void NotifyDependency()
	{
		if (_sqlDep != null)
		{
			_sqlDep.StartTimer(Notification);
		}
	}

	public SqlCommand Clone()
	{
		SqlCommand sqlCommand = new SqlCommand(this);
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlCommand.Clone|API> {0}, clone={1}", ObjectID, sqlCommand?.ObjectID);
		return sqlCommand;
	}

	object ICloneable.Clone()
	{
		return Clone();
	}

	private void RegisterForConnectionCloseNotification<T>(ref Task<T> outterTask)
	{
		SqlConnection activeConnection = _activeConnection;
		if (activeConnection == null)
		{
			throw ADP.ClosedConnectionError();
		}
		activeConnection.RegisterForConnectionCloseNotification(ref outterTask, this, 2);
	}

	private void ValidateCommand(string method, bool async)
	{
		if (_activeConnection == null)
		{
			throw ADP.ConnectionRequired(method);
		}
		SqlInternalConnectionTds sqlInternalConnectionTds = _activeConnection.InnerConnection as SqlInternalConnectionTds;
		if (((ColumnEncryptionSetting == SqlCommandColumnEncryptionSetting.UseConnectionSetting && _activeConnection.IsColumnEncryptionSettingEnabled) || ColumnEncryptionSetting == SqlCommandColumnEncryptionSetting.Enabled || ColumnEncryptionSetting == SqlCommandColumnEncryptionSetting.ResultSetOnly) && sqlInternalConnectionTds != null && sqlInternalConnectionTds.Parser != null && !sqlInternalConnectionTds.Parser.IsColumnEncryptionSupported)
		{
			throw SQL.TceNotSupported();
		}
		if (sqlInternalConnectionTds != null)
		{
			TdsParser parser = sqlInternalConnectionTds.Parser;
			if (parser == null || parser.State == TdsParserState.Closed)
			{
				throw ADP.OpenConnectionRequired(method, ConnectionState.Closed);
			}
			if (parser.State != TdsParserState.OpenLoggedIn)
			{
				throw ADP.OpenConnectionRequired(method, ConnectionState.Broken);
			}
		}
		else
		{
			if (_activeConnection.State == ConnectionState.Closed)
			{
				throw ADP.OpenConnectionRequired(method, ConnectionState.Closed);
			}
			if (_activeConnection.State == ConnectionState.Broken)
			{
				throw ADP.OpenConnectionRequired(method, ConnectionState.Broken);
			}
		}
		ValidateAsyncCommand();
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			_activeConnection.ValidateConnectionForExecute(method, this);
		}
		catch (OutOfMemoryException e)
		{
			_activeConnection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_activeConnection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		if (_transaction != null && _transaction.Connection == null)
		{
			_transaction = null;
		}
		if (_activeConnection.HasLocalTransactionFromAPI && _transaction == null)
		{
			throw ADP.TransactionRequired(method);
		}
		if (_transaction != null && _activeConnection != _transaction.Connection)
		{
			throw ADP.TransactionConnectionMismatch();
		}
		if (ADP.IsEmpty(CommandText))
		{
			throw ADP.CommandTextRequired(method);
		}
		if (Notification != null && !_activeConnection.IsYukonOrNewer)
		{
			throw SQL.NotificationsRequireYukon();
		}
		if (async && _activeConnection.IsContextConnection)
		{
			throw SQL.NotAvailableOnContextConnection();
		}
	}

	private void ValidateAsyncCommand()
	{
		if (cachedAsyncState.PendingAsyncOperation)
		{
			if (cachedAsyncState.IsActiveConnectionValid(_activeConnection))
			{
				throw SQL.PendingBeginXXXExists();
			}
			_stateObj = null;
			cachedAsyncState.ResetAsyncState();
		}
	}

	private void GetStateObject(TdsParser parser = null)
	{
		if (_pendingCancel)
		{
			_pendingCancel = false;
			throw SQL.OperationCancelled();
		}
		if (parser == null)
		{
			parser = _activeConnection.Parser;
			if (parser == null || parser.State == TdsParserState.Broken || parser.State == TdsParserState.Closed)
			{
				throw ADP.ClosedConnectionError();
			}
		}
		TdsParserStateObject session = parser.GetSession(this);
		session.StartSession(ObjectID);
		_stateObj = session;
		if (_pendingCancel)
		{
			_pendingCancel = false;
			throw SQL.OperationCancelled();
		}
	}

	private void ReliablePutStateObject()
	{
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_activeConnection);
			PutStateObject();
		}
		catch (OutOfMemoryException e)
		{
			_activeConnection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_activeConnection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_activeConnection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
	}

	private void PutStateObject()
	{
		TdsParserStateObject stateObj = _stateObj;
		_stateObj = null;
		stateObj?.CloseSession();
	}

	internal void OnDoneDescribeParameterEncryptionProc(TdsParserStateObject stateObj)
	{
		if (BatchRPCMode)
		{
			_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC].cumulativeRecordsAffected = _rowsAffected;
			_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC].recordsAffected = ((0 < _currentlyExecutingDescribeParameterEncryptionRPC && 0 <= _rowsAffected) ? (_rowsAffected - Math.Max(_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC - 1].cumulativeRecordsAffected, 0)) : _rowsAffected);
			_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC].errorsIndexStart = ((0 < _currentlyExecutingDescribeParameterEncryptionRPC) ? _sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC - 1].errorsIndexEnd : 0);
			_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC].errorsIndexEnd = stateObj.ErrorCount;
			_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC].errors = stateObj._errors;
			_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC].warningsIndexStart = ((0 < _currentlyExecutingDescribeParameterEncryptionRPC) ? _sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC - 1].warningsIndexEnd : 0);
			_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC].warningsIndexEnd = stateObj.WarningCount;
			_sqlRPCParameterEncryptionReqArray[_currentlyExecutingDescribeParameterEncryptionRPC].warnings = stateObj._warnings;
			_currentlyExecutingDescribeParameterEncryptionRPC++;
		}
	}

	internal void OnDoneProc()
	{
		if (BatchRPCMode)
		{
			_SqlRPCBatchArray[_currentlyExecutingBatch].cumulativeRecordsAffected = _rowsAffected;
			_SqlRPCBatchArray[_currentlyExecutingBatch].recordsAffected = ((0 < _currentlyExecutingBatch && 0 <= _rowsAffected) ? (_rowsAffected - Math.Max(_SqlRPCBatchArray[_currentlyExecutingBatch - 1].cumulativeRecordsAffected, 0)) : _rowsAffected);
			_SqlRPCBatchArray[_currentlyExecutingBatch].errorsIndexStart = ((0 < _currentlyExecutingBatch) ? _SqlRPCBatchArray[_currentlyExecutingBatch - 1].errorsIndexEnd : 0);
			_SqlRPCBatchArray[_currentlyExecutingBatch].errorsIndexEnd = _stateObj.ErrorCount;
			_SqlRPCBatchArray[_currentlyExecutingBatch].errors = _stateObj._errors;
			_SqlRPCBatchArray[_currentlyExecutingBatch].warningsIndexStart = ((0 < _currentlyExecutingBatch) ? _SqlRPCBatchArray[_currentlyExecutingBatch - 1].warningsIndexEnd : 0);
			_SqlRPCBatchArray[_currentlyExecutingBatch].warningsIndexEnd = _stateObj.WarningCount;
			_SqlRPCBatchArray[_currentlyExecutingBatch].warnings = _stateObj._warnings;
			_currentlyExecutingBatch++;
		}
	}

	internal void OnReturnStatus(int status)
	{
		if (_inPrepare || IsDescribeParameterEncryptionRPCCurrentlyInProgress)
		{
			return;
		}
		SqlParameterCollection sqlParameterCollection = _parameters;
		if (BatchRPCMode)
		{
			sqlParameterCollection = ((_parameterCollectionList.Count <= _currentlyExecutingBatch) ? null : _parameterCollectionList[_currentlyExecutingBatch]);
		}
		int parameterCount = GetParameterCount(sqlParameterCollection);
		for (int i = 0; i < parameterCount; i++)
		{
			SqlParameter sqlParameter = sqlParameterCollection[i];
			if (sqlParameter.Direction == ParameterDirection.ReturnValue)
			{
				object value = sqlParameter.Value;
				if (value != null && value.GetType() == typeof(SqlInt32))
				{
					sqlParameter.Value = new SqlInt32(status);
				}
				else
				{
					sqlParameter.Value = status;
				}
				if (!BatchRPCMode && CachingQueryMetadataPostponed && ShouldCacheEncryptionMetadata && _parameters != null && _parameters.Count > 0)
				{
					SqlQueryMetadataCache.GetInstance().AddQueryMetadata(this, ignoreQueriesWithReturnValueParams: false);
				}
				break;
			}
		}
	}

	internal void OnReturnValue(SqlReturnValue rec, TdsParserStateObject stateObj)
	{
		if (_inPrepare)
		{
			if (!rec.value.IsNull)
			{
				_prepareHandle = rec.value.Int32;
			}
			_inPrepare = false;
			return;
		}
		SqlParameterCollection currentParameterCollection = GetCurrentParameterCollection();
		int parameterCount = GetParameterCount(currentParameterCollection);
		SqlParameter parameterForOutputValueExtraction = GetParameterForOutputValueExtraction(currentParameterCollection, rec.parameter, parameterCount);
		if (parameterForOutputValueExtraction == null)
		{
			return;
		}
		if (rec.cipherMD != null && parameterForOutputValueExtraction.CipherMetadata != null && (parameterForOutputValueExtraction.Direction == ParameterDirection.Output || parameterForOutputValueExtraction.Direction == ParameterDirection.InputOutput || parameterForOutputValueExtraction.Direction == ParameterDirection.ReturnValue))
		{
			if (rec.tdsType != 165)
			{
				throw SQL.InvalidDataTypeForEncryptedParameter(parameterForOutputValueExtraction.ParameterNameFixed, rec.tdsType, 165);
			}
			TdsParser parser = _activeConnection.Parser;
			if (parser == null || parser.State == TdsParserState.Closed || parser.State == TdsParserState.Broken)
			{
				throw ADP.ClosedConnectionError();
			}
			if (!rec.value.IsNull)
			{
				try
				{
					rec.cipherMD.EncryptionInfo = parameterForOutputValueExtraction.CipherMetadata.EncryptionInfo;
					byte[] array = SqlSecurityUtility.DecryptWithKey(rec.value.ByteArray, rec.cipherMD, _activeConnection, this);
					if (array != null)
					{
						SqlBuffer sqlBuffer = new SqlBuffer();
						parser.DeserializeUnencryptedValue(sqlBuffer, array, rec, stateObj, rec.NormalizationRuleVersion);
						parameterForOutputValueExtraction.SetSqlBuffer(sqlBuffer);
					}
					return;
				}
				catch (Exception e)
				{
					throw SQL.ParamDecryptionFailed(parameterForOutputValueExtraction.ParameterNameFixed, null, e);
				}
			}
			SqlBuffer sqlBuffer2 = new SqlBuffer();
			TdsParser.GetNullSqlValue(sqlBuffer2, rec, SqlCommandColumnEncryptionSetting.Enabled, parser.Connection);
			parameterForOutputValueExtraction.SetSqlBuffer(sqlBuffer2);
			return;
		}
		object value = parameterForOutputValueExtraction.Value;
		if (SqlDbType.Udt == parameterForOutputValueExtraction.SqlDbType)
		{
			object obj = null;
			try
			{
				Connection.CheckGetExtendedUDTInfo(rec, fThrow: true);
				obj = ((!rec.value.IsNull) ? ((object)rec.value.ByteArray) : ((object)DBNull.Value));
				parameterForOutputValueExtraction.Value = Connection.GetUdtValue(obj, rec, returnDBNull: false);
				return;
			}
			catch (FileNotFoundException udtLoadError)
			{
				parameterForOutputValueExtraction.SetUdtLoadError(udtLoadError);
				return;
			}
			catch (FileLoadException udtLoadError2)
			{
				parameterForOutputValueExtraction.SetUdtLoadError(udtLoadError2);
				return;
			}
		}
		parameterForOutputValueExtraction.SetSqlBuffer(rec.value);
		MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(rec.type, rec.IsMultiValued);
		if (rec.type == SqlDbType.Decimal)
		{
			parameterForOutputValueExtraction.ScaleInternal = rec.scale;
			parameterForOutputValueExtraction.PrecisionInternal = rec.precision;
		}
		else if (metaTypeFromSqlDbType.IsVarTime)
		{
			parameterForOutputValueExtraction.ScaleInternal = rec.scale;
		}
		else if (rec.type == SqlDbType.Xml && parameterForOutputValueExtraction.Value is SqlCachedBuffer sqlCachedBuffer)
		{
			parameterForOutputValueExtraction.Value = sqlCachedBuffer.ToString();
		}
		if (rec.collation != null)
		{
			parameterForOutputValueExtraction.Collation = rec.collation;
		}
	}

	internal void OnParametersAvailableSmi(SmiParameterMetaData[] paramMetaData, ITypedGettersV3 parameterValues)
	{
		for (int i = 0; i < paramMetaData.Length; i++)
		{
			OnParameterAvailableSmi(paramMetaData[i], parameterValues, i);
		}
	}

	internal void OnParameterAvailableSmi(SmiParameterMetaData metaData, ITypedGettersV3 parameterValues, int ordinal)
	{
		if (ParameterDirection.Input == metaData.Direction)
		{
			return;
		}
		string paramName = null;
		if (ParameterDirection.ReturnValue != metaData.Direction)
		{
			paramName = metaData.Name;
		}
		SqlParameterCollection currentParameterCollection = GetCurrentParameterCollection();
		int parameterCount = GetParameterCount(currentParameterCollection);
		SqlParameter parameterForOutputValueExtraction = GetParameterForOutputValueExtraction(currentParameterCollection, paramName, parameterCount);
		if (parameterForOutputValueExtraction != null)
		{
			parameterForOutputValueExtraction.LocaleId = (int)metaData.LocaleId;
			parameterForOutputValueExtraction.CompareInfo = metaData.CompareOptions;
			SqlBuffer sqlBuffer = new SqlBuffer();
			object obj = ((!_activeConnection.IsKatmaiOrNewer) ? ValueUtilsSmi.GetOutputParameterV3Smi(OutParamEventSink, parameterValues, ordinal, metaData, _smiRequestContext, sqlBuffer) : ValueUtilsSmi.GetOutputParameterV200Smi(OutParamEventSink, (SmiTypedGetterSetter)parameterValues, ordinal, metaData, _smiRequestContext, sqlBuffer));
			if (obj != null)
			{
				parameterForOutputValueExtraction.Value = obj;
			}
			else
			{
				parameterForOutputValueExtraction.SetSqlBuffer(sqlBuffer);
			}
		}
	}

	private SqlParameterCollection GetCurrentParameterCollection()
	{
		if (BatchRPCMode)
		{
			if (_parameterCollectionList.Count > _currentlyExecutingBatch)
			{
				return _parameterCollectionList[_currentlyExecutingBatch];
			}
			return null;
		}
		return _parameters;
	}

	private SqlParameter GetParameterForOutputValueExtraction(SqlParameterCollection parameters, string paramName, int paramCount)
	{
		SqlParameter sqlParameter = null;
		bool flag = false;
		if (paramName == null)
		{
			for (int i = 0; i < paramCount; i++)
			{
				sqlParameter = parameters[i];
				if (sqlParameter.Direction == ParameterDirection.ReturnValue)
				{
					flag = true;
					break;
				}
			}
		}
		else
		{
			for (int j = 0; j < paramCount; j++)
			{
				sqlParameter = parameters[j];
				if (sqlParameter.Direction != ParameterDirection.Input && sqlParameter.Direction != ParameterDirection.ReturnValue && paramName == sqlParameter.ParameterNameFixed)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			return sqlParameter;
		}
		return null;
	}

	private void GetRPCObject(int paramCount, ref _SqlRPC rpc, bool forSpDescribeParameterEncryption = false)
	{
		if (rpc == null)
		{
			if (!forSpDescribeParameterEncryption)
			{
				if (_rpcArrayOf1 == null)
				{
					_rpcArrayOf1 = new _SqlRPC[1];
					_rpcArrayOf1[0] = new _SqlRPC();
				}
				rpc = _rpcArrayOf1[0];
			}
			else
			{
				if (_rpcForEncryption == null)
				{
					_rpcForEncryption = new _SqlRPC();
				}
				rpc = _rpcForEncryption;
			}
		}
		rpc.ProcID = 0;
		rpc.rpcName = null;
		rpc.options = 0;
		rpc.recordsAffected = null;
		rpc.cumulativeRecordsAffected = -1;
		rpc.errorsIndexStart = 0;
		rpc.errorsIndexEnd = 0;
		rpc.errors = null;
		rpc.warningsIndexStart = 0;
		rpc.warningsIndexEnd = 0;
		rpc.warnings = null;
		rpc.needsFetchParameterEncryptionMetadata = false;
		if (rpc.parameters == null || rpc.parameters.Length < paramCount)
		{
			rpc.parameters = new SqlParameter[paramCount];
		}
		else if (rpc.parameters.Length > paramCount)
		{
			rpc.parameters[paramCount] = null;
		}
		if (rpc.paramoptions == null || rpc.paramoptions.Length < paramCount)
		{
			rpc.paramoptions = new byte[paramCount];
			return;
		}
		for (int i = 0; i < paramCount; i++)
		{
			rpc.paramoptions[i] = 0;
		}
	}

	private void SetUpRPCParameters(_SqlRPC rpc, int startCount, bool inSchema, SqlParameterCollection parameters)
	{
		int parameterCount = GetParameterCount(parameters);
		int num = startCount;
		TdsParser parser = _activeConnection.Parser;
		bool isYukonOrNewer = parser.IsYukonOrNewer;
		for (int i = 0; i < parameterCount; i++)
		{
			SqlParameter sqlParameter = parameters[i];
			sqlParameter.Validate(i, CommandType.StoredProcedure == CommandType);
			if (!sqlParameter.ValidateTypeLengths(isYukonOrNewer).IsPlp && sqlParameter.Direction != ParameterDirection.Output)
			{
				sqlParameter.FixStreamDataForNonPLP();
			}
			if (!ShouldSendParameter(sqlParameter))
			{
				continue;
			}
			rpc.parameters[num] = sqlParameter;
			if (sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Output)
			{
				rpc.paramoptions[num] = 1;
			}
			if (sqlParameter.CipherMetadata != null)
			{
				rpc.paramoptions[num] |= 8;
			}
			if (sqlParameter.Direction != ParameterDirection.Output)
			{
				if (sqlParameter.Value == null && (!inSchema || SqlDbType.Structured == sqlParameter.SqlDbType))
				{
					rpc.paramoptions[num] |= 2;
				}
				if (sqlParameter.IsDerivedParameterTypeName)
				{
					string[] array = MultipartIdentifier.ParseMultipartIdentifier(sqlParameter.TypeName, "[\"", "]\"", Strings.SQL_TDSParserTableName, ThrowOnEmptyMultipartName: false);
					if (array != null && array.Length == 4 && array[3] != null && array[2] != null && array[1] != null)
					{
						sqlParameter.TypeName = QuoteIdentifier(array, 2, 2);
					}
				}
			}
			num++;
		}
	}

	private _SqlRPC BuildPrepExec(CommandBehavior behavior)
	{
		int num = 3;
		int num2 = CountSendableParameters(_parameters);
		_SqlRPC rpc = null;
		GetRPCObject(num2 + num, ref rpc);
		rpc.ProcID = 13;
		rpc.rpcName = "sp_prepexec";
		SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
		sqlParameter.Direction = ParameterDirection.InputOutput;
		sqlParameter.Value = _prepareHandle;
		rpc.parameters[0] = sqlParameter;
		rpc.paramoptions[0] = 1;
		string text = BuildParamList(_stateObj.Parser, _parameters);
		sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
		sqlParameter.Value = text;
		rpc.parameters[1] = sqlParameter;
		string commandText = GetCommandText(behavior);
		sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
		sqlParameter.Value = commandText;
		rpc.parameters[2] = sqlParameter;
		SetUpRPCParameters(rpc, num, inSchema: false, _parameters);
		return rpc;
	}

	private static bool ShouldSendParameter(SqlParameter p, bool includeReturnValue = false)
	{
		switch (p.Direction)
		{
		case ParameterDirection.ReturnValue:
			return includeReturnValue;
		case ParameterDirection.Input:
		case ParameterDirection.Output:
		case ParameterDirection.InputOutput:
			return true;
		default:
			return false;
		}
	}

	private int CountSendableParameters(SqlParameterCollection parameters)
	{
		int num = 0;
		if (parameters != null)
		{
			int count = parameters.Count;
			for (int i = 0; i < count; i++)
			{
				if (ShouldSendParameter(parameters[i]))
				{
					num++;
				}
			}
		}
		return num;
	}

	private int GetParameterCount(SqlParameterCollection parameters)
	{
		return parameters?.Count ?? 0;
	}

	private void BuildRPC(bool inSchema, SqlParameterCollection parameters, ref _SqlRPC rpc)
	{
		int paramCount = CountSendableParameters(parameters);
		GetRPCObject(paramCount, ref rpc);
		rpc.rpcName = CommandText;
		SetUpRPCParameters(rpc, 0, inSchema, parameters);
	}

	private _SqlRPC BuildUnprepare()
	{
		_SqlRPC rpc = null;
		GetRPCObject(1, ref rpc);
		rpc.ProcID = 15;
		rpc.rpcName = "sp_unprepare";
		SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
		sqlParameter.Value = _prepareHandle;
		rpc.parameters[0] = sqlParameter;
		return rpc;
	}

	private _SqlRPC BuildExecute(bool inSchema)
	{
		int num = 1;
		int num2 = CountSendableParameters(_parameters);
		_SqlRPC rpc = null;
		GetRPCObject(num2 + num, ref rpc);
		rpc.ProcID = 12;
		rpc.rpcName = "sp_execute";
		SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
		sqlParameter.Value = _prepareHandle;
		rpc.parameters[0] = sqlParameter;
		SetUpRPCParameters(rpc, num, inSchema, _parameters);
		return rpc;
	}

	private void BuildExecuteSql(CommandBehavior behavior, string commandText, SqlParameterCollection parameters, ref _SqlRPC rpc)
	{
		int num = CountSendableParameters(parameters);
		int num2 = ((num <= 0) ? 1 : 2);
		GetRPCObject(num + num2, ref rpc);
		rpc.ProcID = 10;
		rpc.rpcName = "sp_executesql";
		if (commandText == null)
		{
			commandText = GetCommandText(behavior);
		}
		SqlParameter sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
		sqlParameter.Value = commandText;
		rpc.parameters[0] = sqlParameter;
		if (num > 0)
		{
			string text = BuildParamList(_stateObj.Parser, BatchRPCMode ? parameters : _parameters);
			sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
			sqlParameter.Value = text;
			rpc.parameters[1] = sqlParameter;
			bool inSchema = (behavior & CommandBehavior.SchemaOnly) != 0;
			SetUpRPCParameters(rpc, num2, inSchema, parameters);
		}
	}

	private SqlParameter BuildStoredProcedureStatementForColumnEncryption(string storedProcedureName, SqlParameter[] parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("EXEC ");
		SqlParameter sqlParameter = null;
		foreach (SqlParameter sqlParameter2 in parameters)
		{
			if (sqlParameter2.Direction == ParameterDirection.ReturnValue)
			{
				sqlParameter = sqlParameter2;
				break;
			}
		}
		if (sqlParameter != null)
		{
			stringBuilder.AppendFormat("{0}=", sqlParameter.ParameterNameFixed);
		}
		stringBuilder.Append(ParseAndQuoteIdentifier(storedProcedureName, isUdtTypeName: false));
		int j = 0;
		if (parameters.Count() > 0)
		{
			for (; j < parameters.Count() && parameters[j].Direction == ParameterDirection.ReturnValue; j++)
			{
			}
			if (j < parameters.Count())
			{
				stringBuilder.AppendFormat(" {0}={0}", parameters[j].ParameterNameFixed);
				if (parameters[j].Direction == ParameterDirection.Output || parameters[j].Direction == ParameterDirection.InputOutput)
				{
					stringBuilder.AppendFormat(" OUTPUT");
				}
			}
		}
		for (j++; j < parameters.Count(); j++)
		{
			if (parameters[j].Direction != ParameterDirection.ReturnValue)
			{
				stringBuilder.AppendFormat(", {0}={0}", parameters[j].ParameterNameFixed);
				if (parameters[j].Direction == ParameterDirection.Output || parameters[j].Direction == ParameterDirection.InputOutput)
				{
					stringBuilder.AppendFormat(" OUTPUT");
				}
			}
		}
		SqlParameter sqlParameter3 = new SqlParameter(null, (stringBuilder.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, stringBuilder.Length);
		sqlParameter3.Value = stringBuilder.ToString();
		return sqlParameter3;
	}

	internal string BuildParamList(TdsParser parser, SqlParameterCollection parameters, bool includeReturnValue = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		bool isYukonOrNewer = parser.IsYukonOrNewer;
		int num = 0;
		num = parameters.Count;
		for (int i = 0; i < num; i++)
		{
			SqlParameter sqlParameter = parameters[i];
			sqlParameter.Validate(i, CommandType.StoredProcedure == CommandType);
			if (!ShouldSendParameter(sqlParameter, includeReturnValue))
			{
				continue;
			}
			if (flag)
			{
				stringBuilder.Append(',');
			}
			stringBuilder.Append(sqlParameter.ParameterNameFixed);
			MetaType metaType = sqlParameter.InternalMetaType;
			stringBuilder.Append(" ");
			if (metaType.SqlDbType == SqlDbType.Udt)
			{
				string udtTypeName = sqlParameter.UdtTypeName;
				if (ADP.IsEmpty(udtTypeName))
				{
					throw SQL.MustSetUdtTypeNameForUdtParams();
				}
				stringBuilder.Append(ParseAndQuoteIdentifier(udtTypeName, isUdtTypeName: true));
			}
			else if (metaType.SqlDbType == SqlDbType.Structured)
			{
				string typeName = sqlParameter.TypeName;
				if (ADP.IsEmpty(typeName))
				{
					throw SQL.MustSetTypeNameForParam(metaType.TypeName, sqlParameter.ParameterNameFixed);
				}
				stringBuilder.Append(ParseAndQuoteIdentifier(typeName, isUdtTypeName: false));
				stringBuilder.Append(" READONLY");
			}
			else
			{
				metaType = sqlParameter.ValidateTypeLengths(isYukonOrNewer);
				if (!metaType.IsPlp && sqlParameter.Direction != ParameterDirection.Output)
				{
					sqlParameter.FixStreamDataForNonPLP();
				}
				stringBuilder.Append(metaType.TypeName);
			}
			flag = true;
			if (metaType.SqlDbType == SqlDbType.Decimal)
			{
				byte b = sqlParameter.GetActualPrecision();
				byte actualScale = sqlParameter.GetActualScale();
				stringBuilder.Append('(');
				if (b == 0)
				{
					b = (byte)((!IsShiloh) ? 28 : 29);
				}
				stringBuilder.Append(b);
				stringBuilder.Append(',');
				stringBuilder.Append(actualScale);
				stringBuilder.Append(')');
			}
			else if (metaType.IsVarTime)
			{
				byte actualScale2 = sqlParameter.GetActualScale();
				stringBuilder.Append('(');
				stringBuilder.Append(actualScale2);
				stringBuilder.Append(')');
			}
			else if (!metaType.IsFixed && !metaType.IsLong && metaType.SqlDbType != SqlDbType.Timestamp && metaType.SqlDbType != SqlDbType.Udt && SqlDbType.Structured != metaType.SqlDbType)
			{
				int num2 = sqlParameter.Size;
				stringBuilder.Append('(');
				if (metaType.IsAnsiType)
				{
					object coercedValue = sqlParameter.GetCoercedValue();
					string text = null;
					if (coercedValue != null && DBNull.Value != coercedValue)
					{
						text = coercedValue as string;
						if (text == null)
						{
							SqlString sqlString = ((coercedValue is SqlString) ? ((SqlString)coercedValue) : SqlString.Null);
							if (!sqlString.IsNull)
							{
								text = sqlString.Value;
							}
						}
					}
					if (text != null)
					{
						int encodingCharLength = parser.GetEncodingCharLength(text, sqlParameter.GetActualSize(), sqlParameter.Offset, null);
						if (encodingCharLength > num2)
						{
							num2 = encodingCharLength;
						}
					}
				}
				if (num2 == 0)
				{
					num2 = (metaType.IsSizeInCharacters ? 4000 : 8000);
				}
				stringBuilder.Append(num2);
				stringBuilder.Append(')');
			}
			else if (metaType.IsPlp && metaType.SqlDbType != SqlDbType.Xml && metaType.SqlDbType != SqlDbType.Udt)
			{
				stringBuilder.Append("(max) ");
			}
			if (sqlParameter.Direction != ParameterDirection.Input)
			{
				stringBuilder.Append(" output");
			}
		}
		return stringBuilder.ToString();
	}

	private string ParseAndQuoteIdentifier(string identifier, bool isUdtTypeName)
	{
		string[] strings = SqlParameter.ParseTypeName(identifier, isUdtTypeName);
		return ADP.BuildMultiPartName(strings);
	}

	private static string QuoteIdentifier(string[] strings, int offset, int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = offset; i < offset + length; i++)
		{
			if (0 < stringBuilder.Length)
			{
				stringBuilder.Append('.');
			}
			if (strings[i] != null && strings[i].Length != 0)
			{
				ADP.AppendQuotedString(stringBuilder, "[", "]", strings[i]);
			}
		}
		return stringBuilder.ToString();
	}

	private string GetSetOptionsString(CommandBehavior behavior)
	{
		string text = null;
		if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly) || CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
		{
			text = " SET FMTONLY OFF;";
			if (CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
			{
				text += " SET NO_BROWSETABLE ON;";
			}
			if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly))
			{
				text += " SET FMTONLY ON;";
			}
		}
		return text;
	}

	private string GetResetOptionsString(CommandBehavior behavior)
	{
		string text = null;
		if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly))
		{
			text += " SET FMTONLY OFF;";
		}
		if (CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
		{
			text += " SET NO_BROWSETABLE OFF;";
		}
		return text;
	}

	private string GetCommandText(CommandBehavior behavior)
	{
		return GetSetOptionsString(behavior) + CommandText;
	}

	private _SqlRPC BuildPrepare(CommandBehavior behavior)
	{
		_SqlRPC rpc = null;
		GetRPCObject(3, ref rpc);
		rpc.ProcID = 11;
		rpc.rpcName = "sp_prepare";
		SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
		sqlParameter.Direction = ParameterDirection.Output;
		rpc.parameters[0] = sqlParameter;
		rpc.paramoptions[0] = 1;
		string text = BuildParamList(_stateObj.Parser, _parameters);
		sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
		sqlParameter.Value = text;
		rpc.parameters[1] = sqlParameter;
		string commandText = GetCommandText(behavior);
		sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
		sqlParameter.Value = commandText;
		rpc.parameters[2] = sqlParameter;
		return rpc;
	}

	internal void CheckThrowSNIException()
	{
		_stateObj?.CheckThrowSNIException();
	}

	internal void OnConnectionClosed()
	{
		_stateObj?.OnConnectionClosed();
	}

	private void ClearDescribeParameterEncryptionRequests()
	{
		_sqlRPCParameterEncryptionReqArray = null;
		_currentlyExecutingDescribeParameterEncryptionRPC = 0;
		IsDescribeParameterEncryptionRPCCurrentlyInProgress = false;
		_rowsAffectedBySpDescribeParameterEncryption = -1;
	}

	internal void ClearBatchCommand()
	{
		_RPCList?.Clear();
		if (_parameterCollectionList != null)
		{
			_parameterCollectionList.Clear();
		}
		_SqlRPCBatchArray = null;
		_currentlyExecutingBatch = 0;
	}

	private void SetColumnEncryptionSetting(SqlCommandColumnEncryptionSetting newColumnEncryptionSetting)
	{
		if (!_wasBatchModeColumnEncryptionSettingSetOnce)
		{
			_columnEncryptionSetting = newColumnEncryptionSetting;
			_wasBatchModeColumnEncryptionSettingSetOnce = true;
		}
		else if (_columnEncryptionSetting != newColumnEncryptionSetting)
		{
			throw SQL.BatchedUpdateColumnEncryptionSettingMismatch();
		}
	}

	internal void AddBatchCommand(string commandText, SqlParameterCollection parameters, CommandType cmdType, SqlCommandColumnEncryptionSetting columnEncryptionSetting)
	{
		_SqlRPC rpc = new _SqlRPC();
		CommandText = commandText;
		CommandType = cmdType;
		SetColumnEncryptionSetting(columnEncryptionSetting);
		GetStateObject();
		if (cmdType == CommandType.StoredProcedure)
		{
			BuildRPC(inSchema: false, parameters, ref rpc);
		}
		else
		{
			BuildExecuteSql(CommandBehavior.Default, commandText, parameters, ref rpc);
		}
		_RPCList.Add(rpc);
		_parameterCollectionList.Add(parameters);
		ReliablePutStateObject();
	}

	internal int ExecuteBatchRPCCommand()
	{
		_SqlRPCBatchArray = _RPCList.ToArray();
		_currentlyExecutingBatch = 0;
		return ExecuteNonQuery();
	}

	internal int? GetRecordsAffected(int commandIndex)
	{
		return _SqlRPCBatchArray[commandIndex].recordsAffected;
	}

	internal SqlException GetErrors(int commandIndex)
	{
		SqlException result = null;
		int num = _SqlRPCBatchArray[commandIndex].errorsIndexEnd - _SqlRPCBatchArray[commandIndex].errorsIndexStart;
		if (0 < num)
		{
			SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
			for (int i = _SqlRPCBatchArray[commandIndex].errorsIndexStart; i < _SqlRPCBatchArray[commandIndex].errorsIndexEnd; i++)
			{
				sqlErrorCollection.Add(_SqlRPCBatchArray[commandIndex].errors[i]);
			}
			for (int j = _SqlRPCBatchArray[commandIndex].warningsIndexStart; j < _SqlRPCBatchArray[commandIndex].warningsIndexEnd; j++)
			{
				sqlErrorCollection.Add(_SqlRPCBatchArray[commandIndex].warnings[j]);
			}
			result = SqlException.CreateException(sqlErrorCollection, Connection.ServerVersion, Connection.ClientConnectionId);
		}
		return result;
	}

	private SmiRequestExecutor SetUpSmiRequest(SqlInternalConnectionSmi innerConnection)
	{
		if (Notification != null)
		{
			throw SQL.NotificationsNotAvailableOnContextConnection();
		}
		SmiParameterMetaData[] array = null;
		ParameterPeekAheadValue[] array2 = null;
		int parameterCount = GetParameterCount(Parameters);
		if (0 < parameterCount)
		{
			array = new SmiParameterMetaData[parameterCount];
			array2 = new ParameterPeekAheadValue[parameterCount];
			for (int i = 0; i < parameterCount; i++)
			{
				SqlParameter sqlParameter = Parameters[i];
				sqlParameter.Validate(i, CommandType.StoredProcedure == CommandType);
				array[i] = sqlParameter.MetaDataForSmi(out array2[i]);
				if (!innerConnection.IsKatmaiOrNewer)
				{
					MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(array[i].SqlDbType, array[i].IsMultiValued);
					if (!metaTypeFromSqlDbType.Is90Supported)
					{
						throw ADP.VersionDoesNotSupportDataType(metaTypeFromSqlDbType.TypeName);
					}
				}
			}
		}
		CommandType commandType = CommandType;
		_smiRequestContext = innerConnection.InternalContext;
		SmiRequestExecutor smiRequestExecutor = _smiRequestContext.CreateRequestExecutor(CommandText, commandType, array, EventSink);
		EventSink.ProcessMessagesAndThrow();
		for (int j = 0; j < parameterCount; j++)
		{
			if (ParameterDirection.Output == array[j].Direction || ParameterDirection.ReturnValue == array[j].Direction)
			{
				continue;
			}
			SqlParameter sqlParameter2 = Parameters[j];
			object obj = sqlParameter2.GetCoercedValue();
			if (obj is XmlDataFeed && array[j].SqlDbType != SqlDbType.Xml)
			{
				obj = MetaType.GetStringFromXml(((XmlDataFeed)obj)._source);
			}
			ExtendedClrTypeCode extendedClrTypeCode = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(array[j].SqlDbType, array[j].IsMultiValued, obj, null, SmiContextFactory.Instance.NegotiatedSmiVersion);
			if (CommandType.StoredProcedure == commandType && ExtendedClrTypeCode.Empty == extendedClrTypeCode)
			{
				smiRequestExecutor.SetDefault(j);
				continue;
			}
			int size = sqlParameter2.Size;
			if (size != 0 && (long)size != -1 && !sqlParameter2.SizeInferred)
			{
				switch (array[j].SqlDbType)
				{
				case SqlDbType.Image:
				case SqlDbType.Text:
					if (size != int.MaxValue)
					{
						throw SQL.ParameterSizeRestrictionFailure(j);
					}
					break;
				case SqlDbType.NText:
					if (size != 1073741823)
					{
						throw SQL.ParameterSizeRestrictionFailure(j);
					}
					break;
				case SqlDbType.VarBinary:
				case SqlDbType.VarChar:
					if (size > 0 && size != int.MaxValue && array[j].MaxLength == -1)
					{
						throw SQL.ParameterSizeRestrictionFailure(j);
					}
					break;
				case SqlDbType.NVarChar:
					if (size > 0 && size != 1073741823 && array[j].MaxLength == -1)
					{
						throw SQL.ParameterSizeRestrictionFailure(j);
					}
					break;
				case SqlDbType.Timestamp:
					if (size < SmiMetaData.DefaultTimestamp.MaxLength)
					{
						throw SQL.ParameterSizeRestrictionFailure(j);
					}
					break;
				case SqlDbType.Variant:
					if (obj != null)
					{
						MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(obj);
						if ((metaTypeFromValue.IsNCharType && (long)size < 4000L) || (metaTypeFromValue.IsBinType && (long)size < 8000L) || (metaTypeFromValue.IsAnsiType && (long)size < 8000L))
						{
							throw SQL.ParameterSizeRestrictionFailure(j);
						}
					}
					break;
				case SqlDbType.Xml:
					if (obj != null && ExtendedClrTypeCode.SqlXml != extendedClrTypeCode)
					{
						throw SQL.ParameterSizeRestrictionFailure(j);
					}
					break;
				}
			}
			if (innerConnection.IsKatmaiOrNewer)
			{
				ValueUtilsSmi.SetCompatibleValueV200(EventSink, smiRequestExecutor, j, array[j], obj, extendedClrTypeCode, sqlParameter2.Offset, sqlParameter2.Size, array2[j]);
			}
			else
			{
				ValueUtilsSmi.SetCompatibleValue(EventSink, smiRequestExecutor, j, array[j], obj, extendedClrTypeCode, sqlParameter2.Offset);
			}
		}
		return smiRequestExecutor;
	}

	private void WriteBeginExecuteEvent()
	{
		SqlClientEventSource.Log.TryBeginExecuteEvent(ObjectID, Connection?.ClientConnectionId, CommandText, "WriteBeginExecuteEvent");
	}

	private void WriteEndExecuteEvent(bool success, int? sqlExceptionNumber, bool synchronous)
	{
		if (SqlClientEventSource.Log.IsExecutionTraceEnabled())
		{
			int num = (success ? 1 : 0);
			int num2 = (sqlExceptionNumber.HasValue ? 2 : 0);
			int num3 = (synchronous ? 4 : 0);
			int compositeState = num | num2 | num3;
			SqlClientEventSource.Log.TryEndExecuteEvent(ObjectID, Connection?.ClientConnectionId, compositeState, sqlExceptionNumber.GetValueOrDefault(), "WriteEndExecuteEvent");
		}
	}
}
