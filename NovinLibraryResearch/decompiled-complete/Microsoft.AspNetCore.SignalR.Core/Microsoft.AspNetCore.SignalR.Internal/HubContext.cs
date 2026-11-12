namespace Microsoft.AspNetCore.SignalR.Internal;

internal class HubContext<THub> : IHubContext<THub> where THub : Hub
{
	private readonly HubLifetimeManager<THub> _lifetimeManager;

	private readonly IHubClients _clients;

	public IHubClients Clients => _clients;

	public virtual IGroupManager Groups { get; }

	public HubContext(HubLifetimeManager<THub> lifetimeManager)
	{
		_lifetimeManager = lifetimeManager;
		_clients = new HubClients<THub>(_lifetimeManager);
		Groups = new GroupManager<THub>(lifetimeManager);
	}
}
internal class HubContext<THub, T> : IHubContext<THub, T> where THub : Hub<T> where T : class
{
	private readonly HubLifetimeManager<THub> _lifetimeManager;

	private readonly IHubClients<T> _clients;

	public IHubClients<T> Clients => _clients;

	public virtual IGroupManager Groups { get; }

	public HubContext(HubLifetimeManager<THub> lifetimeManager)
	{
		_lifetimeManager = lifetimeManager;
		_clients = new HubClients<THub, T>(_lifetimeManager);
		Groups = new GroupManager<THub>(lifetimeManager);
	}
}
