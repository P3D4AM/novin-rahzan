using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Internal.Broker;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class AuthCodeExchangeComponent : ITokenRequestComponent
{
	private readonly AuthenticationRequestParameters _requestParams;

	private readonly AcquireTokenInteractiveParameters _interactiveParameters;

	private readonly string _authorizationCode;

	private readonly string _pkceCodeVerifier;

	private readonly TokenClient _tokenClient;

	public AuthCodeExchangeComponent(AuthenticationRequestParameters requestParams, AcquireTokenInteractiveParameters interactiveParameters, string authorizationCode, string pkceCodeVerifier)
	{
		_requestParams = requestParams ?? throw new ArgumentNullException("requestParams");
		_interactiveParameters = interactiveParameters ?? throw new ArgumentNullException("interactiveParameters");
		_authorizationCode = authorizationCode ?? throw new ArgumentNullException("authorizationCode");
		_pkceCodeVerifier = pkceCodeVerifier ?? throw new ArgumentNullException("pkceCodeVerifier");
		_tokenClient = new TokenClient(requestParams);
		_interactiveParameters.LogParameters(requestParams.RequestContext.Logger);
	}

	public Task<MsalTokenResponse> FetchTokensAsync(CancellationToken cancellationToken)
	{
		return _tokenClient.SendTokenRequestAsync(GetBodyParameters());
	}

	private Dictionary<string, string> GetBodyParameters()
	{
		return new Dictionary<string, string>
		{
			["grant_type"] = "authorization_code",
			["code"] = _authorizationCode,
			["redirect_uri"] = _requestParams.RedirectUri.OriginalString,
			["code_verifier"] = _pkceCodeVerifier
		};
	}
}
