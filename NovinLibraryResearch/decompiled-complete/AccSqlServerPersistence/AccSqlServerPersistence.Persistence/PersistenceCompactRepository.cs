using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Core;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using Serilog;

namespace AccSqlServerPersistence.Persistence;

public abstract class PersistenceCompactRepository<T, U> : ICompactRepository<T>, IDisposable where T : class, IHasGuid, IHasContext where U : class, IHasGuid, new()
{
	protected ContextHolder dbContext;

	private bool _isDisposed;

	public abstract IItemAsyncMapper<U, T> MapFromDatabase { get; set; }

	public abstract IItemMapper<U, T> MapFromDatabase_nonAsync { get; set; }

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => dbContext.Context.Guid;

	public bool IsDisposed { get; private set; }

	public async Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		BaseModelContext c = null;
		int result;
		try
		{
			c = await dbContext.GetContextAsync(tranName, 60000, "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs", 27);
			result = ((!status.HasValue) ? ((IQueryable<U>)((DbContext)c).Set<U>()).Count() : ((!status.Value) ? ((IQueryable<U>)((DbContext)c).Set<U>()).Count((U q) => !q.Status) : ((IQueryable<U>)((DbContext)c).Set<U>()).Count((U q) => q.Status)));
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs", 36);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs");
			}
		}
		return result;
	}

	internal PersistenceCompactRepository(ContextHolder dbContext)
	{
		this.dbContext = dbContext;
	}

	public virtual async Task<T> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs"))
		{
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs", 60);
			T result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					U source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<U>((IQueryable<U>)((DbContext)c).Set<U>(), (Expression<Func<U, bool>>)((U x) => x.Guid == guid), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs", 73);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual T Get(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs"))
		{
			BaseModelContext context = dbContext.GetContext(tranName, 60000, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs", 86);
			try
			{
				U source = ((IQueryable<U>)((DbContext)context).Set<U>()).FirstOrDefault((U x) => x.Guid == guid);
				return MapFromDatabase_nonAsync.Map(ContextGuid, source, null, tranName, token);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs", 99);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					context.Dispose("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs");
				}
			}
		}
	}

	public virtual async Task<List<T>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", $"PersistenceCompactRepository.GetAll_{typeof(T)}"))
		{
			BaseModelContext c = null;
			List<T> result;
			try
			{
				_ = 1;
				try
				{
					c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs", 114);
					IQueryable<U> source = (IQueryable<U>)((DbContext)c).Set<U>();
					if (status.HasValue)
					{
						source = source.Where((U q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, source.ToList(), null, tranName, token);
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, $"MapFromDatabase is null:{MapFromDatabase == null}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs", 126);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceCompactRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> RemoveRangeFromEntitiesAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}
}
