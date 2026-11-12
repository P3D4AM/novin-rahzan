using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Region;

internal sealed class RegionDiscoveryProvider : IRegionDiscoveryProvider
{
	private const string RegionName = "REGION_NAME";

	private readonly Uri _ImdsUri = new Uri("http://169.254.169.254/metadata/instance/compute?api-version=2020-06-01");

	private IDictionary<string, string> Headers;

	private readonly IHttpManager _httpManager;

	private static INetworkCacheMetadataProvider _networkCacheMetadataProvider;

	public RegionDiscoveryProvider(IHttpManager httpManager, INetworkCacheMetadataProvider networkCacheMetadataProvider = null)
	{
		_httpManager = httpManager;
		Headers = new Dictionary<string, string>();
		Headers.Add("Metadata", "true");
		_networkCacheMetadataProvider = networkCacheMetadataProvider ?? new NetworkCacheMetadataProvider();
	}

	public async Task<InstanceDiscoveryMetadataEntry> GetMetadataAsync(Uri authority, RequestContext requestContext)
	{
		ICoreLogger logger = requestContext.Logger;
		string environment = authority.Host;
		InstanceDiscoveryMetadataEntry metadata = _networkCacheMetadataProvider.GetMetadata(environment, logger);
		if (metadata == null)
		{
			CacheInstanceDiscoveryMetadata(CreateEntry(authority, await BuildAuthorityWithRegionAsync(authority, requestContext.Logger).ConfigureAwait(continueOnCapturedContext: false)));
			metadata = _networkCacheMetadataProvider.GetMetadata(environment, logger);
			logger.Verbose($"[Region Discovery] Created metadata for the regional environment {environment} ? {metadata != null}");
		}
		else
		{
			logger.Verbose("[Region Discovery] The network provider found an entry for " + environment);
		}
		requestContext.ApiEvent.RegionDiscovered = metadata.PreferredNetwork.Split(new char[1] { '.' })[0];
		return metadata;
	}

	private async Task<string> GetRegionAsync(ICoreLogger logger)
	{
		if (!Environment.GetEnvironmentVariable("REGION_NAME").IsNullOrEmpty())
		{
			logger.Info("[Region discovery] Region: " + Environment.GetEnvironmentVariable("REGION_NAME"));
			return Environment.GetEnvironmentVariable("REGION_NAME");
		}
		try
		{
			HttpResponse obj = await _httpManager.SendGetAsync(_ImdsUri, Headers, logger).ConfigureAwait(continueOnCapturedContext: false);
			if (obj.StatusCode != HttpStatusCode.OK)
			{
				throw new MsalClientException("region_discovery_failed", "Region discovery for the instance failed. Region discovery can only be made if the service resides in Azure function or Azure VM. See https://aka.ms/msal-net-region-discovery for more details.");
			}
			LocalImdsResponse localImdsResponse = JsonHelper.DeserializeFromJson<LocalImdsResponse>(obj.Body);
			logger.Info("[Region discovery] Call to local IMDS returned region: " + localImdsResponse.location);
			return localImdsResponse.location;
		}
		catch (MsalClientException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			logger.Info("[Region discovery] Call to local imds failed." + ex2.Message);
			throw new MsalClientException("region_discovery_failed", "Region discovery for the instance failed. Region discovery can only be made if the service resides in Azure function or Azure VM. See https://aka.ms/msal-net-region-discovery for more details.");
		}
	}

	private static InstanceDiscoveryMetadataEntry CreateEntry(Uri orginalAuthority, Uri regionalizedAuthority)
	{
		InstanceDiscoveryMetadataEntry instanceDiscoveryMetadataEntry = new InstanceDiscoveryMetadataEntry();
		instanceDiscoveryMetadataEntry.Aliases = new string[2] { orginalAuthority.Host, regionalizedAuthority.Host };
		instanceDiscoveryMetadataEntry.PreferredCache = orginalAuthority.Host;
		instanceDiscoveryMetadataEntry.PreferredNetwork = regionalizedAuthority.Host;
		return instanceDiscoveryMetadataEntry;
	}

	private void CacheInstanceDiscoveryMetadata(InstanceDiscoveryMetadataEntry metadataEntry)
	{
		IEnumerable<string> aliases = metadataEntry.Aliases;
		foreach (string item in aliases ?? Enumerable.Empty<string>())
		{
			_networkCacheMetadataProvider.AddMetadata(item, metadataEntry);
		}
	}

	private async Task<Uri> BuildAuthorityWithRegionAsync(Uri canonicalAuthority, ICoreLogger logger)
	{
		string text = await GetRegionAsync(logger).ConfigureAwait(continueOnCapturedContext: false);
		UriBuilder uriBuilder = new UriBuilder(canonicalAuthority);
		if (KnownMetadataProvider.IsPublicEnvironment(canonicalAuthority.Host))
		{
			uriBuilder.Host = text + ".login.microsoft.com";
		}
		else
		{
			uriBuilder.Host = text + "." + uriBuilder.Host;
		}
		return uriBuilder.Uri;
	}
}
