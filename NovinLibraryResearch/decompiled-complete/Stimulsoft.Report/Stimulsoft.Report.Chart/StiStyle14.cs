namespace Stimulsoft.Report.Chart;

public class StiStyle14 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle14();
	}

	public StiStyle14()
	{
		base.Core = new StiStyleCoreXF14();
	}
}
