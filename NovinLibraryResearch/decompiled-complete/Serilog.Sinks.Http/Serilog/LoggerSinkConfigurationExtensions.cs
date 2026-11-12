using System;
using Microsoft.Extensions.Configuration;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Http;
using Serilog.Sinks.Http.BatchFormatters;
using Serilog.Sinks.Http.HttpClients;
using Serilog.Sinks.Http.Private.Durable;
using Serilog.Sinks.Http.Private.NonDurable;
using Serilog.Sinks.Http.TextFormatters;

namespace Serilog;

public static class LoggerSinkConfigurationExtensions
{
	public static LoggerConfiguration Http(this LoggerSinkConfiguration sinkConfiguration, string requestUri, long? queueLimitBytes, long? logEventLimitBytes = null, int? logEventsInBatchLimit = 1000, long? batchSizeLimitBytes = null, TimeSpan? period = null, ITextFormatter? textFormatter = null, IBatchFormatter? batchFormatter = null, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, IHttpClient? httpClient = null, IConfiguration? configuration = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		if (requestUri == null)
		{
			throw new ArgumentNullException("requestUri");
		}
		TimeSpan valueOrDefault = period.GetValueOrDefault();
		if (!period.HasValue)
		{
			valueOrDefault = TimeSpan.FromSeconds(2.0);
			period = valueOrDefault;
		}
		if (textFormatter == null)
		{
			textFormatter = new NormalRenderedTextFormatter();
		}
		if (batchFormatter == null)
		{
			batchFormatter = new ArrayBatchFormatter();
		}
		if (httpClient == null)
		{
			httpClient = new JsonHttpClient();
		}
		if (configuration != null)
		{
			httpClient.Configure(configuration);
		}
		HttpSink logEventSink = new HttpSink(requestUri, queueLimitBytes, logEventLimitBytes, logEventsInBatchLimit, batchSizeLimitBytes, period.Value, textFormatter, batchFormatter, httpClient);
		return sinkConfiguration.Sink(logEventSink, restrictedToMinimumLevel);
	}

	public static LoggerConfiguration DurableHttpUsingFileSizeRolledBuffers(this LoggerSinkConfiguration sinkConfiguration, string requestUri, string bufferBaseFileName = "Buffer", long? bufferFileSizeLimitBytes = 1073741824L, bool bufferFileShared = false, int? retainedBufferFileCountLimit = 31, long? logEventLimitBytes = null, int? logEventsInBatchLimit = 1000, long? batchSizeLimitBytes = null, TimeSpan? period = null, ITextFormatter? textFormatter = null, IBatchFormatter? batchFormatter = null, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, IHttpClient? httpClient = null, IConfiguration? configuration = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		TimeSpan valueOrDefault = period.GetValueOrDefault();
		if (!period.HasValue)
		{
			valueOrDefault = TimeSpan.FromSeconds(2.0);
			period = valueOrDefault;
		}
		if (textFormatter == null)
		{
			textFormatter = new NormalRenderedTextFormatter();
		}
		if (batchFormatter == null)
		{
			batchFormatter = new ArrayBatchFormatter();
		}
		if (httpClient == null)
		{
			httpClient = new JsonHttpClient();
		}
		if (configuration != null)
		{
			httpClient.Configure(configuration);
		}
		FileSizeRolledDurableHttpSink logEventSink = new FileSizeRolledDurableHttpSink(requestUri, bufferBaseFileName, bufferFileSizeLimitBytes, bufferFileShared, retainedBufferFileCountLimit, logEventLimitBytes, logEventsInBatchLimit, batchSizeLimitBytes, period.Value, textFormatter, batchFormatter, httpClient);
		return sinkConfiguration.Sink(logEventSink, restrictedToMinimumLevel);
	}

	public static LoggerConfiguration DurableHttpUsingTimeRolledBuffers(this LoggerSinkConfiguration sinkConfiguration, string requestUri, string bufferBaseFileName = "Buffer", BufferRollingInterval bufferRollingInterval = BufferRollingInterval.Day, long? bufferFileSizeLimitBytes = null, bool bufferFileShared = false, int? retainedBufferFileCountLimit = 31, long? logEventLimitBytes = null, int? logEventsInBatchLimit = 1000, long? batchSizeLimitBytes = null, TimeSpan? period = null, ITextFormatter? textFormatter = null, IBatchFormatter? batchFormatter = null, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, IHttpClient? httpClient = null, IConfiguration? configuration = null)
	{
		if (sinkConfiguration == null)
		{
			throw new ArgumentNullException("sinkConfiguration");
		}
		TimeSpan valueOrDefault = period.GetValueOrDefault();
		if (!period.HasValue)
		{
			valueOrDefault = TimeSpan.FromSeconds(2.0);
			period = valueOrDefault;
		}
		if (textFormatter == null)
		{
			textFormatter = new NormalRenderedTextFormatter();
		}
		if (batchFormatter == null)
		{
			batchFormatter = new ArrayBatchFormatter();
		}
		if (httpClient == null)
		{
			httpClient = new JsonHttpClient();
		}
		if (configuration != null)
		{
			httpClient.Configure(configuration);
		}
		TimeRolledDurableHttpSink logEventSink = new TimeRolledDurableHttpSink(requestUri, bufferBaseFileName, bufferRollingInterval, bufferFileSizeLimitBytes, bufferFileShared, retainedBufferFileCountLimit, logEventLimitBytes, logEventsInBatchLimit, batchSizeLimitBytes, period.Value, textFormatter, batchFormatter, httpClient);
		return sinkConfiguration.Sink(logEventSink, restrictedToMinimumLevel);
	}
}
