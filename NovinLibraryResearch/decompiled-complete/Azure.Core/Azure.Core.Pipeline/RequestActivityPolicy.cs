using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

internal class RequestActivityPolicy : HttpPipelinePolicy
{
	private readonly bool _isDistributedTracingEnabled;

	private readonly string? _resourceProviderNamespace;

	private const string TraceParentHeaderName = "traceparent";

	private const string TraceStateHeaderName = "tracestate";

	private const string RequestIdHeaderName = "Request-Id";

	private static readonly DiagnosticListener s_diagnosticSource = new DiagnosticListener("Azure.Core");

	public RequestActivityPolicy(bool isDistributedTracingEnabled, string? resourceProviderNamespace)
	{
		_isDistributedTracingEnabled = isDistributedTracingEnabled;
		_resourceProviderNamespace = resourceProviderNamespace;
	}

	public override ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		return ProcessAsync(message, pipeline, isAsync: true);
	}

	public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		ProcessAsync(message, pipeline, isAsync: false).EnsureCompleted();
	}

	private async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool isAsync)
	{
		if (!_isDistributedTracingEnabled)
		{
			if (isAsync)
			{
				await ProcessNextAsync(message, pipeline, isAsync: true).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				ProcessNextAsync(message, pipeline, isAsync: false).EnsureCompleted();
			}
			return;
		}
		if (s_diagnosticSource.IsEnabled())
		{
			Activity activity = new Activity("Azure.Core.Http.Request");
			activity.AddTag("http.method", message.Request.Method.Method);
			activity.AddTag("http.url", message.Request.Uri.ToString());
			activity.AddTag("requestId", message.Request.ClientRequestId);
			activity.AddTag("kind", "client");
			if (_resourceProviderNamespace != null)
			{
				activity.AddTag("az.namespace", _resourceProviderNamespace);
			}
			if (message.Request.Headers.TryGetValue("User-Agent", out string value))
			{
				activity.AddTag("http.user_agent", value);
			}
			bool diagnosticSourceActivityEnabled = s_diagnosticSource.IsEnabled(activity.OperationName, message);
			if (diagnosticSourceActivityEnabled)
			{
				s_diagnosticSource.StartActivity(activity, message);
			}
			else
			{
				activity.Start();
			}
			try
			{
				if (isAsync)
				{
					await ProcessNextAsync(message, pipeline, isAsync: true).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					ProcessNextAsync(message, pipeline, isAsync: false).EnsureCompleted();
				}
				activity.AddTag("http.status_code", message.Response.Status.ToString(CultureInfo.InvariantCulture));
				activity.AddTag("serviceRequestId", message.Response.Headers.RequestId);
				return;
			}
			finally
			{
				if (diagnosticSourceActivityEnabled)
				{
					s_diagnosticSource.StopActivity(activity, message);
				}
				else
				{
					activity.Stop();
				}
			}
		}
		await ProcessNextAsync(message, pipeline, isAsync).ConfigureAwait(continueOnCapturedContext: false);
	}

	private static async ValueTask ProcessNextAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool isAsync)
	{
		Activity current = Activity.Current;
		if (current != null)
		{
			if (current.IsW3CFormat())
			{
				if (!message.Request.Headers.Contains("traceparent"))
				{
					message.Request.Headers.Add("traceparent", current.Id);
					if (current.TryGetTraceState(out string traceState) && traceState != null)
					{
						message.Request.Headers.Add("tracestate", traceState);
					}
				}
			}
			else if (!message.Request.Headers.Contains("Request-Id"))
			{
				message.Request.Headers.Add("Request-Id", current.Id);
			}
		}
		if (isAsync)
		{
			await HttpPipelinePolicy.ProcessNextAsync(message, pipeline).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			HttpPipelinePolicy.ProcessNext(message, pipeline);
		}
	}
}
