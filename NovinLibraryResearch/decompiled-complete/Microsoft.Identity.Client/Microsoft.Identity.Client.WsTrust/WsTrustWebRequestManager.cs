using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.WsTrust;

internal class WsTrustWebRequestManager : IWsTrustWebRequestManager
{
	private readonly IHttpManager _httpManager;

	public WsTrustWebRequestManager(IHttpManager httpManager)
	{
		_httpManager = httpManager;
	}

	public async Task<MexDocument> GetMexDocumentAsync(string federationMetadataUrl, RequestContext requestContext)
	{
		IDictionary<string, string> msalIdParameters = MsalIdHelper.GetMsalIdParameters(requestContext.Logger);
		UriBuilder uriBuilder = new UriBuilder(federationMetadataUrl);
		HttpResponse httpResponse = await _httpManager.SendGetAsync(uriBuilder.Uri, msalIdParameters, requestContext.Logger).ConfigureAwait(continueOnCapturedContext: false);
		if (httpResponse.StatusCode != HttpStatusCode.OK)
		{
			string errorMessage = string.Format(CultureInfo.CurrentCulture, "Response status code does not indicate success: {0} ({1}).", (int)httpResponse.StatusCode, httpResponse.StatusCode);
			throw MsalServiceExceptionFactory.FromHttpResponse("accessing_ws_metadata_exchange_failed", errorMessage, httpResponse);
		}
		MexDocument result = new MexDocument(httpResponse.Body);
		requestContext.Logger.InfoPii("MEX document fetched and parsed from '" + federationMetadataUrl + "'", "Fetched and parsed MEX");
		return result;
	}

	public async Task<WsTrustResponse> GetWsTrustResponseAsync(WsTrustEndpoint wsTrustEndpoint, string wsTrustRequest, RequestContext requestContext)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "ContentType", "application/soap+xml" },
			{
				"SOAPAction",
				(wsTrustEndpoint.Version == WsTrustVersion.WsTrust2005) ? ((object)XmlNamespace.Issue2005).ToString() : ((object)XmlNamespace.Issue).ToString()
			}
		};
		StringContent body = new StringContent(wsTrustRequest, Encoding.UTF8, dictionary["ContentType"]);
		HttpResponse httpResponse = await _httpManager.SendPostForceResponseAsync(wsTrustEndpoint.Uri, dictionary, body, requestContext.Logger).ConfigureAwait(continueOnCapturedContext: false);
		if (httpResponse.StatusCode != HttpStatusCode.OK)
		{
			string arg;
			try
			{
				arg = WsTrustResponse.ReadErrorResponse(XDocument.Parse(httpResponse.Body, (LoadOptions)0), requestContext);
			}
			catch (XmlException)
			{
				arg = httpResponse.Body;
			}
			string errorMessage = string.Format(CultureInfo.CurrentCulture, "Federated service at {0} returned error: {1}", wsTrustEndpoint.Uri, arg);
			throw MsalServiceExceptionFactory.FromHttpResponse("federated_service_returned_error", errorMessage, httpResponse);
		}
		try
		{
			return WsTrustResponse.CreateFromResponse(httpResponse.Body, wsTrustEndpoint.Version);
		}
		catch (XmlException innerException)
		{
			string errorMessage2 = string.Format(CultureInfo.CurrentCulture, "Federated service at {0} parse error: Body {1}", wsTrustEndpoint.Uri, httpResponse.Body);
			throw new MsalClientException("parsing_wstrust_response_failed", errorMessage2, innerException);
		}
	}

	public async Task<UserRealmDiscoveryResponse> GetUserRealmAsync(string userRealmUriPrefix, string userName, RequestContext requestContext)
	{
		requestContext.Logger.Info("Sending request to userrealm endpoint.");
		IDictionary<string, string> msalIdParameters = MsalIdHelper.GetMsalIdParameters(requestContext.Logger);
		Uri uri = new UriBuilder(userRealmUriPrefix + userName + "?api-version=1.0").Uri;
		HttpResponse httpResponse = await _httpManager.SendGetAsync(uri, msalIdParameters, requestContext.Logger).ConfigureAwait(continueOnCapturedContext: false);
		return (httpResponse.StatusCode == HttpStatusCode.OK) ? JsonHelper.DeserializeFromJson<UserRealmDiscoveryResponse>(httpResponse.Body) : null;
	}
}
