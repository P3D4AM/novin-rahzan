using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public class SharedTokenCacheCredential : TokenCredential
{
	internal const string NoAccountsInCacheMessage = "SharedTokenCacheCredential authentication unavailable. No accounts were found in the cache.";

	internal const string MultipleAccountsInCacheMessage = "SharedTokenCacheCredential authentication unavailable. Multiple accounts were found in the cache. Use username and tenant id to disambiguate.";

	internal const string NoMatchingAccountsInCacheMessage = "SharedTokenCacheCredential authentication unavailable. No account matching the specified{0}{1} was found in the cache.";

	internal const string MultipleMatchingAccountsInCacheMessage = "SharedTokenCacheCredential authentication unavailable. Multiple accounts matching the specified{0}{1} were found in the cache.";

	private static readonly ITokenCacheOptions s_DefaultCacheOptions = new SharedTokenCacheCredentialOptions();

	private readonly MsalPublicClient _client;

	private readonly CredentialPipeline _pipeline;

	private readonly string _tenantId;

	private readonly string _username;

	private readonly AuthenticationRecord _record;

	private readonly AsyncLockWithValue<IAccount> _accountAsyncLock;

	public SharedTokenCacheCredential()
		: this(null, null, null, null, null)
	{
	}

	public SharedTokenCacheCredential(SharedTokenCacheCredentialOptions options)
		: this(options?.TenantId, options?.Username, options, null, null)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public SharedTokenCacheCredential(string username, TokenCredentialOptions options = null)
		: this(null, username, options, null, null)
	{
	}

	internal SharedTokenCacheCredential(string tenantId, string username, TokenCredentialOptions options, CredentialPipeline pipeline)
		: this(tenantId, username, options, pipeline, null)
	{
	}

	internal SharedTokenCacheCredential(string tenantId, string username, TokenCredentialOptions options, CredentialPipeline pipeline, MsalPublicClient client)
	{
		_tenantId = tenantId;
		_username = username;
		_record = (options as SharedTokenCacheCredentialOptions)?.AuthenticationRecord;
		_pipeline = pipeline ?? CredentialPipeline.GetInstance(options);
		_client = client ?? new MsalPublicClient(_pipeline, tenantId, "04b07795-8ddb-461a-bbee-02f9e1bf7b46", null, (options as ITokenCacheOptions) ?? s_DefaultCacheOptions);
		_accountAsyncLock = new AsyncLockWithValue<IAccount>();
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
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("SharedTokenCacheCredential.GetToken", requestContext);
		_ = 1;
		try
		{
			IAccount account = await GetAccountAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			AuthenticationResult authenticationResult = await _client.AcquireTokenSilentAsync(requestContext.Scopes, account, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return scope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
		}
		catch (MsalUiRequiredException)
		{
			throw scope.FailWrapAndThrow(new CredentialUnavailableException("SharedTokenCacheCredential authentication unavailable. Token acquisition failed for user " + _username + ". Ensure that you have authenticated with a developer tool that supports Azure single sign on."));
		}
		catch (Exception ex2)
		{
			throw scope.FailWrapAndThrow(ex2);
		}
	}

	private async ValueTask<IAccount> GetAccountAsync(bool async, CancellationToken cancellationToken)
	{
		using AsyncLockWithValue<IAccount>.Lock asyncLock = await _accountAsyncLock.GetLockOrValueAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (asyncLock.HasValue)
		{
			return asyncLock.Value;
		}
		IAccount account;
		if (_record != null)
		{
			account = new AuthenticationAccount(_record);
			asyncLock.SetValue(account);
			return account;
		}
		List<IAccount> list = await _client.GetAccountsAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		List<IAccount> list2 = list.Where((IAccount a) => (string.IsNullOrEmpty(_username) || string.Compare(a.Username, _username, StringComparison.OrdinalIgnoreCase) == 0) && (string.IsNullOrEmpty(_tenantId) || string.Compare(a.HomeAccountId?.TenantId, _tenantId, StringComparison.OrdinalIgnoreCase) == 0)).ToList();
		if (list2.Count != 1)
		{
			throw new CredentialUnavailableException(GetCredentialUnavailableMessage(list, list2));
		}
		account = list2.First();
		asyncLock.SetValue(account);
		return account;
	}

	private string GetCredentialUnavailableMessage(List<IAccount> accounts, List<IAccount> filteredAccounts)
	{
		if (accounts.Count == 0)
		{
			return "SharedTokenCacheCredential authentication unavailable. No accounts were found in the cache.";
		}
		if (string.IsNullOrEmpty(_username) && string.IsNullOrEmpty(_tenantId))
		{
			return string.Format(CultureInfo.InvariantCulture, "SharedTokenCacheCredential authentication unavailable. Multiple accounts were found in the cache. Use username and tenant id to disambiguate.");
		}
		string arg = (string.IsNullOrEmpty(_username) ? string.Empty : (" username: " + _username));
		string arg2 = (string.IsNullOrEmpty(_tenantId) ? string.Empty : (" tenantId: " + _tenantId));
		if (filteredAccounts.Count == 0)
		{
			return string.Format(CultureInfo.InvariantCulture, "SharedTokenCacheCredential authentication unavailable. No account matching the specified{0}{1} was found in the cache.", arg, arg2);
		}
		return string.Format(CultureInfo.InvariantCulture, "SharedTokenCacheCredential authentication unavailable. Multiple accounts matching the specified{0}{1} were found in the cache.", arg, arg2);
	}
}
