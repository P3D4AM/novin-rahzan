using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.TelemetryCore;
using Microsoft.Identity.Client.TelemetryCore.Internal;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.OAuth2;

internal class OAuth2Client
{
	private readonly Dictionary<string, string> _headers;

	private readonly Dictionary<string, string> _queryParameters = new Dictionary<string, string>();

	private readonly IDictionary<string, string> _bodyParameters = new Dictionary<string, string>();

	private readonly IHttpManager _httpManager;

	public OAuth2Client(ICoreLogger logger, IHttpManager httpManager, IMatsTelemetryManager telemetryManager)
	{
		_headers = new Dictionary<string, string>(MsalIdHelper.GetMsalIdParameters(logger));
		_httpManager = httpManager ?? throw new ArgumentNullException("httpManager");
	}

	public void AddQueryParameter(string key, string value)
	{
		if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
		{
			_queryParameters[key] = value;
		}
	}

	public void AddBodyParameter(string key, string value)
	{
		if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
		{
			_bodyParameters[key] = value;
		}
	}

	internal void AddHeader(string key, string value)
	{
		_headers[key] = value;
	}

	internal IReadOnlyDictionary<string, string> GetBodyParameters()
	{
		return new ReadOnlyDictionary<string, string>(_bodyParameters);
	}

	public async Task<InstanceDiscoveryResponse> DiscoverAadInstanceAsync(Uri endPoint, RequestContext requestContext)
	{
		return await ExecuteRequestAsync<InstanceDiscoveryResponse>(endPoint, HttpMethod.Get, requestContext).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal async Task<MsalTokenResponse> GetTokenAsync(Uri endPoint, RequestContext requestContext, bool addCommonHeaders = true)
	{
		return await ExecuteRequestAsync<MsalTokenResponse>(endPoint, HttpMethod.Post, requestContext, expectErrorsOn200OK: false, addCommonHeaders).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal async Task<T> ExecuteRequestAsync<T>(Uri endPoint, HttpMethod method, RequestContext requestContext, bool expectErrorsOn200OK = false, bool addCommonHeaders = true)
	{
		if (addCommonHeaders)
		{
			AddCommonHeaders(requestContext);
		}
		HttpResponse httpResponse = null;
		Uri uri = AddExtraQueryParams(endPoint);
		HttpEvent httpEvent = new HttpEvent(requestContext.CorrelationId.AsMatsCorrelationId())
		{
			HttpPath = uri,
			QueryParams = uri.Query
		};
		using (requestContext.CreateTelemetryHelper(httpEvent))
		{
			httpResponse = ((!(method == HttpMethod.Post)) ? (await _httpManager.SendGetAsync(uri, _headers, requestContext.Logger).ConfigureAwait(continueOnCapturedContext: false)) : (await _httpManager.SendPostAsync(uri, _headers, _bodyParameters, requestContext.Logger).ConfigureAwait(continueOnCapturedContext: false)));
			DecorateHttpEvent(method, requestContext, httpResponse, httpEvent);
			if (httpResponse.StatusCode != HttpStatusCode.OK || expectErrorsOn200OK)
			{
				try
				{
					httpEvent.OauthErrorCode = "unknown_error";
					if (!string.IsNullOrWhiteSpace(httpResponse.Body))
					{
						MsalTokenResponse msalTokenResponse = JsonHelper.DeserializeFromJson<MsalTokenResponse>(httpResponse.Body);
						if (msalTokenResponse != null)
						{
							httpEvent.OauthErrorCode = msalTokenResponse?.Error;
						}
						if (httpResponse.StatusCode == HttpStatusCode.OK && expectErrorsOn200OK && !string.IsNullOrEmpty(msalTokenResponse?.Error))
						{
							ThrowServerException(httpResponse, requestContext);
						}
					}
				}
				catch (JsonException)
				{
				}
			}
		}
		return CreateResponse<T>(httpResponse, requestContext);
	}

	private void AddCommonHeaders(RequestContext requestContext)
	{
		_headers.Add("client-request-id", requestContext.CorrelationId.ToString());
		_headers.Add("return-client-request-id", "true");
		if (!string.IsNullOrWhiteSpace(requestContext.Logger.ClientName))
		{
			_headers.Add("x-app-name", requestContext.Logger.ClientName);
		}
		if (!string.IsNullOrWhiteSpace(requestContext.Logger.ClientVersion))
		{
			_headers.Add("x-app-ver", requestContext.Logger.ClientVersion);
		}
	}

	private void DecorateHttpEvent(HttpMethod method, RequestContext requestContext, HttpResponse response, HttpEvent httpEvent)
	{
		httpEvent.HttpResponseStatus = (int)response.StatusCode;
		httpEvent.UserAgent = response.UserAgent;
		httpEvent.HttpMethod = method.Method;
		IDictionary<string, string> headersAsDictionary = response.HeadersAsDictionary;
		if (headersAsDictionary.ContainsKey("x-ms-request-id") && headersAsDictionary["x-ms-request-id"] != null)
		{
			httpEvent.RequestIdHeader = headersAsDictionary["x-ms-request-id"];
		}
		if (headersAsDictionary.ContainsKey("x-ms-clitelem") && headersAsDictionary["x-ms-clitelem"] != null)
		{
			XmsCliTelemInfo xmsCliTelemInfo = new XmsCliTelemInfoParser().ParseXMsTelemHeader(headersAsDictionary["x-ms-clitelem"], requestContext.Logger);
			if (xmsCliTelemInfo != null)
			{
				httpEvent.TokenAge = xmsCliTelemInfo.TokenAge;
				httpEvent.SpeInfo = xmsCliTelemInfo.SpeInfo;
				httpEvent.ServerErrorCode = xmsCliTelemInfo.ServerErrorCode;
				httpEvent.ServerSubErrorCode = xmsCliTelemInfo.ServerSubErrorCode;
			}
		}
	}

	public static T CreateResponse<T>(HttpResponse response, RequestContext requestContext)
	{
		if (response.StatusCode != HttpStatusCode.OK)
		{
			ThrowServerException(response, requestContext);
		}
		VerifyCorrelationIdHeaderInResponse(response.HeadersAsDictionary, requestContext);
		return JsonHelper.DeserializeFromJson<T>(response.Body);
	}

	private static void ThrowServerException(HttpResponse response, RequestContext requestContext)
	{
		bool shouldLogAsError = true;
		string text = string.Format(CultureInfo.InvariantCulture, "HttpStatusCode: {0}: {1}", (int)response.StatusCode, response.StatusCode.ToString());
		requestContext.Logger.Info(text);
		MsalServiceException ex;
		try
		{
			ex = ExtractErrorsFromTheResponse(response, ref shouldLogAsError);
		}
		catch (JsonException)
		{
			ex = MsalServiceExceptionFactory.FromHttpResponse("non_parsable_oauth_error", "An error response was returned by the OAuth2 server, but it could not be parsed. Please inspect the exception properties for details.", response);
		}
		catch (Exception innerException)
		{
			ex = MsalServiceExceptionFactory.FromHttpResponse("unknown_error", response.Body, response, innerException);
		}
		if (ex == null)
		{
			ex = MsalServiceExceptionFactory.FromHttpResponse((response.StatusCode == HttpStatusCode.NotFound) ? "not_found" : "http_status_not_200", text, response);
		}
		if (shouldLogAsError)
		{
			requestContext.Logger.ErrorPii(ex);
		}
		else
		{
			requestContext.Logger.InfoPii(ex);
		}
		throw ex;
	}

	private static MsalServiceException ExtractErrorsFromTheResponse(HttpResponse response, ref bool shouldLogAsError)
	{
		if (string.IsNullOrWhiteSpace(response.Body))
		{
			return null;
		}
		MsalTokenResponse msalTokenResponse = JsonHelper.DeserializeFromJson<MsalTokenResponse>(response.Body);
		if (msalTokenResponse == null || msalTokenResponse.Error == null)
		{
			return null;
		}
		if (string.Compare(msalTokenResponse.Error, "authorization_pending", StringComparison.OrdinalIgnoreCase) == 0)
		{
			shouldLogAsError = false;
		}
		return MsalServiceExceptionFactory.FromHttpResponse(msalTokenResponse.Error, msalTokenResponse.ErrorDescription, response);
	}

	private Uri AddExtraQueryParams(Uri endPoint)
	{
		UriBuilder uriBuilder = new UriBuilder(endPoint);
		string queryParams = _queryParameters.ToQueryParameter();
		uriBuilder.AppendQueryParameters(queryParams);
		return uriBuilder.Uri;
	}

	private static void VerifyCorrelationIdHeaderInResponse(IDictionary<string, string> headers, RequestContext requestContext)
	{
		foreach (string key in headers.Keys)
		{
			string text = key.Trim();
			if (string.Compare(text, "client-request-id", StringComparison.OrdinalIgnoreCase) == 0)
			{
				string text2 = headers[text].Trim();
				if (string.Compare(text2, requestContext.CorrelationId.ToString(), StringComparison.OrdinalIgnoreCase) != 0)
				{
					requestContext.Logger.WarningPii(string.Format(CultureInfo.InvariantCulture, "Returned correlation id '{0}' does not match the sent correlation id '{1}'", text2, requestContext.CorrelationId), "Returned correlation id does not match the sent correlation id");
				}
				break;
			}
		}
	}
}
