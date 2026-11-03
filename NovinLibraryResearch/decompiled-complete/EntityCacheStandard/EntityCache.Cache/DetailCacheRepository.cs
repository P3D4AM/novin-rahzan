using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace EntityCache.Cache;

public abstract class DetailCacheRepository<T, TCompact> : INpDisposable, IDisposable, IDetailRepository<T>, IEditableDelivery where T : class, IHasGuid, IHasContext, IDeliverableEntity where TCompact : class, IHasGuid, IHasContext
{
	public bool IsDisposed { get; private set; }

	internal IDetailRepository<T> DbRepository { get; private set; }

	protected ICompactCacheRepository<TCompact> CompactCacheRepository { get; set; }

	public WebContextHolder Context => DbRepository?.Context;

	public abstract IItemAsyncMapper<T, T> MapperTtoT { get; }

	public abstract IItemAsyncMapper<T, TCompact> MapperTtoTCompact { get; }

	protected AsyncTimingCache<Guid, T> Entities { get; private set; }

	public int CachingTime
	{
		get
		{
			return Entities.CachingTime;
		}
		set
		{
			Entities.CachingTime = value;
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		return DbRepository.DbCountAsync(status, token, tranName);
	}

	public DetailCacheRepository(ICompactCacheRepository<TCompact> compactCacheRepository, IDetailRepository<T> dbRepository, int CachingTimeSeconds)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs"))
		{
			try
			{
				DbRepository = dbRepository;
				CompactCacheRepository = compactCacheRepository;
				Entities = new AsyncTimingCache<Guid, T>(Context?.Guid ?? "", MapperTtoT, CachingTimeSeconds);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 47);
				throw;
			}
		}
	}

	public int Count()
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return Entities.Count();
	}

	public async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Entities.Clear(items);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await DbRepository.DeleteRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 66);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 70);
				ret.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 71);
			}
			return ret;
		}
	}

	public virtual async Task<T> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				line = 1;
				if (Entities == null)
				{
					throw new NullReferenceException("Entities");
				}
				line = 2;
				if (Entities.TryGetValue(guid, out var ret))
				{
					line = 3;
					return ret;
				}
				line = 4;
				T result = await DbRepository.GetAsync(guid, token, tranName);
				line = 5;
				if (result == null)
				{
					return null;
				}
				line = 6;
				await Entities.UpdateTimeAsync(result.Guid, result);
				line = 7;
				return result;
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
				WebErrorLog.Instance.StartLog(ex3, $"#Line_{line}\r\n" + $"IsDisposed:{IsDisposed}\r\n" + $"DbRepositoryIsNull:{DbRepository == null}\r\n" + string.Format("cacheIsNullOrDisposed:{0}\r\n", ClsCache.GetCache(Context?.Guid ?? "")?.IsDisposed ?? true) + "Context:" + Json.ToStringJson(Context, indented: true), "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 109);
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
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs"))
		{
			int num = 0;
			try
			{
				if (Entities == null)
				{
					throw new NullReferenceException("Entities");
				}
				num = 1;
				if (Entities.TryGetValue(guid, out var ret))
				{
					num = 2;
					return ret;
				}
				num = 3;
				T val = DbRepository.Get(guid, token, tranName);
				num = 4;
				if (val == null)
				{
					return null;
				}
				num = 5;
				Entities.UpdateTimeAsync(val.Guid, val);
				num = 6;
				return val;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{num}", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 149);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await DbRepository.RemoveAsync(item, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 162);
				Entities.TryRemove(item, out var _);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CompactCacheRepository.RemoveFromEntitiesAsync(item, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 164);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 168);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 169);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Entities.Clear(items);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await DbRepository.KeepRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 183);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 187);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 188);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await DbRepository.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 201);
				await Entities.UpdateTimeAsync(item.Guid, item, tranName);
				TCompact val = await CompactCacheRepository.GetEntityAsync(item.Guid, loadFromDatabaseIfNotCached: false, token, tranName);
				if (val != null)
				{
					await MapperTtoTCompact.MapAsync(Context.Guid, item, val, "", token);
				}
				else
				{
					returnedSaveFuncInfo = ret;
					ICompactCacheRepository<TCompact> compactCacheRepository = CompactCacheRepository;
					returnedSaveFuncInfo.AddReturnedValue(await compactCacheRepository.AddOrUpdateEntitiesAsync(await MapperTtoTCompact.MapAsync(Context.Guid, item, null, tranName, token), tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 207);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 216);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 217);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<T> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await DbRepository.SaveRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 232);
				line = 1;
				foreach (T item in items)
				{
					await Entities.UpdateTimeAsync(item.Guid, item, tranName);
					line = 2;
					TCompact val = await CompactCacheRepository.GetEntityAsync(item.Guid, loadFromDatabaseIfNotCached: false, token, tranName);
					line = 3;
					if (val != null)
					{
						await MapperTtoTCompact.MapAsync(Context.Guid, item, val, "", token);
						line = 4;
						continue;
					}
					returnedSaveFuncInfo = ret;
					ICompactCacheRepository<TCompact> compactCacheRepository = CompactCacheRepository;
					returnedSaveFuncInfo.AddReturnedValue(await compactCacheRepository.AddOrUpdateEntitiesAsync(await MapperTtoTCompact.MapAsync(Context.Guid, item, null, "", token), tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 247);
					line = 5;
				}
			}
			catch (Exception ex)
			{
				if (tryCount >= 0)
				{
					await Task.Delay(1000);
					DetailCacheRepository<T, TCompact> detailCacheRepository = this;
					short num = checked((short)(tryCount - 1));
					tryCount = num;
					return await detailCacheRepository.SaveRangeAsync(items, num, token, tranName);
				}
				WebErrorLog.Instance.StartLog(ex, $"Type Error:{typeof(T)},Line:{line}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 259);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 260);
			}
			return ret;
		}
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
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await DbRepository.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 273);
			T dbitem = await DbRepository.GetAsync(item.Guid, token, tranName);
			returnedSaveFuncInfo = ret;
			ICompactCacheRepository<TCompact> compactCacheRepository = CompactCacheRepository;
			returnedSaveFuncInfo.AddReturnedValue(await compactCacheRepository.AddOrUpdateEntitiesAsync(await MapperTtoTCompact.MapAsync(Context.Guid, dbitem, null, "", token), tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 275);
			Entities.TryGetValue(item.Guid, out var temp);
			if (dbitem != temp)
			{
				await MapperTtoT.MapAsync(Context.Guid, dbitem, temp, "", token);
				await Entities.UpdateTimeAsync(item.Guid, temp, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 285);
			ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 286);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await DbRepository.SetDeliveryAsync(del, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 300);
			T dbitem = await DbRepository.GetAsync(del.Guid, token, tranName);
			if (dbitem == null)
			{
				return ret;
			}
			returnedSaveFuncInfo = ret;
			ICompactCacheRepository<TCompact> compactCacheRepository = CompactCacheRepository;
			returnedSaveFuncInfo.AddReturnedValue(await compactCacheRepository.AddOrUpdateEntitiesAsync(await MapperTtoTCompact.MapAsync(Context.Guid, dbitem, null, "", token), tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 303);
			Entities.TryGetValue(del.Guid, out var temp);
			if (dbitem != temp)
			{
				await MapperTtoT.MapAsync(Context.Guid, dbitem, temp, "", token);
				await Entities.UpdateTimeAsync(del.Guid, temp, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 313);
			ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 314);
		}
		return ret;
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAllNotSentAsync(token, tranName);
	}

	public virtual void Dispose()
	{
		int num = 0;
		try
		{
			if (!IsDisposed)
			{
				num = 2;
				IsDisposed = true;
				num = 3;
				CompactCacheRepository?.Dispose();
				num = 4;
				CompactCacheRepository = null;
				num = 5;
				DbRepository?.Dispose();
				num = 6;
				DbRepository = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\DetailCacheRepository.cs", 343);
			throw;
		}
	}
}
