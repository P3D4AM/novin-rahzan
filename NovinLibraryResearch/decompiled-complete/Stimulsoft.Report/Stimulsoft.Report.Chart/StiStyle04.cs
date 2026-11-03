namespace Stimulsoft.Report.Chart;

public class StiStyle04 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle04();
	}

	public StiStyle04()
	{
		base.Core = new StiStyleCoreXF04();
	}
}
