using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections.Internal.Transports;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Http.Connections.Internal;

public class HttpConnectionDispatcher
{
	private class EmptyServiceProvider : IServiceProvider
	{
		public static EmptyServiceProvider Instance { get; } = new EmptyServiceProvider();


		public object GetService(Type serviceType)
		{
			return null;
		}
	}

	private static class Log
	{
		private static readonly Action<ILogger, string, Exception> _connectionDisposed = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1, "ConnectionDisposed"), "Connection {TransportConnectionId} was disposed.");

		private static readonly Action<ILogger, string, string, Exception> _connectionAlreadyActive = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(2, "ConnectionAlreadyActive"), "Connection {TransportConnectionId} is already active via {RequestId}.");

		private static readonly Action<ILogger, string, string, Exception> _pollCanceled = LoggerMessage.Define<string, string>(LogLevel.Trace, new EventId(3, "PollCanceled"), "Previous poll canceled for {TransportConnectionId} on {RequestId}.");

		private static readonly Action<ILogger, Exception> _establishedConnection = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "EstablishedConnection"), "Establishing new connection.");

		private static readonly Action<ILogger, Exception> _resumingConnection = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "ResumingConnection"), "Resuming existing connection.");

		private static readonly Action<ILogger, long, Exception> _receivedBytes = LoggerMessage.Define<long>(LogLevel.Trace, new EventId(6, "ReceivedBytes"), "Received {Count} bytes.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _transportNotSupported = LoggerMessage.Define<HttpTransportType>(LogLevel.Debug, new EventId(7, "TransportNotSupported"), "{TransportType} transport not supported by this connection handler.");

		private static readonly Action<ILogger, HttpTransportType, HttpTransportType, Exception> _cannotChangeTransport = LoggerMessage.Define<HttpTransportType, HttpTransportType>(LogLevel.Error, new EventId(8, "CannotChangeTransport"), "Cannot change transports mid-connection; currently using {TransportType}, requesting {RequestedTransport}.");

		private static readonly Action<ILogger, Exception> _postNotallowedForWebsockets = LoggerMessage.Define(LogLevel.Debug, new EventId(9, "PostNotAllowedForWebSockets"), "POST requests are not allowed for websocket connections.");

		private static readonly Action<ILogger, Exception> _negotiationRequest = LoggerMessage.Define(LogLevel.Debug, new EventId(10, "NegotiationRequest"), "Sending negotiation response.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _receivedDeleteRequestForUnsupportedTransport = LoggerMessage.Define<HttpTransportType>(LogLevel.Trace, new EventId(11, "ReceivedDeleteRequestForUnsupportedTransport"), "Received DELETE request for unsupported transport: {TransportType}.");

		private static readonly Action<ILogger, Exception> _terminatingConnection = LoggerMessage.Define(LogLevel.Trace, new EventId(12, "TerminatingConection"), "Terminating Long Polling connection due to a DELETE request.");

		private static readonly Action<ILogger, string, Exception> _connectionDisposedWhileWriteInProgress = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(13, "ConnectionDisposedWhileWriteInProgress"), "Connection {TransportConnectionId} was disposed while a write was in progress.");

		public static void ConnectionDisposed(ILogger logger, string connectionId)
		{
			_connectionDisposed(logger, connectionId, null);
		}

		public static void ConnectionAlreadyActive(ILogger logger, string connectionId, string requestId)
		{
			_connectionAlreadyActive(logger, connectionId, requestId, null);
		}

		public static void PollCanceled(ILogger logger, string connectionId, string requestId)
		{
			_pollCanceled(logger, connectionId, requestId, null);
		}

		public static void EstablishedConnection(ILogger logger)
		{
			_establishedConnection(logger, null);
		}

		public static void ResumingConnection(ILogger logger)
		{
			_resumingConnection(logger, null);
		}

		public static void ReceivedBytes(ILogger logger, long count)
		{
			_receivedBytes(logger, count, null);
		}

		public static void TransportNotSupported(ILogger logger, HttpTransportType transport)
		{
			_transportNotSupported(logger, transport, null);
		}

		public static void CannotChangeTransport(ILogger logger, HttpTransportType transport, HttpTransportType requestTransport)
		{
			_cannotChangeTransport(logger, transport, requestTransport, null);
		}

		public static void PostNotAllowedForWebSockets(ILogger logger)
		{
			_postNotallowedForWebsockets(logger, null);
		}

		public static void NegotiationRequest(ILogger logger)
		{
			_negotiationRequest(logger, null);
		}

		public static void ReceivedDeleteRequestForUnsupportedTransport(ILogger logger, HttpTransportType transportType)
		{
			_receivedDeleteRequestForUnsupportedTransport(logger, transportType, null);
		}

		public static void TerminatingConection(ILogger logger)
		{
			_terminatingConnection(logger, null);
		}

		public static void ConnectionDisposedWhileWriteInProgress(ILogger logger, string connectionId, Exception ex)
		{
			_connectionDisposedWhileWriteInProgress(logger, connectionId, ex);
		}
	}

	private static readonly AvailableTransport _webSocketAvailableTransport = new AvailableTransport
	{
		Transport = "WebSockets",
		TransferFormats = new List<string> { "Text", "Binary" }
	};

	private static readonly AvailableTransport _serverSentEventsAvailableTransport = new AvailableTransport
	{
		Transport = "ServerSentEvents",
		TransferFormats = new List<string> { "Text" }
	};

	private static readonly AvailableTransport _longPollingAvailableTransport = new AvailableTransport
	{
		Transport = "LongPolling",
		TransferFormats = new List<string> { "Text", "Binary" }
	};

	private readonly HttpConnectionManager _manager;

	private readonly ILoggerFactory _loggerFactory;

	private readonly ILogger _logger;

	public HttpConnectionDispatcher(HttpConnectionManager manager, ILoggerFactory loggerFactory)
	{
		_manager = manager;
		_loggerFactory = loggerFactory;
		_logger = _loggerFactory.CreateLogger<HttpConnectionDispatcher>();
	}

	public async Task ExecuteAsync(HttpContext context, HttpConnectionDispatcherOptions options, ConnectionDelegate connectionDelegate)
	{
		ConnectionLogScope logScope = new ConnectionLogScope(GetConnectionId(context));
		using (_logger.BeginScope(logScope))
		{
			if (!(await AuthorizeHelper.AuthorizeAsync(context, options.AuthorizationData)))
			{
				return;
			}
			if (HttpMethods.IsPost(context.Request.Method))
			{
				await ProcessSend(context, options);
				return;
			}
			if (HttpMethods.IsGet(context.Request.Method))
			{
				await ExecuteAsync(context, connectionDelegate, options, logScope);
				return;
			}
			if (HttpMethods.IsDelete(context.Request.Method))
			{
				await ProcessDeleteAsync(context);
				return;
			}
			context.Response.ContentType = "text/plain";
			context.Response.StatusCode = 405;
		}
	}

	public async Task ExecuteNegotiateAsync(HttpContext context, HttpConnectionDispatcherOptions options)
	{
		ConnectionLogScope logScope = new ConnectionLogScope(string.Empty);
		using (_logger.BeginScope(logScope))
		{
			if (!(await AuthorizeHelper.AuthorizeAsync(context, options.AuthorizationData)))
			{
				return;
			}
			if (HttpMethods.IsPost(context.Request.Method))
			{
				await ProcessNegotiate(context, options, logScope);
				return;
			}
			context.Response.ContentType = "text/plain";
			context.Response.StatusCode = 405;
		}
	}

	private async Task ExecuteAsync(HttpContext context, ConnectionDelegate connectionDelegate, HttpConnectionDispatcherOptions options, ConnectionLogScope logScope)
	{
		HttpTransportType supportedTransports = options.Transports;
		HttpConnectionContext connection;
		if (context.Request.GetTypedHeaders().Accept?.Contains(new MediaTypeHeaderValue("text/event-stream")) ?? false)
		{
			connection = await GetConnectionAsync(context);
			if (connection != null && await EnsureConnectionStateAsync(connection, context, HttpTransportType.ServerSentEvents, supportedTransports, logScope, options))
			{
				Log.EstablishedConnection(_logger);
				connection.SupportedFormats = TransferFormat.Text;
				ServerSentEventsTransport transport = new ServerSentEventsTransport(connection.Application.Input, ((ConnectionContext)connection).ConnectionId, _loggerFactory);
				await DoPersistentConnection(connectionDelegate, transport, context, connection);
			}
			return;
		}
		if (context.WebSockets.IsWebSocketRequest)
		{
			connection = await GetOrCreateConnectionAsync(context, options);
			if (connection != null && await EnsureConnectionStateAsync(connection, context, HttpTransportType.WebSockets, supportedTransports, logScope, options))
			{
				Log.EstablishedConnection(_logger);
				WebSocketsTransport transport2 = new WebSocketsTransport(options.WebSockets, connection.Application, connection, _loggerFactory);
				await DoPersistentConnection(connectionDelegate, transport2, context, connection);
			}
			return;
		}
		connection = await GetConnectionAsync(context);
		if (connection == null || !(await EnsureConnectionStateAsync(connection, context, HttpTransportType.LongPolling, supportedTransports, logScope, options)))
		{
			return;
		}
		TaskCompletionSource<object> currentRequestTcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
		await connection.StateLock.WaitAsync();
		try
		{
			if (connection.Status == HttpConnectionStatus.Disposed)
			{
				Log.ConnectionDisposed(_logger, ((ConnectionContext)connection).ConnectionId);
				context.Response.StatusCode = 404;
				context.Response.ContentType = "text/plain";
				return;
			}
			if (connection.Status == HttpConnectionStatus.Active)
			{
				HttpContext httpContext = connection.GetHttpContext();
				Log.ConnectionAlreadyActive(_logger, ((ConnectionContext)connection).ConnectionId, httpContext.TraceIdentifier);
			}
			using (connection.Cancellation)
			{
				connection.Cancellation?.Cancel();
				try
				{
					await connection.PreviousPollTask;
				}
				catch (OperationCanceledException)
				{
					context.Response.ContentType = "text/plain";
					context.Response.StatusCode = 204;
					return;
				}
				connection.PreviousPollTask = currentRequestTcs.Task;
			}
			connection.Status = HttpConnectionStatus.Active;
			if (connection.ApplicationTask == null)
			{
				Log.EstablishedConnection(_logger);
				connection.ApplicationTask = ExecuteApplication(connectionDelegate, connection);
				context.Response.ContentType = "application/octet-stream";
				context.Response.ContentLength = 0L;
				connection.TransportTask = context.Response.Body.FlushAsync();
			}
			else
			{
				Log.ResumingConnection(_logger);
				connection.Cancellation = new CancellationTokenSource();
				CancellationTokenSource cancellationTokenSource2 = new CancellationTokenSource();
				CancellationTokenSource cancellationTokenSource3 = CancellationTokenSource.CreateLinkedTokenSource(connection.Cancellation.Token, context.RequestAborted, cancellationTokenSource2.Token);
				context.Response.RegisterForDispose(cancellationTokenSource2);
				context.Response.RegisterForDispose(cancellationTokenSource3);
				LongPollingTransport longPollingTransport = new LongPollingTransport(cancellationTokenSource2.Token, connection.Application.Input, _loggerFactory);
				connection.TransportTask = longPollingTransport.ProcessRequestAsync(context, cancellationTokenSource3.Token);
				cancellationTokenSource2.CancelAfter(options.LongPolling.PollTimeout);
			}
		}
		finally
		{
			connection.StateLock.Release();
		}
		Task task = await Task.WhenAny(new Task[2] { connection.ApplicationTask, connection.TransportTask });
		try
		{
			bool pollAgain = true;
			if (task == connection.ApplicationTask)
			{
				connection.Transport.Output.Complete(connection.ApplicationTask.Exception);
				await connection.TransportTask;
				if (context.Response.StatusCode == 204)
				{
					currentRequestTcs.TrySetCanceled();
					await _manager.DisposeAndRemoveAsync(connection, closeGracefully: false);
					pollAgain = false;
				}
			}
			else if (context.Response.StatusCode == 204)
			{
				pollAgain = false;
			}
			if (pollAgain)
			{
				connection.LastSeenUtc = DateTime.UtcNow;
				connection.Status = HttpConnectionStatus.Inactive;
			}
		}
		finally
		{
			currentRequestTcs.TrySetResult(null);
		}
	}

	private async Task DoPersistentConnection(ConnectionDelegate connectionDelegate, IHttpTransport transport, HttpContext context, HttpConnectionContext connection)
	{
		await connection.StateLock.WaitAsync();
		try
		{
			if (connection.Status == HttpConnectionStatus.Disposed)
			{
				Log.ConnectionDisposed(_logger, ((ConnectionContext)connection).ConnectionId);
				context.Response.StatusCode = 404;
				return;
			}
			if (connection.Status == HttpConnectionStatus.Active)
			{
				Log.ConnectionAlreadyActive(_logger, ((ConnectionContext)connection).ConnectionId, connection.GetHttpContext().TraceIdentifier);
				context.Response.StatusCode = 409;
				return;
			}
			connection.Status = HttpConnectionStatus.Active;
			connection.ApplicationTask = ExecuteApplication(connectionDelegate, connection);
			connection.TransportTask = transport.ProcessRequestAsync(context, context.RequestAborted);
		}
		finally
		{
			connection.StateLock.Release();
		}
		await Task.WhenAny(new Task[2] { connection.ApplicationTask, connection.TransportTask });
		await _manager.DisposeAndRemoveAsync(connection, closeGracefully: true);
	}

	private async Task ExecuteApplication(ConnectionDelegate connectionDelegate, HttpConnectionContext connection)
	{
		await AwaitableThreadPool.Yield();
		await connectionDelegate(connection);
	}

	private async Task ProcessNegotiate(HttpContext context, HttpConnectionDispatcherOptions options, ConnectionLogScope logScope)
	{
		context.Response.ContentType = "application/json";
		HttpConnectionContext httpConnectionContext = CreateConnection(options);
		logScope.ConnectionId = ((ConnectionContext)httpConnectionContext).ConnectionId;
		MemoryBufferWriter writer = new MemoryBufferWriter();
		try
		{
			WriteNegotiatePayload(writer, ((ConnectionContext)httpConnectionContext).ConnectionId, context, options);
			Log.NegotiationRequest(_logger);
			context.Response.ContentLength = writer.Length;
			await writer.CopyToAsync(context.Response.Body);
		}
		finally
		{
			writer.Reset();
		}
	}

	private static void WriteNegotiatePayload(IBufferWriter<byte> writer, string connectionId, HttpContext context, HttpConnectionDispatcherOptions options)
	{
		NegotiationResponse negotiationResponse = new NegotiationResponse();
		negotiationResponse.ConnectionId = connectionId;
		negotiationResponse.AvailableTransports = new List<AvailableTransport>();
		if ((options.Transports & HttpTransportType.WebSockets) != 0 && ServerHasWebSockets(context.Features))
		{
			negotiationResponse.AvailableTransports.Add(_webSocketAvailableTransport);
		}
		if ((options.Transports & HttpTransportType.ServerSentEvents) != 0)
		{
			negotiationResponse.AvailableTransports.Add(_serverSentEventsAvailableTransport);
		}
		if ((options.Transports & HttpTransportType.LongPolling) != 0)
		{
			negotiationResponse.AvailableTransports.Add(_longPollingAvailableTransport);
		}
		NegotiateProtocol.WriteResponse(negotiationResponse, writer);
	}

	private static bool ServerHasWebSockets(IFeatureCollection features)
	{
		return features.Get<IHttpWebSocketFeature>() != null;
	}

	private static string GetConnectionId(HttpContext context)
	{
		return context.Request.Query["id"];
	}

	private async Task ProcessSend(HttpContext context, HttpConnectionDispatcherOptions options)
	{
		HttpConnectionContext connection = await GetConnectionAsync(context);
		if (connection == null)
		{
			return;
		}
		context.Response.ContentType = "text/plain";
		if (connection.TransportType == HttpTransportType.WebSockets)
		{
			Log.PostNotAllowedForWebSockets(_logger);
			context.Response.StatusCode = 405;
			await context.Response.WriteAsync("POST requests are not allowed for WebSocket connections.");
			return;
		}
		await connection.WriteLock.WaitAsync();
		try
		{
			if (connection.Status == HttpConnectionStatus.Disposed)
			{
				Log.ConnectionDisposed(_logger, ((ConnectionContext)connection).ConnectionId);
				context.Response.StatusCode = 404;
				context.Response.ContentType = "text/plain";
				return;
			}
			try
			{
				_ = 3;
				try
				{
					await context.Request.Body.CopyToAsync(connection.ApplicationStream, 4096);
				}
				catch (InvalidOperationException ex)
				{
					Log.ConnectionDisposedWhileWriteInProgress(_logger, ((ConnectionContext)connection).ConnectionId, ex);
					context.Response.StatusCode = 404;
					context.Response.ContentType = "text/plain";
					return;
				}
				catch (OperationCanceledException)
				{
					Log.ConnectionDisposed(_logger, ((ConnectionContext)connection).ConnectionId);
					context.Response.StatusCode = 404;
					context.Response.ContentType = "text/plain";
					return;
				}
				Log.ReceivedBytes(_logger, connection.ApplicationStream.Length);
			}
			finally
			{
				connection.ApplicationStream.Reset();
			}
		}
		finally
		{
			connection.WriteLock.Release();
		}
	}

	private async Task ProcessDeleteAsync(HttpContext context)
	{
		HttpConnectionContext httpConnectionContext = await GetConnectionAsync(context);
		if (httpConnectionContext != null)
		{
			if (httpConnectionContext.TransportType != HttpTransportType.LongPolling)
			{
				Log.ReceivedDeleteRequestForUnsupportedTransport(_logger, httpConnectionContext.TransportType);
				context.Response.StatusCode = 400;
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync("Cannot terminate this connection using the DELETE endpoint.");
			}
			else
			{
				Log.TerminatingConection(_logger);
				httpConnectionContext.Application.Output.Complete();
				httpConnectionContext.DisposeAndRemoveTask = _manager.DisposeAndRemoveAsync(httpConnectionContext, closeGracefully: true);
				context.Response.StatusCode = 202;
				context.Response.ContentType = "text/plain";
			}
		}
	}

	private async Task<bool> EnsureConnectionStateAsync(HttpConnectionContext connection, HttpContext context, HttpTransportType transportType, HttpTransportType supportedTransports, ConnectionLogScope logScope, HttpConnectionDispatcherOptions options)
	{
		if ((supportedTransports & transportType) == 0)
		{
			context.Response.ContentType = "text/plain";
			context.Response.StatusCode = 404;
			Log.TransportNotSupported(_logger, transportType);
			await context.Response.WriteAsync($"{transportType} transport not supported by this end point type");
			return false;
		}
		((ConnectionContext)connection).Features.Set(context.Features.Get<IHttpConnectionFeature>());
		if (connection.TransportType == HttpTransportType.None)
		{
			connection.TransportType = transportType;
		}
		else if (connection.TransportType != transportType)
		{
			context.Response.ContentType = "text/plain";
			context.Response.StatusCode = 400;
			Log.CannotChangeTransport(_logger, connection.TransportType, transportType);
			await context.Response.WriteAsync("Cannot change transports mid-connection");
			return false;
		}
		connection.User = context.User;
		if (transportType == HttpTransportType.LongPolling)
		{
			connection.HasInherentKeepAlive = true;
			HttpContext httpContext = connection.HttpContext;
			if (httpContext == null)
			{
				HttpContext httpContext2 = CloneHttpContext(context);
				connection.HttpContext = httpContext2;
			}
			else
			{
				httpContext.TraceIdentifier = context.TraceIdentifier;
				httpContext.User = context.User;
			}
		}
		else
		{
			connection.HttpContext = context;
		}
		logScope.ConnectionId = ((ConnectionContext)connection).ConnectionId;
		return true;
	}

	private static HttpContext CloneHttpContext(HttpContext context)
	{
		IHttpRequestFeature httpRequestFeature = context.Features.Get<IHttpRequestFeature>();
		HttpRequestFeature httpRequestFeature2 = new HttpRequestFeature();
		httpRequestFeature2.Protocol = httpRequestFeature.Protocol;
		httpRequestFeature2.Method = httpRequestFeature.Method;
		httpRequestFeature2.Scheme = httpRequestFeature.Scheme;
		httpRequestFeature2.Path = httpRequestFeature.Path;
		httpRequestFeature2.PathBase = httpRequestFeature.PathBase;
		httpRequestFeature2.QueryString = httpRequestFeature.QueryString;
		httpRequestFeature2.RawTarget = httpRequestFeature.RawTarget;
		Dictionary<string, StringValues> dictionary = new Dictionary<string, StringValues>(httpRequestFeature.Headers.Count, StringComparer.Ordinal);
		foreach (KeyValuePair<string, StringValues> header in httpRequestFeature.Headers)
		{
			dictionary[header.Key] = header.Value;
		}
		httpRequestFeature2.Headers = new HeaderDictionary(dictionary);
		IHttpConnectionFeature httpConnectionFeature = context.Features.Get<IHttpConnectionFeature>();
		HttpConnectionFeature httpConnectionFeature2 = new HttpConnectionFeature();
		if (httpConnectionFeature != null)
		{
			httpConnectionFeature2.ConnectionId = httpConnectionFeature.ConnectionId;
			httpConnectionFeature2.LocalIpAddress = httpConnectionFeature.LocalIpAddress;
			httpConnectionFeature2.LocalPort = httpConnectionFeature.LocalPort;
			httpConnectionFeature2.RemoteIpAddress = httpConnectionFeature.RemoteIpAddress;
			httpConnectionFeature2.RemotePort = httpConnectionFeature.RemotePort;
		}
		HttpResponseFeature instance = new HttpResponseFeature();
		FeatureCollection featureCollection = new FeatureCollection();
		featureCollection.Set((IHttpRequestFeature)httpRequestFeature2);
		featureCollection.Set((IHttpResponseFeature)instance);
		featureCollection.Set((IHttpConnectionFeature)httpConnectionFeature2);
		return new DefaultHttpContext(featureCollection)
		{
			TraceIdentifier = context.TraceIdentifier,
			User = context.User,
			RequestServices = EmptyServiceProvider.Instance,
			Items = new Dictionary<object, object>(context.Items)
		};
	}

	private async Task<HttpConnectionContext> GetConnectionAsync(HttpContext context)
	{
		string connectionId = GetConnectionId(context);
		if (StringValues.IsNullOrEmpty(connectionId))
		{
			context.Response.StatusCode = 400;
			context.Response.ContentType = "text/plain";
			await context.Response.WriteAsync("Connection ID required");
			return null;
		}
		if (!_manager.TryGetConnection(connectionId, out var connection))
		{
			context.Response.StatusCode = 404;
			context.Response.ContentType = "text/plain";
			await context.Response.WriteAsync("No Connection with that ID");
			return null;
		}
		return connection;
	}

	private async Task<HttpConnectionContext> GetOrCreateConnectionAsync(HttpContext context, HttpConnectionDispatcherOptions options)
	{
		string connectionId = GetConnectionId(context);
		HttpConnectionContext connection;
		if (StringValues.IsNullOrEmpty(connectionId))
		{
			connection = CreateConnection(options);
		}
		else if (!_manager.TryGetConnection(connectionId, out connection))
		{
			context.Response.StatusCode = 404;
			await context.Response.WriteAsync("No Connection with that ID");
			return null;
		}
		return connection;
	}

	private HttpConnectionContext CreateConnection(HttpConnectionDispatcherOptions options)
	{
		PipeOptions transportPipeOptions = new PipeOptions(null, pauseWriterThreshold: options.TransportMaxBufferSize, resumeWriterThreshold: options.TransportMaxBufferSize / 2, readerScheduler: PipeScheduler.ThreadPool, writerScheduler: null, minimumSegmentSize: 2048, useSynchronizationContext: false);
		PipeOptions appPipeOptions = new PipeOptions(null, pauseWriterThreshold: options.ApplicationMaxBufferSize, resumeWriterThreshold: options.ApplicationMaxBufferSize / 2, readerScheduler: PipeScheduler.ThreadPool, writerScheduler: null, minimumSegmentSize: 2048, useSynchronizationContext: false);
		return _manager.CreateConnection(transportPipeOptions, appPipeOptions);
	}
}
