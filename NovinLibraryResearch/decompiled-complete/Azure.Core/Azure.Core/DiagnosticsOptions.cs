using System;
using System.Collections.Generic;

namespace Azure.Core;

public class DiagnosticsOptions
{
	private const int MaxApplicationIdLength = 24;

	private string? _applicationId;

	public bool IsLoggingEnabled { get; set; } = true;


	public bool IsDistributedTracingEnabled { get; set; } = true;


	public bool IsTelemetryEnabled { get; set; }

	public bool IsLoggingContentEnabled { get; set; }

	public int LoggedContentSizeLimit { get; set; } = 4096;


	public IList<string> LoggedHeaderNames { get; }

	public IList<string> LoggedQueryParameters { get; }

	public string? ApplicationId
	{
		get
		{
			return _applicationId;
		}
		set
		{
			if (value != null && value.Length > 24)
			{
				throw new ArgumentOutOfRangeException("value", string.Format("{0} must be shorter than {1} characters", "ApplicationId", 25));
			}
			_applicationId = value;
		}
	}

	public static string? DefaultApplicationId { get; set; }

	internal DiagnosticsOptions()
	{
		IsTelemetryEnabled = (!EnvironmentVariableToBool(Environment.GetEnvironmentVariable("AZURE_TELEMETRY_DISABLED"))) ?? true;
		IsDistributedTracingEnabled = (!EnvironmentVariableToBool(Environment.GetEnvironmentVariable("AZURE_TRACING_DISABLED"))) ?? true;
		ApplicationId = DefaultApplicationId;
		LoggedHeaderNames = new List<string>
		{
			"x-ms-client-request-id", "x-ms-return-client-request-id", "traceparent", "Accept", "Cache-Control", "Connection", "Content-Length", "Content-Type", "Date", "ETag",
			"Expires", "If-Match", "If-Modified-Since", "If-None-Match", "If-Unmodified-Since", "Last-Modified", "Pragma", "Request-Id", "Retry-After", "Server",
			"Transfer-Encoding", "User-Agent"
		};
		LoggedQueryParameters = new List<string>();
	}

	private static bool? EnvironmentVariableToBool(string value)
	{
		if (string.Equals("true", value, StringComparison.OrdinalIgnoreCase) || string.Equals("1", value, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (string.Equals("false", value, StringComparison.OrdinalIgnoreCase) || string.Equals("0", value, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return null;
	}
}
