using System;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Bill;
using EntityCache.Mapping.BusinessMappers.Accounting.Bill;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Bill;

public class BillMasterCacheRepository : CacheRepository<BillMasterBusiness>, IBillMasterRepository, IRepository<BillMasterBusiness>, INoGuidRepository<BillMasterBusiness>, IDisposable
{
	private IBillMasterRepository _dbRepository;

	public override IItemAsyncMapper<BillMasterBusiness, BillMasterBusiness> Mapper => BillMasterMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, BillMasterBusiness> TimingCache { get; set; }

	public BillMasterCacheRepository(IBillMasterRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<BillMasterBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, BillMasterBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
