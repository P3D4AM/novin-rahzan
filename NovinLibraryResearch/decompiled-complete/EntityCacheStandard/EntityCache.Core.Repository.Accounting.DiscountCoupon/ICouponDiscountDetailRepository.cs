using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;

namespace EntityCache.Core.Repository.Accounting.DiscountCoupon;

public interface ICouponDiscountDetailRepository : IPersistenceMasterRepository<CouponDiscountDetailBusiness>, IDisposable
{
	Task GetAllAsync(CouponDiscountBusiness master, CancellationToken token, string tranName);
}
