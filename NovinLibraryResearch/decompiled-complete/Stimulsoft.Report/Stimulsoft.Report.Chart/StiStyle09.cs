namespace Stimulsoft.Report.Chart;

public class StiStyle09 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle09();
	}

	public StiStyle09()
	{
		base.Core = new StiStyleCoreXF09();
	}
}
