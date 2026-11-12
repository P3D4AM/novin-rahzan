using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Internal;

public class HttpConnectionContext : ConnectionContext, IConnectionIdFeature, IConnectionItemsFeature, IConnectionTransportFeature, IConnectionUserFeature, IConnectionHeartbeatFeature, ITransferFormatFeature, IHttpContextFeature, IHttpTransportFeature, IConnectionInherentKeepAliveFeature
{
	private static class Log
	{
		private static readonly Action<ILogger, string, Exception> _disposingConnection = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(1, "DisposingConnection"), "Disposing connection {TransportConnectionId}.");

		private static readonly Action<ILogger, Exception> _waitingForApplication = LoggerMessage.Define(LogLevel.Trace, new EventId(2, "WaitingForApplication"), "Waiting for application to complete.");

		private static readonly Action<ILogger, Exception> _applicationComplete = LoggerMessage.Define(LogLevel.Trace, new EventId(3, "ApplicationComplete"), "Application complete.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _waitingForTransport = LoggerMessage.Define<HttpTransportType>(LogLevel.Trace, new EventId(4, "WaitingForTransport"), "Waiting for {TransportType} transport to complete.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _transportComplete = LoggerMessage.Define<HttpTransportType>(LogLevel.Trace, new EventId(5, "TransportComplete"), "{TransportType} transport complete.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _shuttingDownTransportAndApplication = LoggerMessage.Define<HttpTransportType>(LogLevel.Trace, new EventId(6, "ShuttingDownTransportAndApplication"), "Shutting down both the application and the {TransportType} transport.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _waitingForTransportAndApplication = LoggerMessage.Define<HttpTransportType>(LogLevel.Trace, new EventId(7, "WaitingForTransportAndApplication"), "Waiting for both the application and {TransportType} transport to complete.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _transportAndApplicationComplete = LoggerMessage.Define<HttpTransportType>(LogLevel.Trace, new EventId(8, "TransportAndApplicationComplete"), "The application and {TransportType} transport are both complete.");

		public static void DisposingConnection(ILogger logger, string connectionId)
		{
			if (logger != null)
			{
				_disposingConnection(logger, connectionId, null);
			}
		}

		public static void WaitingForApplication(ILogger logger)
		{
			if (logger != null)
			{
				_waitingForApplication(logger, null);
			}
		}

		public static void ApplicationComplete(ILogger logger)
		{
			if (logger != null)
			{
				_applicationComplete(logger, null);
			}
		}

		public static void WaitingForTransport(ILogger logger, HttpTransportType transportType)
		{
			if (logger != null)
			{
				_waitingForTransport(logger, transportType, null);
			}
		}

		public static void TransportComplete(ILogger logger, HttpTransportType transportType)
		{
			if (logger != null)
			{
				_transportComplete(logger, transportType, null);
			}
		}

		public static void ShuttingDownTransportAndApplication(ILogger logger, HttpTransportType transportType)
		{
			if (logger != null)
			{
				_shuttingDownTransportAndApplication(logger, transportType, null);
			}
		}

		public static void WaitingForTransportAndApplication(ILogger logger, HttpTransportType transportType)
		{
			if (logger != null)
			{
				_waitingForTransportAndApplication(logger, transportType, null);
			}
		}

		public static void TransportAndApplicationComplete(ILogger logger, HttpTransportType transportType)
		{
			if (logger != null)
			{
				_transportAndApplicationComplete(logger, transportType, null);
			}
		}
	}

	private readonly object _itemsLock = new object();

	private readonly object _heartbeatLock = new object();

	private List<(Action<object> handler, object state)> _heartbeatHandlers;

	private readonly ILogger _logger;

	private System.IO.Pipelines.PipeWriterStream _applicationStream;

	private IDuplexPipe _application;

	private IDictionary<object, object> _items;

	private readonly TaskCompletionSource<object> _disposeTcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

	public CancellationTokenSource Cancellation { get; set; }

	public HttpTransportType TransportType { get; set; }

	public SemaphoreSlim WriteLock { get; } = new SemaphoreSlim(1, 1);


	public SemaphoreSlim StateLock { get; } = new SemaphoreSlim(1, 1);


	internal Task DisposeAndRemoveTask { get; set; }

	public Task TransportTask { get; set; }

	public Task PreviousPollTask { get; set; } = Task.CompletedTask;


	public Task ApplicationTask { get; set; }

	public DateTime LastSeenUtc { get; set; }

	public HttpConnectionStatus Status { get; set; }

	public override string ConnectionId { get; set; }

	public override IFeatureCollection Features { get; }

	public ClaimsPrincipal User { get; set; }

	public bool HasInherentKeepAlive { get; set; }

	public override IDictionary<object, object> Items
	{
		get
		{
			if (_items == null)
			{
				lock (_itemsLock)
				{
					if (_items == null)
					{
						_items = new ConnectionItems(new ConcurrentDictionary<object, object>());
					}
				}
			}
			return _items;
		}
		set
		{
			_items = value ?? throw new ArgumentNullException("value");
		}
	}

	public IDuplexPipe Application
	{
		get
		{
			return _application;
		}
		set
		{
			if (value != null)
			{
				_applicationStream = new System.IO.Pipelines.PipeWriterStream(value.Output);
			}
			else
			{
				_applicationStream = null;
			}
			_application = value;
		}
	}

	internal System.IO.Pipelines.PipeWriterStream ApplicationStream => _applicationStream;

	public override IDuplexPipe Transport { get; set; }

	public TransferFormat SupportedFormats { get; set; }

	public TransferFormat ActiveFormat { get; set; }

	public HttpContext HttpContext { get; set; }

	public HttpConnectionContext(string id, ILogger logger)
	{
		((ConnectionContext)this).ConnectionId = id;
		LastSeenUtc = DateTime.UtcNow;
		SupportedFormats = TransferFormat.Binary | TransferFormat.Text;
		ActiveFormat = TransferFormat.Text;
		_logger = logger;
		Features = new FeatureCollection();
		((ConnectionContext)this).Features.Set((IConnectionUserFeature)this);
		((ConnectionContext)this).Features.Set((IConnectionItemsFeature)this);
		((ConnectionContext)this).Features.Set((IConnectionIdFeature)this);
		((ConnectionContext)this).Features.Set((IConnectionTransportFeature)this);
		((ConnectionContext)this).Features.Set((IConnectionHeartbeatFeature)this);
		((ConnectionContext)this).Features.Set((ITransferFormatFeature)this);
		((ConnectionContext)this).Features.Set((IHttpContextFeature)this);
		((ConnectionContext)this).Features.Set((IHttpTransportFeature)this);
		((ConnectionContext)this).Features.Set((IConnectionInherentKeepAliveFeature)this);
	}

	public HttpConnectionContext(string id, IDuplexPipe transport, IDuplexPipe application, ILogger logger = null)
		: this(id, logger)
	{
		Transport = transport;
		Application = application;
	}

	public void OnHeartbeat(Action<object> action, object state)
	{
		lock (_heartbeatLock)
		{
			if (_heartbeatHandlers == null)
			{
				_heartbeatHandlers = new List<(Action<object>, object)>();
			}
			_heartbeatHandlers.Add((action, state));
		}
	}

	public void TickHeartbeat()
	{
		lock (_heartbeatLock)
		{
			if (_heartbeatHandlers == null)
			{
				return;
			}
			foreach (var (action, obj) in _heartbeatHandlers)
			{
				action(obj);
			}
		}
	}

	public async Task DisposeAsync(bool closeGracefully = false)
	{
		Cancellation?.Dispose();
		await StateLock.WaitAsync();
		Task task;
		try
		{
			if (Status == HttpConnectionStatus.Disposed)
			{
				task = _disposeTcs.Task;
			}
			else
			{
				Status = HttpConnectionStatus.Disposed;
				Log.DisposingConnection(_logger, ((ConnectionContext)this).ConnectionId);
				Task applicationTask = ApplicationTask ?? Task.CompletedTask;
				Task transportTask = TransportTask ?? Task.CompletedTask;
				task = WaitOnTasks(applicationTask, transportTask, closeGracefully);
			}
		}
		finally
		{
			StateLock.Release();
		}
		await task;
	}

	private async Task WaitOnTasks(Task applicationTask, Task transportTask, bool closeGracefully)
	{
		_ = 3;
		try
		{
			if (closeGracefully)
			{
				if (await Task.WhenAny(new Task[2] { applicationTask, transportTask }) == applicationTask)
				{
					Transport?.Output.Complete(applicationTask.Exception?.InnerException);
					Transport?.Input.Complete();
					try
					{
						Log.WaitingForTransport(_logger, TransportType);
						await transportTask;
					}
					finally
					{
						Log.TransportComplete(_logger, TransportType);
						Application?.Output.Complete();
						Application?.Input.Complete();
					}
				}
				else
				{
					Application?.Output.Complete(transportTask.Exception?.InnerException);
					Application?.Input.Complete();
					try
					{
						Log.WaitingForApplication(_logger);
						await applicationTask;
					}
					finally
					{
						Log.ApplicationComplete(_logger);
						Transport?.Output.Complete();
						Transport?.Input.Complete();
					}
				}
			}
			else
			{
				Log.ShuttingDownTransportAndApplication(_logger, TransportType);
				Application?.Output.CancelPendingFlush();
				Transport?.Output.Complete(applicationTask.Exception?.InnerException);
				Application?.Output.Complete(transportTask.Exception?.InnerException);
				try
				{
					Log.WaitingForTransportAndApplication(_logger, TransportType);
					await Task.WhenAll(applicationTask, transportTask);
				}
				finally
				{
					Log.TransportAndApplicationComplete(_logger, TransportType);
					Application?.Input.Complete();
					Transport?.Input.Complete();
				}
			}
			_disposeTcs.TrySetResult(null);
		}
		catch (OperationCanceledException)
		{
			_disposeTcs.TrySetCanceled();
			throw;
		}
		catch (Exception exception)
		{
			_disposeTcs.TrySetException(exception);
			throw;
		}
	}
}
