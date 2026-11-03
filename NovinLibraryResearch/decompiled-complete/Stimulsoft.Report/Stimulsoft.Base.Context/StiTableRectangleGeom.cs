using System.Drawing;

namespace Stimulsoft.Base.Context;

public class StiTableRectangleGeom : StiTableGeom
{
	public Color Color { get; set; }

	public RectangleF Rect { get; set; }

	public override StiTableGeomType Type => StiTableGeomType.Rectangle;

	public StiTableRectangleGeom(Color color, RectangleF rect)
	{
		Color = color;
		Rect = rect;
	}
}
