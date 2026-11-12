using System;

namespace Microsoft.AspNetCore.Http.Connections;

[Flags]
public enum HttpTransportType
{
	None = 0,
	WebSockets = 1,
	ServerSentEvents = 2,
	LongPolling = 4
}
