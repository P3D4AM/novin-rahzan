namespace Stimulsoft.Report.Chart;

public class StiStyle02 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle02();
	}

	public StiStyle02()
	{
		base.Core = new StiStyleCoreXF02();
	}
}
