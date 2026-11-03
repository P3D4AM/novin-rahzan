using System;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class CategoryGroupCacheRepository : CacheRepository<CategoryGroupBusiness>, ICategoryGroupRepository, IRepository<CategoryGroupBusiness>, INoGuidRepository<CategoryGroupBusiness>, IDisposable
{
	private ICategoryGroupRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<CategoryGroupBusiness, CategoryGroupBusiness> Mapper => CategoryGroupMapper.Instance;

	protected override TimingCache<Guid, CategoryGroupBusiness> TimingCache { get; set; }

	public CategoryGroupCacheRepository(ICategoryGroupRepository dbRepository, int CachingTime)
		: base((IRepository<CategoryGroupBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CategoryGroupBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
