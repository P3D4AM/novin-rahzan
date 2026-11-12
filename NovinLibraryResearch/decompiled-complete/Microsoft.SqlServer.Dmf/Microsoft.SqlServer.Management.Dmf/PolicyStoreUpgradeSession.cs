using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyStoreUpgradeSession : UpgradeSession
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "PolicyStoreUpgradeSession");

	private const int CTP6VersionNumber = 2;

	private const int CTP6RefreshVersionNumber = 3;

	public override void PostProcessUpgrade(Dictionary<string, object> sfcCache, int fileVersion)
	{
		if (fileVersion < 2)
		{
			CTP5ToCTP6PostProcessUpgrade(sfcCache);
		}
		if (fileVersion < 3)
		{
			CTP6ToCTP6RefreshPostProcessUpgrade(sfcCache);
		}
	}

	private void CTP6ToCTP6RefreshPostProcessUpgrade(Dictionary<string, object> sfcCache)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CTP6ToCTP6RefreshPostProcessUpgrade");
		methodTraceContext.TraceParameters(sfcCache);
		if (sfcCache == null)
		{
			return;
		}
		Policy policy = null;
		foreach (string key in sfcCache.Keys)
		{
			if (sfcCache[key] is Policy)
			{
				policy = (Policy)sfcCache[key];
				break;
			}
		}
		if (policy == null || policy.AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.None)
		{
			return;
		}
		Condition condition = null;
		foreach (string key2 in sfcCache.Keys)
		{
			if (!(sfcCache[key2] is Condition))
			{
				continue;
			}
			condition = (Condition)sfcCache[key2];
			if (policy.Condition == condition.Name)
			{
				if ((condition.GetSupportedEvaluationMode() & policy.AutomatedPolicyEvaluationMode) == 0)
				{
					policy.AutomatedPolicyEvaluationMode = AutomatedPolicyEvaluationMode.None;
					policy.Enabled = false;
				}
				break;
			}
		}
	}

	private void CTP5ToCTP6PostProcessUpgrade(Dictionary<string, object> sfcCache)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CTP5ToCTP6PostProcessUpgrade");
		methodTraceContext.TraceParameters(sfcCache);
		if (sfcCache == null)
		{
			return;
		}
		Policy policy = null;
		string key = string.Empty;
		ObjectSet objectSet = null;
		Condition condition = null;
		foreach (string key7 in sfcCache.Keys)
		{
			if (sfcCache[key7] is Policy)
			{
				policy = (Policy)sfcCache[key7];
				key = key7;
			}
			if (sfcCache[key7] is ObjectSet)
			{
				objectSet = (ObjectSet)sfcCache[key7];
			}
		}
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		Hashtable hashtable3 = new Hashtable();
		if (policy == null || objectSet != null)
		{
			return;
		}
		string key2 = string.Empty;
		string[] array = new string[sfcCache.Keys.Count];
		sfcCache.Keys.CopyTo(array, 0);
		object obj = null;
		string[] array2 = array;
		foreach (string text in array2)
		{
			obj = sfcCache[text];
			int num = (num = text.IndexOf("/TargetSet/", StringComparison.Ordinal));
			if (text.StartsWith("/PolicyStore/Policy", StringComparison.Ordinal) && num > 0)
			{
				if (objectSet == null)
				{
					objectSet = new ObjectSet(policy.Parent, policy.Name + "ObjectSet");
					key2 = string.Format(CultureInfo.InvariantCulture, "/PolicyStore/ObjectSet/{0}", objectSet.Name);
					sfcCache.Add(key2, objectSet);
					policy.ObjectSet = objectSet.Name;
				}
				string arg = text.Substring(num);
				string key3 = string.Format(CultureInfo.InvariantCulture, "/PolicyStore/ObjectSet/{0}{1}", objectSet.Name, arg);
				sfcCache.Remove(text);
				if (obj is TargetSet)
				{
					((TargetSet)obj).Enabled = true;
					hashtable.Add(key3, (TargetSet)obj);
				}
				if (obj is TargetSetLevel)
				{
					hashtable2.Add(key3, (TargetSetLevel)obj);
				}
			}
			else
			{
				if (!(obj is Policy))
				{
					hashtable3.Add(text, obj);
				}
				if (obj is Condition && ((Condition)obj).Name == policy.Condition)
				{
					condition = (Condition)obj;
				}
			}
		}
		sfcCache.Clear();
		sfcCache.Add(key, policy);
		if (objectSet != null)
		{
			objectSet.Properties["Facet"].Value = condition.Facet;
			sfcCache.Add(key2, objectSet);
		}
		foreach (string key8 in hashtable.Keys)
		{
			sfcCache.Add(key8, hashtable[key8]);
		}
		foreach (string key9 in hashtable2.Keys)
		{
			sfcCache.Add(key9, hashtable2[key9]);
		}
		foreach (string key10 in hashtable3.Keys)
		{
			sfcCache.Add(key10, hashtable3[key10]);
		}
	}

	public override bool IsUpgradeRequiredOnType(string instanceType, int fileVersion)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("IsUpgradeRequiredOnType", TraceEventType.Information);
		methodTraceContext.TraceParameters(instanceType, fileVersion);
		if (fileVersion < ((ISfcDomainLite)new PolicyStore()).GetLogicalVersion())
		{
			if (instanceType == "Policy" || instanceType == "Condition")
			{
				methodTraceContext.TraceParameterOut("returnVal", true);
				return true;
			}
			methodTraceContext.TraceParameterOut("returnVal", false);
			return false;
		}
		methodTraceContext.TraceParameterOut("returnVal", false);
		return false;
	}

	public override List<KeyValuePair<string, object>> UpgradeInstance(List<SfcInstanceSerializedData> sfcInstanceData, int fileVersion, string smlUri, Dictionary<string, object> sfcCache)
	{
		List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
		if (smlUri.StartsWith("/PolicyStore/Policy/", StringComparison.Ordinal))
		{
			object value = UpgradeInstance(typeof(Policy), Policy.UpgradeInstance(sfcInstanceData, fileVersion));
			list.Add(new KeyValuePair<string, object>(smlUri, value));
		}
		else if (smlUri.StartsWith("/PolicyStore/Condition/", StringComparison.Ordinal))
		{
			object value2 = UpgradeInstance(typeof(Condition), Condition.UpgradeInstance(sfcInstanceData, fileVersion));
			list.Add(new KeyValuePair<string, object>(smlUri, value2));
		}
		else
		{
			traceContext.DebugAssert(test: true, "We are not supposed to get other than for Policy or Condition");
		}
		return list;
	}
}
