using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientHub.TcpConnection;
using PacketParser.Assistance;
using PacketParser.Assistance.Net;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Scale.EventArguments;
using PacketParser.TaxService;
using Serilog;
using Server.Core.Connection.Dal;

namespace Server.Core.Connection.TcpConnection;

public class ServerTcpConnectionHandler : TcpServerBaseConnection<ServiceTcpClientConnection>, IServer
{
	private static ConcurrentDictionary<string, ClientConnectionSettings> dic_ConnectionSettings = new ConcurrentDictionary<string, ClientConnectionSettings>();

	private static string serverUniqueId = null;

	private static int connectionTimeOut = 9000000;

	private ServiceTcpClientConnection connection;

	public WebContextHolder WebContextHolder { get; set; }

	public string ServerTempGuid => serverUniqueId;

	public AggregateDal DalLayer { get; private set; }

	public ITaxConnectionManager TaxConnectionManager { get; private set; }

	public ServerTcpConnectionHandler(IPAddress address, int port, ILogger logger, AggregateDal dal, ITaxConnectionManager taxManager, string _serverUniqueId)
		: base(address, port, logger)
	{
		try
		{
			ServerFactory.Instance = new ServerFactory();
			DalLayer = dal;
			WebContextHolder = WebContextHolder.GetWebContextHolder(_serverUniqueId);
			TaxConnectionManager = taxManager;
			serverUniqueId = _serverUniqueId;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 46);
		}
	}

	protected override async Task AcceptConnectionAsync(TcpClient con)
	{
		try
		{
			connection = new ServiceTcpClientConnection(Port, con, logger);
			connection.Received += TcpClientConnection_ReceivedAsync;
			connection.Disposed += Connection_Disposed;
			if (ServerFactory.keys == null)
			{
				ServerFactory.keys = new ConcurrentDictionary<string, ConnectionKey>();
			}
			ServerFactory.keys.TryGetValue(connection.ConnectionUniqueId, out var value);
			if (value == null)
			{
				ServerFactory.keys.TryAdd(connection.ConnectionUniqueId, new ConnectionKey());
			}
			List<WebContextHolder> cotexts = new List<WebContextHolder> { WebContextHolder.GetWebContextHolder(GetServerUniqueId()) };
			Initializer.Init(DalLayer, cotexts, 0, TaxConnectionManager);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AcceptConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 67);
		}
		finally
		{
			await AcceptConnectionAsync_(connection);
		}
	}

	private Task Connection_Disposed(object sender, System.EventArgs e)
	{
		try
		{
			ServiceTcpClientConnection serviceTcpClientConnection = (ServiceTcpClientConnection)sender;
			serviceTcpClientConnection?.Dispose();
			if (serviceTcpClientConnection != null)
			{
				serviceTcpClientConnection.Received -= TcpClientConnection_ReceivedAsync;
				serviceTcpClientConnection.Disposed -= Connection_Disposed;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Connection_Disposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 83);
		}
		return Task.CompletedTask;
	}

	private async Task TcpClientConnection_ReceivedAsync(object sender, ReadEventArgs e)
	{
		_ = 1;
		try
		{
			if (base.IsDisposed || e.Bytes == null)
			{
				return;
			}
			dic_ConnectionSettings.TryGetValue(connection.ConnectionUniqueId, out var _);
			BytePacketManager bytePacketManager = BytePacketManager.ParsePacket(e.Bytes);
			if ((bytePacketManager?.Data?.Any()).GetValueOrDefault())
			{
				string @string = Encoding.ASCII.GetString(bytePacketManager.Data);
				if (bytePacketManager.MessageType == EnServerMessageInstruction.ByteData)
				{
					await RecieveBytesAsync(bytePacketManager.Data);
				}
				else
				{
					await ServerFactory.Instance.RecieveSystemMessagesAsync(this, connection.ConnectionUniqueId, bytePacketManager.MessageType, @string);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TcpClientConnection_ReceivedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 103);
			throw;
		}
	}

	public static string GetServerUniqueId()
	{
		try
		{
			if (string.IsNullOrEmpty(serverUniqueId))
			{
				throw new NullReferenceException("serverUniqueId");
			}
			return serverUniqueId;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetServerUniqueId", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 117);
			return "";
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendBytesAsync(string connectionUniqueId, byte[] data, string sender)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(connectionUniqueId))
			{
				throw new ArgumentNullException("connectionUniqueId");
			}
			ServiceTcpClientConnection serviceTcpClientConnection = _clients?.SingleOrDefault((ServiceTcpClientConnection q) => q.ConnectionUniqueId == connectionUniqueId);
			if (serviceTcpClientConnection == null || serviceTcpClientConnection.IsDisposed)
			{
				throw new ArgumentException("Connection with Id:" + connectionUniqueId + " is null or Disposed");
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await BytePacketManager.GenerateAndSavePacketAsync(serviceTcpClientConnection, data), ReturnedState.None, "SendBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 129);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 133);
			res.AddReturnedValue(ex, "", "SendBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 134);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SendAsync(string connectionUniqueId, EnServerMessageInstruction instruction, string data, ClientConnectionSettings sett)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(connectionUniqueId))
			{
				return await res.AddErrorAsync("Connection with Id:" + connectionUniqueId + " is null or Disposed", "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 143);
			}
			ServiceTcpClientConnection serviceTcpClientConnection = _clients?.SingleOrDefault((ServiceTcpClientConnection q) => q.ConnectionUniqueId == connectionUniqueId);
			if (serviceTcpClientConnection == null || serviceTcpClientConnection.IsDisposed)
			{
				return await res.AddErrorAsync("Connection with Id:" + connectionUniqueId + " is null or Disposed", "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 145);
			}
			BytePacketManager bytePacketManager = new BytePacketManager
			{
				MessageType = instruction,
				BufferSize = serviceTcpClientConnection.BufferSize,
				CurrentPart = 1,
				TotalPart = 1,
				RefrenceGuid = Guid.Empty
			};
			if (!string.IsNullOrEmpty(data))
			{
				byte[] bytes = Encoding.ASCII.GetBytes(data);
				bytePacketManager.Data = bytes;
			}
			byte[] data2 = bytePacketManager.GeneratePacket();
			await serviceTcpClientConnection.SendAsync(data2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 165);
			res.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 166);
		}
		return res;
	}

	public Task RecieveSystemMessagesAsync(EnServerMessageInstruction instruction, string data)
	{
		throw new NotImplementedException();
	}

	public async Task RecieveBytesAsync(byte[] data)
	{
		using (new JaegerMonitor("RecieveBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs"))
		{
			StringBuilder sb = new StringBuilder(100000);
			try
			{
				sb.Append($"RecieveBytesAsync time:{DateTime.Now}");
				Task timeout = Task.Delay(connectionTimeOut);
				if (await Task.WhenAny(new Task[2]
				{
					timeout,
					ServerFactory.Instance.handleRecievedBytes(WebContextHolder, connection.Ip.ToString(), connection.ConnectionUniqueId, data, sb, TaxConnectionManager, 10)
				}) == timeout)
				{
					UnHandleConnection(connection.ConnectionUniqueId, "RecieveBytesAsync(" + connection.ConnectionUniqueId + ") process rerquets has been timeOut!\r\n" + $"sb:{sb}");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RecieveBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 183);
			}
		}
	}

	private void UnHandleConnection(string connectionId, string reason)
	{
		using (new JaegerMonitor("UnHandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs"))
		{
			try
			{
				WebErrorLog.Instance.StartLog(reason, "", "UnHandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 190);
				ConnectionManager.Instance.GetConnection(connectionId)?.Dispose(reason);
				connection?.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, connectionId, "UnHandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 194);
			}
		}
	}

	public bool ExistsConnection(string connectionUniqueId)
	{
		try
		{
			return _clients?.Any((ServiceTcpClientConnection p) => p.ConnectionUniqueId == connectionUniqueId) ?? false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ExistsConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\TcpConnection\\ServerTcpConnectionHandler.cs", 205);
			return false;
		}
	}
}
