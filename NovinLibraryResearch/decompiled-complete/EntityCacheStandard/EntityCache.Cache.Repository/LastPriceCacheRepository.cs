using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class LastPriceCacheRepository : ILastPriceRepository, IHasContext, INoGuidRepository<LastPriceBusiness>, IDisposable
{
	private ILastPriceRepository DbRepository;

	private bool _isCached;

	public bool HasStatus => true;

	public bool HasModified => true;

	public WebContextHolder Context => DbRepository.Context;

	protected TimingCache<LastPriceCompositeKey, LastPriceBusiness> TimingCache { get; set; }

	public IItemAsyncMapper<LastPriceBusiness, LastPriceBusiness> Mapper => LastPriceMapper.Instance;

	public bool IsDisposed { get; private set; }

	protected bool isCached
	{
		get
		{
			return _isCached;
		}
		set
		{
			_isCached = value;
		}
	}

	public string ContextGuid { get; set; }

	public LastPriceCacheRepository(ILastPriceRepository dbRepository, int CachingTime, string contextGuid)
	{
		DbRepository = dbRepository;
		ContextGuid = contextGuid;
		TimingCache = new TimingCache<LastPriceCompositeKey, LastPriceBusiness>(ContextGuid, Mapper, CachingTime);
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		return DbRepository.DbCountAsync(status, token, tranName);
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				DbRepository = null;
				TimingCache.OnCollect -= TimingCache_OnCollect;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 53);
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(LastPriceBusiness item, CancellationToken token, short tryCount, string tranName)
	{
		return DbRepository.SaveAsync(item, tryCount, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveAllAsync(List<LastPriceBusiness> lst, bool flush, short tryCount, CancellationToken token, string tranName)
	{
		return DbRepository.SaveAllAsync(lst, flush, tryCount, token, tranName);
	}

	public Task<List<LastPriceCompositeKey>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetAllNotSentAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse<LastPriceCompositeKey> item, CancellationToken token, short tryCount, string tranName)
	{
		return DbRepository.SetSaveResult(item, token, tryCount, tranName);
	}

	public async Task<LastPriceBusiness> GetAsync(LastPriceCompositeKey key, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				if (key.ProductGuid == Guid.Empty || key.TafsilGuid == Guid.Empty)
				{
					return null;
				}
				line = 2;
				TimingCache.TryGetValue(key, out var ret);
				line = 3;
				if (ret == null)
				{
					line = 4;
					ret = await DbRepository.GetAsync(key, token, tranName);
					line = 5;
					if (ret != null)
					{
						await TimingCache.UpdateTimeAsync(key, ret, tranName);
					}
					line = 7;
				}
				line = 8;
				return ret;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//TimingCache is null:{TimingCache == null}//isDisposed:{IsDisposed}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 104);
				throw;
			}
		}
	}

	public virtual async Task<IEnumerable<LastPriceBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs"))
		{
			try
			{
				await LoadCacheAsync(token, tranName);
				return TimingCache.GetAll();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 121);
				throw;
			}
		}
	}

	public async Task<IEnumerable<LastPriceBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs"))
		{
			try
			{
				return await DbRepository.GetAllAsync(token, tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 138);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(LastPriceBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				line = 1;
				ReturnedSaveFuncInfo ret = await DbRepository.SaveAsync(item, tryCount, token, tranName);
				line = 2;
				if (ret.HasError)
				{
					return ret;
				}
				line = 3;
				await TimingCache.UpdateTimeAsync(new LastPriceCompositeKey
				{
					TafsilGuid = item.TafsilGuid,
					ProductGuid = item.PrdGuid
				}, item, tranName);
				line = 4;
				return ret;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"line:{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 164);
				WebErrorLog.Instance.StartLog(ex2, $"IsDisposed:{IsDisposed}//Line:{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 170);
				return new ReturnedSaveFuncInfo(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 171);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<LastPriceBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		if (items == null || items.Count() <= 0)
		{
			return new ReturnedSaveFuncInfo();
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				line = 1;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await DbRepository.SaveRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 185);
				line = 2;
				if (ret.HasError)
				{
					return ret;
				}
				line = 3;
				foreach (LastPriceBusiness item in items)
				{
					if (item != null)
					{
						await TimingCache.UpdateTimeAsync(new LastPriceCompositeKey
						{
							ProductGuid = item.PrdGuid,
							TafsilGuid = item.TafsilGuid
						}, item, tranName);
					}
				}
				line = 4;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"line:{line}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 197);
				ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 198);
			}
			return ret;
		}
	}

	private async Task TimingCache_OnCollect(object arg1, List<LastPriceCompositeKey> arg2)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("TimingCache_OnCollect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs"))
		{
			try
			{
				TimingCache.OnCollect -= TimingCache_OnCollect;
				_isCached = false;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TimingCache_OnCollect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 214);
				throw;
			}
		}
	}

	protected async Task LoadCacheAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor($"LoadCache___type:{typeof(LastPriceCacheRepository)}_cached:{isCached.ToString()}", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				if (isCached)
				{
					return;
				}
				await Locks.DbReadSemaphore.WaitAsync();
				try
				{
					if (isCached)
					{
						return;
					}
					IEnumerable<LastPriceBusiness> enumerable = await DbRepository.GetAllAsync(token, tranName);
					TimingCache.OnCollect += TimingCache_OnCollect;
					if (enumerable != null && enumerable.Any())
					{
						foreach (LastPriceBusiness item in enumerable)
						{
							await TimingCache.UpdateTimeAsync(new LastPriceCompositeKey
							{
								ProductGuid = item.PrdGuid,
								TafsilGuid = item.TafsilGuid
							}, item, tranName);
						}
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "LoadCacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LastPriceCacheRepository.cs", 245);
				throw;
			}
		}
	}

	public Task<IEnumerable<LastPriceBusiness>> GetAllAsync(Guid productGuid, Guid tafsilGuid, Document asType, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllAsync(productGuid, tafsilGuid, asType, token, tranName);
	}

	public Task<IEnumerable<LastPriceBusiness>> GetAllAsync(Guid orderGuid, Document asType, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllAsync(orderGuid, asType, token, tranName);
	}

	public void ResetCache()
	{
	}
}
