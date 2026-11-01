using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Connection.Interface;

namespace TimeAttendanceDAL.Connection.Class;

public class Device587Manager : IDeviceManager, INpDisposable, IDisposable
{
	public bool IsDisposed
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public Task<string> GetUserAsync(short id)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetUserAsync(short id, string name)
	{
		throw new NotImplementedException();
	}
}
