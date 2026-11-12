using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Region;

[JsonObject]
[Preserve(AllMembers = true)]
internal sealed class LocalImdsResponse
{
	[JsonProperty(PropertyName = "azEnvironment")]
	public string azEnvironment { get; set; }

	[JsonProperty(PropertyName = "location")]
	public string location { get; set; }
}
