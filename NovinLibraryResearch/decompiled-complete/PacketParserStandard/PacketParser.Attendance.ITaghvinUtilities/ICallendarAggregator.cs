using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;

namespace PacketParser.Attendance.ITaghvinUtilities;

public interface ICallendarAggregator : ICalender, IHasJustGuid, IHasStatus
{
	Task<IEnumerable<ICalender>> GetAllAsync(string contextGuid, string tranName = "");

	Task<ReturnedSaveFuncInfo> SaveAsync(short tryCount = 10, string tranName = "");

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, string contextGuid, IEnumerable<ICalender> LstBussiness);
}
