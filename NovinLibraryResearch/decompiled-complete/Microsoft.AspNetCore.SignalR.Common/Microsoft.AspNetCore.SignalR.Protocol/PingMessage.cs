namespace Microsoft.AspNetCore.SignalR.Protocol;

public class PingMessage : HubMessage
{
	public static readonly PingMessage Instance = new PingMessage();

	private PingMessage()
	{
	}
}
