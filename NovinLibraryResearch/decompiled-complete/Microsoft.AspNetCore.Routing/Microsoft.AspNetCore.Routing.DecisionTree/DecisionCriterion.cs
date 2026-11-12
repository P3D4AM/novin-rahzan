using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing.DecisionTree;

internal class DecisionCriterion<TItem>
{
	public string Key { get; set; }

	public Dictionary<object, DecisionTreeNode<TItem>> Branches { get; set; }
}
