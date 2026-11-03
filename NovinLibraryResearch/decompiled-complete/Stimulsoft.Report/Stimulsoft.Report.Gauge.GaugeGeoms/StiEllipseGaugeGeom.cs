using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiEllipseGaugeGeom : StiGaugeGeom
{
	public RectangleF Rect { get; }

	public StiBrush Background { get; }

	public StiBrush BorderBrush { get; }

	public float BorderWidth { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.Ellipse;

	public StiEllipseGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth)
	{
		Rect = rect;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
	}
}
