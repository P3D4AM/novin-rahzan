using System;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

internal abstract class MsalItemWithAdditionalFields
{
	internal string AdditionalFieldsJson { get; set; } = "{}";


	internal virtual void PopulateFieldsFromJObject(JObject j)
	{
		AdditionalFieldsJson = j.ToString();
	}

	internal virtual JObject ToJObject()
	{
		if (!string.IsNullOrWhiteSpace(AdditionalFieldsJson))
		{
			return JObject.Parse(AdditionalFieldsJson);
		}
		return new JObject();
	}

	internal void SetItemIfValueNotNull(JObject json, string key, JToken value)
	{
		SetValueIfFilterMatches(json, key, value, (string strVal) => !string.IsNullOrEmpty(strVal));
	}

	internal void SetItemIfValueNotNullOrDefault(JObject json, string key, JToken value, string defaultValue)
	{
		SetValueIfFilterMatches(json, key, value, (string strVal) => !string.IsNullOrEmpty(strVal) && !strVal.Equals(defaultValue, StringComparison.OrdinalIgnoreCase));
	}

	private static void SetValueIfFilterMatches(JObject json, string key, JToken value, Func<string, bool> filter)
	{
		bool flag = true;
		object obj = value.ToObject<object>();
		if (obj == null)
		{
			flag = false;
		}
		else if (obj is string arg)
		{
			flag = filter(arg);
		}
		if (flag)
		{
			json[key] = value;
		}
	}
}
