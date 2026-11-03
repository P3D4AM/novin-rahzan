using System;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Bill;
using EntityCache.Mapping.BusinessMappers.Accounting.Bill;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Hotel;

public class FacilitiesCacheRepository : CacheRepository<FacilitiesBusiness>, IFacilitiesRepository, IRepository<FacilitiesBusiness>, INoGuidRepository<FacilitiesBusiness>, IDisposable
{
	private IFacilitiesRepository _dbRepository;

	public override IItemAsyncMapper<FacilitiesBusiness, FacilitiesBusiness> Mapper => FacilitiesMapper.Instance;

	public override bool HasStatus
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasModified
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	protected override TimingCache<Guid, FacilitiesBusiness> TimingCache { get; set; }

	public FacilitiesCacheRepository(IFacilitiesRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<FacilitiesBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, FacilitiesBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
