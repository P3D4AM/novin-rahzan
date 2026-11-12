using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing.DecisionTree;

internal class ItemDescriptor<TItem>
{
	public IDictionary<string, DecisionCriterionValue> Criteria { get; set; }

	public int Index { get; set; }

	public TItem Item { get; set; }
}
