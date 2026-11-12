using System;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Text;
using Azure.Core;

namespace Azure.Identity;

[EventSource(Name = "Azure-Identity")]
internal sealed class AzureIdentityEventSource : EventSource
{
	private const string EventSourceName = "Azure-Identity";

	private const int GetTokenEvent = 1;

	private const int GetTokenSucceededEvent = 2;

	private const int GetTokenFailedEvent = 3;

	private const int ProbeImdsEndpointEvent = 4;

	private const int ImdsEndpointFoundEvent = 5;

	private const int ImdsEndpointUnavailableEvent = 6;

	public static AzureIdentityEventSource Singleton { get; } = new AzureIdentityEventSource();


	private AzureIdentityEventSource()
		: base("Azure-Identity", EventSourceSettings.Default, "AzureEventSource", "true")
	{
	}

	[NonEvent]
	public void GetToken(string method, TokenRequestContext context)
	{
		if (IsEnabled(EventLevel.Informational, EventKeywords.All))
		{
			GetToken(method, FormatStringArray(context.Scopes), context.ParentRequestId);
		}
	}

	[Event(1, Level = EventLevel.Informational, Message = "{0} invoked. Scopes: {1} ParentRequestId: {2}")]
	public void GetToken(string method, string scopes, string parentRequestId)
	{
		WriteEvent(1, method, scopes, parentRequestId);
	}

	[NonEvent]
	public void GetTokenSucceeded(string method, TokenRequestContext context, DateTimeOffset expiresOn)
	{
		if (IsEnabled(EventLevel.Informational, EventKeywords.All))
		{
			GetTokenSucceeded(method, FormatStringArray(context.Scopes), context.ParentRequestId, expiresOn.ToString("O", CultureInfo.InvariantCulture));
		}
	}

	[Event(2, Level = EventLevel.Informational, Message = "{0} succeeded. Scopes: {1} ParentRequestId: {2} ExpiresOn: {3}")]
	public void GetTokenSucceeded(string method, string scopes, string parentRequestId, string expiresOn)
	{
		WriteEvent(2, method, scopes, parentRequestId, expiresOn);
	}

	[NonEvent]
	public void GetTokenFailed(string method, TokenRequestContext context, Exception ex)
	{
		if (IsEnabled(EventLevel.Informational, EventKeywords.All))
		{
			GetTokenFailed(method, FormatStringArray(context.Scopes), context.ParentRequestId, FormatException(ex));
		}
	}

	[Event(3, Level = EventLevel.Informational, Message = "{0} was unable to retrieve an access token. Scopes: {1} ParentRequestId: {2} Exception: {3}")]
	public void GetTokenFailed(string method, string scopes, string parentRequestId, string exception)
	{
		WriteEvent(3, method, scopes, parentRequestId, exception);
	}

	[NonEvent]
	public void ProbeImdsEndpoint(Uri uri)
	{
		if (IsEnabled(EventLevel.Informational, EventKeywords.All))
		{
			ProbeImdsEndpoint(uri.ToString());
		}
	}

	[Event(4, Level = EventLevel.Informational, Message = "Probing IMDS endpoint for availability. Endpoint: {0}")]
	public void ProbeImdsEndpoint(string uri)
	{
		WriteEvent(4, uri);
	}

	[NonEvent]
	public void ImdsEndpointFound(Uri uri)
	{
		if (IsEnabled(EventLevel.Informational, EventKeywords.All))
		{
			ImdsEndpointFound(uri.ToString());
		}
	}

	[Event(5, Level = EventLevel.Informational, Message = "IMDS endpoint is available. Endpoint: {0}")]
	public void ImdsEndpointFound(string uri)
	{
		WriteEvent(5, uri);
	}

	[NonEvent]
	public void ImdsEndpointUnavailable(Uri uri)
	{
		if (IsEnabled(EventLevel.Informational, EventKeywords.All))
		{
			ImdsEndpointUnavailable(uri.ToString());
		}
	}

	[Event(6, Level = EventLevel.Informational, Message = "IMDS endpoint is did not respond. Endpoint: {0}")]
	public void ImdsEndpointUnavailable(string uri)
	{
		WriteEvent(6, uri);
	}

	[NonEvent]
	private static string FormatException(Exception ex)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		do
		{
			if (flag)
			{
				stringBuilder.AppendLine().Append(" ---> ");
			}
			stringBuilder.Append(ex.GetType().FullName).Append(" (0x").Append(ex.HResult.ToString("x", CultureInfo.InvariantCulture))
				.Append("): ")
				.Append(ex.Message);
			ex = ex.InnerException;
			flag = true;
		}
		while (ex != null);
		return stringBuilder.ToString();
	}

	[NonEvent]
	private static string FormatStringArray(string[] array)
	{
		return new StringBuilder("[ ").Append(string.Join(", ", array)).Append(" ]").ToString();
	}
}
