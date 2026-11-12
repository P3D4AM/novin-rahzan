using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Internal.Transports;

public class WebSocketsTransport : IHttpTransport
{
	private static class Log
	{
		private static readonly Action<ILogger, string, Exception> _socketOpened = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1, "SocketOpened"), "Socket opened using Sub-Protocol: '{SubProtocol}'.");

		private static readonly Action<ILogger, Exception> _socketClosed = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "SocketClosed"), "Socket closed.");

		private static readonly Action<ILogger, WebSocketCloseStatus?, string, Exception> _clientClosed = LoggerMessage.Define<WebSocketCloseStatus?, string>(LogLevel.Debug, new EventId(3, "ClientClosed"), "Client closed connection with status code '{Status}' ({Description}). Signaling end-of-input to application.");

		private static readonly Action<ILogger, Exception> _waitingForSend = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "WaitingForSend"), "Waiting for the application to finish sending data.");

		private static readonly Action<ILogger, Exception> _failedSending = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "FailedSending"), "Application failed during sending. Sending InternalServerError close frame.");

		private static readonly Action<ILogger, Exception> _finishedSending = LoggerMessage.Define(LogLevel.Debug, new EventId(6, "FinishedSending"), "Application finished sending. Sending close frame.");

		private static readonly Action<ILogger, Exception> _waitingForClose = LoggerMessage.Define(LogLevel.Debug, new EventId(7, "WaitingForClose"), "Waiting for the client to close the socket.");

		private static readonly Action<ILogger, Exception> _closeTimedOut = LoggerMessage.Define(LogLevel.Debug, new EventId(8, "CloseTimedOut"), "Timed out waiting for client to send the close frame, aborting the connection.");

		private static readonly Action<ILogger, WebSocketMessageType, int, bool, Exception> _messageReceived = LoggerMessage.Define<WebSocketMessageType, int, bool>(LogLevel.Trace, new EventId(9, "MessageReceived"), "Message received. Type: {MessageType}, size: {Size}, EndOfMessage: {EndOfMessage}.");

		private static readonly Action<ILogger, int, Exception> _messageToApplication = LoggerMessage.Define<int>(LogLevel.Trace, new EventId(10, "MessageToApplication"), "Passing message to application. Payload size: {Size}.");

		private static readonly Action<ILogger, long, Exception> _sendPayload = LoggerMessage.Define<long>(LogLevel.Trace, new EventId(11, "SendPayload"), "Sending payload: {Size} bytes.");

		private static readonly Action<ILogger, Exception> _errorWritingFrame = LoggerMessage.Define(LogLevel.Error, new EventId(12, "ErrorWritingFrame"), "Error writing frame.");

		private static readonly Action<ILogger, Exception> _sendFailed = LoggerMessage.Define(LogLevel.Error, new EventId(13, "SendFailed"), "Socket failed to send.");

		private static readonly Action<ILogger, Exception> _closedPrematurely = LoggerMessage.Define(LogLevel.Debug, new EventId(14, "ClosedPrematurely"), "Socket connection closed prematurely.");

		public static void SocketOpened(ILogger logger, string subProtocol)
		{
			_socketOpened(logger, subProtocol, null);
		}

		public static void SocketClosed(ILogger logger)
		{
			_socketClosed(logger, null);
		}

		public static void ClientClosed(ILogger logger, WebSocketCloseStatus? closeStatus, string closeDescription)
		{
			_clientClosed(logger, closeStatus, closeDescription, null);
		}

		public static void WaitingForSend(ILogger logger)
		{
			_waitingForSend(logger, null);
		}

		public static void FailedSending(ILogger logger)
		{
			_failedSending(logger, null);
		}

		public static void FinishedSending(ILogger logger)
		{
			_finishedSending(logger, null);
		}

		public static void WaitingForClose(ILogger logger)
		{
			_waitingForClose(logger, null);
		}

		public static void CloseTimedOut(ILogger logger)
		{
			_closeTimedOut(logger, null);
		}

		public static void MessageReceived(ILogger logger, WebSocketMessageType type, int size, bool endOfMessage)
		{
			_messageReceived(logger, type, size, endOfMessage, null);
		}

		public static void MessageToApplication(ILogger logger, int size)
		{
			_messageToApplication(logger, size, null);
		}

		public static void SendPayload(ILogger logger, long size)
		{
			_sendPayload(logger, size, null);
		}

		public static void ErrorWritingFrame(ILogger logger, Exception ex)
		{
			_errorWritingFrame(logger, ex);
		}

		public static void SendFailed(ILogger logger, Exception ex)
		{
			_sendFailed(logger, ex);
		}

		public static void ClosedPrematurely(ILogger logger, Exception ex)
		{
			_closedPrematurely(logger, ex);
		}
	}

	private readonly WebSocketOptions _options;

	private readonly ILogger _logger;

	private readonly IDuplexPipe _application;

	private readonly HttpConnectionContext _connection;

	private volatile bool _aborted;

	public WebSocketsTransport(WebSocketOptions options, IDuplexPipe application, HttpConnectionContext connection, ILoggerFactory loggerFactory)
	{
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		if (application == null)
		{
			throw new ArgumentNullException("application");
		}
		if (loggerFactory == null)
		{
			throw new ArgumentNullException("loggerFactory");
		}
		_options = options;
		_application = application;
		_connection = connection;
		_logger = loggerFactory.CreateLogger<WebSocketsTransport>();
	}

	public async Task ProcessRequestAsync(HttpContext context, CancellationToken token)
	{
		string subProtocol = _options.SubProtocolSelector?.Invoke(context.WebSockets.WebSocketRequestedProtocols);
		using WebSocket ws = await context.WebSockets.AcceptWebSocketAsync(subProtocol);
		Log.SocketOpened(_logger, subProtocol);
		try
		{
			await ProcessSocketAsync(ws);
		}
		finally
		{
			Log.SocketClosed(_logger);
		}
	}

	public async Task ProcessSocketAsync(WebSocket socket)
	{
		Task receiving = StartReceiving(socket);
		Task sending = StartSending(socket);
		if (await Task.WhenAny(new Task[2] { receiving, sending }) == receiving)
		{
			Log.WaitingForSend(_logger);
			_application.Input.CancelPendingRead();
			using (CancellationTokenSource delayCts = new CancellationTokenSource())
			{
				if (await Task.WhenAny(new Task[2]
				{
					sending,
					Task.Delay(_options.CloseTimeout, delayCts.Token)
				}) != sending)
				{
					Log.CloseTimedOut(_logger);
					_aborted = true;
					socket.Abort();
				}
				else
				{
					delayCts.Cancel();
				}
			}
			return;
		}
		Log.WaitingForClose(_logger);
		using CancellationTokenSource delayCts = new CancellationTokenSource();
		if (await Task.WhenAny(new Task[2]
		{
			receiving,
			Task.Delay(_options.CloseTimeout, delayCts.Token)
		}) != receiving)
		{
			_aborted = true;
			socket.Abort();
			_application.Output.CancelPendingFlush();
		}
		else
		{
			delayCts.Cancel();
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
					break;
				}
				Log.MessageReceived(_logger, webSocketReceiveResult.MessageType, webSocketReceiveResult.Count, webSocketReceiveResult.EndOfMessage);
				_application.Output.Advance(webSocketReceiveResult.Count);
				flushResult = await _application.Output.FlushAsync();
			}
			while (!flushResult.IsCanceled && !flushResult.IsCompleted);
		}
		catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
		{
			Log.ClosedPrematurely(_logger, ex);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception exception)
		{
			if (!_aborted)
			{
				_application.Output.Complete(exception);
				throw;
			}
		}
		finally
		{
			_application.Output.Complete();
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
								Log.SendPayload(_logger, buffer.Length);
								WebSocketMessageType webSocketMessageType = ((_connection.ActiveFormat == TransferFormat.Binary) ? WebSocketMessageType.Binary : WebSocketMessageType.Text);
								if (WebSocketCanSend(socket))
								{
									await socket.SendAsync(buffer, webSocketMessageType);
									continue;
								}
							}
							catch (Exception ex)
							{
								if (!_aborted)
								{
									Log.ErrorWritingFrame(_logger, ex);
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
			catch (Exception ex2)
			{
				error = ex2;
			}
		}
		finally
		{
			if (WebSocketCanSend(socket))
			{
				await socket.CloseOutputAsync((error != null) ? WebSocketCloseStatus.InternalServerError : WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
			}
			_application.Input.Complete();
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
}
