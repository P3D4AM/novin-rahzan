using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

public class EnvironmentCredential : TokenCredential
{
	private const string UnavailbleErrorMessage = "EnvironmentCredential authentication unavailable. Environment variables are not fully configured.";

	private readonly CredentialPipeline _pipeline;

	internal TokenCredential Credential { get; }

	public EnvironmentCredential()
		: this(CredentialPipeline.GetInstance(null))
	{
	}

	public EnvironmentCredential(TokenCredentialOptions options)
		: this(CredentialPipeline.GetInstance(options))
	{
	}

	internal EnvironmentCredential(CredentialPipeline pipeline)
	{
		_pipeline = pipeline;
		string tenantId = EnvironmentVariables.TenantId;
		string clientId = EnvironmentVariables.ClientId;
		string clientSecret = EnvironmentVariables.ClientSecret;
		string clientCertificatePath = EnvironmentVariables.ClientCertificatePath;
		string username = EnvironmentVariables.Username;
		string password = EnvironmentVariables.Password;
		if (tenantId != null && clientId != null)
		{
			if (clientSecret != null)
			{
				Credential = new ClientSecretCredential(tenantId, clientId, clientSecret, null, _pipeline, null);
			}
			else if (username != null && password != null)
			{
				Credential = new UsernamePasswordCredential(username, password, tenantId, clientId, null, _pipeline, null);
			}
			else if (clientCertificatePath != null)
			{
				Credential = new ClientCertificateCredential(tenantId, clientId, clientCertificatePath, null, _pipeline, null);
			}
		}
	}

	internal EnvironmentCredential(CredentialPipeline pipeline, TokenCredential credential)
	{
		_pipeline = pipeline;
		Credential = credential;
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetTokenImplAsync(async: false, requestContext, cancellationToken).EnsureCompleted();
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await GetTokenImplAsync(async: true, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async ValueTask<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("EnvironmentCredential.GetToken", requestContext);
		if (Credential == null)
		{
			throw scope.FailWrapAndThrow(new CredentialUnavailableException("EnvironmentCredential authentication unavailable. Environment variables are not fully configured."));
		}
		try
		{
			AccessToken accessToken = ((!async) ? Credential.GetToken(requestContext, cancellationToken) : (await Credential.GetTokenAsync(requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
			AccessToken token = accessToken;
			return scope.Succeeded(token);
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}
}
