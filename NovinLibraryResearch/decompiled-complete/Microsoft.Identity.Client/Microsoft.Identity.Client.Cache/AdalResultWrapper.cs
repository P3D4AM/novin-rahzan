using System;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Cache;

[JsonObject]
[Preserve(AllMembers = true)]
internal class AdalResultWrapper
{
	[JsonProperty]
	public AdalResult Result { get; set; }

	[JsonProperty]
	public string RawClientInfo { get; set; }

	[JsonProperty]
	public string RefreshToken { get; set; }

	internal bool IsMultipleResourceRefreshToken
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(RefreshToken))
			{
				return !string.IsNullOrWhiteSpace(ResourceInResponse);
			}
			return false;
		}
	}

	[JsonProperty]
	internal string ResourceInResponse { get; set; }

	internal Exception Exception { get; set; }

	[JsonProperty]
	public string UserAssertionHash { get; set; }

	public static AdalResultWrapper Deserialize(string serializedObject)
	{
		return JsonHelper.DeserializeFromJson<AdalResultWrapper>(serializedObject);
	}

	public string Serialize()
	{
		return JsonHelper.SerializeToJson(this);
	}

	internal AdalResultWrapper Clone()
	{
		return Deserialize(Serialize());
	}
}
