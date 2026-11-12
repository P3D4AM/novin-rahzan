namespace Microsoft.AspNetCore.SignalR;

public interface IUserIdProvider
{
	string GetUserId(HubConnectionContext connection);
}
