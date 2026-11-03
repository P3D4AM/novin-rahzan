namespace Stimulsoft.Report;

public class StiStyleConditionPlacementElement : StiStyleConditionElement
{
	public StiStyleComponentPlacement Placement { get; set; }

	public StiStyleConditionOperation OperationPlacement { get; set; }

	public StiStyleConditionPlacementElement(StiStyleComponentPlacement placement)
		: this(placement, StiStyleConditionOperation.EqualTo)
	{
	}

	public StiStyleConditionPlacementElement(StiStyleComponentPlacement placement, StiStyleConditionOperation operationPlacement)
	{
		Placement = placement;
		OperationPlacement = operationPlacement;
	}
}
