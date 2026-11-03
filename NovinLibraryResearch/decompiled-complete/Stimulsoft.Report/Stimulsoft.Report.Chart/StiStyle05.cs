namespace Stimulsoft.Report.Chart;

public class StiStyle05 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle05();
	}

	public StiStyle05()
	{
		base.Core = new StiStyleCoreXF05();
	}
}
