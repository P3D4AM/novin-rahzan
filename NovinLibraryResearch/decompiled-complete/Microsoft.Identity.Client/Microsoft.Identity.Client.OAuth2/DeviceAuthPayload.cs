using System;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.OAuth2;

[JsonObject]
[Preserve(AllMembers = true)]
internal class DeviceAuthPayload
{
	private Lazy<long> _defaultDeviceAuthJWTTimeSpan = new Lazy<long>(() => (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);

	[JsonProperty("iat")]
	public long Iat { get; set; }

	[JsonProperty("aud")]
	public string Audience { get; set; }

	[JsonProperty("nonce")]
	public string Nonce { get; private set; }

	public DeviceAuthPayload(string audience, string nonce)
	{
		Nonce = nonce;
		Audience = audience;
		Iat = _defaultDeviceAuthJWTTimeSpan.Value;
	}
}
