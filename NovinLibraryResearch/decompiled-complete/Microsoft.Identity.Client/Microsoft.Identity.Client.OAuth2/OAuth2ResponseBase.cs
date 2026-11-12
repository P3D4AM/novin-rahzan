using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.OAuth2;

[JsonObject]
[Preserve(AllMembers = true)]
internal class OAuth2ResponseBase
{
	[JsonProperty(PropertyName = "error")]
	public string Error { get; set; }

	[JsonProperty(PropertyName = "suberror")]
	public string SubError { get; set; }

	[JsonProperty(PropertyName = "error_description")]
	public string ErrorDescription { get; set; }

	[JsonProperty(PropertyName = "error_codes")]
	public string[] ErrorCodes { get; set; }

	[JsonProperty(PropertyName = "correlation_id")]
	public string CorrelationId { get; set; }

	[JsonProperty(PropertyName = "claims")]
	public string Claims { get; set; }
}
