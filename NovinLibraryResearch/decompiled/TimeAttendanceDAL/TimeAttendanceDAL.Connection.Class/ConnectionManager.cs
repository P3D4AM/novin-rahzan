using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Attendance.Entities;
using Serilog;
using TimeAttendanceDAL.Connection.BackGroundServices;
using TimeAttendanceDAL.Connection.Interface;

namespace TimeAttendanceDAL.Connection.Class;

public class ConnectionManager
{
	private static Dictionary<MicroDeviceInfo, IDeviceManager> connections = new Dictionary<MicroDeviceInfo, IDeviceManager>();

	public string ContextGuid { get; private set; }

	public ConnectionManager(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task HanldleConnectionsAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			new Device587BackGroundService(ContextGuid, this);
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static IDeviceManager GetConnection(MicroDeviceInfo key)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			connections.TryGetValue(key, out var value);
			return value;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public void Add(KeyValuePair<MicroDeviceInfo, IDeviceManager> item)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (connections.TryGetValue(item.Key, out var value))
		{
			value?.Dispose();
			connections.Remove(item.Key);
		}
		connections.Add(item.Key, item.Value);
	}
}
