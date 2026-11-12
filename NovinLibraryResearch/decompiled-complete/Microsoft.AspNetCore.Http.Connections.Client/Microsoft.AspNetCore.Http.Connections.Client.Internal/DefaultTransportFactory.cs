using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal;

internal class DefaultTransportFactory : ITransportFactory
{
	private static class Log
	{
		private static readonly Action<ILogger, HttpTransportType, Exception> _transportNotSupported = LoggerMessage.Define<HttpTransportType>(LogLevel.Debug, new EventId(1, "TransportNotSupported"), "Transport '{TransportType}' is not supported.");

		public static void TransportNotSupported(ILogger logger, HttpTransportType transportType, Exception ex)
		{
			_transportNotSupported(logger, transportType, ex);
		}
	}

	private readonly HttpClient _httpClient;

	private readonly HttpConnectionOptions _httpConnectionOptions;

	private readonly Func<Task<string>> _accessTokenProvider;

	private readonly HttpTransportType _requestedTransportType;

	private readonly ILoggerFactory _loggerFactory;

	private static volatile bool _websocketsSupported = true;

	public DefaultTransportFactory(HttpTransportType requestedTransportType, ILoggerFactory loggerFactory, HttpClient? httpClient, HttpConnectionOptions httpConnectionOptions, Func<Task<string?>> accessTokenProvider)
	{
		if (httpClient == null && requestedTransportType != HttpTransportType.WebSockets)
		{
			throw new ArgumentNullException("httpClient");
		}
		_requestedTransportType = requestedTransportType;
		_loggerFactory = loggerFactory;
		_httpClient = httpClient;
		_httpConnectionOptions = httpConnectionOptions;
		_accessTokenProvider = accessTokenProvider;
	}

	public ITransport CreateTransport(HttpTransportType availableServerTransports)
	{
		if (_websocketsSupported && (availableServerTransports & HttpTransportType.WebSockets & _requestedTransportType) == HttpTransportType.WebSockets)
		{
			try
			{
				return new WebSocketsTransport(_httpConnectionOptions, _loggerFactory, _accessTokenProvider);
			}
			catch (PlatformNotSupportedException ex)
			{
				Log.TransportNotSupported(_loggerFactory.CreateLogger<DefaultTransportFactory>(), HttpTransportType.WebSockets, ex);
				_websocketsSupported = false;
			}
		}
		if ((availableServerTransports & HttpTransportType.ServerSentEvents & _requestedTransportType) == HttpTransportType.ServerSentEvents)
		{
			return new ServerSentEventsTransport(_httpClient, _httpConnectionOptions, _loggerFactory);
		}
		if ((availableServerTransports & HttpTransportType.LongPolling & _requestedTransportType) == HttpTransportType.LongPolling)
		{
			return new LongPollingTransport(_httpClient, _httpConnectionOptions, _loggerFactory);
		}
		throw new InvalidOperationException("No requested transports available on the server.");
	}
}
