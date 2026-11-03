using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiRoundedRectangleGaugeGeom : StiGaugeGeom
{
	public RectangleF Rect { get; }

	public StiBrush Background { get; }

	public StiBrush BorderBrush { get; }

	public float BorderWidth { get; }

	public int LeftTop { get; }

	public int RightTop { get; }

	public int RightBottom { get; }

	public int LeftBottom { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.RoundedRectangle;

	public StiRoundedRectangleGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, int leftTop, int rightTop, int rightBottom, int leftBottom)
	{
		Rect = rect;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
		LeftTop = leftTop;
		RightTop = rightTop;
		RightBottom = rightBottom;
		LeftBottom = leftBottom;
	}
}
