using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data.ProviderBase;

internal sealed class DbConnectionPool
{
	private enum State
	{
		Initializing,
		Running,
		ShuttingDown
	}

	private sealed class TransactedConnectionList : List<DbConnectionInternal>
	{
		private Transaction _transaction;

		internal TransactedConnectionList(int initialAllocation, Transaction tx)
			: base(initialAllocation)
		{
			_transaction = tx;
		}

		internal void Dispose()
		{
			if (null != _transaction)
			{
				_transaction.Dispose();
			}
		}
	}

	private sealed class PendingGetConnection
	{
		public long DueTime { get; private set; }

		public DbConnection Owner { get; private set; }

		public TaskCompletionSource<DbConnectionInternal> Completion { get; private set; }

		public DbConnectionOptions UserOptions { get; private set; }

		public PendingGetConnection(long dueTime, DbConnection owner, TaskCompletionSource<DbConnectionInternal> completion, DbConnectionOptions userOptions)
		{
			DueTime = dueTime;
			Owner = owner;
			Completion = completion;
			UserOptions = userOptions;
		}
	}

	private sealed class TransactedConnectionPool
	{
		private Dictionary<Transaction, TransactedConnectionList> _transactedCxns;

		private DbConnectionPool _pool;

		private static int _objectTypeCount;

		internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

		internal int ObjectID => _objectID;

		internal DbConnectionPool Pool => _pool;

		internal TransactedConnectionPool(DbConnectionPool pool)
		{
			_pool = pool;
			_transactedCxns = new Dictionary<Transaction, TransactedConnectionList>();
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.TransactedConnectionPool|RES|CPOOL> {0}, Constructed for connection pool {1}", ObjectID, _pool.ObjectID);
		}

		internal DbConnectionInternal GetTransactedObject(Transaction transaction)
		{
			DbConnectionInternal dbConnectionInternal = null;
			bool flag = false;
			TransactedConnectionList value;
			lock (_transactedCxns)
			{
				flag = _transactedCxns.TryGetValue(transaction, out value);
			}
			if (flag)
			{
				lock (value)
				{
					int num = value.Count - 1;
					if (0 <= num)
					{
						dbConnectionInternal = value[num];
						value.RemoveAt(num);
					}
				}
			}
			if (dbConnectionInternal != null)
			{
				SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.GetTransactedObject|RES|CPOOL> {0}, Transaction {1}, Connection {2}, Popped.", ObjectID, transaction.GetHashCode(), dbConnectionInternal.ObjectID);
			}
			return dbConnectionInternal;
		}

		internal void PutTransactedObject(Transaction transaction, DbConnectionInternal transactedObject)
		{
			bool flag = false;
			TransactedConnectionList value;
			lock (_transactedCxns)
			{
				if (flag = _transactedCxns.TryGetValue(transaction, out value))
				{
					lock (value)
					{
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.PutTransactedObject|RES|CPOOL> {0}, Transaction {1}, Connection {2}, Pushing.", ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
						value.Add(transactedObject);
					}
				}
			}
			if (!flag)
			{
				Transaction transaction2 = null;
				TransactedConnectionList transactedConnectionList = null;
				try
				{
					transaction2 = transaction.Clone();
					transactedConnectionList = new TransactedConnectionList(2, transaction2);
					lock (_transactedCxns)
					{
						if (flag = _transactedCxns.TryGetValue(transaction, out value))
						{
							lock (value)
							{
								SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.PutTransactedObject|RES|CPOOL> {0}, Transaction {1}, Connection {2}, Pushing.", ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
								value.Add(transactedObject);
							}
						}
						else
						{
							SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.PutTransactedObject|RES|CPOOL> {0}, Transaction {1}, Connection {2}, Adding List to transacted pool.", ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
							transactedConnectionList.Add(transactedObject);
							_transactedCxns.Add(transaction2, transactedConnectionList);
							transaction2 = null;
						}
					}
				}
				finally
				{
					if (null != transaction2)
					{
						if (transactedConnectionList != null)
						{
							transactedConnectionList.Dispose();
						}
						else
						{
							transaction2.Dispose();
						}
					}
				}
				SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.PutTransactedObject|RES|CPOOL> {0}, Transaction {1}, Connection {2}, Added.", ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
			}
			Pool.PerformanceCounters.NumberOfFreeConnections.Increment();
		}

		internal void TransactionEnded(Transaction transaction, DbConnectionInternal transactedObject)
		{
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.TransactionEnded|RES|CPOOL> {0}, Transaction {1}, Connection {2}, Transaction Completed", ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
			int num = -1;
			lock (_transactedCxns)
			{
				if (_transactedCxns.TryGetValue(transaction, out var value))
				{
					bool flag = false;
					lock (value)
					{
						num = value.IndexOf(transactedObject);
						if (num >= 0)
						{
							value.RemoveAt(num);
						}
						if (0 >= value.Count)
						{
							SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.TransactionEnded|RES|CPOOL> {0}, Transaction {1}, Removing List from transacted pool.", ObjectID, transaction.GetHashCode());
							_transactedCxns.Remove(transaction);
							flag = true;
						}
					}
					if (flag)
					{
						value.Dispose();
					}
				}
				else
				{
					SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactedConnectionPool.TransactionEnded|RES|CPOOL> {0}, Transaction {1}, Connection {2}, Transacted pool not yet created prior to transaction completing. Connection may be leaked.", ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
				}
			}
			if (0 <= num)
			{
				Pool.PerformanceCounters.NumberOfFreeConnections.Decrement();
				Pool.PutObjectFromTransactedPool(transactedObject);
			}
		}
	}

	private sealed class PoolWaitHandles : DbBuffer
	{
		private readonly Semaphore _poolSemaphore;

		private readonly ManualResetEvent _errorEvent;

		private readonly Semaphore _creationSemaphore;

		private readonly SafeHandle _poolHandle;

		private readonly SafeHandle _errorHandle;

		private readonly SafeHandle _creationHandle;

		private readonly int _releaseFlags;

		internal SafeHandle CreationHandle
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return _creationHandle;
			}
		}

		internal Semaphore CreationSemaphore => _creationSemaphore;

		internal ManualResetEvent ErrorEvent => _errorEvent;

		internal Semaphore PoolSemaphore => _poolSemaphore;

		internal PoolWaitHandles()
			: base(3 * IntPtr.Size)
		{
			bool success = false;
			bool success2 = false;
			bool success3 = false;
			_poolSemaphore = new Semaphore(0, 1048576);
			_errorEvent = new ManualResetEvent(initialState: false);
			_creationSemaphore = new Semaphore(1, 1);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				_poolHandle = _poolSemaphore.SafeWaitHandle;
				_errorHandle = _errorEvent.SafeWaitHandle;
				_creationHandle = _creationSemaphore.SafeWaitHandle;
				_poolHandle.DangerousAddRef(ref success);
				_errorHandle.DangerousAddRef(ref success2);
				_creationHandle.DangerousAddRef(ref success3);
				_ = IntPtr.Size;
				WriteIntPtr(0, _poolHandle.DangerousGetHandle());
				WriteIntPtr(IntPtr.Size, _errorHandle.DangerousGetHandle());
				WriteIntPtr(2 * IntPtr.Size, _creationHandle.DangerousGetHandle());
			}
			finally
			{
				if (success)
				{
					_releaseFlags |= 1;
				}
				if (success2)
				{
					_releaseFlags |= 2;
				}
				if (success3)
				{
					_releaseFlags |= 4;
				}
			}
		}

		protected override bool ReleaseHandle()
		{
			if (((true ? 1u : 0u) & (uint)_releaseFlags) != 0)
			{
				_poolHandle.DangerousRelease();
			}
			if ((2u & (uint)_releaseFlags) != 0)
			{
				_errorHandle.DangerousRelease();
			}
			if ((4u & (uint)_releaseFlags) != 0)
			{
				_creationHandle.DangerousRelease();
			}
			return base.ReleaseHandle();
		}
	}

	private const int MAX_Q_SIZE = 1048576;

	private const int SEMAPHORE_HANDLE = 0;

	private const int ERROR_HANDLE = 1;

	private const int CREATION_HANDLE = 2;

	private const int BOGUS_HANDLE = 3;

	private const int WAIT_OBJECT_0 = 0;

	private const int WAIT_TIMEOUT = 258;

	private const int WAIT_ABANDONED = 128;

	private const int WAIT_FAILED = -1;

	private const int ERROR_WAIT_DEFAULT = 5000;

	private static readonly Random _random = new Random(5101977);

	private readonly int _cleanupWait;

	private readonly DbConnectionPoolIdentity _identity;

	private readonly DbConnectionFactory _connectionFactory;

	private readonly DbConnectionPoolGroup _connectionPoolGroup;

	private readonly DbConnectionPoolGroupOptions _connectionPoolGroupOptions;

	private DbConnectionPoolProviderInfo _connectionPoolProviderInfo;

	private readonly ConcurrentDictionary<DbConnectionPoolAuthenticationContextKey, DbConnectionPoolAuthenticationContext> _pooledDbAuthenticationContexts;

	private State _state;

	private readonly ConcurrentStack<DbConnectionInternal> _stackOld = new ConcurrentStack<DbConnectionInternal>();

	private readonly ConcurrentStack<DbConnectionInternal> _stackNew = new ConcurrentStack<DbConnectionInternal>();

	private readonly ConcurrentQueue<PendingGetConnection> _pendingOpens = new ConcurrentQueue<PendingGetConnection>();

	private int _pendingOpensWaiting;

	private readonly WaitCallback _poolCreateRequest;

	private int _waitCount;

	private readonly PoolWaitHandles _waitHandles;

	private Exception _resError;

	private volatile bool _errorOccurred;

	private int _errorWait;

	private Timer _errorTimer;

	private Timer _cleanupTimer;

	private readonly TransactedConnectionPool _transactedConnectionPool;

	private readonly List<DbConnectionInternal> _objectList;

	private int _totalObjects;

	private static int _objectTypeCount;

	internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private int CreationTimeout => PoolGroupOptions.CreationTimeout;

	internal int Count => _totalObjects;

	internal DbConnectionFactory ConnectionFactory => _connectionFactory;

	internal bool ErrorOccurred => _errorOccurred;

	private bool HasTransactionAffinity => PoolGroupOptions.HasTransactionAffinity;

	internal TimeSpan LoadBalanceTimeout => PoolGroupOptions.LoadBalanceTimeout;

	private bool NeedToReplenish
	{
		get
		{
			if (State.Running != _state)
			{
				return false;
			}
			int count = Count;
			if (count >= MaxPoolSize)
			{
				return false;
			}
			if (count < MinPoolSize)
			{
				return true;
			}
			int num = _stackNew.Count + _stackOld.Count;
			int waitCount = _waitCount;
			return num < waitCount || (num == waitCount && count > 1);
		}
	}

	internal DbConnectionPoolIdentity Identity => _identity;

	internal bool IsRunning => State.Running == _state;

	private int MaxPoolSize => PoolGroupOptions.MaxPoolSize;

	private int MinPoolSize => PoolGroupOptions.MinPoolSize;

	internal int ObjectID => _objectID;

	internal DbConnectionPoolCounters PerformanceCounters => _connectionFactory.PerformanceCounters;

	internal DbConnectionPoolGroup PoolGroup => _connectionPoolGroup;

	internal DbConnectionPoolGroupOptions PoolGroupOptions => _connectionPoolGroupOptions;

	internal DbConnectionPoolProviderInfo ProviderInfo => _connectionPoolProviderInfo;

	internal ConcurrentDictionary<DbConnectionPoolAuthenticationContextKey, DbConnectionPoolAuthenticationContext> AuthenticationContexts => _pooledDbAuthenticationContexts;

	internal bool UseLoadBalancing => PoolGroupOptions.UseLoadBalancing;

	private bool UsingIntegrateSecurity
	{
		get
		{
			if (_identity != null)
			{
				return DbConnectionPoolIdentity.NoIdentity != _identity;
			}
			return false;
		}
	}

	internal DbConnectionPool(DbConnectionFactory connectionFactory, DbConnectionPoolGroup connectionPoolGroup, DbConnectionPoolIdentity identity, DbConnectionPoolProviderInfo connectionPoolProviderInfo)
	{
		if (identity != null && identity.IsRestricted)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.AttemptingToPoolOnRestrictedToken);
		}
		_state = State.Initializing;
		lock (_random)
		{
			_cleanupWait = _random.Next(12, 24) * 10 * 1000;
		}
		_connectionFactory = connectionFactory;
		_connectionPoolGroup = connectionPoolGroup;
		_connectionPoolGroupOptions = connectionPoolGroup.PoolGroupOptions;
		_connectionPoolProviderInfo = connectionPoolProviderInfo;
		_identity = identity;
		_waitHandles = new PoolWaitHandles();
		_errorWait = 5000;
		_errorTimer = null;
		_objectList = new List<DbConnectionInternal>(MaxPoolSize);
		_pooledDbAuthenticationContexts = new ConcurrentDictionary<DbConnectionPoolAuthenticationContextKey, DbConnectionPoolAuthenticationContext>(4 * Environment.ProcessorCount, 2);
		if (ADP.IsPlatformNT5)
		{
			_transactedConnectionPool = new TransactedConnectionPool(this);
		}
		_poolCreateRequest = PoolCreateRequest;
		_state = State.Running;
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.DbConnectionPool|RES|CPOOL> {0}, Constructed.", ObjectID);
	}

	private void CleanupCallback(object state)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.CleanupCallback|RES|INFO|CPOOL> {0}", ObjectID);
		while (Count > MinPoolSize && _waitHandles.PoolSemaphore.WaitOne(0, exitContext: false))
		{
			if (_stackOld.TryPop(out var result))
			{
				PerformanceCounters.NumberOfFreeConnections.Decrement();
				bool flag = true;
				lock (result)
				{
					if (result.IsTransactionRoot)
					{
						flag = false;
					}
				}
				if (flag)
				{
					DestroyObject(result);
				}
				else
				{
					result.SetInStasis();
				}
				continue;
			}
			_waitHandles.PoolSemaphore.Release(1);
			break;
		}
		if (_waitHandles.PoolSemaphore.WaitOne(0, exitContext: false))
		{
			DbConnectionInternal result2;
			while (_stackNew.TryPop(out result2))
			{
				SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.CleanupCallback|RES|INFO|CPOOL> {0}, ChangeStacks={1}", ObjectID, result2.ObjectID);
				_stackOld.Push(result2);
			}
			_waitHandles.PoolSemaphore.Release(1);
		}
		QueuePoolCreateRequest();
	}

	internal void Clear()
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.Clear|RES|CPOOL> {0}, Clearing.", ObjectID);
		DbConnectionInternal result;
		lock (_objectList)
		{
			int count = _objectList.Count;
			for (int i = 0; i < count; i++)
			{
				result = _objectList[i];
				result?.DoNotPoolThisConnection();
			}
		}
		while (_stackNew.TryPop(out result))
		{
			PerformanceCounters.NumberOfFreeConnections.Decrement();
			DestroyObject(result);
		}
		while (_stackOld.TryPop(out result))
		{
			PerformanceCounters.NumberOfFreeConnections.Decrement();
			DestroyObject(result);
		}
		ReclaimEmancipatedObjects();
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.Clear|RES|CPOOL> {0}, Cleared.", ObjectID);
	}

	private Timer CreateCleanupTimer()
	{
		return new Timer(CleanupCallback, null, _cleanupWait, _cleanupWait);
	}

	private bool IsBlockingPeriodEnabled()
	{
		if (!(_connectionPoolGroup.ConnectionOptions is SqlConnectionString sqlConnectionString))
		{
			return true;
		}
		switch (sqlConnectionString.PoolBlockingPeriod)
		{
		case PoolBlockingPeriod.Auto:
			if (ADP.IsAzureSqlServerEndpoint(sqlConnectionString.DataSource))
			{
				return false;
			}
			return true;
		case PoolBlockingPeriod.AlwaysBlock:
			return true;
		case PoolBlockingPeriod.NeverBlock:
			return false;
		default:
			return true;
		}
	}

	private DbConnectionInternal CreateObject(DbConnection owningObject, DbConnectionOptions userOptions, DbConnectionInternal oldConnection)
	{
		DbConnectionInternal dbConnectionInternal = null;
		try
		{
			dbConnectionInternal = _connectionFactory.CreatePooledConnection(this, owningObject, _connectionPoolGroup.ConnectionOptions, _connectionPoolGroup.PoolKey, userOptions);
			if (dbConnectionInternal == null)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.CreateObjectReturnedNull);
			}
			if (!dbConnectionInternal.CanBePooled)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.NewObjectCannotBePooled);
			}
			dbConnectionInternal.PrePush(null);
			lock (_objectList)
			{
				if (oldConnection != null && oldConnection.Pool == this)
				{
					_objectList.Remove(oldConnection);
				}
				_objectList.Add(dbConnectionInternal);
				_totalObjects = _objectList.Count;
				PerformanceCounters.NumberOfPooledConnections.Increment();
			}
			if (oldConnection != null)
			{
				DbConnectionPool pool = oldConnection.Pool;
				if (pool != null && pool != this)
				{
					lock (pool._objectList)
					{
						pool._objectList.Remove(oldConnection);
						pool._totalObjects = pool._objectList.Count;
					}
				}
			}
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.CreateObject|RES|CPOOL> {0}, Connection {1}, Added to pool.", ObjectID, dbConnectionInternal?.ObjectID);
			_errorWait = 5000;
			return dbConnectionInternal;
		}
		catch (Exception ex)
		{
			if (!ADP.IsCatchableExceptionType(ex))
			{
				throw;
			}
			ADP.TraceExceptionForCapture(ex);
			if (!IsBlockingPeriodEnabled())
			{
				throw;
			}
			if (dbConnectionInternal != null && dbConnectionInternal.IsConnectionAlive())
			{
				dbConnectionInternal.Dispose();
			}
			dbConnectionInternal = null;
			_resError = ex;
			Timer timer = new Timer(ErrorCallback, null, -1, -1);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				_waitHandles.ErrorEvent.Set();
				_errorOccurred = true;
				_errorTimer = timer;
				bool flag = timer.Change(_errorWait, _errorWait);
			}
			if (30000 < _errorWait)
			{
				_errorWait = 60000;
			}
			else
			{
				_errorWait *= 2;
			}
			throw;
		}
	}

	private void DeactivateObject(DbConnectionInternal obj)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.DeactivateObject|RES|CPOOL> {0}, Connection {1}, Deactivating.", ObjectID, obj.ObjectID);
		obj.DeactivateConnection();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (obj.IsConnectionDoomed)
		{
			flag2 = true;
		}
		else
		{
			lock (obj)
			{
				if (_state == State.ShuttingDown)
				{
					if (obj.IsTransactionRoot)
					{
						obj.SetInStasis();
						flag3 = true;
					}
					else
					{
						flag2 = true;
					}
				}
				else if (obj.IsNonPoolableTransactionRoot)
				{
					obj.SetInStasis();
					flag3 = true;
				}
				else if (obj.CanBePooled)
				{
					Transaction enlistedTransaction = obj.EnlistedTransaction;
					if (null != enlistedTransaction)
					{
						_transactedConnectionPool.PutTransactedObject(enlistedTransaction, obj);
						flag3 = true;
					}
					else
					{
						flag = true;
					}
				}
				else if (obj.IsTransactionRoot && !obj.IsConnectionDoomed)
				{
					obj.SetInStasis();
					flag3 = true;
				}
				else
				{
					flag2 = true;
				}
			}
		}
		if (flag)
		{
			PutNewObject(obj);
		}
		else if (flag2)
		{
			DestroyObject(obj);
			QueuePoolCreateRequest();
		}
	}

	internal void DestroyObject(DbConnectionInternal obj)
	{
		if (obj.IsTxRootWaitingForTxEnd)
		{
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.DestroyObject|RES|CPOOL> {0}, Connection {1}, Has Delegated Transaction, waiting to Dispose.", ObjectID, obj.ObjectID);
			return;
		}
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.DestroyObject|RES|CPOOL> {0}, Connection {1}, Removing from pool.", ObjectID, obj.ObjectID);
		bool flag = false;
		lock (_objectList)
		{
			flag = _objectList.Remove(obj);
			_totalObjects = _objectList.Count;
		}
		if (flag)
		{
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.DestroyObject|RES|CPOOL> {0}, Connection {1}, Removed from pool.", ObjectID, obj.ObjectID);
			PerformanceCounters.NumberOfPooledConnections.Decrement();
		}
		obj.Dispose();
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.DestroyObject|RES|CPOOL> {0}, Connection {1}, Disposed.", ObjectID, obj.ObjectID);
		PerformanceCounters.HardDisconnectsPerSecond.Increment();
	}

	private void ErrorCallback(object state)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.ErrorCallback|RES|CPOOL> {0}, Resetting Error handling.", ObjectID);
		_errorOccurred = false;
		_waitHandles.ErrorEvent.Reset();
		Timer errorTimer = _errorTimer;
		_errorTimer = null;
		errorTimer?.Dispose();
	}

	private Exception TryCloneCachedException()
	{
		if (_resError == null)
		{
			return null;
		}
		if (_resError.GetType() == typeof(SqlException))
		{
			return ((SqlException)_resError).InternalClone();
		}
		return _resError;
	}

	private void WaitForPendingOpen()
	{
		PendingGetConnection result;
		do
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					flag = Interlocked.CompareExchange(ref _pendingOpensWaiting, 1, 0) == 0;
				}
				if (!flag)
				{
					break;
				}
				while (_pendingOpens.TryDequeue(out result))
				{
					if (!result.Completion.Task.IsCompleted)
					{
						uint waitForMultipleObjectsTimeout = (uint)((result.DueTime != -1) ? Math.Max(ADP.TimerRemainingMilliseconds(result.DueTime), 0L) : uint.MaxValue);
						DbConnectionInternal connection = null;
						bool flag2 = false;
						Exception ex = null;
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							bool allowCreate = true;
							bool onlyOneCheckConnection = false;
							ADP.SetCurrentTransaction(result.Completion.Task.AsyncState as Transaction);
							flag2 = !TryGetConnection(result.Owner, waitForMultipleObjectsTimeout, allowCreate, onlyOneCheckConnection, result.UserOptions, out connection);
						}
						catch (OutOfMemoryException)
						{
							connection?.DoomThisConnection();
							throw;
						}
						catch (StackOverflowException)
						{
							connection?.DoomThisConnection();
							throw;
						}
						catch (ThreadAbortException)
						{
							connection?.DoomThisConnection();
							throw;
						}
						catch (Exception ex5)
						{
							ex = ex5;
						}
						if (ex != null)
						{
							result.Completion.TrySetException(ex);
						}
						else if (flag2)
						{
							result.Completion.TrySetException(ADP.ExceptionWithStackTrace(ADP.PooledOpenTimeout()));
						}
						else if (!result.Completion.TrySetResult(connection))
						{
							PutObject(connection, result.Owner);
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					Interlocked.Exchange(ref _pendingOpensWaiting, 0);
				}
			}
		}
		while (_pendingOpens.TryPeek(out result));
	}

	internal bool TryGetConnection(DbConnection owningObject, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions, out DbConnectionInternal connection)
	{
		uint num = 0u;
		bool allowCreate = false;
		if (retry == null)
		{
			num = (uint)CreationTimeout;
			if (num == 0)
			{
				num = uint.MaxValue;
			}
			allowCreate = true;
		}
		if (_state != State.Running)
		{
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, DbConnectionInternal State != Running.", ObjectID);
			connection = null;
			return true;
		}
		bool onlyOneCheckConnection = true;
		if (TryGetConnection(owningObject, num, allowCreate, onlyOneCheckConnection, userOptions, out connection))
		{
			return true;
		}
		if (retry == null)
		{
			return true;
		}
		PendingGetConnection item = new PendingGetConnection((CreationTimeout == 0) ? (-1) : (ADP.TimerCurrent() + ADP.TimerFromSeconds(CreationTimeout / 1000)), owningObject, retry, userOptions);
		_pendingOpens.Enqueue(item);
		if (_pendingOpensWaiting == 0)
		{
			Thread thread = new Thread(WaitForPendingOpen);
			thread.IsBackground = true;
			thread.Start();
		}
		connection = null;
		return false;
	}

	private bool TryGetConnection(DbConnection owningObject, uint waitForMultipleObjectsTimeout, bool allowCreate, bool onlyOneCheckConnection, DbConnectionOptions userOptions, out DbConnectionInternal connection)
	{
		DbConnectionInternal dbConnectionInternal = null;
		Transaction transaction = null;
		PerformanceCounters.SoftConnectsPerSecond.Increment();
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Getting connection.", ObjectID);
		if (HasTransactionAffinity)
		{
			dbConnectionInternal = GetFromTransactedPool(out transaction);
		}
		if (dbConnectionInternal == null)
		{
			Interlocked.Increment(ref _waitCount);
			uint nCount = (allowCreate ? 3u : 2u);
			do
			{
				int num = 3;
				int num2 = 0;
				bool success = false;
				int errorCode = 0;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					_waitHandles.DangerousAddRef(ref success);
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						num = Microsoft.Data.Common.SafeNativeMethods.WaitForMultipleObjectsEx(nCount, _waitHandles.DangerousGetHandle(), bWaitAll: false, waitForMultipleObjectsTimeout, bAlertable: false);
						if (num == -1)
						{
							errorCode = Marshal.GetHRForLastWin32Error();
						}
					}
					switch (num)
					{
					case 258:
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Wait timed out.", ObjectID);
						Interlocked.Decrement(ref _waitCount);
						connection = null;
						return false;
					case 1:
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Errors are set.", ObjectID);
						Interlocked.Decrement(ref _waitCount);
						throw TryCloneCachedException();
					case 2:
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Creating new connection.", ObjectID);
						try
						{
							dbConnectionInternal = UserCreateRequest(owningObject, userOptions);
						}
						catch
						{
							if (dbConnectionInternal == null)
							{
								Interlocked.Decrement(ref _waitCount);
							}
							throw;
						}
						finally
						{
							if (dbConnectionInternal != null)
							{
								Interlocked.Decrement(ref _waitCount);
							}
						}
						if (dbConnectionInternal == null && Count >= MaxPoolSize && MaxPoolSize != 0 && !ReclaimEmancipatedObjects())
						{
							nCount = 2u;
						}
						goto end_IL_0064;
					case 0:
						Interlocked.Decrement(ref _waitCount);
						dbConnectionInternal = GetFromGeneralPool();
						if (dbConnectionInternal != null && !dbConnectionInternal.IsConnectionAlive())
						{
							SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Connection {1}, found dead and removed.", ObjectID, dbConnectionInternal.ObjectID);
							DestroyObject(dbConnectionInternal);
							dbConnectionInternal = null;
							if (onlyOneCheckConnection)
							{
								if (!_waitHandles.CreationSemaphore.WaitOne((int)waitForMultipleObjectsTimeout))
								{
									SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Wait timed out.", ObjectID);
									connection = null;
									return false;
								}
								RuntimeHelpers.PrepareConstrainedRegions();
								try
								{
									SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Creating new connection.", ObjectID);
									dbConnectionInternal = UserCreateRequest(owningObject, userOptions);
								}
								finally
								{
									_waitHandles.CreationSemaphore.Release(1);
								}
							}
						}
						goto end_IL_0064;
					case -1:
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Wait failed.", ObjectID);
						Interlocked.Decrement(ref _waitCount);
						Marshal.ThrowExceptionForHR(errorCode);
						break;
					case 128:
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Semaphore handle abandonded.", ObjectID);
						Interlocked.Decrement(ref _waitCount);
						throw new AbandonedMutexException(0, _waitHandles.PoolSemaphore);
					case 129:
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Error handle abandonded.", ObjectID);
						Interlocked.Decrement(ref _waitCount);
						throw new AbandonedMutexException(1, _waitHandles.ErrorEvent);
					case 130:
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, Creation handle abandoned.", ObjectID);
						Interlocked.Decrement(ref _waitCount);
						throw new AbandonedMutexException(2, _waitHandles.CreationSemaphore);
					}
					SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetConnection|RES|CPOOL> {0}, WaitForMultipleObjects={1}", ObjectID, num);
					Interlocked.Decrement(ref _waitCount);
					throw ADP.InternalError(ADP.InternalErrorCode.UnexpectedWaitAnyResult);
					end_IL_0064:;
				}
				finally
				{
					if (2 == num && Microsoft.Data.Common.SafeNativeMethods.ReleaseSemaphore(_waitHandles.CreationHandle.DangerousGetHandle(), 1, IntPtr.Zero) == 0)
					{
						num2 = Marshal.GetHRForLastWin32Error();
					}
					if (success)
					{
						_waitHandles.DangerousRelease();
					}
				}
				if (num2 != 0)
				{
					Marshal.ThrowExceptionForHR(num2);
				}
				if (dbConnectionInternal != null && dbConnectionInternal.IsAccessTokenExpired)
				{
					DestroyObject(dbConnectionInternal);
					dbConnectionInternal = null;
				}
			}
			while (dbConnectionInternal == null);
		}
		if (dbConnectionInternal != null)
		{
			PrepareConnection(owningObject, dbConnectionInternal, transaction);
		}
		connection = dbConnectionInternal;
		return true;
	}

	private void PrepareConnection(DbConnection owningObject, DbConnectionInternal obj, Transaction transaction)
	{
		lock (obj)
		{
			obj.PostPop(owningObject);
		}
		try
		{
			obj.ActivateConnection(transaction);
		}
		catch
		{
			PutObject(obj, owningObject);
			throw;
		}
	}

	internal DbConnectionInternal ReplaceConnection(DbConnection owningObject, DbConnectionOptions userOptions, DbConnectionInternal oldConnection)
	{
		PerformanceCounters.SoftConnectsPerSecond.Increment();
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.ReplaceConnection|RES|CPOOL> {0}, replacing connection.", ObjectID);
		DbConnectionInternal dbConnectionInternal = UserCreateRequest(owningObject, userOptions, oldConnection);
		if (dbConnectionInternal != null)
		{
			PrepareConnection(owningObject, dbConnectionInternal, oldConnection.EnlistedTransaction);
			oldConnection.PrepareForReplaceConnection();
			oldConnection.DeactivateConnection();
			oldConnection.Dispose();
		}
		return dbConnectionInternal;
	}

	private DbConnectionInternal GetFromGeneralPool()
	{
		DbConnectionInternal result = null;
		if (!_stackNew.TryPop(out result) && !_stackOld.TryPop(out result))
		{
			result = null;
		}
		if (result != null)
		{
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetFromGeneralPool|RES|CPOOL> {0}, Connection {1}, Popped from general pool.", ObjectID, result.ObjectID);
			PerformanceCounters.NumberOfFreeConnections.Decrement();
		}
		return result;
	}

	private DbConnectionInternal GetFromTransactedPool(out Transaction transaction)
	{
		transaction = ADP.GetCurrentTransaction();
		DbConnectionInternal dbConnectionInternal = null;
		if (null != transaction && _transactedConnectionPool != null)
		{
			dbConnectionInternal = _transactedConnectionPool.GetTransactedObject(transaction);
			if (dbConnectionInternal != null)
			{
				SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetFromTransactedPool|RES|CPOOL> {0}, Connection {1}, Popped from transacted pool.", ObjectID, dbConnectionInternal.ObjectID);
				PerformanceCounters.NumberOfFreeConnections.Decrement();
				if (dbConnectionInternal.IsTransactionRoot)
				{
					try
					{
						dbConnectionInternal.IsConnectionAlive(throwOnException: true);
					}
					catch
					{
						SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetFromTransactedPool|RES|CPOOL> {0}, Connection {1}, found dead and removed.", ObjectID, dbConnectionInternal.ObjectID);
						DestroyObject(dbConnectionInternal);
						throw;
					}
				}
				else if (!dbConnectionInternal.IsConnectionAlive())
				{
					SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.GetFromTransactedPool|RES|CPOOL> {0}, Connection {1}, found dead and removed.", ObjectID, dbConnectionInternal.ObjectID);
					DestroyObject(dbConnectionInternal);
					dbConnectionInternal = null;
				}
			}
		}
		return dbConnectionInternal;
	}

	private void PoolCreateRequest(object state)
	{
		long scopeId = SqlClientEventSource.Log.TryPoolerScopeEnterEvent("<prov.DbConnectionPool.PoolCreateRequest|RES|INFO|CPOOL> {0}", ObjectID);
		try
		{
			if (State.Running != _state)
			{
				return;
			}
			if (!_pendingOpens.IsEmpty && _pendingOpensWaiting == 0)
			{
				Thread thread = new Thread(WaitForPendingOpen);
				thread.IsBackground = true;
				thread.Start();
			}
			ReclaimEmancipatedObjects();
			if (ErrorOccurred || !NeedToReplenish || (UsingIntegrateSecurity && !_identity.Equals(DbConnectionPoolIdentity.GetCurrent())))
			{
				return;
			}
			bool success = false;
			int num = 3;
			uint creationTimeout = (uint)CreationTimeout;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				_waitHandles.DangerousAddRef(ref success);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					num = Microsoft.Data.Common.SafeNativeMethods.WaitForSingleObjectEx(_waitHandles.CreationHandle.DangerousGetHandle(), creationTimeout, bAlertable: false);
				}
				if (num == 0)
				{
					if (ErrorOccurred)
					{
						return;
					}
					while (NeedToReplenish)
					{
						DbConnectionInternal dbConnectionInternal = CreateObject(null, null, null);
						if (dbConnectionInternal != null)
						{
							PutNewObject(dbConnectionInternal);
							continue;
						}
						break;
					}
				}
				else if (258 == num)
				{
					QueuePoolCreateRequest();
				}
				else
				{
					SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.PoolCreateRequest|RES|CPOOL> {0}, PoolCreateRequest called WaitForSingleObject failed {1}", ObjectID, num);
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.PoolCreateRequest|RES|CPOOL> {0}, PoolCreateRequest called CreateConnection which threw an exception: {1}", ObjectID, ex);
			}
			finally
			{
				if (num == 0)
				{
					num = Microsoft.Data.Common.SafeNativeMethods.ReleaseSemaphore(_waitHandles.CreationHandle.DangerousGetHandle(), 1, IntPtr.Zero);
				}
				if (success)
				{
					_waitHandles.DangerousRelease();
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryPoolerScopeLeaveEvent(scopeId);
		}
	}

	internal void PutNewObject(DbConnectionInternal obj)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.PutNewObject|RES|CPOOL> {0}, Connection {1}, Pushing to general pool.", ObjectID, obj.ObjectID);
		_stackNew.Push(obj);
		_waitHandles.PoolSemaphore.Release(1);
		PerformanceCounters.NumberOfFreeConnections.Increment();
	}

	internal void PutObject(DbConnectionInternal obj, object owningObject)
	{
		PerformanceCounters.SoftDisconnectsPerSecond.Increment();
		lock (obj)
		{
			obj.PrePush(owningObject);
		}
		DeactivateObject(obj);
	}

	internal void PutObjectFromTransactedPool(DbConnectionInternal obj)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.PutObjectFromTransactedPool|RES|CPOOL> {0}, Connection {1}, Transaction has ended.", ObjectID, obj.ObjectID);
		if (_state == State.Running && obj.CanBePooled)
		{
			PutNewObject(obj);
			return;
		}
		DestroyObject(obj);
		QueuePoolCreateRequest();
	}

	private void QueuePoolCreateRequest()
	{
		if (State.Running == _state)
		{
			ThreadPool.QueueUserWorkItem(_poolCreateRequest);
		}
	}

	private bool ReclaimEmancipatedObjects()
	{
		bool result = false;
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.ReclaimEmancipatedObjects|RES|CPOOL> {0}", ObjectID);
		List<DbConnectionInternal> list = new List<DbConnectionInternal>();
		int count;
		lock (_objectList)
		{
			count = _objectList.Count;
			for (int i = 0; i < count; i++)
			{
				DbConnectionInternal dbConnectionInternal = _objectList[i];
				if (dbConnectionInternal == null)
				{
					continue;
				}
				bool lockTaken = false;
				try
				{
					Monitor.TryEnter(dbConnectionInternal, ref lockTaken);
					if (lockTaken && dbConnectionInternal.IsEmancipated)
					{
						dbConnectionInternal.PrePush(null);
						list.Add(dbConnectionInternal);
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(dbConnectionInternal);
					}
				}
			}
		}
		count = list.Count;
		for (int j = 0; j < count; j++)
		{
			DbConnectionInternal dbConnectionInternal2 = list[j];
			SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.ReclaimEmancipatedObjects|RES|CPOOL> {0}, Connection {1}, Reclaiming.", ObjectID, dbConnectionInternal2.ObjectID);
			PerformanceCounters.NumberOfReclaimedConnections.Increment();
			result = true;
			dbConnectionInternal2.DetachCurrentTransactionIfEnded();
			DeactivateObject(dbConnectionInternal2);
		}
		return result;
	}

	internal void Startup()
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.Startup|RES|INFO|CPOOL> {0}, CleanupWait={1}", ObjectID, _cleanupWait);
		_cleanupTimer = CreateCleanupTimer();
		if (NeedToReplenish)
		{
			QueuePoolCreateRequest();
		}
	}

	internal void Shutdown()
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.Shutdown|RES|INFO|CPOOL> {0}", ObjectID);
		_state = State.ShuttingDown;
		Timer cleanupTimer = _cleanupTimer;
		_cleanupTimer = null;
		cleanupTimer?.Dispose();
	}

	internal void TransactionEnded(Transaction transaction, DbConnectionInternal transactedObject)
	{
		SqlClientEventSource.Log.TryPoolerTraceEvent("<prov.DbConnectionPool.TransactionEnded|RES|CPOOL> {0}, Transaction {1}, Connection {2}, Transaction Completed", ObjectID, transaction.GetHashCode(), transactedObject.ObjectID);
		_transactedConnectionPool?.TransactionEnded(transaction, transactedObject);
	}

	private DbConnectionInternal UserCreateRequest(DbConnection owningObject, DbConnectionOptions userOptions, DbConnectionInternal oldConnection = null)
	{
		DbConnectionInternal result = null;
		if (ErrorOccurred)
		{
			throw TryCloneCachedException();
		}
		if ((oldConnection != null || Count < MaxPoolSize || MaxPoolSize == 0) && (oldConnection != null || (Count & 1) == 1 || !ReclaimEmancipatedObjects()))
		{
			result = CreateObject(owningObject, userOptions, oldConnection);
		}
		return result;
	}
}
