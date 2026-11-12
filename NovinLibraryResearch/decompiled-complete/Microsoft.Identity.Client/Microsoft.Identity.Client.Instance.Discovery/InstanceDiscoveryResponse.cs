using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Instance.Discovery;

[JsonObject]
[Preserve(AllMembers = true)]
internal sealed class InstanceDiscoveryResponse : OAuth2ResponseBase
{
	[JsonProperty(PropertyName = "tenant_discovery_endpoint")]
	public string TenantDiscoveryEndpoint { get; set; }

	[JsonProperty(PropertyName = "metadata")]
	public InstanceDiscoveryMetadataEntry[] Metadata { get; set; }
}
