using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal;

internal class LoggingHttpMessageHandler : DelegatingHandler
{
	private static class Log
	{
		private static readonly Action<ILogger, HttpMethod, Uri, Exception> _sendingHttpRequest = LoggerMessage.Define<HttpMethod, Uri>(LogLevel.Trace, new EventId(1, "SendingHttpRequest"), "Sending HTTP request {RequestMethod} '{RequestUrl}'.");

		private static readonly Action<ILogger, int, HttpMethod, Uri, Exception> _unsuccessfulHttpResponse = LoggerMessage.Define<int, HttpMethod, Uri>(LogLevel.Warning, new EventId(2, "UnsuccessfulHttpResponse"), "Unsuccessful HTTP response {StatusCode} return from {RequestMethod} '{RequestUrl}'.");

		public static void SendingHttpRequest(ILogger logger, HttpMethod requestMethod, Uri requestUrl)
		{
			_sendingHttpRequest(logger, requestMethod, requestUrl, null);
		}

		public static void UnsuccessfulHttpResponse(ILogger logger, HttpStatusCode statusCode, HttpMethod requestMethod, Uri requestUrl)
		{
			_unsuccessfulHttpResponse(logger, (int)statusCode, requestMethod, requestUrl, null);
		}
	}

	private readonly ILogger<LoggingHttpMessageHandler> _logger;

	public LoggingHttpMessageHandler(HttpMessageHandler inner, ILoggerFactory loggerFactory)
		: base(inner)
	{
		if (loggerFactory == null)
		{
			throw new ArgumentNullException("loggerFactory");
		}
		_logger = loggerFactory.CreateLogger<LoggingHttpMessageHandler>();
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		Log.SendingHttpRequest(_logger, request.Method, request.RequestUri);
		HttpResponseMessage val = await _003C_003En__0(request, cancellationToken);
		if (!val.IsSuccessStatusCode)
		{
			Log.UnsuccessfulHttpResponse(_logger, val.StatusCode, request.Method, request.RequestUri);
		}
		return val;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _003C_003En__0(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return ((DelegatingHandler)this).SendAsync(request, cancellationToken);
	}
}
