using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.InterFaces;

public interface IDalSaveAttLogOnPerson
{
	Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, WebAttLogOnPerson logonPersonel);
}
