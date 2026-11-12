using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyCategoryInformation
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "PolicyCategoryInformation");

	private Policy policy;

	private bool targetSubscribes;

	private PolicyCategory policyCategory;

	public int ID
	{
		get
		{
			if (policyCategory != null)
			{
				traceContext.DebugAssert(policyCategory.ID != 0);
			}
			if (policyCategory != null)
			{
				return policyCategory.ID;
			}
			return -1;
		}
	}

	public string Name
	{
		get
		{
			if (policyCategory != null)
			{
				traceContext.DebugAssert(policyCategory.ID != 0);
			}
			if (policyCategory != null)
			{
				return policyCategory.Name;
			}
			return string.Empty;
		}
	}

	public bool IsSubscribed => targetSubscribes;

	public bool MandateDatabaseSubscriptions
	{
		get
		{
			if (policyCategory != null)
			{
				return policyCategory.MandateDatabaseSubscriptions;
			}
			return true;
		}
	}

	public int PolicyId
	{
		get
		{
			if (policy == null)
			{
				return 0;
			}
			return policy.ID;
		}
	}

	public bool IsEmptyCategory => policy == null;

	public string PolicyName
	{
		get
		{
			if (policy == null)
			{
				return null;
			}
			return policy.Name;
		}
	}

	public bool PolicyEnabled
	{
		get
		{
			if (policy == null)
			{
				return false;
			}
			return policy.Enabled;
		}
	}

	public AutomatedPolicyEvaluationMode EvaluationMode
	{
		get
		{
			if (policy == null)
			{
				return AutomatedPolicyEvaluationMode.None;
			}
			return policy.AutomatedPolicyEvaluationMode;
		}
	}

	internal PolicyCategoryInformation(PolicyCategory policyCategory)
	{
		traceContext.TraceMethodEnter("PolicyCategoryInformation");
		traceContext.TraceParameters(policyCategory);
		policy = null;
		targetSubscribes = false;
		this.policyCategory = policyCategory;
		traceContext.TraceMethodExit("PolicyCategoryInformation");
	}

	internal PolicyCategoryInformation(PolicyCategory policyCategory, Policy policy, bool targetSubscribes)
	{
		traceContext.TraceMethodEnter("PolicyCategoryInformation");
		traceContext.TraceParameters(policyCategory, policy, targetSubscribes);
		this.policy = policy;
		this.targetSubscribes = targetSubscribes;
		this.policyCategory = policyCategory;
		traceContext.TraceMethodExit("PolicyCategoryInformation");
	}

	internal static int CompareByCategoryIDPolicyName(PolicyCategoryInformation left, PolicyCategoryInformation right)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CompareByCategoryIDPolicyName");
		methodTraceContext.TraceParameters(left, right);
		if (left.ID == right.ID)
		{
			if (left.policy != null && right.policy != null)
			{
				return left.PolicyName.CompareTo(right.PolicyName);
			}
			methodTraceContext.TraceParameterOut("returnVal", 0);
			return 0;
		}
		return left.ID.CompareTo(right.ID);
	}
}
