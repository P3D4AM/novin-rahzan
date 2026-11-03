namespace Stimulsoft.Report.Chart;

public class StiStyle06 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle06();
	}

	public StiStyle06()
	{
		base.Core = new StiStyleCoreXF06();
	}
}
