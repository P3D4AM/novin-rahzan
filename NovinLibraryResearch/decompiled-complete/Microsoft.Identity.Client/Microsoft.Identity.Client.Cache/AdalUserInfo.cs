using System;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Cache;

[JsonObject]
[Preserve(AllMembers = true)]
internal sealed class AdalUserInfo
{
	[JsonProperty]
	public string UniqueId { get; internal set; }

	[JsonProperty]
	public string DisplayableId { get; internal set; }

	[JsonProperty]
	public string GivenName { get; internal set; }

	[JsonProperty]
	public string FamilyName { get; internal set; }

	[JsonProperty]
	public DateTimeOffset? PasswordExpiresOn { get; internal set; }

	[JsonProperty]
	public Uri PasswordChangeUrl { get; internal set; }

	[JsonProperty]
	public string IdentityProvider { get; internal set; }

	public AdalUserInfo()
	{
	}

	public AdalUserInfo(AdalUserInfo other)
	{
		if (other != null)
		{
			UniqueId = other.UniqueId;
			DisplayableId = other.DisplayableId;
			GivenName = other.GivenName;
			FamilyName = other.FamilyName;
			IdentityProvider = other.IdentityProvider;
			PasswordChangeUrl = other.PasswordChangeUrl;
			PasswordExpiresOn = other.PasswordExpiresOn;
		}
	}
}
