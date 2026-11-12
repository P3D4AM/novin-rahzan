using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Attendance;

namespace Server.Core.Attendance;

public interface IDalAttPersonReport
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebAttPersonForPostToApi item, short depId);

	Task<ReturnedSaveFuncInfoWithValue<WebAttPerson>> GetAsync(short depId, Guid guid);
}
