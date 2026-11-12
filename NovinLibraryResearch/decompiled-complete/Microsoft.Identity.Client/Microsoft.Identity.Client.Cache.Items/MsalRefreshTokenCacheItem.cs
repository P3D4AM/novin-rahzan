using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

internal class MsalRefreshTokenCacheItem : MsalCredentialCacheItemBase
{
	public string FamilyId { get; set; }

	public bool IsFRT => !string.IsNullOrEmpty(FamilyId);

	internal MsalRefreshTokenCacheItem()
	{
		base.CredentialType = "RefreshToken";
	}

	internal MsalRefreshTokenCacheItem(string preferredCacheEnv, string clientId, MsalTokenResponse response, string homeAccountId)
		: this(preferredCacheEnv, clientId, response.RefreshToken, response.ClientInfo, response.FamilyId, homeAccountId)
	{
	}

	internal MsalRefreshTokenCacheItem(string preferredCacheEnv, string clientId, string secret, string rawClientInfo, string familyId, string homeAccountId)
		: this()
	{
		base.ClientId = clientId;
		base.Environment = preferredCacheEnv;
		base.Secret = secret;
		base.RawClientInfo = rawClientInfo;
		FamilyId = familyId;
		base.HomeAccountId = homeAccountId;
	}

	internal MsalRefreshTokenCacheKey GetKey()
	{
		return new MsalRefreshTokenCacheKey(base.Environment, base.ClientId, base.HomeAccountId, FamilyId);
	}

	internal static MsalRefreshTokenCacheItem FromJsonString(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
		{
			return null;
		}
		return FromJObject(JObject.Parse(json));
	}

	internal static MsalRefreshTokenCacheItem FromJObject(JObject j)
	{
		MsalRefreshTokenCacheItem msalRefreshTokenCacheItem = new MsalRefreshTokenCacheItem();
		msalRefreshTokenCacheItem.FamilyId = JsonUtils.ExtractExistingOrEmptyString(j, "family_id");
		msalRefreshTokenCacheItem.PopulateFieldsFromJObject(j);
		return msalRefreshTokenCacheItem;
	}

	internal override JObject ToJObject()
	{
		JObject jObject = base.ToJObject();
		SetItemIfValueNotNull(jObject, "family_id", FamilyId);
		return jObject;
	}

	internal string ToJsonString()
	{
		return ToJObject().ToString();
	}
}
