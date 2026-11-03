using System;
using EntityCache.Business.Accounting.Selection.ProductGroup;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Selection.ProductGroup;
using EntityCache.Mapping.BusinessMappers.Accounting.Selection.ProductGroup;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Selection;

public class PrdGroupSelectedListCacheRepository : CacheRepository<PrdGroupSelectedListBusiness>, IPrdGroupSelectedListRepository, IRepository<PrdGroupSelectedListBusiness>, INoGuidRepository<PrdGroupSelectedListBusiness>, IDisposable
{
	private IPrdGroupSelectedListRepository _dbRepository;

	public override IItemAsyncMapper<PrdGroupSelectedListBusiness, PrdGroupSelectedListBusiness> Mapper => PrdGroupSelectedMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, PrdGroupSelectedListBusiness> TimingCache { get; set; }

	public PrdGroupSelectedListCacheRepository(IPrdGroupSelectedListRepository dbRepository, int CachingTime)
		: base((IRepository<PrdGroupSelectedListBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, PrdGroupSelectedListBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
