using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.SignalR.Client.Internal;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.SignalR.Client;

public class HubConnection : IAsyncDisposable
{
	private class Subscription : IDisposable
	{
		private readonly InvocationHandler _handler;

		private readonly InvocationHandlerList _handlerList;

		public Subscription(InvocationHandler handler, InvocationHandlerList handlerList)
		{
			_handler = handler;
			_handlerList = handlerList;
		}

		public void Dispose()
		{
			_handlerList.Remove(_handler);
		}
	}

	private class InvocationHandlerList
	{
		private readonly List<InvocationHandler> _invocationHandlers;

		private InvocationHandler[] _copiedHandlers;

		internal InvocationHandlerList(InvocationHandler handler)
		{
			_invocationHandlers = new List<InvocationHandler> { handler };
		}

		internal InvocationHandler[] GetHandlers()
		{
			InvocationHandler[] copiedHandlers = _copiedHandlers;
			if (copiedHandlers == null)
			{
				lock (_invocationHandlers)
				{
					if (_copiedHandlers == null)
					{
						_copiedHandlers = _invocationHandlers.ToArray();
					}
					copiedHandlers = _copiedHandlers;
				}
			}
			return copiedHandlers;
		}

		internal void Add(InvocationHandler handler)
		{
			lock (_invocationHandlers)
			{
				_invocationHandlers.Add(handler);
				_copiedHandlers = null;
			}
		}

		internal void Remove(InvocationHandler handler)
		{
			lock (_invocationHandlers)
			{
				if (_invocationHandlers.Remove(handler))
				{
					_copiedHandlers = null;
				}
			}
		}
	}

	private readonly struct InvocationHandler
	{
		private readonly Func<object[], object, Task> _callback;

		private readonly object _state;

		public Type[] ParameterTypes { get; }

		public InvocationHandler(Type[] parameterTypes, Func<object[], object, Task> callback, object state)
		{
			_callback = callback;
			ParameterTypes = parameterTypes;
			_state = state;
		}

		public Task InvokeAsync(object[] parameters)
		{
			return _callback(parameters, _state);
		}
	}

	private class ConnectionState : IInvocationBinder
	{
		private readonly HubConnection _hubConnection;

		private readonly ILogger _logger;

		private readonly bool _hasInherentKeepAlive;

		private readonly object _lock = new object();

		private readonly Dictionary<string, InvocationRequest> _pendingCalls = new Dictionary<string, InvocationRequest>(StringComparer.Ordinal);

		private TaskCompletionSource<object> _stopTcs;

		private volatile bool _stopping;

		private int _nextInvocationId;

		private long _nextActivationServerTimeout;

		private long _nextActivationSendPing;

		public ConnectionContext Connection { get; }

		public Task ReceiveTask { get; set; }

		public Exception CloseException { get; set; }

		public CancellationToken UploadStreamToken { get; set; }

		public Task InvocationMessageReceiveTask { get; set; }

		public bool Stopping
		{
			get
			{
				return _stopping;
			}
			set
			{
				_stopping = value;
			}
		}

		public ConnectionState(ConnectionContext connection, HubConnection hubConnection)
		{
			Connection = connection;
			_hubConnection = hubConnection;
			_hubConnection._logScope.ConnectionId = connection.ConnectionId;
			_logger = _hubConnection._logger;
			_hasInherentKeepAlive = connection.Features.Get<IConnectionInherentKeepAliveFeature>()?.HasInherentKeepAlive ?? false;
		}

		public string GetNextId()
		{
			int num = ++_nextInvocationId;
			return num.ToString(CultureInfo.InvariantCulture);
		}

		public void AddInvocation(InvocationRequest irq)
		{
			lock (_lock)
			{
				if (_pendingCalls.ContainsKey(irq.InvocationId))
				{
					Log.InvocationAlreadyInUse(_logger, irq.InvocationId);
					throw new InvalidOperationException("Invocation ID '" + irq.InvocationId + "' is already in use.");
				}
				_pendingCalls.Add(irq.InvocationId, irq);
			}
		}

		public bool TryGetInvocation(string invocationId, [NotNullWhen(true)] out InvocationRequest irq)
		{
			lock (_lock)
			{
				return _pendingCalls.TryGetValue(invocationId, out irq);
			}
		}

		public bool TryRemoveInvocation(string invocationId, [NotNullWhen(true)] out InvocationRequest irq)
		{
			lock (_lock)
			{
				if (_pendingCalls.TryGetValue(invocationId, out irq))
				{
					_pendingCalls.Remove(invocationId);
					return true;
				}
				return false;
			}
		}

		public void CancelOutstandingInvocations(Exception exception)
		{
			Log.CancelingOutstandingInvocations(_logger);
			lock (_lock)
			{
				foreach (InvocationRequest value in _pendingCalls.Values)
				{
					Log.RemovingInvocation(_logger, value.InvocationId);
					if (exception != null)
					{
						value.Fail(exception);
					}
					value.Dispose();
				}
				_pendingCalls.Clear();
			}
		}

		public Task StopAsync()
		{
			lock (_lock)
			{
				if (_stopTcs != null)
				{
					return _stopTcs.Task;
				}
				_stopTcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
				return StopAsyncCore();
			}
		}

		private async Task StopAsyncCore()
		{
			Log.Stopping(_logger);
			Log.TerminatingReceiveLoop(_logger);
			Connection.Transport.Input.CancelPendingRead();
			Log.WaitingForReceiveLoopToTerminate(_logger);
			await (ReceiveTask ?? Task.CompletedTask);
			Log.Stopped(_logger);
			_hubConnection._logScope.ConnectionId = null;
			_stopTcs.TrySetResult(null);
		}

		public async Task TimerLoop(TimerAwaitable timer)
		{
			timer.Start();
			ResetTimeout();
			ResetSendPing();
			using (timer)
			{
				while (await timer)
				{
					await RunTimerActions();
				}
			}
		}

		public void ResetSendPing()
		{
			Volatile.Write(ref _nextActivationSendPing, (DateTime.UtcNow + _hubConnection.KeepAliveInterval).Ticks);
		}

		public void ResetTimeout()
		{
			Volatile.Write(ref _nextActivationServerTimeout, (DateTime.UtcNow + _hubConnection.ServerTimeout).Ticks);
		}

		internal async Task RunTimerActions()
		{
			if (_hasInherentKeepAlive)
			{
				return;
			}
			if (DateTime.UtcNow.Ticks > Volatile.Read(ref _nextActivationServerTimeout))
			{
				OnServerTimeout();
			}
			if (DateTime.UtcNow.Ticks <= Volatile.Read(ref _nextActivationSendPing) || Stopping)
			{
				return;
			}
			if (!_hubConnection._state.TryAcquireConnectionLock())
			{
				Log.UnableToAcquireConnectionLockForPing(_logger);
				return;
			}
			Log.AcquiredConnectionLockForPing(_logger);
			try
			{
				if (_hubConnection._state.CurrentConnectionStateUnsynchronized != null)
				{
					await _hubConnection.SendHubMessage(this, PingMessage.Instance);
				}
			}
			finally
			{
				_hubConnection._state.ReleaseConnectionLock("RunTimerActions", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1881);
			}
		}

		internal void OnServerTimeout()
		{
			CloseException = new TimeoutException($"Server timeout ({_hubConnection.ServerTimeout.TotalMilliseconds:0.00}ms) elapsed without receiving a message from the server.");
			Connection.Transport.Input.CancelPendingRead();
		}

		Type IInvocationBinder.GetReturnType(string invocationId)
		{
			if (!TryGetInvocation(invocationId, out var irq))
			{
				Log.ReceivedUnexpectedResponse(_logger, invocationId);
				throw new KeyNotFoundException("No invocation with id '" + invocationId + "' could be found.");
			}
			return irq.ResultType;
		}

		Type IInvocationBinder.GetStreamItemType(string invocationId)
		{
			if (!TryGetInvocation(invocationId, out var irq))
			{
				Log.ReceivedUnexpectedResponse(_logger, invocationId);
				throw new KeyNotFoundException("No invocation with id '" + invocationId + "' could be found.");
			}
			return irq.ResultType;
		}

		IReadOnlyList<Type> IInvocationBinder.GetParameterTypes(string methodName)
		{
			if (!_hubConnection._handlers.TryGetValue(methodName, out var value))
			{
				Log.MissingHandler(_logger, methodName);
				return Type.EmptyTypes;
			}
			InvocationHandler[] handlers = value.GetHandlers();
			if (handlers.Length != 0)
			{
				return handlers[0].ParameterTypes;
			}
			throw new InvalidOperationException("There are no callbacks registered for the method '" + methodName + "'");
		}
	}

	private class ReconnectingConnectionState
	{
		private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

		private readonly ILogger _logger;

		public ConnectionState CurrentConnectionStateUnsynchronized { get; set; }

		public HubConnectionState OverallState { get; private set; }

		public CancellationTokenSource StopCts { get; set; } = new CancellationTokenSource();


		public Task ReconnectTask { get; set; } = Task.CompletedTask;


		public ReconnectingConnectionState(ILogger logger)
		{
			_logger = logger;
			StopCts = new CancellationTokenSource();
			ReconnectTask = Task.CompletedTask;
		}

		public void ChangeState(HubConnectionState expectedState, HubConnectionState newState)
		{
			if (!TryChangeState(expectedState, newState))
			{
				Log.StateTransitionFailed(_logger, expectedState, newState, OverallState);
				throw new InvalidOperationException($"The HubConnection failed to transition from the '{expectedState}' state to the '{newState}' state because it was actually in the '{OverallState}' state.");
			}
		}

		public bool TryChangeState(HubConnectionState expectedState, HubConnectionState newState)
		{
			Log.AttemptingStateTransition(_logger, expectedState, newState);
			if (OverallState != expectedState)
			{
				return false;
			}
			OverallState = newState;
			return true;
		}

		[Conditional("DEBUG")]
		public void AssertInConnectionLock([CallerMemberName] string memberName = null, [CallerFilePath] string fileName = null, [CallerLineNumber] int lineNumber = 0)
		{
		}

		[Conditional("DEBUG")]
		public void AssertConnectionValid([CallerMemberName] string memberName = null, [CallerFilePath] string fileName = null, [CallerLineNumber] int lineNumber = 0)
		{
		}

		public Task WaitConnectionLockAsync(CancellationToken token, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
		{
			Log.WaitingOnConnectionLock(_logger, memberName, filePath, lineNumber);
			return _connectionLock.WaitAsync(token);
		}

		public bool TryAcquireConnectionLock()
		{
			if (OperatingSystem.IsBrowser())
			{
				return _connectionLock.WaitAsync(0).Result;
			}
			return _connectionLock.Wait(0);
		}

		public async Task<ConnectionState> WaitForActiveConnectionAsync(string methodName, CancellationToken token, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
		{
			await WaitConnectionLockAsync(token, methodName, "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 2008);
			if (CurrentConnectionStateUnsynchronized == null || CurrentConnectionStateUnsynchronized.Stopping)
			{
				ReleaseConnectionLock(methodName, "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 2012);
				throw new InvalidOperationException("The '" + methodName + "' method cannot be called if the connection is not active");
			}
			return CurrentConnectionStateUnsynchronized;
		}

		public void ReleaseConnectionLock([CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
		{
			Log.ReleasingConnectionLock(_logger, memberName, filePath, lineNumber);
			_connectionLock.Release();
		}
	}

	private static class Log
	{
		private static readonly LogDefineOptions SkipEnabledCheckLogOptions = new LogDefineOptions
		{
			SkipEnabledCheck = true
		};

		private static readonly Action<ILogger, string, int, Exception> _preparingNonBlockingInvocation = LoggerMessage.Define<string, int>(LogLevel.Trace, new EventId(1, "PreparingNonBlockingInvocation"), "Preparing non-blocking invocation of '{Target}', with {ArgumentCount} argument(s).");

		private static readonly Action<ILogger, string, string, string, int, Exception> _preparingBlockingInvocation = LoggerMessage.Define<string, string, string, int>(LogLevel.Trace, new EventId(2, "PreparingBlockingInvocation"), "Preparing blocking invocation '{InvocationId}' of '{Target}', with return type '{ReturnType}' and {ArgumentCount} argument(s).");

		private static readonly Action<ILogger, string, Exception> _registeringInvocation = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(3, "RegisteringInvocation"), "Registering Invocation ID '{InvocationId}' for tracking.");

		private static readonly Action<ILogger, string, string, string, string, Exception> _issuingInvocation = LoggerMessage.Define<string, string, string, string>(LogLevel.Trace, new EventId(4, "IssuingInvocation"), "Issuing Invocation '{InvocationId}': {ReturnType} {MethodName}({Args}).", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, string, string, Exception> _sendingMessage = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(5, "SendingMessage"), "Sending {MessageType} message '{InvocationId}'.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, string, string, Exception> _messageSent = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(6, "MessageSent"), "Sending {MessageType} message '{InvocationId}' completed.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, string, Exception> _failedToSendInvocation = LoggerMessage.Define<string>(LogLevel.Error, new EventId(7, "FailedToSendInvocation"), "Sending Invocation '{InvocationId}' failed.");

		private static readonly Action<ILogger, string, string, string, Exception> _receivedInvocation = LoggerMessage.Define<string, string, string>(LogLevel.Trace, new EventId(8, "ReceivedInvocation"), "Received Invocation '{InvocationId}': {MethodName}({Args}).", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, string, Exception> _droppedCompletionMessage = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(9, "DroppedCompletionMessage"), "Dropped unsolicited Completion message for invocation '{InvocationId}'.");

		private static readonly Action<ILogger, string, Exception> _droppedStreamMessage = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(10, "DroppedStreamMessage"), "Dropped unsolicited StreamItem message for invocation '{InvocationId}'.");

		private static readonly Action<ILogger, Exception> _shutdownConnection = LoggerMessage.Define(LogLevel.Trace, new EventId(11, "ShutdownConnection"), "Shutting down connection.");

		private static readonly Action<ILogger, Exception> _shutdownWithError = LoggerMessage.Define(LogLevel.Error, new EventId(12, "ShutdownWithError"), "Connection is shutting down due to an error.");

		private static readonly Action<ILogger, string, Exception> _removingInvocation = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(13, "RemovingInvocation"), "Removing pending invocation {InvocationId}.");

		private static readonly Action<ILogger, string, Exception> _missingHandler = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(14, "MissingHandler"), "Failed to find handler for '{Target}' method.");

		private static readonly Action<ILogger, string, Exception> _receivedStreamItem = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(15, "ReceivedStreamItem"), "Received StreamItem for Invocation {InvocationId}.");

		private static readonly Action<ILogger, string, Exception> _cancelingStreamItem = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(16, "CancelingStreamItem"), "Canceling dispatch of StreamItem message for Invocation {InvocationId}. The invocation was canceled.");

		private static readonly Action<ILogger, string, Exception> _receivedStreamItemAfterClose = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(17, "ReceivedStreamItemAfterClose"), "Invocation {InvocationId} received stream item after channel was closed.");

		private static readonly Action<ILogger, string, Exception> _receivedInvocationCompletion = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(18, "ReceivedInvocationCompletion"), "Received Completion for Invocation {InvocationId}.");

		private static readonly Action<ILogger, string, Exception> _cancelingInvocationCompletion = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(19, "CancelingInvocationCompletion"), "Canceling dispatch of Completion message for Invocation {InvocationId}. The invocation was canceled.");

		private static readonly Action<ILogger, string, string, int, Exception> _releasingConnectionLock = LoggerMessage.Define<string, string, int>(LogLevel.Trace, new EventId(20, "ReleasingConnectionLock"), "Releasing Connection Lock in {MethodName} ({FilePath}:{LineNumber}).");

		private static readonly Action<ILogger, Exception> _stopped = LoggerMessage.Define(LogLevel.Debug, new EventId(21, "Stopped"), "HubConnection stopped.");

		private static readonly Action<ILogger, string, Exception> _invocationAlreadyInUse = LoggerMessage.Define<string>(LogLevel.Critical, new EventId(22, "InvocationAlreadyInUse"), "Invocation ID '{InvocationId}' is already in use.");

		private static readonly Action<ILogger, string, Exception> _receivedUnexpectedResponse = LoggerMessage.Define<string>(LogLevel.Error, new EventId(23, "ReceivedUnexpectedResponse"), "Unsolicited response received for invocation '{InvocationId}'.");

		private static readonly Action<ILogger, string, int, Exception> _hubProtocol = LoggerMessage.Define<string, int>(LogLevel.Information, new EventId(24, "HubProtocol"), "Using HubProtocol '{Protocol} v{Version}'.");

		private static readonly Action<ILogger, string, string, string, int, Exception> _preparingStreamingInvocation = LoggerMessage.Define<string, string, string, int>(LogLevel.Trace, new EventId(25, "PreparingStreamingInvocation"), "Preparing streaming invocation '{InvocationId}' of '{Target}', with return type '{ReturnType}' and {ArgumentCount} argument(s).");

		private static readonly Action<ILogger, Exception> _resettingKeepAliveTimer = LoggerMessage.Define(LogLevel.Trace, new EventId(26, "ResettingKeepAliveTimer"), "Resetting keep-alive timer, received a message from the server.");

		private static readonly Action<ILogger, Exception> _errorDuringClosedEvent = LoggerMessage.Define(LogLevel.Error, new EventId(27, "ErrorDuringClosedEvent"), "An exception was thrown in the handler for the Closed event.");

		private static readonly Action<ILogger, Exception> _sendingHubHandshake = LoggerMessage.Define(LogLevel.Debug, new EventId(28, "SendingHubHandshake"), "Sending Hub Handshake.");

		private static readonly Action<ILogger, Exception> _receivedPing = LoggerMessage.Define(LogLevel.Trace, new EventId(31, "ReceivedPing"), "Received a ping message.");

		private static readonly Action<ILogger, string, Exception> _errorInvokingClientSideMethod = LoggerMessage.Define<string>(LogLevel.Error, new EventId(34, "ErrorInvokingClientSideMethod"), "Invoking client side method '{MethodName}' failed.");

		private static readonly Action<ILogger, Exception> _errorProcessingHandshakeResponse = LoggerMessage.Define(LogLevel.Error, new EventId(35, "ErrorReceivingHandshakeResponse"), "The underlying connection closed while processing the handshake response. See exception for details.");

		private static readonly Action<ILogger, string, Exception> _handshakeServerError = LoggerMessage.Define<string>(LogLevel.Error, new EventId(36, "HandshakeServerError"), "Server returned handshake error: {Error}");

		private static readonly Action<ILogger, Exception> _receivedClose = LoggerMessage.Define(LogLevel.Debug, new EventId(37, "ReceivedClose"), "Received close message.");

		private static readonly Action<ILogger, string, Exception> _receivedCloseWithError = LoggerMessage.Define<string>(LogLevel.Error, new EventId(38, "ReceivedCloseWithError"), "Received close message with an error: {Error}");

		private static readonly Action<ILogger, Exception> _handshakeComplete = LoggerMessage.Define(LogLevel.Debug, new EventId(39, "HandshakeComplete"), "Handshake with server complete.");

		private static readonly Action<ILogger, string, Exception> _registeringHandler = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(40, "RegisteringHandler"), "Registering handler for client method '{MethodName}'.");

		private static readonly Action<ILogger, Exception> _starting = LoggerMessage.Define(LogLevel.Debug, new EventId(41, "Starting"), "Starting HubConnection.");

		private static readonly Action<ILogger, string, string, int, Exception> _waitingOnConnectionLock = LoggerMessage.Define<string, string, int>(LogLevel.Trace, new EventId(42, "WaitingOnConnectionLock"), "Waiting on Connection Lock in {MethodName} ({FilePath}:{LineNumber}).");

		private static readonly Action<ILogger, Exception> _errorStartingConnection = LoggerMessage.Define(LogLevel.Error, new EventId(43, "ErrorStartingConnection"), "Error starting connection.");

		private static readonly Action<ILogger, Exception> _started = LoggerMessage.Define(LogLevel.Information, new EventId(44, "Started"), "HubConnection started.");

		private static readonly Action<ILogger, string, Exception> _sendingCancellation = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(45, "SendingCancellation"), "Sending Cancellation for Invocation '{InvocationId}'.");

		private static readonly Action<ILogger, Exception> _cancelingOutstandingInvocations = LoggerMessage.Define(LogLevel.Debug, new EventId(46, "CancelingOutstandingInvocations"), "Canceling all outstanding invocations.");

		private static readonly Action<ILogger, Exception> _receiveLoopStarting = LoggerMessage.Define(LogLevel.Debug, new EventId(47, "ReceiveLoopStarting"), "Receive loop starting.");

		private static readonly Action<ILogger, double, Exception> _startingServerTimeoutTimer = LoggerMessage.Define<double>(LogLevel.Debug, new EventId(48, "StartingServerTimeoutTimer"), "Starting server timeout timer. Duration: {ServerTimeout:0.00}ms", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, Exception> _notUsingServerTimeout = LoggerMessage.Define(LogLevel.Debug, new EventId(49, "NotUsingServerTimeout"), "Not using server timeout because the transport inherently tracks server availability.");

		private static readonly Action<ILogger, Exception> _serverDisconnectedWithError = LoggerMessage.Define(LogLevel.Error, new EventId(50, "ServerDisconnectedWithError"), "The server connection was terminated with an error.");

		private static readonly Action<ILogger, Exception> _invokingClosedEventHandler = LoggerMessage.Define(LogLevel.Debug, new EventId(51, "InvokingClosedEventHandler"), "Invoking the Closed event handler.");

		private static readonly Action<ILogger, Exception> _stopping = LoggerMessage.Define(LogLevel.Debug, new EventId(52, "Stopping"), "Stopping HubConnection.");

		private static readonly Action<ILogger, Exception> _terminatingReceiveLoop = LoggerMessage.Define(LogLevel.Debug, new EventId(53, "TerminatingReceiveLoop"), "Terminating receive loop.");

		private static readonly Action<ILogger, Exception> _waitingForReceiveLoopToTerminate = LoggerMessage.Define(LogLevel.Debug, new EventId(54, "WaitingForReceiveLoopToTerminate"), "Waiting for the receive loop to terminate.");

		private static readonly Action<ILogger, string, Exception> _unableToSendCancellation = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(55, "UnableToSendCancellation"), "Unable to send cancellation for invocation '{InvocationId}'. The connection is inactive.");

		private static readonly Action<ILogger, long, Exception> _processingMessage = LoggerMessage.Define<long>(LogLevel.Debug, new EventId(56, "ProcessingMessage"), "Processing {MessageLength} byte message from server.");

		private static readonly Action<ILogger, string, string, Exception> _argumentBindingFailure = LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(57, "ArgumentBindingFailure"), "Failed to bind arguments received in invocation '{InvocationId}' of '{MethodName}'.");

		private static readonly Action<ILogger, string, Exception> _removingHandlers = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(58, "RemovingHandlers"), "Removing handlers for client method '{MethodName}'.");

		private static readonly Action<ILogger, string, Exception> _sendingMessageGeneric = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(59, "SendingMessageGeneric"), "Sending {MessageType} message.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, string, Exception> _messageSentGeneric = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(60, "MessageSentGeneric"), "Sending {MessageType} message completed.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, Exception> _acquiredConnectionLockForPing = LoggerMessage.Define(LogLevel.Trace, new EventId(61, "AcquiredConnectionLockForPing"), "Acquired the Connection Lock in order to ping the server.");

		private static readonly Action<ILogger, Exception> _unableToAcquireConnectionLockForPing = LoggerMessage.Define(LogLevel.Trace, new EventId(62, "UnableToAcquireConnectionLockForPing"), "Skipping ping because a send is already in progress.");

		private static readonly Action<ILogger, string, Exception> _startingStream = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(63, "StartingStream"), "Initiating stream '{StreamId}'.");

		private static readonly Action<ILogger, string, Exception> _sendingStreamItem = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(64, "StreamItemSent"), "Sending item for stream '{StreamId}'.");

		private static readonly Action<ILogger, string, Exception> _cancelingStream = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(65, "CancelingStream"), "Stream '{StreamId}' has been canceled by client.");

		private static readonly Action<ILogger, string, Exception> _completingStream = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(66, "CompletingStream"), "Sending completion message for stream '{StreamId}'.");

		private static readonly Action<ILogger, HubConnectionState, HubConnectionState, HubConnectionState, Exception> _stateTransitionFailed = LoggerMessage.Define<HubConnectionState, HubConnectionState, HubConnectionState>(LogLevel.Error, new EventId(67, "StateTransitionFailed"), "The HubConnection failed to transition from the {ExpectedState} state to the {NewState} state because it was actually in the {ActualState} state.");

		private static readonly Action<ILogger, Exception> _reconnecting = LoggerMessage.Define(LogLevel.Information, new EventId(68, "Reconnecting"), "HubConnection reconnecting.");

		private static readonly Action<ILogger, Exception> _reconnectingWithError = LoggerMessage.Define(LogLevel.Error, new EventId(69, "ReconnectingWithError"), "HubConnection reconnecting due to an error.");

		private static readonly Action<ILogger, long, TimeSpan, Exception> _reconnected = LoggerMessage.Define<long, TimeSpan>(LogLevel.Information, new EventId(70, "Reconnected"), "HubConnection reconnected successfully after {ReconnectAttempts} attempts and {ElapsedTime} elapsed.");

		private static readonly Action<ILogger, long, TimeSpan, Exception> _reconnectAttemptsExhausted = LoggerMessage.Define<long, TimeSpan>(LogLevel.Information, new EventId(71, "ReconnectAttemptsExhausted"), "Reconnect retries have been exhausted after {ReconnectAttempts} failed attempts and {ElapsedTime} elapsed. Disconnecting.");

		private static readonly Action<ILogger, long, TimeSpan, Exception> _awaitingReconnectRetryDelay = LoggerMessage.Define<long, TimeSpan>(LogLevel.Trace, new EventId(72, "AwaitingReconnectRetryDelay"), "Reconnect attempt number {ReconnectAttempts} will start in {RetryDelay}.");

		private static readonly Action<ILogger, Exception> _reconnectAttemptFailed = LoggerMessage.Define(LogLevel.Trace, new EventId(73, "ReconnectAttemptFailed"), "Reconnect attempt failed.");

		private static readonly Action<ILogger, Exception> _errorDuringReconnectingEvent = LoggerMessage.Define(LogLevel.Error, new EventId(74, "ErrorDuringReconnectingEvent"), "An exception was thrown in the handler for the Reconnecting event.");

		private static readonly Action<ILogger, Exception> _errorDuringReconnectedEvent = LoggerMessage.Define(LogLevel.Error, new EventId(75, "ErrorDuringReconnectedEvent"), "An exception was thrown in the handler for the Reconnected event.");

		private static readonly Action<ILogger, Exception> _errorDuringNextRetryDelay = LoggerMessage.Define(LogLevel.Error, new EventId(76, "ErrorDuringNextRetryDelay"), "An exception was thrown from IRetryPolicy.NextRetryDelay().");

		private static readonly Action<ILogger, Exception> _firstReconnectRetryDelayNull = LoggerMessage.Define(LogLevel.Warning, new EventId(77, "FirstReconnectRetryDelayNull"), "Connection not reconnecting because the IRetryPolicy returned null on the first reconnect attempt.");

		private static readonly Action<ILogger, Exception> _reconnectingStoppedDuringRetryDelay = LoggerMessage.Define(LogLevel.Trace, new EventId(78, "ReconnectingStoppedDueToStateChangeDuringRetryDelay"), "Connection stopped during reconnect delay. Done reconnecting.");

		private static readonly Action<ILogger, Exception> _reconnectingStoppedDuringReconnectAttempt = LoggerMessage.Define(LogLevel.Trace, new EventId(79, "ReconnectingStoppedDueToStateChangeDuringReconnectAttempt"), "Connection stopped during reconnect attempt. Done reconnecting.");

		private static readonly Action<ILogger, HubConnectionState, HubConnectionState, Exception> _attemptingStateTransition = LoggerMessage.Define<HubConnectionState, HubConnectionState>(LogLevel.Trace, new EventId(80, "AttemptingStateTransition"), "The HubConnection is attempting to transition from the {ExpectedState} state to the {NewState} state.");

		private static readonly Action<ILogger, Exception> _errorInvalidHandshakeResponse = LoggerMessage.Define(LogLevel.Error, new EventId(81, "ErrorInvalidHandshakeResponse"), "Received an invalid handshake response.");

		private static readonly Action<ILogger, double, Exception> _errorHandshakeTimedOut = LoggerMessage.Define<double>(LogLevel.Error, new EventId(82, "ErrorHandshakeTimedOut"), "The handshake timed out after {HandshakeTimeoutSeconds} seconds.");

		private static readonly Action<ILogger, Exception> _errorHandshakeCanceled = LoggerMessage.Define(LogLevel.Error, new EventId(83, "ErrorHandshakeCanceled"), "The handshake was canceled by the client.");

		private static readonly Action<ILogger, string, Exception> _erroredStream = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(84, "ErroredStream"), "Client threw an error for stream '{StreamId}'.");

		public static void PreparingNonBlockingInvocation(ILogger logger, string target, int count)
		{
			_preparingNonBlockingInvocation(logger, target, count, null);
		}

		public static void PreparingBlockingInvocation(ILogger logger, string invocationId, string target, string returnType, int count)
		{
			_preparingBlockingInvocation(logger, invocationId, target, returnType, count, null);
		}

		public static void PreparingStreamingInvocation(ILogger logger, string invocationId, string target, string returnType, int count)
		{
			_preparingStreamingInvocation(logger, invocationId, target, returnType, count, null);
		}

		public static void RegisteringInvocation(ILogger logger, string invocationId)
		{
			_registeringInvocation(logger, invocationId, null);
		}

		public static void IssuingInvocation(ILogger logger, string invocationId, string returnType, string methodName, object[] args)
		{
			if (logger.IsEnabled(LogLevel.Trace))
			{
				string arg = ((args == null) ? string.Empty : string.Join(", ", args.Select((object a) => a?.GetType().FullName ?? "(null)")));
				_issuingInvocation(logger, invocationId, returnType, methodName, arg, null);
			}
		}

		public static void SendingMessage(ILogger logger, HubMessage message)
		{
			if (logger.IsEnabled(LogLevel.Debug))
			{
				if (message is HubInvocationMessage hubInvocationMessage)
				{
					_sendingMessage(logger, message.GetType().Name, hubInvocationMessage.InvocationId, null);
				}
				else
				{
					_sendingMessageGeneric(logger, message.GetType().Name, null);
				}
			}
		}

		public static void MessageSent(ILogger logger, HubMessage message)
		{
			if (logger.IsEnabled(LogLevel.Debug))
			{
				if (message is HubInvocationMessage hubInvocationMessage)
				{
					_messageSent(logger, message.GetType().Name, hubInvocationMessage.InvocationId, null);
				}
				else
				{
					_messageSentGeneric(logger, message.GetType().Name, null);
				}
			}
		}

		public static void FailedToSendInvocation(ILogger logger, string invocationId, Exception exception)
		{
			_failedToSendInvocation(logger, invocationId, exception);
		}

		public static void ReceivedInvocation(ILogger logger, string invocationId, string methodName, object[] args)
		{
			if (logger.IsEnabled(LogLevel.Trace))
			{
				string arg = ((args == null) ? string.Empty : string.Join(", ", args.Select((object a) => a?.GetType().FullName ?? "(null)")));
				_receivedInvocation(logger, invocationId, methodName, arg, null);
			}
		}

		public static void DroppedCompletionMessage(ILogger logger, string invocationId)
		{
			_droppedCompletionMessage(logger, invocationId, null);
		}

		public static void DroppedStreamMessage(ILogger logger, string invocationId)
		{
			_droppedStreamMessage(logger, invocationId, null);
		}

		public static void ShutdownConnection(ILogger logger)
		{
			_shutdownConnection(logger, null);
		}

		public static void ShutdownWithError(ILogger logger, Exception exception)
		{
			_shutdownWithError(logger, exception);
		}

		public static void RemovingInvocation(ILogger logger, string invocationId)
		{
			_removingInvocation(logger, invocationId, null);
		}

		public static void MissingHandler(ILogger logger, string target)
		{
			_missingHandler(logger, target, null);
		}

		public static void ReceivedStreamItem(ILogger logger, string invocationId)
		{
			_receivedStreamItem(logger, invocationId, null);
		}

		public static void CancelingStreamItem(ILogger logger, string invocationId)
		{
			_cancelingStreamItem(logger, invocationId, null);
		}

		public static void ReceivedStreamItemAfterClose(ILogger logger, string invocationId)
		{
			_receivedStreamItemAfterClose(logger, invocationId, null);
		}

		public static void ReceivedInvocationCompletion(ILogger logger, string invocationId)
		{
			_receivedInvocationCompletion(logger, invocationId, null);
		}

		public static void CancelingInvocationCompletion(ILogger logger, string invocationId)
		{
			_cancelingInvocationCompletion(logger, invocationId, null);
		}

		public static void Stopped(ILogger logger)
		{
			_stopped(logger, null);
		}

		public static void InvocationAlreadyInUse(ILogger logger, string invocationId)
		{
			_invocationAlreadyInUse(logger, invocationId, null);
		}

		public static void ReceivedUnexpectedResponse(ILogger logger, string invocationId)
		{
			_receivedUnexpectedResponse(logger, invocationId, null);
		}

		public static void HubProtocol(ILogger logger, string hubProtocol, int version)
		{
			_hubProtocol(logger, hubProtocol, version, null);
		}

		public static void ResettingKeepAliveTimer(ILogger logger)
		{
			_resettingKeepAliveTimer(logger, null);
		}

		public static void ErrorDuringClosedEvent(ILogger logger, Exception exception)
		{
			_errorDuringClosedEvent(logger, exception);
		}

		public static void SendingHubHandshake(ILogger logger)
		{
			_sendingHubHandshake(logger, null);
		}

		public static void ReceivedPing(ILogger logger)
		{
			_receivedPing(logger, null);
		}

		public static void ErrorInvokingClientSideMethod(ILogger logger, string methodName, Exception exception)
		{
			_errorInvokingClientSideMethod(logger, methodName, exception);
		}

		public static void ErrorReceivingHandshakeResponse(ILogger logger, Exception exception)
		{
			_errorProcessingHandshakeResponse(logger, exception);
		}

		public static void HandshakeServerError(ILogger logger, string error)
		{
			_handshakeServerError(logger, error, null);
		}

		public static void ReceivedClose(ILogger logger)
		{
			_receivedClose(logger, null);
		}

		public static void ReceivedCloseWithError(ILogger logger, string error)
		{
			_receivedCloseWithError(logger, error, null);
		}

		public static void HandshakeComplete(ILogger logger)
		{
			_handshakeComplete(logger, null);
		}

		public static void RegisteringHandler(ILogger logger, string methodName)
		{
			_registeringHandler(logger, methodName, null);
		}

		public static void RemovingHandlers(ILogger logger, string methodName)
		{
			_removingHandlers(logger, methodName, null);
		}

		public static void Starting(ILogger logger)
		{
			_starting(logger, null);
		}

		public static void ErrorStartingConnection(ILogger logger, Exception ex)
		{
			_errorStartingConnection(logger, ex);
		}

		public static void Started(ILogger logger)
		{
			_started(logger, null);
		}

		public static void SendingCancellation(ILogger logger, string invocationId)
		{
			_sendingCancellation(logger, invocationId, null);
		}

		public static void CancelingOutstandingInvocations(ILogger logger)
		{
			_cancelingOutstandingInvocations(logger, null);
		}

		public static void ReceiveLoopStarting(ILogger logger)
		{
			_receiveLoopStarting(logger, null);
		}

		public static void StartingServerTimeoutTimer(ILogger logger, TimeSpan serverTimeout)
		{
			if (logger.IsEnabled(LogLevel.Debug))
			{
				_startingServerTimeoutTimer(logger, serverTimeout.TotalMilliseconds, null);
			}
		}

		public static void NotUsingServerTimeout(ILogger logger)
		{
			_notUsingServerTimeout(logger, null);
		}

		public static void ServerDisconnectedWithError(ILogger logger, Exception ex)
		{
			_serverDisconnectedWithError(logger, ex);
		}

		public static void InvokingClosedEventHandler(ILogger logger)
		{
			_invokingClosedEventHandler(logger, null);
		}

		public static void Stopping(ILogger logger)
		{
			_stopping(logger, null);
		}

		public static void TerminatingReceiveLoop(ILogger logger)
		{
			_terminatingReceiveLoop(logger, null);
		}

		public static void WaitingForReceiveLoopToTerminate(ILogger logger)
		{
			_waitingForReceiveLoopToTerminate(logger, null);
		}

		public static void ProcessingMessage(ILogger logger, long length)
		{
			_processingMessage(logger, length, null);
		}

		public static void WaitingOnConnectionLock(ILogger logger, string memberName, string filePath, int lineNumber)
		{
			_waitingOnConnectionLock(logger, memberName, filePath, lineNumber, null);
		}

		public static void ReleasingConnectionLock(ILogger logger, string memberName, string filePath, int lineNumber)
		{
			_releasingConnectionLock(logger, memberName, filePath, lineNumber, null);
		}

		public static void UnableToSendCancellation(ILogger logger, string invocationId)
		{
			_unableToSendCancellation(logger, invocationId, null);
		}

		public static void ArgumentBindingFailure(ILogger logger, string invocationId, string target, Exception exception)
		{
			_argumentBindingFailure(logger, invocationId, target, exception);
		}

		public static void AcquiredConnectionLockForPing(ILogger logger)
		{
			_acquiredConnectionLockForPing(logger, null);
		}

		public static void UnableToAcquireConnectionLockForPing(ILogger logger)
		{
			_unableToAcquireConnectionLockForPing(logger, null);
		}

		public static void StartingStream(ILogger logger, string streamId)
		{
			_startingStream(logger, streamId, null);
		}

		public static void SendingStreamItem(ILogger logger, string streamId)
		{
			_sendingStreamItem(logger, streamId, null);
		}

		public static void CancelingStream(ILogger logger, string streamId)
		{
			_cancelingStream(logger, streamId, null);
		}

		public static void CompletingStream(ILogger logger, string streamId)
		{
			_completingStream(logger, streamId, null);
		}

		public static void StateTransitionFailed(ILogger logger, HubConnectionState expectedState, HubConnectionState newState, HubConnectionState actualState)
		{
			_stateTransitionFailed(logger, expectedState, newState, actualState, null);
		}

		public static void Reconnecting(ILogger logger)
		{
			_reconnecting(logger, null);
		}

		public static void ReconnectingWithError(ILogger logger, Exception exception)
		{
			_reconnectingWithError(logger, exception);
		}

		public static void Reconnected(ILogger logger, long reconnectAttempts, TimeSpan elapsedTime)
		{
			_reconnected(logger, reconnectAttempts, elapsedTime, null);
		}

		public static void ReconnectAttemptsExhausted(ILogger logger, long reconnectAttempts, TimeSpan elapsedTime)
		{
			_reconnectAttemptsExhausted(logger, reconnectAttempts, elapsedTime, null);
		}

		public static void AwaitingReconnectRetryDelay(ILogger logger, long reconnectAttempts, TimeSpan retryDelay)
		{
			_awaitingReconnectRetryDelay(logger, reconnectAttempts, retryDelay, null);
		}

		public static void ReconnectAttemptFailed(ILogger logger, Exception exception)
		{
			_reconnectAttemptFailed(logger, exception);
		}

		public static void ErrorDuringReconnectingEvent(ILogger logger, Exception exception)
		{
			_errorDuringReconnectingEvent(logger, exception);
		}

		public static void ErrorDuringReconnectedEvent(ILogger logger, Exception exception)
		{
			_errorDuringReconnectedEvent(logger, exception);
		}

		public static void ErrorDuringNextRetryDelay(ILogger logger, Exception exception)
		{
			_errorDuringNextRetryDelay(logger, exception);
		}

		public static void FirstReconnectRetryDelayNull(ILogger logger)
		{
			_firstReconnectRetryDelayNull(logger, null);
		}

		public static void ReconnectingStoppedDuringRetryDelay(ILogger logger)
		{
			_reconnectingStoppedDuringRetryDelay(logger, null);
		}

		public static void ReconnectingStoppedDuringReconnectAttempt(ILogger logger)
		{
			_reconnectingStoppedDuringReconnectAttempt(logger, null);
		}

		public static void AttemptingStateTransition(ILogger logger, HubConnectionState expectedState, HubConnectionState newState)
		{
			_attemptingStateTransition(logger, expectedState, newState, null);
		}

		public static void ErrorInvalidHandshakeResponse(ILogger logger, Exception exception)
		{
			_errorInvalidHandshakeResponse(logger, exception);
		}

		public static void ErrorHandshakeTimedOut(ILogger logger, TimeSpan handshakeTimeout, Exception exception)
		{
			_errorHandshakeTimedOut(logger, handshakeTimeout.TotalSeconds, exception);
		}

		public static void ErrorHandshakeCanceled(ILogger logger, Exception exception)
		{
			_errorHandshakeCanceled(logger, exception);
		}

		public static void ErroredStream(ILogger logger, string streamId, Exception exception)
		{
			_erroredStream(logger, streamId, exception);
		}
	}

	public static readonly TimeSpan DefaultServerTimeout = TimeSpan.FromSeconds(30.0);

	public static readonly TimeSpan DefaultHandshakeTimeout = TimeSpan.FromSeconds(15.0);

	public static readonly TimeSpan DefaultKeepAliveInterval = TimeSpan.FromSeconds(15.0);

	private static readonly UnboundedChannelOptions _receiveLoopOptions = new UnboundedChannelOptions
	{
		SingleReader = true,
		SingleWriter = true
	};

	private static readonly MethodInfo _sendStreamItemsMethod = typeof(HubConnection).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Single((MethodInfo m) => m.Name.Equals("SendStreamItems"));

	private static readonly MethodInfo _sendIAsyncStreamItemsMethod = typeof(HubConnection).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Single((MethodInfo m) => m.Name.Equals("SendIAsyncEnumerableStreamItems"));

	private readonly ILoggerFactory _loggerFactory;

	private readonly ILogger _logger;

	private readonly ConnectionLogScope _logScope;

	private readonly IHubProtocol _protocol;

	private readonly IServiceProvider _serviceProvider;

	private readonly IConnectionFactory _connectionFactory;

	private readonly IRetryPolicy _reconnectPolicy;

	private readonly EndPoint _endPoint;

	private readonly ConcurrentDictionary<string, InvocationHandlerList> _handlers = new ConcurrentDictionary<string, InvocationHandlerList>(StringComparer.Ordinal);

	private readonly ReconnectingConnectionState _state;

	private bool _disposed;

	internal TimeSpan TickRate { get; set; } = TimeSpan.FromSeconds(1.0);


	public TimeSpan ServerTimeout { get; set; } = DefaultServerTimeout;


	public TimeSpan KeepAliveInterval { get; set; } = DefaultKeepAliveInterval;


	public TimeSpan HandshakeTimeout { get; set; } = DefaultHandshakeTimeout;


	public string? ConnectionId => _state.CurrentConnectionStateUnsynchronized?.Connection.ConnectionId;

	public HubConnectionState State => _state.OverallState;

	public event Func<Exception?, Task>? Closed;

	public event Func<Exception?, Task>? Reconnecting;

	public event Func<string?, Task>? Reconnected;

	public HubConnection(IConnectionFactory connectionFactory, IHubProtocol protocol, EndPoint endPoint, IServiceProvider serviceProvider, ILoggerFactory loggerFactory, IRetryPolicy reconnectPolicy)
		: this(connectionFactory, protocol, endPoint, serviceProvider, loggerFactory)
	{
		_reconnectPolicy = reconnectPolicy;
	}

	public HubConnection(IConnectionFactory connectionFactory, IHubProtocol protocol, EndPoint endPoint, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
	{
		_connectionFactory = connectionFactory ?? throw new ArgumentNullException("connectionFactory");
		_protocol = protocol ?? throw new ArgumentNullException("protocol");
		_endPoint = endPoint ?? throw new ArgumentNullException("endPoint");
		_serviceProvider = serviceProvider ?? throw new ArgumentNullException("serviceProvider");
		_loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
		_logger = _loggerFactory.CreateLogger<HubConnection>();
		_state = new ReconnectingConnectionState(_logger);
		_logScope = new ConnectionLogScope();
	}

	public virtual async Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckDisposed();
		using (_logger.BeginScope(_logScope))
		{
			await StartAsyncInner(cancellationToken).ForceAsync();
		}
	}

	private async Task StartAsyncInner(CancellationToken cancellationToken = default(CancellationToken))
	{
		await _state.WaitConnectionLockAsync(cancellationToken, "StartAsyncInner", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 247);
		try
		{
			if (!_state.TryChangeState(HubConnectionState.Disconnected, HubConnectionState.Connecting))
			{
				throw new InvalidOperationException("The HubConnection cannot be started if it is not in the Disconnected state.");
			}
			if (_state.StopCts.Token.IsCancellationRequested)
			{
				throw new InvalidOperationException("The HubConnection cannot be started while StopAsync is running.");
			}
			CancellationToken linkedToken;
			using (CreateLinkedToken(cancellationToken, _state.StopCts.Token, out linkedToken))
			{
				await StartAsyncCore(linkedToken);
			}
			_state.ChangeState(HubConnectionState.Connecting, HubConnectionState.Connected);
		}
		catch
		{
			if (_state.TryChangeState(HubConnectionState.Connecting, HubConnectionState.Disconnected))
			{
				_state.StopCts = new CancellationTokenSource();
			}
			throw;
		}
		finally
		{
			_state.ReleaseConnectionLock("StartAsyncInner", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 280);
		}
	}

	public virtual async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckDisposed();
		using (_logger.BeginScope(_logScope))
		{
			await StopAsyncCore(disposing: false).ForceAsync();
		}
	}

	public virtual async ValueTask DisposeAsync()
	{
		if (!_disposed)
		{
			using (_logger.BeginScope(_logScope))
			{
				await StopAsyncCore(disposing: true).ForceAsync();
			}
		}
	}

	public virtual IDisposable On(string methodName, Type[] parameterTypes, Func<object?[], object, Task> handler, object state)
	{
		Log.RegisteringHandler(_logger, methodName);
		CheckDisposed();
		InvocationHandler invocationHandler = new InvocationHandler(parameterTypes, handler, state);
		InvocationHandlerList handlerList = _handlers.AddOrUpdate(methodName, (string _) => new InvocationHandlerList(invocationHandler), delegate(string _, InvocationHandlerList invocations)
		{
			lock (invocations)
			{
				invocations.Add(invocationHandler);
				return invocations;
			}
		});
		return new Subscription(invocationHandler, handlerList);
	}

	public virtual void Remove(string methodName)
	{
		CheckDisposed();
		Log.RemovingHandlers(_logger, methodName);
		_handlers.TryRemove(methodName, out var _);
	}

	public virtual async Task<ChannelReader<object?>> StreamAsChannelCoreAsync(string methodName, Type returnType, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		using (_logger.BeginScope(_logScope))
		{
			return await StreamAsChannelCoreAsyncCore(methodName, returnType, args, cancellationToken).ForceAsync();
		}
	}

	public virtual async Task<object?> InvokeCoreAsync(string methodName, Type returnType, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		using (_logger.BeginScope(_logScope))
		{
			return await InvokeCoreAsyncCore(methodName, returnType, args, cancellationToken).ForceAsync();
		}
	}

	public virtual async Task SendCoreAsync(string methodName, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		using (_logger.BeginScope(_logScope))
		{
			await SendCoreAsyncCore(methodName, args, cancellationToken).ForceAsync();
		}
	}

	private async Task StartAsyncCore(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		CheckDisposed();
		Log.Starting(_logger);
		ConnectionContext connection = await _connectionFactory.ConnectAsync(_endPoint, cancellationToken);
		ConnectionState startingConnectionState = new ConnectionState(connection, this);
		try
		{
			Log.HubProtocol(_logger, _protocol.Name, _protocol.Version);
			await HandshakeAsync(startingConnectionState, cancellationToken);
		}
		catch (Exception ex)
		{
			Log.ErrorStartingConnection(_logger, ex);
			await CloseAsync(startingConnectionState.Connection);
			throw;
		}
		_state.CurrentConnectionStateUnsynchronized = startingConnectionState;
		if (!(connection.Features.Get<IConnectionInherentKeepAliveFeature>()?.HasInherentKeepAlive ?? false))
		{
			await SendHubMessage(startingConnectionState, PingMessage.Instance, cancellationToken);
		}
		startingConnectionState.ReceiveTask = ReceiveLoop(startingConnectionState);
		Log.Started(_logger);
	}

	private static ValueTask CloseAsync(ConnectionContext connection)
	{
		return connection.DisposeAsync();
	}

	private async Task StopAsyncCore(bool disposing)
	{
		_state.StopCts.Cancel();
		await _state.WaitConnectionLockAsync(default(CancellationToken), "StopAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 484);
		Task reconnectTask = _state.ReconnectTask;
		if (reconnectTask.Status != TaskStatus.RanToCompletion)
		{
			_state.ReleaseConnectionLock("StopAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 495);
			await reconnectTask;
			await _state.WaitConnectionLockAsync(default(CancellationToken), "StopAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 497);
		}
		ConnectionState connectionState;
		try
		{
			if (disposing && _disposed)
			{
				return;
			}
			CheckDisposed();
			connectionState = _state.CurrentConnectionStateUnsynchronized;
			if (connectionState != null)
			{
				connectionState.Stopping = true;
			}
			else
			{
				_state.StopCts = new CancellationTokenSource();
			}
			if (disposing)
			{
				_disposed = true;
				if (_serviceProvider is IAsyncDisposable asyncDisposable)
				{
					await asyncDisposable.DisposeAsync();
				}
				else
				{
					(_serviceProvider as IDisposable)?.Dispose();
				}
			}
		}
		finally
		{
			_state.ReleaseConnectionLock("StopAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 541);
		}
		if (connectionState != null)
		{
			await connectionState.StopAsync();
		}
	}

	public virtual IAsyncEnumerable<TResult> StreamAsyncCore<TResult>(string methodName, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
	{
		CancellationTokenSource cts = (cancellationToken.CanBeCanceled ? CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { cancellationToken }) : new CancellationTokenSource());
		return AsyncEnumerableAdapters.MakeCancelableTypedAsyncEnumerable(CastIAsyncEnumerable<TResult>(methodName, args, cts), cts);
	}

	private async IAsyncEnumerable<T> CastIAsyncEnumerable<T>(string methodName, object[] args, CancellationTokenSource cts)
	{
		ChannelReader<object> reader = await StreamAsChannelCoreAsync(methodName, typeof(T), args, cts.Token);
		while (await reader.WaitToReadAsync(cts.Token))
		{
			object item;
			while (reader.TryRead(out item))
			{
				yield return (T)item;
			}
		}
	}

	private async Task<ChannelReader<object>> StreamAsChannelCoreAsyncCore(string methodName, Type returnType, object[] args, CancellationToken cancellationToken)
	{
		CheckDisposed();
		ConnectionState connectionState = await _state.WaitForActiveConnectionAsync("StreamAsChannelCoreAsync", cancellationToken, "StreamAsChannelCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 613);
		try
		{
			CheckDisposed();
			cancellationToken.ThrowIfCancellationRequested();
			List<string> streamIds;
			Dictionary<string, object> readers = PackageStreamingParams(connectionState, ref args, out streamIds);
			ChannelReader<object> channel;
			InvocationRequest irq2 = InvocationRequest.Stream(cancellationToken, returnType, connectionState.GetNextId(), _loggerFactory, this, out channel);
			await InvokeStreamCore(connectionState, methodName, irq2, args, streamIds?.ToArray(), cancellationToken);
			if (cancellationToken.CanBeCanceled)
			{
				cancellationToken.Register(delegate(object state)
				{
					OnStreamCanceled((InvocationRequest)state);
				}, irq2);
			}
			LaunchStreams(connectionState, readers, cancellationToken);
			return channel;
		}
		finally
		{
			_state.ReleaseConnectionLock("StreamAsChannelCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 636);
		}
		async Task OnStreamCanceled(InvocationRequest irq)
		{
			await _state.WaitConnectionLockAsync(default(CancellationToken), "StreamAsChannelCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 586);
			try
			{
				if (_state.CurrentConnectionStateUnsynchronized != null)
				{
					Log.SendingCancellation(_logger, irq.InvocationId);
					SendHubMessage(_state.CurrentConnectionStateUnsynchronized, new CancelInvocationMessage(irq.InvocationId), irq.CancellationToken);
				}
				else
				{
					Log.UnableToSendCancellation(_logger, irq.InvocationId);
				}
			}
			finally
			{
				_state.ReleaseConnectionLock("StreamAsChannelCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 603);
			}
			irq.Dispose();
		}
	}

	private Dictionary<string, object> PackageStreamingParams(ConnectionState connectionState, ref object[] args, out List<string> streamIds)
	{
		Dictionary<string, object> dictionary = null;
		streamIds = null;
		int num = args.Length;
		Span<bool> span = ((args.Length > 256) ? ((Span<bool>)new bool[args.Length]) : stackalloc bool[256].Slice(0, args.Length));
		Span<bool> span2 = span;
		for (int i = 0; i < args.Length; i++)
		{
			object obj = args[i];
			if (obj != null && ReflectionHelper.IsStreamingType(obj.GetType()))
			{
				span2[i] = true;
				num--;
				if (dictionary == null)
				{
					dictionary = new Dictionary<string, object>();
				}
				if (streamIds == null)
				{
					streamIds = new List<string>();
				}
				string nextId = connectionState.GetNextId();
				dictionary[nextId] = obj;
				streamIds.Add(nextId);
				Log.StartingStream(_logger, nextId);
			}
		}
		if (num == args.Length)
		{
			return null;
		}
		object[] array = ((num > 0) ? new object[num] : Array.Empty<object>());
		int num2 = 0;
		for (int j = 0; j < args.Length; j++)
		{
			if (!span2[j])
			{
				array[num2] = args[j];
				num2++;
			}
		}
		args = array;
		return dictionary;
	}

	private void LaunchStreams(ConnectionState connectionState, Dictionary<string, object> readers, CancellationToken cancellationToken)
	{
		if (readers == null)
		{
			return;
		}
		CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(connectionState.UploadStreamToken, cancellationToken);
		foreach (KeyValuePair<string, object> reader in readers)
		{
			object value = reader.Value;
			if (ReflectionHelper.IsIAsyncEnumerable(value.GetType()))
			{
				_sendIAsyncStreamItemsMethod.MakeGenericMethod(value.GetType().GetInterface("IAsyncEnumerable`1").GetGenericArguments()).Invoke(this, new object[4]
				{
					connectionState,
					reader.Key.ToString(),
					value,
					cancellationTokenSource
				});
			}
			else
			{
				_sendStreamItemsMethod.MakeGenericMethod(value.GetType().GetGenericArguments()).Invoke(this, new object[4]
				{
					connectionState,
					reader.Key.ToString(),
					value,
					cancellationTokenSource
				});
			}
		}
	}

	private Task SendStreamItems<T>(ConnectionState connectionState, string streamId, ChannelReader<T> reader, CancellationTokenSource tokenSource)
	{
		return CommonStreaming(connectionState, streamId, ReadChannelStream);
		async Task ReadChannelStream()
		{
			while (await reader.WaitToReadAsync(tokenSource.Token))
			{
				T item;
				while (!tokenSource.Token.IsCancellationRequested && reader.TryRead(out item))
				{
					await SendWithLock(connectionState, new StreamItemMessage(streamId, item), tokenSource.Token, "SendStreamItems");
					Log.SendingStreamItem(_logger, streamId);
				}
			}
		}
	}

	private Task SendIAsyncEnumerableStreamItems<T>(ConnectionState connectionState, string streamId, IAsyncEnumerable<T> stream, CancellationTokenSource tokenSource)
	{
		return CommonStreaming(connectionState, streamId, ReadAsyncEnumerableStream);
		async Task ReadAsyncEnumerableStream()
		{
			IAsyncEnumerable<T> asyncEnumerable = AsyncEnumerableAdapters.MakeCancelableTypedAsyncEnumerable(stream, tokenSource);
			await foreach (T item in asyncEnumerable)
			{
				await SendWithLock(connectionState, new StreamItemMessage(streamId, item), tokenSource.Token, "SendIAsyncEnumerableStreamItems");
				Log.SendingStreamItem(_logger, streamId);
			}
		}
	}

	private async Task CommonStreaming(ConnectionState connectionState, string streamId, Func<Task> createAndConsumeStream)
	{
		Log.StartingStream(_logger, streamId);
		string responseError = null;
		try
		{
			await createAndConsumeStream();
		}
		catch (OperationCanceledException)
		{
			Log.CancelingStream(_logger, streamId);
			responseError = "Stream canceled by client.";
		}
		catch (Exception ex2)
		{
			Log.ErroredStream(_logger, streamId, ex2);
			responseError = $"Stream errored by client: '{ex2}'";
		}
		Log.CompletingStream(_logger, streamId);
		await SendWithLock(connectionState, CompletionMessage.WithError(streamId, responseError), default(CancellationToken), "CommonStreaming");
	}

	private async Task<object> InvokeCoreAsyncCore(string methodName, Type returnType, object[] args, CancellationToken cancellationToken)
	{
		CheckDisposed();
		ConnectionState connectionState = await _state.WaitForActiveConnectionAsync("InvokeCoreAsync", cancellationToken, "InvokeCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 797);
		Task<object> invocationTask;
		try
		{
			CheckDisposed();
			List<string> streamIds;
			Dictionary<string, object> readers = PackageStreamingParams(connectionState, ref args, out streamIds);
			InvocationRequest irq = InvocationRequest.Invoke(cancellationToken, returnType, connectionState.GetNextId(), _loggerFactory, this, out invocationTask);
			await InvokeCore(connectionState, methodName, irq, args, streamIds?.ToArray(), cancellationToken);
			LaunchStreams(connectionState, readers, cancellationToken);
		}
		finally
		{
			_state.ReleaseConnectionLock("InvokeCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 813);
		}
		return await invocationTask;
	}

	private async Task InvokeCore(ConnectionState connectionState, string methodName, InvocationRequest irq, object[] args, string[] streams, CancellationToken cancellationToken)
	{
		Log.PreparingBlockingInvocation(_logger, irq.InvocationId, methodName, irq.ResultType.FullName, args.Length);
		InvocationMessage hubMessage = new InvocationMessage(irq.InvocationId, methodName, args, streams);
		Log.RegisteringInvocation(_logger, irq.InvocationId);
		connectionState.AddInvocation(irq);
		Log.IssuingInvocation(_logger, irq.InvocationId, irq.ResultType.FullName, methodName, args);
		try
		{
			await SendHubMessage(connectionState, hubMessage, cancellationToken);
		}
		catch (Exception exception)
		{
			Log.FailedToSendInvocation(_logger, irq.InvocationId, exception);
			connectionState.TryRemoveInvocation(irq.InvocationId, out var _);
			irq.Fail(exception);
		}
	}

	private async Task InvokeStreamCore(ConnectionState connectionState, string methodName, InvocationRequest irq, object[] args, string[] streams, CancellationToken cancellationToken)
	{
		Log.PreparingStreamingInvocation(_logger, irq.InvocationId, methodName, irq.ResultType.FullName, args.Length);
		StreamInvocationMessage hubMessage = new StreamInvocationMessage(irq.InvocationId, methodName, args, streams);
		Log.RegisteringInvocation(_logger, irq.InvocationId);
		connectionState.AddInvocation(irq);
		Log.IssuingInvocation(_logger, irq.InvocationId, irq.ResultType.FullName, methodName, args);
		try
		{
			await SendHubMessage(connectionState, hubMessage, cancellationToken);
		}
		catch (Exception exception)
		{
			Log.FailedToSendInvocation(_logger, irq.InvocationId, exception);
			connectionState.TryRemoveInvocation(irq.InvocationId, out var _);
			irq.Fail(exception);
		}
	}

	private async Task SendHubMessage(ConnectionState connectionState, HubMessage hubMessage, CancellationToken cancellationToken = default(CancellationToken))
	{
		_protocol.WriteMessage(hubMessage, connectionState.Connection.Transport.Output);
		Log.SendingMessage(_logger, hubMessage);
		await connectionState.Connection.Transport.Output.FlushAsync();
		Log.MessageSent(_logger, hubMessage);
		connectionState.ResetSendPing();
	}

	private async Task SendCoreAsyncCore(string methodName, object[] args, CancellationToken cancellationToken)
	{
		CheckDisposed();
		ConnectionState connectionState = await _state.WaitForActiveConnectionAsync("SendCoreAsync", cancellationToken, "SendCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 894);
		try
		{
			CheckDisposed();
			List<string> streamIds;
			Dictionary<string, object> readers = PackageStreamingParams(connectionState, ref args, out streamIds);
			Log.PreparingNonBlockingInvocation(_logger, methodName, args.Length);
			InvocationMessage hubMessage = new InvocationMessage(null, methodName, args, streamIds?.ToArray());
			await SendHubMessage(connectionState, hubMessage, cancellationToken);
			LaunchStreams(connectionState, readers, cancellationToken);
		}
		finally
		{
			_state.ReleaseConnectionLock("SendCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 909);
		}
	}

	private async Task SendWithLock(ConnectionState expectedConnectionState, HubMessage message, CancellationToken cancellationToken, [CallerMemberName] string callerName = "")
	{
		CheckDisposed();
		ConnectionState connectionState = await _state.WaitForActiveConnectionAsync(callerName, cancellationToken, "SendWithLock", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 916);
		try
		{
			CheckDisposed();
			await SendHubMessage(connectionState, message, cancellationToken);
		}
		finally
		{
			_state.ReleaseConnectionLock("SendWithLock", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 927);
		}
	}

	private async Task<CloseMessage> ProcessMessagesAsync(HubMessage message, ConnectionState connectionState, ChannelWriter<InvocationMessage> invocationMessageWriter)
	{
		Log.ResettingKeepAliveTimer(_logger);
		connectionState.ResetTimeout();
		if (!(message is InvocationBindingFailureMessage invocationBindingFailureMessage))
		{
			if (!(message is InvocationMessage invocationMessage))
			{
				InvocationRequest irq;
				if (!(message is CompletionMessage completionMessage))
				{
					if (!(message is StreamItemMessage streamItemMessage))
					{
						if (message is CloseMessage closeMessage)
						{
							if (string.IsNullOrEmpty(closeMessage.Error))
							{
								Log.ReceivedClose(_logger);
							}
							else
							{
								Log.ReceivedCloseWithError(_logger, closeMessage.Error);
							}
							return closeMessage;
						}
						if (!(message is PingMessage))
						{
							throw new InvalidOperationException("Unexpected message type: " + message.GetType().FullName);
						}
						Log.ReceivedPing(_logger);
					}
					else if (connectionState.TryGetInvocation(streamItemMessage.InvocationId, out irq))
					{
						await DispatchInvocationStreamItemAsync(streamItemMessage, irq);
					}
					else
					{
						Log.DroppedStreamMessage(_logger, streamItemMessage.InvocationId);
					}
				}
				else if (!connectionState.TryRemoveInvocation(completionMessage.InvocationId, out irq))
				{
					Log.DroppedCompletionMessage(_logger, completionMessage.InvocationId);
				}
				else
				{
					DispatchInvocationCompletion(completionMessage, irq);
					irq.Dispose();
				}
			}
			else
			{
				Log.ReceivedInvocation(_logger, invocationMessage.InvocationId, invocationMessage.Target, invocationMessage.Arguments);
				await invocationMessageWriter.WriteAsync(invocationMessage);
			}
		}
		else
		{
			Log.ArgumentBindingFailure(_logger, invocationBindingFailureMessage.InvocationId, invocationBindingFailureMessage.Target, invocationBindingFailureMessage.BindingFailure.SourceException);
		}
		return null;
	}

	private async Task DispatchInvocationAsync(InvocationMessage invocation)
	{
		if (!_handlers.TryGetValue(invocation.Target, out var value))
		{
			Log.MissingHandler(_logger, invocation.Target);
			return;
		}
		InvocationHandler[] handlers = value.GetHandlers();
		InvocationHandler[] array = handlers;
		for (int i = 0; i < array.Length; i++)
		{
			InvocationHandler invocationHandler = array[i];
			try
			{
				await invocationHandler.InvokeAsync(invocation.Arguments);
			}
			catch (Exception exception)
			{
				Log.ErrorInvokingClientSideMethod(_logger, invocation.Target, exception);
			}
		}
	}

	private async Task DispatchInvocationStreamItemAsync(StreamItemMessage streamItem, InvocationRequest irq)
	{
		Log.ReceivedStreamItem(_logger, irq.InvocationId);
		if (irq.CancellationToken.IsCancellationRequested)
		{
			Log.CancelingStreamItem(_logger, irq.InvocationId);
		}
		else if (!(await irq.StreamItem(streamItem.Item)))
		{
			Log.ReceivedStreamItemAfterClose(_logger, irq.InvocationId);
		}
	}

	private void DispatchInvocationCompletion(CompletionMessage completion, InvocationRequest irq)
	{
		Log.ReceivedInvocationCompletion(_logger, irq.InvocationId);
		if (irq.CancellationToken.IsCancellationRequested)
		{
			Log.CancelingInvocationCompletion(_logger, irq.InvocationId);
		}
		else
		{
			irq.Complete(completion);
		}
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HubConnection");
		}
	}

	private async Task HandshakeAsync(ConnectionState startingConnectionState, CancellationToken cancellationToken)
	{
		Log.SendingHubHandshake(_logger);
		HandshakeProtocol.WriteRequestMessage(new HandshakeRequestMessage(_protocol.Name, _protocol.Version), startingConnectionState.Connection.Transport.Output);
		if ((await startingConnectionState.Connection.Transport.Output.FlushAsync(CancellationToken.None)).IsCompleted)
		{
			IOException ex = new IOException("The server disconnected before the handshake could be started.");
			Log.ErrorReceivingHandshakeResponse(_logger, ex);
			throw ex;
		}
		PipeReader input = startingConnectionState.Connection.Transport.Input;
		using CancellationTokenSource handshakeCts = new CancellationTokenSource(HandshakeTimeout);
		_ = 1;
		try
		{
			CancellationToken linkedToken;
			using (CreateLinkedToken(cancellationToken, handshakeCts.Token, out linkedToken))
			{
				while (true)
				{
					ReadResult readResult = await input.ReadAsync(linkedToken);
					ReadOnlySequence<byte> buffer = readResult.Buffer;
					SequencePosition start = buffer.Start;
					SequencePosition examined = buffer.End;
					try
					{
						if (!buffer.IsEmpty && HandshakeProtocol.TryParseResponseMessage(ref buffer, out HandshakeResponseMessage responseMessage))
						{
							start = buffer.Start;
							examined = start;
							if (responseMessage.Error != null)
							{
								Log.HandshakeServerError(_logger, responseMessage.Error);
								throw new HubException("Unable to complete handshake with the server due to an error: " + responseMessage.Error);
							}
							Log.HandshakeComplete(_logger);
							break;
						}
						if (readResult.IsCompleted)
						{
							throw new InvalidOperationException("The server disconnected before sending a handshake response");
						}
					}
					finally
					{
						input.AdvanceTo(start, examined);
					}
				}
			}
		}
		catch (HubException)
		{
			throw;
		}
		catch (InvalidDataException exception)
		{
			Log.ErrorInvalidHandshakeResponse(_logger, exception);
			throw;
		}
		catch (OperationCanceledException exception2)
		{
			if (handshakeCts.IsCancellationRequested)
			{
				Log.ErrorHandshakeTimedOut(_logger, HandshakeTimeout, exception2);
			}
			else
			{
				Log.ErrorHandshakeCanceled(_logger, exception2);
			}
			throw;
		}
		catch (Exception exception3)
		{
			Log.ErrorReceivingHandshakeResponse(_logger, exception3);
			throw;
		}
	}

	private async Task ReceiveLoop(ConnectionState connectionState)
	{
		Log.ReceiveLoopStarting(_logger);
		TimerAwaitable timer = new TimerAwaitable(TickRate, TickRate);
		Task timerTask = connectionState.TimerLoop(timer);
		CancellationTokenSource uploadStreamSource = new CancellationTokenSource();
		connectionState.UploadStreamToken = uploadStreamSource.Token;
		Channel<InvocationMessage> invocationMessageChannel = Channel.CreateUnbounded<InvocationMessage>(_receiveLoopOptions);
		connectionState.InvocationMessageReceiveTask = StartProcessingInvocationMessages(invocationMessageChannel.Reader);
		PipeReader input = connectionState.Connection.Transport.Input;
		try
		{
			_ = 1;
			try
			{
				while (true)
				{
					ReadResult result = await input.ReadAsync();
					ReadOnlySequence<byte> buffer = result.Buffer;
					try
					{
						if (result.IsCanceled)
						{
							break;
						}
						if (buffer.IsEmpty)
						{
							goto IL_028a;
						}
						Log.ProcessingMessage(_logger, buffer.Length);
						CloseMessage closeMessage = null;
						HubMessage message;
						while (_protocol.TryParseMessage(ref buffer, connectionState, out message))
						{
							closeMessage = await ProcessMessagesAsync(message, connectionState, invocationMessageChannel.Writer);
							if (closeMessage != null)
							{
								if (closeMessage.Error != null)
								{
									connectionState.CloseException = new HubException("The server closed the connection with the following error: " + closeMessage.Error);
								}
								if (!closeMessage.AllowReconnect)
								{
									connectionState.Stopping = true;
								}
								break;
							}
						}
						if (closeMessage == null)
						{
							goto IL_028a;
						}
						goto end_IL_0172;
						IL_028a:
						if (result.IsCompleted)
						{
							if (buffer.IsEmpty)
							{
								break;
							}
							throw new InvalidDataException("Connection terminated while reading a message.");
						}
						continue;
						end_IL_0172:;
					}
					finally
					{
						input.AdvanceTo(buffer.Start, buffer.End);
					}
					break;
				}
			}
			catch (Exception ex)
			{
				Log.ServerDisconnectedWithError(_logger, ex);
				connectionState.CloseException = ex;
			}
		}
		finally
		{
			invocationMessageChannel.Writer.TryComplete();
			timer.Stop();
			await timerTask;
			uploadStreamSource.Cancel();
			await HandleConnectionClose(connectionState);
		}
		async Task StartProcessingInvocationMessages(ChannelReader<InvocationMessage> invocationMessageChannelReader)
		{
			while (await invocationMessageChannelReader.WaitToReadAsync())
			{
				InvocationMessage item;
				while (invocationMessageChannelReader.TryRead(out item))
				{
					await DispatchInvocationAsync(item);
				}
			}
		}
	}

	internal Task RunTimerActions()
	{
		return _state.CurrentConnectionStateUnsynchronized.RunTimerActions();
	}

	internal void OnServerTimeout()
	{
		_state.CurrentConnectionStateUnsynchronized.OnServerTimeout();
	}

	private async Task HandleConnectionClose(ConnectionState connectionState)
	{
		await _state.WaitConnectionLockAsync(default(CancellationToken), "HandleConnectionClose", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1286);
		try
		{
			_state.CurrentConnectionStateUnsynchronized = null;
			await CloseAsync(connectionState.Connection);
			connectionState.CancelOutstandingInvocations(connectionState.CloseException);
			if (connectionState.Stopping || _reconnectPolicy == null)
			{
				if (connectionState.CloseException != null)
				{
					Log.ShutdownWithError(_logger, connectionState.CloseException);
				}
				else
				{
					Log.ShutdownConnection(_logger);
				}
				_state.ChangeState(HubConnectionState.Connected, HubConnectionState.Disconnected);
				CompleteClose(connectionState.CloseException);
			}
			else
			{
				_state.ReconnectTask = ReconnectAsync(connectionState.CloseException);
			}
		}
		finally
		{
			_state.ReleaseConnectionLock("HandleConnectionClose", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1320);
		}
	}

	private void CompleteClose(Exception closeException)
	{
		_state.StopCts = new CancellationTokenSource();
		RunCloseEvent(closeException);
	}

	private void RunCloseEvent(Exception closeException)
	{
		Func<Exception, Task> closed = this.Closed;
		if (closed != null)
		{
			RunClosedEventAsync();
		}
		async Task RunClosedEventAsync()
		{
			await AwaitableThreadPool.Yield();
			try
			{
				Log.InvokingClosedEventHandler(_logger);
				await closed(closeException);
			}
			catch (Exception exception)
			{
				Log.ErrorDuringClosedEvent(_logger, exception);
			}
		}
	}

	private async Task ReconnectAsync(Exception closeException)
	{
		int previousReconnectAttempts = 0;
		DateTime reconnectStartTime = DateTime.UtcNow;
		Exception retryReason = closeException;
		TimeSpan? nextRetryDelay = GetNextRetryDelay(previousReconnectAttempts, TimeSpan.Zero, retryReason);
		if (!nextRetryDelay.HasValue)
		{
			Log.FirstReconnectRetryDelayNull(_logger);
			_state.ChangeState(HubConnectionState.Connected, HubConnectionState.Disconnected);
			CompleteClose(closeException);
			return;
		}
		_state.ChangeState(HubConnectionState.Connected, HubConnectionState.Reconnecting);
		if (closeException != null)
		{
			Log.ReconnectingWithError(_logger, closeException);
		}
		else
		{
			Log.Reconnecting(_logger);
		}
		RunReconnectingEvent(closeException);
		while (nextRetryDelay.HasValue)
		{
			Log.AwaitingReconnectRetryDelay(_logger, previousReconnectAttempts + 1, nextRetryDelay.Value);
			try
			{
				await Task.Delay(nextRetryDelay.Value, _state.StopCts.Token);
			}
			catch (OperationCanceledException ex)
			{
				Log.ReconnectingStoppedDuringRetryDelay(_logger);
				await _state.WaitConnectionLockAsync(default(CancellationToken), "ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1404);
				try
				{
					_state.ChangeState(HubConnectionState.Reconnecting, HubConnectionState.Disconnected);
					CompleteClose(GetOperationCanceledException("Connection stopped during reconnect delay. Done reconnecting.", ex, _state.StopCts.Token));
					return;
				}
				finally
				{
					_state.ReleaseConnectionLock("ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1413);
				}
			}
			await _state.WaitConnectionLockAsync(default(CancellationToken), "ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1419);
			try
			{
				await StartAsyncCore(_state.StopCts.Token);
				Log.Reconnected(_logger, previousReconnectAttempts, DateTime.UtcNow - reconnectStartTime);
				_state.ChangeState(HubConnectionState.Reconnecting, HubConnectionState.Connected);
				RunReconnectedEvent();
				return;
			}
			catch (Exception ex2)
			{
				retryReason = ex2;
				Log.ReconnectAttemptFailed(_logger, ex2);
				if (_state.StopCts.IsCancellationRequested)
				{
					Log.ReconnectingStoppedDuringReconnectAttempt(_logger);
					_state.ChangeState(HubConnectionState.Reconnecting, HubConnectionState.Disconnected);
					CompleteClose(GetOperationCanceledException("Connection stopped during reconnect attempt. Done reconnecting.", ex2, _state.StopCts.Token));
					return;
				}
				previousReconnectAttempts++;
			}
			finally
			{
				_state.ReleaseConnectionLock("ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1454);
			}
			nextRetryDelay = GetNextRetryDelay(previousReconnectAttempts, DateTime.UtcNow - reconnectStartTime, retryReason);
		}
		await _state.WaitConnectionLockAsync(default(CancellationToken), "ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1460);
		try
		{
			TimeSpan timeSpan = DateTime.UtcNow - reconnectStartTime;
			Log.ReconnectAttemptsExhausted(_logger, previousReconnectAttempts, timeSpan);
			_state.ChangeState(HubConnectionState.Reconnecting, HubConnectionState.Disconnected);
			string message = $"Reconnect retries have been exhausted after {previousReconnectAttempts} failed attempts and {timeSpan} elapsed. Disconnecting.";
			CompleteClose(new OperationCanceledException(message));
		}
		finally
		{
			_state.ReleaseConnectionLock("ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1476);
		}
	}

	private TimeSpan? GetNextRetryDelay(long previousRetryCount, TimeSpan elapsedTime, Exception retryReason)
	{
		try
		{
			return _reconnectPolicy.NextRetryDelay(new RetryContext
			{
				PreviousRetryCount = previousRetryCount,
				ElapsedTime = elapsedTime,
				RetryReason = retryReason
			});
		}
		catch (Exception exception)
		{
			Log.ErrorDuringNextRetryDelay(_logger, exception);
			return null;
		}
	}

	private OperationCanceledException GetOperationCanceledException(string message, Exception innerException, CancellationToken cancellationToken)
	{
		return new OperationCanceledException(message, innerException);
	}

	private void RunReconnectingEvent(Exception closeException)
	{
		Func<Exception, Task> reconnecting = this.Reconnecting;
		if (reconnecting != null)
		{
			RunReconnectingEventAsync();
		}
		async Task RunReconnectingEventAsync()
		{
			await AwaitableThreadPool.Yield();
			try
			{
				await reconnecting(closeException);
			}
			catch (Exception exception)
			{
				Log.ErrorDuringReconnectingEvent(_logger, exception);
			}
		}
	}

	private void RunReconnectedEvent()
	{
		Func<string, Task> reconnected = this.Reconnected;
		if (reconnected != null)
		{
			RunReconnectedEventAsync();
		}
		async Task RunReconnectedEventAsync()
		{
			await AwaitableThreadPool.Yield();
			try
			{
				await reconnected(ConnectionId);
			}
			catch (Exception exception)
			{
				Log.ErrorDuringReconnectedEvent(_logger, exception);
			}
		}
	}

	private static IDisposable CreateLinkedToken(CancellationToken token1, CancellationToken token2, out CancellationToken linkedToken)
	{
		if (!token1.CanBeCanceled)
		{
			linkedToken = token2;
			return null;
		}
		if (!token2.CanBeCanceled)
		{
			linkedToken = token1;
			return null;
		}
		CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token1, token2);
		linkedToken = cancellationTokenSource.Token;
		return cancellationTokenSource;
	}

	[Conditional("DEBUG")]
	private static void SafeAssert(bool condition, string message, [CallerMemberName] string memberName = null, [CallerFilePath] string fileName = null, [CallerLineNumber] int lineNumber = 0)
	{
		if (!condition)
		{
			throw new Exception($"Assertion failed in {memberName}, at {fileName}:{lineNumber}: {message}");
		}
	}
}
