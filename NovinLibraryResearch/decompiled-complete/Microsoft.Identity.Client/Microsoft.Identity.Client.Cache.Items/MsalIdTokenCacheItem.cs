using System;
using System.Globalization;
using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

internal class MsalIdTokenCacheItem : MsalCredentialCacheItemBase
{
	private readonly Lazy<IdToken> idTokenLazy;

	internal bool IsAdfs { get; set; }

	internal string TenantId { get; set; }

	internal string Authority
	{
		get
		{
			if (!IsAdfs)
			{
				return string.Format(CultureInfo.InvariantCulture, "https://{0}/{1}/", base.Environment, TenantId ?? "common");
			}
			return string.Format(CultureInfo.InvariantCulture, "https://{0}/{1}/", base.Environment, "adfs");
		}
	}

	internal IdToken IdToken => idTokenLazy.Value;

	internal MsalIdTokenCacheItem()
	{
		base.CredentialType = "IdToken";
		idTokenLazy = new Lazy<IdToken>(() => IdToken.Parse(base.Secret));
	}

	internal MsalIdTokenCacheItem(string preferredCacheEnv, string clientId, MsalTokenResponse response, string tenantId, string homeAccountId)
		: this(preferredCacheEnv, clientId, response.IdToken, response.ClientInfo, homeAccountId, tenantId)
	{
	}

	internal MsalIdTokenCacheItem(string preferredCacheEnv, string clientId, string secret, string rawClientInfo, string homeAccountId, string tenantId)
		: this()
	{
		base.Environment = preferredCacheEnv;
		TenantId = tenantId;
		base.ClientId = clientId;
		base.Secret = secret;
		base.RawClientInfo = rawClientInfo;
		base.HomeAccountId = homeAccountId;
	}

	internal MsalIdTokenCacheKey GetKey()
	{
		return new MsalIdTokenCacheKey(base.Environment, TenantId, base.HomeAccountId, base.ClientId);
	}

	internal static MsalIdTokenCacheItem FromJsonString(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
		{
			return null;
		}
		return FromJObject(JObject.Parse(json));
	}

	internal static MsalIdTokenCacheItem FromJObject(JObject j)
	{
		MsalIdTokenCacheItem msalIdTokenCacheItem = new MsalIdTokenCacheItem();
		msalIdTokenCacheItem.TenantId = JsonUtils.ExtractExistingOrEmptyString(j, "realm");
		msalIdTokenCacheItem.PopulateFieldsFromJObject(j);
		return msalIdTokenCacheItem;
	}

	internal override JObject ToJObject()
	{
		JObject jObject = base.ToJObject();
		SetItemIfValueNotNull(jObject, "realm", TenantId);
		return jObject;
	}

	internal string ToJsonString()
	{
		return ToJObject().ToString();
	}
}
