namespace Microsoft.AspNetCore.Http.Connections;

public static class HttpTransports
{
	public static readonly HttpTransportType All = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling;
}
