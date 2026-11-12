using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.SignalR;

public class HubOptions
{
	public TimeSpan? HandshakeTimeout { get; set; }

	public TimeSpan? KeepAliveInterval { get; set; }

	public IList<string> SupportedProtocols { get; set; }

	public bool? EnableDetailedErrors { get; set; }
}
public class HubOptions<THub> : HubOptions where THub : Hub
{
}
