using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal;

internal class WebSocketsTransport : ITransport, IDuplexPipe
{
	private static class Log
	{
		private static readonly Action<ILogger, TransferFormat, Uri, Exception> _startTransport = LoggerMessage.Define<TransferFormat, Uri>(LogLevel.Information, new EventId(1, "StartTransport"), "Starting transport. Transfer mode: {TransferFormat}. Url: '{WebSocketUrl}'.");

		private static readonly Action<ILogger, Exception> _transportStopped = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "TransportStopped"), "Transport stopped.");

		private static readonly Action<ILogger, Exception> _startReceive = LoggerMessage.Define(LogLevel.Debug, new EventId(3, "StartReceive"), "Starting receive loop.");

		private static readonly Action<ILogger, Exception> _receiveStopped = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "ReceiveStopped"), "Receive loop stopped.");

		private static readonly Action<ILogger, Exception> _receiveCanceled = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "ReceiveCanceled"), "Receive loop canceled.");

		private static readonly Action<ILogger, Exception> _transportStopping = LoggerMessage.Define(LogLevel.Information, new EventId(6, "TransportStopping"), "Transport is stopping.");

		private static readonly Action<ILogger, Exception> _sendStarted = LoggerMessage.Define(LogLevel.Debug, new EventId(7, "SendStarted"), "Starting the send loop.");

		private static readonly Action<ILogger, Exception> _sendStopped = LoggerMessage.Define(LogLevel.Debug, new EventId(8, "SendStopped"), "Send loop stopped.");

		private static readonly Action<ILogger, Exception> _sendCanceled = LoggerMessage.Define(LogLevel.Debug, new EventId(9, "SendCanceled"), "Send loop canceled.");

		private static readonly Action<ILogger, int, Exception> _messageToApp = LoggerMessage.Define<int>(LogLevel.Debug, new EventId(10, "MessageToApp"), "Passing message to application. Payload size: {Count}.");

		private static readonly Action<ILogger, WebSocketCloseStatus?, Exception> _webSocketClosed = LoggerMessage.Define<WebSocketCloseStatus?>(LogLevel.Information, new EventId(11, "WebSocketClosed"), "WebSocket closed by the server. Close status {CloseStatus}.");

		private static readonly Action<ILogger, WebSocketMessageType, int, bool, Exception> _messageReceived = LoggerMessage.Define<WebSocketMessageType, int, bool>(LogLevel.Debug, new EventId(12, "MessageReceived"), "Message received. Type: {MessageType}, size: {Count}, EndOfMessage: {EndOfMessage}.");

		private static readonly Action<ILogger, long, Exception> _receivedFromApp = LoggerMessage.Define<long>(LogLevel.Debug, new EventId(13, "ReceivedFromApp"), "Received message from application. Payload size: {Count}.");

		private static readonly Action<ILogger, Exception> _sendMessageCanceled = LoggerMessage.Define(LogLevel.Information, new EventId(14, "SendMessageCanceled"), "Sending a message canceled.");

		private static readonly Action<ILogger, Exception> _errorSendingMessage = LoggerMessage.Define(LogLevel.Error, new EventId(15, "ErrorSendingMessage"), "Error while sending a message.");

		private static readonly Action<ILogger, Exception> _closingWebSocket = LoggerMessage.Define(LogLevel.Information, new EventId(16, "ClosingWebSocket"), "Closing WebSocket.");

		private static readonly Action<ILogger, Exception> _closingWebSocketFailed = LoggerMessage.Define(LogLevel.Debug, new EventId(17, "ClosingWebSocketFailed"), "Closing webSocket failed.");

		private static readonly Action<ILogger, Exception> _cancelMessage = LoggerMessage.Define(LogLevel.Debug, new EventId(18, "CancelMessage"), "Canceled passing message to application.");

		private static readonly Action<ILogger, Exception> _startedTransport = LoggerMessage.Define(LogLevel.Debug, new EventId(19, "StartedTransport"), "Started transport.");

		private static readonly Action<ILogger, Exception> _headersNotSupported = LoggerMessage.Define(LogLevel.Warning, new EventId(20, "HeadersNotSupported"), "Configuring request headers using HttpConnectionOptions.Headers is not supported when using websockets transport on the browser platform.");

		public static void StartTransport(ILogger logger, TransferFormat transferFormat, Uri webSocketUrl)
		{
			_startTransport(logger, transferFormat, webSocketUrl, null);
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

		public static void MessageToApp(ILogger logger, int count)
		{
			_messageToApp(logger, count, null);
		}

		public static void ReceiveCanceled(ILogger logger)
		{
			_receiveCanceled(logger, null);
		}

		public static void ReceiveStopped(ILogger logger)
		{
			_receiveStopped(logger, null);
		}

		public static void SendStarted(ILogger logger)
		{
			_sendStarted(logger, null);
		}

		public static void SendCanceled(ILogger logger)
		{
			_sendCanceled(logger, null);
		}

		public static void SendStopped(ILogger logger)
		{
			_sendStopped(logger, null);
		}

		public static void WebSocketClosed(ILogger logger, WebSocketCloseStatus? closeStatus)
		{
			_webSocketClosed(logger, closeStatus, null);
		}

		public static void MessageReceived(ILogger logger, WebSocketMessageType messageType, int count, bool endOfMessage)
		{
			_messageReceived(logger, messageType, count, endOfMessage, null);
		}

		public static void ReceivedFromApp(ILogger logger, long count)
		{
			_receivedFromApp(logger, count, null);
		}

		public static void SendMessageCanceled(ILogger logger)
		{
			_sendMessageCanceled(logger, null);
		}

		public static void ErrorSendingMessage(ILogger logger, Exception exception)
		{
			_errorSendingMessage(logger, exception);
		}

		public static void ClosingWebSocket(ILogger logger)
		{
			_closingWebSocket(logger, null);
		}

		public static void ClosingWebSocketFailed(ILogger logger, Exception exception)
		{
			_closingWebSocketFailed(logger, exception);
		}

		public static void CancelMessage(ILogger logger)
		{
			_cancelMessage(logger, null);
		}

		public static void StartedTransport(ILogger logger)
		{
			_startedTransport(logger, null);
		}

		public static void HeadersNotSupported(ILogger logger)
		{
			_headersNotSupported(logger, null);
		}
	}

	private WebSocket _webSocket;

	private IDuplexPipe _application;

	private WebSocketMessageType _webSocketMessageType;

	private readonly ILogger _logger;

	private readonly TimeSpan _closeTimeout;

	private volatile bool _aborted;

	private readonly HttpConnectionOptions _httpConnectionOptions;

	private IDuplexPipe _transport;

	internal Task Running { get; private set; } = Task.CompletedTask;


	public PipeReader Input => _transport.Input;

	public PipeWriter Output => _transport.Output;

	public WebSocketsTransport(HttpConnectionOptions httpConnectionOptions, ILoggerFactory loggerFactory, Func<Task<string?>> accessTokenProvider)
	{
		_logger = (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<WebSocketsTransport>();
		_httpConnectionOptions = httpConnectionOptions ?? new HttpConnectionOptions();
		_closeTimeout = _httpConnectionOptions.CloseTimeout;
		_httpConnectionOptions.AccessTokenProvider = accessTokenProvider;
	}

	private async ValueTask<WebSocket> DefaultWebSocketFactory(WebSocketConnectionContext context, CancellationToken cancellationToken)
	{
		ClientWebSocket webSocket = new ClientWebSocket();
		Uri url = context.Uri;
		bool flag = OperatingSystem.IsBrowser();
		if (!flag)
		{
			webSocket.Options.SetRequestHeader("X-SignalR-User-Agent", Constants.UserAgentHeader.ToString());
			webSocket.Options.SetRequestHeader("X-Requested-With", "XMLHttpRequest");
		}
		if (context.Options != null)
		{
			if (context.Options.Headers.Count > 0)
			{
				if (flag)
				{
					Log.HeadersNotSupported(_logger);
				}
				else
				{
					foreach (KeyValuePair<string, string> header in context.Options.Headers)
					{
						webSocket.Options.SetRequestHeader(header.Key, header.Value);
					}
				}
			}
			if (!flag)
			{
				if (context.Options.Cookies != null)
				{
					webSocket.Options.Cookies = context.Options.Cookies;
				}
				if (context.Options.ClientCertificates != null)
				{
					webSocket.Options.ClientCertificates.AddRange(context.Options.ClientCertificates);
				}
				if (context.Options.Credentials != null)
				{
					webSocket.Options.Credentials = context.Options.Credentials;
				}
				if (context.Options.Proxy != null)
				{
					webSocket.Options.Proxy = context.Options.Proxy;
				}
				if (context.Options.UseDefaultCredentials.HasValue)
				{
					webSocket.Options.UseDefaultCredentials = context.Options.UseDefaultCredentials.Value;
				}
				context.Options.WebSocketConfiguration?.Invoke(webSocket.Options);
			}
		}
		if (_httpConnectionOptions.AccessTokenProvider != null)
		{
			string text = await _httpConnectionOptions.AccessTokenProvider();
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (OperatingSystem.IsBrowser())
				{
					string text2 = UrlEncoder.Default.Encode(text);
					text2 = "access_token=" + text2;
					url = Utils.AppendQueryString(url, text2);
				}
				else
				{
					webSocket.Options.SetRequestHeader("Authorization", "Bearer " + text);
				}
			}
		}
		try
		{
			await webSocket.ConnectAsync(url, cancellationToken);
			return webSocket;
		}
		catch
		{
			webSocket.Dispose();
			throw;
		}
	}

	public async Task StartAsync(Uri url, TransferFormat transferFormat, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (url == null)
		{
			throw new ArgumentNullException("url");
		}
		if (transferFormat != TransferFormat.Binary && transferFormat != TransferFormat.Text)
		{
			throw new ArgumentException($"The '{transferFormat}' transfer format is not supported by this transport.", "transferFormat");
		}
		_webSocketMessageType = ((transferFormat == TransferFormat.Binary) ? WebSocketMessageType.Binary : WebSocketMessageType.Text);
		Uri uri = ResolveWebSocketsUrl(url);
		Log.StartTransport(_logger, transferFormat, uri);
		WebSocketConnectionContext arg = new WebSocketConnectionContext(uri, _httpConnectionOptions);
		_webSocket = await (_httpConnectionOptions.WebSocketFactory ?? new Func<WebSocketConnectionContext, CancellationToken, ValueTask<WebSocket>>(DefaultWebSocketFactory))(arg, cancellationToken);
		if (_webSocket == null)
		{
			throw new InvalidOperationException("Configured WebSocketFactory did not return a value.");
		}
		Log.StartedTransport(_logger);
		DuplexPipe.DuplexPipePair duplexPipePair = DuplexPipe.CreateConnectionPair(_httpConnectionOptions.TransportPipeOptions, _httpConnectionOptions.AppPipeOptions);
		_transport = duplexPipePair.Transport;
		_application = duplexPipePair.Application;
		Running = ProcessSocketAsync(_webSocket);
	}

	private async Task ProcessSocketAsync(WebSocket socket)
	{
		using (socket)
		{
			Task receiving = StartReceiving(socket);
			Task sending = StartSending(socket);
			if (await Task.WhenAny(new Task[2] { receiving, sending }) == receiving)
			{
				_application.Input.CancelPendingRead();
				using CancellationTokenSource delayCts = new CancellationTokenSource();
				if (await Task.WhenAny(new Task[2]
				{
					sending,
					Task.Delay(_closeTimeout, delayCts.Token)
				}) != sending)
				{
					_aborted = true;
					socket.Abort();
				}
				else
				{
					delayCts.Cancel();
				}
			}
			else
			{
				_aborted = true;
				socket.Abort();
				_application.Output.CancelPendingFlush();
			}
		}
	}

	private async Task StartReceiving(WebSocket socket)
	{
		_ = 1;
		try
		{
			FlushResult flushResult;
			do
			{
				MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)_application.Output.GetMemory(), out ArraySegment<byte> segment);
				WebSocketReceiveResult webSocketReceiveResult = await socket.ReceiveAsync(segment, CancellationToken.None);
				if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
				{
					Log.WebSocketClosed(_logger, socket.CloseStatus);
					if (socket.CloseStatus != WebSocketCloseStatus.NormalClosure)
					{
						throw new InvalidOperationException($"Websocket closed with error: {socket.CloseStatus}.");
					}
					break;
				}
				Log.MessageReceived(_logger, webSocketReceiveResult.MessageType, webSocketReceiveResult.Count, webSocketReceiveResult.EndOfMessage);
				_application.Output.Advance(webSocketReceiveResult.Count);
				flushResult = await _application.Output.FlushAsync();
			}
			while (!flushResult.IsCanceled && !flushResult.IsCompleted);
		}
		catch (OperationCanceledException)
		{
			Log.ReceiveCanceled(_logger);
		}
		catch (Exception exception)
		{
			if (!_aborted)
			{
				_application.Output.Complete(exception);
			}
		}
		finally
		{
			_application.Output.Complete();
			Log.ReceiveStopped(_logger);
		}
	}

	private async Task StartSending(WebSocket socket)
	{
		Exception error = null;
		try
		{
			_ = 1;
			try
			{
				while (true)
				{
					ReadResult readResult = await _application.Input.ReadAsync();
					ReadOnlySequence<byte> buffer = readResult.Buffer;
					try
					{
						if (readResult.IsCanceled)
						{
							break;
						}
						if (!buffer.IsEmpty)
						{
							try
							{
								Log.ReceivedFromApp(_logger, buffer.Length);
								if (WebSocketCanSend(socket))
								{
									await socket.SendAsync(buffer, _webSocketMessageType);
									continue;
								}
							}
							catch (Exception exception)
							{
								if (!_aborted)
								{
									Log.ErrorSendingMessage(_logger, exception);
								}
							}
						}
						else if (!readResult.IsCompleted)
						{
							continue;
						}
					}
					finally
					{
						_application.Input.AdvanceTo(buffer.End);
					}
					break;
				}
			}
			catch (Exception ex)
			{
				error = ex;
			}
		}
		finally
		{
			if (WebSocketCanSend(socket))
			{
				try
				{
					await socket.CloseOutputAsync((error != null) ? WebSocketCloseStatus.InternalServerError : WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
				}
				catch (Exception exception2)
				{
					Log.ClosingWebSocketFailed(_logger, exception2);
				}
			}
			_application.Input.Complete();
			Log.SendStopped(_logger);
		}
	}

	private static bool WebSocketCanSend(WebSocket ws)
	{
		if (ws.State != WebSocketState.Aborted && ws.State != WebSocketState.Closed)
		{
			return ws.State != WebSocketState.CloseSent;
		}
		return false;
	}

	private static Uri ResolveWebSocketsUrl(Uri url)
	{
		UriBuilder uriBuilder = new UriBuilder(url);
		if (url.Scheme == "http")
		{
			uriBuilder.Scheme = "ws";
		}
		else if (url.Scheme == "https")
		{
			uriBuilder.Scheme = "wss";
		}
		return uriBuilder.Uri;
	}

	public async Task StopAsync()
	{
		Log.TransportStopping(_logger);
		if (_application == null)
		{
			return;
		}
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
			return;
		}
		finally
		{
			_webSocket?.Dispose();
		}
		Log.TransportStopped(_logger, null);
	}
}
