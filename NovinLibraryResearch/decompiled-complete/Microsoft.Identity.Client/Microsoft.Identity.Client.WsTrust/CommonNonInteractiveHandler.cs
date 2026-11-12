using System;
using System.Globalization;
using System.Security;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.WsTrust;

internal class CommonNonInteractiveHandler
{
	private readonly RequestContext _requestContext;

	private readonly IServiceBundle _serviceBundle;

	public CommonNonInteractiveHandler(RequestContext requestContext, IServiceBundle serviceBundle)
	{
		_requestContext = requestContext;
		_serviceBundle = serviceBundle;
	}

	public async Task<string> GetPlatformUserAsync()
	{
		string text = await _serviceBundle.PlatformProxy.GetUserPrincipalNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (string.IsNullOrWhiteSpace(text))
		{
			_requestContext.Logger.Error("Could not find UPN for logged in user.");
			throw new MsalClientException("unknown_user", "Could not identify the user logged into the OS. See http://aka.ms/msal-net-iwa for details.");
		}
		_requestContext.Logger.InfoPii("Logged in user detected with user name '" + text + "'", "Logged in user detected");
		return text;
	}

	public async Task<UserRealmDiscoveryResponse> QueryUserRealmDataAsync(string userRealmUriPrefix, string username)
	{
		UserRealmDiscoveryResponse userRealmDiscoveryResponse = await _serviceBundle.WsTrustWebRequestManager.GetUserRealmAsync(userRealmUriPrefix, username, _requestContext).ConfigureAwait(continueOnCapturedContext: false);
		if (userRealmDiscoveryResponse == null)
		{
			throw new MsalClientException("user_realm_discovery_failed", "User realm discovery failed");
		}
		_requestContext.Logger.InfoPii("User with user name '" + username + "' detected as '" + userRealmDiscoveryResponse.AccountType + "'", string.Empty);
		return userRealmDiscoveryResponse;
	}

	public async Task<WsTrustResponse> PerformWsTrustMexExchangeAsync(string federationMetadataUrl, string cloudAudienceUrn, UserAuthType userAuthType, string username, SecureString password)
	{
		MexDocument mexDocument;
		try
		{
			mexDocument = await _serviceBundle.WsTrustWebRequestManager.GetMexDocumentAsync(federationMetadataUrl, _requestContext).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (XmlException innerException)
		{
			throw new MsalClientException("parsing_ws_metadata_exchange_failed", "Parsing WS metadata exchange failed", innerException);
		}
		WsTrustEndpoint wsTrustEndpoint = ((userAuthType == UserAuthType.IntegratedAuth) ? mexDocument.GetWsTrustWindowsTransportEndpoint() : mexDocument.GetWsTrustUsernamePasswordEndpoint());
		if (wsTrustEndpoint == null)
		{
			throw new MsalClientException("wstrust_endpoint_not_found", "WS-Trust endpoint not found in metadata document");
		}
		_requestContext.Logger.InfoPii(string.Format(CultureInfo.InvariantCulture, "WS-Trust endpoint '{0}' being used from MEX at '{1}'", wsTrustEndpoint.Uri, federationMetadataUrl), "Fetched and parsed MEX");
		WsTrustResponse wsTrustResponse = await GetWsTrustResponseAsync(userAuthType, cloudAudienceUrn, wsTrustEndpoint, username, password).ConfigureAwait(continueOnCapturedContext: false);
		_requestContext.Logger.Info("Token of type '" + wsTrustResponse.TokenType + "' acquired from WS-Trust endpoint");
		return wsTrustResponse;
	}

	internal async Task<WsTrustResponse> GetWsTrustResponseAsync(UserAuthType userAuthType, string cloudAudienceUrn, WsTrustEndpoint endpoint, string username, SecureString securePassword)
	{
		string wsTrustRequest = ((userAuthType == UserAuthType.IntegratedAuth) ? endpoint.BuildTokenRequestMessageWindowsIntegratedAuth(cloudAudienceUrn) : endpoint.BuildTokenRequestMessageUsernamePassword(cloudAudienceUrn, username, new string(securePassword.PasswordToCharArray())));
		try
		{
			WsTrustResponse wsTrustResponse = await _serviceBundle.WsTrustWebRequestManager.GetWsTrustResponseAsync(endpoint, wsTrustRequest, _requestContext).ConfigureAwait(continueOnCapturedContext: false);
			_requestContext.Logger.Info("Token of type '" + wsTrustResponse.TokenType + "' acquired from WS-Trust endpoint");
			return wsTrustResponse;
		}
		catch (Exception ex)
		{
			throw new MsalClientException("parsing_wstrust_response_failed", ex.Message, ex);
		}
	}
}
