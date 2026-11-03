namespace Stimulsoft.Report.Chart;

public class StiStyle10 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle10();
	}

	public StiStyle10()
	{
		base.Core = new StiStyleCoreXF10();
	}
}
