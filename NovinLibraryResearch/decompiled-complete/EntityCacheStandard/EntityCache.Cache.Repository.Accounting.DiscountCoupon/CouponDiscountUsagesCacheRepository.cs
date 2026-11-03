using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.DiscountCoupon;
using EntityCache.Mapping.BusinessMappers.Accounting.DiscountCoupon;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.DiscountCoupon;

public class CouponDiscountUsagesCacheRepository : CacheRepositoryNoGuiId<CouponDiscountUsagesBusiness, string>, ICouponDiscountUsagesRepository, INoGuidRepository<CouponDiscountUsagesBusiness>, IDisposable
{
	private ICouponDiscountUsagesRepository _dbRepository;

	public override IItemAsyncMapper<CouponDiscountUsagesBusiness, CouponDiscountUsagesBusiness> Mapper => CouponDiscountUsagesMapper.Instans;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<string, CouponDiscountUsagesBusiness> TimingCache { get; set; }

	public CouponDiscountUsagesCacheRepository(ICouponDiscountUsagesRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<CouponDiscountUsagesBusiness>)dbRepository, cachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, CouponDiscountUsagesBusiness>(base.ContextGuid, Mapper, cachingTime);
	}

	public Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllAsync(Guid couponGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(couponGuid, token, tranName);
	}
}
