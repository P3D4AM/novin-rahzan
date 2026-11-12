using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Http.Connections;

public class WebSocketOptions
{
	public TimeSpan CloseTimeout { get; set; } = TimeSpan.FromSeconds(5.0);


	public Func<IList<string>, string> SubProtocolSelector { get; set; }
}
