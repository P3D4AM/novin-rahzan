using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class SingleClientProxy<THub> : IClientProxy where THub : Hub
{
	private readonly string _connectionId;

	private readonly HubLifetimeManager<THub> _lifetimeManager;

	public SingleClientProxy(HubLifetimeManager<THub> lifetimeManager, string connectionId)
	{
		_lifetimeManager = lifetimeManager;
		_connectionId = connectionId;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendConnectionAsync(_connectionId, method, args, cancellationToken);
	}
}
