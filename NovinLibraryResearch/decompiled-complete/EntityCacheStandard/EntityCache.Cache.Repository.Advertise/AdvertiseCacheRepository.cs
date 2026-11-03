using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class AdvertiseCacheRepository : CacheRepository<AdvertiseBusiness>, IAdvertiseRepository, IRepository<AdvertiseBusiness>, INoGuidRepository<AdvertiseBusiness>, IDisposable
{
	private IAdvertiseRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AdvertiseBusiness, AdvertiseBusiness> Mapper => AdvertiseMapper.Instance;

	protected override TimingCache<Guid, AdvertiseBusiness> TimingCache { get; set; }

	public AdvertiseCacheRepository(IAdvertiseRepository dbRepository, int CachingTime)
		: base((IRepository<AdvertiseBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AdvertiseBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<IEnumerable<AdvertiseBusiness>> GetAllAsync(Guid groupGuid, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(groupGuid, status, tranName);
	}
}
