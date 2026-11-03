using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Attendance;

namespace EntityCache.Core.Repository;

public interface IDeviceInfoRepository : ISyncRepository<DeviceInfoBusiness>, IEditableDelivery, IRepository<DeviceInfoBusiness>, INoGuidRepository<DeviceInfoBusiness>, IDisposable
{
	Task<DeviceInfoBusiness> GetBySerialAsync(string serial, string tranName, CancellationToken token);

	Task<DeviceInfoBusiness> GetByNumberAsync(short number, string tranName, CancellationToken token);

	Task<List<DeviceInfoBusiness>> GetAllAsync(DeviceInfoFilter filter, string tranName, CancellationToken token);
}
