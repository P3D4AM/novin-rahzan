using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class Utils
{
	private static TraceContext traceContext;

	private static Dictionary<AdHocPolicyEvaluationMode, string> AdHocEvaluationModeToDescriptionMap;

	private static Dictionary<string, AutomatedPolicyEvaluationMode> DescriptionToEvaluationModeMap;

	private static Dictionary<AutomatedPolicyEvaluationMode, string> EvaluationModeToDescriptionMap;

	static Utils()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "Utils");
		DescriptionToEvaluationModeMap = new Dictionary<string, AutomatedPolicyEvaluationMode>();
		EvaluationModeToDescriptionMap = new Dictionary<AutomatedPolicyEvaluationMode, string>();
		AdHocEvaluationModeToDescriptionMap = new Dictionary<AdHocPolicyEvaluationMode, string>();
		EvaluationModeToDescriptionMap[AutomatedPolicyEvaluationMode.None] = ExceptionTemplatesSR.EvaluationModeNoneDescription;
		EvaluationModeToDescriptionMap[AutomatedPolicyEvaluationMode.Enforce] = ExceptionTemplatesSR.EvaluationModeEnforceDescription;
		EvaluationModeToDescriptionMap[AutomatedPolicyEvaluationMode.CheckOnChanges] = ExceptionTemplatesSR.EvaluationModeCoCDescription;
		EvaluationModeToDescriptionMap[AutomatedPolicyEvaluationMode.CheckOnSchedule] = ExceptionTemplatesSR.EvaluationModeCoSDescription;
		DescriptionToEvaluationModeMap[ExceptionTemplatesSR.EvaluationModeNoneDescription] = AutomatedPolicyEvaluationMode.None;
		DescriptionToEvaluationModeMap[ExceptionTemplatesSR.EvaluationModeEnforceDescription] = AutomatedPolicyEvaluationMode.Enforce;
		DescriptionToEvaluationModeMap[ExceptionTemplatesSR.EvaluationModeCoCDescription] = AutomatedPolicyEvaluationMode.CheckOnChanges;
		DescriptionToEvaluationModeMap[ExceptionTemplatesSR.EvaluationModeCoSDescription] = AutomatedPolicyEvaluationMode.CheckOnSchedule;
		AdHocEvaluationModeToDescriptionMap[AdHocPolicyEvaluationMode.Check] = ExceptionTemplatesSR.CheckMode;
		AdHocEvaluationModeToDescriptionMap[AdHocPolicyEvaluationMode.Configure] = ExceptionTemplatesSR.ConfigureMode;
		AdHocEvaluationModeToDescriptionMap[AdHocPolicyEvaluationMode.CheckSqlScriptAsProxy] = ExceptionTemplatesSR.CheckSqlScriptAsProxyMode;
	}

	public static string GetDescriptionForEvaluationMode(AutomatedPolicyEvaluationMode mode)
	{
		string value = string.Empty;
		EvaluationModeToDescriptionMap.TryGetValue(mode, out value);
		return value;
	}

	public static string GetDescriptionForAdHocEvaluationMode(AdHocPolicyEvaluationMode mode)
	{
		string value = string.Empty;
		AdHocEvaluationModeToDescriptionMap.TryGetValue(mode, out value);
		return value;
	}

	public static AutomatedPolicyEvaluationMode GetEvaluationModeByDescription(string execModeDescription)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetEvaluationModeByDescription", TraceEventType.Information);
		methodTraceContext.TraceParameters(execModeDescription);
		AutomatedPolicyEvaluationMode value = AutomatedPolicyEvaluationMode.None;
		DescriptionToEvaluationModeMap.TryGetValue(execModeDescription, out value);
		methodTraceContext.TraceParameterOut("returnVal", value);
		return value;
	}

	public static bool IsValidHelpLink(string link)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("IsValidHelpLink", TraceEventType.Information);
		methodTraceContext.TraceParameters(link);
		Uri uri = new Uri(link);
		if (!(uri.Scheme == Uri.UriSchemeHttp) && !(uri.Scheme == Uri.UriSchemeHttps) && !(uri.Scheme == Uri.UriSchemeMailto))
		{
			methodTraceContext.TraceParameterOut("returnVal", false);
			return false;
		}
		methodTraceContext.TraceParameterOut("returnVal", true);
		return true;
	}

	internal static Dictionary<string, object> ReplaceSfcProperties(SfcInstance lhs, SfcInstance rhs)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ReplaceSfcProperties");
		methodTraceContext.TraceParameters(lhs, rhs);
		List<SfcMetadataRelation> properties = new SfcMetadataDiscovery(lhs.GetType()).Properties;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (SfcMetadataRelation item in properties)
		{
			foreach (Attribute relationshipAttribute in item.RelationshipAttributes)
			{
				if (!(relationshipAttribute is SfcPropertyAttribute))
				{
					continue;
				}
				SfcPropertyAttribute sfcPropertyAttribute = relationshipAttribute as SfcPropertyAttribute;
				if (!sfcPropertyAttribute.Data && !sfcPropertyAttribute.Computed && !sfcPropertyAttribute.ReadOnlyAfterCreation)
				{
					object value = rhs.Properties[item.PropertyName].Value;
					if (lhs.Properties[item.PropertyName].Value != value)
					{
						dictionary.Add(item.PropertyName, lhs.Properties[item.PropertyName].Value);
						lhs.Properties[item.PropertyName].Value = value;
					}
				}
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", dictionary);
		return dictionary;
	}

	internal static void ReplaceSfcProperties(SfcInstance lhs, Dictionary<string, object> props)
	{
		traceContext.TraceMethodEnter("ReplaceSfcProperties");
		traceContext.TraceParameters(lhs, props);
		foreach (string key in props.Keys)
		{
			lhs.Properties[key].Value = props[key];
		}
		traceContext.TraceMethodExit("ReplaceSfcProperties");
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static SqlStoreConnection GetSqlStoreConnection(ISfcConnection targetConnection, string methodName)
	{
		if (!(targetConnection is SqlStoreConnection result))
		{
			throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.UnsupportedObjectType(targetConnection.GetType().Name, methodName)));
		}
		return result;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static bool ShouldProcessException(Exception e)
	{
		if (e is OutOfMemoryException || e is StackOverflowException)
		{
			return false;
		}
		return true;
	}

	internal static PropertyInfo[] GetPhysicalFacetProperties(Type managementFacet)
	{
		traceContext.DebugAssert(managementFacet != null, "management facet can't be null");
		if (managementFacet == null)
		{
			return null;
		}
		if (managementFacet.IsSubclassOf(typeof(SqlSmoObject)))
		{
			return (from pi in SmoDmfAdapter.GetTypeProperties(managementFacet)
				where EvaluationFactory.IsTypeSupported(pi.PropertyType)
				select pi).ToArray();
		}
		List<PropertyInfo> list = new List<PropertyInfo>();
		PropertyInfo[] properties = managementFacet.GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.GetCustomAttributes(typeof(SfcPropertyAttribute), inherit: false).Length != 0 && propertyInfo.GetCustomAttributes(typeof(DmfIgnorePropertyAttribute), inherit: false).Length == 0)
			{
				list.Add(propertyInfo);
			}
		}
		return list.ToArray();
	}

	internal static bool IsSmoDomain(SfcDomainInfo domainInfo)
	{
		if (domainInfo == null)
		{
			return false;
		}
		return domainInfo.NamespaceQualifier == "SMO";
	}

	internal static bool IsSmoPath(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}
		Urn urn = new Urn(path);
		traceContext.DebugAssert(urn.XPathExpression.Length > 0, "Xpath expression should contain non zero elements");
		return string.Equals(urn.XPathExpression[0].Name, "Server", StringComparison.Ordinal);
	}

	internal static SfcDomainInfo GetDomainFromUrnSkeleton(string skeleton)
	{
		XPathExpression xPathExpression = new Urn(skeleton).XPathExpression;
		traceContext.DebugAssert(xPathExpression != null, "skeleton doesn't describe a valid urn");
		traceContext.DebugAssert(xPathExpression.Length > 0, "xpe length should be non zero");
		string name = xPathExpression[0].Name;
		SfcDomainInfo sfcDomainInfo = SfcRegistration.Domains[name];
		traceContext.DebugAssert(sfcDomainInfo != null);
		return sfcDomainInfo;
	}

	internal static Type GetTypeFromUrnSkeleton(string skeleton)
	{
		if (string.IsNullOrEmpty(skeleton))
		{
			return null;
		}
		SfcDomainInfo domainFromUrnSkeleton = GetDomainFromUrnSkeleton(skeleton);
		if (domainFromUrnSkeleton != null)
		{
			if (domainFromUrnSkeleton.NamespaceQualifier == "SMO")
			{
				return SqlSmoObject.GetTypeFromUrnSkeleton(skeleton);
			}
			if (domainFromUrnSkeleton.Name == skeleton)
			{
				return domainFromUrnSkeleton.RootType;
			}
			string type = new Urn(skeleton).Type;
			return ((ISfcDomain)Activator.CreateInstance(domainFromUrnSkeleton.RootType)).GetType(type);
		}
		return null;
	}

	internal static SfcObjectQuery GetQueryFromDomainInfo(SfcDomainInfo domainInfo, ISfcConnection connection)
	{
		traceContext.DebugAssert(domainInfo != null, "DomainInfo can't be null");
		SqlStoreConnection sqlStoreConnection = GetSqlStoreConnection(connection, "Utils.GetQueryFromDomainInfo");
		if (domainInfo.NamespaceQualifier == "SMO")
		{
			return new SfcObjectQuery(new Microsoft.SqlServer.Management.Smo.Server(sqlStoreConnection.ServerConnection));
		}
		return new SfcObjectQuery(Activator.CreateInstance(domainInfo.RootType, sqlStoreConnection) as ISfcDomain);
	}
}
