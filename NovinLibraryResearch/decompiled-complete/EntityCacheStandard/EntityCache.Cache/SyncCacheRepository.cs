using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace EntityCache.Cache;

public abstract class SyncCacheRepository<T> : CacheRepository<T>, ISyncRepository<T>, IEditableDelivery, IRepository<T>, INoGuidRepository<T>, IDisposable where T : class, IHasGuid, IHasContext
{
	protected ISyncRepository<T> DbSyncRepository { get; set; }

	public abstract override IItemAsyncMapper<T, T> Mapper { get; }

	protected SyncCacheRepository(ISyncRepository<T> dbRepo, int CachingTime, bool cachingEnable = true)
		: base((IRepository<T>)dbRepo, CachingTime, cachingEnable)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbSyncRepository = dbRepo;
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbSyncRepository.GetAllNotSentAsync(token, tranName);
	}

	public virtual async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		_ = 2;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			ReturnedSaveFuncInfo ret = await DbSyncRepository.SetSaveResult(item, token, tryCount, tranName);
			TimingCache.TryGetValue(item.Guid, out var temp);
			if (temp != null)
			{
				T val = await base.DbRepository.GetAsync(item.Guid, token, tranName);
				if (val != temp)
				{
					await Mapper.MapAsync(base.Context.Guid, val, temp);
				}
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\SyncCacheRepository.cs", 52);
			throw;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		_ = 2;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			ReturnedSaveFuncInfo ret = await DbSyncRepository.SetDeliveryAsync(del, token, tryCount, tranName);
			TimingCache.TryGetValue(del.Guid, out var temp);
			T val = await base.DbRepository.GetAsync(del.Guid, token, tranName);
			if (val == null)
			{
				return ret;
			}
			if (val != temp)
			{
				await Mapper.MapAsync(base.Context.Guid, val, temp);
			}
			return ret;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\SyncCacheRepository.cs", 75);
			throw;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				DbSyncRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\SyncCacheRepository.cs", 89);
		}
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		return DbSyncRepository.UpdateServerStatusAsync(guid, status, token, tranName);
	}
}
