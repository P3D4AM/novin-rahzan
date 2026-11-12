using System;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal;

internal class LongPollingTransport : ITransport, IDuplexPipe
{
	private static class Log
	{
		private static readonly LogDefineOptions SkipEnabledCheckLogOptions = new LogDefineOptions
		{
			SkipEnabledCheck = true
		};

		private static readonly Action<ILogger, TransferFormat, Exception> _startTransport = LoggerMessage.Define<TransferFormat>(LogLevel.Information, new EventId(1, "StartTransport"), "Starting transport. Transfer mode: {TransferFormat}.");

		private static readonly Action<ILogger, Exception> _transportStopped = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "TransportStopped"), "Transport stopped.");

		private static readonly Action<ILogger, Exception> _startReceive = LoggerMessage.Define(LogLevel.Debug, new EventId(3, "StartReceive"), "Starting receive loop.");

		private static readonly Action<ILogger, Exception> _receiveStopped = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "ReceiveStopped"), "Receive loop stopped.");

		private static readonly Action<ILogger, Exception> _receiveCanceled = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "ReceiveCanceled"), "Receive loop canceled.");

		private static readonly Action<ILogger, Exception> _transportStopping = LoggerMessage.Define(LogLevel.Information, new EventId(6, "TransportStopping"), "Transport is stopping.");

		private static readonly Action<ILogger, Exception> _closingConnection = LoggerMessage.Define(LogLevel.Debug, new EventId(7, "ClosingConnection"), "The server is closing the connection.");

		private static readonly Action<ILogger, Exception> _receivedMessages = LoggerMessage.Define(LogLevel.Debug, new EventId(8, "ReceivedMessages"), "Received messages from the server.");

		private static readonly Action<ILogger, Uri, Exception> _errorPolling = LoggerMessage.Define<Uri>(LogLevel.Error, new EventId(9, "ErrorPolling"), "Error while polling '{PollUrl}'.");

		private static readonly Action<ILogger, int, long?, Exception> _pollResponseReceived = LoggerMessage.Define<int, long?>(LogLevel.Trace, new EventId(10, "PollResponseReceived"), "Poll response with status code {StatusCode} received from server. Content length: {ContentLength}.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, Uri, Exception> _sendingDeleteRequest = LoggerMessage.Define<Uri>(LogLevel.Debug, new EventId(11, "SendingDeleteRequest"), "Sending DELETE request to '{PollUrl}'.");

		private static readonly Action<ILogger, Uri, Exception> _deleteRequestAccepted = LoggerMessage.Define<Uri>(LogLevel.Debug, new EventId(12, "DeleteRequestAccepted"), "DELETE request to '{PollUrl}' accepted.");

		private static readonly Action<ILogger, Uri, Exception> _errorSendingDeleteRequest = LoggerMessage.Define<Uri>(LogLevel.Error, new EventId(13, "ErrorSendingDeleteRequest"), "Error sending DELETE request to '{PollUrl}'.");

		private static readonly Action<ILogger, Uri, Exception> _connectionAlreadyClosedSendingDeleteRequest = LoggerMessage.Define<Uri>(LogLevel.Debug, new EventId(14, "ConnectionAlreadyClosedSendingDeleteRequest"), "A 404 response was returned from sending DELETE request to '{PollUrl}', likely because the transport was already closed on the server.");

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

		public static void ReceiveCanceled(ILogger logger)
		{
			_receiveCanceled(logger, null);
		}

		public static void ReceiveStopped(ILogger logger)
		{
			_receiveStopped(logger, null);
		}

		public static void ClosingConnection(ILogger logger)
		{
			_closingConnection(logger, null);
		}

		public static void ReceivedMessages(ILogger logger)
		{
			_receivedMessages(logger, null);
		}

		public static void ErrorPolling(ILogger logger, Uri pollUrl, Exception exception)
		{
			_errorPolling(logger, pollUrl, exception);
		}

		public static void PollResponseReceived(ILogger logger, HttpResponseMessage response)
		{
			if (logger.IsEnabled(LogLevel.Trace))
			{
				_pollResponseReceived(logger, (int)response.StatusCode, response.Content.Headers.ContentLength ?? (-1), null);
			}
		}

		public static void SendingDeleteRequest(ILogger logger, Uri pollUrl)
		{
			_sendingDeleteRequest(logger, pollUrl, null);
		}

		public static void DeleteRequestAccepted(ILogger logger, Uri pollUrl)
		{
			_deleteRequestAccepted(logger, pollUrl, null);
		}

		public static void ErrorSendingDeleteRequest(ILogger logger, Uri pollUrl, Exception ex)
		{
			_errorSendingDeleteRequest(logger, pollUrl, ex);
		}

		public static void ConnectionAlreadyClosedSendingDeleteRequest(ILogger logger, Uri pollUrl)
		{
			_connectionAlreadyClosedSendingDeleteRequest(logger, pollUrl, null);
		}
	}

	private readonly HttpClient _httpClient;

	private readonly ILogger _logger;

	private readonly HttpConnectionOptions _httpConnectionOptions;

	private IDuplexPipe _application;

	private IDuplexPipe _transport;

	private volatile Exception _error;

	private readonly CancellationTokenSource _transportCts = new CancellationTokenSource();

	internal Task Running { get; private set; } = Task.CompletedTask;


	public PipeReader Input => _transport.Input;

	public PipeWriter Output => _transport.Output;

	public LongPollingTransport(HttpClient httpClient, HttpConnectionOptions? httpConnectionOptions = null, ILoggerFactory? loggerFactory = null)
	{
		_httpClient = httpClient;
		_logger = (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<LongPollingTransport>();
		_httpConnectionOptions = httpConnectionOptions ?? new HttpConnectionOptions();
	}

	public async Task StartAsync(Uri url, TransferFormat transferFormat, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (transferFormat != TransferFormat.Binary && transferFormat != TransferFormat.Text)
		{
			throw new ArgumentException($"The '{transferFormat}' transfer format is not supported by this transport.", "transferFormat");
		}
		Log.StartTransport(_logger, transferFormat);
		HttpRequestMessage val = new HttpRequestMessage(HttpMethod.Get, url);
		HttpResponseMessage val2 = await ((HttpMessageInvoker)_httpClient).SendAsync(val, cancellationToken);
		try
		{
			val2.EnsureSuccessStatusCode();
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		DuplexPipe.DuplexPipePair duplexPipePair = DuplexPipe.CreateConnectionPair(_httpConnectionOptions.TransportPipeOptions, _httpConnectionOptions.AppPipeOptions);
		_transport = duplexPipePair.Transport;
		_application = duplexPipePair.Application;
		Running = ProcessAsync(url);
	}

	private async Task ProcessAsync(Uri url)
	{
		Task receiving = Poll(url, _transportCts.Token);
		Task sending = SendUtils.SendMessages(url, _application, _httpClient, _logger);
		if (await Task.WhenAny(new Task[2] { receiving, sending }) == receiving)
		{
			_application.Input.CancelPendingRead();
			await sending;
			return;
		}
		_error = (sending.IsFaulted ? sending.Exception.InnerException : null);
		_transportCts.Cancel();
		_application.Output.CancelPendingFlush();
		await receiving;
		await SendDeleteRequest(url);
	}

	public async Task StopAsync()
	{
		Log.TransportStopping(_logger);
		if (_application != null)
		{
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
			_transport.Output.Complete();
			_transport.Input.Complete();
			Log.TransportStopped(_logger, null);
		}
	}

	private async Task Poll(Uri pollUrl, CancellationToken cancellationToken)
	{
		Log.StartReceive(_logger);
		System.IO.Pipelines.PipeWriterStream applicationStream = new System.IO.Pipelines.PipeWriterStream(_application.Output);
		try
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				HttpRequestMessage val = new HttpRequestMessage(HttpMethod.Get, pollUrl);
				HttpResponseMessage val2;
				try
				{
					val2 = await ((HttpMessageInvoker)_httpClient).SendAsync(val, cancellationToken);
				}
				catch (OperationCanceledException)
				{
					continue;
				}
				catch (WebException ex2) when (!OperatingSystem.IsBrowser() && ex2.Status == WebExceptionStatus.RequestCanceled)
				{
					continue;
				}
				Log.PollResponseReceived(_logger, val2);
				val2.EnsureSuccessStatusCode();
				if (val2.StatusCode == HttpStatusCode.NoContent || cancellationToken.IsCancellationRequested)
				{
					Log.ClosingConnection(_logger);
					break;
				}
				Log.ReceivedMessages(_logger);
				await val2.Content.CopyToAsync((Stream)applicationStream);
				FlushResult flushResult = await _application.Output.FlushAsync(cancellationToken);
				if (flushResult.IsCanceled || flushResult.IsCompleted)
				{
					break;
				}
			}
		}
		catch (OperationCanceledException)
		{
			Log.ReceiveCanceled(_logger);
		}
		catch (Exception ex4)
		{
			Log.ErrorPolling(_logger, pollUrl, ex4);
			_error = ex4;
		}
		finally
		{
			_application.Output.Complete(_error);
			Log.ReceiveStopped(_logger);
		}
	}

	private async Task SendDeleteRequest(Uri url)
	{
		try
		{
			Log.SendingDeleteRequest(_logger, url);
			HttpResponseMessage val = await _httpClient.DeleteAsync(url);
			if (val.StatusCode == HttpStatusCode.NotFound)
			{
				Log.ConnectionAlreadyClosedSendingDeleteRequest(_logger, url);
				return;
			}
			val.EnsureSuccessStatusCode();
			Log.DeleteRequestAccepted(_logger, url);
		}
		catch (Exception ex)
		{
			Log.ErrorSendingDeleteRequest(_logger, url, ex);
		}
	}
}
