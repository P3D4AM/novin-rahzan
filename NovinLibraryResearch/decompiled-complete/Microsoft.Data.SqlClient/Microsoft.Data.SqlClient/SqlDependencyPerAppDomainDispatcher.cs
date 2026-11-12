using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal class SqlDependencyPerAppDomainDispatcher : MarshalByRefObject
{
	private sealed class DependencyList : List<SqlDependency>
	{
		public readonly string CommandHash;

		internal DependencyList(string commandHash)
		{
			CommandHash = commandHash;
		}
	}

	internal static readonly SqlDependencyPerAppDomainDispatcher SingletonInstance = new SqlDependencyPerAppDomainDispatcher();

	private Dictionary<string, SqlDependency> _dependencyIdToDependencyHash;

	private Dictionary<string, DependencyList> _notificationIdToDependenciesHash;

	private Dictionary<string, string> _commandHashToNotificationId;

	private bool _SqlDependencyTimeOutTimerStarted;

	private DateTime _nextTimeout;

	private Timer _timeoutTimer;

	private readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private static int _objectTypeCount;

	internal int ObjectID => _objectID;

	private SqlDependencyPerAppDomainDispatcher()
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher|DEP> {0}", ObjectID);
		try
		{
			_dependencyIdToDependencyHash = new Dictionary<string, SqlDependency>();
			_notificationIdToDependenciesHash = new Dictionary<string, DependencyList>();
			_commandHashToNotificationId = new Dictionary<string, string>();
			_timeoutTimer = new Timer(TimeoutTimerCallback, null, -1, -1);
			AppDomain.CurrentDomain.DomainUnload += UnloadEventHandler;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	public override object InitializeLifetimeService()
	{
		return null;
	}

	private void UnloadEventHandler(object sender, EventArgs e)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.UnloadEventHandler|DEP> {0}", ObjectID);
		try
		{
			SqlDependency.ProcessDispatcher?.QueueAppDomainUnloading(SqlDependency.AppDomainKey);
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal void AddDependencyEntry(SqlDependency dep)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.AddDependencyEntry|DEP> {0}, SqlDependency: {1}", ObjectID, dep.ObjectID);
		try
		{
			lock (this)
			{
				_dependencyIdToDependencyHash.Add(dep.Id, dep);
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal string AddCommandEntry(string commandHash, SqlDependency dep)
	{
		string value = string.Empty;
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> {0}, commandHash: '{1}', SqlDependency: {2}", ObjectID, commandHash, dep.ObjectID);
		try
		{
			lock (this)
			{
				if (!_dependencyIdToDependencyHash.ContainsKey(dep.Id))
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> Dependency not present in depId->dep hash, must have been invalidated.");
				}
				else if (_commandHashToNotificationId.TryGetValue(commandHash, out value))
				{
					DependencyList value2 = null;
					if (!_notificationIdToDependenciesHash.TryGetValue(value, out value2))
					{
						throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyCommandHashIsNotAssociatedWithNotification);
					}
					if (!value2.Contains(dep))
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> Dependency not present for commandHash, adding.");
						value2.Add(dep);
					}
					else
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> Dependency already present for commandHash.");
					}
				}
				else
				{
					value = string.Format(CultureInfo.InvariantCulture, "{0};{1}", SqlDependency.AppDomainKey, Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> Creating new Dependencies list for commandHash.");
					DependencyList dependencyList = new DependencyList(commandHash);
					dependencyList.Add(dep);
					try
					{
					}
					finally
					{
						_commandHashToNotificationId.Add(commandHash, value);
						_notificationIdToDependenciesHash.Add(value, dependencyList);
					}
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
		return value;
	}

	internal void InvalidateCommandID(SqlNotification sqlNotification)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.InvalidateCommandID|DEP> {0}, commandHash: '{1}'", ObjectID, sqlNotification.Key);
		try
		{
			List<SqlDependency> list = null;
			lock (this)
			{
				list = LookupCommandEntryWithRemove(sqlNotification.Key);
				if (list != null)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.InvalidateCommandID|DEP> commandHash found in hashtable.");
					foreach (SqlDependency item in list)
					{
						LookupDependencyEntryWithRemove(item.Id);
						RemoveDependencyFromCommandToDependenciesHash(item);
					}
				}
				else
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.InvalidateCommandID|DEP> commandHash NOT found in hashtable.");
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (SqlDependency item2 in list)
			{
				SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.InvalidateCommandID|DEP> Dependency found in commandHash dependency ArrayList - calling invalidate.");
				try
				{
					item2.Invalidate(sqlNotification.Type, sqlNotification.Info, sqlNotification.Source);
				}
				catch (Exception e)
				{
					if (!ADP.IsCatchableExceptionType(e))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(e);
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal void InvalidateServer(string server, SqlNotification sqlNotification)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.Invalidate|DEP> {0}, server: '{1}'", ObjectID, server);
		try
		{
			List<SqlDependency> list = new List<SqlDependency>();
			lock (this)
			{
				foreach (KeyValuePair<string, SqlDependency> item in _dependencyIdToDependencyHash)
				{
					SqlDependency value = item.Value;
					if (value.ContainsServer(server))
					{
						list.Add(value);
					}
				}
				foreach (SqlDependency item2 in list)
				{
					LookupDependencyEntryWithRemove(item2.Id);
					RemoveDependencyFromCommandToDependenciesHash(item2);
				}
			}
			foreach (SqlDependency item3 in list)
			{
				try
				{
					item3.Invalidate(sqlNotification.Type, sqlNotification.Info, sqlNotification.Source);
				}
				catch (Exception e)
				{
					if (!ADP.IsCatchableExceptionType(e))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(e);
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal SqlDependency LookupDependencyEntry(string id)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntry|DEP> {0}, Key: '{1}'", ObjectID, id);
		try
		{
			if (id == null)
			{
				throw ADP.ArgumentNull("id");
			}
			if (ADP.IsEmpty(id))
			{
				throw SQL.SqlDependencyIdMismatch();
			}
			SqlDependency result = null;
			lock (this)
			{
				if (_dependencyIdToDependencyHash.ContainsKey(id))
				{
					result = _dependencyIdToDependencyHash[id];
				}
				else
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntry|DEP|ERR> ERROR - dependency ID mismatch - not throwing.");
				}
			}
			return result;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private void LookupDependencyEntryWithRemove(string id)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntryWithRemove|DEP> {0}, id: '{1}'", ObjectID, id);
		try
		{
			lock (this)
			{
				if (_dependencyIdToDependencyHash.ContainsKey(id))
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntryWithRemove|DEP> Entry found in hashtable - removing.");
					_dependencyIdToDependencyHash.Remove(id);
					if (_dependencyIdToDependencyHash.Count == 0)
					{
						_timeoutTimer.Change(-1, -1);
						_SqlDependencyTimeOutTimerStarted = false;
					}
				}
				else
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntryWithRemove|DEP> Entry NOT found in hashtable.");
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private List<SqlDependency> LookupCommandEntryWithRemove(string notificationId)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.LookupCommandEntryWithRemove|DEP> {0}, commandHash: '{1}'", ObjectID, notificationId);
		try
		{
			DependencyList value = null;
			lock (this)
			{
				if (_notificationIdToDependenciesHash.TryGetValue(notificationId, out value))
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntriesWithRemove|DEP> Entries found in hashtable - removing.");
					try
					{
					}
					finally
					{
						_notificationIdToDependenciesHash.Remove(notificationId);
						_commandHashToNotificationId.Remove(value.CommandHash);
					}
				}
				else
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntriesWithRemove|DEP> Entries NOT found in hashtable.");
				}
			}
			return value;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private void RemoveDependencyFromCommandToDependenciesHash(SqlDependency dependency)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.RemoveDependencyFromCommandToDependenciesHash|DEP> {0}, SqlDependency: {1}", ObjectID, dependency.ObjectID);
		try
		{
			lock (this)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<string, DependencyList> item in _notificationIdToDependenciesHash)
				{
					DependencyList value = item.Value;
					if (value.Remove(dependency))
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.RemoveDependencyFromCommandToDependenciesHash|DEP> Removed SqlDependency: {0}, with ID: '{1}'.", dependency.ObjectID, dependency.Id);
						if (value.Count == 0)
						{
							list.Add(item.Key);
							list2.Add(item.Value.CommandHash);
						}
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					try
					{
					}
					finally
					{
						_notificationIdToDependenciesHash.Remove(list[i]);
						_commandHashToNotificationId.Remove(list2[i]);
					}
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal void StartTimer(SqlDependency dep)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.StartTimer|DEP> {0}, SqlDependency: {1}", ObjectID, dep.ObjectID);
		try
		{
			lock (this)
			{
				if (!_SqlDependencyTimeOutTimerStarted)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.StartTimer|DEP> Timer not yet started, starting.");
					_timeoutTimer.Change(15000, 15000);
					_nextTimeout = dep.ExpirationTime;
					_SqlDependencyTimeOutTimerStarted = true;
				}
				else if (_nextTimeout > dep.ExpirationTime)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.StartTimer|DEP> Timer already started, resetting time.");
					_nextTimeout = dep.ExpirationTime;
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private static void TimeoutTimerCallback(object state)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyPerAppDomainDispatcher.TimeoutTimerCallback|DEP> AppDomainKey: '{0}'", SqlDependency.AppDomainKey);
		try
		{
			SqlDependency[] array;
			lock (SingletonInstance)
			{
				if (SingletonInstance._dependencyIdToDependencyHash.Count == 0)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.TimeoutTimerCallback|DEP> No dependencies, exiting.");
					return;
				}
				if (SingletonInstance._nextTimeout > DateTime.UtcNow)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyPerAppDomainDispatcher.TimeoutTimerCallback|DEP> No timeouts expired, exiting.");
					return;
				}
				array = new SqlDependency[SingletonInstance._dependencyIdToDependencyHash.Count];
				SingletonInstance._dependencyIdToDependencyHash.Values.CopyTo(array, 0);
			}
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = DateTime.MaxValue;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ExpirationTime <= utcNow)
				{
					try
					{
						array[i].Invalidate(SqlNotificationType.Change, SqlNotificationInfo.Error, SqlNotificationSource.Timeout);
					}
					catch (Exception e)
					{
						if (!ADP.IsCatchableExceptionType(e))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(e);
					}
				}
				else
				{
					if (array[i].ExpirationTime < dateTime)
					{
						dateTime = array[i].ExpirationTime;
					}
					array[i] = null;
				}
			}
			lock (SingletonInstance)
			{
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] != null)
					{
						SingletonInstance._dependencyIdToDependencyHash.Remove(array[j].Id);
					}
				}
				if (dateTime < SingletonInstance._nextTimeout)
				{
					SingletonInstance._nextTimeout = dateTime;
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}
}
