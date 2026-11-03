using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using PacketParser.Entities;
using Serilog;

namespace PacketParser.Assistance.Net.Ebio;

public class EbioServerInstanceHolder
{
	private Dictionary<int, EbioServer> _dic = new Dictionary<int, EbioServer>();

	public static EbioServerInstanceHolder Instance { get; } = new EbioServerInstanceHolder();


	public event Func<object, AdminLogEventArgs, Task> AdminLog;

	public event Func<object, AlarmEventArgs, Task> AlarmLog;

	public event Func<object, TimeLogEventLog, Task> TimeLog;

	public event Func<object, PingEventLog, Task> PingLog;

	public EbioServer GetInstance(int port, ILogger logger = null)
	{
		try
		{
			_dic.TryGetValue(port, out var value);
			if (value != null)
			{
				return value;
			}
			value = new EbioServer(IPAddress.Any, port, logger);
			value.AdminLog += Ret_AdminLog;
			value.AlarmLog += Ret_AlarmLog;
			value.TimeLog += Ret_TimeLog;
			value.PingLog += Ret_PingLog;
			_dic.Add(port, value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\AttendanceConnection\\EbioServerInstanceHolder.cs", 46);
			throw;
		}
	}

	private Task Ret_PingLog(object sender, PingEventLog e)
	{
		if (this.PingLog == null)
		{
			return Task.CompletedTask;
		}
		return this.PingLog(sender, e);
	}

	private Task Ret_TimeLog(object sender, TimeLogEventLog e)
	{
		if (this.TimeLog == null)
		{
			return Task.CompletedTask;
		}
		return this.TimeLog(sender, e);
	}

	private Task Ret_AlarmLog(object sender, AlarmEventArgs e)
	{
		if (this.AlarmLog == null)
		{
			return Task.CompletedTask;
		}
		return this.AlarmLog(sender, e);
	}

	private Task Ret_AdminLog(object sender, AdminLogEventArgs e)
	{
		if (this.AdminLog == null)
		{
			return Task.CompletedTask;
		}
		return this.AdminLog(sender, e);
	}
}
