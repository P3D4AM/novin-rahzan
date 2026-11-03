namespace Stimulsoft.Report.Chart;

public class StiStyle22 : StiChartStyle
{
	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle22();
	}

	public StiStyle22()
	{
		base.Core = new StiStyleCoreXF22();
	}
}
