using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Serilog.Sinks.Http.HttpClients;

public class JsonHttpClient : IHttpClient, IDisposable
{
	private const string JsonContentType = "application/json";

	private readonly HttpClient httpClient;

	public JsonHttpClient()
		: this(new HttpClient())
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_000b: Expected O, but got Unknown


	public JsonHttpClient(HttpClient httpClient)
	{
		this.httpClient = httpClient ?? throw new ArgumentNullException("httpClient");
	}

	~JsonHttpClient()
	{
		Dispose(disposing: false);
	}

	public virtual void Configure(IConfiguration configuration)
	{
	}

	public virtual async Task<HttpResponseMessage> PostAsync(string requestUri, Stream contentStream)
	{
		StreamContent content = new StreamContent(contentStream);
		try
		{
			((HttpHeaders)((HttpContent)content).Headers).Add("Content-Type", "application/json");
			return await httpClient.PostAsync(requestUri, (HttpContent)(object)content).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			((IDisposable)content)?.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			((HttpMessageInvoker)httpClient).Dispose();
		}
	}
}
