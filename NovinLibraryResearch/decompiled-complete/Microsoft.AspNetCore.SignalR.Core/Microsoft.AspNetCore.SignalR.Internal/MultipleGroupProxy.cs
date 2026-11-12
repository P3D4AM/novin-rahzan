using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class MultipleGroupProxy<THub> : IClientProxy where THub : Hub
{
	private readonly HubLifetimeManager<THub> _lifetimeManager;

	private readonly IReadOnlyList<string> _groupNames;

	public MultipleGroupProxy(HubLifetimeManager<THub> lifetimeManager, IReadOnlyList<string> groupNames)
	{
		_lifetimeManager = lifetimeManager;
		_groupNames = groupNames;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendGroupsAsync(_groupNames, method, args, cancellationToken);
	}
}
