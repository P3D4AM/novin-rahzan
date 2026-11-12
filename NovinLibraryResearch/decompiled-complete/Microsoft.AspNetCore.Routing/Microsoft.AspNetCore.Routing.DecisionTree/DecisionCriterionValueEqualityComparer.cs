using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing.DecisionTree;

internal class DecisionCriterionValueEqualityComparer : IEqualityComparer<DecisionCriterionValue>
{
	public IEqualityComparer<object> InnerComparer { get; private set; }

	public DecisionCriterionValueEqualityComparer(IEqualityComparer<object> innerComparer)
	{
		InnerComparer = innerComparer;
	}

	public bool Equals(DecisionCriterionValue x, DecisionCriterionValue y)
	{
		return InnerComparer.Equals(x.Value, y.Value);
	}

	public int GetHashCode(DecisionCriterionValue obj)
	{
		return InnerComparer.GetHashCode(obj.Value);
	}
}
