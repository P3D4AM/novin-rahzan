using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class ActionComparer
{
	public static bool IsEquivalentClass(ActionPropertyBag action1, ActionPropertyBag action2)
	{
		if (action1 == action2 || action1.ReadyForUpload || action2.ReadyForUpload || !action1.IsAggregable || !action2.IsAggregable)
		{
			return false;
		}
		PropertyBagContents contents = action1.GetContents();
		PropertyBagContents contents2 = action2.GetContents();
		foreach (string comparisonStringProperty in GetComparisonStringProperties())
		{
			if (!IsPropertyEquivalent(comparisonStringProperty, contents.StringProperties, contents2.StringProperties))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsPropertyEquivalent(string propertyName, ConcurrentDictionary<string, string> propertyMap1, ConcurrentDictionary<string, string> propertyMap2)
	{
		string value = string.Empty;
		string value2 = string.Empty;
		bool num = propertyMap1.TryGetValue(propertyName, out value);
		bool flag = propertyMap2.TryGetValue(propertyName, out value2);
		if (num == flag)
		{
			return string.Compare(value, value2, StringComparison.OrdinalIgnoreCase) == 0;
		}
		return false;
	}

	private static List<string> GetComparisonStringProperties()
	{
		return new List<string> { "accountid", "actiontype", "Microsoft_MSAL_broker_app", "Microsoft_MSAL_tenant_id", "identity", "identityservice", "Microsoft_MSAL_idp", "tenantid", "resource", "scope" };
	}
}
