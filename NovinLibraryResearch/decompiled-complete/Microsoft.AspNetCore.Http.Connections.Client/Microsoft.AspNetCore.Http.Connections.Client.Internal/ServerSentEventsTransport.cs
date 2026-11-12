using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal;

internal class ServerSentEventsTransport : ITransport, IDuplexPipe
{
	private static class Log
	{
		private static readonly Action<ILogger, TransferFormat, Exception> _startTransport = LoggerMessage.Define<TransferFormat>(LogLevel.Information, new EventId(1, "StartTransport"), "Starting transport. Transfer mode: {TransferFormat}.");

		private static readonly Action<ILogger, Exception> _transportStopped = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "TransportStopped"), "Transport stopped.");

		private static readonly Action<ILogger, Exception> _startReceive = LoggerMessage.Define(LogLevel.Debug, new EventId(3, "StartReceive"), "Starting receive loop.");

		private static readonly Action<ILogger, Exception> _receiveStopped = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "ReceiveStopped"), "Receive loop stopped.");

		private static readonly Action<ILogger, Exception> _receiveCanceled = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "ReceiveCanceled"), "Receive loop canceled.");

		private static readonly Action<ILogger, Exception> _transportStopping = LoggerMessage.Define(LogLevel.Information, new EventId(6, "TransportStopping"), "Transport is stopping.");

		private static readonly Action<ILogger, int, Exception> _messageToApplication = LoggerMessage.Define<int>(LogLevel.Debug, new EventId(7, "MessageToApplication"), "Passing message to application. Payload size: {Count}.");

		private static readonly Action<ILogger, Exception> _eventStreamEnded = LoggerMessage.Define(LogLevel.Debug, new EventId(8, "EventStreamEnded"), "Server-Sent Event Stream ended.");

		private static readonly Action<ILogger, long, Exception> _parsingSSE = LoggerMessage.Define<long>(LogLevel.Debug, new EventId(9, "ParsingSSE"), "Received {Count} bytes. Parsing SSE frame.");

		public static void StartTransport(ILogger logger, TransferFormat transferFormat)
		{
			_startTransport(logger, transferFormat, null);
		}

		public static void TransportStopped(ILogger logger, Exception exception)
		{
			_transportStopped(logger, exception);
		}

		public static void StartReceive(ILogger logger)
		{
			_startReceive(logger, null);
		}

		public static void TransportStopping(ILogger logger)
		{
			_transportStopping(logger, null);
		}

		public static void MessageToApplication(ILogger logger, int count)
		{
			_messageToApplication(logger, count, null);
		}

		public static void ReceiveCanceled(ILogger logger)
		{
			_receiveCanceled(logger, null);
		}

		public static void ReceiveStopped(ILogger logger)
		{
			_receiveStopped(logger, null);
		}

		public static void EventStreamEnded(ILogger logger)
		{
			_eventStreamEnded(logger, null);
		}

		public static void ParsingSSE(ILogger logger, long bytes)
		{
			_parsingSSE(logger, bytes, null);
		}
	}

	private readonly HttpClient _httpClient;

	private readonly ILogger _logger;

	private readonly HttpConnectionOptions _httpConnectionOptions;

	private volatile Exception _error;

	private readonly CancellationTokenSource _transportCts = new CancellationTokenSource();

	private readonly CancellationTokenSource _inputCts = new CancellationTokenSource();

	private readonly ServerSentEventsMessageParser _parser = new ServerSentEventsMessageParser();

	private IDuplexPipe _transport;

	private IDuplexPipe _application;

	internal Task Running { get; private set; } = Task.CompletedTask;


	public PipeReader Input => _transport.Input;

	public PipeWriter Output => _transport.Output;

	public ServerSentEventsTransport(HttpClient httpClient, HttpConnectionOptions? httpConnectionOptions = null, ILoggerFactory? loggerFactory = null)
	{
		if (httpClient == null)
		{
			throw new ArgumentNullException("httpClient");
		}
		_httpClient = httpClient;
		_logger = (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<ServerSentEventsTransport>();
		_httpConnectionOptions = httpConnectionOptions ?? new HttpConnectionOptions();
	}

	public async Task StartAsync(Uri url, TransferFormat transferFormat, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (transferFormat != TransferFormat.Text)
		{
			throw new ArgumentException($"The '{transferFormat}' transfer format is not supported by this transport.", "transferFormat");
		}
		Log.StartTransport(_logger, transferFormat);
		HttpRequestMessage val = new HttpRequestMessage(HttpMethod.Get, url);
		val.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
		HttpResponseMessage response = null;
		try
		{
			response = await _httpClient.SendAsync(val, (HttpCompletionOption)1, cancellationToken);
			response.EnsureSuccessStatusCode();
		}
		catch
		{
			HttpResponseMessage obj = response;
			if (obj != null)
			{
				obj.Dispose();
			}
			Log.TransportStopping(_logger);
			throw;
		}
		DuplexPipe.DuplexPipePair duplexPipePair = DuplexPipe.CreateConnectionPair(_httpConnectionOptions.TransportPipeOptions, _httpConnectionOptions.AppPipeOptions);
		_transport = duplexPipePair.Transport;
		_application = duplexPipePair.Application;
		Running = ProcessAsync(url, response);
	}

	private async Task ProcessAsync(Uri url, HttpResponseMessage response)
	{
		Task receiving = ProcessEventStream(response, _transportCts.Token);
		Task sending = SendUtils.SendMessages(url, _application, _httpClient, _logger, _inputCts.Token);
		if (await Task.WhenAny(new Task[2] { receiving, sending }) == receiving)
		{
			_inputCts.Cancel();
			_application.Input.CancelPendingRead();
			await sending;
		}
		else
		{
			_error = (sending.IsFaulted ? sending.Exception.InnerException : null);
			_transportCts.Cancel();
			_application.Output.CancelPendingFlush();
			await receiving;
		}
	}

	private async Task ProcessEventStream(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		Log.StartReceive(_logger);
		try
		{
			using Stream stream = await response.Content.ReadAsStreamAsync();
			PipeReader reader = PipeReader.Create(stream);
			using (cancellationToken.Register(CancelReader, reader))
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					while (true)
					{
						ReadResult readResult = await reader.ReadAsync();
						ReadOnlySequence<byte> buffer = readResult.Buffer;
						SequencePosition consumed = buffer.Start;
						SequencePosition examined = buffer.End;
						try
						{
							if (readResult.IsCanceled)
							{
								Log.ReceiveCanceled(_logger);
								break;
							}
							if (!buffer.IsEmpty)
							{
								Log.ParsingSSE(_logger, buffer.Length);
								byte[] message;
								ServerSentEventsMessageParser.ParseResult parseResult = _parser.ParseMessage(buffer, out consumed, out examined, out message);
								FlushResult flushResult = default(FlushResult);
								switch (parseResult)
								{
								case ServerSentEventsMessageParser.ParseResult.Completed:
									Log.MessageToApplication(_logger, message.Length);
									flushResult = await _application.Output.WriteAsync(message);
									_parser.Reset();
									break;
								case ServerSentEventsMessageParser.ParseResult.Incomplete:
									if (readResult.IsCompleted)
									{
										throw new FormatException("Incomplete message.");
									}
									break;
								}
								if (flushResult.IsCanceled || flushResult.IsCompleted)
								{
									Log.EventStreamEnded(_logger);
									break;
								}
							}
							else if (readResult.IsCompleted)
							{
								break;
							}
						}
						finally
						{
							reader.AdvanceTo(consumed, examined);
						}
					}
				}
				catch (Exception error)
				{
					_error = error;
				}
				finally
				{
					_application.Output.Complete(_error);
					Log.ReceiveStopped(_logger);
					reader.Complete();
				}
			}
		}
		finally
		{
			((IDisposable)response)?.Dispose();
		}
		static void CancelReader(object? state)
		{
			((PipeReader)state).CancelPendingRead();
		}
	}

	public async Task StopAsync()
	{
		Log.TransportStopping(_logger);
		if (_application != null)
		{
			_transport.Output.Complete();
			_transport.Input.Complete();
			_application.Input.CancelPendingRead();
			try
			{
				await Running;
			}
			catch (Exception exception)
			{
				Log.TransportStopped(_logger, exception);
				throw;
			}
			Log.TransportStopped(_logger, null);
		}
	}
}
