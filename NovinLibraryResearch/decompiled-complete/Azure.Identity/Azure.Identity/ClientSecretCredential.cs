using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public class ClientSecretCredential : TokenCredential
{
	private readonly MsalConfidentialClient _client;

	private readonly CredentialPipeline _pipeline;

	internal string TenantId { get; }

	internal string ClientId { get; }

	internal string ClientSecret { get; }

	protected ClientSecretCredential()
	{
	}

	public ClientSecretCredential(string tenantId, string clientId, string clientSecret)
		: this(tenantId, clientId, clientSecret, null, null, null)
	{
	}

	internal ClientSecretCredential(string tenantId, string clientId, string clientSecret, ClientSecretCredentialOptions options)
		: this(tenantId, clientId, clientSecret, options, null, null)
	{
	}

	public ClientSecretCredential(string tenantId, string clientId, string clientSecret, TokenCredentialOptions options)
		: this(tenantId, clientId, clientSecret, options, null, null)
	{
	}

	internal ClientSecretCredential(string tenantId, string clientId, string clientSecret, TokenCredentialOptions options, CredentialPipeline pipeline, MsalConfidentialClient client)
	{
		TenantId = Validations.ValidateTenantId(tenantId, "tenantId");
		ClientId = clientId ?? throw new ArgumentNullException("clientId");
		ClientSecret = clientSecret ?? throw new ArgumentNullException("clientSecret");
		_pipeline = pipeline ?? CredentialPipeline.GetInstance(options);
		_client = client ?? new MsalConfidentialClient(_pipeline, tenantId, clientId, clientSecret, options as ITokenCacheOptions);
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("ClientSecretCredential.GetToken", requestContext);
		try
		{
			AuthenticationResult authenticationResult = await _client.AcquireTokenForClientAsync(requestContext.Scopes, async: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return scope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		using CredentialDiagnosticScope credentialDiagnosticScope = _pipeline.StartGetTokenScope("ClientSecretCredential.GetToken", requestContext);
		try
		{
			AuthenticationResult authenticationResult = _client.AcquireTokenForClientAsync(requestContext.Scopes, async: false, cancellationToken).EnsureCompleted();
			return credentialDiagnosticScope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
		}
		catch (Exception ex)
		{
			throw credentialDiagnosticScope.FailWrapAndThrow(ex);
		}
	}
}
