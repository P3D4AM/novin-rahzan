using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class AllClientsExceptProxy<THub> : IClientProxy where THub : Hub
{
	private readonly HubLifetimeManager<THub> _lifetimeManager;

	private readonly IReadOnlyList<string> _excludedConnectionIds;

	public AllClientsExceptProxy(HubLifetimeManager<THub> lifetimeManager, IReadOnlyList<string> excludedConnectionIds)
	{
		_lifetimeManager = lifetimeManager;
		_excludedConnectionIds = excludedConnectionIds;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendAllExceptAsync(method, args, _excludedConnectionIds, cancellationToken);
	}
}
