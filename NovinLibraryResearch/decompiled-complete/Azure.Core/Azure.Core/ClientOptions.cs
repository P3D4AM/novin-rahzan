using System;
using System.Collections.Generic;
using System.ComponentModel;
using Azure.Core.Pipeline;

namespace Azure.Core;

public abstract class ClientOptions
{
	private HttpPipelineTransport _transport = GetDefaultTransport();

	public HttpPipelineTransport Transport
	{
		get
		{
			return _transport;
		}
		set
		{
			_transport = value ?? throw new ArgumentNullException("value");
		}
	}

	public DiagnosticsOptions Diagnostics { get; }

	public RetryOptions Retry { get; }

	internal IList<HttpPipelinePolicy> PerCallPolicies { get; } = new List<HttpPipelinePolicy>();


	internal IList<HttpPipelinePolicy> PerRetryPolicies { get; } = new List<HttpPipelinePolicy>();


	protected ClientOptions()
	{
		Retry = new RetryOptions();
		Diagnostics = new DiagnosticsOptions();
	}

	public void AddPolicy(HttpPipelinePolicy policy, HttpPipelinePosition position)
	{
		switch (position)
		{
		case HttpPipelinePosition.PerCall:
			PerCallPolicies.Add(policy);
			break;
		case HttpPipelinePosition.PerRetry:
			PerRetryPolicies.Add(policy);
			break;
		default:
			throw new ArgumentOutOfRangeException("position", position, null);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object? obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	private static HttpPipelineTransport GetDefaultTransport()
	{
		if (!GetSwitchValue("Azure.Core.Pipeline.DisableHttpWebRequestTransport", "AZURE_CORE_DISABLE_HTTPWEBREQUESTTRANSPORT"))
		{
			return HttpWebRequestTransport.Shared;
		}
		return HttpClientTransport.Shared;
		static bool GetSwitchValue(string switchName, string envVariable)
		{
			if (!AppContext.TryGetSwitch(switchName, out var isEnabled))
			{
				string environmentVariable = Environment.GetEnvironmentVariable(envVariable);
				if (environmentVariable != null)
				{
					return string.Equals("true", environmentVariable, StringComparison.InvariantCultureIgnoreCase) || environmentVariable.Equals("1", StringComparison.InvariantCultureIgnoreCase);
				}
			}
			return isEnabled;
		}
	}
}
