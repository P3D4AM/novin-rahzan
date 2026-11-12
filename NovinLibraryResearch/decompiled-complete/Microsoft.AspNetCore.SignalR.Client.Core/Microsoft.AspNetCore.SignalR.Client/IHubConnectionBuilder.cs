namespace Microsoft.AspNetCore.SignalR.Client;

public interface IHubConnectionBuilder : ISignalRBuilder
{
	HubConnection Build();
}
