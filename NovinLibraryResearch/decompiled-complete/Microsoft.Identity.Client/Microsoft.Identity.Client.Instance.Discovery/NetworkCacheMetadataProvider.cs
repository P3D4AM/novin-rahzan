using System.Collections.Concurrent;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.Instance.Discovery;

internal class NetworkCacheMetadataProvider : INetworkCacheMetadataProvider
{
	private static readonly ConcurrentDictionary<string, InstanceDiscoveryMetadataEntry> s_cache = new ConcurrentDictionary<string, InstanceDiscoveryMetadataEntry>();

	public InstanceDiscoveryMetadataEntry GetMetadata(string environment, ICoreLogger logger)
	{
		s_cache.TryGetValue(environment, out var value);
		logger.Verbose($"[Instance Discovery] Tried to use network cache provider for {environment}. Success? {value != null}");
		return value;
	}

	public void AddMetadata(string environment, InstanceDiscoveryMetadataEntry entry)
	{
		s_cache.AddOrUpdate(environment, entry, (string key, InstanceDiscoveryMetadataEntry oldValue) => entry);
	}

	public void Clear()
	{
		s_cache.Clear();
	}
}
