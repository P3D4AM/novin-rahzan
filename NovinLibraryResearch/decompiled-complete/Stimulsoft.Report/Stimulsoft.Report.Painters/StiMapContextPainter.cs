using System.Drawing;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Painters;

public abstract class StiMapContextPainter
{
	public float Zoom { get; set; }

	public RectangleF Rect { get; set; }

	public StiMap Map { get; set; }

	public static Font ChangeFontSize(Font font, float zoom)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		float num = font.Size * zoom;
		if (num < 1f)
		{
			num = 1f;
		}
		return new Font(font.FontFamily.Name, num, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public abstract SizeF MeasureString(string text, Font font);

	public abstract void Render();

	public StiMapContextPainter(StiMap map, RectangleF rect, float zoom)
	{
		Map = map;
		Rect = rect;
		Zoom = zoom;
	}
}
