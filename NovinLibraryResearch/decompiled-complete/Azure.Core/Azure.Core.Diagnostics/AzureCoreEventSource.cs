using System.Diagnostics.Tracing;

namespace Azure.Core.Diagnostics;

[EventSource(Name = "Azure-Core")]
internal sealed class AzureCoreEventSource : EventSource
{
	private const string EventSourceName = "Azure-Core";

	private const int BackgroundRefreshFailedEvent = 19;

	private const int RequestEvent = 1;

	private const int RequestContentEvent = 2;

	private const int RequestContentTextEvent = 17;

	private const int ResponseEvent = 5;

	private const int ResponseContentEvent = 6;

	private const int ResponseDelayEvent = 7;

	private const int ResponseContentTextEvent = 13;

	private const int ResponseContentBlockEvent = 11;

	private const int ResponseContentTextBlockEvent = 15;

	private const int ErrorResponseEvent = 8;

	private const int ErrorResponseContentEvent = 9;

	private const int ErrorResponseContentTextEvent = 14;

	private const int ErrorResponseContentBlockEvent = 12;

	private const int ErrorResponseContentTextBlockEvent = 16;

	private const int RequestRetryingEvent = 10;

	private const int ExceptionResponseEvent = 18;

	public static AzureCoreEventSource Singleton { get; } = new AzureCoreEventSource();


	private AzureCoreEventSource()
		: base("Azure-Core", EventSourceSettings.Default, "AzureEventSource", "true")
	{
	}

	[Event(19, Level = EventLevel.Informational, Message = "Background token refresh [{0}] failed with exception {1}")]
	public void BackgroundRefreshFailed(string requestId, string exception)
	{
		WriteEvent(19, requestId, exception);
	}

	[Event(1, Level = EventLevel.Informational, Message = "Request [{0}] {1} {2}\r\n{3}client assembly: {4}")]
	public void Request(string requestId, string method, string uri, string headers, string? clientAssembly)
	{
		WriteEvent(1, requestId, method, uri, headers, clientAssembly);
	}

	[Event(2, Level = EventLevel.Verbose, Message = "Request [{0}] content: {1}")]
	public void RequestContent(string requestId, byte[] content)
	{
		WriteEvent(2, requestId, content);
	}

	[Event(17, Level = EventLevel.Verbose, Message = "Request [{0}] content: {1}")]
	public void RequestContentText(string requestId, string content)
	{
		WriteEvent(17, requestId, content);
	}

	[Event(5, Level = EventLevel.Informational, Message = "Response [{0}] {1} {2} ({4:00.0}s)\r\n{3}")]
	public void Response(string requestId, int status, string reasonPhrase, string headers, double seconds)
	{
		WriteEvent(5, requestId, status, reasonPhrase, headers, seconds);
	}

	[Event(6, Level = EventLevel.Verbose, Message = "Response [{0}] content: {1}")]
	public void ResponseContent(string requestId, byte[] content)
	{
		WriteEvent(6, requestId, content);
	}

	[Event(11, Level = EventLevel.Verbose, Message = "Response [{0}] content block {1}: {2}")]
	public void ResponseContentBlock(string requestId, int blockNumber, byte[] content)
	{
		WriteEvent(11, requestId, blockNumber, content);
	}

	[Event(13, Level = EventLevel.Verbose, Message = "Response [{0}] content: {1}")]
	public void ResponseContentText(string requestId, string content)
	{
		WriteEvent(13, requestId, content);
	}

	[Event(15, Level = EventLevel.Verbose, Message = "Response [{0}] content block {1}: {2}")]
	public void ResponseContentTextBlock(string requestId, int blockNumber, string content)
	{
		WriteEvent(15, requestId, blockNumber, content);
	}

	[Event(8, Level = EventLevel.Warning, Message = "Error response [{0}] {1} {2} ({4:00.0}s)\r\n{3}")]
	public void ErrorResponse(string requestId, int status, string reasonPhrase, string headers, double seconds)
	{
		WriteEvent(8, requestId, status, reasonPhrase, headers, seconds);
	}

	[Event(9, Level = EventLevel.Informational, Message = "Error response [{0}] content: {1}")]
	public void ErrorResponseContent(string requestId, byte[] content)
	{
		WriteEvent(9, requestId, content);
	}

	[Event(12, Level = EventLevel.Informational, Message = "Error response [{0}] content block {1}: {2}")]
	public void ErrorResponseContentBlock(string requestId, int blockNumber, byte[] content)
	{
		WriteEvent(12, requestId, blockNumber, content);
	}

	[Event(14, Level = EventLevel.Informational, Message = "Error response [{0}] content: {1}")]
	public void ErrorResponseContentText(string requestId, string content)
	{
		WriteEvent(14, requestId, content);
	}

	[Event(16, Level = EventLevel.Informational, Message = "Error response [{0}] content block {1}: {2}")]
	public void ErrorResponseContentTextBlock(string requestId, int blockNumber, string content)
	{
		WriteEvent(16, requestId, blockNumber, content);
	}

	[Event(10, Level = EventLevel.Informational, Message = "Request [{0}] retry number {1} took {2:00.0}s")]
	public void RequestRetrying(string requestId, int retryNumber, double seconds)
	{
		WriteEvent(10, requestId, retryNumber, seconds);
	}

	[Event(7, Level = EventLevel.Warning, Message = "Response [{0}] took {1:00.0}s")]
	public void ResponseDelay(string requestId, double seconds)
	{
		WriteEvent(7, requestId, seconds);
	}

	[Event(18, Level = EventLevel.Informational, Message = "Request [{0}] exception {1}")]
	public void ExceptionResponse(string requestId, string exception)
	{
		WriteEvent(18, requestId, exception);
	}
}
