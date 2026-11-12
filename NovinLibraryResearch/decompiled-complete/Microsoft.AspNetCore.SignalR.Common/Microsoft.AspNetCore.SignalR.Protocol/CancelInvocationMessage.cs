namespace Microsoft.AspNetCore.SignalR.Protocol;

public class CancelInvocationMessage : HubInvocationMessage
{
	public CancelInvocationMessage(string invocationId)
		: base(invocationId)
	{
	}
}
