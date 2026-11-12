using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Http.Private.IO;

namespace Serilog.Sinks.Http.Private.Durable;

public class TimeRolledDurableHttpSink : ILogEventSink, IDisposable
{
	private readonly HttpLogShipper shipper;

	private readonly ILogEventSink sink;

	public TimeRolledDurableHttpSink(string requestUri, string bufferBaseFileName, BufferRollingInterval bufferRollingInterval, long? bufferFileSizeLimitBytes, bool bufferFileShared, int? retainedBufferFileCountLimit, long? logEventLimitBytes, int? logEventsInBatchLimit, long? batchSizeLimitBytes, TimeSpan period, ITextFormatter textFormatter, IBatchFormatter batchFormatter, IHttpClient httpClient)
	{
		shipper = new HttpLogShipper(httpClient, requestUri, new TimeRolledBufferFiles(new DirectoryService(), bufferBaseFileName), logEventLimitBytes, logEventsInBatchLimit, batchSizeLimitBytes, period, batchFormatter);
		sink = CreateFileSink(bufferBaseFileName, bufferRollingInterval, bufferFileSizeLimitBytes, bufferFileShared, retainedBufferFileCountLimit, textFormatter);
	}

	public void Emit(LogEvent logEvent)
	{
		sink.Emit(logEvent);
	}

	public void Dispose()
	{
		(sink as IDisposable)?.Dispose();
		shipper.Dispose();
	}

	private static ILogEventSink CreateFileSink(string bufferBaseFileName, BufferRollingInterval bufferRollingInterval, long? bufferFileSizeLimitBytes, bool bufferFileShared, int? retainedBufferFileCountLimit, ITextFormatter textFormatter)
	{
		LoggerSinkConfiguration writeTo = new LoggerConfiguration().WriteTo;
		string path = bufferBaseFileName + "-.txt";
		RollingInterval rollingInterval = bufferRollingInterval.ToRollingInterval();
		bool shared = bufferFileShared;
		return writeTo.File(textFormatter, path, LogEventLevel.Verbose, bufferFileSizeLimitBytes, null, buffered: false, shared, null, rollingInterval, rollOnFileSizeLimit: false, retainedBufferFileCountLimit).CreateLogger();
	}
}
