namespace Stimulsoft.Report.Chart;

public class StiStyle21 : StiChartStyle
{
	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle21();
	}

	public StiStyle21()
	{
		base.Core = new StiStyleCoreXF21();
	}
}
