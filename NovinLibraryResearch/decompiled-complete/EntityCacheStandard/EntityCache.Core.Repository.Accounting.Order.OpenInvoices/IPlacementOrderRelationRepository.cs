using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OpenInvoices;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting.Order.OpenInvoices;

public interface IPlacementOrderRelationRepository : INoGuidRepository<PlacementOrderRelationBusiness>, IDisposable
{
	Task<IEnumerable<PlacementOrderRelationReportBusiness>> GetAllModelAsync(Guid? placementGuid, Guid? orderGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(List<PlacementOrderRelationBusiness> list, CancellationToken token, string tranName);
}
