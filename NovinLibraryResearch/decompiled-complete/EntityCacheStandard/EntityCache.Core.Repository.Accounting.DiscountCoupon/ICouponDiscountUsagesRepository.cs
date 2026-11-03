using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;

namespace EntityCache.Core.Repository.Accounting.DiscountCoupon;

public interface ICouponDiscountUsagesRepository : INoGuidRepository<CouponDiscountUsagesBusiness>, IDisposable
{
	Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllAsync(Guid couponGuid, CancellationToken token, string tranName);
}
