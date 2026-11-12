using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

internal class MsalAppMetadataCacheItem : MsalItemWithAdditionalFields, IEquatable<MsalAppMetadataCacheItem>
{
	public string ClientId { get; }

	public string Environment { get; }

	public string FamilyId { get; }

	public MsalAppMetadataCacheItem(string clientId, string preferredCacheEnv, string familyId)
	{
		ClientId = clientId;
		Environment = preferredCacheEnv;
		FamilyId = familyId;
	}

	public MsalAppMetadataCacheKey GetKey()
	{
		return new MsalAppMetadataCacheKey(ClientId, Environment);
	}

	internal static MsalAppMetadataCacheItem FromJsonString(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
		{
			return null;
		}
		return FromJObject(JObject.Parse(json));
	}

	internal static MsalAppMetadataCacheItem FromJObject(JObject j)
	{
		string clientId = JsonUtils.ExtractExistingOrEmptyString(j, "client_id");
		string preferredCacheEnv = JsonUtils.ExtractExistingOrEmptyString(j, "environment");
		string familyId = JsonUtils.ExtractExistingOrEmptyString(j, "family_id");
		MsalAppMetadataCacheItem msalAppMetadataCacheItem = new MsalAppMetadataCacheItem(clientId, preferredCacheEnv, familyId);
		msalAppMetadataCacheItem.PopulateFieldsFromJObject(j);
		return msalAppMetadataCacheItem;
	}

	internal string ToJsonString()
	{
		return ToJObject().ToString();
	}

	internal override JObject ToJObject()
	{
		JObject jObject = base.ToJObject();
		SetItemIfValueNotNull(jObject, "environment", Environment);
		SetItemIfValueNotNull(jObject, "client_id", ClientId);
		SetItemIfValueNotNull(jObject, "family_id", FamilyId);
		return jObject;
	}

	public override int GetHashCode()
	{
		return (((-1793347351 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClientId)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Environment)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FamilyId)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(base.AdditionalFieldsJson);
	}

	public bool Equals(MsalAppMetadataCacheItem other)
	{
		if (ClientId == other.ClientId && Environment == other.Environment && FamilyId == other.FamilyId)
		{
			return base.AdditionalFieldsJson == other.AdditionalFieldsJson;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is MsalAppMetadataCacheItem other)
		{
			return Equals(other);
		}
		return false;
	}
}
