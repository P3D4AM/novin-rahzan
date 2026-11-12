using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.SignalR;

public class HubConnectionContext
{
	private static class Log
	{
		private static readonly Action<ILogger, string, Exception> _handshakeComplete = LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "HandshakeComplete"), "Completed connection handshake. Using HubProtocol '{Protocol}'.");

		private static readonly Action<ILogger, Exception> _handshakeCanceled = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "HandshakeCanceled"), "Handshake was canceled.");

		private static readonly Action<ILogger, Exception> _sentPing = LoggerMessage.Define(LogLevel.Trace, new EventId(3, "SentPing"), "Sent a ping message to the client.");

		private static readonly Action<ILogger, Exception> _transportBufferFull = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "TransportBufferFull"), "Unable to send Ping message to client, the transport buffer is full.");

		private static readonly Action<ILogger, Exception> _handshakeFailed = LoggerMessage.Define(LogLevel.Error, new EventId(5, "HandshakeFailed"), "Failed connection handshake.");

		private static readonly Action<ILogger, Exception> _failedWritingMessage = LoggerMessage.Define(LogLevel.Debug, new EventId(6, "FailedWritingMessage"), "Failed writing message.");

		private static readonly Action<ILogger, string, int, Exception> _protocolVersionFailed = LoggerMessage.Define<string, int>(LogLevel.Warning, new EventId(7, "ProtocolVersionFailed"), "Server does not support version {Version} of the {Protocol} protocol.");

		public static void HandshakeComplete(ILogger logger, string hubProtocol)
		{
			_handshakeComplete(logger, hubProtocol, null);
		}

		public static void HandshakeCanceled(ILogger logger)
		{
			_handshakeCanceled(logger, null);
		}

		public static void SentPing(ILogger logger)
		{
			_sentPing(logger, null);
		}

		public static void TransportBufferFull(ILogger logger)
		{
			_transportBufferFull(logger, null);
		}

		public static void HandshakeFailed(ILogger logger, Exception exception)
		{
			_handshakeFailed(logger, exception);
		}

		public static void FailedWritingMessage(ILogger logger, Exception exception)
		{
			_failedWritingMessage(logger, exception);
		}

		public static void ProtocolVersionFailed(ILogger logger, string protocolName, int version)
		{
			_protocolVersionFailed(logger, protocolName, version, null);
		}
	}

	private static readonly Action<object> _abortedCallback = AbortConnection;

	private readonly ConnectionContext _connectionContext;

	private readonly ILogger _logger;

	private readonly CancellationTokenSource _connectionAbortedTokenSource = new CancellationTokenSource();

	private readonly TaskCompletionSource<object> _abortCompletedTcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

	private readonly long _keepAliveDuration;

	private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1);

	private long _lastSendTimestamp = Stopwatch.GetTimestamp();

	private ReadOnlyMemory<byte> _cachedPingMessage;

	public virtual CancellationToken ConnectionAborted { get; }

	public virtual string ConnectionId => _connectionContext.ConnectionId;

	public virtual ClaimsPrincipal User => Features.Get<IConnectionUserFeature>()?.User;

	public virtual IFeatureCollection Features => _connectionContext.Features;

	public virtual IDictionary<object, object> Items => _connectionContext.Items;

	internal PipeReader Input => _connectionContext.Transport.Input;

	public string UserIdentifier { get; set; }

	public virtual IHubProtocol Protocol { get; internal set; }

	internal ExceptionDispatchInfo AbortException { get; private set; }

	internal ConcurrentDictionary<string, CancellationTokenSource> ActiveRequestCancellationSources { get; } = new ConcurrentDictionary<string, CancellationTokenSource>(StringComparer.Ordinal);


	public HubConnectionContext(ConnectionContext connectionContext, TimeSpan keepAliveInterval, ILoggerFactory loggerFactory)
	{
		_connectionContext = connectionContext;
		_logger = loggerFactory.CreateLogger<HubConnectionContext>();
		ConnectionAborted = _connectionAbortedTokenSource.Token;
		_keepAliveDuration = (int)keepAliveInterval.TotalMilliseconds * (Stopwatch.Frequency / 1000);
	}

	public virtual ValueTask WriteAsync(HubMessage message, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!_writeLock.Wait(0))
		{
			return new ValueTask(WriteSlowAsync(message));
		}
		ValueTask<FlushResult> task = WriteCore(message);
		if (!task.IsCompletedSuccessfully)
		{
			return new ValueTask(CompleteWriteAsync(task));
		}
		_writeLock.Release();
		return default(ValueTask);
	}

	public virtual ValueTask WriteAsync(SerializedHubMessage message, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!_writeLock.Wait(0))
		{
			return new ValueTask(WriteSlowAsync(message));
		}
		ValueTask<FlushResult> task = WriteCore(message);
		if (!task.IsCompletedSuccessfully)
		{
			return new ValueTask(CompleteWriteAsync(task));
		}
		_writeLock.Release();
		return default(ValueTask);
	}

	private ValueTask<FlushResult> WriteCore(HubMessage message)
	{
		try
		{
			Protocol.WriteMessage(message, _connectionContext.Transport.Output);
			return _connectionContext.Transport.Output.FlushAsync();
		}
		catch (Exception exception)
		{
			Log.FailedWritingMessage(_logger, exception);
			return new ValueTask<FlushResult>(new FlushResult(isCanceled: false, isCompleted: true));
		}
	}

	private ValueTask<FlushResult> WriteCore(SerializedHubMessage message)
	{
		try
		{
			ReadOnlyMemory<byte> serializedMessage = message.GetSerializedMessage(Protocol);
			return _connectionContext.Transport.Output.WriteAsync(serializedMessage);
		}
		catch (Exception exception)
		{
			Log.FailedWritingMessage(_logger, exception);
			return new ValueTask<FlushResult>(new FlushResult(isCanceled: false, isCompleted: true));
		}
	}

	private async Task CompleteWriteAsync(ValueTask<FlushResult> task)
	{
		try
		{
			await task;
		}
		catch (Exception exception)
		{
			Log.FailedWritingMessage(_logger, exception);
		}
		finally
		{
			_writeLock.Release();
		}
	}

	private async Task WriteSlowAsync(HubMessage message)
	{
		await _writeLock.WaitAsync();
		try
		{
			await WriteCore(message);
		}
		catch (Exception exception)
		{
			Log.FailedWritingMessage(_logger, exception);
		}
		finally
		{
			_writeLock.Release();
		}
	}

	private async Task WriteSlowAsync(SerializedHubMessage message)
	{
		_ = 1;
		try
		{
			await _writeLock.WaitAsync();
			await WriteCore(message);
		}
		catch (Exception exception)
		{
			Log.FailedWritingMessage(_logger, exception);
		}
		finally
		{
			_writeLock.Release();
		}
	}

	private ValueTask TryWritePingAsync()
	{
		if (!_writeLock.Wait(0))
		{
			return default(ValueTask);
		}
		return new ValueTask(TryWritePingSlowAsync());
	}

	private async Task TryWritePingSlowAsync()
	{
		try
		{
			await _connectionContext.Transport.Output.WriteAsync(_cachedPingMessage);
			Log.SentPing(_logger);
		}
		catch (Exception exception)
		{
			Log.FailedWritingMessage(_logger, exception);
		}
		finally
		{
			_writeLock.Release();
		}
	}

	private async Task WriteHandshakeResponseAsync(HandshakeResponseMessage message)
	{
		await _writeLock.WaitAsync();
		try
		{
			if (message == HandshakeResponseMessage.Empty)
			{
				_connectionContext.Transport.Output.Write(HandshakeProtocol.SuccessHandshakeData.Span);
			}
			else
			{
				HandshakeProtocol.WriteResponseMessage(message, _connectionContext.Transport.Output);
			}
			await _connectionContext.Transport.Output.FlushAsync();
		}
		finally
		{
			_writeLock.Release();
		}
	}

	public virtual void Abort()
	{
		if (!_connectionAbortedTokenSource.IsCancellationRequested)
		{
			Task.Factory.StartNew(_abortedCallback, this);
		}
	}

	internal async Task<bool> HandshakeAsync(TimeSpan timeout, IReadOnlyList<string> supportedProtocols, IHubProtocolResolver protocolResolver, IUserIdProvider userIdProvider, bool enableDetailedErrors)
	{
		bool result = default(bool);
		object obj;
		int num;
		try
		{
			using CancellationTokenSource cts = new CancellationTokenSource();
			if (!Debugger.IsAttached)
			{
				cts.CancelAfter(timeout);
			}
			while (true)
			{
				ReadResult readResult = await _connectionContext.Transport.Input.ReadAsync(cts.Token);
				ReadOnlySequence<byte> buffer = readResult.Buffer;
				SequencePosition consumed = buffer.Start;
				SequencePosition examined = buffer.End;
				try
				{
					if (!buffer.IsEmpty && HandshakeProtocol.TryParseRequestMessage(ref buffer, out HandshakeRequestMessage requestMessage))
					{
						consumed = buffer.Start;
						examined = consumed;
						Protocol = protocolResolver.GetProtocol(requestMessage.Protocol, supportedProtocols);
						if (Protocol == null)
						{
							Log.HandshakeFailed(_logger, null);
							await WriteHandshakeResponseAsync(new HandshakeResponseMessage("The protocol '" + requestMessage.Protocol + "' is not supported."));
							result = false;
							return result;
						}
						if (!Protocol.IsVersionSupported(requestMessage.Version))
						{
							Log.ProtocolVersionFailed(_logger, requestMessage.Protocol, requestMessage.Version);
							await WriteHandshakeResponseAsync(new HandshakeResponseMessage($"The server does not support version {requestMessage.Version} of the '{requestMessage.Protocol}' protocol."));
							result = false;
							return result;
						}
						ITransferFormatFeature transferFormatFeature = Features.Get<ITransferFormatFeature>();
						if (transferFormatFeature != null)
						{
							if ((transferFormatFeature.SupportedFormats & Protocol.TransferFormat) == 0)
							{
								Log.HandshakeFailed(_logger, null);
								await WriteHandshakeResponseAsync(new HandshakeResponseMessage($"Cannot use the '{Protocol.Name}' protocol on the current transport. The transport does not support '{Protocol.TransferFormat}' transfer format."));
								result = false;
								return result;
							}
							transferFormatFeature.ActiveFormat = Protocol.TransferFormat;
						}
						_cachedPingMessage = Protocol.GetMessageBytes(PingMessage.Instance);
						UserIdentifier = userIdProvider.GetUserId(this);
						IConnectionInherentKeepAliveFeature connectionInherentKeepAliveFeature = Features.Get<IConnectionInherentKeepAliveFeature>();
						if (connectionInherentKeepAliveFeature == null || !connectionInherentKeepAliveFeature.HasInherentKeepAlive)
						{
							Features.Get<IConnectionHeartbeatFeature>()?.OnHeartbeat(delegate(object state)
							{
								((HubConnectionContext)state).KeepAliveTick();
							}, this);
						}
						Log.HandshakeComplete(_logger, Protocol.Name);
						await WriteHandshakeResponseAsync(HandshakeResponseMessage.Empty);
						result = true;
						return result;
					}
					if (readResult.IsCompleted)
					{
						Log.HandshakeFailed(_logger, null);
						result = false;
						return result;
					}
				}
				finally
				{
					_connectionContext.Transport.Input.AdvanceTo(consumed, examined);
				}
			}
		}
		catch (OperationCanceledException)
		{
			Log.HandshakeCanceled(_logger);
			await WriteHandshakeResponseAsync(new HandshakeResponseMessage("Handshake was canceled."));
			return false;
		}
		catch (Exception ex2)
		{
			obj = ex2;
			num = 2;
		}
		if (num != 2)
		{
			return result;
		}
		Exception exception = (Exception)obj;
		Log.HandshakeFailed(_logger, exception);
		string error = ErrorMessageHelper.BuildErrorMessage("An unexpected error occurred during connection handshake.", exception, enableDetailedErrors);
		await WriteHandshakeResponseAsync(new HandshakeResponseMessage(error));
		return false;
	}

	internal void Abort(Exception exception)
	{
		AbortException = ExceptionDispatchInfo.Capture(exception);
		Abort();
	}

	internal Task AbortAsync()
	{
		Abort();
		return _abortCompletedTcs.Task;
	}

	private void KeepAliveTick()
	{
		long timestamp = Stopwatch.GetTimestamp();
		if (timestamp - Interlocked.Read(ref _lastSendTimestamp) > _keepAliveDuration)
		{
			TryWritePingAsync();
			Interlocked.Exchange(ref _lastSendTimestamp, timestamp);
		}
	}

	private static void AbortConnection(object state)
	{
		HubConnectionContext hubConnectionContext = (HubConnectionContext)state;
		try
		{
			hubConnectionContext._connectionAbortedTokenSource.Cancel();
			hubConnectionContext._abortCompletedTcs.TrySetResult(null);
		}
		catch (Exception exception)
		{
			hubConnectionContext._abortCompletedTcs.TrySetException(exception);
		}
	}
}
