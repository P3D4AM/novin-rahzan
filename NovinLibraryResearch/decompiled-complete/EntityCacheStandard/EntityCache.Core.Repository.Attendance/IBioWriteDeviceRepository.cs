using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities.CompositeKeys;
using PacketParser.EventArgs;

namespace EntityCache.Core.Repository.Attendance;

public interface IBioWriteDeviceRepository : INoGuidRepository<BioWriteDeviceBusiness>, IDisposable
{
	Task<BioWriteDeviceBusiness> FetchNextForSendAsync(Guid deviceGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse<BioWriteDeviceCompositeKey> item, CancellationToken token, string tranName);

	Task<IEnumerable<BioWriteDeviceModel>> GetAllAync(string contextGuid, CancellationToken token, string tranName);
}
