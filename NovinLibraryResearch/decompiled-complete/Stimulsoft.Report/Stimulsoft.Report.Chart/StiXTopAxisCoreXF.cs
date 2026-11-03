namespace Stimulsoft.Report.Chart;

public class StiXTopAxisCoreXF : StiXAxisCoreXF
{
	public override StiXAxisDock Dock => StiXAxisDock.Top;

	public StiXTopAxisCoreXF(IStiAxis axis)
		: base(axis)
	{
	}
}
