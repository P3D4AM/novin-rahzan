using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

public class ManagedIdentityCredential : TokenCredential
{
	internal const string MsiUnavailableError = "No managed identity endpoint found.";

	private readonly CredentialPipeline _pipeline;

	private readonly ManagedIdentityClient _client;

	protected ManagedIdentityCredential()
	{
	}

	public ManagedIdentityCredential(string clientId = null, TokenCredentialOptions options = null)
		: this(clientId, CredentialPipeline.GetInstance(options))
	{
	}

	internal ManagedIdentityCredential(string clientId, CredentialPipeline pipeline)
		: this(new ManagedIdentityClient(pipeline, clientId))
	{
	}

	internal ManagedIdentityCredential(ManagedIdentityClient client)
	{
		_pipeline = client.Pipeline;
		_client = client;
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await GetTokenImplAsync(async: true, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetTokenImplAsync(async: false, requestContext, cancellationToken).EnsureCompleted();
	}

	private async ValueTask<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("ManagedIdentityCredential.GetToken", requestContext);
		try
		{
			return scope.Succeeded(await _client.AuthenticateAsync(async, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}
}
