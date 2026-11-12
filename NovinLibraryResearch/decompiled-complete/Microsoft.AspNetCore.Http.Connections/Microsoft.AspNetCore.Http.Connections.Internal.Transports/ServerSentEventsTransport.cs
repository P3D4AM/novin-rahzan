using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Internal.Transports;

public class ServerSentEventsTransport : IHttpTransport
{
	private static class Log
	{
		private static readonly Action<ILogger, long, Exception> _sseWritingMessage = LoggerMessage.Define<long>(LogLevel.Trace, new EventId(1, "SSEWritingMessage"), "Writing a {Count} byte message.");

		public static void SSEWritingMessage(ILogger logger, long count)
		{
			_sseWritingMessage(logger, count, null);
		}
	}

	private readonly PipeReader _application;

	private readonly string _connectionId;

	private readonly ILogger _logger;

	public ServerSentEventsTransport(PipeReader application, string connectionId, ILoggerFactory loggerFactory)
	{
		_application = application;
		_connectionId = connectionId;
		_logger = loggerFactory.CreateLogger<ServerSentEventsTransport>();
	}

	public async Task ProcessRequestAsync(HttpContext context, CancellationToken token)
	{
		context.Response.ContentType = "text/event-stream";
		context.Response.Headers["Cache-Control"] = "no-cache";
		context.Features.Get<IHttpBufferingFeature>()?.DisableResponseBuffering();
		context.Response.Headers["Content-Encoding"] = "identity";
		await context.Response.WriteAsync(":\r\n");
		await context.Response.Body.FlushAsync();
		try
		{
			while (true)
			{
				ReadResult readResult = await _application.ReadAsync(token);
				ReadOnlySequence<byte> buffer = readResult.Buffer;
				try
				{
					if (!buffer.IsEmpty)
					{
						Log.SSEWritingMessage(_logger, buffer.Length);
						await ServerSentEventsMessageFormatter.WriteMessageAsync(buffer, context.Response.Body);
					}
					else if (readResult.IsCompleted)
					{
						break;
					}
				}
				finally
				{
					_application.AdvanceTo(buffer.End);
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
	}
}
