using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.SignalR.Internal;

public class DefaultHubDispatcher<THub> : HubDispatcher<THub> where THub : Hub
{
	private static class Log
	{
		private static readonly Action<ILogger, InvocationMessage, Exception> _receivedHubInvocation = LoggerMessage.Define<InvocationMessage>(LogLevel.Debug, new EventId(1, "ReceivedHubInvocation"), "Received hub invocation: {InvocationMessage}.");

		private static readonly Action<ILogger, string, Exception> _unsupportedMessageReceived = LoggerMessage.Define<string>(LogLevel.Error, new EventId(2, "UnsupportedMessageReceived"), "Received unsupported message of type '{MessageType}'.");

		private static readonly Action<ILogger, string, Exception> _unknownHubMethod = LoggerMessage.Define<string>(LogLevel.Error, new EventId(3, "UnknownHubMethod"), "Unknown hub method '{HubMethod}'.");

		private static readonly Action<ILogger, Exception> _outboundChannelClosed = LoggerMessage.Define(LogLevel.Warning, new EventId(4, "OutboundChannelClosed"), "Outbound channel was closed while trying to write hub message.");

		private static readonly Action<ILogger, string, Exception> _hubMethodNotAuthorized = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(5, "HubMethodNotAuthorized"), "Failed to invoke '{HubMethod}' because user is unauthorized.");

		private static readonly Action<ILogger, string, string, Exception> _streamingResult = LoggerMessage.Define<string, string>(LogLevel.Trace, new EventId(6, "StreamingResult"), "InvocationId {InvocationId}: Streaming result of type '{ResultType}'.");

		private static readonly Action<ILogger, string, string, Exception> _sendingResult = LoggerMessage.Define<string, string>(LogLevel.Trace, new EventId(7, "SendingResult"), "InvocationId {InvocationId}: Sending result of type '{ResultType}'.");

		private static readonly Action<ILogger, string, Exception> _failedInvokingHubMethod = LoggerMessage.Define<string>(LogLevel.Error, new EventId(8, "FailedInvokingHubMethod"), "Failed to invoke hub method '{HubMethod}'.");

		private static readonly Action<ILogger, string, string, Exception> _hubMethodBound = LoggerMessage.Define<string, string>(LogLevel.Trace, new EventId(9, "HubMethodBound"), "'{HubName}' hub method '{HubMethod}' is bound.");

		private static readonly Action<ILogger, string, Exception> _cancelStream = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(10, "CancelStream"), "Canceling stream for invocation {InvocationId}.");

		private static readonly Action<ILogger, Exception> _unexpectedCancel = LoggerMessage.Define(LogLevel.Debug, new EventId(11, "UnexpectedCancel"), "CancelInvocationMessage received unexpectedly.");

		private static readonly Action<ILogger, StreamInvocationMessage, Exception> _receivedStreamHubInvocation = LoggerMessage.Define<StreamInvocationMessage>(LogLevel.Debug, new EventId(12, "ReceivedStreamHubInvocation"), "Received stream hub invocation: {InvocationMessage}.");

		private static readonly Action<ILogger, HubMethodInvocationMessage, Exception> _streamingMethodCalledWithInvoke = LoggerMessage.Define<HubMethodInvocationMessage>(LogLevel.Error, new EventId(13, "StreamingMethodCalledWithInvoke"), "A streaming method was invoked with a non-streaming invocation : {InvocationMessage}.");

		private static readonly Action<ILogger, HubMethodInvocationMessage, Exception> _nonStreamingMethodCalledWithStream = LoggerMessage.Define<HubMethodInvocationMessage>(LogLevel.Error, new EventId(14, "NonStreamingMethodCalledWithStream"), "A non-streaming method was invoked with a streaming invocation : {InvocationMessage}.");

		private static readonly Action<ILogger, string, Exception> _invalidReturnValueFromStreamingMethod = LoggerMessage.Define<string>(LogLevel.Error, new EventId(15, "InvalidReturnValueFromStreamingMethod"), "A streaming method returned a value that cannot be used to build enumerator {HubMethod}.");

		public static void ReceivedHubInvocation(ILogger logger, InvocationMessage invocationMessage)
		{
			_receivedHubInvocation(logger, invocationMessage, null);
		}

		public static void UnsupportedMessageReceived(ILogger logger, string messageType)
		{
			_unsupportedMessageReceived(logger, messageType, null);
		}

		public static void UnknownHubMethod(ILogger logger, string hubMethod)
		{
			_unknownHubMethod(logger, hubMethod, null);
		}

		public static void OutboundChannelClosed(ILogger logger)
		{
			_outboundChannelClosed(logger, null);
		}

		public static void HubMethodNotAuthorized(ILogger logger, string hubMethod)
		{
			_hubMethodNotAuthorized(logger, hubMethod, null);
		}

		public static void StreamingResult(ILogger logger, string invocationId, ObjectMethodExecutor objectMethodExecutor)
		{
			Type type = ((objectMethodExecutor.AsyncResultType == null) ? objectMethodExecutor.MethodReturnType : objectMethodExecutor.AsyncResultType);
			_streamingResult(logger, invocationId, type.FullName, null);
		}

		public static void SendingResult(ILogger logger, string invocationId, ObjectMethodExecutor objectMethodExecutor)
		{
			Type type = ((objectMethodExecutor.AsyncResultType == null) ? objectMethodExecutor.MethodReturnType : objectMethodExecutor.AsyncResultType);
			_sendingResult(logger, invocationId, type.FullName, null);
		}

		public static void FailedInvokingHubMethod(ILogger logger, string hubMethod, Exception exception)
		{
			_failedInvokingHubMethod(logger, hubMethod, exception);
		}

		public static void HubMethodBound(ILogger logger, string hubName, string hubMethod)
		{
			_hubMethodBound(logger, hubName, hubMethod, null);
		}

		public static void CancelStream(ILogger logger, string invocationId)
		{
			_cancelStream(logger, invocationId, null);
		}

		public static void UnexpectedCancel(ILogger logger)
		{
			_unexpectedCancel(logger, null);
		}

		public static void ReceivedStreamHubInvocation(ILogger logger, StreamInvocationMessage invocationMessage)
		{
			_receivedStreamHubInvocation(logger, invocationMessage, null);
		}

		public static void StreamingMethodCalledWithInvoke(ILogger logger, HubMethodInvocationMessage invocationMessage)
		{
			_streamingMethodCalledWithInvoke(logger, invocationMessage, null);
		}

		public static void NonStreamingMethodCalledWithStream(ILogger logger, HubMethodInvocationMessage invocationMessage)
		{
			_nonStreamingMethodCalledWithStream(logger, invocationMessage, null);
		}

		public static void InvalidReturnValueFromStreamingMethod(ILogger logger, string hubMethod)
		{
			_invalidReturnValueFromStreamingMethod(logger, hubMethod, null);
		}
	}

	private readonly Dictionary<string, HubMethodDescriptor> _methods = new Dictionary<string, HubMethodDescriptor>(StringComparer.OrdinalIgnoreCase);

	private readonly IServiceScopeFactory _serviceScopeFactory;

	private readonly IHubContext<THub> _hubContext;

	private readonly ILogger<HubDispatcher<THub>> _logger;

	private readonly bool _enableDetailedErrors;

	public DefaultHubDispatcher(IServiceScopeFactory serviceScopeFactory, IHubContext<THub> hubContext, IOptions<HubOptions<THub>> hubOptions, IOptions<HubOptions> globalHubOptions, ILogger<DefaultHubDispatcher<THub>> logger)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_hubContext = hubContext;
		_enableDetailedErrors = hubOptions.Value.EnableDetailedErrors ?? globalHubOptions.Value.EnableDetailedErrors ?? false;
		_logger = logger;
		DiscoverHubMethods();
	}

	public override async Task OnConnectedAsync(HubConnectionContext connection)
	{
		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		IHubActivator<THub> hubActivator = scope.ServiceProvider.GetRequiredService<IHubActivator<THub>>();
		THub hub = hubActivator.Create();
		try
		{
			InitializeHub(hub, connection);
			await hub.OnConnectedAsync();
		}
		finally
		{
			hubActivator.Release(hub);
		}
	}

	public override async Task OnDisconnectedAsync(HubConnectionContext connection, Exception exception)
	{
		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		IHubActivator<THub> hubActivator = scope.ServiceProvider.GetRequiredService<IHubActivator<THub>>();
		THub hub = hubActivator.Create();
		try
		{
			InitializeHub(hub, connection);
			await hub.OnDisconnectedAsync(exception);
		}
		finally
		{
			hubActivator.Release(hub);
		}
	}

	public override async Task DispatchMessageAsync(HubConnectionContext connection, HubMessage hubMessage)
	{
		if (hubMessage != null)
		{
			if (hubMessage is InvocationBindingFailureMessage invocationBindingFailureMessage)
			{
				InvocationBindingFailureMessage bindingFailureMessage = invocationBindingFailureMessage;
				await ProcessBindingFailure(connection, bindingFailureMessage);
				return;
			}
			if (hubMessage is InvocationMessage invocationMessage)
			{
				InvocationMessage invocationMessage2 = invocationMessage;
				Log.ReceivedHubInvocation(_logger, invocationMessage2);
				await ProcessInvocation(connection, invocationMessage2, isStreamedInvocation: false);
				return;
			}
			if (hubMessage is StreamInvocationMessage streamInvocationMessage)
			{
				StreamInvocationMessage streamInvocationMessage2 = streamInvocationMessage;
				Log.ReceivedStreamHubInvocation(_logger, streamInvocationMessage2);
				await ProcessInvocation(connection, streamInvocationMessage2, isStreamedInvocation: true);
				return;
			}
			if (hubMessage is CancelInvocationMessage cancelInvocationMessage)
			{
				CancelInvocationMessage cancelInvocationMessage2 = cancelInvocationMessage;
				if (connection.ActiveRequestCancellationSources.TryGetValue(cancelInvocationMessage2.InvocationId, out var value))
				{
					Log.CancelStream(_logger, cancelInvocationMessage2.InvocationId);
					value.Cancel();
				}
				else
				{
					Log.UnexpectedCancel(_logger);
				}
				return;
			}
			if (hubMessage is PingMessage)
			{
				return;
			}
		}
		Log.UnsupportedMessageReceived(_logger, hubMessage.GetType().FullName);
		throw new NotSupportedException($"Received unsupported message: {hubMessage}");
	}

	private Task ProcessBindingFailure(HubConnectionContext connection, InvocationBindingFailureMessage bindingFailureMessage)
	{
		Log.FailedInvokingHubMethod(_logger, bindingFailureMessage.Target, bindingFailureMessage.BindingFailure.SourceException);
		string errorMessage = ErrorMessageHelper.BuildErrorMessage("Failed to invoke '" + bindingFailureMessage.Target + "' due to an error on the server.", bindingFailureMessage.BindingFailure.SourceException, _enableDetailedErrors);
		return SendInvocationError(bindingFailureMessage.InvocationId, connection, errorMessage);
	}

	public override Type GetReturnType(string invocationId)
	{
		return typeof(object);
	}

	public override IReadOnlyList<Type> GetParameterTypes(string methodName)
	{
		if (!_methods.TryGetValue(methodName, out var value))
		{
			return Type.EmptyTypes;
		}
		return value.ParameterTypes;
	}

	private async Task ProcessInvocation(HubConnectionContext connection, HubMethodInvocationMessage hubMethodInvocationMessage, bool isStreamedInvocation)
	{
		_ = 1;
		try
		{
			if (!_methods.TryGetValue(hubMethodInvocationMessage.Target, out var value))
			{
				Log.UnknownHubMethod(_logger, hubMethodInvocationMessage.Target);
				await connection.WriteAsync(CompletionMessage.WithError(hubMethodInvocationMessage.InvocationId, "Unknown hub method '" + hubMethodInvocationMessage.Target + "'"));
			}
			else
			{
				await Invoke(value, connection, hubMethodInvocationMessage, isStreamedInvocation);
			}
		}
		catch (Exception exception)
		{
			connection.Abort(exception);
		}
	}

	private async Task Invoke(HubMethodDescriptor descriptor, HubConnectionContext connection, HubMethodInvocationMessage hubMethodInvocationMessage, bool isStreamedInvocation)
	{
		ObjectMethodExecutor methodExecutor = descriptor.MethodExecutor;
		bool disposeScope = true;
		IServiceScope scope = _serviceScopeFactory.CreateScope();
		IHubActivator<THub> hubActivator = null;
		THub hub = null;
		try
		{
			if (!(await IsHubMethodAuthorized(scope.ServiceProvider, connection.User, descriptor.Policies)))
			{
				Log.HubMethodNotAuthorized(_logger, hubMethodInvocationMessage.Target);
				await SendInvocationError(hubMethodInvocationMessage.InvocationId, connection, "Failed to invoke '" + hubMethodInvocationMessage.Target + "' because user is unauthorized");
			}
			else
			{
				if (!(await ValidateInvocationMode(descriptor, isStreamedInvocation, hubMethodInvocationMessage, connection)))
				{
					return;
				}
				hubActivator = scope.ServiceProvider.GetRequiredService<IHubActivator<THub>>();
				hub = hubActivator.Create();
				try
				{
					InitializeHub(hub, connection);
					object obj = await ExecuteHubMethod(methodExecutor, hub, hubMethodInvocationMessage.Arguments);
					if (isStreamedInvocation)
					{
						if (!TryGetStreamingEnumerator(connection, hubMethodInvocationMessage.InvocationId, descriptor, obj, out var enumerator, out var streamCts))
						{
							Log.InvalidReturnValueFromStreamingMethod(_logger, methodExecutor.MethodInfo.Name);
							await SendInvocationError(hubMethodInvocationMessage.InvocationId, connection, "The value returned by the streaming method '" + methodExecutor.MethodInfo.Name + "' is not a ChannelReader<>.");
							return;
						}
						disposeScope = false;
						Log.StreamingResult(_logger, hubMethodInvocationMessage.InvocationId, methodExecutor);
						StreamResultsAsync(hubMethodInvocationMessage.InvocationId, connection, enumerator, scope, hubActivator, hub, streamCts);
					}
					else if (!string.IsNullOrEmpty(hubMethodInvocationMessage.InvocationId))
					{
						Log.SendingResult(_logger, hubMethodInvocationMessage.InvocationId, methodExecutor);
						await connection.WriteAsync(CompletionMessage.WithResult(hubMethodInvocationMessage.InvocationId, obj));
					}
				}
				catch (TargetInvocationException ex)
				{
					Log.FailedInvokingHubMethod(_logger, hubMethodInvocationMessage.Target, ex);
					await SendInvocationError(hubMethodInvocationMessage.InvocationId, connection, ErrorMessageHelper.BuildErrorMessage("An unexpected error occurred invoking '" + hubMethodInvocationMessage.Target + "' on the server.", ex.InnerException, _enableDetailedErrors));
				}
				catch (Exception exception)
				{
					Log.FailedInvokingHubMethod(_logger, hubMethodInvocationMessage.Target, exception);
					await SendInvocationError(hubMethodInvocationMessage.InvocationId, connection, ErrorMessageHelper.BuildErrorMessage("An unexpected error occurred invoking '" + hubMethodInvocationMessage.Target + "' on the server.", exception, _enableDetailedErrors));
				}
			}
		}
		finally
		{
			if (disposeScope)
			{
				hubActivator?.Release(hub);
				scope.Dispose();
			}
		}
	}

	private async Task StreamResultsAsync(string invocationId, HubConnectionContext connection, IAsyncEnumerator<object> enumerator, IServiceScope scope, IHubActivator<THub> hubActivator, THub hub, CancellationTokenSource streamCts)
	{
		string error = null;
		using (scope)
		{
			try
			{
				_ = 1;
				try
				{
					while (await enumerator.MoveNextAsync())
					{
						await connection.WriteAsync(new StreamItemMessage(invocationId, enumerator.Current));
					}
				}
				catch (ChannelClosedException ex)
				{
					error = ErrorMessageHelper.BuildErrorMessage("An error occurred on the server while streaming results.", ex.InnerException ?? ex, _enableDetailedErrors);
				}
				catch (Exception ex2)
				{
					if (!(ex2 is OperationCanceledException) || !connection.ActiveRequestCancellationSources.TryGetValue(invocationId, out var value) || !value.IsCancellationRequested)
					{
						error = ErrorMessageHelper.BuildErrorMessage("An error occurred on the server while streaming results.", ex2, _enableDetailedErrors);
					}
				}
			}
			finally
			{
				(enumerator as IDisposable)?.Dispose();
				hubActivator.Release(hub);
				streamCts.Dispose();
				await connection.WriteAsync(CompletionMessage.WithError(invocationId, error));
				if (connection.ActiveRequestCancellationSources.TryRemove(invocationId, out var value2))
				{
					value2.Dispose();
				}
			}
		}
	}

	private static async Task<object> ExecuteHubMethod(ObjectMethodExecutor methodExecutor, THub hub, object[] arguments)
	{
		if (methodExecutor.IsMethodAsync)
		{
			if (!(methodExecutor.MethodReturnType == typeof(Task)))
			{
				return await methodExecutor.ExecuteAsync(hub, arguments);
			}
			await (Task)methodExecutor.Execute(hub, arguments);
			return null;
		}
		return methodExecutor.Execute(hub, arguments);
	}

	private async Task SendInvocationError(string invocationId, HubConnectionContext connection, string errorMessage)
	{
		if (!string.IsNullOrEmpty(invocationId))
		{
			await connection.WriteAsync(CompletionMessage.WithError(invocationId, errorMessage));
		}
	}

	private void InitializeHub(THub hub, HubConnectionContext connection)
	{
		hub.Clients = new HubCallerClients(_hubContext.Clients, connection.ConnectionId);
		hub.Context = new DefaultHubCallerContext(connection);
		hub.Groups = _hubContext.Groups;
	}

	private async Task<bool> IsHubMethodAuthorized(IServiceProvider provider, ClaimsPrincipal principal, IList<IAuthorizeData> policies)
	{
		if (!policies.Any())
		{
			return true;
		}
		IAuthorizationService authService = provider.GetRequiredService<IAuthorizationService>();
		return (await authService.AuthorizeAsync(principal, await AuthorizationPolicy.CombineAsync(provider.GetRequiredService<IAuthorizationPolicyProvider>(), policies))).Succeeded;
	}

	private async Task<bool> ValidateInvocationMode(HubMethodDescriptor hubMethodDescriptor, bool isStreamedInvocation, HubMethodInvocationMessage hubMethodInvocationMessage, HubConnectionContext connection)
	{
		if (hubMethodDescriptor.IsStreamable && !isStreamedInvocation)
		{
			if (!string.IsNullOrEmpty(hubMethodInvocationMessage.InvocationId))
			{
				Log.StreamingMethodCalledWithInvoke(_logger, hubMethodInvocationMessage);
				await connection.WriteAsync(CompletionMessage.WithError(hubMethodInvocationMessage.InvocationId, "The client attempted to invoke the streaming '" + hubMethodInvocationMessage.Target + "' method with a non-streaming invocation."));
			}
			return false;
		}
		if (!hubMethodDescriptor.IsStreamable && isStreamedInvocation)
		{
			Log.NonStreamingMethodCalledWithStream(_logger, hubMethodInvocationMessage);
			await connection.WriteAsync(CompletionMessage.WithError(hubMethodInvocationMessage.InvocationId, "The client attempted to invoke the non-streaming '" + hubMethodInvocationMessage.Target + "' method with a streaming invocation."));
			return false;
		}
		return true;
	}

	private bool TryGetStreamingEnumerator(HubConnectionContext connection, string invocationId, HubMethodDescriptor hubMethodDescriptor, object result, out IAsyncEnumerator<object> enumerator, out CancellationTokenSource streamCts)
	{
		if (result != null && hubMethodDescriptor.IsChannel)
		{
			streamCts = CreateCancellation();
			enumerator = hubMethodDescriptor.FromChannel(result, streamCts.Token);
			return true;
		}
		streamCts = null;
		enumerator = null;
		return false;
		CancellationTokenSource CreateCancellation()
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			connection.ActiveRequestCancellationSources.TryAdd(invocationId, cancellationTokenSource);
			return CancellationTokenSource.CreateLinkedTokenSource(connection.ConnectionAborted, cancellationTokenSource.Token);
		}
	}

	private void DiscoverHubMethods()
	{
		Type typeFromHandle = typeof(THub);
		TypeInfo typeInfo = typeFromHandle.GetTypeInfo();
		string name = typeFromHandle.Name;
		foreach (MethodInfo hubMethod in HubReflectionHelper.GetHubMethods(typeFromHandle))
		{
			string text = hubMethod.GetCustomAttribute<HubMethodNameAttribute>()?.Name ?? hubMethod.Name;
			if (_methods.ContainsKey(text))
			{
				throw new NotSupportedException("Duplicate definitions of '" + text + "'. Overloading is not supported.");
			}
			ObjectMethodExecutor methodExecutor = ObjectMethodExecutor.Create(hubMethod, typeInfo);
			IEnumerable<AuthorizeAttribute> customAttributes = hubMethod.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
			_methods[text] = new HubMethodDescriptor(methodExecutor, customAttributes);
			Log.HubMethodBound(_logger, name, text);
		}
	}
}
