using System.Collections.Generic;

namespace Microsoft.AspNetCore.SignalR;

public interface IHubClients : IHubClients<IClientProxy>
{
}
public interface IHubClients<T>
{
	T All { get; }

	T AllExcept(IReadOnlyList<string> excludedConnectionIds);

	T Client(string connectionId);

	T Clients(IReadOnlyList<string> connectionIds);

	T Group(string groupName);

	T Groups(IReadOnlyList<string> groupNames);

	T GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds);

	T User(string userId);

	T Users(IReadOnlyList<string> userIds);
}
