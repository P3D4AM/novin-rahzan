using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.Entities;

namespace EntityCache.Business.OrderDepot.OrderSanad;

public interface IOrderSanadGenerator
{
	Task<SanadBusiness> GenerateSanadAsync(OrderBusiness order, OrderBusiness oldOrder, EnSystem system, CancellationToken token, string tranName);

	Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(OrderBusiness order, EnSystem system, CancellationToken token, string tranName);
}
