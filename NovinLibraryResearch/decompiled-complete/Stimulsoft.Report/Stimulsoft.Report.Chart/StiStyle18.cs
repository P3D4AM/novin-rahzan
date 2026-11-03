namespace Stimulsoft.Report.Chart;

public class StiStyle18 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle18();
	}

	public StiStyle18()
	{
		base.Core = new StiStyleCoreXF18();
	}
}
