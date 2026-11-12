using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.Instance.Discovery;

internal class UserMetadataProvider : IUserMetadataProvider
{
	private readonly IDictionary<string, InstanceDiscoveryMetadataEntry> _entries = new Dictionary<string, InstanceDiscoveryMetadataEntry>();

	public UserMetadataProvider(InstanceDiscoveryResponse instanceDiscoveryResponse)
	{
		foreach (InstanceDiscoveryMetadataEntry item in instanceDiscoveryResponse?.Metadata ?? Enumerable.Empty<InstanceDiscoveryMetadataEntry>())
		{
			IEnumerable<string> aliases = item.Aliases;
			foreach (string item2 in aliases ?? Enumerable.Empty<string>())
			{
				_entries.Add(item2, item);
			}
		}
	}

	public InstanceDiscoveryMetadataEntry GetMetadataOrThrow(string environment, ICoreLogger logger)
	{
		_entries.TryGetValue(environment ?? "", out var value);
		logger.Verbose($"[Instance Discovery] Tried to use user metadata provider for {environment}. Success? {value != null}");
		if (value == null)
		{
			throw new MsalClientException("invalid-custom-instance-metadata", MsalErrorMessage.NoUserInstanceMetadataEntry(environment));
		}
		return value;
	}
}
