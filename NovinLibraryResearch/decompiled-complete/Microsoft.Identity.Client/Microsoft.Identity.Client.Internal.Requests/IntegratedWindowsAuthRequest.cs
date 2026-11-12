using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.WsTrust;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class IntegratedWindowsAuthRequest : RequestBase
{
	private readonly CommonNonInteractiveHandler _commonNonInteractiveHandler;

	private readonly AcquireTokenByIntegratedWindowsAuthParameters _integratedWindowsAuthParameters;

	public IntegratedWindowsAuthRequest(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenByIntegratedWindowsAuthParameters integratedWindowsAuthParameters)
		: base(serviceBundle, authenticationRequestParameters, integratedWindowsAuthParameters)
	{
		_integratedWindowsAuthParameters = integratedWindowsAuthParameters;
		_commonNonInteractiveHandler = new CommonNonInteractiveHandler(authenticationRequestParameters.RequestContext, serviceBundle);
	}

	protected override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		await ResolveAuthorityEndpointsAsync().ConfigureAwait(continueOnCapturedContext: false);
		await UpdateUsernameAsync().ConfigureAwait(continueOnCapturedContext: false);
		return await CacheTokenResponseAndCreateAuthenticationResultAsync(await SendTokenRequestAsync(GetAdditionalBodyParameters(await FetchAssertionFromWsTrustAsync().ConfigureAwait(continueOnCapturedContext: false)), cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<UserAssertion> FetchAssertionFromWsTrustAsync()
	{
		if (base.AuthenticationRequestParameters.AuthorityInfo.AuthorityType == AuthorityType.Adfs)
		{
			return null;
		}
		UserRealmDiscoveryResponse userRealmDiscoveryResponse = await _commonNonInteractiveHandler.QueryUserRealmDataAsync(base.AuthenticationRequestParameters.AuthorityInfo.UserRealmUriPrefix, _integratedWindowsAuthParameters.Username).ConfigureAwait(continueOnCapturedContext: false);
		if (userRealmDiscoveryResponse.IsFederated)
		{
			WsTrustResponse wsTrustResponse = await _commonNonInteractiveHandler.PerformWsTrustMexExchangeAsync(userRealmDiscoveryResponse.FederationMetadataUrl, userRealmDiscoveryResponse.CloudAudienceUrn, UserAuthType.IntegratedAuth, _integratedWindowsAuthParameters.Username, null).ConfigureAwait(continueOnCapturedContext: false);
			return new UserAssertion(wsTrustResponse.Token, (wsTrustResponse.TokenType == "urn:oasis:names:tc:SAML:1.0:assertion") ? "urn:ietf:params:oauth:grant-type:saml1_1-bearer" : "urn:ietf:params:oauth:grant-type:saml2-bearer");
		}
		if (userRealmDiscoveryResponse.IsManaged)
		{
			throw new MsalClientException("integrated_windows_auth_not_supported_managed_user", "Integrated Windows Auth is not supported for managed users. See https://aka.ms/msal-net-iwa for details.");
		}
		throw new MsalClientException("unknown_user_type", string.Format(CultureInfo.CurrentCulture, "Unsupported User Type '{0}'. Please see https://aka.ms/msal-net-up", userRealmDiscoveryResponse.AccountType));
	}

	private async Task UpdateUsernameAsync()
	{
		if (string.IsNullOrWhiteSpace(_integratedWindowsAuthParameters.Username))
		{
			string username = await _commonNonInteractiveHandler.GetPlatformUserAsync().ConfigureAwait(continueOnCapturedContext: false);
			_integratedWindowsAuthParameters.Username = username;
		}
	}

	private static Dictionary<string, string> GetAdditionalBodyParameters(UserAssertion userAssertion)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (userAssertion != null)
		{
			dictionary["grant_type"] = userAssertion.AssertionType;
			dictionary["assertion"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(userAssertion.Assertion));
		}
		return dictionary;
	}
}
