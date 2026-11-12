using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Internal;

public class HttpConnectionManager
{
	private static class Log
	{
		private static readonly Action<ILogger, string, Exception> _createdNewConnection = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1, "CreatedNewConnection"), "New connection {TransportConnectionId} created.");

		private static readonly Action<ILogger, string, Exception> _removedConnection = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(2, "RemovedConnection"), "Removing connection {TransportConnectionId} from the list of connections.");

		private static readonly Action<ILogger, string, Exception> _failedDispose = LoggerMessage.Define<string>(LogLevel.Error, new EventId(3, "FailedDispose"), "Failed disposing connection {TransportConnectionId}.");

		private static readonly Action<ILogger, string, Exception> _connectionReset = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(4, "ConnectionReset"), "Connection {TransportConnectionId} was reset.");

		private static readonly Action<ILogger, string, Exception> _connectionTimedOut = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(5, "ConnectionTimedOut"), "Connection {TransportConnectionId} timed out.");

		private static readonly Action<ILogger, Exception> _scanningConnections = LoggerMessage.Define(LogLevel.Trace, new EventId(6, "ScanningConnections"), "Scanning connections.");

		private static readonly Action<ILogger, Exception> _scanningConnectionsFailed = LoggerMessage.Define(LogLevel.Error, new EventId(7, "ScanningConnectionsFailed"), "Scanning connections failed.");

		private static readonly Action<ILogger, TimeSpan, Exception> _scannedConnections = LoggerMessage.Define<TimeSpan>(LogLevel.Trace, new EventId(8, "ScannedConnections"), "Scanned connections in {Duration}.");

		private static readonly Action<ILogger, Exception> _heartbeatStarted = LoggerMessage.Define(LogLevel.Trace, new EventId(9, "HeartBeatStarted"), "Starting connection heartbeat.");

		private static readonly Action<ILogger, Exception> _heartbeatEnded = LoggerMessage.Define(LogLevel.Trace, new EventId(10, "HeartBeatEnded"), "Ending connection heartbeat.");

		public static void CreatedNewConnection(ILogger logger, string connectionId)
		{
			_createdNewConnection(logger, connectionId, null);
		}

		public static void RemovedConnection(ILogger logger, string connectionId)
		{
			_removedConnection(logger, connectionId, null);
		}

		public static void FailedDispose(ILogger logger, string connectionId, Exception exception)
		{
			_failedDispose(logger, connectionId, exception);
		}

		public static void ConnectionTimedOut(ILogger logger, string connectionId)
		{
			_connectionTimedOut(logger, connectionId, null);
		}

		public static void ConnectionReset(ILogger logger, string connectionId, Exception exception)
		{
			_connectionReset(logger, connectionId, exception);
		}

		public static void ScanningConnections(ILogger logger)
		{
			_scanningConnections(logger, null);
		}

		public static void ScanningConnectionsFailed(ILogger logger, Exception exception)
		{
			_scanningConnectionsFailed(logger, exception);
		}

		public static void ScannedConnections(ILogger logger, TimeSpan duration)
		{
			_scannedConnections(logger, duration, null);
		}

		public static void HeartBeatStarted(ILogger logger)
		{
			_heartbeatStarted(logger, null);
		}

		public static void HeartBeatEnded(ILogger logger)
		{
			_heartbeatEnded(logger, null);
		}
	}

	private static readonly TimeSpan _heartbeatTickRate = TimeSpan.FromSeconds(1.0);

	private static readonly RNGCryptoServiceProvider _keyGenerator = new RNGCryptoServiceProvider();

	private readonly ConcurrentDictionary<string, (HttpConnectionContext Connection, Microsoft.Extensions.Internal.ValueStopwatch Timer)> _connections = new ConcurrentDictionary<string, (HttpConnectionContext, Microsoft.Extensions.Internal.ValueStopwatch)>(StringComparer.Ordinal);

	private readonly TimerAwaitable _nextHeartbeat;

	private readonly ILogger<HttpConnectionManager> _logger;

	private readonly ILogger<HttpConnectionContext> _connectionLogger;

	public HttpConnectionManager(ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
	{
		_logger = loggerFactory.CreateLogger<HttpConnectionManager>();
		_connectionLogger = loggerFactory.CreateLogger<HttpConnectionContext>();
		appLifetime.ApplicationStarted.Register(delegate
		{
			Start();
		});
		appLifetime.ApplicationStopping.Register(delegate
		{
			CloseConnections();
		});
		_nextHeartbeat = new TimerAwaitable(_heartbeatTickRate, _heartbeatTickRate);
	}

	public void Start()
	{
		_nextHeartbeat.Start();
		ExecuteTimerLoop();
	}

	public bool TryGetConnection(string id, out HttpConnectionContext connection)
	{
		connection = null;
		if (_connections.TryGetValue(id, out (HttpConnectionContext, Microsoft.Extensions.Internal.ValueStopwatch) value))
		{
			(connection, _) = value;
			return true;
		}
		return false;
	}

	public HttpConnectionContext CreateConnection()
	{
		return CreateConnection(PipeOptions.Default, PipeOptions.Default);
	}

	public HttpConnectionContext CreateConnection(PipeOptions transportPipeOptions, PipeOptions appPipeOptions)
	{
		string text = MakeNewConnectionId();
		Log.CreatedNewConnection(_logger, text);
		Microsoft.Extensions.Internal.ValueStopwatch item = HttpConnectionsEventSource.Log.ConnectionStart(text);
		HttpConnectionContext httpConnectionContext = new HttpConnectionContext(text, _connectionLogger);
		DuplexPipe.DuplexPipePair duplexPipePair = DuplexPipe.CreateConnectionPair(transportPipeOptions, appPipeOptions);
		httpConnectionContext.Transport = duplexPipePair.Application;
		httpConnectionContext.Application = duplexPipePair.Transport;
		_connections.TryAdd(text, (httpConnectionContext, item));
		return httpConnectionContext;
	}

	public void RemoveConnection(string id)
	{
		if (_connections.TryRemove(id, out (HttpConnectionContext, Microsoft.Extensions.Internal.ValueStopwatch) value))
		{
			HttpConnectionsEventSource.Log.ConnectionStop(id, value.Item2);
			Log.RemovedConnection(_logger, id);
		}
	}

	private static string MakeNewConnectionId()
	{
		byte[] array = new byte[16];
		_keyGenerator.GetBytes(array);
		return WebEncoders.Base64UrlEncode(array);
	}

	private async Task ExecuteTimerLoop()
	{
		Log.HeartBeatStarted(_logger);
		using (_nextHeartbeat)
		{
			while (await _nextHeartbeat)
			{
				try
				{
					await ScanAsync();
				}
				catch (Exception exception)
				{
					Log.ScanningConnectionsFailed(_logger, exception);
				}
			}
		}
		Log.HeartBeatEnded(_logger);
	}

	public async Task ScanAsync()
	{
		Microsoft.Extensions.Internal.ValueStopwatch timer = Microsoft.Extensions.Internal.ValueStopwatch.StartNew();
		HttpConnectionsEventSource.Log.ScanningConnections();
		Log.ScanningConnections(_logger);
		foreach (KeyValuePair<string, (HttpConnectionContext, Microsoft.Extensions.Internal.ValueStopwatch)> connection2 in _connections)
		{
			HttpConnectionContext connection = connection2.Value.Item1;
			await connection.StateLock.WaitAsync();
			HttpConnectionStatus status;
			DateTimeOffset dateTimeOffset;
			try
			{
				status = connection.Status;
				dateTimeOffset = connection.LastSeenUtc;
			}
			finally
			{
				connection.StateLock.Release();
			}
			if (!Debugger.IsAttached && status == HttpConnectionStatus.Inactive && (DateTimeOffset.UtcNow - dateTimeOffset).TotalSeconds > 5.0)
			{
				Log.ConnectionTimedOut(_logger, ((ConnectionContext)connection).ConnectionId);
				HttpConnectionsEventSource.Log.ConnectionTimedOut(((ConnectionContext)connection).ConnectionId);
				DisposeAndRemoveAsync(connection, closeGracefully: true);
			}
			else
			{
				connection.TickHeartbeat();
			}
		}
		TimeSpan elapsedTime = timer.GetElapsedTime();
		HttpConnectionsEventSource.Log.ScannedConnections(elapsedTime);
		Log.ScannedConnections(_logger, elapsedTime);
	}

	public void CloseConnections()
	{
		_nextHeartbeat.Stop();
		List<Task> list = new List<Task>();
		foreach (KeyValuePair<string, (HttpConnectionContext, Microsoft.Extensions.Internal.ValueStopwatch)> connection in _connections)
		{
			list.Add(DisposeAndRemoveAsync(connection.Value.Item1, closeGracefully: false));
		}
		Task.WaitAll(list.ToArray(), TimeSpan.FromSeconds(5.0));
	}

	public async Task DisposeAndRemoveAsync(HttpConnectionContext connection, bool closeGracefully)
	{
		try
		{
			await connection.DisposeAsync(closeGracefully);
		}
		catch (IOException exception)
		{
			Log.ConnectionReset(_logger, ((ConnectionContext)connection).ConnectionId, exception);
		}
		catch (WebSocketException ex) when (ex.InnerException is IOException)
		{
			Log.ConnectionReset(_logger, ((ConnectionContext)connection).ConnectionId, ex);
		}
		catch (Exception exception2)
		{
			Log.FailedDispose(_logger, ((ConnectionContext)connection).ConnectionId, exception2);
		}
		finally
		{
			RemoveConnection(((ConnectionContext)connection).ConnectionId);
		}
	}
}
