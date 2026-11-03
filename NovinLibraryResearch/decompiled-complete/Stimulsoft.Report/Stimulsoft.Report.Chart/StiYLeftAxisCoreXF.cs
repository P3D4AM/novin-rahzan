namespace Stimulsoft.Report.Chart;

public class StiYLeftAxisCoreXF : StiYAxisCoreXF
{
	public override StiYAxisDock Dock => StiYAxisDock.Left;

	public StiYLeftAxisCoreXF(IStiAxis axis)
		: base(axis)
	{
	}
}
