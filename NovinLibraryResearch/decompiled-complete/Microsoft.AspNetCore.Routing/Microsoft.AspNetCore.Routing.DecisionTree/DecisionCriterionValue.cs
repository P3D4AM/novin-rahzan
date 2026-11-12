namespace Microsoft.AspNetCore.Routing.DecisionTree;

internal struct DecisionCriterionValue
{
	private readonly object _value;

	public object Value => _value;

	public DecisionCriterionValue(object value)
	{
		_value = value;
	}
}
