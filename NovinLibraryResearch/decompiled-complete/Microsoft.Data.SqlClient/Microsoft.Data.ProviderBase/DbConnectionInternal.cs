using System;
using System.Data;
using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data.ProviderBase;

internal abstract class DbConnectionInternal
{
	private static int _objectTypeCount;

	internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	internal static readonly StateChangeEventArgs StateChangeClosed = new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Closed);

	internal static readonly StateChangeEventArgs StateChangeOpen = new StateChangeEventArgs(ConnectionState.Closed, ConnectionState.Open);

	private readonly bool _allowSetConnectionString;

	private readonly bool _hidePassword;

	private readonly ConnectionState _state;

	private readonly WeakReference _owningObject = new WeakReference(null, trackResurrection: false);

	private DbConnectionPool _connectionPool;

	private DbConnectionPoolCounters _performanceCounters;

	private DbReferenceCollection _referenceCollection;

	private int _pooledCount;

	private bool _connectionIsDoomed;

	private bool _cannotBePooled;

	private bool _isInStasis;

	private DateTime _createTime;

	private Transaction _enlistedTransaction;

	private Transaction _enlistedTransactionOriginal;

	internal bool AllowSetConnectionString => _allowSetConnectionString;

	internal bool CanBePooled => !_connectionIsDoomed && !_cannotBePooled && !_owningObject.IsAlive;

	protected internal Transaction EnlistedTransaction
	{
		get
		{
			return _enlistedTransaction;
		}
		set
		{
			Transaction enlistedTransaction = _enlistedTransaction;
			if ((!(null == enlistedTransaction) || !(null != value)) && (!(null != enlistedTransaction) || enlistedTransaction.Equals(value)))
			{
				return;
			}
			Transaction transaction = null;
			Transaction transaction2 = null;
			try
			{
				if (null != value)
				{
					transaction = value.Clone();
				}
				lock (this)
				{
					transaction2 = Interlocked.Exchange(ref _enlistedTransaction, transaction);
					_enlistedTransactionOriginal = value;
					value = transaction;
					transaction = null;
				}
			}
			finally
			{
				if (null != transaction2 && (object)transaction2 != _enlistedTransaction)
				{
					transaction2.Dispose();
				}
				if (null != transaction && (object)transaction != _enlistedTransaction)
				{
					transaction.Dispose();
				}
			}
			if (null != value)
			{
				SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.set_EnlistedTransaction|RES|CPOOL> {0}, Transaction {1}, Enlisting.", ObjectID, value.GetHashCode());
				TransactionOutcomeEnlist(value);
			}
		}
	}

	protected bool EnlistedTransactionDisposed
	{
		get
		{
			try
			{
				Transaction enlistedTransactionOriginal = _enlistedTransactionOriginal;
				return enlistedTransactionOriginal != null && enlistedTransactionOriginal.TransactionInformation == null;
			}
			catch (ObjectDisposedException)
			{
				return true;
			}
		}
	}

	internal bool IsTxRootWaitingForTxEnd => _isInStasis;

	protected virtual bool UnbindOnTransactionCompletion => true;

	protected internal virtual bool IsNonPoolableTransactionRoot => false;

	internal virtual bool IsTransactionRoot => false;

	protected internal bool IsConnectionDoomed => _connectionIsDoomed;

	internal bool IsEmancipated => !IsTxRootWaitingForTxEnd && _pooledCount < 1 && !_owningObject.IsAlive;

	internal bool IsInPool => _pooledCount == 1;

	internal int ObjectID => _objectID;

	protected internal object Owner => _owningObject.Target;

	internal DbConnectionPool Pool => _connectionPool;

	protected DbConnectionPoolCounters PerformanceCounters => _performanceCounters;

	protected virtual bool ReadyToPrepareTransaction => true;

	protected internal DbReferenceCollection ReferenceCollection => _referenceCollection;

	public abstract string ServerVersion { get; }

	public virtual string ServerVersionNormalized
	{
		get
		{
			throw ADP.NotSupported();
		}
	}

	public bool ShouldHidePassword => _hidePassword;

	public ConnectionState State => _state;

	internal virtual bool IsAccessTokenExpired => false;

	protected DbConnectionInternal()
		: this(ConnectionState.Open, hidePassword: true, allowSetConnectionString: false)
	{
	}

	internal DbConnectionInternal(ConnectionState state, bool hidePassword, bool allowSetConnectionString)
	{
		_allowSetConnectionString = allowSetConnectionString;
		_hidePassword = hidePassword;
		_state = state;
	}

	protected abstract void Activate(Transaction transaction);

	internal void ActivateConnection(Transaction transaction)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.ActivateConnection|RES|INFO|CPOOL> {0}, Activating", ObjectID);
		Activate(transaction);
		PerformanceCounters.NumberOfActiveConnections.Increment();
	}

	internal void AddWeakReference(object value, int tag)
	{
		if (_referenceCollection == null)
		{
			_referenceCollection = CreateReferenceCollection();
			if (_referenceCollection == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.CreateReferenceCollectionReturnedNull);
			}
		}
		_referenceCollection.Add(value, tag);
	}

	public abstract DbTransaction BeginTransaction(System.Data.IsolationLevel il);

	public virtual void ChangeDatabase(string value)
	{
		throw ADP.MethodNotImplemented("ChangeDatabase");
	}

	internal virtual void CloseConnection(DbConnection owningObject, DbConnectionFactory connectionFactory)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.CloseConnection|RES|CPOOL> {0} Closing.", ObjectID);
		if (!connectionFactory.SetInnerConnectionFrom(owningObject, DbConnectionOpenBusy.SingletonInstance, this))
		{
			return;
		}
		lock (this)
		{
			object lockToken = ObtainAdditionalLocksForClose();
			try
			{
				PrepareForCloseConnection();
				DbConnectionPool pool = Pool;
				DetachCurrentTransactionIfEnded();
				if (pool != null)
				{
					pool.PutObject(this, owningObject);
					return;
				}
				Deactivate();
				PerformanceCounters.HardDisconnectsPerSecond.Increment();
				_owningObject.Target = null;
				if (IsTransactionRoot)
				{
					SetInStasis();
					return;
				}
				PerformanceCounters.NumberOfNonPooledConnections.Decrement();
				if (GetType() != typeof(SqlInternalConnectionSmi))
				{
					Dispose();
				}
			}
			finally
			{
				ReleaseAdditionalLocksForClose(lockToken);
				connectionFactory.SetInnerConnectionEvent(owningObject, DbConnectionClosedPreviouslyOpened.SingletonInstance);
			}
		}
	}

	internal virtual void PrepareForReplaceConnection()
	{
	}

	protected virtual void PrepareForCloseConnection()
	{
	}

	protected virtual object ObtainAdditionalLocksForClose()
	{
		return null;
	}

	protected virtual void ReleaseAdditionalLocksForClose(object lockToken)
	{
	}

	protected virtual DbReferenceCollection CreateReferenceCollection()
	{
		throw ADP.InternalError(ADP.InternalErrorCode.AttemptingToConstructReferenceCollectionOnStaticObject);
	}

	protected abstract void Deactivate();

	internal void DeactivateConnection()
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.DeactivateConnection|RES|INFO|CPOOL> {0}, Deactivating", ObjectID);
		if (PerformanceCounters != null)
		{
			PerformanceCounters.NumberOfActiveConnections.Decrement();
		}
		if (!_connectionIsDoomed && Pool.UseLoadBalancing && DateTime.UtcNow.Ticks - _createTime.Ticks > Pool.LoadBalanceTimeout.Ticks)
		{
			DoNotPoolThisConnection();
		}
		Deactivate();
	}

	internal virtual void DelegatedTransactionEnded()
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.DelegatedTransactionEnded|RES|CPOOL> {0}, Delegated Transaction Completed.", ObjectID);
		if (1 == _pooledCount)
		{
			TerminateStasis(returningToPool: true);
			Deactivate();
			DbConnectionPool pool = Pool;
			if (pool == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectWithoutPool);
			}
			pool.PutObjectFromTransactedPool(this);
		}
		else if (-1 == _pooledCount && !_owningObject.IsAlive)
		{
			TerminateStasis(returningToPool: false);
			Deactivate();
			PerformanceCounters.NumberOfNonPooledConnections.Decrement();
			Dispose();
		}
	}

	public virtual void Dispose()
	{
		_connectionPool = null;
		_performanceCounters = null;
		_connectionIsDoomed = true;
		_enlistedTransactionOriginal = null;
		Transaction transaction = Interlocked.Exchange(ref _enlistedTransaction, null);
		if (transaction != null)
		{
			transaction.Dispose();
		}
	}

	protected internal void DoNotPoolThisConnection()
	{
		_cannotBePooled = true;
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.DoNotPoolThisConnection|RES|INFO|CPOOL> {0}, Marking pooled object as non-poolable so it will be disposed", ObjectID);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	protected internal void DoomThisConnection()
	{
		_connectionIsDoomed = true;
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.DoomThisConnection|RES|INFO|CPOOL> {0}, Dooming", ObjectID);
	}

	protected internal void UnDoomThisConnection()
	{
		_connectionIsDoomed = false;
	}

	public abstract void EnlistTransaction(Transaction transaction);

	protected internal virtual DataTable GetSchema(DbConnectionFactory factory, DbConnectionPoolGroup poolGroup, DbConnection outerConnection, string collectionName, string[] restrictions)
	{
		DbMetaDataFactory metaDataFactory = factory.GetMetaDataFactory(poolGroup, this);
		return metaDataFactory.GetSchema(outerConnection, collectionName, restrictions);
	}

	internal void MakeNonPooledObject(object owningObject, DbConnectionPoolCounters performanceCounters)
	{
		_connectionPool = null;
		_performanceCounters = performanceCounters;
		_owningObject.Target = owningObject;
		_pooledCount = -1;
	}

	internal void MakePooledConnection(DbConnectionPool connectionPool)
	{
		_createTime = DateTime.UtcNow;
		_connectionPool = connectionPool;
		_performanceCounters = connectionPool.PerformanceCounters;
	}

	internal void NotifyWeakReference(int message)
	{
		ReferenceCollection?.Notify(message);
	}

	internal virtual void OpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory)
	{
		if (!TryOpenConnection(outerConnection, connectionFactory, null, null))
		{
			throw ADP.InternalError(ADP.InternalErrorCode.SynchronousConnectReturnedPending);
		}
	}

	internal virtual bool TryOpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		throw ADP.ConnectionAlreadyOpen(State);
	}

	internal virtual bool TryReplaceConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		throw ADP.MethodNotImplemented("TryReplaceConnection");
	}

	protected bool TryOpenConnectionInternal(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions)
	{
		if (connectionFactory.SetInnerConnectionFrom(outerConnection, DbConnectionClosedConnecting.SingletonInstance, this))
		{
			DbConnectionInternal connection = null;
			try
			{
				connectionFactory.PermissionDemand(outerConnection);
				if (!connectionFactory.TryGetConnection(outerConnection, retry, userOptions, this, out connection))
				{
					return false;
				}
			}
			catch
			{
				connectionFactory.SetInnerConnectionTo(outerConnection, this);
				throw;
			}
			if (connection == null)
			{
				connectionFactory.SetInnerConnectionTo(outerConnection, this);
				throw ADP.InternalConnectionError(ADP.ConnectionError.GetConnectionReturnsNull);
			}
			connectionFactory.SetInnerConnectionEvent(outerConnection, connection);
		}
		return true;
	}

	internal void PrePush(object expectedOwner)
	{
		if (expectedOwner == null)
		{
			if (_owningObject.Target != null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasOwner);
			}
		}
		else if (_owningObject.Target != expectedOwner)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnpooledObjectHasWrongOwner);
		}
		if (_pooledCount != 0)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.PushingObjectSecondTime);
		}
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.PrePush|RES|CPOOL> {0}, Preparing to push into pool, owning connection {1}, pooledCount={2}", ObjectID, 0, _pooledCount);
		_pooledCount++;
		_owningObject.Target = null;
	}

	internal void PostPop(object newOwner)
	{
		if (_owningObject.Target != null)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectHasOwner);
		}
		_owningObject.Target = newOwner;
		_pooledCount--;
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.PostPop|RES|CPOOL> {0}, Preparing to pop from pool,  owning connection {1}, pooledCount={2}", ObjectID, 0, _pooledCount);
		if (Pool != null)
		{
			if (_pooledCount != 0)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.PooledObjectInPoolMoreThanOnce);
			}
		}
		else if (-1 != _pooledCount)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.NonPooledObjectUsedMoreThanOnce);
		}
	}

	internal void RemoveWeakReference(object value)
	{
		ReferenceCollection?.Remove(value);
	}

	protected virtual void CleanupTransactionOnCompletion(Transaction transaction)
	{
	}

	internal void DetachCurrentTransactionIfEnded()
	{
		Transaction enlistedTransaction = EnlistedTransaction;
		if (enlistedTransaction != null)
		{
			bool flag;
			try
			{
				flag = enlistedTransaction.TransactionInformation.Status != TransactionStatus.Active;
			}
			catch (TransactionException)
			{
				flag = true;
			}
			if (flag)
			{
				DetachTransaction(enlistedTransaction, isExplicitlyReleasing: true);
			}
		}
	}

	internal void DetachTransaction(Transaction transaction, bool isExplicitlyReleasing)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.DetachTransaction|RES|CPOOL> {0}, Transaction Completed. (pooledCount={1})", ObjectID, _pooledCount);
		lock (this)
		{
			DbConnection dbConnection = (DbConnection)Owner;
			if (!isExplicitlyReleasing && !UnbindOnTransactionCompletion && dbConnection != null)
			{
				return;
			}
			Transaction enlistedTransaction = _enlistedTransaction;
			if (enlistedTransaction != null && transaction.Equals(enlistedTransaction))
			{
				EnlistedTransaction = null;
				if (IsTxRootWaitingForTxEnd)
				{
					DelegatedTransactionEnded();
				}
			}
		}
	}

	internal void CleanupConnectionOnTransactionCompletion(Transaction transaction)
	{
		DetachTransaction(transaction, isExplicitlyReleasing: false);
		Pool?.TransactionEnded(transaction, this);
	}

	private void TransactionCompletedEvent(object sender, TransactionEventArgs e)
	{
		Transaction transaction = e.Transaction;
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.TransactionCompletedEvent|RES|CPOOL> {0}, Transaction Completed. (pooledCount = {1})", ObjectID, _pooledCount);
		CleanupTransactionOnCompletion(transaction);
		CleanupConnectionOnTransactionCompletion(transaction);
	}

	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	private void TransactionOutcomeEnlist(Transaction transaction)
	{
		transaction.TransactionCompleted += TransactionCompletedEvent;
	}

	internal void SetInStasis()
	{
		_isInStasis = true;
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.SetInStasis|RES|CPOOL> {0}, Non-Pooled Connection has Delegated Transaction, waiting to Dispose.", ObjectID);
		PerformanceCounters.NumberOfStasisConnections.Increment();
	}

	private void TerminateStasis(bool returningToPool)
	{
		if (returningToPool)
		{
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.TerminateStasis|RES|CPOOL> {0}, Delegated Transaction has ended, connection is closed.  Returning to general pool.", ObjectID);
		}
		else
		{
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionInternal.TerminateStasis|RES|CPOOL> {0}, Delegated Transaction has ended, connection is closed/leaked.  Disposing.", ObjectID);
		}
		PerformanceCounters.NumberOfStasisConnections.Decrement();
		_isInStasis = false;
	}

	internal virtual bool IsConnectionAlive(bool throwOnException = false)
	{
		return true;
	}
}
