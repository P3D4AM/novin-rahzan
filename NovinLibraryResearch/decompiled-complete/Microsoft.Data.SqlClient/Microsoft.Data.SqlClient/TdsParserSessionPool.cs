using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal class TdsParserSessionPool
{
	private const int MaxInactiveCount = 10;

	private static int _objectTypeCount;

	private readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private readonly TdsParser _parser;

	private readonly List<TdsParserStateObject> _cache;

	private int _cachedCount;

	private TdsParserStateObject[] _freeStateObjects;

	private int _freeStateObjectCount;

	private bool IsDisposed => _freeStateObjects == null;

	internal int ObjectID => _objectID;

	internal int ActiveSessionsCount => _cachedCount - _freeStateObjectCount;

	internal TdsParserSessionPool(TdsParser parser)
	{
		_parser = parser;
		_cache = new List<TdsParserStateObject>();
		_freeStateObjects = new TdsParserStateObject[10];
		_freeStateObjectCount = 0;
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserSessionPool.ctor|ADV> {0} created session pool for parser {1}", ObjectID, parser.ObjectID);
	}

	internal void Deactivate()
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.TdsParserSessionPool.Deactivate|ADV> {0} deactivating cachedCount={1}", ObjectID, _cachedCount);
		try
		{
			lock (_cache)
			{
				for (int num = _cache.Count - 1; num >= 0; num--)
				{
					TdsParserStateObject tdsParserStateObject = _cache[num];
					if (tdsParserStateObject != null && tdsParserStateObject.IsOrphaned)
					{
						SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserSessionPool.Deactivate|ADV> {0} reclaiming session {1}", ObjectID, tdsParserStateObject.ObjectID);
						PutSession(tdsParserStateObject);
					}
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	internal void BestEffortCleanup()
	{
		for (int i = 0; i < _cache.Count; i++)
		{
			_cache[i]?.Handle?.Dispose();
		}
	}

	internal void Dispose()
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserSessionPool.Dispose|ADV> {0} disposing cachedCount={1}", ObjectID, _cachedCount);
		lock (_cache)
		{
			for (int i = 0; i < _freeStateObjectCount; i++)
			{
				if (_freeStateObjects[i] != null)
				{
					_freeStateObjects[i].Dispose();
				}
			}
			_freeStateObjects = null;
			_freeStateObjectCount = 0;
			for (int j = 0; j < _cache.Count; j++)
			{
				if (_cache[j] != null)
				{
					if (_cache[j].IsOrphaned)
					{
						_cache[j].Dispose();
					}
					else
					{
						_cache[j].DecrementPendingCallbacks(release: false);
					}
				}
			}
			_cache.Clear();
			_cachedCount = 0;
		}
	}

	internal TdsParserStateObject GetSession(object owner)
	{
		TdsParserStateObject tdsParserStateObject;
		lock (_cache)
		{
			if (IsDisposed)
			{
				throw ADP.ClosedConnectionError();
			}
			if (_freeStateObjectCount > 0)
			{
				_freeStateObjectCount--;
				tdsParserStateObject = _freeStateObjects[_freeStateObjectCount];
				_freeStateObjects[_freeStateObjectCount] = null;
			}
			else
			{
				tdsParserStateObject = _parser.CreateSession();
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserSessionPool.CreateSession|ADV> {0} adding session {1} to pool", ObjectID, tdsParserStateObject.ObjectID);
				_cache.Add(tdsParserStateObject);
				_cachedCount = _cache.Count;
			}
			tdsParserStateObject.Activate(owner);
		}
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserSessionPool.GetSession|ADV> {0} using session {1}", ObjectID, tdsParserStateObject.ObjectID);
		return tdsParserStateObject;
	}

	internal void PutSession(TdsParserStateObject session)
	{
		bool flag = session.Deactivate();
		lock (_cache)
		{
			if (IsDisposed)
			{
				session.Dispose();
			}
			else if (flag && _freeStateObjectCount < 10)
			{
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserSessionPool.PutSession|ADV> {0} keeping session {1} cachedCount={2}", ObjectID, session.ObjectID, _cachedCount);
				_freeStateObjects[_freeStateObjectCount] = session;
				_freeStateObjectCount++;
			}
			else
			{
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserSessionPool.PutSession|ADV> {0} disposing session {1} cachedCount={2}", ObjectID, session.ObjectID, _cachedCount);
				bool flag2 = _cache.Remove(session);
				_cachedCount = _cache.Count;
				session.Dispose();
			}
			session.RemoveOwner();
		}
	}

	internal string TraceString()
	{
		return string.Format(null, "(ObjID={0}, free={1}, cached={2}, total={3})", _objectID, (_freeStateObjects == null) ? "(null)" : _freeStateObjectCount.ToString((IFormatProvider?)null), _cachedCount, _cache.Count);
	}
}
