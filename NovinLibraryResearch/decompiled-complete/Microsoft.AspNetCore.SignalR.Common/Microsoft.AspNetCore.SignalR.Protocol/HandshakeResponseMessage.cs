namespace Microsoft.AspNetCore.SignalR.Protocol;

public class HandshakeResponseMessage : HubMessage
{
	public static readonly HandshakeResponseMessage Empty = new HandshakeResponseMessage(null);

	public string? Error { get; }

	public HandshakeResponseMessage(string? error)
	{
		Error = error;
	}
}
