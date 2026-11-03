namespace Stimulsoft.Report.Chart;

public class StiStyle12 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle12();
	}

	public StiStyle12()
	{
		base.Core = new StiStyleCoreXF12();
	}
}
