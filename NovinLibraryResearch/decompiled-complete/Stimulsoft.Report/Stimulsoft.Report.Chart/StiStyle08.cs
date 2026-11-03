namespace Stimulsoft.Report.Chart;

public class StiStyle08 : StiStyle03
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle08();
	}

	public StiStyle08()
	{
		base.Core = new StiStyleCoreXF08();
	}
}
