using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.WebSockets;

public class ExtendedWebSocketAcceptContext : WebSocketAcceptContext
{
	public override string SubProtocol { get; set; }

	public int? ReceiveBufferSize { get; set; }

	public TimeSpan? KeepAliveInterval { get; set; }
}
