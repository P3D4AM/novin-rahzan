using System.Collections.Concurrent;
using System.Text;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.TelemetryCore.Http;

internal class HttpTelemetryManager : IHttpTelemetryManager
{
	private int _successfulSilentCallCount;

	private ConcurrentQueue<ApiEvent> _failedEvents = new ConcurrentQueue<ApiEvent>();

	public void ResetPreviousUnsentData()
	{
		_successfulSilentCallCount = 0;
		ApiEvent result;
		while (_failedEvents.TryDequeue(out result))
		{
		}
	}

	public void RecordStoppedEvent(ApiEvent stoppedEvent)
	{
		if (!string.IsNullOrEmpty(stoppedEvent.ApiErrorCode))
		{
			_failedEvents.Enqueue(stoppedEvent);
		}
		if (stoppedEvent.IsAccessTokenCacheHit)
		{
			_successfulSilentCallCount++;
		}
	}

	public string GetLastRequestHeader()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		bool flag = true;
		StringBuilder stringBuilder3 = new StringBuilder();
		foreach (ApiEvent failedEvent in _failedEvents)
		{
			_ = failedEvent["msal.api_error_code"];
			if (!flag)
			{
				stringBuilder2.Append(",");
			}
			stringBuilder2.Append(failedEvent.ApiErrorCode);
			_ = failedEvent["Microsoft.MSAL.correlation_id"];
			_ = failedEvent["msal.api_id"];
			if (!flag)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(failedEvent.ApiIdString);
			stringBuilder.Append(",");
			stringBuilder.Append(failedEvent.CorrelationId);
			if (failedEvent.ContainsKey("msal.region_discovered"))
			{
				string value = failedEvent["msal.region_discovered"];
				if (!flag)
				{
					stringBuilder3.Append(",");
				}
				stringBuilder3.Append(value);
			}
			flag = false;
		}
		string text = "2|" + $"{_successfulSilentCallCount}|" + $"{stringBuilder}|" + $"{stringBuilder2}|" + $"{stringBuilder3}";
		if (text.Length > 3800)
		{
			ResetPreviousUnsentData();
			return string.Empty;
		}
		return text;
	}

	public string GetCurrentRequestHeader(ApiEvent eventInProgress)
	{
		if (eventInProgress == null)
		{
			return string.Empty;
		}
		eventInProgress.TryGetValue("msal.api_id", out var value);
		eventInProgress.TryGetValue("api_with_force_refresh", out var value2);
		eventInProgress.TryGetValue("msal.region_discovered", out var value3);
		return "2|" + value + "," + ConvertFromStringToBitwise(value2) + "|" + value3;
	}

	private string ConvertFromStringToBitwise(string value)
	{
		if (string.IsNullOrEmpty(value) || value == "false")
		{
			return "0";
		}
		return "1";
	}
}
