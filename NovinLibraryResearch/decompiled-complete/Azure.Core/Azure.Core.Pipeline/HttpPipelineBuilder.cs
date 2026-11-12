using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Azure.Core.Pipeline;

public static class HttpPipelineBuilder
{
	public static HttpPipeline Build(ClientOptions options, params HttpPipelinePolicy[] perRetryPolicies)
	{
		return Build(options, Array.Empty<HttpPipelinePolicy>(), perRetryPolicies, new ResponseClassifier());
	}

	public static HttpPipeline Build(ClientOptions options, HttpPipelinePolicy[] perCallPolicies, HttpPipelinePolicy[] perRetryPolicies, ResponseClassifier responseClassifier)
	{
		if (perCallPolicies == null)
		{
			throw new ArgumentNullException("perCallPolicies");
		}
		if (perRetryPolicies == null)
		{
			throw new ArgumentNullException("perRetryPolicies");
		}
		List<HttpPipelinePolicy> list = new List<HttpPipelinePolicy>();
		bool isDistributedTracingEnabled = options.Diagnostics.IsDistributedTracingEnabled;
		list.Add(ReadClientRequestIdPolicy.Shared);
		list.AddRange(perCallPolicies);
		list.AddRange(options.PerCallPolicies);
		list.Add(ClientRequestIdPolicy.Shared);
		DiagnosticsOptions diagnostics = options.Diagnostics;
		if (diagnostics.IsTelemetryEnabled)
		{
			list.Add(CreateTelemetryPolicy(options));
		}
		RetryOptions retry = options.Retry;
		list.Add(new RetryPolicy(retry.Mode, retry.Delay, retry.MaxDelay, retry.MaxRetries));
		list.AddRange(perRetryPolicies);
		list.AddRange(options.PerRetryPolicies);
		if (diagnostics.IsLoggingEnabled)
		{
			string name = options.GetType().Assembly.GetName().Name;
			list.Add(new LoggingPolicy(diagnostics.IsLoggingContentEnabled, diagnostics.LoggedContentSizeLimit, diagnostics.LoggedHeaderNames.ToArray(), diagnostics.LoggedQueryParameters.ToArray(), name));
		}
		list.Add(new ResponseBodyPolicy(options.Retry.NetworkTimeout));
		list.Add(new RequestActivityPolicy(isDistributedTracingEnabled, ClientDiagnostics.GetResourceProviderNamespace(options.GetType().Assembly)));
		list.RemoveAll((HttpPipelinePolicy policy) => policy == null);
		return new HttpPipeline(options.Transport, list.ToArray(), responseClassifier);
	}

	internal static TelemetryPolicy CreateTelemetryPolicy(ClientOptions options)
	{
		Assembly assembly = options.GetType().Assembly;
		AssemblyInformationalVersionAttribute customAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
		if (customAttribute == null)
		{
			throw new InvalidOperationException("AssemblyInformationalVersionAttribute is required on client SDK assembly '" + assembly.FullName + "' (inferred from the use of options type '" + options.GetType().FullName + "').");
		}
		string text = customAttribute.InformationalVersion;
		string text2 = assembly.GetName().Name;
		if (text2.StartsWith("Azure.", StringComparison.Ordinal))
		{
			text2 = text2.Substring("Azure.".Length);
		}
		int num = text.IndexOf('+');
		if (num != -1)
		{
			text = text.Substring(0, num);
		}
		return new TelemetryPolicy(text2, text, options.Diagnostics.ApplicationId);
	}
}
