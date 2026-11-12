using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class MultipleClientProxy<THub> : IClientProxy where THub : Hub
{
	private readonly HubLifetimeManager<THub> _lifetimeManager;

	private readonly IReadOnlyList<string> _connectionIds;

	public MultipleClientProxy(HubLifetimeManager<THub> lifetimeManager, IReadOnlyList<string> connectionIds)
	{
		_lifetimeManager = lifetimeManager;
		_connectionIds = connectionIds;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendConnectionsAsync(_connectionIds, method, args, cancellationToken);
	}
}
