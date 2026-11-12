using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data.ProviderBase;

internal abstract class DbConnectionFactory
{
	private Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> _connectionPoolGroups;

	private readonly List<DbConnectionPool> _poolsToRelease;

	private readonly List<DbConnectionPoolGroup> _poolGroupsToRelease;

	private readonly DbConnectionPoolCounters _performanceCounters;

	private readonly Timer _pruningTimer;

	private const int PruningDueTime = 240000;

	private const int PruningPeriod = 30000;

	private static int _objectTypeCount;

	internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private static int s_pendingOpenNonPooledNext = 0;

	private static Task<DbConnectionInternal>[] s_pendingOpenNonPooled = new Task<DbConnectionInternal>[Environment.ProcessorCount];

	private static Task<DbConnectionInternal> s_completedTask;

	internal DbConnectionPoolCounters PerformanceCounters => _performanceCounters;

	public abstract DbProviderFactory ProviderFactory { get; }

	internal int ObjectID => _objectID;

	protected DbConnectionFactory()
		: this(DbConnectionPoolCountersNoCounters.SingletonInstance)
	{
	}

	protected DbConnectionFactory(DbConnectionPoolCounters performanceCounters)
	{
		_performanceCounters = performanceCounters;
		_connectionPoolGroups = new Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup>();
		_poolsToRelease = new List<DbConnectionPool>();
		_poolGroupsToRelease = new List<DbConnectionPoolGroup>();
		_pruningTimer = CreatePruningTimer();
	}

	public void ClearAllPools()
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<prov.DbConnectionFactory.ClearAllPools|API>", "ClearAllPools");
		try
		{
			Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> connectionPoolGroups = _connectionPoolGroups;
			foreach (KeyValuePair<DbConnectionPoolKey, DbConnectionPoolGroup> item in connectionPoolGroups)
			{
				item.Value?.Clear();
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	public void ClearPool(DbConnection connection)
	{
		ADP.CheckArgumentNull(connection, "connection");
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<prov.DbConnectionFactory.ClearPool|API> {0}", GetObjectId(connection));
		try
		{
			GetConnectionPoolGroup(connection)?.Clear();
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	public void ClearPool(DbConnectionPoolKey key)
	{
		ADP.CheckArgumentNull(key.ConnectionString, "key.ConnectionString");
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<prov.DbConnectionFactory.ClearPool|API> connectionString", "ClearPool");
		try
		{
			Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> connectionPoolGroups = _connectionPoolGroups;
			if (connectionPoolGroups.TryGetValue(key, out var value))
			{
				value.Clear();
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	internal virtual DbConnectionPoolProviderInfo CreateConnectionPoolProviderInfo(DbConnectionOptions connectionOptions)
	{
		return null;
	}

	protected virtual DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
	{
		cacheMetaDataFactory = false;
		throw ADP.NotSupported();
	}

	internal DbConnectionInternal CreateNonPooledConnection(DbConnection owningConnection, DbConnectionPoolGroup poolGroup, DbConnectionOptions userOptions)
	{
		DbConnectionOptions connectionOptions = poolGroup.ConnectionOptions;
		DbConnectionPoolGroupProviderInfo providerInfo = poolGroup.ProviderInfo;
		DbConnectionPoolKey poolKey = poolGroup.PoolKey;
		DbConnectionInternal dbConnectionInternal = CreateConnection(connectionOptions, poolKey, providerInfo, null, owningConnection, userOptions);
		if (dbConnectionInternal != null)
		{
			PerformanceCounters.HardConnectsPerSecond.Increment();
			dbConnectionInternal.MakeNonPooledObject(owningConnection, PerformanceCounters);
		}
		SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionFactory.CreateNonPooledConnection|RES|CPOOL> {0}, Non-pooled database connection created.", ObjectID);
		return dbConnectionInternal;
	}

	internal DbConnectionInternal CreatePooledConnection(DbConnectionPool pool, DbConnection owningObject, DbConnectionOptions options, DbConnectionPoolKey poolKey, DbConnectionOptions userOptions)
	{
		DbConnectionPoolGroupProviderInfo providerInfo = pool.PoolGroup.ProviderInfo;
		DbConnectionInternal dbConnectionInternal = CreateConnection(options, poolKey, providerInfo, pool, owningObject, userOptions);
		if (dbConnectionInternal != null)
		{
			PerformanceCounters.HardConnectsPerSecond.Increment();
			dbConnectionInternal.MakePooledConnection(pool);
		}
		SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionFactory.CreatePooledConnection|RES|CPOOL> {0}, Pooled database connection created.", ObjectID);
		return dbConnectionInternal;
	}

	internal virtual DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
	{
		return null;
	}

	private Timer CreatePruningTimer()
	{
		TimerCallback callback = PruneConnectionPoolGroups;
		return new Timer(callback, null, 240000, 30000);
	}

	protected DbConnectionOptions FindConnectionOptions(DbConnectionPoolKey key)
	{
		if (!ADP.IsEmpty(key.ConnectionString))
		{
			Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> connectionPoolGroups = _connectionPoolGroups;
			if (connectionPoolGroups.TryGetValue(key, out var value))
			{
				return value.ConnectionOptions;
			}
		}
		return null;
	}

	private static Task<DbConnectionInternal> GetCompletedTask()
	{
		if (s_completedTask == null)
		{
			TaskCompletionSource<DbConnectionInternal> taskCompletionSource = new TaskCompletionSource<DbConnectionInternal>();
			taskCompletionSource.SetResult(null);
			s_completedTask = taskCompletionSource.Task;
		}
		return s_completedTask;
	}

	internal bool TryGetConnection(DbConnection owningConnection, TaskCompletionSource<DbConnectionInternal> retry, DbConnectionOptions userOptions, DbConnectionInternal oldConnection, out DbConnectionInternal connection)
	{
		connection = null;
		int num = 10;
		int num2 = 1;
		do
		{
			DbConnectionPoolGroup poolGroup = GetConnectionPoolGroup(owningConnection);
			DbConnectionPool connectionPool = GetConnectionPool(owningConnection, poolGroup);
			if (connectionPool == null)
			{
				poolGroup = GetConnectionPoolGroup(owningConnection);
				if (retry != null)
				{
					CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
					Task<DbConnectionInternal> task3;
					lock (s_pendingOpenNonPooled)
					{
						int i;
						for (i = 0; i < s_pendingOpenNonPooled.Length; i++)
						{
							Task task2 = s_pendingOpenNonPooled[i];
							if (task2 == null)
							{
								s_pendingOpenNonPooled[i] = GetCompletedTask();
								break;
							}
							if (task2.IsCompleted)
							{
								break;
							}
						}
						if (i == s_pendingOpenNonPooled.Length)
						{
							i = s_pendingOpenNonPooledNext++ % s_pendingOpenNonPooled.Length;
						}
						task3 = s_pendingOpenNonPooled[i].ContinueWith(delegate
						{
							Transaction currentTransaction = ADP.GetCurrentTransaction();
							try
							{
								ADP.SetCurrentTransaction(retry.Task.AsyncState as Transaction);
								DbConnectionInternal result = CreateNonPooledConnection(owningConnection, poolGroup, userOptions);
								if (oldConnection != null && oldConnection.State == ConnectionState.Open)
								{
									oldConnection.PrepareForReplaceConnection();
									oldConnection.Dispose();
								}
								return result;
							}
							finally
							{
								ADP.SetCurrentTransaction(currentTransaction);
							}
						}, cancellationTokenSource.Token, TaskContinuationOptions.LongRunning, TaskScheduler.Default);
						s_pendingOpenNonPooled[i] = task3;
					}
					if (owningConnection.ConnectionTimeout > 0)
					{
						int millisecondsDelay = owningConnection.ConnectionTimeout * 1000;
						cancellationTokenSource.CancelAfter(millisecondsDelay);
					}
					task3.ContinueWith(delegate(Task<DbConnectionInternal> task)
					{
						cancellationTokenSource.Dispose();
						if (task.IsCanceled)
						{
							retry.TrySetException(ADP.ExceptionWithStackTrace(ADP.NonPooledOpenTimeout()));
						}
						else if (task.IsFaulted)
						{
							retry.TrySetException(task.Exception.InnerException);
						}
						else if (retry.TrySetResult(task.Result))
						{
							PerformanceCounters.NumberOfNonPooledConnections.Increment();
						}
						else
						{
							task.Result.DoomThisConnection();
							task.Result.Dispose();
						}
					}, TaskScheduler.Default);
					return false;
				}
				connection = CreateNonPooledConnection(owningConnection, poolGroup, userOptions);
				PerformanceCounters.NumberOfNonPooledConnections.Increment();
				continue;
			}
			if (((SqlConnection)owningConnection).ForceNewConnection)
			{
				connection = connectionPool.ReplaceConnection(owningConnection, userOptions, oldConnection);
			}
			else if (!connectionPool.TryGetConnection(owningConnection, retry, userOptions, out connection))
			{
				return false;
			}
			if (connection == null)
			{
				if (connectionPool.IsRunning)
				{
					SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionFactory.GetConnection|RES|CPOOL> {0}, GetConnection failed because a pool timeout occurred.", ObjectID);
					throw ADP.PooledOpenTimeout();
				}
				Thread.Sleep(num2);
				num2 *= 2;
			}
		}
		while (connection == null && num-- > 0);
		if (connection == null)
		{
			SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionFactory.GetConnection|RES|CPOOL> {0}, GetConnection failed because a pool timeout occurred and all retries were exhausted.", ObjectID);
			throw ADP.PooledOpenTimeout();
		}
		return true;
	}

	private DbConnectionPool GetConnectionPool(DbConnection owningObject, DbConnectionPoolGroup connectionPoolGroup)
	{
		if (connectionPoolGroup.IsDisabled && connectionPoolGroup.PoolGroupOptions != null)
		{
			SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionFactory.GetConnectionPool|RES|INFO|CPOOL> {0}, DisabledPoolGroup={1}", ObjectID, connectionPoolGroup.ObjectID);
			DbConnectionPoolGroupOptions poolGroupOptions = connectionPoolGroup.PoolGroupOptions;
			DbConnectionOptions userConnectionOptions = connectionPoolGroup.ConnectionOptions;
			connectionPoolGroup = GetConnectionPoolGroup(connectionPoolGroup.PoolKey, poolGroupOptions, ref userConnectionOptions);
			SetConnectionPoolGroup(owningObject, connectionPoolGroup);
		}
		return connectionPoolGroup.GetConnectionPool(this);
	}

	internal DbConnectionPoolGroup GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, ref DbConnectionOptions userConnectionOptions)
	{
		if (ADP.IsEmpty(key.ConnectionString))
		{
			return null;
		}
		Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> connectionPoolGroups = _connectionPoolGroups;
		if (!connectionPoolGroups.TryGetValue(key, out var value) || (value.IsDisabled && value.PoolGroupOptions != null))
		{
			DbConnectionOptions dbConnectionOptions = CreateConnectionOptions(key.ConnectionString, userConnectionOptions);
			if (dbConnectionOptions == null)
			{
				throw ADP.InternalConnectionError(ADP.ConnectionError.ConnectionOptionsMissing);
			}
			string connectionString = key.ConnectionString;
			if (userConnectionOptions == null)
			{
				userConnectionOptions = dbConnectionOptions;
				connectionString = dbConnectionOptions.Expand();
				if ((object)connectionString != key.ConnectionString)
				{
					DbConnectionPoolKey dbConnectionPoolKey = (DbConnectionPoolKey)((ICloneable)key).Clone();
					dbConnectionPoolKey.ConnectionString = connectionString;
					return GetConnectionPoolGroup(dbConnectionPoolKey, null, ref userConnectionOptions);
				}
			}
			if (poolOptions == null && ADP.IsWindowsNT)
			{
				poolOptions = ((value == null) ? CreateConnectionPoolGroupOptions(dbConnectionOptions) : value.PoolGroupOptions);
			}
			lock (this)
			{
				connectionPoolGroups = _connectionPoolGroups;
				if (!connectionPoolGroups.TryGetValue(key, out value))
				{
					DbConnectionPoolGroup dbConnectionPoolGroup = new DbConnectionPoolGroup(dbConnectionOptions, key, poolOptions);
					dbConnectionPoolGroup.ProviderInfo = CreateConnectionPoolGroupProviderInfo(dbConnectionOptions);
					Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> dictionary = new Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup>(1 + connectionPoolGroups.Count);
					foreach (KeyValuePair<DbConnectionPoolKey, DbConnectionPoolGroup> item in connectionPoolGroups)
					{
						dictionary.Add(item.Key, item.Value);
					}
					dictionary.Add(key, dbConnectionPoolGroup);
					PerformanceCounters.NumberOfActiveConnectionPoolGroups.Increment();
					value = dbConnectionPoolGroup;
					_connectionPoolGroups = dictionary;
				}
			}
		}
		else if (userConnectionOptions == null)
		{
			userConnectionOptions = value.ConnectionOptions;
		}
		return value;
	}

	internal DbMetaDataFactory GetMetaDataFactory(DbConnectionPoolGroup connectionPoolGroup, DbConnectionInternal internalConnection)
	{
		DbMetaDataFactory dbMetaDataFactory = connectionPoolGroup.MetaDataFactory;
		if (dbMetaDataFactory == null)
		{
			bool cacheMetaDataFactory = false;
			dbMetaDataFactory = CreateMetaDataFactory(internalConnection, out cacheMetaDataFactory);
			if (cacheMetaDataFactory)
			{
				connectionPoolGroup.MetaDataFactory = dbMetaDataFactory;
			}
		}
		return dbMetaDataFactory;
	}

	private void PruneConnectionPoolGroups(object state)
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<prov.DbConnectionFactory.PruneConnectionPoolGroups|RES|INFO|CPOOL> {0}", ObjectID);
		lock (_poolsToRelease)
		{
			if (_poolsToRelease.Count != 0)
			{
				DbConnectionPool[] array = _poolsToRelease.ToArray();
				DbConnectionPool[] array2 = array;
				foreach (DbConnectionPool dbConnectionPool in array2)
				{
					if (dbConnectionPool != null)
					{
						dbConnectionPool.Clear();
						if (dbConnectionPool.Count == 0)
						{
							_poolsToRelease.Remove(dbConnectionPool);
							SqlClientEventSource.Log.TryAdvancedTraceEvent("<prov.DbConnectionFactory.PruneConnectionPoolGroups|RES|INFO|CPOOL> {0}, ReleasePool={1}", ObjectID, dbConnectionPool.ObjectID);
							PerformanceCounters.NumberOfInactiveConnectionPools.Decrement();
						}
					}
				}
			}
		}
		lock (_poolGroupsToRelease)
		{
			if (_poolGroupsToRelease.Count != 0)
			{
				DbConnectionPoolGroup[] array3 = _poolGroupsToRelease.ToArray();
				DbConnectionPoolGroup[] array4 = array3;
				foreach (DbConnectionPoolGroup dbConnectionPoolGroup in array4)
				{
					if (dbConnectionPoolGroup != null && dbConnectionPoolGroup.Clear() == 0)
					{
						_poolGroupsToRelease.Remove(dbConnectionPoolGroup);
						SqlClientEventSource.Log.TryAdvancedTraceEvent("<prov.DbConnectionFactory.PruneConnectionPoolGroups|RES|INFO|CPOOL> {0}, ReleasePoolGroup={1}", ObjectID, dbConnectionPoolGroup.ObjectID);
						PerformanceCounters.NumberOfInactiveConnectionPoolGroups.Decrement();
					}
				}
			}
		}
		lock (this)
		{
			Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> connectionPoolGroups = _connectionPoolGroups;
			Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup> dictionary = new Dictionary<DbConnectionPoolKey, DbConnectionPoolGroup>(connectionPoolGroups.Count);
			foreach (KeyValuePair<DbConnectionPoolKey, DbConnectionPoolGroup> item in connectionPoolGroups)
			{
				if (item.Value != null)
				{
					if (item.Value.Prune())
					{
						PerformanceCounters.NumberOfActiveConnectionPoolGroups.Decrement();
						QueuePoolGroupForRelease(item.Value);
					}
					else
					{
						dictionary.Add(item.Key, item.Value);
					}
				}
			}
			_connectionPoolGroups = dictionary;
		}
	}

	internal void QueuePoolForRelease(DbConnectionPool pool, bool clearing)
	{
		pool.Shutdown();
		lock (_poolsToRelease)
		{
			if (clearing)
			{
				pool.Clear();
			}
			_poolsToRelease.Add(pool);
		}
		PerformanceCounters.NumberOfInactiveConnectionPools.Increment();
	}

	internal void QueuePoolGroupForRelease(DbConnectionPoolGroup poolGroup)
	{
		SqlClientEventSource.Log.TryTraceEvent("<prov.DbConnectionFactory.QueuePoolGroupForRelease|RES|INFO|CPOOL> {0}, poolGroup={1}", ObjectID, poolGroup.ObjectID);
		lock (_poolGroupsToRelease)
		{
			_poolGroupsToRelease.Add(poolGroup);
		}
		PerformanceCounters.NumberOfInactiveConnectionPoolGroups.Increment();
	}

	protected virtual DbConnectionInternal CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection, DbConnectionOptions userOptions)
	{
		return CreateConnection(options, poolKey, poolGroupProviderInfo, pool, owningConnection);
	}

	protected abstract DbConnectionInternal CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection);

	protected abstract DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous);

	protected abstract DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions options);

	internal abstract DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection);

	internal abstract DbConnectionInternal GetInnerConnection(DbConnection connection);

	protected abstract int GetObjectId(DbConnection connection);

	internal abstract void PermissionDemand(DbConnection outerConnection);

	internal abstract void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup);

	internal abstract void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to);

	internal abstract bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from);

	internal abstract void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to);
}
