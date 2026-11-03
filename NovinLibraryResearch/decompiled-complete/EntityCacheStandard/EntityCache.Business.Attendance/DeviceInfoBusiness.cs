using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using PacketParser.Filters.Attendance;

namespace EntityCache.Business.Attendance;

public class DeviceInfoBusiness : BaseBusiness<DeviceInfoBusiness>, IDeviceInfo, IHasContext, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public bool AutoRecieve { get; set; }

	public bool AutoSetDateTime { get; set; }

	public string CommunicationPass { get; set; }

	public string DefultGetWay { get; set; }

	public string Desc { get; set; }

	public string DeviceName { get; set; }

	public short DeviceNo { get; set; }

	public string DeviceType { get; set; }

	public string EngineVersion { get; set; }

	public string FwVersion { get; set; }

	public bool GetNewTaradodOnly { get; set; }

	public string HostIp { get; set; }

	public short Id { get; set; }

	public string Ip { get; set; }

	public string MacAddress { get; set; }

	public int MinCheck { get; set; }

	public DeviceModelEnumType Model { get; set; }

	public int PortRealtime { get; set; }

	public DateTime RdEnrollDataTime { get; set; }

	public string SerialNumberStr { get; set; }

	public object SourceObj { get; set; }

	public string SubnetMask { get; set; }

	public DateTime SyncTime { get; set; }

	public int TcpPort { get; set; }

	public TypeConnection TransferMode { get; set; }

	public DeviceManufacture TypeDeviceManufacture { get; set; }

	public bool UseDhcp { get; set; }

	public bool UseRealTime { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AttDatabaseManager;

	public override string ContextGuid { get; set; }

	public DeviceInfoBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<List<DeviceInfoBusiness>> GetAllAsync(string contextGuid, DeviceInfoFilter filter = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return (await ClsCache.GetCache(contextGuid).CacheRepository.DeviceInfo.GetAllAsync(filter, tranName, token))?.OrderBy((DeviceInfoBusiness p) => p.DeviceName)?.ToList();
	}

	public static Task<DeviceInfoBusiness> GetBySerialAsync(string contextGuid, string serial, string tranName, CancellationToken token)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DeviceInfo.GetBySerialAsync(serial, tranName, token);
	}

	public static Task<DeviceInfoBusiness> GetByNumberAsync(string contextGuid, short number, string tranName, CancellationToken token)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DeviceInfo.GetByNumberAsync(number, tranName, token);
	}

	public static Task<DeviceInfoBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DeviceInfo.GetAsync(guid, token, tranName);
	}

	public static async Task<PacketParser.BusinessLayer.Models.Attendance.MicroDeviceInfo> GetMicroAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (guid == Guid.Empty)
			{
				return null;
			}
			DeviceInfoBusiness deviceInfoBusiness = await GetAsync(contextGuid, guid, token, tranName);
			if (deviceInfoBusiness == null)
			{
				return null;
			}
			return new PacketParser.BusinessLayer.Models.Attendance.MicroDeviceInfo(deviceInfoBusiness.Guid, deviceInfoBusiness.Ip, deviceInfoBusiness.TcpPort, deviceInfoBusiness.DeviceNo, deviceInfoBusiness.CommunicationPass.ParseToInt(), EnDeviceGroup._290);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMicroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\DeviceInfoBusiness.cs", 75);
			throw;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.DeviceInfo;
	}

	public PacketParser.Attendance.Entities.MicroDeviceInfo MappToMicro()
	{
		throw new NotImplementedException();
	}
}
