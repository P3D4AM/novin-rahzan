using System.Collections.Generic;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class HubClients<THub> : IHubClients, IHubClients<IClientProxy> where THub : Hub
{
	private readonly HubLifetimeManager<THub> _lifetimeManager;

	public IClientProxy All { get; }

	public HubClients(HubLifetimeManager<THub> lifetimeManager)
	{
		_lifetimeManager = lifetimeManager;
		All = new AllClientProxy<THub>(_lifetimeManager);
	}

	public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds)
	{
		return new AllClientsExceptProxy<THub>(_lifetimeManager, excludedConnectionIds);
	}

	public IClientProxy Client(string connectionId)
	{
		return new SingleClientProxy<THub>(_lifetimeManager, connectionId);
	}

	public IClientProxy Group(string groupName)
	{
		return new GroupProxy<THub>(_lifetimeManager, groupName);
	}

	public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
	{
		return new GroupExceptProxy<THub>(_lifetimeManager, groupName, excludedConnectionIds);
	}

	public IClientProxy Clients(IReadOnlyList<string> connectionIds)
	{
		return new MultipleClientProxy<THub>(_lifetimeManager, connectionIds);
	}

	public IClientProxy Groups(IReadOnlyList<string> groupNames)
	{
		return new MultipleGroupProxy<THub>(_lifetimeManager, groupNames);
	}

	public IClientProxy User(string userId)
	{
		return new UserProxy<THub>(_lifetimeManager, userId);
	}

	public IClientProxy Users(IReadOnlyList<string> userIds)
	{
		return new MultipleUserProxy<THub>(_lifetimeManager, userIds);
	}
}
internal class HubClients<THub, T> : IHubClients<T> where THub : Hub
{
	private readonly HubLifetimeManager<THub> _lifetimeManager;

	public T All { get; }

	public HubClients(HubLifetimeManager<THub> lifetimeManager)
	{
		_lifetimeManager = lifetimeManager;
		All = TypedClientBuilder<T>.Build(new AllClientProxy<THub>(_lifetimeManager));
	}

	public T AllExcept(IReadOnlyList<string> excludedConnectionIds)
	{
		return TypedClientBuilder<T>.Build(new AllClientsExceptProxy<THub>(_lifetimeManager, excludedConnectionIds));
	}

	public virtual T Client(string connectionId)
	{
		return TypedClientBuilder<T>.Build(new SingleClientProxy<THub>(_lifetimeManager, connectionId));
	}

	public T Clients(IReadOnlyList<string> connectionIds)
	{
		return TypedClientBuilder<T>.Build(new MultipleClientProxy<THub>(_lifetimeManager, connectionIds));
	}

	public virtual T Group(string groupName)
	{
		return TypedClientBuilder<T>.Build(new GroupProxy<THub>(_lifetimeManager, groupName));
	}

	public T GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
	{
		return TypedClientBuilder<T>.Build(new GroupExceptProxy<THub>(_lifetimeManager, groupName, excludedConnectionIds));
	}

	public T Groups(IReadOnlyList<string> groupNames)
	{
		return TypedClientBuilder<T>.Build(new MultipleGroupProxy<THub>(_lifetimeManager, groupNames));
	}

	public virtual T User(string userId)
	{
		return TypedClientBuilder<T>.Build(new UserProxy<THub>(_lifetimeManager, userId));
	}

	public virtual T Users(IReadOnlyList<string> userIds)
	{
		return TypedClientBuilder<T>.Build(new MultipleUserProxy<THub>(_lifetimeManager, userIds));
	}
}
