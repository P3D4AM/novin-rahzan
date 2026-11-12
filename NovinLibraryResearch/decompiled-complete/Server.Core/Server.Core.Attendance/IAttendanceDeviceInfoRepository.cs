using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Attendance;

namespace Server.Core.Attendance;

public interface IAttendanceDeviceInfoRepository
{
	Task<ReturnedSaveFuncInfoWithValue<Guid>> SaveAsync(WebAttendanceDeviceInfo item);

	Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceDeviceInfo>>> GetAllAsync(short? depId);

	Task<ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo>> GetAsync(string serialid);

	Task<ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo>> GetByGuidAsync(Guid guid);

	Task<ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo>> GetOrSave(string enTerminalType, string serialNumber, int terminalId, string portnumber);

	Task<ReturnedSaveFuncInfoWithValue<Guid>> UpdateDeviceInfo(WebAttendanceDeviceInfo item);

	Task<ReturnedSaveFuncInfo> SetDepartmentIdAsync(Guid deviceGuid, short? depId, string depName);

	Task<ReturnedSaveFuncInfo> UpdateLastPing(string serial);
}
