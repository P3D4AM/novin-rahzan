namespace Stimulsoft.Report.Chart;

public class StiCenterPie3dLabels : StiPieSeriesLabels
{
	public override StiComponentId ComponentId => StiComponentId.StiCenterPie3dLabels;

	public override StiSeriesLabels CreateNew()
	{
		return new StiCenterPie3dLabels();
	}

	public StiCenterPie3dLabels()
	{
		base.Core = new StiCenterPie3dLabelsCoreXF(this);
	}
}
