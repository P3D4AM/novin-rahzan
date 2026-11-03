namespace Stimulsoft.Report.Chart;

public class StiStyle03 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle03();
	}

	public StiStyle03()
	{
		base.Core = new StiStyleCoreXF03();
	}
}
