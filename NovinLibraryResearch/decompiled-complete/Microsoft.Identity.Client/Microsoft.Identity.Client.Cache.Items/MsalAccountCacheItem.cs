using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

[DebuggerDisplay("{PreferredUsername} {base.Environment}")]
internal class MsalAccountCacheItem : MsalCacheItemBase
{
	internal string TenantId { get; set; }

	internal string PreferredUsername { get; set; }

	internal string Name { get; set; }

	internal string GivenName { get; set; }

	internal string FamilyName { get; set; }

	internal string LocalAccountId { get; set; }

	internal string AuthorityType { get; set; }

	internal IDictionary<string, string> WamAccountIds { get; set; }

	internal MsalAccountCacheItem()
	{
		AuthorityType = CacheAuthorityType.MSSTS.ToString();
	}

	internal MsalAccountCacheItem(string preferredCacheEnv, string clientInfo, string homeAccountId, IdToken idToken, string preferredUsername, string tenantId, IDictionary<string, string> wamAccountIds)
		: this()
	{
		Init(preferredCacheEnv, idToken?.ObjectId, clientInfo, homeAccountId, idToken?.Name, preferredUsername, tenantId, idToken?.GivenName, idToken?.FamilyName, wamAccountIds);
	}

	internal MsalAccountCacheItem(string environment, string localAccountId, string rawClientInfo, string homeAccountId, string name, string preferredUsername, string tenantId, string givenName, string familyName, IDictionary<string, string> wamAccountIds)
		: this()
	{
		Init(environment, localAccountId, rawClientInfo, homeAccountId, name, preferredUsername, tenantId, givenName, familyName, wamAccountIds);
	}

	private void Init(string environment, string localAccountId, string rawClientInfo, string homeAccountId, string name, string preferredUsername, string tenantId, string givenName, string familyName, IDictionary<string, string> wamAccountIds)
	{
		base.Environment = environment;
		PreferredUsername = preferredUsername;
		Name = name;
		TenantId = tenantId;
		LocalAccountId = localAccountId;
		base.RawClientInfo = rawClientInfo;
		GivenName = givenName;
		FamilyName = familyName;
		base.HomeAccountId = homeAccountId;
		WamAccountIds = wamAccountIds;
	}

	internal MsalAccountCacheKey GetKey()
	{
		return new MsalAccountCacheKey(base.Environment, TenantId, base.HomeAccountId, PreferredUsername, AuthorityType);
	}

	internal static MsalAccountCacheItem FromJsonString(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
		{
			return null;
		}
		return FromJObject(JObject.Parse(json));
	}

	internal static MsalAccountCacheItem FromJObject(JObject j)
	{
		MsalAccountCacheItem msalAccountCacheItem = new MsalAccountCacheItem();
		msalAccountCacheItem.PreferredUsername = JsonUtils.ExtractExistingOrEmptyString(j, "username");
		msalAccountCacheItem.Name = JsonUtils.ExtractExistingOrEmptyString(j, "name");
		msalAccountCacheItem.GivenName = JsonUtils.ExtractExistingOrEmptyString(j, "given_name");
		msalAccountCacheItem.FamilyName = JsonUtils.ExtractExistingOrEmptyString(j, "family_name");
		msalAccountCacheItem.LocalAccountId = JsonUtils.ExtractExistingOrEmptyString(j, "local_account_id");
		msalAccountCacheItem.AuthorityType = JsonUtils.ExtractExistingOrEmptyString(j, "authority_type");
		msalAccountCacheItem.TenantId = JsonUtils.ExtractExistingOrEmptyString(j, "realm");
		msalAccountCacheItem.WamAccountIds = JsonUtils.ExtractInnerJsonAsDictionary(j, "wam_account_ids");
		msalAccountCacheItem.PopulateFieldsFromJObject(j);
		return msalAccountCacheItem;
	}

	internal override JObject ToJObject()
	{
		JObject jObject = base.ToJObject();
		SetItemIfValueNotNull(jObject, "username", PreferredUsername);
		SetItemIfValueNotNull(jObject, "name", Name);
		SetItemIfValueNotNull(jObject, "given_name", GivenName);
		SetItemIfValueNotNull(jObject, "family_name", FamilyName);
		SetItemIfValueNotNull(jObject, "local_account_id", LocalAccountId);
		SetItemIfValueNotNull(jObject, "authority_type", AuthorityType);
		SetItemIfValueNotNull(jObject, "realm", TenantId);
		if (WamAccountIds != null && WamAccountIds.Any())
		{
			jObject["wam_account_ids"] = JObject.FromObject(WamAccountIds);
		}
		return jObject;
	}

	internal string ToJsonString()
	{
		return ToJObject().ToString();
	}
}
