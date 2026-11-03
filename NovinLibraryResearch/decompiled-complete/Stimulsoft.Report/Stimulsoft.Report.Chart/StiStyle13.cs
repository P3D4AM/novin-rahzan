namespace Stimulsoft.Report.Chart;

public class StiStyle13 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle13();
	}

	public StiStyle13()
	{
		base.Core = new StiStyleCoreXF13();
	}
}
