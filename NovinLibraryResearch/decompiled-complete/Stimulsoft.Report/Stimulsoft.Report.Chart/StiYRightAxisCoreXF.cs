namespace Stimulsoft.Report.Chart;

public class StiYRightAxisCoreXF : StiYAxisCoreXF
{
	public override StiYAxisDock Dock => StiYAxisDock.Right;

	public override bool GetStartFromZero()
	{
		if (base.Axis.Area.AxisCore.ValuesCount == 1)
		{
			return true;
		}
		if (base.Axis != null && base.Axis.Range != null && !base.Axis.Range.Auto)
		{
			return false;
		}
		if (base.Axis == null)
		{
			return true;
		}
		return base.Axis.StartFromZero;
	}

	public StiYRightAxisCoreXF(IStiAxis axis)
		: base(axis)
	{
	}
}
