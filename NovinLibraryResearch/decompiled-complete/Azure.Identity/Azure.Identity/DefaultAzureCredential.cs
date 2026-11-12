using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

public class DefaultAzureCredential : TokenCredential
{
	private const string DefaultExceptionMessage = "DefaultAzureCredential failed to retrieve a token from the included credentials.";

	private const string UnhandledExceptionMessage = "DefaultAzureCredential authentication failed due to an unhandled exception: ";

	private static readonly TokenCredential[] s_defaultCredentialChain = GetDefaultAzureCredentialChain(new DefaultAzureCredentialFactory(null), new DefaultAzureCredentialOptions());

	private readonly CredentialPipeline _pipeline;

	private readonly AsyncLockWithValue<TokenCredential> _credentialLock;

	private TokenCredential[] _sources;

	internal DefaultAzureCredential()
		: this(includeInteractiveCredentials: false)
	{
	}

	public DefaultAzureCredential(bool includeInteractiveCredentials = false)
		: this(includeInteractiveCredentials ? new DefaultAzureCredentialOptions
		{
			ExcludeInteractiveBrowserCredential = false
		} : null)
	{
	}

	public DefaultAzureCredential(DefaultAzureCredentialOptions options)
		: this(new DefaultAzureCredentialFactory(ValidateAuthorityHostOption(options)), options)
	{
	}

	internal DefaultAzureCredential(DefaultAzureCredentialFactory factory, DefaultAzureCredentialOptions options)
	{
		_pipeline = factory.Pipeline;
		_sources = GetDefaultAzureCredentialChain(factory, options);
		_credentialLock = new AsyncLockWithValue<TokenCredential>();
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
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScopeGroup("DefaultAzureCredential.GetToken", requestContext);
		_ = 2;
		try
		{
			using AsyncLockWithValue<TokenCredential>.Lock asyncLock = await _credentialLock.GetLockOrValueAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			AccessToken token;
			if (asyncLock.HasValue)
			{
				token = await GetTokenFromCredentialAsync(asyncLock.Value, requestContext, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				(AccessToken, TokenCredential) obj = await GetTokenFromSourcesAsync(_sources, requestContext, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				token = obj.Item1;
				TokenCredential item = obj.Item2;
				_sources = null;
				asyncLock.SetValue(item);
			}
			return scope.Succeeded(token);
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}

	private static async ValueTask<AccessToken> GetTokenFromCredentialAsync(TokenCredential credential, TokenRequestContext requestContext, bool async, CancellationToken cancellationToken)
	{
		try
		{
			return (!async) ? credential.GetToken(requestContext, cancellationToken) : (await credential.GetTokenAsync(requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
		}
		catch (Exception ex) when (!(ex is CredentialUnavailableException))
		{
			throw new AuthenticationFailedException("DefaultAzureCredential authentication failed due to an unhandled exception: ", ex);
		}
	}

	private static async ValueTask<(AccessToken, TokenCredential)> GetTokenFromSourcesAsync(TokenCredential[] sources, TokenRequestContext requestContext, bool async, CancellationToken cancellationToken)
	{
		List<CredentialUnavailableException> exceptions = new List<CredentialUnavailableException>();
		for (int i = 0; i < sources.Length && sources[i] != null; i++)
		{
			try
			{
				AccessToken item = ((!async) ? sources[i].GetToken(requestContext, cancellationToken) : (await sources[i].GetTokenAsync(requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
				return (item, sources[i]);
			}
			catch (CredentialUnavailableException item2)
			{
				exceptions.Add(item2);
			}
		}
		throw CredentialUnavailableException.CreateAggregateException("DefaultAzureCredential failed to retrieve a token from the included credentials.", exceptions);
	}

	private static TokenCredential[] GetDefaultAzureCredentialChain(DefaultAzureCredentialFactory factory, DefaultAzureCredentialOptions options)
	{
		if (options == null)
		{
			return s_defaultCredentialChain;
		}
		int num = 0;
		TokenCredential[] array = new TokenCredential[7];
		if (!options.ExcludeEnvironmentCredential)
		{
			array[num++] = factory.CreateEnvironmentCredential();
		}
		if (!options.ExcludeManagedIdentityCredential)
		{
			array[num++] = factory.CreateManagedIdentityCredential(options.ManagedIdentityClientId);
		}
		if (!options.ExcludeSharedTokenCacheCredential)
		{
			array[num++] = factory.CreateSharedTokenCacheCredential(options.SharedTokenCacheTenantId, options.SharedTokenCacheUsername);
		}
		if (!options.ExcludeVisualStudioCredential)
		{
			array[num++] = factory.CreateVisualStudioCredential(options.VisualStudioTenantId);
		}
		if (!options.ExcludeVisualStudioCodeCredential)
		{
			array[num++] = factory.CreateVisualStudioCodeCredential(options.VisualStudioCodeTenantId);
		}
		if (!options.ExcludeAzureCliCredential)
		{
			array[num++] = factory.CreateAzureCliCredential();
		}
		if (!options.ExcludeInteractiveBrowserCredential)
		{
			array[num++] = factory.CreateInteractiveBrowserCredential(options.InteractiveBrowserTenantId);
		}
		if (num == 0)
		{
			throw new ArgumentException("At least one credential type must be included in the authentication flow.", "options");
		}
		return array;
	}

	private static DefaultAzureCredentialOptions ValidateAuthorityHostOption(DefaultAzureCredentialOptions options)
	{
		Validations.ValidateAuthorityHost(options?.AuthorityHost ?? AzureAuthorityHosts.GetDefault());
		return options;
	}
}
