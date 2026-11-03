using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace AccSqlServerPersistence.Model;

public class BaseModelContext : DbContext, IAsyncDisposable
{
	private bool _isDisposed;

	private TaskCompletionSource<bool> _isDisposedTask = new TaskCompletionSource<bool>();

	private readonly SemaphoreSlim _addUpdateSemaphore;

	private SemaphoreSlim saveChangeSemaphore = new SemaphoreSlim(1, 1);

	private JaegerMonitor _JaegerMonitor;

	private readonly string _tranName;

	private readonly string _memberName;

	private readonly int _sourceLineNumber;

	private readonly string _sourceFilePath;

	public ILogger ContextLogger => LogCache.Instance?.Value?.GetContextLogger();

	public string TranName => _tranName;

	public int TimeOut { get; private set; }

	public string ConnectionString { get; private set; }

	public bool IsDisposed => _isDisposed;

	public event Func<object, string, Task> OnDisposed;

	public string GetCallerName()
	{
		return $"<{_sourceFilePath}><{_memberName}><Line:{_sourceLineNumber}>";
	}

	public BaseModelContext(string connectionString, string tranName, int timeOut, string debugInfo, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
	{
		try
		{
			_isDisposedTask = new TaskCompletionSource<bool>();
			TimeOut = timeOut;
			_tranName = tranName;
			_memberName = memberName;
			_sourceLineNumber = sourceLineNumber;
			_sourceFilePath = sourceFilePath;
			ConnectionString = connectionString;
			_JaegerMonitor = new JaegerMonitor(memberName, sourceFilePath);
			_addUpdateSemaphore = new SemaphoreSlim(1, 1);
			if (!string.IsNullOrEmpty(connectionString))
			{
				SetConfiguration();
			}
			CheckDisposeAsync();
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 75);
		}
	}

	private async Task CheckDisposeAsync()
	{
		try
		{
			TaskCompletionSource<bool> completedDisposeTask = _isDisposedTask;
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(TimeOut),
				_isDisposedTask.Task
			}) != completedDisposeTask.Task)
			{
				string ex = "context has not been disposed!\r\n" + $"Class:{_sourceFilePath}/Func:{_memberName}/Line:{_sourceLineNumber}";
				WebErrorLog.Instance.StartLog(ex, "", "CheckDisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 88);
				Dispose(disposing: true, $"CheckDisposeAsync({TimeOut}(ms))", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs");
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "CheckDisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 93);
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		Stopwatch.StartNew();
		try
		{
			((DbContext)this).OnModelCreating(modelBuilder);
			RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo");
			foreach (IMutableProperty item in from p in modelBuilder.Model.GetEntityTypes().SelectMany((IMutableEntityType t) => t.GetProperties())
				where ((IPropertyBase)p).ClrType == typeof(decimal) || ((IPropertyBase)p).ClrType == typeof(decimal?)
				select p)
			{
				RelationalPropertyExtensions.SetColumnType(item, "decimal(18, 3)");
			}
			foreach (IMutableProperty item2 in from p in modelBuilder.Model.GetEntityTypes().SelectMany((IMutableEntityType t) => t.GetProperties())
				where ((IPropertyBase)p).ClrType == typeof(DateTime) || ((IPropertyBase)p).ClrType == typeof(DateTime?)
				select p)
			{
				RelationalPropertyExtensions.SetColumnType(item2, "datetime");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreating", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 121);
		}
	}

	public ValueTask DisposeAsync([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		if (_isDisposed)
		{
			return default(ValueTask);
		}
		return DisposeAsync(disposing: true, memberName, sourceFilePath);
	}

	public override ValueTask DisposeAsync()
	{
		if (_isDisposed)
		{
			return default(ValueTask);
		}
		Dispose(disposing: true, "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs");
		return ((DbContext)this).DisposeAsync();
	}

	public void Dispose([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		if (!_isDisposed)
		{
			Dispose(disposing: true, memberName, sourceFilePath);
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true, "withoutParams", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs");
	}

	private void SetConfiguration()
	{
		((DbContext)this).ChangeTracker.LazyLoadingEnabled = false;
		((DbContext)this).ChangeTracker.AutoDetectChangesEnabled = false;
		((DbContext)this).ChangeTracker.QueryTrackingBehavior = (QueryTrackingBehavior)1;
	}

	public async ValueTask DisposeAsync(bool disposing, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_isDisposedTask.TrySetResult(result: true);
				_isDisposedTask = null;
				_JaegerMonitor?.Dispose();
				_JaegerMonitor = null;
				await _003C_003En__0();
				if (this.OnDisposed != null)
				{
					Utilities.RaiseEvent(this.OnDisposed, this, _tranName, default(CancellationToken), 60, "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Class:{_sourceFilePath}/Func:{_memberName}/Line:{_sourceLineNumber}", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 175);
		}
	}

	public void Dispose(bool disposing, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_isDisposedTask.TrySetResult(result: true);
				_isDisposedTask = null;
				_JaegerMonitor?.Dispose();
				_JaegerMonitor = null;
				((DbContext)this).Dispose();
				if (this.OnDisposed != null)
				{
					Utilities.RaiseEvent(this.OnDisposed, this, _tranName, default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Class:{_sourceFilePath}/Func:{_memberName}/Line:{_sourceLineNumber}", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 195);
		}
	}

	public async Task<ReturnedSaveFuncInfo> AddOrUpdateAsync<T>(T item, CancellationToken token, params object[] keyValues) where T : class
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		try
		{
			if (item is IHasModified hasModified)
			{
				hasModified.Modified = DateTime.Now;
			}
			if (keyValues == null || !keyValues.Any())
			{
				throw new NotImplementedException("داداش داری اشتباه میزنی کلید نمیتونه نال باشه");
			}
			T found = await ((DbContext)this).FindAsync<T>(keyValues, token);
			if (found == null)
			{
				await ((DbContext)this).AddAsync<T>(item, token);
			}
			else
			{
				((EntityEntry)((DbContext)this).Entry<T>(found)).CurrentValues.SetValues((object)item);
				if ((int)((EntityEntry)((DbContext)this).Entry<T>(found)).State != 4)
				{
					try
					{
						await _addUpdateSemaphore.WaitAsync(30000, token);
						semaphoreHasBeenTaken = true;
						((DbContext)this).Update<T>(found);
					}
					finally
					{
						if (semaphoreHasBeenTaken)
						{
							_addUpdateSemaphore.Release();
						}
					}
				}
			}
		}
		catch (NotImplementedException)
		{
			throw;
		}
		catch (Exception exception)
		{
			ret.AddReturnedValue(exception, "", "AddOrUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 227);
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> AddOrUpdateAsync<T>(T item, params object[] keyValues) where T : class
	{
		return AddOrUpdateAsync(item, default(CancellationToken), keyValues);
	}

	public override async Task AddRangeAsync(IEnumerable<object> entities, CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _addUpdateSemaphore.WaitAsync(30000, token);
			semaphoreHasBeenTaken = true;
			await _003C_003En__1(entities, token);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_addUpdateSemaphore.Release();
			}
		}
	}

	public override async ValueTask<EntityEntry> AddAsync(object entity, CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _addUpdateSemaphore.WaitAsync(30000, token);
			semaphoreHasBeenTaken = true;
			return await _003C_003En__2(entity, token);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_addUpdateSemaphore.Release();
			}
		}
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return SaveChangesAsync(cancellationToken, "");
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken, string debugInfo)
	{
		using (new JaegerMonitor("SaveChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs"))
		{
			try
			{
				if (IsDisposed)
				{
					throw new ObjectDisposedException("baseModelContext is disposed!!!");
				}
				return await _003C_003En__3(cancellationToken);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (SqlException val)
			{
				SqlException val2 = val;
				if (((Exception)(object)val2).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val2, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "SaveChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 297);
				throw;
			}
			catch (SqlException val3)
			{
				SqlException val4 = val3;
				if (((Exception)(object)val4).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val4, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "SaveChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 304);
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "SaveChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 310);
				throw;
			}
		}
	}

	public async Task<DbDataReader> ExecuteReaderAsync(string commandText, List<SqlParameter> parameters, string tranName, CancellationToken token = default(CancellationToken), CommandType commandType = CommandType.StoredProcedure, int timeOut = 30, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		using (new JaegerMonitor("ExecuteReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs"))
		{
			_ = 1;
			try
			{
				using DbCommand cmd = RelationalDatabaseFacadeExtensions.GetDbConnection(((DbContext)this).Database).CreateCommand();
				cmd.CommandText = commandText;
				cmd.CommandType = commandType;
				cmd.CommandTimeout = timeOut;
				token.ThrowIfCancellationRequested();
				if (cmd.Connection.State == ConnectionState.Closed)
				{
					await cmd.Connection.OpenAsync(token);
				}
				if (parameters != null)
				{
					cmd.Parameters.AddRange(parameters.CastToMicrosoft());
				}
				if (!string.IsNullOrEmpty(tranName))
				{
					cmd.Transaction = DbContextTransactionExtensions.GetDbTransaction(((DbContext)this).Database.CurrentTransaction);
				}
				token.ThrowIfCancellationRequested();
				if (token.IsCancellationRequested)
				{
					throw new OperationCanceledException();
				}
				return await cmd.ExecuteReaderAsync(token);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (SqlException val)
			{
				SqlException val2 = val;
				if (((Exception)(object)val2).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val2, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "ExecuteReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 341);
				throw;
			}
			catch (SqlException val3)
			{
				SqlException val4 = val3;
				if (((Exception)(object)val4).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val4, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "ExecuteReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 348);
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "ExecuteReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 354);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<object>> ExecuteNoneQueryAsync(string spName, List<SqlParameter> parameters, string tranName, CancellationToken token = default(CancellationToken), string outPutName = "", CommandType commandType = CommandType.StoredProcedure, int timeout = 30, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		using (new JaegerMonitor("ExecuteNoneQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs"))
		{
			ReturnedSaveFuncInfoWithValue<object> res = new ReturnedSaveFuncInfoWithValue<object>();
			string parametersForDebug = "";
			try
			{
				if (parameters?.Any() ?? false)
				{
					parametersForDebug = string.Join(",", parameters?.Select((SqlParameter p) => $"{((DbParameter)(object)p)?.ParameterName}='{((DbParameter)(object)p)?.Value}'"));
				}
				using DbCommand cmd = RelationalDatabaseFacadeExtensions.GetDbConnection(((DbContext)this).Database).CreateCommand();
				cmd.CommandText = spName;
				cmd.CommandType = commandType;
				cmd.CommandTimeout = timeout;
				if (cmd.Connection.State == ConnectionState.Closed)
				{
					await cmd.Connection.OpenAsync(token);
				}
				if (parameters != null)
				{
					cmd.Parameters.AddRange(parameters?.CastToMicrosoft());
				}
				if (!string.IsNullOrEmpty(tranName))
				{
					cmd.Transaction = DbContextTransactionExtensions.GetDbTransaction(((DbContext)this).Database.CurrentTransaction);
				}
				await cmd.ExecuteNonQueryAsync(token);
				if (!string.IsNullOrEmpty(outPutName))
				{
					res.value = cmd.Parameters[outPutName ?? ""].Value;
				}
				return res;
			}
			catch (SqlException val)
			{
				SqlException val2 = val;
				res.AddReturnedValue((Exception)(object)val2, "", "ExecuteNoneQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 385);
				if (((Exception)(object)val2).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val2, "spName:" + spName + " parameters:" + parametersForDebug, "ExecuteNoneQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 387);
				throw;
			}
			catch (SqlException val3)
			{
				SqlException val4 = val3;
				res.AddReturnedValue((Exception)(object)val4, "", "ExecuteNoneQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 392);
				if (((Exception)(object)val4).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val4, "spName:" + spName + " parameters:" + parametersForDebug, "ExecuteNoneQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 394);
				throw;
			}
			catch (Exception ex)
			{
				if (parameters?.Any() ?? false)
				{
					WebErrorLog.Instance.StartLog(ex, "spName:" + spName + " parameters:" + parametersForDebug, "ExecuteNoneQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 400);
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, "spName:" + spName + " parameters : NULL !!!", "ExecuteNoneQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 401);
				}
				res.AddReturnedValue(ex, "", "ExecuteNoneQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 402);
				throw;
			}
		}
	}

	public async Task<T> ExecuteScalarAsync<T>(string spName, List<SqlParameter> parameters, string tranName, CancellationToken token = default(CancellationToken), int timeout = 30, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", CommandType type = CommandType.StoredProcedure)
	{
		using (new JaegerMonitor("ExecuteScalarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs"))
		{
			new ReturnedSaveFuncInfo();
			try
			{
				DbCommand cmd = RelationalDatabaseFacadeExtensions.GetDbConnection(((DbContext)this).Database).CreateCommand();
				cmd.CommandText = spName;
				cmd.CommandType = type;
				cmd.CommandTimeout = timeout;
				if (cmd.Connection.State == ConnectionState.Closed)
				{
					await cmd.Connection.OpenAsync(token);
				}
				if (parameters != null)
				{
					cmd.Parameters.AddRange(parameters?.CastToMicrosoft());
				}
				if (!string.IsNullOrEmpty(tranName))
				{
					cmd.Transaction = DbContextTransactionExtensions.GetDbTransaction(((DbContext)this).Database.CurrentTransaction);
				}
				return (T)(await cmd.ExecuteScalarAsync(token));
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (SqlException val)
			{
				SqlException val2 = val;
				if (((Exception)(object)val2).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val2, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "ExecuteScalarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 430);
				throw;
			}
			catch (SqlException val3)
			{
				SqlException val4 = val3;
				if (((Exception)(object)val4).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val4, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "ExecuteScalarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 437);
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "Class:" + _sourceFilePath + "\r\nFunction:" + _memberName, "ExecuteScalarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\BaseModelContext.cs", 443);
				throw;
			}
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private ValueTask _003C_003En__0()
	{
		return ((DbContext)this).DisposeAsync();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _003C_003En__1(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
	{
		return ((DbContext)this).AddRangeAsync(entities, cancellationToken);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private ValueTask<EntityEntry> _003C_003En__2(object entity, CancellationToken cancellationToken = default(CancellationToken))
	{
		return ((DbContext)this).AddAsync(entity, cancellationToken);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<int> _003C_003En__3(CancellationToken cancellationToken = default(CancellationToken))
	{
		return ((DbContext)this).SaveChangesAsync(cancellationToken);
	}
}
