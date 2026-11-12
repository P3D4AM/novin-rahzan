using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing.DecisionTree;

internal interface IClassifier<TItem>
{
	IEqualityComparer<object> ValueComparer { get; }

	IDictionary<string, DecisionCriterionValue> GetCriteria(TItem item);
}
