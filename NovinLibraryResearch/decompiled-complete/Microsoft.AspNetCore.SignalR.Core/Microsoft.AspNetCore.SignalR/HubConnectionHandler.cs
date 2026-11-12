using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.SignalR;

public class HubConnectionHandler<THub> : ConnectionHandler where THub : Hub
{
	private static class Log
	{
		private static readonly Action<ILogger, string, Exception> _errorDispatchingHubEvent = LoggerMessage.Define<string>(LogLevel.Error, new EventId(1, "ErrorDispatchingHubEvent"), "Error when dispatching '{HubMethod}' on hub.");

		private static readonly Action<ILogger, Exception> _errorProcessingRequest = LoggerMessage.Define(LogLevel.Error, new EventId(2, "ErrorProcessingRequest"), "Error when processing requests.");

		private static readonly Action<ILogger, Exception> _abortFailed = LoggerMessage.Define(LogLevel.Trace, new EventId(3, "AbortFailed"), "Abort callback failed.");

		private static readonly Action<ILogger, Exception> _errorSendingClose = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "ErrorSendingClose"), "Error when sending Close message.");

		private static readonly Action<ILogger, Exception> _connectedStarting = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "ConnectedStarting"), "OnConnectedAsync started.");

		private static readonly Action<ILogger, Exception> _connectedEnding = LoggerMessage.Define(LogLevel.Debug, new EventId(6, "ConnectedEnding"), "OnConnectedAsync ending.");

		public static void ErrorDispatchingHubEvent(ILogger logger, string hubMethod, Exception exception)
		{
			_errorDispatchingHubEvent(logger, hubMethod, exception);
		}

		public static void ErrorProcessingRequest(ILogger logger, Exception exception)
		{
			_errorProcessingRequest(logger, exception);
		}

		public static void AbortFailed(ILogger logger, Exception exception)
		{
			_abortFailed(logger, exception);
		}

		public static void ErrorSendingClose(ILogger logger, Exception exception)
		{
			_errorSendingClose(logger, exception);
		}

		public static void ConnectedStarting(ILogger logger)
		{
			_connectedStarting(logger, null);
		}

		public static void ConnectedEnding(ILogger logger)
		{
			_connectedEnding(logger, null);
		}
	}

	private readonly HubLifetimeManager<THub> _lifetimeManager;

	private readonly ILoggerFactory _loggerFactory;

	private readonly ILogger<HubConnectionHandler<THub>> _logger;

	private readonly IHubProtocolResolver _protocolResolver;

	private readonly HubOptions<THub> _hubOptions;

	private readonly HubOptions _globalHubOptions;

	private readonly IUserIdProvider _userIdProvider;

	private readonly HubDispatcher<THub> _dispatcher;

	private readonly bool _enableDetailedErrors;

	public HubConnectionHandler(HubLifetimeManager<THub> lifetimeManager, IHubProtocolResolver protocolResolver, IOptions<HubOptions> globalHubOptions, IOptions<HubOptions<THub>> hubOptions, ILoggerFactory loggerFactory, IUserIdProvider userIdProvider, HubDispatcher<THub> dispatcher)
	{
		_protocolResolver = protocolResolver;
		_lifetimeManager = lifetimeManager;
		_loggerFactory = loggerFactory;
		_hubOptions = hubOptions.Value;
		_globalHubOptions = globalHubOptions.Value;
		_logger = loggerFactory.CreateLogger<HubConnectionHandler<THub>>();
		_userIdProvider = userIdProvider;
		_dispatcher = dispatcher;
		_enableDetailedErrors = _hubOptions.EnableDetailedErrors ?? _globalHubOptions.EnableDetailedErrors ?? false;
	}

	public override async Task OnConnectedAsync(ConnectionContext connection)
	{
		TimeSpan keepAliveInterval = _hubOptions.KeepAliveInterval ?? _globalHubOptions.KeepAliveInterval ?? HubOptionsSetup.DefaultKeepAliveInterval;
		TimeSpan timeout = _hubOptions.HandshakeTimeout ?? _globalHubOptions.HandshakeTimeout ?? HubOptionsSetup.DefaultHandshakeTimeout;
		IList<string> list = _hubOptions.SupportedProtocols ?? _globalHubOptions.SupportedProtocols;
		if (list != null && list.Count == 0)
		{
			throw new InvalidOperationException("There are no supported protocols");
		}
		Log.ConnectedStarting(_logger);
		HubConnectionContext connectionContext = new HubConnectionContext(connection, keepAliveInterval, _loggerFactory);
		IReadOnlyList<string> supportedProtocols = (list as IReadOnlyList<string>) ?? list.ToList();
		if (await connectionContext.HandshakeAsync(timeout, supportedProtocols, _protocolResolver, _userIdProvider, _enableDetailedErrors))
		{
			try
			{
				await _lifetimeManager.OnConnectedAsync(connectionContext);
				await RunHubAsync(connectionContext);
			}
			finally
			{
				Log.ConnectedEnding(_logger);
				await _lifetimeManager.OnDisconnectedAsync(connectionContext);
			}
		}
	}

	private async Task RunHubAsync(HubConnectionContext connection)
	{
		try
		{
			await _dispatcher.OnConnectedAsync(connection);
		}
		catch (Exception exception)
		{
			Log.ErrorDispatchingHubEvent(_logger, "OnConnectedAsync", exception);
			await SendCloseAsync(connection, exception);
			return;
		}
		try
		{
			await DispatchMessagesAsync(connection);
		}
		catch (Exception exception2)
		{
			Log.ErrorProcessingRequest(_logger, exception2);
			await HubOnDisconnectedAsync(connection, exception2);
			return;
		}
		await HubOnDisconnectedAsync(connection, null);
	}

	private async Task HubOnDisconnectedAsync(HubConnectionContext connection, Exception exception)
	{
		await SendCloseAsync(connection, exception);
		try
		{
			await connection.AbortAsync();
		}
		catch (Exception exception2)
		{
			Log.AbortFailed(_logger, exception2);
		}
		try
		{
			await _dispatcher.OnDisconnectedAsync(connection, exception);
		}
		catch (Exception exception3)
		{
			Log.ErrorDispatchingHubEvent(_logger, "OnDisconnectedAsync", exception3);
			throw;
		}
	}

	private async Task SendCloseAsync(HubConnectionContext connection, Exception exception)
	{
		CloseMessage message = CloseMessage.Empty;
		if (exception != null)
		{
			message = new CloseMessage(ErrorMessageHelper.BuildErrorMessage("Connection closed with an error.", exception, _enableDetailedErrors));
		}
		try
		{
			await connection.WriteAsync(message);
		}
		catch (Exception exception2)
		{
			Log.ErrorSendingClose(_logger, exception2);
		}
	}

	private async Task DispatchMessagesAsync(HubConnectionContext connection)
	{
		_ = 1;
		try
		{
			PipeReader input = connection.Input;
			IHubProtocol protocol = connection.Protocol;
			while (true)
			{
				ReadResult result = await input.ReadAsync(connection.ConnectionAborted);
				ReadOnlySequence<byte> buffer = result.Buffer;
				try
				{
					if (!buffer.IsEmpty)
					{
						HubMessage message;
						while (protocol.TryParseMessage(ref buffer, _dispatcher, out message))
						{
							await _dispatcher.DispatchMessageAsync(connection, message);
						}
					}
					if (result.IsCompleted)
					{
						if (!buffer.IsEmpty)
						{
							throw new InvalidDataException("Connection terminated while reading a message.");
						}
						break;
					}
				}
				finally
				{
					input.AdvanceTo(buffer.Start, buffer.End);
				}
			}
		}
		catch (OperationCanceledException)
		{
			connection.AbortException?.Throw();
		}
	}
}
