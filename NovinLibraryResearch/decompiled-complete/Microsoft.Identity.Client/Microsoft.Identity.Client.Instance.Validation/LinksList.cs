using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Instance.Validation;

[JsonObject(Title = "links")]
[Preserve(AllMembers = true)]
internal class LinksList
{
	[JsonProperty(PropertyName = "rel")]
	public string Rel { get; set; }

	[JsonProperty(PropertyName = "href")]
	public string Href { get; set; }
}
