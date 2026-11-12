using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Connection.EventArgs;
using Server.Core.Mapper;

namespace Server.Core.Connection;

public class ServerMessaging
{
	private static SemaphoreSlim _proccedPacketsSemaphore = new SemaphoreSlim(1, 1);

	private static HashSet<Guid> _proccedPackets = new HashSet<Guid>();

	private static ConcurrentDictionary<string, IServerConnectionManager> Servers = new ConcurrentDictionary<string, IServerConnectionManager>();

	public static ServerMessaging Instance { get; } = new ServerMessaging();


	public void Initialize()
	{
		try
		{
			ServerMessagingFactory.Instance.OnResetConnections += Instance_OnResetConnections;
			ServerMessagingFactory.Instance.OnServerConnected += Instance_OnServerConnected;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Initialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 32);
		}
	}

	private Task Instance_OnServerConnected(object sender, string serverUniqueId)
	{
		try
		{
			IEnumerable<ConnectionData> enumerable = (ConnectionManager.Instance.GetAllConnectionsData()?.Where((ClsConnectionData p) => p.IsOwnedConnection)).Cast<ConnectionData>();
			if (enumerable != null && enumerable.Any())
			{
				return ((IServerConnectionManager)sender).GenerateHandleConnectionsAsync(enumerable?.ToList(), serverUniqueId);
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnServerConnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 49);
			return Task.CompletedTask;
		}
	}

	private Task Instance_OnResetConnections(object sender, string serverUniqueId)
	{
		int num = 0;
		try
		{
			num = 1;
			List<Task> list = new List<Task>();
			num = 2;
			IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetAllConnectionsData()?.Where((ClsConnectionData p) => p.Context.Guid == serverUniqueId);
			num = 3;
			if (enumerable != null && enumerable.Any())
			{
				num = 4;
				foreach (ClsConnectionData item in enumerable?.ToList())
				{
					num = 5;
					if (item != null)
					{
						num = 6;
						item.Context = WebContextHolder.GetWebContextHolder(serverUniqueId);
						list.Add(Task.Run(delegate
						{
							item.Dispose("Instance_OnResetConnections");
						}));
					}
				}
			}
			num = 20;
			if (list.Any())
			{
				num = 21;
				return Task.WhenAll(list);
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "Instance_OnResetConnections", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 89);
			return Task.CompletedTask;
		}
	}

	public void RegisterServer(string serverUniqueId, IServerConnectionManager server)
	{
		try
		{
			Servers.TryGetValue(serverUniqueId, out var value);
			if (value == null)
			{
				Servers.TryAdd(serverUniqueId, server);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RegisterServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 106);
		}
	}

	public IServerConnectionManager GetServer(string serverUniqueId)
	{
		try
		{
			Servers.TryGetValue(serverUniqueId, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 119);
			return null;
		}
	}

	public IEnumerable<IServerConnectionManager> GetServers()
	{
		return Servers?.Values;
	}

	public void RemoveServer(string serverUniqueId)
	{
		try
		{
			Servers.TryRemove(serverUniqueId, out var _);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 135);
		}
	}

	public async Task Connect2ServerAsync(string url, WebContextHolder context)
	{
	}

	private async Task Hub_OnSendPacketAsync(object sender, ServerSendPacketEventArgs e)
	{
		_ = 1;
		try
		{
			await _proccedPacketsSemaphore.WaitAsync();
			try
			{
				if (_proccedPackets.Contains(e.Packet.PacketUniqueId))
				{
					return;
				}
				_proccedPackets.Add(e.Packet.PacketUniqueId);
			}
			finally
			{
				_proccedPacketsSemaphore.Release();
			}
			ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
			if (connection != null)
			{
				await (connection?.SendPacketAsync(e.Packet, e.SendPriority));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Hub_OnSendPacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 174);
		}
	}

	private Task Hub_OnUnHandleConnection(object sender, string connectionUniqueId)
	{
		try
		{
			string connectionId = ((IServerConnectionManager)sender).ConnectionId;
			return ConnectionManager.Instance.UnHandleAnotherServerConnectionAsync(connectionId, connectionUniqueId);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Hub_OnUnHandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 186);
			return Task.CompletedTask;
		}
	}

	private static async Task Hub_OnHandleConnections(object sender, List<ConnectionData> connections)
	{
		int line = 0;
		try
		{
			line = 1;
			string connectionId = ((IServerConnectionManager)sender).ConnectionId;
			line = 2;
			List<Task> list = new List<Task>();
			line = 3;
			if (connections != null && connections.Any())
			{
				line = 4;
				foreach (ConnectionData connection in connections)
				{
					line = 5;
					if (connection != null && connection.DepId > 0)
					{
						line = 6;
						connection.Context = WebContextHolder.GetWebContextHolder(connectionId);
						list.Add(Hub_OnHandleConnection(sender, new HandleConnectionEventArgs
						{
							connection = connection,
							ServerUniqueId = connectionId
						}));
						line = 7;
					}
					line = 7;
				}
				line = 9;
			}
			line = 10;
			if (list != null && list.Any())
			{
				await Task.WhenAll(list);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Hub_OnHandleConnections", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 224);
		}
	}

	private static Task Hub_OnHandleConnection(object sender, HandleConnectionEventArgs e)
	{
		return Task.CompletedTask;
	}

	public static async Task HandledConnection(ClsConnectionData connection)
	{
		_ = 1;
		try
		{
			if (connection == null || !connection.IsOwnedConnection || connection.DepId <= 0)
			{
				return;
			}
			ConnectionData con = await ConnectionMapper.Instance.MapAsync(connection.Context.Guid, connection);
			IEnumerable<IServerConnectionManager> servers = Instance.GetServers();
			if (servers != null && servers.Any())
			{
				await Task.WhenAll(servers.Select((IServerConnectionManager p) => p.GenerateHandleConnectionAsync(con)));
			}
		}
		catch (InvalidOperationException ex)
		{
			if (!ex.Message.Contains("The 'SendCoreAsync' method cannot be called if the connection is not active"))
			{
				WebErrorLog.Instance.StartLog(ex, "", "HandledConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 262);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "HandledConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 264);
		}
	}

	public static async Task UnHandledConnection(string connectionUniqueId)
	{
		try
		{
			ClsConnectionData connection = ConnectionManager.Instance.GetConnection(connectionUniqueId);
			if (connection != null && !connection.IsOwnedConnection)
			{
				return;
			}
			IEnumerable<IServerConnectionManager> servers = Instance.GetServers();
			if (servers != null && servers.Any())
			{
				await Task.WhenAll(servers.Select((IServerConnectionManager p) => p.GenerateUnHandleConnectionAsync(connectionUniqueId)));
			}
		}
		catch (Exception ex)
		{
			if (!ex.Message.ToLower().Contains("method cannot be called if the connection is not active".ToLower()))
			{
				WebErrorLog.Instance.StartLog(ex, "", "UnHandledConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 288);
			}
		}
	}

	internal Task SendToClient(Packet packet, string clientUniqeId, Priority priority)
	{
		try
		{
			if (Servers.Any())
			{
				return Task.WhenAll(Servers.Values.Select((IServerConnectionManager p) => p.GenerateSendPacketAsync(packet, clientUniqeId, priority) ?? Task.CompletedTask));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessaging.cs", 299);
		}
		return Task.CompletedTask;
	}
}
