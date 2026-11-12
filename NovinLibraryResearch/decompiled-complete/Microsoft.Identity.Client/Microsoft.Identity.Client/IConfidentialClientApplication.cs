using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client;

public interface IConfidentialClientApplication : IClientApplicationBase
{
	ITokenCache AppTokenCache { get; }

	AcquireTokenByAuthorizationCodeParameterBuilder AcquireTokenByAuthorizationCode(IEnumerable<string> scopes, string authorizationCode);

	AcquireTokenForClientParameterBuilder AcquireTokenForClient(IEnumerable<string> scopes);

	AcquireTokenOnBehalfOfParameterBuilder AcquireTokenOnBehalfOf(IEnumerable<string> scopes, UserAssertion userAssertion);

	GetAuthorizationRequestUrlParameterBuilder GetAuthorizationRequestUrl(IEnumerable<string> scopes);

	[Obsolete("Use AcquireTokenOnBehalfOf instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenOnBehalfOfAsync(IEnumerable<string> scopes, UserAssertion userAssertion);

	[Obsolete("Use AcquireTokenOnBehalfOf instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenOnBehalfOfAsync(IEnumerable<string> scopes, UserAssertion userAssertion, string authority);

	[Obsolete("Use AcquireTokenByAuthorizationCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenByAuthorizationCodeAsync(string authorizationCode, IEnumerable<string> scopes);

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenForClientAsync(IEnumerable<string> scopes);

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenForClientAsync(IEnumerable<string> scopes, bool forceRefresh);

	[Obsolete("Use GetAuthorizationRequestUrl instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<Uri> GetAuthorizationRequestUrlAsync(IEnumerable<string> scopes, string loginHint, string extraQueryParameters);

	[Obsolete("Use GetAuthorizationRequestUrl instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<Uri> GetAuthorizationRequestUrlAsync(IEnumerable<string> scopes, string redirectUri, string loginHint, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority);
}
