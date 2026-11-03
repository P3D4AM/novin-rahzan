using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Attendance;

public class BioWriteDeviceMapper : IItemAsyncMapper<BioWriteDeviceBusiness, BioWriteDeviceBusiness>, IItemAsyncMapper<WebBioWriteDevice, BioWriteDeviceBusiness>, IItemAsyncMapper<BioWriteDeviceBusiness, WebBioWriteDevice>
{
	public static BioWriteDeviceMapper Instance { get; private set; } = new BioWriteDeviceMapper();


	public Task<BioWriteDeviceBusiness> MapAsync(string contextGuid, BioWriteDeviceBusiness source, BioWriteDeviceBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new ArgumentNullException("Please Pass a new Object");
		}
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			destination.ContextGuid = contextGuid;
			destination.BioGuid = source.BioGuid;
			destination.DeviceGuid = source.DeviceGuid;
			destination.HardwareCode = source.HardwareCode;
			destination.HarwareName = source.HarwareName;
			destination.InsertedDateM = source.InsertedDateM;
			destination.TryCount = source.TryCount;
			destination.NextTry = source.NextTry;
			destination.UserGuid = source.UserGuid;
			destination.MessageType = source.MessageType;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BioWriteDeviceMapper.cs", 43);
			throw;
		}
	}

	public Task<BioWriteDeviceBusiness> MapAsync(string contextGuid, WebBioWriteDevice source, BioWriteDeviceBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new ArgumentNullException("Please Pass a new Object");
		}
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			destination.ContextGuid = contextGuid;
			destination.BioGuid = source.BioGuid;
			destination.DeviceGuid = source.DeviceGuid;
			destination.HardwareCode = source.HardwareCode;
			destination.HarwareName = source.HarwareName;
			destination.InsertedDateM = source.InsertedDateM;
			destination.TryCount = source.TryCount;
			destination.NextTry = source.NextTry;
			destination.UserGuid = source.UserGuid;
			destination.MessageType = source.MessageType;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BioWriteDeviceMapper.cs", 71);
			throw;
		}
	}

	public async Task<WebBioWriteDevice> MapAsync(string contextGuid, BioWriteDeviceBusiness source, WebBioWriteDevice destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebBioWriteDevice();
			}
			destination.BioGuid = source.BioGuid;
			destination.DeviceGuid = source.DeviceGuid;
			destination.HardwareCode = source.HardwareCode;
			destination.HarwareName = source.HarwareName;
			destination.InsertedDateM = source.InsertedDateM;
			destination.TryCount = source.TryCount;
			destination.NextTry = source.NextTry;
			destination.UserGuid = source.UserGuid;
			destination.MessageType = source.MessageType;
			BiometricBusiness biometricBusiness = await BiometricBusiness.GetAsync(contextGuid, source.BioGuid, token, tranName);
			if (biometricBusiness != null)
			{
				WebBioWriteDevice webBioWriteDevice = destination;
				webBioWriteDevice.BiometricData = await BiometricMapper.Instance.MapAsync(contextGuid, biometricBusiness, new WebBiometricData(), tranName, token);
			}
			DeviceInfoBusiness deviceInfoBusiness = await DeviceInfoBusiness.GetAsync(contextGuid, source.DeviceGuid, token, tranName);
			if (deviceInfoBusiness != null)
			{
				destination.DeviceIp = deviceInfoBusiness.Ip;
				destination.DevicePort = deviceInfoBusiness.TcpPort;
				destination.DeviceNumber = deviceInfoBusiness.DeviceNo;
				destination.DevicePassword = deviceInfoBusiness.CommunicationPass.ParseToInt();
			}
			BioDeviceRelationBusiness bioDeviceRelationBusiness = await BioDeviceRelationBusiness.GetAsync(contextGuid, source.BioGuid, source.DeviceGuid, token, tranName);
			if (bioDeviceRelationBusiness != null)
			{
				destination.Privilage = bioDeviceRelationBusiness.Privilage;
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BioWriteDeviceMapper.cs", 115);
			throw;
		}
	}

	public Task<List<BioWriteDeviceBusiness>> MapListAsync(string contextGuid, IEnumerable<BioWriteDeviceBusiness> source, List<BioWriteDeviceBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<BioWriteDeviceBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBioWriteDevice> source, List<BioWriteDeviceBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebBioWriteDevice>> MapListAsync(string contextGuid, IEnumerable<BioWriteDeviceBusiness> source, List<WebBioWriteDevice> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
