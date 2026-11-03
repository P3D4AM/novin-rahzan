using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.DiscountCoupon.Reports;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.CouponDiscount;

namespace EntityCache.Core.Repository.Accounting.DiscountCoupon;

public interface ICouponDiscountRepository : ISyncRepository<CouponDiscountBusiness>, IEditableDelivery, IRepository<CouponDiscountBusiness>, INoGuidRepository<CouponDiscountBusiness>, IDisposable
{
	Task<IEnumerable<CouponDiscountReportBusiness>> GetAllAsync(CouponFilter filters, CancellationToken token, string tranName);

	Task<IEnumerable<Guid>> GetAllOrderRelationAsync(Guid tafsilGuid, DateTime orderDate, decimal orderSum, int refId, Guid? specificCouponGuid, long mobile, CancellationToken token, string tranName);

	Task<CouponDiscountReportBusiness> GetAsync(string title, CancellationToken token, string tranName);

	Task<IEnumerable<Guid>> GetAllForScheduleAsync(CancellationToken token, string tranName);

	Task<int> GetUsedCountAsync(Guid guid, CancellationToken token, string tranName);

	Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllUagesAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<Guid> couponGuids, ServerStatusForClient status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateServerStatusByGiftGuidAsync(Guid giftGuid, ServerStatusForClient status, CancellationToken token, string tranName);
}
