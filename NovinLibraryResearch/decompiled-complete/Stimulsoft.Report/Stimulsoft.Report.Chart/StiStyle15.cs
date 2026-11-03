namespace Stimulsoft.Report.Chart;

public class StiStyle15 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle15();
	}

	public StiStyle15()
	{
		base.Core = new StiStyleCoreXF15();
	}
}
