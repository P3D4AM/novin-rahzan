namespace Stimulsoft.Report;

public class StiStyleConditionPlacementNestedLevelElement : StiStyleConditionElement
{
	public int PlacementNestedLevel { get; set; }

	public StiStyleConditionOperation OperationPlacementNestedLevel { get; set; }

	public StiStyleConditionPlacementNestedLevelElement(int placementNestedLevel)
		: this(placementNestedLevel, StiStyleConditionOperation.EqualTo)
	{
	}

	public StiStyleConditionPlacementNestedLevelElement(int placementNestedLevel, StiStyleConditionOperation operationPlacementNestedLevel)
	{
		PlacementNestedLevel = placementNestedLevel;
		OperationPlacementNestedLevel = operationPlacementNestedLevel;
	}
}
