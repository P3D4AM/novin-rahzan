namespace Stimulsoft.Report.Chart;

public class StiStyle11 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle11();
	}

	public StiStyle11()
	{
		base.Core = new StiStyleCoreXF11();
	}
}
