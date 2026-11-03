using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiPieGaugeGeom : StiGaugeGeom
{
	public RectangleF Rect { get; }

	public StiBrush Background { get; }

	public StiBrush BorderBrush { get; }

	public float BorderWidth { get; }

	public float StartAngle { get; }

	public float SweepAngle { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.Pie;

	public StiPieGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, float startAngle, float sweepAngle)
	{
		Rect = rect;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
		StartAngle = startAngle;
		SweepAngle = sweepAngle;
	}
}
