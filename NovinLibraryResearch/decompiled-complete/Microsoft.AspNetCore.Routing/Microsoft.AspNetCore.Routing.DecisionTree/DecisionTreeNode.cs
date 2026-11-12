using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing.DecisionTree;

internal class DecisionTreeNode<TItem>
{
	public IList<TItem> Matches { get; set; }

	public IList<DecisionCriterion<TItem>> Criteria { get; set; }
}
