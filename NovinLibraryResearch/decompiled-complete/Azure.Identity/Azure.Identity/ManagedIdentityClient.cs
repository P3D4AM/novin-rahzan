using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Azure.Identity;

internal class ManagedIdentityClient
{
	internal const string MsiUnavailableError = "ManagedIdentityCredential authentication unavailable. No Managed Identity endpoint found.";

	private readonly AsyncLockWithValue<ManagedIdentitySource> _identitySourceAsyncLock = new AsyncLockWithValue<ManagedIdentitySource>();

	private readonly ManagedIdentityClientOptions _options;

	internal CredentialPipeline Pipeline { get; }

	protected string ClientId { get; }

	protected ManagedIdentityClient()
	{
	}

	public ManagedIdentityClient(CredentialPipeline pipeline, string clientId = null)
		: this(new ManagedIdentityClientOptions
		{
			Pipeline = pipeline,
			ClientId = clientId
		})
	{
	}

	public ManagedIdentityClient(ManagedIdentityClientOptions options)
	{
		_options = options;
		ClientId = options.ClientId;
		Pipeline = options.Pipeline;
	}

	public virtual async ValueTask<AccessToken> AuthenticateAsync(bool async, TokenRequestContext context, CancellationToken cancellationToken)
	{
		return await ((await GetManagedIdentitySourceAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) ?? throw new CredentialUnavailableException("ManagedIdentityCredential authentication unavailable. No Managed Identity endpoint found.")).AuthenticateAsync(async, context, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private protected virtual async ValueTask<ManagedIdentitySource> GetManagedIdentitySourceAsync(bool async, CancellationToken cancellationToken)
	{
		using AsyncLockWithValue<ManagedIdentitySource>.Lock asyncLock = await _identitySourceAsyncLock.GetLockOrValueAsync(async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (asyncLock.HasValue)
		{
			return asyncLock.Value;
		}
		ManagedIdentitySource managedIdentitySource = AppServiceV2017ManagedIdentitySource.TryCreate(_options);
		if (managedIdentitySource == null)
		{
			ManagedIdentitySource managedIdentitySource2 = CloudShellManagedIdentitySource.TryCreate(_options);
			if (managedIdentitySource2 == null)
			{
				ManagedIdentitySource managedIdentitySource3 = AzureArcManagedIdentitySource.TryCreate(_options);
				if (managedIdentitySource3 == null)
				{
					ManagedIdentitySource managedIdentitySource4 = ServiceFabricManagedIdentitySource.TryCreate(_options);
					if (managedIdentitySource4 == null)
					{
						managedIdentitySource4 = await ImdsManagedIdentitySource.TryCreateAsync(_options, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
					managedIdentitySource3 = managedIdentitySource4;
				}
				managedIdentitySource2 = managedIdentitySource3;
			}
			managedIdentitySource = managedIdentitySource2;
		}
		ManagedIdentitySource managedIdentitySource5 = managedIdentitySource;
		asyncLock.SetValue(managedIdentitySource5);
		return managedIdentitySource5;
	}
}
