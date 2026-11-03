using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Attendance.Entities;

public interface IDeviceLogOnPersonRepository
{
	Task<ReturnedSaveFuncInfoWithValue<LogOnPersonSaveReqult>> SaveAsync(DeviceLogOnPerson item);
}
