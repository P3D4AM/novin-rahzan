using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Attendance;

namespace Server.Core.Attendance;

public interface IAttendaceLogOnAlarmRepository
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebAttendanceLogOnAlarm item);

	Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceLogOnAlarm>>> GetAllAsync(short? DepId);

	Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceLogOnAlarm>>> GetAsync(Guid deviceinfoId, int partitionId, DateTime modified);

	Task<ReturnedSaveFuncInfoWithValue<WebAttendanceLogOnAlarm>> GetByGuidAsync(Guid guid);
}
