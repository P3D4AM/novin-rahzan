using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class GroupExceptProxy<THub> : IClientProxy where THub : Hub
{
	private readonly string _groupName;

	private readonly HubLifetimeManager<THub> _lifetimeManager;

	private readonly IReadOnlyList<string> _excludedConnectionIds;

	public GroupExceptProxy(HubLifetimeManager<THub> lifetimeManager, string groupName, IReadOnlyList<string> excludedConnectionIds)
	{
		_lifetimeManager = lifetimeManager;
		_groupName = groupName;
		_excludedConnectionIds = excludedConnectionIds;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendGroupExceptAsync(_groupName, method, args, _excludedConnectionIds, cancellationToken);
	}
}
