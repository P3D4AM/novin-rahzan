using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace Azure.Identity;

internal abstract class MsalClientBase<TClient> where TClient : IClientApplicationBase
{
	private static readonly string s_msalCacheClientId = Guid.NewGuid().ToString();

	private readonly AsyncLockWithValue<TClient> _clientAsyncLock;

	internal string TenantId { get; }

	internal string ClientId { get; }

	internal bool EnablePersistentCache { get; }

	internal bool AllowUnencryptedCache { get; }

	protected CredentialPipeline Pipeline { get; }

	protected MsalClientBase()
	{
	}

	protected MsalClientBase(CredentialPipeline pipeline, string tenantId, string clientId, ITokenCacheOptions cacheOptions)
	{
		Validations.ValidateAuthorityHost(pipeline.AuthorityHost);
		Pipeline = pipeline;
		TenantId = tenantId;
		ClientId = clientId;
		EnablePersistentCache = cacheOptions?.EnablePersistentCache ?? false;
		AllowUnencryptedCache = cacheOptions?.AllowUnencryptedCache ?? false;
		_clientAsyncLock = new AsyncLockWithValue<TClient>();
	}

	protected abstract ValueTask<TClient> CreateClientAsync(bool async, CancellationToken cancellationToken);

	protected async ValueTask<TClient> GetClientAsync(bool async, CancellationToken cancellationToken)
	{
		using AsyncLockWithValue<TClient>.Lock asyncLock = await _clientAsyncLock.GetLockOrValueAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (asyncLock.HasValue)
		{
			return asyncLock.Value;
		}
		TClient client = await CreateClientAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (EnablePersistentCache)
		{
			StorageCreationProperties storageProperties = new StorageCreationPropertiesBuilder("msal.cache", Constants.DefaultMsalTokenCacheDirectory, s_msalCacheClientId).WithMacKeyChain("Microsoft.Developer.IdentityService", "MSALCache").WithLinuxKeyring("msal.cache", "default", "MSALCache", Constants.DefaultMsaltokenCacheKeyringAttribute1, Constants.DefaultMsaltokenCacheKeyringAttribute2).Build();
			MsalCacheHelper cacheHelper;
			try
			{
				cacheHelper = await CreateCacheHelper(storageProperties, async).ConfigureAwait(continueOnCapturedContext: false);
				cacheHelper.VerifyPersistence();
			}
			catch (MsalCachePersistenceException)
			{
				if (!AllowUnencryptedCache)
				{
					throw;
				}
				storageProperties = new StorageCreationPropertiesBuilder("msal.cache", Constants.DefaultMsalTokenCacheDirectory, s_msalCacheClientId).WithMacKeyChain("Microsoft.Developer.IdentityService", "MSALCache").WithLinuxUnprotectedFile().Build();
				cacheHelper = await CreateCacheHelper(storageProperties, async).ConfigureAwait(continueOnCapturedContext: false);
				cacheHelper.VerifyPersistence();
			}
			cacheHelper.RegisterCache(client.UserTokenCache);
		}
		asyncLock.SetValue(client);
		return client;
	}

	private static async ValueTask<MsalCacheHelper> CreateCacheHelper(StorageCreationProperties storageProperties, bool async)
	{
		return (!async) ? MsalCacheHelper.CreateAsync(storageProperties).GetAwaiter().GetResult() : (await MsalCacheHelper.CreateAsync(storageProperties).ConfigureAwait(continueOnCapturedContext: false));
	}
}
