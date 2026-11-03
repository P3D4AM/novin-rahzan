using System;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Bill;
using EntityCache.Mapping.BusinessMappers.Accounting.Bill;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Bill;

public class TempBillCacheRepository : CacheRepositoryNoGuiId<TempBillBusiness, string>, ITempBillRepository, INoGuidRepository<TempBillBusiness>, IDisposable
{
	private ITempBillRepository _dbRepository;

	public override IItemAsyncMapper<TempBillBusiness, TempBillBusiness> Mapper => TempBillMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<string, TempBillBusiness> TimingCache { get; set; }

	public TempBillCacheRepository(ITempBillRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<TempBillBusiness>)dbRepository, cachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, TempBillBusiness>(base.ContextGuid, Mapper, cachingTime);
	}
}
