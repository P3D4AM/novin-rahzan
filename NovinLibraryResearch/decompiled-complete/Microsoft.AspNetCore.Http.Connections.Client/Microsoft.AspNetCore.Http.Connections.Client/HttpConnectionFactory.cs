using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Http.Connections.Client;

public class HttpConnectionFactory : IConnectionFactory
{
	private readonly HttpConnectionOptions _httpConnectionOptions;

	private readonly ILoggerFactory _loggerFactory;

	public HttpConnectionFactory(IOptions<HttpConnectionOptions> options, ILoggerFactory loggerFactory)
	{
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		_httpConnectionOptions = options.Value;
		_loggerFactory = loggerFactory ?? throw new ArgumentNullException("loggerFactory");
	}

	public async ValueTask<ConnectionContext> ConnectAsync(EndPoint endPoint, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (endPoint == null)
		{
			throw new ArgumentNullException("endPoint");
		}
		if (!(endPoint is UriEndPoint uriEndPoint))
		{
			throw new NotSupportedException("The provided EndPoint must be of type UriEndPoint.");
		}
		if (_httpConnectionOptions.Url != null && _httpConnectionOptions.Url != uriEndPoint.Uri)
		{
			throw new InvalidOperationException("If HttpConnectionOptions.Url was set, it must match the UriEndPoint.Uri passed to ConnectAsync.");
		}
		HttpConnectionOptions httpConnectionOptions = ShallowCopyHttpConnectionOptions(_httpConnectionOptions);
		httpConnectionOptions.Url = uriEndPoint.Uri;
		HttpConnection connection = new HttpConnection(httpConnectionOptions, _loggerFactory);
		try
		{
			await connection.StartAsync(cancellationToken);
			return connection;
		}
		catch
		{
			await connection.DisposeAsync();
			throw;
		}
	}

	internal static HttpConnectionOptions ShallowCopyHttpConnectionOptions(HttpConnectionOptions options)
	{
		HttpConnectionOptions httpConnectionOptions = new HttpConnectionOptions
		{
			HttpMessageHandlerFactory = options.HttpMessageHandlerFactory,
			Headers = options.Headers,
			Url = options.Url,
			Transports = options.Transports,
			SkipNegotiation = options.SkipNegotiation,
			AccessTokenProvider = options.AccessTokenProvider,
			CloseTimeout = options.CloseTimeout,
			DefaultTransferFormat = options.DefaultTransferFormat,
			ApplicationMaxBufferSize = options.ApplicationMaxBufferSize,
			TransportMaxBufferSize = options.TransportMaxBufferSize
		};
		if (!OperatingSystem.IsBrowser())
		{
			httpConnectionOptions.Cookies = options.Cookies;
			httpConnectionOptions.ClientCertificates = options.ClientCertificates;
			httpConnectionOptions.Credentials = options.Credentials;
			httpConnectionOptions.Proxy = options.Proxy;
			httpConnectionOptions.UseDefaultCredentials = options.UseDefaultCredentials;
			httpConnectionOptions.WebSocketConfiguration = options.WebSocketConfiguration;
			httpConnectionOptions.WebSocketFactory = options.WebSocketFactory;
		}
		return httpConnectionOptions;
	}
}
