using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Internal.Transports;

public class LongPollingTransport : IHttpTransport
{
	private static class Log
	{
		private static readonly Action<ILogger, Exception> _longPolling204 = LoggerMessage.Define(LogLevel.Debug, new EventId(1, "LongPolling204"), "Terminating Long Polling connection by sending 204 response.");

		private static readonly Action<ILogger, Exception> _pollTimedOut = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "PollTimedOut"), "Poll request timed out. Sending 200 response to connection.");

		private static readonly Action<ILogger, long, Exception> _longPollingWritingMessage = LoggerMessage.Define<long>(LogLevel.Trace, new EventId(3, "LongPollingWritingMessage"), "Writing a {Count} byte message to connection.");

		private static readonly Action<ILogger, Exception> _longPollingDisconnected = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "LongPollingDisconnected"), "Client disconnected from Long Polling endpoint for connection.");

		private static readonly Action<ILogger, Exception> _longPollingTerminated = LoggerMessage.Define(LogLevel.Error, new EventId(5, "LongPollingTerminated"), "Long Polling transport was terminated due to an error on connection.");

		public static void LongPolling204(ILogger logger)
		{
			_longPolling204(logger, null);
		}

		public static void PollTimedOut(ILogger logger)
		{
			_pollTimedOut(logger, null);
		}

		public static void LongPollingWritingMessage(ILogger logger, long count)
		{
			_longPollingWritingMessage(logger, count, null);
		}

		public static void LongPollingDisconnected(ILogger logger)
		{
			_longPollingDisconnected(logger, null);
		}

		public static void LongPollingTerminated(ILogger logger, Exception ex)
		{
			_longPollingTerminated(logger, ex);
		}
	}

	private readonly PipeReader _application;

	private readonly ILogger _logger;

	private readonly CancellationToken _timeoutToken;

	public LongPollingTransport(CancellationToken timeoutToken, PipeReader application, ILoggerFactory loggerFactory)
	{
		_timeoutToken = timeoutToken;
		_application = application;
		_logger = loggerFactory.CreateLogger<LongPollingTransport>();
	}

	public async Task ProcessRequestAsync(HttpContext context, CancellationToken token)
	{
		_ = 1;
		try
		{
			ReadResult readResult = await _application.ReadAsync(token);
			ReadOnlySequence<byte> buffer = readResult.Buffer;
			if (buffer.IsEmpty && readResult.IsCompleted)
			{
				Log.LongPolling204(_logger);
				context.Response.ContentType = "text/plain";
				context.Response.StatusCode = 204;
				return;
			}
			Log.LongPollingWritingMessage(_logger, buffer.Length);
			context.Response.ContentLength = buffer.Length;
			context.Response.ContentType = "application/octet-stream";
			try
			{
				await context.Response.Body.WriteAsync(buffer);
			}
			finally
			{
				_application.AdvanceTo(buffer.End);
			}
		}
		catch (OperationCanceledException)
		{
			if (context.RequestAborted.IsCancellationRequested)
			{
				Log.LongPollingDisconnected(_logger);
				return;
			}
			CancellationToken timeoutToken = _timeoutToken;
			if (timeoutToken.IsCancellationRequested)
			{
				Log.PollTimedOut(_logger);
				context.Response.ContentLength = 0L;
				context.Response.ContentType = "text/plain";
				context.Response.StatusCode = 200;
			}
			else
			{
				Log.LongPolling204(_logger);
				context.Response.ContentType = "text/plain";
				context.Response.StatusCode = 204;
			}
		}
		catch (Exception ex2)
		{
			Log.LongPollingTerminated(_logger, ex2);
			throw;
		}
	}
}
