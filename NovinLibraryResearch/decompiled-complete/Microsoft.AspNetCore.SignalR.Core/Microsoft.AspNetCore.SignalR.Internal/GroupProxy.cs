using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class GroupProxy<THub> : IClientProxy where THub : Hub
{
	private readonly string _groupName;

	private readonly HubLifetimeManager<THub> _lifetimeManager;

	public GroupProxy(HubLifetimeManager<THub> lifetimeManager, string groupName)
	{
		_lifetimeManager = lifetimeManager;
		_groupName = groupName;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendGroupAsync(_groupName, method, args, cancellationToken);
	}
}
