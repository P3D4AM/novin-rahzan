using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyExtender : SfcObjectExtender<Policy>
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "PolicyExtender");

	private List<PolicyCategory> policyCategories;

	private PolicyCategory defaultCategory;

	private string policyFilePath = string.Empty;

	[ExtendedProperty("Condition")]
	public Condition ConditionInstance
	{
		get
		{
			if (base.Parent.Parent.Conditions.Count > 0)
			{
				string condition = base.Parent.Condition;
				if (string.IsNullOrEmpty(condition))
				{
					return null;
				}
				return base.Parent.Parent.Conditions[condition];
			}
			return null;
		}
		set
		{
			traceContext.TraceVerbose("Setting ConditionInstance to: {0}", value);
			string text = value?.Name;
			if (base.Parent.Condition != text)
			{
				base.Parent.Condition = text;
				base.Parent.AutomatedPolicyEvaluationMode = AutomatedPolicyEvaluationMode.None;
			}
		}
	}

	[ExtendedProperty("Condition")]
	public AutomatedPolicyEvaluationMode SupportedPolicyEvaluationMode => ConditionInstance?.GetSupportedEvaluationMode() ?? AutomatedPolicyEvaluationMode.None;

	[ExtendedProperty("PolicyCategory")]
	public PolicyCategory Category
	{
		get
		{
			string policyCategory = base.Parent.PolicyCategory;
			if (string.IsNullOrEmpty(policyCategory))
			{
				return DefaultCategory;
			}
			PolicyCategory policyCategory2 = base.Parent.Parent.PolicyCategories[policyCategory];
			if (policyCategory2 == null)
			{
				return DefaultCategory;
			}
			return policyCategory2;
		}
		set
		{
			traceContext.TraceVerbose("Setting Category to: {0}", value);
			string text = ((value == null) ? string.Empty : ((value != DefaultCategory) ? value.Name : string.Empty));
			if (base.Parent.PolicyCategory != text)
			{
				base.Parent.PolicyCategory = text;
			}
		}
	}

	[ExtendedProperty]
	public List<PolicyCategory> Categories
	{
		get
		{
			if (policyCategories == null && base.Parent.Parent != null)
			{
				policyCategories = new List<PolicyCategory>(base.Parent.Parent.PolicyCategories);
				policyCategories.Insert(0, DefaultCategory);
			}
			return policyCategories;
		}
	}

	private PolicyCategory DefaultCategory
	{
		get
		{
			if (defaultCategory == null)
			{
				defaultCategory = new PolicyCategory(base.Parent.Parent, PolicyCategory.DefaultCategory);
			}
			return defaultCategory;
		}
	}

	[ExtendedProperty]
	public TargetSetCollection Filters
	{
		get
		{
			if (base.Parent.Parent.ObjectSets.Count > 0)
			{
				if (string.IsNullOrEmpty(base.Parent.ObjectSet))
				{
					return null;
				}
				return base.Parent.Parent.ObjectSets[base.Parent.ObjectSet].TargetSets;
			}
			return null;
		}
	}

	[ExtendedProperty]
	public string PolicyFilePath
	{
		get
		{
			return policyFilePath;
		}
		set
		{
			traceContext.TraceVerbose("Setting PolicyFilePath to: {0}", value);
			if (value != policyFilePath)
			{
				policyFilePath = value;
			}
		}
	}

	[ExtendedProperty]
	public bool OfflineMode
	{
		get
		{
			bool result = false;
			Policy policy = base.Parent;
			if (policy != null)
			{
				result = ((policy.Parent == null || ((ISfcHasConnection)policy.Parent).ConnectionContext.Mode == SfcConnectionContextMode.Offline) ? true : false);
			}
			return result;
		}
	}

	[ExtendedProperty("Condition")]
	public string RootName
	{
		get
		{
			if (base.Parent.Parent.ObjectSets.Count > 0)
			{
				string objectSet = base.Parent.ObjectSet;
				if (!string.IsNullOrEmpty(objectSet))
				{
					return base.Parent.Parent.ObjectSets[objectSet].RootLevel;
				}
			}
			return "Server";
		}
	}

	[ExtendedProperty("Condition")]
	public bool EnableRootRestriction
	{
		get
		{
			try
			{
				SfcDomainInfo sfcDomainInfo = SfcRegistration.Domains[RootName];
				if (sfcDomainInfo != null)
				{
					foreach (Type rootFacet in FacetRepository.GetRootFacets(sfcDomainInfo.RootType))
					{
						_ = rootFacet;
					}
					return FacetRepository.GetRootFacets(sfcDomainInfo.RootType).Count > 0;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}
	}

	public PolicyExtender()
	{
	}

	public PolicyExtender(Policy policy)
		: base(policy)
	{
	}

	public PolicyExtender(PolicyStore policyStore, string name)
		: base(new Policy(policyStore, name))
	{
	}
}
