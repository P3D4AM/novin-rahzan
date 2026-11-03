using System.Net.Sockets;
using Serilog;

namespace PacketParser.Assistance.Net.Client;

public class TcpClientConnection : BaseTcpClientConnection
{
	public override string PingCommand => "";

	public TcpClientConnection(int port, TcpClient connection, ILogger logger = null)
		: base(port, connection, logger, 1024)
	{
	}

	public TcpClientConnection(int port, string hostName, ILogger logger = null)
		: base(port, new TcpClient(hostName, port), logger, 1024)
	{
	}
}
