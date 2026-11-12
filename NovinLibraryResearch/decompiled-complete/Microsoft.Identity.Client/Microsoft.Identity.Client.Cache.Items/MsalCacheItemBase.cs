using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

internal abstract class MsalCacheItemBase : MsalItemWithAdditionalFields
{
	internal string HomeAccountId { get; set; }

	internal string Environment { get; set; }

	internal string RawClientInfo { get; set; }

	internal override void PopulateFieldsFromJObject(JObject j)
	{
		HomeAccountId = JsonUtils.ExtractExistingOrEmptyString(j, "home_account_id");
		Environment = JsonUtils.ExtractExistingOrEmptyString(j, "environment");
		RawClientInfo = JsonUtils.ExtractExistingOrEmptyString(j, "client_info");
		base.PopulateFieldsFromJObject(j);
	}

	internal override JObject ToJObject()
	{
		JObject jObject = base.ToJObject();
		SetItemIfValueNotNull(jObject, "home_account_id", HomeAccountId);
		SetItemIfValueNotNull(jObject, "environment", Environment);
		SetItemIfValueNotNull(jObject, "client_info", RawClientInfo);
		return jObject;
	}
}
