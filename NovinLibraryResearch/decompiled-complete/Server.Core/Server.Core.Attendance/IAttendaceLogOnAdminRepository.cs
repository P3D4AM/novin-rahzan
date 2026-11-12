using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Attendance;

namespace Server.Core.Attendance;

public interface IAttendaceLogOnAdminRepository
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebAttendanceLogOnAdmin item);

	Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceLogOnAdmin>>> GetAsync(Guid deviceinfoId, int partitionId, DateTime modified);

	Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceLogOnAdmin>>> GetAllAsync(short? DepID);

	Task<ReturnedSaveFuncInfoWithValue<WebAttendanceLogOnAdmin>> GetByGuidAsync(Guid guid);
}
