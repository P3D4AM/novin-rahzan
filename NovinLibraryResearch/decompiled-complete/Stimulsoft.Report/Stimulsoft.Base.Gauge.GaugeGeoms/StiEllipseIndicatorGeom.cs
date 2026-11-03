using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Indicator;

namespace Stimulsoft.Base.Gauge.GaugeGeoms;

public class StiEllipseIndicatorGeom : StiIndicatorGeom
{
	public readonly RectangleF Rect;

	public readonly StiBrush Background;

	public readonly StiBrush BorderBrush;

	public readonly float BorderWidth;

	public override StiIndicatorGeomType Type => StiIndicatorGeomType.Ellipse;

	public StiEllipseIndicatorGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth)
	{
		Rect = rect;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
	}
}
