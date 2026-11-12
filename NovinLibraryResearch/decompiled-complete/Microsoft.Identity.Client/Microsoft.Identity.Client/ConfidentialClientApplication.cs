using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client;

public sealed class ConfidentialClientApplication : ClientApplicationBase, IConfidentialClientApplication, IClientApplicationBase, IConfidentialClientApplicationWithCertificate, IByRefreshToken
{
	internal ClientCredentialWrapper ClientCredential => base.ServiceBundle.Config.ClientCredential;

	public ITokenCache AppTokenCache => AppTokenCacheInternal;

	public X509Certificate2 Certificate { get; }

	internal ITokenCacheInternal AppTokenCacheInternal { get; set; }

	internal ConfidentialClientApplication(ApplicationConfiguration configuration)
		: base(configuration)
	{
		GuardMobileFrameworks();
		AppTokenCacheInternal = new TokenCache(base.ServiceBundle, isApplicationTokenCache: true);
		Certificate = configuration.ClientCredentialCertificate;
	}

	public AcquireTokenByAuthorizationCodeParameterBuilder AcquireTokenByAuthorizationCode(IEnumerable<string> scopes, string authorizationCode)
	{
		return AcquireTokenByAuthorizationCodeParameterBuilder.Create(ClientExecutorFactory.CreateConfidentialClientExecutor(this), scopes, authorizationCode);
	}

	public AcquireTokenForClientParameterBuilder AcquireTokenForClient(IEnumerable<string> scopes)
	{
		return AcquireTokenForClientParameterBuilder.Create(ClientExecutorFactory.CreateConfidentialClientExecutor(this), scopes);
	}

	public AcquireTokenOnBehalfOfParameterBuilder AcquireTokenOnBehalfOf(IEnumerable<string> scopes, UserAssertion userAssertion)
	{
		return AcquireTokenOnBehalfOfParameterBuilder.Create(ClientExecutorFactory.CreateConfidentialClientExecutor(this), scopes, userAssertion);
	}

	public GetAuthorizationRequestUrlParameterBuilder GetAuthorizationRequestUrl(IEnumerable<string> scopes)
	{
		return GetAuthorizationRequestUrlParameterBuilder.Create(ClientExecutorFactory.CreateConfidentialClientExecutor(this), scopes);
	}

	AcquireTokenByRefreshTokenParameterBuilder IByRefreshToken.AcquireTokenByRefreshToken(IEnumerable<string> scopes, string refreshToken)
	{
		return AcquireTokenByRefreshTokenParameterBuilder.Create(ClientExecutorFactory.CreateClientApplicationBaseExecutor(this), scopes, refreshToken);
	}

	internal override AuthenticationRequestParameters CreateRequestParameters(AcquireTokenCommonParameters commonParameters, RequestContext requestContext, ITokenCacheInternal cache)
	{
		AuthenticationRequestParameters authenticationRequestParameters = base.CreateRequestParameters(commonParameters, requestContext, cache);
		authenticationRequestParameters.ClientCredential = base.ServiceBundle.Config.ClientCredential;
		return authenticationRequestParameters;
	}

	internal static void GuardMobileFrameworks()
	{
	}

	[Obsolete("Use ConfidentialClientApplicationBuilder instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public ConfidentialClientApplication(string clientId, string redirectUri, ClientCredential clientCredential, TokenCache userTokenCache, TokenCache appTokenCache)
		: this(ConfidentialClientApplicationBuilder.Create(clientId).BuildConfiguration())
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use ConfidentialClientApplicationBuilder instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public ConfidentialClientApplication(string clientId, string authority, string redirectUri, ClientCredential clientCredential, TokenCache userTokenCache, TokenCache appTokenCache)
		: this(ConfidentialClientApplicationBuilder.Create(clientId).BuildConfiguration())
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenOnBehalfOf instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenOnBehalfOfAsync(IEnumerable<string> scopes, UserAssertion userAssertion)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenOnBehalfOf instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenOnBehalfOfAsync(IEnumerable<string> scopes, UserAssertion userAssertion, string authority)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenOnBehalfOf instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> IConfidentialClientApplicationWithCertificate.AcquireTokenOnBehalfOfWithCertificateAsync(IEnumerable<string> scopes, UserAssertion userAssertion)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenOnBehalfOf instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> IConfidentialClientApplicationWithCertificate.AcquireTokenOnBehalfOfWithCertificateAsync(IEnumerable<string> scopes, UserAssertion userAssertion, string authority)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenByAuthorizationCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenByAuthorizationCodeAsync(string authorizationCode, IEnumerable<string> scopes)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenForClientAsync(IEnumerable<string> scopes)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenForClientAsync(IEnumerable<string> scopes, bool forceRefresh)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> IConfidentialClientApplicationWithCertificate.AcquireTokenForClientWithCertificateAsync(IEnumerable<string> scopes)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> IConfidentialClientApplicationWithCertificate.AcquireTokenForClientWithCertificateAsync(IEnumerable<string> scopes, bool forceRefresh)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenByRefreshToken instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> IByRefreshToken.AcquireTokenByRefreshTokenAsync(IEnumerable<string> scopes, string refreshToken)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use GetAuthorizationRequestUrl instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<Uri> GetAuthorizationRequestUrlAsync(IEnumerable<string> scopes, string loginHint, string extraQueryParameters)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use GetAuthorizationRequestUrl instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<Uri> GetAuthorizationRequestUrlAsync(IEnumerable<string> scopes, string redirectUri, string loginHint, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}
}
