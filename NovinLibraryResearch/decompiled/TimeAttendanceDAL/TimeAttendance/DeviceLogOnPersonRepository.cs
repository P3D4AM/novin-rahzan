using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendance;

public class DeviceLogOnPersonRepository : IDeviceLogOnPersonRepository
{
	public string ContextGuid { get; set; }

	public DeviceLogOnPersonRepository(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<LogOnPersonSaveReqult>> SaveAsync(DeviceLogOnPerson item)
	{
		ReturnedSaveFuncInfoWithValue<LogOnPersonSaveReqult> ret = new ReturnedSaveFuncInfoWithValue<LogOnPersonSaveReqult>();
		LogOnPersonSaveReqult retValue = (ret.value = new LogOnPersonSaveReqult());
		try
		{
			if (string.IsNullOrEmpty(item.DeviceSerial))
			{
				ret.AddError("سریال دستگاه نامشخص است. لذا تردد دریافت شده، نادیده گرفته می شود", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceLogOnPersonRepository.cs", 35);
				return ret;
			}
			DeviceInfo device = null;
			ReturnedSaveFuncInfoWithValue<DeviceInfo> val2 = await DeviceInfo.CheckAndGetDeviceInfoAsync(ContextGuid, item.DeviceSerial, (short)item.DeviceNo, default(CancellationToken));
			if (val2 != null)
			{
				device = val2.value;
			}
			List<HardwareCodeCus> list = await HardwareCodeCus.GetAllAsync(ContextGuid);
			short deviceId = device?.Id ?? 0;
			HardwareCodeCus hardwareCodeCus = null;
			if (list != null && list.Count > 0)
			{
				hardwareCodeCus = list.SingleOrDefault((HardwareCodeCus p) => p.DeviceId == deviceId && p.HardwareCode == item.PersonNo.ToString());
			}
			retValue.LogonPersonId = await new LogOnPersonel(ContextGuid)
			{
				DateTimeLogM = item.FixedDateM,
				PersonId = (hardwareCodeCus?.PersonId ?? 0),
				DeviceSerial_ = item.DeviceSerial,
				TrafficId = item.IdTraffic,
				PersonNo = item.PersonNo,
				TypeTaradod = (TypeTaradodEnumType)3,
				IsVirtual = (SavedTaradodEnumType)1,
				TypeTaradodVacationOrMission = (TypeTaradodEnumType)34,
				DeviceId_ = device.Id
			}.SaveAsync("SaveAsync in deviceLogonPersonRepository", default(CancellationToken), "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceLogOnPersonRepository.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceLogOnPersonRepository.cs", 87);
		}
		return ret;
	}
}
