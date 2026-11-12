using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public class InteractiveBrowserCredential : TokenCredential
{
	private const string AuthenticationRequiredMessage = "Interactive authentication is needed to acquire token. Call Authenticate to interactively authenticate.";

	private const string NoDefaultScopeMessage = "Authenticating in this environment requires specifying a TokenRequestContext.";

	internal string ClientId { get; }

	internal MsalPublicClient Client { get; }

	internal CredentialPipeline Pipeline { get; }

	internal bool DisableAutomaticAuthentication { get; }

	internal AuthenticationRecord Record { get; private set; }

	public InteractiveBrowserCredential()
		: this(null, "04b07795-8ddb-461a-bbee-02f9e1bf7b46", null, null)
	{
	}

	public InteractiveBrowserCredential(InteractiveBrowserCredentialOptions options)
		: this(options?.TenantId, options?.ClientId ?? "04b07795-8ddb-461a-bbee-02f9e1bf7b46", options, null)
	{
		DisableAutomaticAuthentication = options?.DisableAutomaticAuthentication ?? false;
		Record = options?.AuthenticationRecord;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public InteractiveBrowserCredential(string clientId)
		: this(null, clientId, null, null)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public InteractiveBrowserCredential(string tenantId, string clientId, TokenCredentialOptions options = null)
		: this(Validations.ValidateTenantId(tenantId, "tenantId", allowNull: true), clientId, options, null, null)
	{
	}

	internal InteractiveBrowserCredential(string tenantId, string clientId, TokenCredentialOptions options, CredentialPipeline pipeline)
		: this(tenantId, clientId, options, pipeline, null)
	{
	}

	internal InteractiveBrowserCredential(string tenantId, string clientId, TokenCredentialOptions options, CredentialPipeline pipeline, MsalPublicClient client)
	{
		ClientId = clientId ?? throw new ArgumentNullException("clientId");
		Pipeline = pipeline ?? CredentialPipeline.GetInstance(options);
		string redirectUrl = (options as InteractiveBrowserCredentialOptions)?.RedirectUri?.AbsoluteUri ?? "http://localhost";
		Client = client ?? new MsalPublicClient(Pipeline, tenantId, clientId, redirectUrl, options as ITokenCacheOptions);
	}

	internal virtual AuthenticationRecord Authenticate(CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = AzureAuthorityHosts.GetDefaultScope(Pipeline.AuthorityHost) ?? throw new CredentialUnavailableException("Authenticating in this environment requires specifying a TokenRequestContext.");
		return Authenticate(new TokenRequestContext(new string[1] { text }), cancellationToken);
	}

	internal virtual async Task<AuthenticationRecord> AuthenticateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = AzureAuthorityHosts.GetDefaultScope(Pipeline.AuthorityHost) ?? throw new CredentialUnavailableException("Authenticating in this environment requires specifying a TokenRequestContext.");
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
		using CredentialDiagnosticScope scope = Pipeline.StartGetTokenScope("InteractiveBrowserCredential.Authenticate", requestContext);
		try
		{
			scope.Succeeded(await GetTokenViaBrowserLoginAsync(requestContext.Scopes, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			return Record;
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}

	private async ValueTask<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = Pipeline.StartGetTokenScope("InteractiveBrowserCredential.GetToken", requestContext);
		_ = 1;
		try
		{
			Exception innerException = null;
			if (Record != null)
			{
				try
				{
					AuthenticationResult authenticationResult = await Client.AcquireTokenSilentAsync(requestContext.Scopes, Record, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					return scope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
				}
				catch (MsalUiRequiredException ex)
				{
					innerException = ex;
				}
			}
			if (DisableAutomaticAuthentication)
			{
				throw new AuthenticationRequiredException("Interactive authentication is needed to acquire token. Call Authenticate to interactively authenticate.", requestContext, innerException);
			}
			return scope.Succeeded(await GetTokenViaBrowserLoginAsync(requestContext.Scopes, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
		}
		catch (Exception ex2)
		{
			throw scope.FailWrapAndThrow(ex2);
		}
	}

	private async Task<AccessToken> GetTokenViaBrowserLoginAsync(string[] scopes, bool async, CancellationToken cancellationToken)
	{
		AuthenticationResult authenticationResult = await Client.AcquireTokenInteractiveAsync(scopes, Prompt.SelectAccount, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		Record = new AuthenticationRecord(authenticationResult, ClientId);
		return new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn);
	}
}
