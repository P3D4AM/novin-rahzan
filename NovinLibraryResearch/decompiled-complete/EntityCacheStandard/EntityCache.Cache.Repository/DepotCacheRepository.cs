using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class DepotCacheRepository : CacheRepository<DepotBusiness>, IDepotRepository, ISyncRepository<DepotBusiness>, IEditableDelivery, IRepository<DepotBusiness>, INoGuidRepository<DepotBusiness>, IDisposable
{
	protected IDepotRepository DbDepotRepository { get; set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, DepotBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<DepotBusiness, DepotBusiness> Mapper => DepotMapper.Instance;

	public DepotCacheRepository(IRepository<DepotBusiness> dbRepository, int cachingTime)
		: base(dbRepository, cachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbDepotRepository = (IDepotRepository)dbRepository;
		TimingCache = new TimingCache<Guid, DepotBusiness>(base.ContextGuid, Mapper, base.CachingTime);
	}

	public Task<DepotBusiness> GetByNameAsync(string name, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbDepotRepository.GetByNameAsync(name, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				DbDepotRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\DepotCacheRepository.cs", 48);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		TimingCache?.ClearAll();
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return DbDepotRepository.GetAllNotSentAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		return DbDepotRepository.SetDeliveryAsync(del, token, tryCount, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		return DbDepotRepository.SetSaveResult(item, token, tryCount, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		return DbDepotRepository.UpdateServerStatusAsync(guid, status, token, tranName);
	}
}
