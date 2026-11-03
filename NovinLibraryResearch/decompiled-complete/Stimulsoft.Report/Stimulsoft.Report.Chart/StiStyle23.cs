namespace Stimulsoft.Report.Chart;

public class StiStyle23 : StiChartStyle
{
	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle23();
	}

	public StiStyle23()
	{
		base.Core = new StiStyleCoreXF23();
	}
}
