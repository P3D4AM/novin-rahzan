using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace TimeAttendanceDAL.Connection.Interface;

public interface IDeviceManager : INpDisposable, IDisposable
{
	Task<ReturnedSaveFuncInfo> SetUserAsync(short id, string name);

	Task<string> GetUserAsync(short id);
}
