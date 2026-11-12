using System.Collections.Generic;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.OAuth2;

[JsonObject]
[Preserve(AllMembers = true)]
internal class DeviceAuthHeader
{
	[JsonProperty("x5c")]
	public IList<string> X5c { get; set; }

	[JsonProperty("typ")]
	public string Type { get; set; }

	[JsonProperty("alg")]
	public string Alg { get; private set; }

	public DeviceAuthHeader(string base64EncodedCertificate)
	{
		Alg = "RS256";
		Type = "JWT";
		X5c = new List<string>();
		X5c.Add(base64EncodedCertificate);
	}
}
