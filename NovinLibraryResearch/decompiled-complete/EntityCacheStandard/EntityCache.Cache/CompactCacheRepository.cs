using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache;

public class CompactCacheRepository<T> : ICompactCacheRepository<T>, ICompactRepository<T>, IDisposable where T : class, IHasGuid, IHasContext
{
	private bool _isCached;

	private bool _isDisposed;

	protected AsyncTimingCache<Guid, T> TimingCache;

	private int cachingTime;

	public IItemAsyncMapper<T, T> MapperForCaching { get; set; }

	public bool IsDisposed => _isDisposed;

	protected bool isCached
	{
		get
		{
			if (_isCached)
			{
				return TimingCache.Count() > 0;
			}
			return false;
		}
		set
		{
			_isCached = value;
		}
	}

	internal ICompactRepository<T> DbRepository { get; set; }

	public WebContextHolder Context => DbRepository?.Context;

	public string ContextGuid => Context?.Guid;

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		return DbRepository.DbCountAsync(status, token, tranName);
	}

	public CompactCacheRepository(ICompactRepository<T> dbRepository, int CachingTime, IItemAsyncMapper<T, T> mapperForCaching)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			try
			{
				MapperForCaching = mapperForCaching;
				DbRepository = dbRepository;
				TimingCache = new AsyncTimingCache<Guid, T>(Context?.Guid ?? "", mapperForCaching, CachingTime);
				TimingCache.OnCollect += TimingCache_OnCollect;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 55);
				throw;
			}
		}
	}

	public CompactCacheRepository(ICompactRepository<T> dbRepository, int cachingTime)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = dbRepository;
		this.cachingTime = cachingTime;
	}

	public virtual async Task<T> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			int line = 0;
			try
			{
				if (guid == Guid.Empty)
				{
					return null;
				}
				line = 1;
				TimingCache.TryGetValue(guid, out var ret2);
				line = 2;
				if (ret2 != null)
				{
					return ret2;
				}
				line = 3;
				ret2 = await DbRepository.GetAsync(guid, token, tranName);
				line = 4;
				if (IsDisposed)
				{
					return null;
				}
				if (ret2 != null)
				{
					await TimingCache.UpdateTimeAsync(guid, ret2, tranName);
				}
				line = 5;
				return ret2;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"IsDisposed:{IsDisposed}//Line:{line}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 97);
				throw;
			}
		}
	}

	public virtual T Get(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			int num = 0;
			try
			{
				if (guid == Guid.Empty)
				{
					return null;
				}
				num = 1;
				TimingCache.TryGetValue(guid, out var ret);
				num = 2;
				if (ret != null)
				{
					return ret;
				}
				num = 3;
				ret = DbRepository.Get(guid, token, tranName);
				num = 4;
				if (IsDisposed)
				{
					return null;
				}
				if (ret != null)
				{
					TimingCache.UpdateTimeAsync(guid, ret, tranName);
				}
				num = 5;
				return ret;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"IsDisposed:{IsDisposed}//Line:{num}", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 128);
				throw;
			}
		}
	}

	public virtual async Task<List<T>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			try
			{
				line = 1;
				await LoadCacheAsync(token, tranName);
				line = 2;
				List<T> list = TimingCache?.GetAll();
				return (!status.HasValue) ? list : list?.Where((T q) => q.Status == status)?.ToList();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 151);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				List<Guid> items2 = (await GetAllAsync(null, token, tranName)).Select((T p) => p.Guid).Except(items)?.ToList();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await RemoveRangeFromEntitiesAsync(items2, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 166);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 170);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 171);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveFromEntitiesAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("RemoveFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			try
			{
				if (items != null && items.Count() > 0)
				{
					foreach (Guid item in items)
					{
						await RemoveFromEntitiesAsync(item, tryCount, token, tranName);
					}
				}
				return new ReturnedSaveFuncInfo();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 191);
				return new ReturnedSaveFuncInfo(ex2, "", "RemoveFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 192);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveFromEntitiesAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("RemoveFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			try
			{
				TimingCache.TryRemove(item, out var _);
				return new ReturnedSaveFuncInfo();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 209);
				return new ReturnedSaveFuncInfo(ex2, "", "RemoveFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 210);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeFromEntitiesAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("RemoveRangeFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			try
			{
				foreach (Guid item2 in items)
				{
					TimingCache.TryRemove(item2, out var _);
				}
				return new ReturnedSaveFuncInfo();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveRangeFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 228);
				return new ReturnedSaveFuncInfo(ex2, "", "RemoveRangeFromEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 229);
			}
		}
	}

	protected async Task LoadCacheAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		using (new JaegerMonitor("LoadCacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				line = 2;
				if (isCached)
				{
					return;
				}
				line = 3;
				await Locks.DbReadSemaphore.WaitAsync();
				line = 4;
				try
				{
					line = 5;
					if (isCached)
					{
						return;
					}
					line = 7;
					IEnumerable<T> enumerable = await DbRepository.GetAllAsync(null, token, tranName);
					line = 8;
					if (enumerable != null)
					{
						foreach (T item in enumerable)
						{
							if (item != null)
							{
								if (IsDisposed)
								{
									return;
								}
								line = 90;
								await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
								line = 10;
							}
						}
					}
					line = 11;
					isCached = true;
				}
				finally
				{
					Locks.DbReadSemaphore.Release();
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				isCached = false;
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//IsDisposed:{IsDisposed}//TimingCache is null:{TimingCache == null}", "LoadCacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 270);
			}
		}
	}

	private async Task TimingCache_OnCollect(object arg1, List<Guid> arg2)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("TimingCache_OnCollect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs"))
		{
			try
			{
				_isCached = false;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TimingCache_OnCollect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 285);
			}
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> AddOrUpdateEntitiesAsync(T item, short tryCount, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
		return new ReturnedSaveFuncInfo();
	}

	public async Task<ReturnedSaveFuncInfo> AddRangeToEntitiesAsync(IEnumerable<T> items, short tryCount, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (items != null && items.Count() > 0)
		{
			foreach (T item in items)
			{
				if (item != null)
				{
					await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
				}
			}
		}
		return new ReturnedSaveFuncInfo();
	}

	public async Task<T> GetEntityAsync(Guid guid, bool loadFromDatabaseIfNotCached, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (TimingCache.TryGetValue(guid, out var ret))
		{
			return ret;
		}
		if (loadFromDatabaseIfNotCached)
		{
			return await GetAsync(guid, token, tranName);
		}
		return null;
	}

	public virtual void Dispose()
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				DbRepository?.Dispose();
				DbRepository = null;
				TimingCache?.Dispose();
				TimingCache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CompactCacheRepository.cs", 329);
		}
	}
}
