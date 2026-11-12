using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.Http;

internal class HttpManager : IHttpManager
{
	private readonly IMsalHttpClientFactory _httpClientFactory;

	public HttpManager(IMsalHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException("httpClientFactory");
	}

	protected virtual HttpClient GetHttpClient()
	{
		return _httpClientFactory.GetHttpClient();
	}

	public async Task<HttpResponse> SendPostAsync(Uri endpoint, IDictionary<string, string> headers, IDictionary<string, string> bodyParameters, ICoreLogger logger)
	{
		HttpContent body = ((bodyParameters == null) ? null : new FormUrlEncodedContent(bodyParameters));
		return await SendPostAsync(endpoint, headers, body, logger).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<HttpResponse> SendPostAsync(Uri endpoint, IDictionary<string, string> headers, HttpContent body, ICoreLogger logger)
	{
		return await ExecuteWithRetryAsync(endpoint, headers, body, HttpMethod.Post, logger).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<HttpResponse> SendGetAsync(Uri endpoint, IDictionary<string, string> headers, ICoreLogger logger)
	{
		return await ExecuteWithRetryAsync(endpoint, headers, null, HttpMethod.Get, logger).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<HttpResponse> SendPostForceResponseAsync(Uri uri, Dictionary<string, string> headers, StringContent body, ICoreLogger logger)
	{
		return await ExecuteWithRetryAsync(uri, headers, body, HttpMethod.Post, logger, doNotThrow: true).ConfigureAwait(continueOnCapturedContext: false);
	}

	private HttpRequestMessage CreateRequestMessage(Uri endpoint, IDictionary<string, string> headers)
	{
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage
		{
			RequestUri = endpoint
		};
		httpRequestMessage.Headers.Accept.Clear();
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				httpRequestMessage.Headers.Add(header.Key, header.Value);
			}
		}
		return httpRequestMessage;
	}

	private async Task<HttpResponse> ExecuteWithRetryAsync(Uri endpoint, IDictionary<string, string> headers, HttpContent body, HttpMethod method, ICoreLogger logger, bool doNotThrow = false, bool retry = true)
	{
		Exception timeoutException = null;
		bool is5xxError = false;
		HttpResponse response = null;
		bool flag;
		try
		{
			HttpContent body2 = body;
			if (body != null)
			{
				body2 = await CloneHttpContentAsync(body).ConfigureAwait(continueOnCapturedContext: false);
			}
			response = await ExecuteAsync(endpoint, headers, body2, method).ConfigureAwait(continueOnCapturedContext: false);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				return response;
			}
			logger.Info(string.Format(CultureInfo.InvariantCulture, "Response status code does not indicate success: {0} ({1}).", (int)response.StatusCode, response.StatusCode));
			is5xxError = response.StatusCode >= HttpStatusCode.InternalServerError && response.StatusCode < (HttpStatusCode)600;
			flag = is5xxError && !HasRetryAfterHeader(response);
		}
		catch (TaskCanceledException ex)
		{
			logger.Error("The HTTP request failed or it was canceled. " + ex.Message);
			flag = true;
			timeoutException = ex;
		}
		if (flag && retry)
		{
			logger.Info("Retrying one more time..");
			await Task.Delay(TimeSpan.FromSeconds(1.0)).ConfigureAwait(continueOnCapturedContext: false);
			return await ExecuteWithRetryAsync(endpoint, headers, body, method, logger, doNotThrow, retry: false).ConfigureAwait(continueOnCapturedContext: false);
		}
		logger.Warning("Request retry failed.");
		if (timeoutException != null)
		{
			throw new MsalServiceException("request_timeout", "Request to the endpoint timed out.", timeoutException);
		}
		if (doNotThrow)
		{
			return response;
		}
		if (is5xxError)
		{
			throw MsalServiceExceptionFactory.FromHttpResponse("service_not_available", "Service is unavailable to process the request", response);
		}
		return response;
	}

	private static bool HasRetryAfterHeader(HttpResponse response)
	{
		RetryConditionHeaderValue retryConditionHeaderValue = response?.Headers?.RetryAfter;
		if (retryConditionHeaderValue != null)
		{
			if (!retryConditionHeaderValue.Delta.HasValue)
			{
				return retryConditionHeaderValue.Date.HasValue;
			}
			return true;
		}
		return false;
	}

	private async Task<HttpResponse> ExecuteAsync(Uri endpoint, IDictionary<string, string> headers, HttpContent body, HttpMethod method)
	{
		HttpClient httpClient = GetHttpClient();
		using HttpRequestMessage requestMessage = CreateRequestMessage(endpoint, headers);
		requestMessage.Method = method;
		requestMessage.Content = body;
		using HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage).ConfigureAwait(continueOnCapturedContext: false);
		HttpResponse obj = await CreateResponseAsync(responseMessage).ConfigureAwait(continueOnCapturedContext: false);
		obj.UserAgent = requestMessage.Headers.UserAgent.ToString();
		return obj;
	}

	internal static async Task<HttpResponse> CreateResponseAsync(HttpResponseMessage response)
	{
		string text = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
		string body = text;
		return new HttpResponse
		{
			Headers = response.Headers,
			Body = body,
			StatusCode = response.StatusCode
		};
	}

	private async Task<HttpContent> CloneHttpContentAsync(HttpContent httpContent)
	{
		MemoryStream temp = new MemoryStream();
		await httpContent.CopyToAsync(temp).ConfigureAwait(continueOnCapturedContext: false);
		temp.Position = 0L;
		StreamContent streamContent = new StreamContent(temp);
		if (httpContent.Headers != null)
		{
			foreach (KeyValuePair<string, IEnumerable<string>> header in httpContent.Headers)
			{
				streamContent.Headers.Add(header.Key, header.Value);
			}
		}
		return streamContent;
	}
}
