using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Transactions;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlInternalConnectionSmi : SqlInternalConnection
{
	private sealed class EventSink : SmiEventSink_Default
	{
		private SqlInternalConnectionSmi _connection;

		internal override string ServerVersion => SmiContextFactory.Instance.ServerVersion;

		protected override void DispatchMessages(bool ignoreNonFatalMessages)
		{
			SqlException ex = ProcessMessages(ignoreWarnings: false, ignoreNonFatalMessages);
			if (ex != null)
			{
				SqlConnection connection = _connection.Connection;
				if (connection != null && connection.FireInfoMessageEventOnUserErrors)
				{
					connection.OnInfoMessage(new SqlInfoMessageEventArgs(ex));
				}
				else
				{
					_connection.OnError(ex, breakConnection: false);
				}
			}
		}

		internal EventSink(SqlInternalConnectionSmi connection)
		{
			_connection = connection;
		}

		internal override void DefaultDatabaseChanged(string databaseName)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.EventSink.DefaultDatabaseChanged|ADV> {0}, databaseName='{1}'.", _connection.ObjectID, databaseName);
			_connection.CurrentDatabase = databaseName;
		}

		internal override void TransactionCommitted(long transactionId)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.EventSink.TransactionCommitted|ADV> {0}, transactionId=0x{1}.", _connection.ObjectID, transactionId);
			_connection.TransactionEnded(transactionId, TransactionState.Committed);
		}

		internal override void TransactionDefected(long transactionId)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.EventSink.TransactionDefected|ADV> {0}, transactionId=0x{1}.", _connection.ObjectID, transactionId);
			_connection.TransactionEnded(transactionId, TransactionState.Unknown);
		}

		internal override void TransactionEnlisted(long transactionId)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.EventSink.TransactionEnlisted|ADV> {0}, transactionId=0x{1}.", _connection.ObjectID, transactionId);
			_connection.TransactionStarted(transactionId, isDistributed: true);
		}

		internal override void TransactionEnded(long transactionId)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.EventSink.TransactionEnded|ADV> {0}, transactionId=0x{1}.", _connection.ObjectID, transactionId);
			_connection.TransactionEndedByServer(transactionId, TransactionState.Unknown);
		}

		internal override void TransactionRolledBack(long transactionId)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.EventSink.TransactionRolledBack|ADV> {0}, transactionId=0x{1}.", _connection.ObjectID, transactionId);
			_connection.TransactionEndedByServer(transactionId, TransactionState.Aborted);
		}

		internal override void TransactionStarted(long transactionId)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.EventSink.TransactionStarted|ADV> {0}, transactionId=0x{1}.", _connection.ObjectID, transactionId);
			_connection.TransactionStarted(transactionId, isDistributed: false);
		}
	}

	private SmiContext _smiContext;

	private SmiConnection _smiConnection;

	private SmiEventSink_Default _smiEventSink;

	private int _isInUse;

	private SqlInternalTransaction _pendingTransaction;

	private SqlInternalTransaction _currentTransaction;

	internal SmiContext InternalContext => _smiContext;

	internal SmiConnection SmiConnection => _smiConnection;

	internal SmiEventSink CurrentEventSink => _smiEventSink;

	internal override SqlInternalTransaction CurrentTransaction => _currentTransaction;

	internal override bool IsLockedForBulkCopy => false;

	internal override bool IsShiloh => false;

	internal override bool IsYukonOrNewer => true;

	internal override bool IsKatmaiOrNewer => SmiContextFactory.Instance.NegotiatedSmiVersion >= 210;

	internal override SqlInternalTransaction PendingTransaction => CurrentTransaction;

	public override string ServerVersion => SmiContextFactory.Instance.ServerVersion;

	protected override bool UnbindOnTransactionCompletion => base.ConnectionOptions.TransactionBinding == SqlConnectionString.TransactionBindingEnum.ImplicitUnbind;

	private Transaction ContextTransaction { get; set; }

	private Transaction InternalEnlistedTransaction
	{
		get
		{
			Transaction transaction = base.EnlistedTransaction;
			if (null == transaction)
			{
				transaction = ContextTransaction;
			}
			return transaction;
		}
	}

	internal SqlInternalConnectionSmi(SqlConnectionString connectionOptions, SmiContext smiContext)
		: base(connectionOptions)
	{
		_smiContext = smiContext;
		_smiContext.OutOfScope += OnOutOfScope;
		_smiConnection = _smiContext.ContextConnection;
		_smiEventSink = new EventSink(this);
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.ctor|ADV> {0}, constructed new SMI internal connection", base.ObjectID);
	}

	protected override void Activate(Transaction transaction)
	{
	}

	internal void Activate()
	{
		if (Interlocked.Exchange(ref _isInUse, 1) != 0)
		{
			throw SQL.ContextConnectionIsInUse();
		}
		base.CurrentDatabase = _smiConnection.GetCurrentDatabase(_smiEventSink);
		_smiEventSink.ProcessMessagesAndThrow();
	}

	internal void AutomaticEnlistment()
	{
		Transaction currentTransaction = ADP.GetCurrentTransaction();
		Transaction contextTransaction = _smiContext.ContextTransaction;
		long contextTransactionId = _smiContext.ContextTransactionId;
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.AutomaticEnlistment|ADV> {0}, contextTransactionId=0x{1}, contextTransaction={2}, currentSystemTransaction={3}.", base.ObjectID, contextTransactionId, (null != contextTransaction) ? contextTransaction.GetHashCode() : 0, (null != currentTransaction) ? currentTransaction.GetHashCode() : 0);
		if (contextTransactionId != 0L)
		{
			if (null != currentTransaction && contextTransaction != currentTransaction)
			{
				throw SQL.NestedTransactionScopesNotSupported();
			}
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlInternalConnectionSmi.AutomaticEnlistment|ADV> {0}, using context transaction with transactionId=0x{1}", base.ObjectID, contextTransactionId);
			_currentTransaction = new SqlInternalTransaction(this, TransactionType.Context, null, contextTransactionId);
			ContextTransaction = contextTransaction;
		}
		else if (null == currentTransaction)
		{
			_currentTransaction = null;
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.AutomaticEnlistment|ADV> {0}, no transaction.", base.ObjectID);
		}
		else
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.AutomaticEnlistment|ADV> {0}, using current System.Transaction.", base.ObjectID);
			Enlist(currentTransaction);
		}
	}

	protected override void ChangeDatabaseInternal(string database)
	{
		_smiConnection.SetCurrentDatabase(database, _smiEventSink);
		_smiEventSink.ProcessMessagesAndThrow();
	}

	protected override void InternalDeactivate()
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.Deactivate|ADV> {0}, Deactivating.", base.ObjectID);
		if (!IsNonPoolableTransactionRoot)
		{
			Enlist(null);
		}
		if (_currentTransaction != null)
		{
			if (_currentTransaction.IsContext)
			{
				_currentTransaction = null;
			}
			else if (_currentTransaction.IsLocal)
			{
				_currentTransaction.CloseFromConnection();
			}
		}
		ContextTransaction = null;
		_isInUse = 0;
	}

	internal override void DelegatedTransactionEnded()
	{
		base.DelegatedTransactionEnded();
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.DelegatedTransactionEnded|ADV> {0}, cleaning up after Delegated Transaction Completion", base.ObjectID);
		_currentTransaction = null;
	}

	internal override void DisconnectTransaction(SqlInternalTransaction internalTransaction)
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.DisconnectTransaction|ADV> {0}, Disconnecting Transaction {1}.", base.ObjectID, internalTransaction.ObjectID);
		if (_currentTransaction != null && _currentTransaction == internalTransaction)
		{
			_currentTransaction = null;
		}
	}

	public override void Dispose()
	{
		_smiContext.OutOfScope -= OnOutOfScope;
		base.Dispose();
	}

	internal override void ExecuteTransaction(TransactionRequest transactionRequest, string transactionName, System.Data.IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest)
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.ExecuteTransaction|ADV> {0}, transactionRequest={1}, transactionName='{2}', isolationLevel={3}, internalTransaction=#{4} transactionId=0x{5}.", base.ObjectID, transactionRequest, transactionName, iso, internalTransaction?.ObjectID ?? 0, internalTransaction?.TransactionId ?? 0);
		switch (transactionRequest)
		{
		case TransactionRequest.Begin:
			try
			{
				_pendingTransaction = internalTransaction;
				_smiConnection.BeginTransaction(transactionName, iso, _smiEventSink);
			}
			finally
			{
				_pendingTransaction = null;
			}
			break;
		case TransactionRequest.Commit:
			_smiConnection.CommitTransaction(_currentTransaction.TransactionId, _smiEventSink);
			break;
		case TransactionRequest.Promote:
			base.PromotedDTCToken = _smiConnection.PromoteTransaction(_currentTransaction.TransactionId, _smiEventSink);
			break;
		case TransactionRequest.Rollback:
		case TransactionRequest.IfRollback:
			_smiConnection.RollbackTransaction(_currentTransaction.TransactionId, transactionName, _smiEventSink);
			break;
		case TransactionRequest.Save:
			_smiConnection.CreateTransactionSavePoint(_currentTransaction.TransactionId, transactionName, _smiEventSink);
			break;
		}
		_smiEventSink.ProcessMessagesAndThrow();
	}

	protected override byte[] GetDTCAddress()
	{
		byte[] dTCAddress = _smiConnection.GetDTCAddress(_smiEventSink);
		_smiEventSink.ProcessMessagesAndThrow();
		if (dTCAddress != null)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.GetDTCAddress|ADV> whereAbouts = {0}, Length = {1}", dTCAddress, (ushort)dTCAddress.Length);
		}
		else
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.GetDTCAddress|ADV> whereAbouts=null");
		}
		return dTCAddress;
	}

	internal void GetCurrentTransactionPair(out long transactionId, out Transaction transaction)
	{
		lock (this)
		{
			transactionId = ((CurrentTransaction != null) ? CurrentTransaction.TransactionId : 0);
			transaction = null;
			if (transactionId != 0L)
			{
				transaction = InternalEnlistedTransaction;
			}
		}
	}

	private void OnOutOfScope(object s, EventArgs e)
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.OutOfScope|ADV> {0} context is out of scope", base.ObjectID);
		base.DelegatedTransaction = null;
		DbConnection dbConnection = (DbConnection)base.Owner;
		try
		{
			if (dbConnection != null && 1 == _isInUse)
			{
				dbConnection.Close();
			}
		}
		finally
		{
			ContextTransaction = null;
			_isInUse = 0;
		}
	}

	protected override void PropagateTransactionCookie(byte[] transactionCookie)
	{
		if (transactionCookie != null)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.PropagateTransactionCookie|ADV> transactionCookie", transactionCookie, (ushort)transactionCookie.Length);
		}
		else
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlInternalConnectionSmi.PropagateTransactionCookie|ADV> null");
		}
		_smiConnection.EnlistTransaction(transactionCookie, _smiEventSink);
		_smiEventSink.ProcessMessagesAndThrow();
	}

	private void TransactionEndedByServer(long transactionId, TransactionState transactionState)
	{
		SqlDelegatedTransaction delegatedTransaction = base.DelegatedTransaction;
		if (delegatedTransaction != null)
		{
			delegatedTransaction.Transaction.Rollback();
			base.DelegatedTransaction = null;
		}
		TransactionEnded(transactionId, transactionState);
	}

	private void TransactionEnded(long transactionId, TransactionState transactionState)
	{
		if (_currentTransaction != null)
		{
			_currentTransaction.Completed(transactionState);
			_currentTransaction = null;
		}
	}

	private void TransactionStarted(long transactionId, bool isDistributed)
	{
		_currentTransaction = _pendingTransaction;
		_pendingTransaction = null;
		if (_currentTransaction != null)
		{
			_currentTransaction.TransactionId = transactionId;
		}
		else
		{
			TransactionType type = ((!isDistributed) ? TransactionType.LocalFromTSQL : TransactionType.Distributed);
			_currentTransaction = new SqlInternalTransaction(this, type, null, transactionId);
		}
		_currentTransaction.Activate();
	}

	internal override void ValidateConnectionForExecute(SqlCommand command)
	{
		SqlDataReader sqlDataReader = FindLiveReader(null);
		if (sqlDataReader != null)
		{
			TdsParser parser = command.Connection.Parser;
			throw ADP.OpenReaderExists(parser.MARSOn);
		}
	}
}
