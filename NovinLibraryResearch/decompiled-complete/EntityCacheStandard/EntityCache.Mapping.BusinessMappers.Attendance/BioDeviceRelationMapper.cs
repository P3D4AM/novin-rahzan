using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Attendance;

public class BioDeviceRelationMapper : IItemAsyncMapper<BioDeviceRelationBusiness, BioDeviceRelationBusiness>
{
	public static BioDeviceRelationMapper Instance { get; private set; } = new BioDeviceRelationMapper();


	public Task<BioDeviceRelationBusiness> MapAsync(string contextGuid, BioDeviceRelationBusiness source, BioDeviceRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BioDeviceRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.BioGuid = source.BioGuid;
			destination.DeviceGuid = source.DeviceGuid;
			destination.HardwareCode = source.HardwareCode;
			destination.Privilage = source.Privilage;
			destination.FromDateM = source.FromDateM;
			destination.ToDateM = source.ToDateM;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BioDeviceRelationMapper.cs", 38);
			throw;
		}
	}

	public Task<List<BioDeviceRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<BioDeviceRelationBusiness> source, List<BioDeviceRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
