using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Attendance;

public class BioWriteDeviceLogMapper : IItemAsyncMapper<BioWriteDeviceLogBusiness, BioWriteDeviceLogBusiness>
{
	public static BioWriteDeviceLogMapper Instance { get; private set; } = new BioWriteDeviceLogMapper();


	public Task<BioWriteDeviceLogBusiness> MapAsync(string contextGuid, BioWriteDeviceLogBusiness source, BioWriteDeviceLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BioWriteDeviceLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.DeviceGuid = source.DeviceGuid;
			destination.HardwareCode = source.HardwareCode;
			destination.HardwareName = source.HardwareName;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.Result = source.Result;
			destination.BioGuid = source.BioGuid;
			destination.UserGuid = source.UserGuid;
			destination.Modified = source.Modified;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BioWriteDeviceLogMapper.cs", 40);
			throw;
		}
	}

	public Task<List<BioWriteDeviceLogBusiness>> MapListAsync(string contextGuid, IEnumerable<BioWriteDeviceLogBusiness> source, List<BioWriteDeviceLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
