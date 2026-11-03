namespace Stimulsoft.Report.Chart;

public class StiStyle07 : StiChartStyle
{
	public override StiChartStyle CreateNew()
	{
		return new StiStyle07();
	}

	public StiStyle07()
	{
		base.Core = new StiStyleCoreXF07();
	}
}
