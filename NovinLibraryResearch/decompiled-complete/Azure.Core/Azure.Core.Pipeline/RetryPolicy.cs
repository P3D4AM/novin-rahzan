using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Diagnostics;

namespace Azure.Core.Pipeline;

internal class RetryPolicy : HttpPipelinePolicy
{
	private readonly RetryMode _mode;

	private readonly TimeSpan _delay;

	private readonly TimeSpan _maxDelay;

	private readonly int _maxRetries;

	private readonly Random _random = new ThreadSafeRandom();

	private const string RetryAfterHeaderName = "Retry-After";

	private const string RetryAfterMsHeaderName = "retry-after-ms";

	private const string XRetryAfterMsHeaderName = "x-ms-retry-after-ms";

	public RetryPolicy(RetryMode mode, TimeSpan delay, TimeSpan maxDelay, int maxRetries)
	{
		_mode = mode;
		_delay = delay;
		_maxDelay = maxDelay;
		_maxRetries = maxRetries;
	}

	public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		ProcessAsync(message, pipeline, async: false).EnsureCompleted();
	}

	public override ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		return ProcessAsync(message, pipeline, async: true);
	}

	private async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool async)
	{
		int attempt = 0;
		List<Exception> exceptions = null;
		while (true)
		{
			Exception lastException = null;
			long before = Stopwatch.GetTimestamp();
			try
			{
				if (async)
				{
					await HttpPipelinePolicy.ProcessNextAsync(message, pipeline).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					HttpPipelinePolicy.ProcessNext(message, pipeline);
				}
			}
			catch (Exception ex)
			{
				if (exceptions == null)
				{
					exceptions = new List<Exception>();
				}
				exceptions.Add(ex);
				lastException = ex;
			}
			long timestamp = Stopwatch.GetTimestamp();
			double elapsed = (double)(timestamp - before) / (double)Stopwatch.Frequency;
			attempt++;
			bool flag = attempt <= _maxRetries;
			TimeSpan delay;
			if (lastException != null)
			{
				if (!flag || !message.ResponseClassifier.IsRetriable(message, lastException))
				{
					List<Exception> list = exceptions;
					if (list != null && list.Count == 1)
					{
						ExceptionDispatchInfo.Capture(lastException).Throw();
					}
					throw new AggregateException($"Retry failed after {attempt} tries.", exceptions);
				}
				GetDelay(attempt, out delay);
			}
			else
			{
				if (!message.ResponseClassifier.IsErrorResponse(message) || !flag || !message.ResponseClassifier.IsRetriableResponse(message))
				{
					break;
				}
				GetDelay(message, attempt, out delay);
			}
			if (delay > TimeSpan.Zero)
			{
				if (async)
				{
					await WaitAsync(delay, message.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					Wait(delay, message.CancellationToken);
				}
			}
			if (message.HasResponse)
			{
				message.Response.ContentStream?.Dispose();
			}
			AzureCoreEventSource.Singleton.RequestRetrying(message.Request.ClientRequestId, attempt, elapsed);
		}
	}

	internal virtual async Task WaitAsync(TimeSpan time, CancellationToken cancellationToken)
	{
		await Task.Delay(time, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal virtual void Wait(TimeSpan time, CancellationToken cancellationToken)
	{
		cancellationToken.WaitHandle.WaitOne(time);
	}

	protected virtual TimeSpan GetServerDelay(HttpMessage message)
	{
		if (message.Response == null)
		{
			return TimeSpan.Zero;
		}
		if ((message.Response.TryGetHeader("retry-after-ms", out string value) || message.Response.TryGetHeader("x-ms-retry-after-ms", out value)) && int.TryParse(value, out var result))
		{
			return TimeSpan.FromMilliseconds(result);
		}
		if (message.Response.TryGetHeader("Retry-After", out value))
		{
			if (int.TryParse(value, out var result2))
			{
				return TimeSpan.FromSeconds(result2);
			}
			if (DateTimeOffset.TryParse(value, out var result3))
			{
				return result3 - DateTimeOffset.Now;
			}
		}
		return TimeSpan.Zero;
	}

	private void GetDelay(HttpMessage message, int attempted, out TimeSpan delay)
	{
		delay = TimeSpan.Zero;
		switch (_mode)
		{
		case RetryMode.Fixed:
			delay = _delay;
			break;
		case RetryMode.Exponential:
			delay = CalculateExponentialDelay(attempted);
			break;
		}
		TimeSpan serverDelay = GetServerDelay(message);
		if (serverDelay > delay)
		{
			delay = serverDelay;
		}
	}

	private void GetDelay(int attempted, out TimeSpan delay)
	{
		delay = CalculateExponentialDelay(attempted);
	}

	private TimeSpan CalculateExponentialDelay(int attempted)
	{
		int num = 1 << attempted - 1;
		Random random = _random;
		TimeSpan delay = _delay;
		int minValue = (int)(delay.TotalMilliseconds * 0.8);
		delay = _delay;
		double val = num * random.Next(minValue, (int)(delay.TotalMilliseconds * 1.2));
		delay = _maxDelay;
		return TimeSpan.FromMilliseconds(Math.Min(val, delay.TotalMilliseconds));
	}
}
