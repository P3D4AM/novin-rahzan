using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiRadialRangeGaugeGeom : StiGaugeGeom
{
	public RectangleF Rect { get; }

	public StiBrush Background { get; }

	public StiBrush BorderBrush { get; }

	public float BorderWidth { get; }

	public PointF CenterPoint { get; }

	public float StartAngle { get; }

	public float SweepAngle { get; }

	public float Radius1 { get; }

	public float Radius2 { get; }

	public float Radius3 { get; }

	public float Radius4 { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.RadialRange;

	public StiRadialRangeGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, PointF centerPoint, float startAngle, float sweepAngle, float radius1, float radius2, float radius3, float radius4)
	{
		Rect = rect;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
		CenterPoint = centerPoint;
		StartAngle = startAngle;
		SweepAngle = sweepAngle;
		Radius1 = radius1;
		Radius2 = radius2;
		Radius3 = radius3;
		Radius4 = radius4;
	}
}
