namespace Microsoft.AspNetCore.SignalR;

public interface IHubCallerClients : IHubCallerClients<IClientProxy>, IHubClients<IClientProxy>
{
}
public interface IHubCallerClients<T> : IHubClients<T>
{
	T Caller { get; }

	T Others { get; }

	T OthersInGroup(string groupName);
}
