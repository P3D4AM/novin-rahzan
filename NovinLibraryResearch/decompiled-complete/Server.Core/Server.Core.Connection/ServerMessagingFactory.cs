using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;

namespace Server.Core.Connection;

public class ServerMessagingFactory
{
	public static ServerMessagingFactory Instance { get; private set; } = new ServerMessagingFactory();


	public event Func<object, string, Task> OnResetConnections;

	public event Func<object, string, Task> OnServerConnected;

	public Task OnResetAsync(IServerConnectionManager sender, string serverUniqueId)
	{
		return Utilities.RaiseEvent(this.OnResetConnections, sender, serverUniqueId, default(CancellationToken), 60, "OnResetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessagingFactory.cs");
	}

	public Task OnConnectedAsync(IServerConnectionManager sender, string serverUniqueId)
	{
		return Utilities.RaiseEvent(this.OnServerConnected, sender, serverUniqueId, default(CancellationToken), 60, "OnConnectedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerMessagingFactory.cs");
	}
}
