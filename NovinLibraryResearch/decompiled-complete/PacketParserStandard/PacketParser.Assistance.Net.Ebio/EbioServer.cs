using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using Serilog;

namespace PacketParser.Assistance.Net.Ebio;

public class EbioServer : TcpServerBaseConnection<EbioTcpClientConnection>
{
	public event Func<object, AdminLogEventArgs, Task> AdminLog;

	public event Func<object, AlarmEventArgs, Task> AlarmLog;

	public event Func<object, TimeLogEventLog, Task> TimeLog;

	public event Func<object, PingEventLog, Task> PingLog;

	public EbioServer(IPAddress address, int port, ILogger logger)
		: base(address, port, logger)
	{
	}

	protected override async Task AcceptConnectionAsync(TcpClient con)
	{
		EbioTcpClientConnection ebioTcpClientConnection = null;
		try
		{
			ebioTcpClientConnection = new EbioTcpClientConnection(Port, con, logger);
			ebioTcpClientConnection.Disposed += EbioTcpClientConnection_Disposed;
			ebioTcpClientConnection.AdminLog += Connection_AdminLog;
			ebioTcpClientConnection.AlarmLog += Connection_AlarmLog;
			ebioTcpClientConnection.TimeLog += Connection_TimeLog;
			ebioTcpClientConnection.PingLog += Connection_PingLog;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AcceptConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\AttendanceConnection\\EbioServer.cs", 33);
		}
		finally
		{
			await AcceptConnectionAsync_(ebioTcpClientConnection);
		}
	}

	private Task EbioTcpClientConnection_Disposed(object sender, System.EventArgs e)
	{
		try
		{
			EbioTcpClientConnection ebioTcpClientConnection = (EbioTcpClientConnection)sender;
			ebioTcpClientConnection?.Dispose();
			if (ebioTcpClientConnection != null)
			{
				ebioTcpClientConnection.Disposed -= EbioTcpClientConnection_Disposed;
				ebioTcpClientConnection.AdminLog -= Connection_AdminLog;
				ebioTcpClientConnection.AlarmLog -= Connection_AlarmLog;
				ebioTcpClientConnection.TimeLog -= Connection_TimeLog;
				ebioTcpClientConnection.PingLog -= Connection_PingLog;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EbioTcpClientConnection_Disposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\AttendanceConnection\\EbioServer.cs", 53);
		}
		return Task.CompletedTask;
	}

	private Task Connection_PingLog(object sender, PingEventLog e)
	{
		return Utilities.RaiseEvent(this.PingLog, sender, e, default(CancellationToken), 60, "Connection_PingLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Ebio\\EbioServer.cs");
	}

	private Task Connection_TimeLog(object sender, TimeLogEventLog e)
	{
		return Utilities.RaiseEvent(this.TimeLog, sender, e, default(CancellationToken), 60, "Connection_TimeLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Ebio\\EbioServer.cs");
	}

	private Task Connection_AlarmLog(object sender, AlarmEventArgs e)
	{
		return Utilities.RaiseEvent(this.AlarmLog, sender, e, default(CancellationToken), 60, "Connection_AlarmLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Ebio\\EbioServer.cs");
	}

	private Task Connection_AdminLog(object sender, AdminLogEventArgs e)
	{
		return Utilities.RaiseEvent(this.AdminLog, sender, e, default(CancellationToken), 60, "Connection_AdminLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Ebio\\EbioServer.cs");
	}
}
