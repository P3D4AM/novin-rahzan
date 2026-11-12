using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class UserProxy<THub> : IClientProxy where THub : Hub
{
	private readonly string _userId;

	private readonly HubLifetimeManager<THub> _lifetimeManager;

	public UserProxy(HubLifetimeManager<THub> lifetimeManager, string userId)
	{
		_lifetimeManager = lifetimeManager;
		_userId = userId;
	}

	public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _lifetimeManager.SendUserAsync(_userId, method, args, cancellationToken);
	}
}
