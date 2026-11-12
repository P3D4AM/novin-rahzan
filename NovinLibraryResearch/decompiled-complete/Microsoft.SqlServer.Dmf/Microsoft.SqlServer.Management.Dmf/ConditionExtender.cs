using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ConditionExtender : SfcObjectExtender<Condition>
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ConditionExtender");

	private FacetInfoCollection facetsCollection;

	private ReadOnlyCollection<FacetInfo> facets;

	private ReadOnlyCollection<FacetInfo> rootFacets;

	private ReadOnlyCollection<Policy> dependentPolicies;

	[ExtendedProperty("Expression")]
	public ExpressionNode ExpressionNode
	{
		get
		{
			return base.Parent.ExpressionNode;
		}
		set
		{
			traceContext.TraceVerbose("Setting ExpressionNode to: {0}", value);
			if (base.Parent.ExpressionNode != value)
			{
				base.Parent.ExpressionNode = value;
			}
		}
	}

	[ExtendedProperty("Facet")]
	public FacetInfo FacetInfo
	{
		get
		{
			string facet = base.Parent.Facet;
			if (string.IsNullOrEmpty(facet))
			{
				return null;
			}
			if (!FacetsCollection.Contains(facet))
			{
				facetsCollection.Add(new FacetInfo(facet));
			}
			return FacetsCollection[facet];
		}
		set
		{
			traceContext.TraceVerbose("Setting FacetInfo to: {0}", value);
			string text = value?.Name;
			if (base.Parent.Facet != text)
			{
				base.Parent.Facet = text;
				base.Parent.ExpressionNode = null;
			}
		}
	}

	[ExtendedProperty]
	public ReadOnlyCollection<FacetInfo> Facets
	{
		get
		{
			if (facets == null)
			{
				facets = new ReadOnlyCollection<FacetInfo>(FacetsCollection);
			}
			return facets;
		}
	}

	[ExtendedProperty]
	public ReadOnlyCollection<Policy> DependentPolicies
	{
		get
		{
			if (dependentPolicies == null)
			{
				List<Policy> list = new List<Policy>();
				string name = base.Parent.Name;
				if (!string.IsNullOrEmpty(name))
				{
					foreach (Policy policy in base.Parent.Parent.Policies)
					{
						if (policy.DependsOnCondition(name))
						{
							list.Add(policy);
						}
					}
				}
				dependentPolicies = new ReadOnlyCollection<Policy>(list);
			}
			return dependentPolicies;
		}
	}

	private FacetInfoCollection FacetsCollection
	{
		get
		{
			if (facetsCollection == null)
			{
				if (base.Parent.IsSystemObject)
				{
					facetsCollection = PolicyStore.Facets;
				}
				else
				{
					facetsCollection = PolicyStore.EnumDomainFacets("SMO", null);
				}
			}
			return facetsCollection;
		}
	}

	[ExtendedProperty]
	public ReadOnlyCollection<FacetInfo> RootFacets
	{
		get
		{
			if (rootFacets == null)
			{
				FacetInfoCollection list = PolicyStore.EnumRootFacets(typeof(Microsoft.SqlServer.Management.Smo.Server));
				rootFacets = new ReadOnlyCollection<FacetInfo>(list);
			}
			return rootFacets;
		}
	}

	public ConditionExtender()
	{
	}

	public ConditionExtender(Condition condition)
		: base(condition)
	{
	}

	public ConditionExtender(PolicyStore policyStore, string name)
		: base(new Condition(policyStore, name))
	{
	}
}
