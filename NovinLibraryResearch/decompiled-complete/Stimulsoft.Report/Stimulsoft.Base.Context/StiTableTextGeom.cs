using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Context;

public class StiTableTextGeom : StiTableGeom
{
	public string Text { get; set; }

	public StiFontGeom Font { get; set; }

	public StiBrush Foreground { get; set; }

	public RectangleF Rect { get; set; }

	public StringAlignment Alignment { get; set; }

	public override StiTableGeomType Type => StiTableGeomType.Text;

	public StiTableTextGeom(string text, StiFontGeom font, StiBrush foreground, RectangleF rect, StringAlignment alignment)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		Font = font;
		Foreground = foreground;
		Rect = rect;
		Alignment = alignment;
	}
}
