using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace EntityCache.Cache;

public abstract class CacheEntityRepository<T, K> : IEntityRepository<T, K>, IEditableDelivery, IDisposable where T : class, IEntityCache, IHasSanadNo, IHasContext
{
	private IEntityRepository<T, K> dbRepository;

	public bool IsDisposed { get; private set; }

	public WebContextHolder Context => dbRepository?.Context;

	public string ContextGuid => Context?.Guid;

	public abstract IItemAsyncMapper<T, T> Mapper { get; }

	public TimingCache<Guid, T> TimingCache { get; private set; }

	public int CachingTime
	{
		get
		{
			return TimingCache.CachingTime;
		}
		set
		{
			TimingCache.CachingTime = value;
		}
	}

	public event Func<object, List<Guid>, Task> OnCollect;

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo obj = await dbRepository.SetSanadNoAsync(guid, sanadNo, token, tranName);
		if (!obj.HasError)
		{
			TimingCache.TryGetValue(guid, out var ret);
			if (ret != null)
			{
				ret.SanadNo = sanadNo;
			}
		}
		return obj;
	}

	public CacheEntityRepository(IEntityRepository<T, K> dbRepository, int CachingTime)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs"))
		{
			try
			{
				this.dbRepository = dbRepository;
				TimingCache = new TimingCache<Guid, T>(ContextGuid, Mapper, CachingTime);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 49);
				throw;
			}
		}
	}

	public virtual async Task Add2CacheAsync(T item, CancellationToken token, string tranName = "")
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (item.Guid == Guid.Empty)
		{
			throw new ArgumentException("Guid is Empty");
		}
		await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
	}

	public virtual async Task<T> GetAsync(Guid guid, CancellationToken token, string tranName = "")
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (TimingCache.TryGetValue(guid, out var ret) && ret != null)
		{
			return ret;
		}
		ret = await dbRepository.GetAsync(guid, token, tranName);
		TimingCache.UpdateTimeAsync(guid, ret);
		return ret;
	}

	public List<K> GetAllByDeliveryStatus(bool isDelivered)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return GetAllByDeliveryStatus(isDelivered);
	}

	public Task<List<K>> GetAllByDeliveryStatusAsync(bool isDelivered)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return GetAllByDeliveryStatusAsync(isDelivered);
	}

	public virtual async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		TimingCache.TryRemove(item, out var _);
		return await dbRepository.RemoveAsync(item, tryCount, token, tranName);
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		await TimingCache.UpdateTimeAsync(item?.Guid ?? Guid.Empty, item, tranName);
		return await dbRepository.SaveAsync(item, tryCount, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			TimingCache.TryRemove(item?.Guid ?? Guid.Empty, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await dbRepository.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 116);
			T temp = await GetAsync(item?.Guid ?? Guid.Empty, token, tranName);
			if (temp?.GetType() == typeof(OrderBusiness))
			{
				OrderBusiness x = (OrderBusiness)(object)temp;
				await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.ClearCachedItemAsync(x?.ReceptionGuid ?? Guid.Empty, token, tranName);
				await ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.ClearCachedItemAsync(x?.PardakhtGuid ?? Guid.Empty, token, tranName);
				x.ReceptionGuid = Guid.Empty;
				x.PardakhtGuid = Guid.Empty;
			}
			T val = await dbRepository.GetAsync(item?.Guid ?? Guid.Empty, token, tranName);
			if (val != temp)
			{
				await Mapper.MapAsync(ContextGuid, val, temp, tranName, token);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 134);
			WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 135);
		}
		return ret;
	}

	public virtual async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery item, CancellationToken token, short tryCount, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			line = 10;
			if (item == null)
			{
				throw new NullReferenceException("item is null!!");
			}
			line = 11;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await dbRepository.SetDeliveryAsync(item, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 150);
			line = 2;
			T val = await dbRepository.GetAsync(item?.Guid ?? Guid.Empty, token, tranName);
			line = 3;
			if (val != null)
			{
				line = 4;
				TimingCache.UpdateTimeAsync(val.Guid, val);
				line = 5;
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 161);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}\r\n" + $"isDisposed:{IsDisposed}\r\n" + $"token is cancelled:{token.IsCancellationRequested}", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 164);
			ret.AddReturnedValue(ex2, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 167);
		}
		return ret;
	}

	public Task<List<T>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllNotSentAsync(token, tranName);
	}

	public Task<K> GetReportAsync(Guid guid, CancellationToken token, string tranName = "")
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetReportAsync(guid, token, tranName);
	}

	public virtual void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbRepository?.Dispose();
				dbRepository = null;
				TimingCache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheEntityRepository.cs", 194);
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		return dbRepository.DbCountAsync(status, token, tranName);
	}

	public void ClearCachedItem(Guid guid)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		TimingCache.TryRemove(guid, out var _);
	}

	public async Task ClearCachedItemAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		TimingCache.TryRemove(guid, out var _);
	}

	public async Task ClearCachedAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		TimingCache.ClearAll();
	}

	public virtual void ResetCache()
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		TimingCache.ClearAll();
	}
}
