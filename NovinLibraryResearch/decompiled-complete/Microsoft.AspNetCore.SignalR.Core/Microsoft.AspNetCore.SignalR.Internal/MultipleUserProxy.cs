using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class MultipleUserProxy<THub> : IClientProxy where THub : Hub
{
	private readonly IReadOnlyList<string> _userIds;

	private readonly HubLifetimeManager<THub> _lifetimeManager;

	public MultipleUserProxy(HubLifetimeManager<THub> lifetimeManager, IReadOnlyList<string> userIds)
	{
		_lifetimeManager = lifetimeManager;
		_userIds = userIds;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendUsersAsync(_userIds, method, args, cancellationToken);
	}
}
