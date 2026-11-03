using System;
using System.Net.Sockets;
using PacketParser.Assistance.Net;
using PacketParser.Entities;
using Serilog;

namespace ClientHub.TcpConnection;

public class ServiceTcpClientConnection : BaseTcpClientConnection
{
	public override string PingCommand => "";

	public ServiceTcpClientConnection(int port, TcpClient connection, ILogger logger = null)
		: base(port, connection, logger, 4096)
	{
		try
		{
			InitializeAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\ServiceTcpClientConnection.cs", 19);
		}
	}
}
