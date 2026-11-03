namespace Stimulsoft.Report.Chart;

public class StiStyle17 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle17();
	}

	public StiStyle17()
	{
		base.Core = new StiStyleCoreXF17();
	}
}
