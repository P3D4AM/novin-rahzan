using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class RegionCacheRepository : CacheRepository<RegionBusiness>, IRegionRepository, IRepository<RegionBusiness>, INoGuidRepository<RegionBusiness>, IDisposable
{
	private IRegionRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, RegionBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<RegionBusiness, RegionBusiness> Mapper => RegionMapper.Instance;

	public RegionCacheRepository(IRegionRepository dbRepository, int CachingTime)
		: base((IRepository<RegionBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, RegionBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<RegionBusiness>> GetAllAsync(Guid cityGuid, AdvertiseType type, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(cityGuid, type, status, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveByTypeAsync(AdvertiseType type, short tryCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.RemoveByTypeAsync(type, tryCount, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\RegionCacheRepository.cs", 49);
		}
	}
}
