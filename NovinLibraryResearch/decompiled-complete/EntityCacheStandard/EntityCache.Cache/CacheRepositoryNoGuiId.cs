using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache;

public abstract class CacheRepositoryNoGuiId<T, TCacheKey> : INoGuidRepository<T>, IDisposable where T : class, IHasContext
{
	public WebContextHolder Context => DbRepository?.Context;

	public string ContextGuid => Context?.Guid;

	protected INoGuidRepository<T> DbRepository { get; set; }

	protected int CachingTime { get; set; }

	protected abstract TimingCache<TCacheKey, T> TimingCache { get; set; }

	public abstract IItemAsyncMapper<T, T> Mapper { get; }

	public abstract bool HasStatus { get; }

	public abstract bool HasModified { get; }

	public bool IsDisposed { get; private set; }

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		return DbRepository.DbCountAsync(status, token, tranName);
	}

	protected CacheRepositoryNoGuiId(INoGuidRepository<T> dbRepository, int cachingTime)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs"))
		{
			try
			{
				DbRepository = dbRepository;
				CachingTime = cachingTime;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 36);
				throw;
			}
		}
	}

	public int Count()
	{
		return TimingCache.Count();
	}

	public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs"))
		{
			try
			{
				return await DbRepository.GetAllAsync(token, tranName);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"IsDisposed:{IsDisposed}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 59);
				throw;
			}
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs"))
		{
			try
			{
				return await DbRepository.SaveAsync(item, tryCount, token, tranName);
			}
			catch (OperationCanceledException exception)
			{
				return new ReturnedSaveFuncInfo(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 72);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 75);
				return new ReturnedSaveFuncInfo(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 76);
			}
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<T> items, short tryCount, CancellationToken token, string tranName)
	{
		int line = 0;
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (items == null || !items.Any())
		{
			return ret;
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs"))
		{
			try
			{
				line = 1;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await DbRepository.SaveRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 90);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line:{line}//IsDisposed:{IsDisposed}//Data:{Json.ToStringJson(items, indented: true)}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 94);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 95);
			}
			return ret;
		}
	}

	public void ResetCache()
	{
		TimingCache.ClearAll();
	}

	public virtual void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				DbRepository?.Dispose();
				DbRepository = null;
				TimingCache?.ClearAll(fireCollectEvent: false);
				TimingCache?.Dispose();
				TimingCache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepositoryNoGuiId.cs", 118);
		}
	}
}
