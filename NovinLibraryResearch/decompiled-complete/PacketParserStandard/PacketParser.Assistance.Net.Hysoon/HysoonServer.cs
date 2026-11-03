using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using Serilog;

namespace PacketParser.Assistance.Net.Hysoon;

public class HysoonServer : TcpServerBaseConnection<HysoonTcpClientConnection>
{
	public event Func<object, AdminLogEventArgs, Task> AdminLog;

	public event Func<object, AlarmEventArgs, Task> AlarmLog;

	public event Func<object, TimeLogEventLog, Task> TimeLog;

	public event Func<object, PingEventLog, Task> PingLog;

	public HysoonServer(IPAddress address, int port, ILogger logger)
		: base(address, port, logger)
	{
	}

	protected override Task AcceptConnectionAsync(TcpClient con)
	{
		HysoonTcpClientConnection HysoonTcpClientConnection = new HysoonTcpClientConnection(Port, con, logger);
		HysoonTcpClientConnection.Disposed += HysoonTcpClientConnection_Disposed;
		HysoonTcpClientConnection.AdminLog += Connection_AdminLog;
		HysoonTcpClientConnection.AlarmLog += Connection_AlarmLog;
		HysoonTcpClientConnection.TimeLog += Connection_TimeLog;
		HysoonTcpClientConnection.PingLog += Connection_PingLog;
		Task.Run(async delegate
		{
			_ = 1;
			try
			{
				await Task.Delay(500);
				byte[] data = new byte[17]
				{
					85, 170, 0, 169, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 168, 1, 0
				};
				await HysoonTcpClientConnection.SendAsync(data);
			}
			catch (SocketException)
			{
				HysoonTcpClientConnection?.Dispose();
			}
			catch (IOException)
			{
				HysoonTcpClientConnection?.Dispose();
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "AcceptConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\AttendanceConnection\\HysoonServer.cs", 53);
				HysoonTcpClientConnection?.Dispose();
			}
		});
		return Task.CompletedTask;
	}

	private Task HysoonTcpClientConnection_Disposed(object conn, System.EventArgs e)
	{
		try
		{
			HysoonTcpClientConnection hysoonTcpClientConnection = (HysoonTcpClientConnection)conn;
			hysoonTcpClientConnection?.Dispose();
			if (hysoonTcpClientConnection != null)
			{
				hysoonTcpClientConnection.Disposed -= HysoonTcpClientConnection_Disposed;
				hysoonTcpClientConnection.AdminLog -= Connection_AdminLog;
				hysoonTcpClientConnection.AlarmLog -= Connection_AlarmLog;
				hysoonTcpClientConnection.TimeLog -= Connection_TimeLog;
				hysoonTcpClientConnection.PingLog -= Connection_PingLog;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HysoonTcpClientConnection_Disposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\AttendanceConnection\\HysoonServer.cs", 76);
		}
		return Task.CompletedTask;
	}

	private Task Connection_PingLog(object sender, PingEventLog e)
	{
		return Utilities.RaiseEvent(this.PingLog, sender, e, default(CancellationToken), 60, "Connection_PingLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Hysoon\\HysoonServer.cs");
	}

	private Task Connection_TimeLog(object sender, TimeLogEventLog e)
	{
		return Utilities.RaiseEvent(this.TimeLog, sender, e, default(CancellationToken), 60, "Connection_TimeLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Hysoon\\HysoonServer.cs");
	}

	private Task Connection_AlarmLog(object sender, AlarmEventArgs e)
	{
		return Utilities.RaiseEvent(this.AlarmLog, sender, e, default(CancellationToken), 60, "Connection_AlarmLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Hysoon\\HysoonServer.cs");
	}

	private Task Connection_AdminLog(object sender, AdminLogEventArgs e)
	{
		return Utilities.RaiseEvent(this.AdminLog, sender, e, default(CancellationToken), 60, "Connection_AdminLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Hysoon\\HysoonServer.cs");
	}
}
