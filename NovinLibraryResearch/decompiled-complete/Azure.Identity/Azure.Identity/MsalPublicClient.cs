using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Azure.Identity;

internal class MsalPublicClient : MsalClientBase<IPublicClientApplication>
{
	internal string RedirectUrl { get; }

	protected MsalPublicClient()
	{
	}

	public MsalPublicClient(CredentialPipeline pipeline, string tenantId, string clientId, string redirectUrl, ITokenCacheOptions cacheOptions)
		: base(pipeline, tenantId, clientId, cacheOptions)
	{
		RedirectUrl = redirectUrl;
	}

	protected override ValueTask<IPublicClientApplication> CreateClientAsync(bool async, CancellationToken cancellationToken)
	{
		Uri authorityHost = base.Pipeline.AuthorityHost;
		Uri uri = new UriBuilder(authorityHost.Scheme, authorityHost.Host, authorityHost.Port, base.TenantId ?? "organizations").Uri;
		PublicClientApplicationBuilder publicClientApplicationBuilder = PublicClientApplicationBuilder.Create(base.ClientId).WithAuthority(uri).WithHttpClientFactory(new HttpPipelineClientFactory(base.Pipeline.HttpPipeline));
		if (!string.IsNullOrEmpty(RedirectUrl))
		{
			publicClientApplicationBuilder = publicClientApplicationBuilder.WithRedirectUri(RedirectUrl);
		}
		return new ValueTask<IPublicClientApplication>(publicClientApplicationBuilder.Build());
	}

	public virtual async ValueTask<List<IAccount>> GetAccountsAsync(bool async, CancellationToken cancellationToken)
	{
		return await GetAccountsAsync(await GetClientAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false), async).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async ValueTask<AuthenticationResult> AcquireTokenSilentAsync(string[] scopes, IAccount account, bool async, CancellationToken cancellationToken)
	{
		return await (await GetClientAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).AcquireTokenSilent(scopes, account).ExecuteAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async ValueTask<AuthenticationResult> AcquireTokenSilentAsync(string[] scopes, AuthenticationRecord record, bool async, CancellationToken cancellationToken)
	{
		return await (await GetClientAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).AcquireTokenSilent(scopes, (AuthenticationAccount)record).WithAuthority(base.Pipeline.AuthorityHost.AbsoluteUri, base.TenantId ?? record.TenantId).ExecuteAsync(async, cancellationToken)
			.ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async ValueTask<AuthenticationResult> AcquireTokenInteractiveAsync(string[] scopes, Prompt prompt, bool async, CancellationToken cancellationToken)
	{
		return await (await GetClientAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).AcquireTokenInteractive(scopes).WithPrompt(prompt).ExecuteAsync(async, cancellationToken)
			.ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async ValueTask<AuthenticationResult> AcquireTokenByUsernamePasswordAsync(string[] scopes, string username, SecureString password, bool async, CancellationToken cancellationToken)
	{
		return await (await GetClientAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).AcquireTokenByUsernamePassword(scopes, username, password).ExecuteAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async ValueTask<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(string[] scopes, Func<DeviceCodeResult, Task> deviceCodeCallback, bool async, CancellationToken cancellationToken)
	{
		return await (await GetClientAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).AcquireTokenWithDeviceCode(scopes, deviceCodeCallback).ExecuteAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async ValueTask<AuthenticationResult> AcquireTokenByRefreshToken(string[] scopes, string refreshToken, AzureCloudInstance azureCloudInstance, string tenant, bool async, CancellationToken cancellationToken)
	{
		return await ((IByRefreshToken)(await GetClientAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))).AcquireTokenByRefreshToken(scopes, refreshToken).WithAuthority(azureCloudInstance, tenant).ExecuteAsync(async, cancellationToken)
			.ConfigureAwait(continueOnCapturedContext: false);
	}

	private static async ValueTask<List<IAccount>> GetAccountsAsync(IPublicClientApplication client, bool async)
	{
		IEnumerable<IAccount> source = ((!async) ? client.GetAccountsAsync().GetAwaiter().GetResult() : (await client.GetAccountsAsync().ConfigureAwait(continueOnCapturedContext: false)));
		return source.ToList();
	}
}
