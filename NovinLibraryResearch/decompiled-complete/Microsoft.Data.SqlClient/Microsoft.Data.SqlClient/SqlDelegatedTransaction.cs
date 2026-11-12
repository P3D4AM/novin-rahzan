using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Transactions;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlDelegatedTransaction : IPromotableSinglePhaseNotification, ITransactionPromoter
{
	private static int _objectTypeCount;

	private readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private const int _globalTransactionsTokenVersionSizeInBytes = 4;

	private SqlInternalConnection _connection;

	private System.Data.IsolationLevel _isolationLevel;

	private SqlInternalTransaction _internalTransaction;

	private Transaction _atomicTransaction;

	private bool _active;

	internal int ObjectID => _objectID;

	internal Transaction Transaction => _atomicTransaction;

	internal bool IsActive => _active;

	internal SqlDelegatedTransaction(SqlInternalConnection connection, Transaction tx)
	{
		_connection = connection;
		_atomicTransaction = tx;
		_active = false;
		System.Transactions.IsolationLevel isolationLevel = tx.IsolationLevel;
		switch (isolationLevel)
		{
		case System.Transactions.IsolationLevel.ReadCommitted:
			_isolationLevel = System.Data.IsolationLevel.ReadCommitted;
			break;
		case System.Transactions.IsolationLevel.ReadUncommitted:
			_isolationLevel = System.Data.IsolationLevel.ReadUncommitted;
			break;
		case System.Transactions.IsolationLevel.RepeatableRead:
			_isolationLevel = System.Data.IsolationLevel.RepeatableRead;
			break;
		case System.Transactions.IsolationLevel.Serializable:
			_isolationLevel = System.Data.IsolationLevel.Serializable;
			break;
		case System.Transactions.IsolationLevel.Snapshot:
			_isolationLevel = System.Data.IsolationLevel.Snapshot;
			break;
		default:
			throw SQL.UnknownSysTxIsolationLevel(isolationLevel);
		}
	}

	public void Initialize()
	{
		SqlInternalConnection connection = _connection;
		SqlConnection connection2 = connection.Connection;
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.Initialize|RES|CPOOL> {0}, Connection {1}, delegating transaction.", ObjectID, connection.ObjectID);
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (connection.IsEnlistedInTransaction)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.Initialize|RES|CPOOL> {0}, Connection {1}, was enlisted, now defecting.", ObjectID, connection.ObjectID);
				connection.EnlistNull();
			}
			_internalTransaction = new SqlInternalTransaction(connection, TransactionType.Delegated, null);
			connection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Begin, null, _isolationLevel, _internalTransaction, isDelegateControlRequest: true);
			if (connection.CurrentTransaction == null)
			{
				connection.DoomThisConnection();
				throw ADP.InternalError(ADP.InternalErrorCode.UnknownTransactionFailure);
			}
			_active = true;
		}
		catch (OutOfMemoryException e)
		{
			connection2.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			connection2.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			connection2.Abort(e3);
			throw;
		}
	}

	public byte[] Promote()
	{
		SqlInternalConnection validConnection = GetValidConnection();
		byte[] result = null;
		if (validConnection != null)
		{
			SqlConnection connection = validConnection.Connection;
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.Promote|RES|CPOOL> {0}, Connection {1}, promoting transaction.", ObjectID, validConnection.ObjectID);
			RuntimeHelpers.PrepareConstrainedRegions();
			Exception ex;
			try
			{
				lock (validConnection)
				{
					try
					{
						ValidateActiveOnConnection(validConnection);
						validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Promote, null, System.Data.IsolationLevel.Unspecified, _internalTransaction, isDelegateControlRequest: true);
						result = _connection.PromotedDTCToken;
						if (_connection.IsGlobalTransaction)
						{
							if (SysTxForGlobalTransactions.SetDistributedTransactionIdentifier == null)
							{
								throw SQL.UnsupportedSysTxForGlobalTransactions();
							}
							if (!_connection.IsGlobalTransactionsEnabledForServer)
							{
								throw SQL.GlobalTransactionsNotEnabled();
							}
							SysTxForGlobalTransactions.SetDistributedTransactionIdentifier.Invoke(_atomicTransaction, new object[2]
							{
								this,
								GetGlobalTxnIdentifierFromToken()
							});
						}
						ex = null;
					}
					catch (SqlException ex2)
					{
						ex = ex2;
						ADP.TraceExceptionWithoutRethrow(ex2);
						validConnection.DoomThisConnection();
					}
					catch (InvalidOperationException ex3)
					{
						ex = ex3;
						ADP.TraceExceptionWithoutRethrow(ex3);
						validConnection.DoomThisConnection();
					}
				}
			}
			catch (OutOfMemoryException e)
			{
				connection.Abort(e);
				throw;
			}
			catch (StackOverflowException e2)
			{
				connection.Abort(e2);
				throw;
			}
			catch (ThreadAbortException e3)
			{
				connection.Abort(e3);
				throw;
			}
			if (ex != null && Transaction.TransactionInformation.Status != TransactionStatus.Aborted)
			{
				throw SQL.PromotionFailed(ex);
			}
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.Promote|RES|CPOOL> {0}, Connection {1}, aborted during promote.", ObjectID, validConnection.ObjectID);
		}
		else
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.Promote|RES|CPOOL> {0}, Connection null, aborted before promoting.", ObjectID);
		}
		return result;
	}

	public void Rollback(SinglePhaseEnlistment enlistment)
	{
		SqlInternalConnection validConnection = GetValidConnection();
		if (validConnection != null)
		{
			SqlConnection connection = validConnection.Connection;
			RuntimeHelpers.PrepareConstrainedRegions();
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.Rollback|RES|CPOOL> {0}, Connection {1}, rolling back transaction.", ObjectID, validConnection.ObjectID);
			try
			{
				lock (validConnection)
				{
					try
					{
						ValidateActiveOnConnection(validConnection);
						_active = false;
						_connection = null;
						if (!_internalTransaction.IsAborted)
						{
							validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Rollback, null, System.Data.IsolationLevel.Unspecified, _internalTransaction, isDelegateControlRequest: true);
						}
					}
					catch (SqlException e)
					{
						ADP.TraceExceptionWithoutRethrow(e);
						validConnection.DoomThisConnection();
					}
					catch (InvalidOperationException e2)
					{
						ADP.TraceExceptionWithoutRethrow(e2);
						validConnection.DoomThisConnection();
					}
				}
				validConnection.CleanupConnectionOnTransactionCompletion(_atomicTransaction);
				enlistment.Aborted();
				return;
			}
			catch (OutOfMemoryException e3)
			{
				connection.Abort(e3);
				throw;
			}
			catch (StackOverflowException e4)
			{
				connection.Abort(e4);
				throw;
			}
			catch (ThreadAbortException e5)
			{
				connection.Abort(e5);
				throw;
			}
		}
		enlistment.Aborted();
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.Rollback|RES|CPOOL> {0}, Connection null, aborted before rollback.", ObjectID);
	}

	public void SinglePhaseCommit(SinglePhaseEnlistment enlistment)
	{
		SqlInternalConnection validConnection = GetValidConnection();
		if (validConnection != null)
		{
			SqlConnection connection = validConnection.Connection;
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.SinglePhaseCommit|RES|CPOOL> {0}, Connection {1}, committing transaction.", ObjectID, validConnection.ObjectID);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				lock (validConnection)
				{
					if (validConnection.IsConnectionDoomed)
					{
						_active = false;
						_connection = null;
						enlistment.Aborted(SQL.ConnectionDoomed());
						return;
					}
					Exception ex;
					try
					{
						ValidateActiveOnConnection(validConnection);
						_active = false;
						_connection = null;
						validConnection.ExecuteTransaction(SqlInternalConnection.TransactionRequest.Commit, null, System.Data.IsolationLevel.Unspecified, _internalTransaction, isDelegateControlRequest: true);
						ex = null;
					}
					catch (SqlException ex2)
					{
						ex = ex2;
						ADP.TraceExceptionWithoutRethrow(ex2);
						validConnection.DoomThisConnection();
					}
					catch (InvalidOperationException ex3)
					{
						ex = ex3;
						ADP.TraceExceptionWithoutRethrow(ex3);
						validConnection.DoomThisConnection();
					}
					if (ex != null)
					{
						if (_internalTransaction.IsCommitted)
						{
							enlistment.Committed();
						}
						else if (_internalTransaction.IsAborted)
						{
							enlistment.Aborted(ex);
						}
						else
						{
							enlistment.InDoubt(ex);
						}
					}
					validConnection.CleanupConnectionOnTransactionCompletion(_atomicTransaction);
					if (ex == null)
					{
						enlistment.Committed();
					}
					return;
				}
			}
			catch (OutOfMemoryException e)
			{
				connection.Abort(e);
				throw;
			}
			catch (StackOverflowException e2)
			{
				connection.Abort(e2);
				throw;
			}
			catch (ThreadAbortException e3)
			{
				connection.Abort(e3);
				throw;
			}
		}
		enlistment.Aborted();
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.SinglePhaseCommit|RES|CPOOL> {0}, Connection null, aborted before commit.", ObjectID);
	}

	internal void TransactionEnded(Transaction transaction)
	{
		SqlInternalConnection connection = _connection;
		if (connection == null)
		{
			return;
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDelegatedTransaction.TransactionEnded|RES|CPOOL> {0}, Connection {1}, transaction completed externally.", ObjectID, connection.ObjectID);
		lock (connection)
		{
			if (_atomicTransaction.Equals(transaction))
			{
				_active = false;
				_connection = null;
			}
			connection.DoomThisConnection();
		}
	}

	private SqlInternalConnection GetValidConnection()
	{
		SqlInternalConnection connection = _connection;
		if (connection == null && _atomicTransaction.TransactionInformation.Status != TransactionStatus.Aborted)
		{
			throw ADP.ObjectDisposed(this);
		}
		return connection;
	}

	private void ValidateActiveOnConnection(SqlInternalConnection connection)
	{
		if (!_active || connection != _connection || connection.DelegatedTransaction != this)
		{
			connection?.DoomThisConnection();
			if (connection != _connection && _connection != null)
			{
				_connection.DoomThisConnection();
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasWrongOwner);
		}
	}

	private Guid GetGlobalTxnIdentifierFromToken()
	{
		byte[] array = new byte[16];
		Array.Copy(_connection.PromotedDTCToken, 4, array, 0, array.Length);
		return new Guid(array);
	}
}
