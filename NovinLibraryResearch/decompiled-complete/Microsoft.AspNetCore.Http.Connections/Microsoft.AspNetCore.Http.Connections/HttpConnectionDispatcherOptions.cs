using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.AspNetCore.Http.Connections;

public class HttpConnectionDispatcherOptions
{
	private const int DefaultPipeBufferSize = 32768;

	public IList<IAuthorizeData> AuthorizationData { get; }

	public HttpTransportType Transports { get; set; }

	public WebSocketOptions WebSockets { get; }

	public LongPollingOptions LongPolling { get; }

	public long TransportMaxBufferSize { get; set; }

	public long ApplicationMaxBufferSize { get; set; }

	public HttpConnectionDispatcherOptions()
	{
		AuthorizationData = new List<IAuthorizeData>();
		Transports = HttpTransports.All;
		WebSockets = new WebSocketOptions();
		LongPolling = new LongPollingOptions();
		TransportMaxBufferSize = 32768L;
		ApplicationMaxBufferSize = 32768L;
	}
}
