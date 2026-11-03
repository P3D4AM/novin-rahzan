namespace Stimulsoft.Report.Chart;

public class StiStyle16 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle16();
	}

	public StiStyle16()
	{
		base.Core = new StiStyleCoreXF16();
	}
}
