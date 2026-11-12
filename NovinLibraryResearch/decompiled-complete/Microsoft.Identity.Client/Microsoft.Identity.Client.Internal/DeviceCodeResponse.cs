using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Internal;

[JsonObject]
[Preserve(AllMembers = true)]
internal class DeviceCodeResponse : OAuth2ResponseBase
{
	[JsonProperty(PropertyName = "user_code")]
	public string UserCode { get; internal set; }

	[JsonProperty(PropertyName = "device_code")]
	public string DeviceCode { get; internal set; }

	[JsonProperty(PropertyName = "verification_url")]
	public string VerificationUrl { get; internal set; }

	[JsonProperty(PropertyName = "verification_uri")]
	public string VerificationUri { get; internal set; }

	[JsonProperty(PropertyName = "expires_in")]
	public long ExpiresIn { get; internal set; }

	[JsonProperty(PropertyName = "interval")]
	public long Interval { get; internal set; }

	[JsonProperty(PropertyName = "message")]
	public string Message { get; internal set; }

	public DeviceCodeResult GetResult(string clientId, ISet<string> scopes)
	{
		string verificationUrl = (string.IsNullOrWhiteSpace(VerificationUri) ? VerificationUrl : VerificationUri);
		return new DeviceCodeResult(UserCode, DeviceCode, verificationUrl, DateTime.UtcNow.AddSeconds(ExpiresIn), Interval, Message, clientId, scopes);
	}
}
