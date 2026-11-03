using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Core.Repository;

public interface IOrderDetailRepository : IPersistenceMasterRepository<OrderDetailBusiness>, IDisposable
{
	Task GetAllAsync(OrderBusiness order, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, Document orderType, CancellationToken token, string tranName);

	Task<Guid> GetMasterGuidAsync(Guid guid, CancellationToken token, string tranName);
}
