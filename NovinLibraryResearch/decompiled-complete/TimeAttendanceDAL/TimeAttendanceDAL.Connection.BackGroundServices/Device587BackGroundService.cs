using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Attendance.Entities;
using Serilog;
using TimeAttendanceDAL.Connection.Class;
using TimeAttendanceDAL.Connection.Interface;
using TimeAttendanceDAL.Dal.Device;

namespace TimeAttendanceDAL.Connection.BackGroundServices;

public class Device587BackGroundService : BaseBackGroundService
{
	public Device587BackGroundService(string contextGuid, ConnectionManager connection)
		: base(contextGuid, connection)
	{
		Task.Run((Func<Task?>)InitializeAsync);
	}

	public override async Task InitializeAsync()
	{
		try
		{
			List<DeviceInfo> list = (await DeviceInfo.GetAllAsync(base.ContextGuid))?.Where((DeviceInfo q) => (int)q.Model == 9304)?.ToList();
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (DeviceInfo item in list)
			{
				Device587Manager value = new Device587Manager();
				MicroDeviceInfo val = default(MicroDeviceInfo);
				((MicroDeviceInfo)(ref val)).Id = item.Id;
				MicroDeviceInfo key = val;
				base.ConnectionManager.Add(new KeyValuePair<MicroDeviceInfo, IDeviceManager>(key, value));
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}
}
