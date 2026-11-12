using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.Connections.Client.Internal;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Http.Connections.Client;

public class HttpConnection : ConnectionContext, IConnectionInherentKeepAliveFeature
{
	private static class Log
	{
		private static readonly LogDefineOptions SkipEnabledCheckLogOptions = new LogDefineOptions
		{
			SkipEnabledCheck = true
		};

		private static readonly Action<ILogger, Exception> _starting = LoggerMessage.Define(LogLevel.Debug, new EventId(1, "Starting"), "Starting HttpConnection.");

		private static readonly Action<ILogger, Exception> _skippingStart = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "SkippingStart"), "Skipping start, connection is already started.");

		private static readonly Action<ILogger, Exception> _started = LoggerMessage.Define(LogLevel.Information, new EventId(3, "Started"), "HttpConnection Started.");

		private static readonly Action<ILogger, Exception> _disposingHttpConnection = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "DisposingHttpConnection"), "Disposing HttpConnection.");

		private static readonly Action<ILogger, Exception> _skippingDispose = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "SkippingDispose"), "Skipping dispose, connection is already disposed.");

		private static readonly Action<ILogger, Exception> _disposed = LoggerMessage.Define(LogLevel.Information, new EventId(6, "Disposed"), "HttpConnection Disposed.");

		private static readonly Action<ILogger, string, Uri, Exception> _startingTransport = LoggerMessage.Define<string, Uri>(LogLevel.Debug, new EventId(7, "StartingTransport"), "Starting transport '{Transport}' with Url: {Url}.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, Uri, Exception> _establishingConnection = LoggerMessage.Define<Uri>(LogLevel.Debug, new EventId(8, "EstablishingConnection"), "Establishing connection with server at '{Url}'.");

		private static readonly Action<ILogger, string, Exception> _connectionEstablished = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(9, "Established"), "Established connection '{ConnectionId}' with the server.");

		private static readonly Action<ILogger, Uri, Exception> _errorWithNegotiation = LoggerMessage.Define<Uri>(LogLevel.Error, new EventId(10, "ErrorWithNegotiation"), "Failed to start connection. Error getting negotiation response from '{Url}'.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _errorStartingTransport = LoggerMessage.Define<HttpTransportType>(LogLevel.Error, new EventId(11, "ErrorStartingTransport"), "Failed to start connection. Error starting transport '{Transport}'.");

		private static readonly Action<ILogger, string, Exception> _transportNotSupported = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(12, "TransportNotSupported"), "Skipping transport {TransportName} because it is not supported by this client.");

		private static readonly Action<ILogger, string, string, Exception> _transportDoesNotSupportTransferFormat = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(13, "TransportDoesNotSupportTransferFormat"), "Skipping transport {TransportName} because it does not support the requested transfer format '{TransferFormat}'.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, string, Exception> _transportDisabledByClient = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(14, "TransportDisabledByClient"), "Skipping transport {TransportName} because it was disabled by the client.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, string, Exception> _transportFailed = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(15, "TransportFailed"), "Skipping transport {TransportName} because it failed to initialize.", SkipEnabledCheckLogOptions);

		private static readonly Action<ILogger, Exception> _webSocketsNotSupportedByOperatingSystem = LoggerMessage.Define(LogLevel.Debug, new EventId(16, "WebSocketsNotSupportedByOperatingSystem"), "Skipping WebSockets because they are not supported by the operating system.");

		private static readonly Action<ILogger, Exception> _transportThrewExceptionOnStop = LoggerMessage.Define(LogLevel.Error, new EventId(17, "TransportThrewExceptionOnStop"), "The transport threw an exception while stopping.");

		private static readonly Action<ILogger, HttpTransportType, Exception> _transportStarted = LoggerMessage.Define<HttpTransportType>(LogLevel.Debug, new EventId(18, "TransportStarted"), "Transport '{Transport}' started.");

		private static readonly Action<ILogger, Exception> _serverSentEventsNotSupportedByBrowser = LoggerMessage.Define(LogLevel.Debug, new EventId(19, "ServerSentEventsNotSupportedByBrowser"), "Skipping ServerSentEvents because they are not supported by the browser.");

		private static readonly Action<ILogger, Exception> _cookiesNotSupported = LoggerMessage.Define(LogLevel.Trace, new EventId(20, "CookiesNotSupported"), "Cookies are not supported on this platform.");

		public static void Starting(ILogger logger)
		{
			_starting(logger, null);
		}

		public static void SkippingStart(ILogger logger)
		{
			_skippingStart(logger, null);
		}

		public static void Started(ILogger logger)
		{
			_started(logger, null);
		}

		public static void DisposingHttpConnection(ILogger logger)
		{
			_disposingHttpConnection(logger, null);
		}

		public static void SkippingDispose(ILogger logger)
		{
			_skippingDispose(logger, null);
		}

		public static void Disposed(ILogger logger)
		{
			_disposed(logger, null);
		}

		public static void StartingTransport(ILogger logger, HttpTransportType transportType, Uri url)
		{
			if (logger.IsEnabled(LogLevel.Debug))
			{
				_startingTransport(logger, transportType.ToString(), url, null);
			}
		}

		public static void EstablishingConnection(ILogger logger, Uri url)
		{
			_establishingConnection(logger, url, null);
		}

		public static void ConnectionEstablished(ILogger logger, string connectionId)
		{
			_connectionEstablished(logger, connectionId, null);
		}

		public static void ErrorWithNegotiation(ILogger logger, Uri url, Exception exception)
		{
			_errorWithNegotiation(logger, url, exception);
		}

		public static void ErrorStartingTransport(ILogger logger, HttpTransportType transportType, Exception exception)
		{
			_errorStartingTransport(logger, transportType, exception);
		}

		public static void TransportNotSupported(ILogger logger, string transport)
		{
			_transportNotSupported(logger, transport, null);
		}

		public static void TransportDoesNotSupportTransferFormat(ILogger logger, HttpTransportType transport, TransferFormat transferFormat)
		{
			if (logger.IsEnabled(LogLevel.Debug))
			{
				_transportDoesNotSupportTransferFormat(logger, transport.ToString(), transferFormat.ToString(), null);
			}
		}

		public static void TransportDisabledByClient(ILogger logger, HttpTransportType transport)
		{
			if (logger.IsEnabled(LogLevel.Debug))
			{
				_transportDisabledByClient(logger, transport.ToString(), null);
			}
		}

		public static void TransportFailed(ILogger logger, HttpTransportType transport, Exception ex)
		{
			if (logger.IsEnabled(LogLevel.Debug))
			{
				_transportFailed(logger, transport.ToString(), ex);
			}
		}

		public static void WebSocketsNotSupportedByOperatingSystem(ILogger logger)
		{
			_webSocketsNotSupportedByOperatingSystem(logger, null);
		}

		public static void TransportThrewExceptionOnStop(ILogger logger, Exception ex)
		{
			_transportThrewExceptionOnStop(logger, ex);
		}

		public static void TransportStarted(ILogger logger, HttpTransportType transportType)
		{
			_transportStarted(logger, transportType, null);
		}

		public static void ServerSentEventsNotSupportedByBrowser(ILogger logger)
		{
			_serverSentEventsNotSupportedByBrowser(logger, null);
		}

		public static void CookiesNotSupported(ILogger logger)
		{
			_cookiesNotSupported(logger, null);
		}
	}

	private const int _maxRedirects = 100;

	private const int _protocolVersionNumber = 1;

	private static readonly Task<string> _noAccessToken = Task.FromResult<string>(null);

	private static readonly TimeSpan HttpClientTimeout = TimeSpan.FromSeconds(120.0);

	private readonly ILogger _logger;

	private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

	private bool _started;

	private bool _disposed;

	private bool _hasInherentKeepAlive;

	private readonly HttpClient _httpClient;

	private readonly HttpConnectionOptions _httpConnectionOptions;

	private ITransport _transport;

	private readonly ITransportFactory _transportFactory;

	private string _connectionId;

	private readonly ConnectionLogScope _logScope;

	private readonly ILoggerFactory _loggerFactory;

	private readonly Uri _url;

	private Func<Task<string>> _accessTokenProvider;

	public override IDuplexPipe Transport
	{
		get
		{
			CheckDisposed();
			if (_transport == null)
			{
				throw new InvalidOperationException("Cannot access the Transport pipe before the connection has started.");
			}
			return _transport;
		}
		set
		{
			throw new NotSupportedException("The transport pipe isn't settable.");
		}
	}

	public override IFeatureCollection Features { get; } = new FeatureCollection();


	public override string? ConnectionId
	{
		get
		{
			return _connectionId;
		}
		set
		{
			throw new InvalidOperationException("The ConnectionId is set internally and should not be set by user code.");
		}
	}

	public override IDictionary<object, object?> Items { get; set; } = new ConnectionItems();


	bool IConnectionInherentKeepAliveFeature.HasInherentKeepAlive => _hasInherentKeepAlive;

	public HttpConnection(Uri url)
		: this(url, HttpTransports.All)
	{
	}

	public HttpConnection(Uri url, HttpTransportType transports)
		: this(url, transports, null)
	{
	}

	public HttpConnection(Uri url, HttpTransportType transports, ILoggerFactory? loggerFactory)
		: this(CreateHttpOptions(url, transports), loggerFactory)
	{
	}

	private static HttpConnectionOptions CreateHttpOptions(Uri url, HttpTransportType transports)
	{
		if (url == null)
		{
			throw new ArgumentNullException("url");
		}
		return new HttpConnectionOptions
		{
			Url = url,
			Transports = transports
		};
	}

	public HttpConnection(HttpConnectionOptions httpConnectionOptions, ILoggerFactory? loggerFactory)
	{
		if (httpConnectionOptions == null)
		{
			throw new ArgumentNullException("httpConnectionOptions");
		}
		if (httpConnectionOptions.Url == null)
		{
			throw new ArgumentException("Options does not have a URL specified.", "httpConnectionOptions");
		}
		_loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
		_logger = _loggerFactory.CreateLogger<HttpConnection>();
		_httpConnectionOptions = httpConnectionOptions;
		_url = _httpConnectionOptions.Url;
		if (!httpConnectionOptions.SkipNegotiation || httpConnectionOptions.Transports != HttpTransportType.WebSockets)
		{
			_httpClient = CreateHttpClient();
		}
		if (httpConnectionOptions.Transports == HttpTransportType.ServerSentEvents && OperatingSystem.IsBrowser())
		{
			throw new ArgumentException("ServerSentEvents can not be the only transport specified when running in the browser.", "httpConnectionOptions");
		}
		_transportFactory = new DefaultTransportFactory(httpConnectionOptions.Transports, _loggerFactory, _httpClient, httpConnectionOptions, GetAccessTokenAsync);
		_logScope = new ConnectionLogScope();
		Features.Set((IConnectionInherentKeepAliveFeature?)this);
	}

	internal HttpConnection(HttpConnectionOptions httpConnectionOptions, ILoggerFactory loggerFactory, ITransportFactory transportFactory)
		: this(httpConnectionOptions, loggerFactory)
	{
		_transportFactory = transportFactory;
	}

	public Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return StartAsync(_httpConnectionOptions.DefaultTransferFormat, cancellationToken);
	}

	public async Task StartAsync(TransferFormat transferFormat, CancellationToken cancellationToken = default(CancellationToken))
	{
		using (_logger.BeginScope(_logScope))
		{
			await StartAsyncCore(transferFormat, cancellationToken).ForceAsync();
		}
	}

	private async Task StartAsyncCore(TransferFormat transferFormat, CancellationToken cancellationToken)
	{
		CheckDisposed();
		if (_started)
		{
			Log.SkippingStart(_logger);
			return;
		}
		await _connectionLock.WaitAsync(cancellationToken);
		try
		{
			CheckDisposed();
			if (_started)
			{
				Log.SkippingStart(_logger);
				return;
			}
			Log.Starting(_logger);
			await SelectAndStartTransport(transferFormat, cancellationToken);
			_started = true;
			Log.Started(_logger);
		}
		finally
		{
			_connectionLock.Release();
		}
	}

	public override async ValueTask DisposeAsync()
	{
		using (_logger.BeginScope(_logScope))
		{
			await DisposeAsyncCore().ForceAsync();
		}
	}

	private async Task DisposeAsyncCore()
	{
		if (_disposed)
		{
			return;
		}
		await _connectionLock.WaitAsync();
		try
		{
			if (!_disposed && _started)
			{
				Log.DisposingHttpConnection(_logger);
				try
				{
					await _transport.StopAsync();
				}
				catch (Exception ex)
				{
					Log.TransportThrewExceptionOnStop(_logger, ex);
				}
				Log.Disposed(_logger);
			}
			else
			{
				Log.SkippingDispose(_logger);
			}
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}
		finally
		{
			if (!_disposed)
			{
				_disposed = true;
			}
			_connectionLock.Release();
		}
	}

	private async Task SelectAndStartTransport(TransferFormat transferFormat, CancellationToken cancellationToken)
	{
		Uri uri = _url;
		_accessTokenProvider = _httpConnectionOptions.AccessTokenProvider;
		List<Exception> transportExceptions = new List<Exception>();
		if (_httpConnectionOptions.SkipNegotiation)
		{
			if (_httpConnectionOptions.Transports != HttpTransportType.WebSockets)
			{
				throw new InvalidOperationException("Negotiation can only be skipped when using the WebSocket transport directly.");
			}
			Log.StartingTransport(_logger, _httpConnectionOptions.Transports, uri);
			await StartTransport(uri, _httpConnectionOptions.Transports, transferFormat, cancellationToken);
		}
		else
		{
			int redirects = 0;
			NegotiationResponse negotiationResponse;
			do
			{
				negotiationResponse = await GetNegotiationResponseAsync(uri, cancellationToken);
				if (negotiationResponse.Url != null)
				{
					uri = new Uri(negotiationResponse.Url);
				}
				if (negotiationResponse.AccessToken != null)
				{
					string accessToken = negotiationResponse.AccessToken;
					_accessTokenProvider = () => Task.FromResult(accessToken);
				}
				redirects++;
			}
			while (negotiationResponse.Url != null && redirects < 100);
			if (redirects == 100 && negotiationResponse.Url != null)
			{
				throw new InvalidOperationException("Negotiate redirection limit exceeded.");
			}
			Uri connectUrl = CreateConnectUrl(uri, negotiationResponse.ConnectionToken);
			string transferFormatString = transferFormat.ToString();
			foreach (AvailableTransport availableTransport in negotiationResponse.AvailableTransports)
			{
				if (!Enum.TryParse<HttpTransportType>(availableTransport.Transport, out var transportType))
				{
					Log.TransportNotSupported(_logger, availableTransport.Transport);
					transportExceptions.Add(new TransportFailedException(availableTransport.Transport, "The transport is not supported by the client."));
					continue;
				}
				if (transportType == HttpTransportType.WebSockets && !IsWebSocketsSupported())
				{
					Log.WebSocketsNotSupportedByOperatingSystem(_logger);
					transportExceptions.Add(new TransportFailedException("WebSockets", "The transport is not supported on this operating system."));
					continue;
				}
				if (transportType == HttpTransportType.ServerSentEvents && OperatingSystem.IsBrowser())
				{
					Log.ServerSentEventsNotSupportedByBrowser(_logger);
					transportExceptions.Add(new TransportFailedException("ServerSentEvents", "The transport is not supported in the browser."));
					continue;
				}
				try
				{
					if ((transportType & _httpConnectionOptions.Transports) == 0)
					{
						Log.TransportDisabledByClient(_logger, transportType);
						transportExceptions.Add(new TransportFailedException(transportType.ToString(), "The transport is disabled by the client."));
						continue;
					}
					if (!availableTransport.TransferFormats.Contains<string>(transferFormatString, StringComparer.Ordinal))
					{
						Log.TransportDoesNotSupportTransferFormat(_logger, transportType, transferFormat);
						transportExceptions.Add(new TransportFailedException(transportType.ToString(), $"The transport does not support the '{transferFormat}' transfer format."));
						continue;
					}
					if (negotiationResponse == null)
					{
						connectUrl = CreateConnectUrl(uri, (await GetNegotiationResponseAsync(uri, cancellationToken)).ConnectionToken);
					}
					Log.StartingTransport(_logger, transportType, uri);
					await StartTransport(connectUrl, transportType, transferFormat, cancellationToken);
				}
				catch (Exception ex)
				{
					Log.TransportFailed(_logger, transportType, ex);
					transportExceptions.Add(new TransportFailedException(transportType.ToString(), ex.Message, ex));
					negotiationResponse = null;
					continue;
				}
				break;
			}
		}
		if (_transport == null)
		{
			if (transportExceptions.Count > 0)
			{
				throw new AggregateException("Unable to connect to the server with any of the available transports.", transportExceptions);
			}
			throw new NoTransportSupportedException("None of the transports supported by the client are supported by the server.");
		}
	}

	private async Task<NegotiationResponse> NegotiateAsync(Uri url, HttpClient httpClient, ILogger logger, CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			Log.EstablishingConnection(logger, url);
			UriBuilder uriBuilder = new UriBuilder(url);
			if (!uriBuilder.Path.EndsWith("/", StringComparison.Ordinal))
			{
				uriBuilder.Path += "/";
			}
			uriBuilder.Path += "negotiate";
			Uri uri = ((!uriBuilder.Query.Contains("negotiateVersion")) ? Utils.AppendQueryString(uriBuilder.Uri, $"negotiateVersion={1}") : uriBuilder.Uri);
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
			try
			{
				request.Version = new Version(1, 1);
				HttpResponseMessage response = await httpClient.SendAsync(request, (HttpCompletionOption)1, cancellationToken);
				try
				{
					response.EnsureSuccessStatusCode();
					NegotiationResponse negotiationResponse = NegotiateProtocol.ParseResponse(await response.Content.ReadAsByteArrayAsync());
					if (!string.IsNullOrEmpty(negotiationResponse.Error))
					{
						throw new Exception(negotiationResponse.Error);
					}
					Log.ConnectionEstablished(_logger, negotiationResponse.ConnectionId);
					return negotiationResponse;
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)request)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			Log.ErrorWithNegotiation(logger, url, exception);
			throw;
		}
	}

	private static Uri CreateConnectUrl(Uri url, string connectionId)
	{
		if (string.IsNullOrWhiteSpace(connectionId))
		{
			throw new FormatException("Invalid connection id.");
		}
		return Utils.AppendQueryString(url, "id=" + connectionId);
	}

	private async Task StartTransport(Uri connectUrl, HttpTransportType transportType, TransferFormat transferFormat, CancellationToken cancellationToken)
	{
		ITransport transport = _transportFactory.CreateTransport(transportType);
		try
		{
			await transport.StartAsync(connectUrl, transferFormat, cancellationToken);
		}
		catch (Exception exception)
		{
			Log.ErrorStartingTransport(_logger, transportType, exception);
			_transport = null;
			throw;
		}
		_hasInherentKeepAlive = transportType == HttpTransportType.LongPolling;
		_transport = transport;
		Log.TransportStarted(_logger, transportType);
	}

	private HttpClient CreateHttpClient()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		HttpClientHandler val = new HttpClientHandler();
		HttpMessageHandler inner = (HttpMessageHandler)(object)val;
		bool flag = OperatingSystem.IsBrowser();
		if (_httpConnectionOptions != null)
		{
			if (!flag)
			{
				if (_httpConnectionOptions.Proxy != null)
				{
					val.Proxy = _httpConnectionOptions.Proxy;
				}
				try
				{
					val.CookieContainer = _httpConnectionOptions.Cookies;
				}
				catch (Exception ex) when (ex is NotSupportedException || ex is NotImplementedException)
				{
					Log.CookiesNotSupported(_logger);
				}
				X509CertificateCollection clientCertificates = _httpConnectionOptions.ClientCertificates;
				if (clientCertificates != null && clientCertificates.Count > 0)
				{
					val.ClientCertificates.AddRange(clientCertificates);
				}
				if (_httpConnectionOptions.UseDefaultCredentials.HasValue)
				{
					val.UseDefaultCredentials = _httpConnectionOptions.UseDefaultCredentials.Value;
				}
				if (_httpConnectionOptions.Credentials != null)
				{
					val.Credentials = _httpConnectionOptions.Credentials;
				}
			}
			inner = (HttpMessageHandler)(object)val;
			if (_httpConnectionOptions.HttpMessageHandlerFactory != null)
			{
				inner = _httpConnectionOptions.HttpMessageHandlerFactory((HttpMessageHandler)(object)val);
				if (inner == null)
				{
					throw new InvalidOperationException("Configured HttpMessageHandlerFactory did not return a value.");
				}
			}
			inner = (HttpMessageHandler)(object)new AccessTokenHttpMessageHandler(inner, this);
		}
		inner = (HttpMessageHandler)(object)new LoggingHttpMessageHandler(inner, _loggerFactory);
		HttpClient val2 = new HttpClient(inner);
		val2.Timeout = HttpClientTimeout;
		bool flag2 = false;
		if (_httpConnectionOptions?.Headers != null)
		{
			foreach (KeyValuePair<string, string> header in _httpConnectionOptions.Headers)
			{
				if (string.Equals(header.Key, Constants.UserAgent, StringComparison.OrdinalIgnoreCase))
				{
					flag2 = true;
					if (string.IsNullOrEmpty(header.Value))
					{
						((HttpHeaders)val2.DefaultRequestHeaders).Remove(header.Key);
					}
					else if (((HttpHeaders)val2.DefaultRequestHeaders).Contains(header.Key))
					{
						((HttpHeaders)val2.DefaultRequestHeaders).Remove(header.Key);
						((HttpHeaders)val2.DefaultRequestHeaders).Add(header.Key, header.Value);
					}
					else
					{
						((HttpHeaders)val2.DefaultRequestHeaders).Add(header.Key, header.Value);
					}
				}
				else
				{
					((HttpHeaders)val2.DefaultRequestHeaders).Add(header.Key, header.Value);
				}
			}
		}
		if (!flag2)
		{
			((HttpHeaders)val2.DefaultRequestHeaders).Add(Constants.UserAgent, Constants.UserAgentHeader);
		}
		((HttpHeaders)val2.DefaultRequestHeaders).Remove("X-Requested-With");
		((HttpHeaders)val2.DefaultRequestHeaders).Add("X-Requested-With", "XMLHttpRequest");
		return val2;
	}

	internal Task<string?> GetAccessTokenAsync()
	{
		if (_accessTokenProvider == null)
		{
			return _noAccessToken;
		}
		return _accessTokenProvider();
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("HttpConnection");
		}
	}

	private static bool IsWebSocketsSupported()
	{
		try
		{
			new ClientWebSocket().Dispose();
			return true;
		}
		catch
		{
			return false;
		}
	}

	private async Task<NegotiationResponse> GetNegotiationResponseAsync(Uri uri, CancellationToken cancellationToken)
	{
		NegotiationResponse negotiationResponse = await NegotiateAsync(uri, _httpClient, _logger, cancellationToken);
		_connectionId = negotiationResponse.ConnectionId;
		if (negotiationResponse.Version == 0)
		{
			negotiationResponse.ConnectionToken = _connectionId;
		}
		_logScope.ConnectionId = _connectionId;
		return negotiationResponse;
	}
}
