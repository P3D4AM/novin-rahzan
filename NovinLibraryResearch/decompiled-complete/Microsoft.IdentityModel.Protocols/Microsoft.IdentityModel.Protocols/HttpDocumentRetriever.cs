using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols;

public class HttpDocumentRetriever : IDocumentRetriever
{
	private HttpClient _httpClient;

	private static readonly HttpClient _defaultHttpClient = new HttpClient();

	public bool RequireHttps { get; set; } = true;


	public HttpDocumentRetriever()
	{
	}

	public HttpDocumentRetriever(HttpClient httpClient)
	{
		if (httpClient == null)
		{
			throw LogHelper.LogArgumentNullException("httpClient");
		}
		_httpClient = httpClient;
	}

	public async Task<string> GetDocumentAsync(string address, CancellationToken cancel)
	{
		if (string.IsNullOrWhiteSpace(address))
		{
			throw LogHelper.LogArgumentNullException("address");
		}
		if (!Utility.IsHttps(address) && RequireHttps)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX20108: The address specified '{0}' is not valid as per HTTPS scheme. Please specify an https address for security reasons. If you want to test with http address, set the RequireHttps property  on IDocumentRetriever to false.", address), "address"));
		}
		Exception exception;
		try
		{
			LogHelper.LogVerbose("IDX20805: Obtaining information from metadata endpoint: '{0}'.", address);
			HttpClient obj = _httpClient ?? _defaultHttpClient;
			Uri requestUri = new Uri(address, UriKind.RelativeOrAbsolute);
			HttpResponseMessage response = await obj.GetAsync(requestUri, cancel).ConfigureAwait(continueOnCapturedContext: false);
			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (response.IsSuccessStatusCode)
			{
				return text;
			}
			exception = new IOException(LogHelper.FormatInvariant("IDX20807: Unable to retrieve document from: '{0}'. HttpResponseMessage: '{1}', HttpResponseMessage.Content: '{2}'.", address, response, text));
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new IOException(LogHelper.FormatInvariant("IDX20804: Unable to retrieve document from: '{0}'.", address), innerException));
		}
		throw LogHelper.LogExceptionMessage(exception);
	}
}
