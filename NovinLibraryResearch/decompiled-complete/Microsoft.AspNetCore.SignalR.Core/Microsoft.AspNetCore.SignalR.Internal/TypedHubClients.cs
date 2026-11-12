using System.Collections.Generic;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class TypedHubClients<T> : IHubCallerClients<T>, IHubClients<T>
{
	private readonly IHubCallerClients _hubClients;

	public T All => TypedClientBuilder<T>.Build(_hubClients.All);

	public T Caller => TypedClientBuilder<T>.Build(_hubClients.Caller);

	public T Others => TypedClientBuilder<T>.Build(_hubClients.Others);

	public TypedHubClients(IHubCallerClients dynamicContext)
	{
		_hubClients = dynamicContext;
	}

	public T AllExcept(IReadOnlyList<string> excludedConnectionIds)
	{
		return TypedClientBuilder<T>.Build(_hubClients.AllExcept(excludedConnectionIds));
	}

	public T Client(string connectionId)
	{
		return TypedClientBuilder<T>.Build(_hubClients.Client(connectionId));
	}

	public T Group(string groupName)
	{
		return TypedClientBuilder<T>.Build(_hubClients.Group(groupName));
	}

	public T GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
	{
		return TypedClientBuilder<T>.Build(_hubClients.GroupExcept(groupName, excludedConnectionIds));
	}

	public T Clients(IReadOnlyList<string> connectionIds)
	{
		return TypedClientBuilder<T>.Build(_hubClients.Clients(connectionIds));
	}

	public T Groups(IReadOnlyList<string> groupNames)
	{
		return TypedClientBuilder<T>.Build(_hubClients.Groups(groupNames));
	}

	public T OthersInGroup(string groupName)
	{
		return TypedClientBuilder<T>.Build(_hubClients.OthersInGroup(groupName));
	}

	public T User(string userId)
	{
		return TypedClientBuilder<T>.Build(_hubClients.User(userId));
	}

	public T Users(IReadOnlyList<string> userIds)
	{
		return TypedClientBuilder<T>.Build(_hubClients.Users(userIds));
	}
}
