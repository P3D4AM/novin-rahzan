using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Business.Accounting.Bill.Reports;
using PacketParser.BusinessLayer.Models.Hotel;
using PacketParser.BusinessLayer.Models.Placement;
using PacketParser.Filters.Hotel;

namespace EntityCache.Core.Repository.Accounting.Bill;

public interface IPlacementRepository : IRepository<PlacementBusiness>, INoGuidRepository<PlacementBusiness>, IDisposable
{
	Task<PlacementBusiness> GetAsync(int number, CancellationToken token, string tranName);

	Task<int> GetNextNumber(CancellationToken token, string tranName);

	Task<List<Guid>> GetTypePlacementListAsync(CancellationToken token, string tranName);

	Task<List<RoomTypeModel>> GetTypeRoomCount(CancellationToken token, string tranName);

	Task<IEnumerable<PlacementReportsBusiness>> GetAllAsync(RoomFilter filter, CancellationToken token, string tranName);

	Task<IEnumerable<PlacementKardexViewModel>> GetAllKardexAsync(PlacementKardexFilter filter, CancellationToken token, string tranName);
}
