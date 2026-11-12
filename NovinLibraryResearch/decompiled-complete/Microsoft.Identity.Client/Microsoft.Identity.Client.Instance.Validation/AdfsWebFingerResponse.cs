using System.Collections.Generic;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Instance.Validation;

[JsonObject]
[Preserve(AllMembers = true)]
internal class AdfsWebFingerResponse : OAuth2ResponseBase
{
	[JsonProperty(PropertyName = "subject")]
	public string Subject { get; set; }

	[JsonProperty(PropertyName = "links")]
	public List<LinksList> Links { get; set; }
}
