using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Indicator;

namespace Stimulsoft.Base.Gauge.GaugeGeoms;

public class StiRectangleIndicatorGeom : StiIndicatorGeom
{
	public RectangleF Rect { get; }

	public StiBrush Background { get; }

	public StiBrush BorderBrush { get; }

	public float BorderWidth { get; }

	public override StiIndicatorGeomType Type => StiIndicatorGeomType.Rectangle;

	public StiRectangleIndicatorGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth)
	{
		Rect = rect;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
	}
}
