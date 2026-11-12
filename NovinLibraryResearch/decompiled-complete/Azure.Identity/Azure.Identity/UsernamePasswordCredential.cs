using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public class UsernamePasswordCredential : TokenCredential
{
	private const string NoDefaultScopeMessage = "Authenticating in this environment requires specifying a TokenRequestContext.";

	private readonly string _clientId;

	private readonly MsalPublicClient _client;

	private readonly CredentialPipeline _pipeline;

	private readonly string _username;

	private readonly SecureString _password;

	private AuthenticationRecord _record;

	protected UsernamePasswordCredential()
	{
	}

	public UsernamePasswordCredential(string username, string password, string tenantId, string clientId)
		: this(username, password, tenantId, clientId, (TokenCredentialOptions)null)
	{
	}

	public UsernamePasswordCredential(string username, string password, string tenantId, string clientId, TokenCredentialOptions options)
		: this(username, password, tenantId, clientId, options, null, null)
	{
	}

	internal UsernamePasswordCredential(string username, string password, string tenantId, string clientId, UsernamePasswordCredentialOptions options)
		: this(username, password, tenantId, clientId, options, null, null)
	{
	}

	internal UsernamePasswordCredential(string username, string password, string tenantId, string clientId, TokenCredentialOptions options, CredentialPipeline pipeline, MsalPublicClient client)
	{
		_username = username ?? throw new ArgumentNullException("username");
		if (password == null)
		{
			throw new ArgumentNullException("password");
		}
		_password = password.ToSecureString();
		_clientId = clientId ?? throw new ArgumentNullException("clientId");
		Validations.ValidateTenantId(tenantId, "tenantId");
		_pipeline = pipeline ?? CredentialPipeline.GetInstance(options);
		_client = client ?? new MsalPublicClient(_pipeline, tenantId, clientId, null, options as ITokenCacheOptions);
	}

	internal virtual AuthenticationRecord Authenticate(CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = AzureAuthorityHosts.GetDefaultScope(_pipeline.AuthorityHost) ?? throw new CredentialUnavailableException("Authenticating in this environment requires specifying a TokenRequestContext.");
		return Authenticate(new TokenRequestContext(new string[1] { text }), cancellationToken);
	}

	internal virtual async Task<AuthenticationRecord> AuthenticateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = AzureAuthorityHosts.GetDefaultScope(_pipeline.AuthorityHost) ?? throw new CredentialUnavailableException("Authenticating in this environment requires specifying a TokenRequestContext.");
		return await AuthenticateAsync(new TokenRequestContext(new string[1] { text }), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal virtual AuthenticationRecord Authenticate(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AuthenticateImplAsync(async: false, requestContext, cancellationToken).EnsureCompleted();
	}

	internal virtual async Task<AuthenticationRecord> AuthenticateAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await AuthenticateImplAsync(async: true, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetTokenImplAsync(async: false, requestContext, cancellationToken).EnsureCompleted();
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await GetTokenImplAsync(async: true, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<AuthenticationRecord> AuthenticateImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("UsernamePasswordCredential.Authenticate", requestContext);
		try
		{
			scope.Succeeded(await GetTokenImplAsync(async, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			return _record;
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}

	private async Task<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("UsernamePasswordCredential.GetToken", requestContext);
		try
		{
			AuthenticationResult authenticationResult = await _client.AcquireTokenByUsernamePasswordAsync(requestContext.Scopes, _username, _password, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			_record = new AuthenticationRecord(authenticationResult, _clientId);
			return scope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}
}
