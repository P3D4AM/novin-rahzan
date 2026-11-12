using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;

namespace Microsoft.AspNetCore.Http.Connections.Client;

public class HttpConnectionOptions
{
	private IDictionary<string, string> _headers;

	private X509CertificateCollection _clientCertificates;

	private CookieContainer _cookies;

	private ICredentials _credentials;

	private IWebProxy _proxy;

	private bool? _useDefaultCredentials;

	private Action<ClientWebSocketOptions> _webSocketConfiguration;

	private PipeOptions _transportPipeOptions;

	private PipeOptions _appPipeOptions;

	private long _transportMaxBufferSize;

	private long _applicationMaxBufferSize;

	private const int DefaultBufferSize = 1048576;

	public Func<HttpMessageHandler, HttpMessageHandler>? HttpMessageHandlerFactory { get; set; }

	public Func<WebSocketConnectionContext, CancellationToken, ValueTask<WebSocket>>? WebSocketFactory { get; set; }

	public IDictionary<string, string> Headers
	{
		get
		{
			return _headers;
		}
		set
		{
			_headers = value ?? throw new ArgumentNullException("value");
		}
	}

	public long TransportMaxBufferSize
	{
		get
		{
			return _transportMaxBufferSize;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			_transportMaxBufferSize = value;
		}
	}

	public long ApplicationMaxBufferSize
	{
		get
		{
			return _applicationMaxBufferSize;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			_applicationMaxBufferSize = value;
		}
	}

	internal PipeOptions TransportPipeOptions => _transportPipeOptions ?? (_transportPipeOptions = new PipeOptions(null, pauseWriterThreshold: TransportMaxBufferSize, resumeWriterThreshold: TransportMaxBufferSize / 2, readerScheduler: PipeScheduler.ThreadPool, writerScheduler: null, minimumSegmentSize: -1, useSynchronizationContext: false));

	internal PipeOptions AppPipeOptions => _appPipeOptions ?? (_appPipeOptions = new PipeOptions(null, pauseWriterThreshold: ApplicationMaxBufferSize, resumeWriterThreshold: ApplicationMaxBufferSize / 2, readerScheduler: PipeScheduler.ThreadPool, writerScheduler: null, minimumSegmentSize: -1, useSynchronizationContext: false));

	[UnsupportedOSPlatform("browser")]
	public X509CertificateCollection? ClientCertificates
	{
		get
		{
			ThrowIfUnsupportedPlatform();
			return _clientCertificates;
		}
		set
		{
			ThrowIfUnsupportedPlatform();
			_clientCertificates = value ?? throw new ArgumentNullException("value");
		}
	}

	[UnsupportedOSPlatform("browser")]
	public CookieContainer Cookies
	{
		get
		{
			ThrowIfUnsupportedPlatform();
			return _cookies;
		}
		set
		{
			ThrowIfUnsupportedPlatform();
			_cookies = value ?? throw new ArgumentNullException("value");
		}
	}

	public Uri? Url { get; set; }

	public HttpTransportType Transports { get; set; }

	public bool SkipNegotiation { get; set; }

	public Func<Task<string?>>? AccessTokenProvider { get; set; }

	public TimeSpan CloseTimeout { get; set; } = TimeSpan.FromSeconds(5.0);


	[UnsupportedOSPlatform("browser")]
	public ICredentials? Credentials
	{
		get
		{
			ThrowIfUnsupportedPlatform();
			return _credentials;
		}
		set
		{
			ThrowIfUnsupportedPlatform();
			_credentials = value;
		}
	}

	[UnsupportedOSPlatform("browser")]
	public IWebProxy? Proxy
	{
		get
		{
			ThrowIfUnsupportedPlatform();
			return _proxy;
		}
		set
		{
			ThrowIfUnsupportedPlatform();
			_proxy = value;
		}
	}

	[UnsupportedOSPlatform("browser")]
	public bool? UseDefaultCredentials
	{
		get
		{
			ThrowIfUnsupportedPlatform();
			return _useDefaultCredentials;
		}
		set
		{
			ThrowIfUnsupportedPlatform();
			_useDefaultCredentials = value;
		}
	}

	public TransferFormat DefaultTransferFormat { get; set; } = TransferFormat.Binary;


	[UnsupportedOSPlatform("browser")]
	public Action<ClientWebSocketOptions>? WebSocketConfiguration
	{
		get
		{
			ThrowIfUnsupportedPlatform();
			return _webSocketConfiguration;
		}
		set
		{
			ThrowIfUnsupportedPlatform();
			_webSocketConfiguration = value;
		}
	}

	public HttpConnectionOptions()
	{
		_headers = new Dictionary<string, string>();
		if (!OperatingSystem.IsBrowser())
		{
			_clientCertificates = new X509CertificateCollection();
		}
		_cookies = new CookieContainer();
		Transports = HttpTransports.All;
		TransportMaxBufferSize = 1048576L;
		ApplicationMaxBufferSize = 1048576L;
	}

	private static void ThrowIfUnsupportedPlatform()
	{
		if (OperatingSystem.IsBrowser())
		{
			throw new PlatformNotSupportedException();
		}
	}
}
