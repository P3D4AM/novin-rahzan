using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Http.Private.Time;

namespace Serilog.Sinks.Http.Private.NonDurable;

public class HttpSink : ILogEventSink, IDisposable
{
	private readonly string requestUri;

	private readonly long? logEventLimitBytes;

	private readonly int? logEventsInBatchLimit;

	private readonly long? batchSizeLimitBytes;

	private readonly ITextFormatter textFormatter;

	private readonly IBatchFormatter batchFormatter;

	private readonly IHttpClient httpClient;

	private readonly ExponentialBackoffConnectionSchedule connectionSchedule;

	private readonly PortableTimer timer;

	private readonly object syncRoot = new object();

	private readonly LogEventQueue queue;

	private Batch? unsentBatch;

	private volatile bool disposed;

	public HttpSink(string requestUri, long? queueLimitBytes, long? logEventLimitBytes, int? logEventsInBatchLimit, long? batchSizeLimitBytes, TimeSpan period, ITextFormatter textFormatter, IBatchFormatter batchFormatter, IHttpClient httpClient)
	{
		this.requestUri = requestUri ?? throw new ArgumentNullException("requestUri");
		this.logEventLimitBytes = logEventLimitBytes;
		this.logEventsInBatchLimit = logEventsInBatchLimit;
		this.batchSizeLimitBytes = batchSizeLimitBytes;
		this.textFormatter = textFormatter ?? throw new ArgumentNullException("textFormatter");
		this.batchFormatter = batchFormatter ?? throw new ArgumentNullException("batchFormatter");
		this.httpClient = httpClient ?? throw new ArgumentNullException("httpClient");
		connectionSchedule = new ExponentialBackoffConnectionSchedule(period);
		timer = new PortableTimer(OnTick);
		queue = new LogEventQueue(queueLimitBytes);
		SetTimer();
	}

	public void Emit(LogEvent logEvent)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		StringWriter stringWriter = new StringWriter();
		textFormatter.Format(logEvent, stringWriter);
		string text = stringWriter.ToString();
		if (ByteSize.From(text) > logEventLimitBytes)
		{
			SelfLog.WriteLine("Log event exceeds the size limit of {0} bytes set for this sink and will be dropped; data: {1}", logEventLimitBytes, text);
		}
		else if (queue.TryEnqueue(text) == LogEventQueue.EnqueueResult.QueueFull)
		{
			SelfLog.WriteLine("Queue has reached its limit and the log event will be dropped; data: {0}", text);
		}
	}

	public void Dispose()
	{
		lock (syncRoot)
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
		}
		timer.Dispose();
		OnTick().GetAwaiter().GetResult();
		httpClient.Dispose();
	}

	private void SetTimer()
	{
		timer.Start(connectionSchedule.NextInterval);
	}

	private async Task OnTick()
	{
		_ = 2;
		try
		{
			Batch batch;
			do
			{
				batch = unsentBatch ?? LogEventQueueReader.Read(queue, logEventsInBatchLimit, batchSizeLimitBytes);
				HttpResponseMessage val;
				if (batch.LogEvents.Count > 0)
				{
					using MemoryStream contentStream = new MemoryStream();
					using StreamWriter contentWriter = new StreamWriter(contentStream, Encoding.UTF8WithoutBom);
					batchFormatter.Format(batch.LogEvents, contentWriter);
					await contentWriter.FlushAsync();
					contentStream.Position = 0L;
					if (contentStream.Length == 0L)
					{
						continue;
					}
					val = await httpClient.PostAsync(requestUri, contentStream).ConfigureAwait(continueOnCapturedContext: false);
					goto IL_01dc;
				}
				connectionSchedule.MarkSuccess();
				continue;
				IL_01dc:
				if (val.IsSuccessStatusCode)
				{
					connectionSchedule.MarkSuccess();
					unsentBatch = null;
					continue;
				}
				connectionSchedule.MarkFailure();
				unsentBatch = batch;
				HttpStatusCode statusCode = val.StatusCode;
				string arg = await val.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				SelfLog.WriteLine("Received failed HTTP shipping result {0}: {1}", statusCode, arg);
				break;
			}
			while (batch.HasReachedLimit);
		}
		catch (Exception arg2)
		{
			SelfLog.WriteLine("Exception while emitting periodic batch from {0}: {1}", this, arg2);
			connectionSchedule.MarkFailure();
		}
		finally
		{
			lock (syncRoot)
			{
				if (!disposed)
				{
					SetTimer();
				}
			}
		}
	}
}
