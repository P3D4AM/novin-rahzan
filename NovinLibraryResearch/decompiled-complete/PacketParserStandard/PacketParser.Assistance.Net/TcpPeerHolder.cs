using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using PacketParser.Assistance.Net.Client;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Scale.EventArguments;
using Serilog;

namespace PacketParser.Assistance.Net;

public class TcpPeerHolder : BaseTcpClientConnection, INpDisposable, IDisposable
{
	private bool _isDisposed;

	public BaseTcpClientConnection Client1 { get; }

	public BaseTcpClientConnection Client2 { get; }

	public new bool IsDisposed => _isDisposed;

	public override string PingCommand => "";

	public event Func<object, ReadEventArgs, Task> Client1Received;

	public event Func<object, ReadEventArgs, Task> Client2Received;

	public new event Func<object, System.EventArgs, Task> Disposed;

	public TcpPeerHolder(BaseTcpClientConnection client1, BaseTcpClientConnection client2, ILogger logger = null)
		: base(client1.Port, client1.Connection, logger, 1024)
	{
		Client1 = client1;
		Client2 = client2;
		Client1.Received += Client1_Received;
		Client2.Received += Client2_Received;
		Client1.Disposed += Client_Disposed;
		Client2.Disposed += Client_Disposed;
	}

	public TcpPeerHolder(BaseTcpClientConnection client1, int destinationPort, string destinationHostName, ILogger logger = null)
		: base(destinationPort, client1.Connection, logger, 1024)
	{
		Client1 = client1;
		Client2 = new TcpClientConnection(destinationPort, destinationHostName, logger);
		Client1.Received += Client1_Received;
		Client2.Received += Client2_Received;
		Client1.Disposed += Client_Disposed;
		Client2.Disposed += Client_Disposed;
	}

	private async Task Client_Disposed(object sender, System.EventArgs e)
	{
		Dispose();
	}

	private async Task WriteText(string text)
	{
		try
		{
			File.AppendAllText("c:\\log2.txt", text);
			File.AppendAllText("c:\\log2.txt", "\r\n");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WriteText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\TcpSniffer\\TcpPeerHolder.cs", 58);
		}
	}

	private async Task Client1_Received(object sender, ReadEventArgs e)
	{
		try
		{
			WriteText("client1 received!");
			this.Client1Received?.Invoke(sender, e);
			WriteText(string.Join(",", e.Bytes));
			await Client2.SendAsync(e.Bytes);
		}
		catch (SocketException)
		{
			Dispose();
		}
		catch (IOException)
		{
			Dispose();
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "Client1_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\TcpSniffer\\TcpPeerHolder.cs", 76);
			Dispose();
		}
	}

	private async Task Client2_Received(object sender, ReadEventArgs e)
	{
		try
		{
			WriteText("client2 received!");
			this.Client2Received?.Invoke(sender, e);
			WriteText(string.Join(",", e.Bytes));
			await Client1.SendAsync(e.Bytes);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Client2_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\TcpSniffer\\TcpPeerHolder.cs", 92);
			Dispose();
		}
	}

	public new void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (Client1 != null)
				{
					Client1.Disposed -= Client_Disposed;
				}
				if (Client2 != null)
				{
					Client2.Disposed -= Client_Disposed;
				}
				Client1?.Dispose();
				Client2?.Dispose();
				this.Disposed?.Invoke(this, new System.EventArgs());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\TcpSniffer\\TcpPeerHolder.cs", 118);
		}
	}
}
