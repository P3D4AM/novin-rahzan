using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Client.WsTrust;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class UsernamePasswordRequest : RequestBase
{
	private readonly CommonNonInteractiveHandler _commonNonInteractiveHandler;

	private readonly AcquireTokenByUsernamePasswordParameters _usernamePasswordParameters;

	public UsernamePasswordRequest(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenByUsernamePasswordParameters usernamePasswordParameters)
		: base(serviceBundle, authenticationRequestParameters, usernamePasswordParameters)
	{
		_usernamePasswordParameters = usernamePasswordParameters;
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
		if (base.AuthenticationRequestParameters.AuthorityInfo.AuthorityType == AuthorityType.Adfs || base.AuthenticationRequestParameters.AuthorityInfo.AuthorityType == AuthorityType.B2C)
		{
			return null;
		}
		UserRealmDiscoveryResponse userRealmDiscoveryResponse = await _commonNonInteractiveHandler.QueryUserRealmDataAsync(base.AuthenticationRequestParameters.AuthorityInfo.UserRealmUriPrefix, _usernamePasswordParameters.Username).ConfigureAwait(continueOnCapturedContext: false);
		if (userRealmDiscoveryResponse.IsFederated)
		{
			WsTrustResponse wsTrustResponse = await _commonNonInteractiveHandler.PerformWsTrustMexExchangeAsync(userRealmDiscoveryResponse.FederationMetadataUrl, userRealmDiscoveryResponse.CloudAudienceUrn, UserAuthType.UsernamePassword, _usernamePasswordParameters.Username, _usernamePasswordParameters.Password).ConfigureAwait(continueOnCapturedContext: false);
			return new UserAssertion(wsTrustResponse.Token, (wsTrustResponse.TokenType == "urn:oasis:names:tc:SAML:1.0:assertion") ? "urn:ietf:params:oauth:grant-type:saml1_1-bearer" : "urn:ietf:params:oauth:grant-type:saml2-bearer");
		}
		if (userRealmDiscoveryResponse.IsManaged)
		{
			if (_usernamePasswordParameters.Password == null)
			{
				throw new MsalClientException("password_required_for_managed_user");
			}
			return null;
		}
		throw new MsalClientException("unknown_user_type", string.Format(CultureInfo.CurrentCulture, "Unsupported User Type '{0}'. Please see https://aka.ms/msal-net-up", userRealmDiscoveryResponse.AccountType));
	}

	private async Task UpdateUsernameAsync()
	{
		if (string.IsNullOrWhiteSpace(_usernamePasswordParameters.Username))
		{
			string username = await _commonNonInteractiveHandler.GetPlatformUserAsync().ConfigureAwait(continueOnCapturedContext: false);
			_usernamePasswordParameters.Username = username;
		}
	}

	private Dictionary<string, string> GetAdditionalBodyParameters(UserAssertion userAssertion)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (userAssertion != null)
		{
			dictionary["grant_type"] = userAssertion.AssertionType;
			dictionary["assertion"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(userAssertion.Assertion));
		}
		else
		{
			dictionary["grant_type"] = "password";
			dictionary["username"] = _usernamePasswordParameters.Username;
			dictionary["password"] = new string(_usernamePasswordParameters.Password.PasswordToCharArray());
		}
		ISet<string> set = new HashSet<string> { "openid", "offline_access", "profile" };
		set.UnionWith(base.AuthenticationRequestParameters.Scope);
		dictionary["scope"] = set.AsSingleString();
		return dictionary;
	}
}
