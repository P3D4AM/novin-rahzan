using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using PacketParser.Assistance.Net.Client;
using PacketParser.Entities;
using PacketParser.Scale.EventArguments;
using Serilog;

namespace PacketParser.Assistance.Net;

public class TcpSniffer : TcpServerBaseConnection<TcpPeerHolder>
{
	private int _port;

	private int _destinationPort;

	private string _destinationHostName;

	public event Func<object, ReadEventArgs, Task> Client1Received;

	public event Func<object, ReadEventArgs, Task> Client2Received;

	public event Func<object, System.EventArgs, Task> ClientDisposed;

	public TcpSniffer(IPAddress address, int port, int destinationPort, string destinationHostName, ILogger logger = null)
		: base(address, port, logger)
	{
		_port = port;
		_destinationPort = destinationPort;
		_destinationHostName = destinationHostName;
	}

	protected override Task AcceptConnectionAsync(TcpClient con)
	{
		try
		{
			TcpClientConnection tcpClientConnection = new TcpClientConnection(_port, con, logger);
			TcpClientConnection tcpClientConnection2 = null;
			try
			{
				tcpClientConnection2 = new TcpClientConnection(_destinationPort, _destinationHostName, logger);
			}
			catch (SocketException)
			{
				tcpClientConnection?.Dispose();
				return Task.CompletedTask;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "AcceptConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\TcpSniffer\\TcpSniffer.cs", 50);
				tcpClientConnection?.Dispose();
				return Task.CompletedTask;
			}
			tcpClientConnection.InitializeAsync();
			tcpClientConnection2.InitializeAsync();
			TcpPeerHolder tcpPeerHolder = new TcpPeerHolder(tcpClientConnection, tcpClientConnection2);
			_clients.Add(tcpPeerHolder);
			tcpPeerHolder.Client1Received += this.Client1Received;
			tcpPeerHolder.Client2Received += this.Client2Received;
			tcpPeerHolder.Disposed += Peer_Disposed;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "AcceptConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\TcpSniffer\\TcpSniffer.cs", 64);
		}
		return Task.CompletedTask;
	}

	private Task Peer_Disposed(object sender, System.EventArgs e)
	{
		try
		{
			TcpPeerHolder tcpPeerHolder = (TcpPeerHolder)sender;
			tcpPeerHolder.Client1Received -= this.Client1Received;
			tcpPeerHolder.Client2Received -= this.Client2Received;
			tcpPeerHolder.Disposed -= Peer_Disposed;
			_clients?.Remove(tcpPeerHolder);
			this.ClientDisposed?.Invoke(this, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Peer_Disposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\TcpSniffer\\TcpSniffer.cs", 82);
		}
		return Task.CompletedTask;
	}
}
