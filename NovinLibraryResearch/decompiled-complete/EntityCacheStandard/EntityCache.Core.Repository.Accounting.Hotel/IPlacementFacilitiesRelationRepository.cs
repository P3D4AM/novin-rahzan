using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting.Hotel;

public interface IPlacementFacilitiesRelationRepository : INoGuidRepository<PlacementFacilitiesRelationBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid placementGuid, CancellationToken token, string tranName);

	Task<IEnumerable<PlacementFacilitiesRelationBusiness>> GetAllAsync(Guid placementGuid, CancellationToken token, string tranName);
}
