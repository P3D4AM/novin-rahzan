using System;
using EntityCache.Business.Accounting.DiscountCoupon;

namespace EntityCache.Core.Repository.Accounting.DiscountCoupon;

public interface ICouponDiscountEntityRelationRepository : INoGuidRepository<CouponDiscountEntityRelationBusiness>, IDisposable
{
}
