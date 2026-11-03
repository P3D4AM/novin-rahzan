namespace Stimulsoft.Report.Chart;

public class StiXBottomAxisCoreXF : StiXAxisCoreXF
{
	public override StiXAxisDock Dock => StiXAxisDock.Bottom;

	public StiXBottomAxisCoreXF(IStiAxis axis)
		: base(axis)
	{
	}
}
