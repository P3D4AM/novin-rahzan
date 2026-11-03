using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Attendance;

public class BiometricMapper : IItemAsyncMapper<BiometricBusiness, BiometricBusiness>, IItemAsyncMapper<WebBiometricData, BiometricBusiness>, IItemAsyncMapper<BiometricBusiness, WebBiometricData>
{
	public static BiometricMapper Instance { get; private set; } = new BiometricMapper();


	public async Task<BiometricBusiness> MapAsync(string contextGuid, BiometricBusiness source, BiometricBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new BiometricBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DeviceGroup = source.DeviceGroup;
			destination.PersonGuid = source.PersonGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			if (source.DetailList?.Any() ?? false)
			{
				await destination.AddDetailListAsync(await BiometricDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<BiometricDetailBusiness>(), tranName, token));
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BiometricMapper.cs", 51);
			throw;
		}
	}

	public async Task<BiometricBusiness> MapAsync(string contextGuid, WebBiometricData source, BiometricBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new BiometricBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DeviceGroup = source.DeviceGroup;
			destination.PersonGuid = source.PersonGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			if (source.DetailList?.Any() ?? false)
			{
				await destination.AddDetailListAsync(await BiometricDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<BiometricDetailBusiness>(), tranName, token));
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BiometricMapper.cs", 87);
			throw;
		}
	}

	public async Task<WebBiometricData> MapAsync(string contextGuid, BiometricBusiness source, WebBiometricData destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebBiometricData();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DeviceGroup = source.DeviceGroup;
			destination.PersonGuid = source.PersonGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.FingerPrintCount = source.FingerPrintCount;
			destination.HasPalm = source.HasPalm;
			destination.HasFace = source.HasFace;
			destination.HasCard = source.HasCard;
			destination.HasPassword = source.HasPassword;
			if (source.DetailList?.Any() ?? false)
			{
				WebBiometricData webBiometricData = destination;
				webBiometricData.DetailList = await BiometricDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<WebBiometricDetail>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BiometricMapper.cs", 124);
			throw;
		}
	}

	public Task<List<BiometricBusiness>> MapListAsync(string contextGuid, IEnumerable<BiometricBusiness> source, List<BiometricBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<BiometricBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBiometricData> source, List<BiometricBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebBiometricData>> MapListAsync(string contextGuid, IEnumerable<BiometricBusiness> source, List<WebBiometricData> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
