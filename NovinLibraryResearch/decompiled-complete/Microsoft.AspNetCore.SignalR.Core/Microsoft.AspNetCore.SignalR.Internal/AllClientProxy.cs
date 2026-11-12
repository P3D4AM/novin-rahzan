using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class AllClientProxy<THub> : IClientProxy where THub : Hub
{
	private readonly HubLifetimeManager<THub> _lifetimeManager;

	public AllClientProxy(HubLifetimeManager<THub> lifetimeManager)
	{
		_lifetimeManager = lifetimeManager;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendAllAsync(method, args);
	}
}
