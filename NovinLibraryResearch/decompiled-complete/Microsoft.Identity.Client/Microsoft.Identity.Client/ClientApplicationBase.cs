using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client;

public abstract class ClientApplicationBase : IClientApplicationBase
{
	internal const string DefaultAuthority = "https://login.microsoftonline.com/common/";

	internal IServiceBundle ServiceBundle { get; }

	public IAppConfig AppConfig => ServiceBundle.Config;

	public string Authority => ServiceBundle.Config.AuthorityInfo.CanonicalAuthority;

	public ITokenCache UserTokenCache => UserTokenCacheInternal;

	internal ITokenCacheInternal UserTokenCacheInternal { get; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[Obsolete("Use GetAccountsAsync instead (See https://aka.ms/msal-net-2-released)", true)]
	public IEnumerable<IUser> Users
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use WithComponent on AbstractApplicationBuilder<T> to configure this instead.See https://aka.ms/msal-net-3-breaking-changes", true)]
	public string Component { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ExtraQueryParameters on each call instead.See https://aka.ms/msal-net-3-breaking-changes", true)]
	public string SliceParameters { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Can be set on AbstractApplicationBuilder<T>.WithAuthority as needed.See https://aka.ms/msal-net-3-breaking-changes", true)]
	public bool ValidateAuthority { get; set; }

	[Obsolete("Should be set using AbstractApplicationBuilder<T>.WithRedirectUri and can be viewed with ClientApplicationBase.AppConfig.RedirectUri.See https://aka.ms/msal-net-3-breaking-changes", true)]
	public string RedirectUri { get; set; }

	[Obsolete("Use AppConfig.ClientId instead.See https://aka.ms/msal-net-3-breaking-changes", true)]
	public string ClientId => AppConfig.ClientId;

	internal ClientApplicationBase(ApplicationConfiguration config)
	{
		ServiceBundle = Microsoft.Identity.Client.Internal.ServiceBundle.Create(config);
		if (config.UserTokenLegacyCachePersistenceForTest != null)
		{
			UserTokenCacheInternal = new TokenCache(ServiceBundle, config.UserTokenLegacyCachePersistenceForTest, isApplicationTokenCache: false);
		}
		else
		{
			UserTokenCacheInternal = new TokenCache(ServiceBundle, isApplicationTokenCache: false);
		}
	}

	internal virtual AuthenticationRequestParameters CreateRequestParameters(AcquireTokenCommonParameters commonParameters, RequestContext requestContext, ITokenCacheInternal cache)
	{
		return new AuthenticationRequestParameters(ServiceBundle, cache, commonParameters, requestContext);
	}

	public async Task<IEnumerable<IAccount>> GetAccountsAsync()
	{
		return await GetAccountsInternalAsync(ApiEvent.ApiIds.GetAccounts).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<IEnumerable<IAccount>> GetAccountsAsync(string userFlow)
	{
		if (string.IsNullOrWhiteSpace(userFlow))
		{
			throw new ArgumentException("userFlow should not be null or whitespace", "userFlow");
		}
		return (await GetAccountsInternalAsync(ApiEvent.ApiIds.GetAccountsByUserFlow).ConfigureAwait(continueOnCapturedContext: false)).Where((IAccount acc) => acc.HomeAccountId.ObjectId.Split(new char[1] { '.' })[0].EndsWith(userFlow, StringComparison.OrdinalIgnoreCase));
	}

	public async Task<IAccount> GetAccountAsync(string accountId)
	{
		return (await GetAccountsFromCacheAsync(ApiEvent.ApiIds.GetAccountById, accountId).ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
	}

	public async Task RemoveAsync(IAccount account)
	{
		RequestContext requestContext = CreateRequestContext(Guid.NewGuid());
		if (account != null && UserTokenCacheInternal != null)
		{
			await UserTokenCacheInternal.RemoveAccountAsync(account, requestContext).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (AppConfig.IsBrokerEnabled && ServiceBundle.PlatformProxy.CanBrokerSupportSilentAuth())
		{
			await ServiceBundle.PlatformProxy.CreateBroker(null).RemoveAccountAsync(AppConfig as IApplicationConfiguration, account).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private async Task<IEnumerable<IAccount>> GetAccountsInternalAsync(ApiEvent.ApiIds apiId, string homeAccountIdFilter = null)
	{
		IEnumerable<IAccount> accountsFromCache = await GetAccountsFromCacheAsync(apiId, homeAccountIdFilter).ConfigureAwait(continueOnCapturedContext: false);
		IEnumerable<IAccount> brokerAccounts = await GetAccountsFromBrokerAsync(homeAccountIdFilter).ConfigureAwait(continueOnCapturedContext: false);
		ServiceBundle.DefaultLogger.Info($"Found {accountsFromCache.Count()} cache accounts and {accountsFromCache.Count()} broker accounts");
		IEnumerable<IAccount> enumerable = MergeAccounts(accountsFromCache, brokerAccounts);
		ServiceBundle.DefaultLogger.Verbose($"Returning {enumerable.Count()} accounts");
		return enumerable;
	}

	private async Task<IEnumerable<IAccount>> GetAccountsFromBrokerAsync(string homeAccountIdFilter)
	{
		if (AppConfig.IsBrokerEnabled && ServiceBundle.PlatformProxy.CanBrokerSupportSilentAuth())
		{
			IEnumerable<IAccount> enumerable = (await ServiceBundle.PlatformProxy.CreateBroker(null).GetAccountsAsync(AppConfig.ClientId, AppConfig.RedirectUri).ConfigureAwait(continueOnCapturedContext: false)) ?? Enumerable.Empty<IAccount>();
			if (!string.IsNullOrEmpty(homeAccountIdFilter))
			{
				enumerable = enumerable.Where((IAccount acc) => homeAccountIdFilter.Equals(acc.HomeAccountId.Identifier, StringComparison.OrdinalIgnoreCase));
			}
			return await FilterBrokerAccountsByEnvAsync(enumerable).ConfigureAwait(continueOnCapturedContext: false);
		}
		return Enumerable.Empty<IAccount>();
	}

	private async Task<IEnumerable<IAccount>> FilterBrokerAccountsByEnvAsync(IEnumerable<IAccount> brokerAccounts)
	{
		ServiceBundle.DefaultLogger.Verbose("Filtering broker accounts by env. Before filtering: " + brokerAccounts.Count());
		ISet<string> existingEnvironmentsInCache = new HashSet<string>(brokerAccounts.Select((IAccount aci) => aci.Environment), StringComparer.OrdinalIgnoreCase);
		InstanceDiscoveryMetadataEntry instanceMetadata = await ServiceBundle.InstanceDiscoveryManager.GetMetadataEntryTryAvoidNetworkAsync(Authority, existingEnvironmentsInCache, CreateRequestContext(Guid.NewGuid())).ConfigureAwait(continueOnCapturedContext: false);
		brokerAccounts = brokerAccounts.Where((IAccount acc) => instanceMetadata.Aliases.ContainsOrdinalIgnoreCase(acc.Environment));
		ServiceBundle.DefaultLogger.Verbose("After filtering: " + brokerAccounts.Count());
		return brokerAccounts;
	}

	private IEnumerable<IAccount> MergeAccounts(IEnumerable<IAccount> cacheAccounts, IEnumerable<IAccount> brokerAccounts)
	{
		List<IAccount> list = new List<IAccount>(cacheAccounts);
		foreach (IAccount account in brokerAccounts)
		{
			if (!cacheAccounts.Any((IAccount x) => x.HomeAccountId.Equals(account.HomeAccountId)))
			{
				list.Add(account);
			}
		}
		return list;
	}

	private async Task<IEnumerable<IAccount>> GetAccountsFromCacheAsync(ApiEvent.ApiIds apiId, string homeAccountIdFilter)
	{
		RequestContext requestContext = CreateRequestContext(Guid.NewGuid());
		AuthenticationRequestParameters requestParams = new AuthenticationRequestParameters(ServiceBundle, UserTokenCacheInternal, new AcquireTokenCommonParameters
		{
			ApiId = apiId
		}, requestContext, homeAccountIdFilter);
		return await new CacheSessionManager(UserTokenCacheInternal, requestParams).GetAccountsAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private RequestContext CreateRequestContext(Guid correlationId)
	{
		return new RequestContext(ServiceBundle, correlationId);
	}

	public AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, IAccount account)
	{
		return AcquireTokenSilentParameterBuilder.Create(ClientExecutorFactory.CreateClientApplicationBaseExecutor(this), scopes, account);
	}

	public AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, string loginHint)
	{
		if (string.IsNullOrWhiteSpace(loginHint))
		{
			throw new ArgumentNullException("loginHint");
		}
		return AcquireTokenSilentParameterBuilder.Create(ClientExecutorFactory.CreateClientApplicationBaseExecutor(this), scopes, loginHint);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use GetAccountAsync instead and pass IAccount.HomeAccountId.Identifier (See https://aka.ms/msal-net-2-released)", true)]
	public IUser GetUser(string identifier)
	{
		throw new NotImplementedException();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use RemoveAccountAsync instead (See https://aka.ms/msal-net-2-released)", true)]
	public void Remove(IUser user)
	{
		throw new NotImplementedException();
	}

	[Obsolete("Use AcquireTokenSilent instead.See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenSilentAsync(IEnumerable<string> scopes, IAccount account, string authority, bool forceRefresh)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenSilent instead.See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenSilentAsync(IEnumerable<string> scopes, IAccount account)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}
}
