namespace Stimulsoft.Report.Chart;

public class StiStyle20 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle20();
	}

	public StiStyle20()
	{
		base.Core = new StiStyleCoreXF20();
	}
}
