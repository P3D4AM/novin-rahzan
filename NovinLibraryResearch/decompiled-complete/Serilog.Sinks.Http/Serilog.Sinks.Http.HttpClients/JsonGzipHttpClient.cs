using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Serilog.Sinks.Http.HttpClients;

public class JsonGzipHttpClient : IHttpClient, IDisposable
{
	private const string JsonContentType = "application/json";

	private const string GzipContentEncoding = "gzip";

	private readonly HttpClient httpClient;

	protected CompressionLevel CompressionLevel { get; set; }

	public JsonGzipHttpClient()
		: this(CompressionLevel.Fastest)
	{
	}

	public JsonGzipHttpClient(CompressionLevel compressionLevel)
		: this(new HttpClient(), compressionLevel)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Expected O, but got Unknown


	public JsonGzipHttpClient(HttpClient httpClient, CompressionLevel compressionLevel)
	{
		this.httpClient = httpClient ?? throw new ArgumentNullException("httpClient");
		CompressionLevel = compressionLevel;
	}

	~JsonGzipHttpClient()
	{
		Dispose(disposing: false);
	}

	public virtual void Configure(IConfiguration configuration)
	{
	}

	public virtual async Task<HttpResponseMessage> PostAsync(string requestUri, Stream contentStream)
	{
		using MemoryStream output = new MemoryStream();
		using (GZipStream gzipStream = new GZipStream(output, CompressionLevel, leaveOpen: true))
		{
			await contentStream.CopyToAsync(gzipStream).ConfigureAwait(continueOnCapturedContext: false);
		}
		output.Position = 0L;
		StreamContent val = new StreamContent((Stream)output);
		((HttpHeaders)((HttpContent)val).Headers).Add("Content-Type", "application/json");
		((HttpHeaders)((HttpContent)val).Headers).Add("Content-Encoding", "gzip");
		return await httpClient.PostAsync(requestUri, (HttpContent)(object)val).ConfigureAwait(continueOnCapturedContext: false);
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
