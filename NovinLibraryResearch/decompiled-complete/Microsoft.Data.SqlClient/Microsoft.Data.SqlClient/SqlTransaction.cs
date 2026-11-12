using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

public sealed class SqlTransaction : DbTransaction
{
	private static int _objectTypeCount;

	internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	internal readonly IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

	private SqlInternalTransaction _internalTransaction;

	private SqlConnection _connection;

	private bool _isFromAPI;

	public new SqlConnection Connection
	{
		get
		{
			if (IsZombied)
			{
				return null;
			}
			return _connection;
		}
	}

	protected override DbConnection DbConnection => Connection;

	internal SqlInternalTransaction InternalTransaction => _internalTransaction;

	public override IsolationLevel IsolationLevel
	{
		get
		{
			ZombieCheck();
			return _isolationLevel;
		}
	}

	private bool IsYukonPartialZombie
	{
		get
		{
			if (_internalTransaction != null)
			{
				return _internalTransaction.IsCompleted;
			}
			return false;
		}
	}

	internal bool IsZombied
	{
		get
		{
			if (_internalTransaction != null)
			{
				return _internalTransaction.IsCompleted;
			}
			return true;
		}
	}

	internal int ObjectID => _objectID;

	internal SqlStatistics Statistics
	{
		get
		{
			if (_connection != null && _connection.StatisticsEnabled)
			{
				return _connection.Statistics;
			}
			return null;
		}
	}

	internal SqlTransaction(SqlInternalConnection internalConnection, SqlConnection con, IsolationLevel iso, SqlInternalTransaction internalTransaction)
	{
		_isolationLevel = iso;
		_connection = con;
		if (internalTransaction == null)
		{
			_internalTransaction = new SqlInternalTransaction(internalConnection, TransactionType.LocalFromAPI, this);
			return;
		}
		_internalTransaction = internalTransaction;
		_internalTransaction.InitParent(this);
	}

	public override void Commit()
	{
		SqlConnection.ExecutePermission.Demand();
		ZombieCheck();
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlTransaction.Commit|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlTransaction.Commit|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_connection);
			statistics = SqlStatistics.StartTimer(Statistics);
			_isFromAPI = true;
			_internalTransaction.Commit();
		}
		catch (OutOfMemoryException e)
		{
			_connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_connection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		catch (SqlException ex)
		{
			if (ex.InnerException is Win32Exception ex2 && (long)ex2.NativeErrorCode == 258)
			{
				_connection.Abort(ex);
			}
			throw;
		}
		finally
		{
			_isFromAPI = false;
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			TdsParser target = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				target = SqlInternalConnection.GetBestEffortCleanupTarget(_connection);
				if (!IsZombied && !IsYukonPartialZombie)
				{
					_internalTransaction.Dispose();
				}
			}
			catch (OutOfMemoryException e)
			{
				_connection.Abort(e);
				throw;
			}
			catch (StackOverflowException e2)
			{
				_connection.Abort(e2);
				throw;
			}
			catch (ThreadAbortException e3)
			{
				_connection.Abort(e3);
				SqlInternalConnection.BestEffortCleanup(target);
				throw;
			}
		}
		base.Dispose(disposing);
	}

	public override void Rollback()
	{
		if (IsYukonPartialZombie)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlTransaction.Rollback|ADV> {0} partial zombie no rollback required", ObjectID);
			_internalTransaction = null;
			return;
		}
		ZombieCheck();
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlTransaction.Rollback|API> {0}", ObjectID);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlTransaction.Rollback|API|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_connection);
			statistics = SqlStatistics.StartTimer(Statistics);
			_isFromAPI = true;
			_internalTransaction.Rollback();
		}
		catch (OutOfMemoryException e)
		{
			_connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_connection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		finally
		{
			_isFromAPI = false;
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	public new void Rollback(string transactionName)
	{
		SqlConnection.ExecutePermission.Demand();
		ZombieCheck();
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlTransaction.Rollback|API> {0} transactionName='{1}'", ObjectID, transactionName);
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_connection);
			statistics = SqlStatistics.StartTimer(Statistics);
			_isFromAPI = true;
			_internalTransaction.Rollback(transactionName);
		}
		catch (OutOfMemoryException e)
		{
			_connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_connection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		finally
		{
			_isFromAPI = false;
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	public new void Save(string savePointName)
	{
		SqlConnection.ExecutePermission.Demand();
		ZombieCheck();
		SqlStatistics statistics = null;
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlTransaction.Save|API> {0} savePointName='{1}'", ObjectID, savePointName);
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_connection);
			statistics = SqlStatistics.StartTimer(Statistics);
			_internalTransaction.Save(savePointName);
		}
		catch (OutOfMemoryException e)
		{
			_connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_connection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	internal void Zombie()
	{
		if (_connection.InnerConnection is SqlInternalConnection { IsYukonOrNewer: not false } && !_isFromAPI)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlTransaction.Zombie|ADV> {0} yukon deferred zombie", ObjectID);
		}
		else
		{
			_internalTransaction = null;
		}
	}

	private void ZombieCheck()
	{
		if (IsZombied)
		{
			if (IsYukonPartialZombie)
			{
				_internalTransaction = null;
			}
			throw ADP.TransactionZombied(this);
		}
	}
}
