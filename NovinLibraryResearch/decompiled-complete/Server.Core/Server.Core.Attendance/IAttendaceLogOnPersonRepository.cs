using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Attendance.InterFaces;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Attendance;

namespace Server.Core.Attendance;

public interface IAttendaceLogOnPersonRepository : IDalSaveAttLogOnPerson
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebAttendanceLogOnPerson item);

	Task<List<WebAttendanceLogOnPerson>> GetAllAsync(short DepId, long HashId, short Partition, CancellationToken cancellationToken);

	Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceLogOnPerson>>> GetAsync(Guid deviceinfoId, int partitionId, DateTime modified);

	Task<ReturnedSaveFuncInfoWithValue<WebAttendanceLogOnPerson>> GetByGuidAsync(Guid guid);

	Task<List<int>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);

	Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceLogOnPerson>>> GetLastTardod(short DepId, int Top, CancellationToken cancellationToken);

	Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceLogOnPerson>>> GetLastTardodSerailString(string serailString, int Top, CancellationToken cancellationToken);
}
