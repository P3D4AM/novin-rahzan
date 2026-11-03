using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OrderCouponCalculation;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting.Order;

public interface IOrderDetailCouponCalculationRepository : IRepository<OrderDetailCouponCalculationBusiness>, INoGuidRepository<OrderDetailCouponCalculationBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<int>> GetNextRefrencedAsync(CancellationToken token, string tranName);
}
