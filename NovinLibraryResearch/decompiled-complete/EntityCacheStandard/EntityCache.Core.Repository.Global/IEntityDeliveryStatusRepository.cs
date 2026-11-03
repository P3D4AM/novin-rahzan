using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Global;

public interface IEntityDeliveryStatusRepository : INoGuidRepository<EntityDeliveryStatusBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid entityGuid, EntityPart entityPart, bool isTemp, CancellationToken token, string tranName);

	Task<IEnumerable<EntityDeliveryStatusBusiness>> GetAllAsync(EntityPart entityPart, bool isTemp, CancellationToken token, string tranName);
}
