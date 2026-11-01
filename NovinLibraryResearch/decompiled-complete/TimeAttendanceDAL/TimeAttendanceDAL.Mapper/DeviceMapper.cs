using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Device;

namespace TimeAttendanceDAL.Mapper;

public class DeviceMapper : IItemAsyncMapper<DeviceInfo, WebAttendanceDeviceInfo>
{
	public static DeviceMapper Instance { get; } = new DeviceMapper();


	public async Task<WebAttendanceDeviceInfo> MapAsync(string contextGuid, DeviceInfo source, WebAttendanceDeviceInfo destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return await Task.FromResult<WebAttendanceDeviceInfo>(destination);
			}
			if (destination == null)
			{
				destination = new WebAttendanceDeviceInfo();
			}
			destination.Status = source.Status;
			destination.Description = source.Desc;
			destination.Guid = source.Guid;
			destination.Name = source.DeviceName;
			destination.PortNumber = source.TcpPort;
			destination.SerialIdString = source.SerialNumberStr;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return await Task.FromResult<WebAttendanceDeviceInfo>(destination);
	}

	public Task<List<WebAttendanceDeviceInfo>> MapListAsync(string contextGuid, IEnumerable<DeviceInfo> source, List<WebAttendanceDeviceInfo> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<DeviceInfo, WebAttendanceDeviceInfo>(source, contextGuid, (IItemAsyncMapper<DeviceInfo, WebAttendanceDeviceInfo>)this, destination, tranName, (short)20);
	}
}
