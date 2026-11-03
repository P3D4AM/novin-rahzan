using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Core;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence;

public abstract class PersistenceNoGuidRepository<T, U> : INoGuidRepository<T>, IDisposable where T : class, IHasContext where U : class, new()
{
	private bool _isDisposed;

	protected ContextHolder dbContext;

	protected string ContextGuid => dbContext?.Context?.Guid;

	public abstract IItemAsyncMapper<U, T> MapFromDatabase { get; }

	public abstract IItemAsyncMapper<T, U> MapToDatabase { get; }

	public bool IsDisposed => _isDisposed;

	public WebContextHolder Context => dbContext.Context;

	public async Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		BaseModelContext c = null;
		int result;
		try
		{
			c = await dbContext.GetContextAsync(tranName, 60000, "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs", 28);
			result = ((IQueryable<U>)((DbContext)c).Set<U>()).Count();
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
			WebErrorLog.Instance.StartLog(ex2, "", "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs", 35);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs");
			}
		}
		return result;
	}

	internal PersistenceNoGuidRepository(ContextHolder db)
	{
		dbContext = db;
	}

	public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", $"PersistenceNoGuidRepository.GetAll_{typeof(T)}"))
		{
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs", 51);
			IEnumerable<T> result;
			try
			{
				_ = 1;
				try
				{
					result = await MapFromDatabase.MapListAsync(ContextGuid, ((IEnumerable<U>)((DbContext)c).Set<U>())?.ToList(), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs", 60);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<ReturnedSaveFuncInfo> SaveAsync(T item, short TryCount, CancellationToken token, string tranName);

	public abstract Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<T> items, short tryCount, CancellationToken token, string tranName);

	public virtual async Task<ReturnedSaveFuncInfo> RemoveAllAsync(short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs", 80);
				((DbSet<_003F>)(object)((DbContext)baseModelContext).Set<U>()).RemoveRange((IEnumerable<_003F>)(((IEnumerable<U>)((DbContext)baseModelContext).Set<U>())?.ToList()));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs", 90);
				ret.AddReturnedValue(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs", 91);
			}
			return ret;
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceNoGuidRepository.cs", 107);
		}
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}
}
