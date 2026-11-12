namespace Microsoft.AspNetCore.SignalR;

public class DefaultUserIdProvider : IUserIdProvider
{
	public virtual string GetUserId(HubConnectionContext connection)
	{
		return connection.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
	}
}
