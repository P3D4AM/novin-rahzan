using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR;

public abstract class HubLifetimeManager<THub> where THub : Hub
{
	public abstract Task OnConnectedAsync(HubConnectionContext connection);

	public abstract Task OnDisconnectedAsync(HubConnectionContext connection);

	public abstract Task SendAllAsync(string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task SendAllExceptAsync(string methodName, object[] args, IReadOnlyList<string> excludedConnectionIds, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task SendConnectionAsync(string connectionId, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task SendConnectionsAsync(IReadOnlyList<string> connectionIds, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task SendGroupAsync(string groupName, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task SendGroupsAsync(IReadOnlyList<string> groupNames, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task SendGroupExceptAsync(string groupName, string methodName, object[] args, IReadOnlyList<string> excludedConnectionIds, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task SendUserAsync(string userId, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task SendUsersAsync(IReadOnlyList<string> userIds, string methodName, object[] args, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task AddToGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task RemoveFromGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default(CancellationToken));
}
