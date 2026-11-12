namespace Microsoft.AspNetCore.SignalR;

public interface IHubContext<THub> where THub : Hub
{
	IHubClients Clients { get; }

	IGroupManager Groups { get; }
}
public interface IHubContext<THub, T> where THub : Hub<T> where T : class
{
	IHubClients<T> Clients { get; }

	IGroupManager Groups { get; }
}
