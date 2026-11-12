using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

internal class MsalCredentialCacheItemBase : MsalCacheItemBase
{
	internal string CredentialType { get; set; }

	public string ClientId { get; set; }

	public string Secret { get; set; }

	internal override void PopulateFieldsFromJObject(JObject j)
	{
		CredentialType = JsonUtils.ExtractExistingOrEmptyString(j, "credential_type");
		ClientId = JsonUtils.ExtractExistingOrEmptyString(j, "client_id");
		Secret = JsonUtils.ExtractExistingOrEmptyString(j, "secret");
		base.PopulateFieldsFromJObject(j);
	}

	internal override JObject ToJObject()
	{
		JObject jObject = base.ToJObject();
		SetItemIfValueNotNull(jObject, "client_id", ClientId);
		SetItemIfValueNotNull(jObject, "secret", Secret);
		SetItemIfValueNotNull(jObject, "credential_type", CredentialType);
		return jObject;
	}
}
