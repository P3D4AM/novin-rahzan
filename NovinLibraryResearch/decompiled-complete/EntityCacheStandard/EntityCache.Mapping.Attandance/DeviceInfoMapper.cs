using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Attandance;

public class DeviceInfoMapper : IItemAsyncMapper<DeviceInfoBusiness, DeviceInfoBusiness>
{
	public static DeviceInfoMapper Instance { get; private set; } = new DeviceInfoMapper();


	public Task<DeviceInfoBusiness> MapAsync(string contextGuid, DeviceInfoBusiness source, DeviceInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new DeviceInfoBusiness(contextGuid);
			}
			destination.Guid = Guid.NewGuid();
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
			destination.SerialNumberStr = source.SerialNumberStr;
			destination.Ip = source.Ip;
			destination.Status = source.Status;
			destination.Desc = source.Desc;
			destination.DeviceNo = source.DeviceNo;
			destination.DeviceType = source.DeviceType;
			destination.DeviceName = source.DeviceName;
			destination.DefultGetWay = source.DefultGetWay;
			destination.AutoRecieve = source.AutoRecieve;
			destination.AutoSetDateTime = source.AutoSetDateTime;
			destination.ChangeStatus = source.ChangeStatus;
			destination.CommunicationPass = source.CommunicationPass;
			destination.FwVersion = source.FwVersion;
			destination.HostIp = source.HostIp;
			destination.GetNewTaradodOnly = source.GetNewTaradodOnly;
			destination.Id = source.Id;
			destination.MacAddress = source.MacAddress;
			destination.MinCheck = source.MinCheck;
			destination.Model = source.Model;
			destination.PortRealtime = source.PortRealtime;
			destination.TcpPort = source.TcpPort;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Attandance\\DeviceInfoMapper.cs", 53);
			throw;
		}
	}

	public Task<List<DeviceInfoBusiness>> MapListAsync(string contextGuid, IEnumerable<DeviceInfoBusiness> source, List<DeviceInfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
