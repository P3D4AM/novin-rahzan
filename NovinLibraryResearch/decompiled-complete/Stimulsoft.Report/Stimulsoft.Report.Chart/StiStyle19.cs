namespace Stimulsoft.Report.Chart;

public class StiStyle19 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle19();
	}

	public StiStyle19()
	{
		base.Core = new StiStyleCoreXF19();
	}
}
