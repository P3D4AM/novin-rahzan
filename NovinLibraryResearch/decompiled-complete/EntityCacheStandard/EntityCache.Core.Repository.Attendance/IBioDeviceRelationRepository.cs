using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Filters.Attendance;

namespace EntityCache.Core.Repository.Attendance;

public interface IBioDeviceRelationRepository : INoGuidRepository<BioDeviceRelationBusiness>, IDisposable
{
	Task<BioDeviceRelationBusiness> GetAsync(Guid bioGuid, Guid deviceGuid, CancellationToken token, string tranName);

	Task<BioDeviceRelationBusiness> GetAsync(Guid deviceGuid, int hardwareCode, CancellationToken token, string tranName);

	Task<BioDeviceRelationBusiness> GetByPersonAsync(Guid deviceGuid, Guid personGuid, CancellationToken token, string tranName);

	Task<IEnumerable<BioDeviceRelationBusiness>> GetAllAsync(BioDeviceRelationFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid bioGuid, Guid deviceGuid, CancellationToken token, string tranName);

	Task<IEnumerable<BioDeviceRelationModel>> GetAllAsync(Guid deviceGuid, CancellationToken token, string tranName);
}
