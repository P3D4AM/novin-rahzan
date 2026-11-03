using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Filters.Attendance;

namespace EntityCache.Core.Repository.Attendance;

public interface IBioWriteDeviceLogRepository : IRepository<BioWriteDeviceLogBusiness>, INoGuidRepository<BioWriteDeviceLogBusiness>, IDisposable
{
	Task<IEnumerable<BioWriteDeviceLogModel>> GetAllAsync(BioWriteDeviceLogFilter filter, CancellationToken token, string tranName);
}
