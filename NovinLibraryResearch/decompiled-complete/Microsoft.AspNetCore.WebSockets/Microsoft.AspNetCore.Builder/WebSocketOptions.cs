using System;

namespace Microsoft.AspNetCore.Builder;

public class WebSocketOptions
{
	public TimeSpan KeepAliveInterval { get; set; }

	public int ReceiveBufferSize { get; set; }

	public WebSocketOptions()
	{
		KeepAliveInterval = TimeSpan.FromMinutes(2.0);
		ReceiveBufferSize = 4096;
	}
}
