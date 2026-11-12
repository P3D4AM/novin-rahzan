using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public class AuthorizationCodeCredential : TokenCredential
{
	private readonly IConfidentialClientApplication _confidentialClient;

	private readonly Azure.Core.Pipeline.ClientDiagnostics _clientDiagnostics;

	private readonly string _authCode;

	private readonly string _clientId;

	private readonly CredentialPipeline _pipeline;

	private AuthenticationRecord _record;

	protected AuthorizationCodeCredential()
	{
	}

	public AuthorizationCodeCredential(string tenantId, string clientId, string clientSecret, string authorizationCode)
		: this(tenantId, clientId, clientSecret, authorizationCode, null)
	{
	}

	public AuthorizationCodeCredential(string tenantId, string clientId, string clientSecret, string authorizationCode, TokenCredentialOptions options)
	{
		Validations.ValidateTenantId(tenantId, "tenantId");
		if (clientSecret == null)
		{
			throw new ArgumentNullException("clientSecret");
		}
		_clientId = clientId ?? throw new ArgumentNullException("clientId");
		_authCode = authorizationCode ?? throw new ArgumentNullException("authorizationCode");
		if (options == null)
		{
			options = new TokenCredentialOptions();
		}
		_pipeline = CredentialPipeline.GetInstance(options);
		_confidentialClient = ConfidentialClientApplicationBuilder.Create(clientId).WithHttpClientFactory(new HttpPipelineClientFactory(_pipeline.HttpPipeline)).WithTenantId(tenantId)
			.WithClientSecret(clientSecret)
			.Build();
		_clientDiagnostics = new Azure.Core.Pipeline.ClientDiagnostics(options);
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetTokenImplAsync(async: false, requestContext, cancellationToken).EnsureCompleted();
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await GetTokenImplAsync(async: true, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async ValueTask<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("AuthorizationCodeCredential.GetToken", requestContext);
		_ = 1;
		try
		{
			AccessToken token;
			if (_record == null)
			{
				AuthenticationResult authenticationResult = await _confidentialClient.AcquireTokenByAuthorizationCode(requestContext.Scopes, _authCode).ExecuteAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				_record = new AuthenticationRecord(authenticationResult, _clientId);
				token = new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn);
			}
			else
			{
				AuthenticationResult authenticationResult2 = await _confidentialClient.AcquireTokenSilent(requestContext.Scopes, (AuthenticationAccount)_record).ExecuteAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				token = new AccessToken(authenticationResult2.AccessToken, authenticationResult2.ExpiresOn);
			}
			return scope.Succeeded(token);
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}
}
