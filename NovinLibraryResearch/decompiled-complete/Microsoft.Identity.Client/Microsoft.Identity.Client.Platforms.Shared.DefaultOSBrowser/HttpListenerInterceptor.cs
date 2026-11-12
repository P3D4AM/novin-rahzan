using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Platforms.Shared.Desktop.OsBrowser;

namespace Microsoft.Identity.Client.Platforms.Shared.DefaultOSBrowser;

internal class HttpListenerInterceptor : IUriInterceptor
{
	private ICoreLogger _logger;

	public Action TestBeforeTopLevelCall { get; set; }

	public Action TestBeforeStart { get; set; }

	public Action TestBeforeGetContext { get; set; }

	public HttpListenerInterceptor(ICoreLogger logger)
	{
		_logger = logger;
	}

	public async Task<Uri> ListenToSingleRequestAndRespondAsync(int port, Func<Uri, MessageAndHttpCode> responseProducer, CancellationToken cancellationToken)
	{
		TestBeforeTopLevelCall?.Invoke();
		cancellationToken.ThrowIfCancellationRequested();
		HttpListener httpListener = null;
		try
		{
			string urlToListenTo = "http://localhost:" + port + "/";
			httpListener = new HttpListener();
			httpListener.Prefixes.Add(urlToListenTo);
			TestBeforeStart?.Invoke();
			httpListener.Start();
			_logger.Info("Listening for authorization code on " + urlToListenTo);
			using (cancellationToken.Register(delegate
			{
				_logger.Warning("HttpListener stopped because cancellation was requested.");
				TryStopListening(httpListener);
			}))
			{
				TestBeforeGetContext?.Invoke();
				HttpListenerContext httpListenerContext = await httpListener.GetContextAsync().ConfigureAwait(continueOnCapturedContext: false);
				cancellationToken.ThrowIfCancellationRequested();
				Respond(responseProducer, httpListenerContext);
				_logger.Verbose("HttpListner received a message on " + urlToListenTo);
				return httpListenerContext.Request.Url;
			}
		}
		catch (Exception ex) when (ex is HttpListenerException || ex is ObjectDisposedException)
		{
			_logger.Info("HttpListenerException - cancellation requested? " + cancellationToken.IsCancellationRequested);
			cancellationToken.ThrowIfCancellationRequested();
			throw;
		}
		finally
		{
			TryStopListening(httpListener);
		}
	}

	private static void TryStopListening(HttpListener httpListener)
	{
		try
		{
			httpListener?.Abort();
		}
		catch
		{
		}
	}

	private void Respond(Func<Uri, MessageAndHttpCode> responseProducer, HttpListenerContext context)
	{
		MessageAndHttpCode messageAndHttpCode = responseProducer(context.Request.Url);
		_logger.Info("Processing a response message to the browser. HttpStatus:" + messageAndHttpCode.HttpCode);
		switch (messageAndHttpCode.HttpCode)
		{
		case HttpStatusCode.Found:
			context.Response.StatusCode = 302;
			context.Response.RedirectLocation = messageAndHttpCode.Message;
			break;
		case HttpStatusCode.OK:
		{
			byte[] bytes = Encoding.UTF8.GetBytes(messageAndHttpCode.Message);
			context.Response.ContentLength64 = bytes.Length;
			context.Response.OutputStream.Write(bytes, 0, bytes.Length);
			break;
		}
		default:
			throw new NotImplementedException("HttpCode not supported" + messageAndHttpCode.HttpCode);
		}
		context.Response.OutputStream.Close();
	}
}
