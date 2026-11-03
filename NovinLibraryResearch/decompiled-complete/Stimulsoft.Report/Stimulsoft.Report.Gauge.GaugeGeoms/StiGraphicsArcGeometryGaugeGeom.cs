using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiGraphicsArcGeometryGaugeGeom : StiGaugeGeom
{
	public RectangleF Rect { get; }

	public StiBrush Background { get; }

	public StiBrush BorderBrush { get; }

	public float BorderWidth { get; }

	public float StartAngle { get; }

	public float SweepAngle { get; }

	public float StartWidth { get; }

	public float EndWidth { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.GraphicsArcGeometry;

	public StiGraphicsArcGeometryGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, float startAngle, float sweepAngle, float startWidth, float endWidth)
	{
		Rect = rect;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
		StartAngle = startAngle;
		SweepAngle = sweepAngle;
		StartWidth = startWidth;
		EndWidth = endWidth;
	}
}
