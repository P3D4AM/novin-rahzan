using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Internal;

namespace Microsoft.AspNetCore.SignalR;

public class DynamicHubClients
{
	private readonly IHubCallerClients _clients;

	public dynamic All => new DynamicClientProxy(_clients.All);

	public dynamic Caller => new DynamicClientProxy(_clients.Caller);

	public dynamic Others => new DynamicClientProxy(_clients.Others);

	public DynamicHubClients(IHubCallerClients clients)
	{
		_clients = clients;
	}

	public dynamic AllExcept(IReadOnlyList<string> excludedConnectionIds)
	{
		return new DynamicClientProxy(_clients.AllExcept(excludedConnectionIds));
	}

	public dynamic Client(string connectionId)
	{
		return new DynamicClientProxy(_clients.Client(connectionId));
	}

	public dynamic Clients(IReadOnlyList<string> connectionIds)
	{
		return new DynamicClientProxy(_clients.Clients(connectionIds));
	}

	public dynamic Group(string groupName)
	{
		return new DynamicClientProxy(_clients.Group(groupName));
	}

	public dynamic Groups(IReadOnlyList<string> groupNames)
	{
		return new DynamicClientProxy(_clients.Groups(groupNames));
	}

	public dynamic GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
	{
		return new DynamicClientProxy(_clients.GroupExcept(groupName, excludedConnectionIds));
	}

	public dynamic OthersInGroup(string groupName)
	{
		return new DynamicClientProxy(_clients.OthersInGroup(groupName));
	}

	public dynamic User(string userId)
	{
		return new DynamicClientProxy(_clients.User(userId));
	}

	public dynamic Users(IReadOnlyList<string> userIds)
	{
		return new DynamicClientProxy(_clients.Users(userIds));
	}
}
