namespace Stimulsoft.Report.Chart;

public class StiStyle01 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle01();
	}

	public StiStyle01()
	{
		base.Core = new StiStyleCoreXF01();
	}
}
