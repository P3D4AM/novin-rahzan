using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Region;
using Microsoft.Identity.Client.TelemetryCore;

namespace Microsoft.Identity.Client.Instance.Discovery;

internal class InstanceDiscoveryManager : IInstanceDiscoveryManager
{
	private readonly IHttpManager _httpManager;

	private readonly IMatsTelemetryManager _telemetryManager;

	private readonly IUserMetadataProvider _userMetadataProvider;

	private readonly IKnownMetadataProvider _knownMetadataProvider;

	private readonly INetworkCacheMetadataProvider _networkCacheMetadataProvider;

	private readonly INetworkMetadataProvider _networkMetadataProvider;

	private readonly IRegionDiscoveryProvider _regionDiscoveryProvider;

	public InstanceDiscoveryManager(IHttpManager httpManager, IMatsTelemetryManager telemetryManager, bool shouldClearCaches, InstanceDiscoveryResponse userProvidedInstanceDiscoveryResponse = null, Uri userProvidedInstanceDiscoveryUri = null)
		: this(httpManager, telemetryManager, shouldClearCaches, (userProvidedInstanceDiscoveryResponse != null) ? new UserMetadataProvider(userProvidedInstanceDiscoveryResponse) : null, userProvidedInstanceDiscoveryUri)
	{
	}

	public InstanceDiscoveryManager(IHttpManager httpManager, IMatsTelemetryManager telemetryManager, bool shouldClearCaches, IUserMetadataProvider userMetadataProvider = null, Uri userProvidedInstanceDiscoveryUri = null, IKnownMetadataProvider knownMetadataProvider = null, INetworkCacheMetadataProvider networkCacheMetadataProvider = null, INetworkMetadataProvider networkMetadataProvider = null, IRegionDiscoveryProvider regionDiscovery = null)
	{
		_httpManager = httpManager ?? throw new ArgumentNullException("httpManager");
		_telemetryManager = telemetryManager ?? throw new ArgumentNullException("telemetryManager");
		_userMetadataProvider = userMetadataProvider;
		_knownMetadataProvider = knownMetadataProvider ?? new KnownMetadataProvider();
		_networkCacheMetadataProvider = networkCacheMetadataProvider ?? new NetworkCacheMetadataProvider();
		_networkMetadataProvider = networkMetadataProvider ?? new NetworkMetadataProvider(_httpManager, _telemetryManager, _networkCacheMetadataProvider, userProvidedInstanceDiscoveryUri);
		_regionDiscoveryProvider = regionDiscovery ?? new RegionDiscoveryProvider(_httpManager, _networkCacheMetadataProvider);
		if (shouldClearCaches)
		{
			_networkCacheMetadataProvider.Clear();
		}
	}

	public async Task<InstanceDiscoveryMetadataEntry> GetMetadataEntryTryAvoidNetworkAsync(string authority, IEnumerable<string> existingEnvironmentsInCache, RequestContext requestContext)
	{
		AuthorityType authorityType = Authority.GetAuthorityType(authority);
		string host = new Uri(authority).Host;
		switch (authorityType)
		{
		case AuthorityType.Aad:
		{
			InstanceDiscoveryMetadataEntry instanceDiscoveryMetadataEntry = _userMetadataProvider?.GetMetadataOrThrow(host, requestContext.Logger);
			if (instanceDiscoveryMetadataEntry == null)
			{
				InstanceDiscoveryMetadataEntry instanceDiscoveryMetadataEntry2 = _networkCacheMetadataProvider.GetMetadata(host, requestContext.Logger);
				if (instanceDiscoveryMetadataEntry2 == null)
				{
					InstanceDiscoveryMetadataEntry instanceDiscoveryMetadataEntry3 = _knownMetadataProvider.GetMetadata(host, existingEnvironmentsInCache, requestContext.Logger);
					if (instanceDiscoveryMetadataEntry3 == null)
					{
						instanceDiscoveryMetadataEntry3 = await GetMetadataEntryAsync(authority, requestContext).ConfigureAwait(continueOnCapturedContext: false);
					}
					instanceDiscoveryMetadataEntry2 = instanceDiscoveryMetadataEntry3;
				}
				instanceDiscoveryMetadataEntry = instanceDiscoveryMetadataEntry2;
			}
			return instanceDiscoveryMetadataEntry;
		}
		case AuthorityType.Adfs:
		case AuthorityType.B2C:
			requestContext.Logger.Info("[Instance Discovery] Skipping Instance discovery for non-AAD authority");
			return await GetMetadataEntryAsync(authority, requestContext).ConfigureAwait(continueOnCapturedContext: false);
		default:
			throw new InvalidOperationException("Unexpected authority type " + authorityType);
		}
	}

	public async Task<InstanceDiscoveryMetadataEntry> GetMetadataEntryAsync(string authority, RequestContext requestContext)
	{
		if (requestContext.ServiceBundle.Config.AuthorityInfo.AutoDetectRegion)
		{
			return await _regionDiscoveryProvider.GetMetadataAsync(new Uri(authority), requestContext).ConfigureAwait(continueOnCapturedContext: false);
		}
		AuthorityType authorityType = Authority.GetAuthorityType(authority);
		Uri authorityUri = new Uri(authority);
		string host = authorityUri.Host;
		switch (authorityType)
		{
		case AuthorityType.Aad:
		{
			InstanceDiscoveryMetadataEntry instanceDiscoveryMetadataEntry = _userMetadataProvider?.GetMetadataOrThrow(host, requestContext.Logger);
			if (instanceDiscoveryMetadataEntry == null)
			{
				instanceDiscoveryMetadataEntry = await FetchNetworkMetadataOrFallbackAsync(requestContext, authorityUri).ConfigureAwait(continueOnCapturedContext: false);
			}
			InstanceDiscoveryMetadataEntry instanceDiscoveryMetadataEntry2 = instanceDiscoveryMetadataEntry;
			if (instanceDiscoveryMetadataEntry2 == null)
			{
				string text = "[Instance Discovery] Instance metadata for this authority could neither be fetched nor found. MSAL will continue regardless. SSO might be broken if authority aliases exist. ";
				requestContext.Logger.WarningPii(text + "Authority: " + authority, text);
				instanceDiscoveryMetadataEntry2 = CreateEntryForSingleAuthority(authorityUri);
			}
			return instanceDiscoveryMetadataEntry2;
		}
		case AuthorityType.Adfs:
		case AuthorityType.B2C:
			requestContext.Logger.Info("[Instance Discovery] Skipping Instance discovery for non-AAD authority");
			return CreateEntryForSingleAuthority(authorityUri);
		default:
			throw new InvalidOperationException("Unexpected authority type " + authorityType);
		}
	}

	private async Task<InstanceDiscoveryMetadataEntry> FetchNetworkMetadataOrFallbackAsync(RequestContext requestContext, Uri authorityUri)
	{
		try
		{
			return await _networkMetadataProvider.GetMetadataAsync(authorityUri, requestContext).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (MsalServiceException ex)
		{
			if (!requestContext.ServiceBundle.Config.AuthorityInfo.ValidateAuthority)
			{
				requestContext.Logger.Info("[Instance Discovery] Skipping Instance discovery as validate authority is set to false.");
				return CreateEntryForSingleAuthority(authorityUri);
			}
			if (ex.ErrorCode == "invalid_instance")
			{
				throw;
			}
			string text = "[Instance Discovery] Instance Discovery failed. Potential cause: no network connection or discovery endpoint is busy. See exception below. MSAL will continue without network instance metadata.";
			requestContext.Logger.WarningPii(text + " Authority: " + authorityUri, text);
			requestContext.Logger.WarningPii(ex);
			return _knownMetadataProvider.GetMetadata(authorityUri.Host, Enumerable.Empty<string>(), requestContext.Logger);
		}
	}

	internal void AddTestValueToStaticProvider(string environment, InstanceDiscoveryMetadataEntry entry)
	{
		_networkCacheMetadataProvider.AddMetadata(environment, entry);
	}

	private static InstanceDiscoveryMetadataEntry CreateEntryForSingleAuthority(Uri authority)
	{
		InstanceDiscoveryMetadataEntry instanceDiscoveryMetadataEntry = new InstanceDiscoveryMetadataEntry();
		instanceDiscoveryMetadataEntry.Aliases = new string[1] { authority.Host };
		instanceDiscoveryMetadataEntry.PreferredCache = authority.Host;
		instanceDiscoveryMetadataEntry.PreferredNetwork = authority.Host;
		return instanceDiscoveryMetadataEntry;
	}
}
