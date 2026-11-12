using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SimpleNodeDisplay
{
	public static void Paint(SimpleNodeDisplayInfo di)
	{
		Rectangle bounds = di.Element.Bounds;
		Region clip = di.Graphics.Clip;
		if (clip != null)
		{
			di.Graphics.SetClip(bounds, (CombineMode)1);
		}
		else
		{
			di.Graphics.SetClip(bounds, (CombineMode)0);
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(di.Style, di.Graphics, bounds);
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		di.Graphics.ResetClip();
		if (di.Element.ImageVisible)
		{
			PaintImage(di);
		}
		if (di.Element.TextVisible)
		{
			elementStyleDisplayInfo.Bounds = (di.TextBounds.IsEmpty ? di.Element.TextBounds : di.TextBounds);
			eTextFormat eTextFormat2 = di.Style.TextFormat;
			if (di.RightToLeft)
			{
				eTextFormat2 |= eTextFormat.RightToLeft;
			}
			ElementStyleDisplay.PaintText(elementStyleDisplayInfo, di.Element.Text, di.Font, useDefaultFont: false, eTextFormat2);
		}
		if (clip != null)
		{
			di.Graphics.Clip = clip;
		}
		else
		{
			di.Graphics.ResetClip();
		}
	}

	private static void PaintImage(SimpleNodeDisplayInfo di)
	{
		if (!di.Element.ImageLayoutSize.IsEmpty && di.Element.Image != null)
		{
			Rectangle imageBounds = di.Element.ImageBounds;
			di.Graphics.DrawImage(di.Element.Image, imageBounds.X + (imageBounds.Width - di.Element.Image.Width) / 2, imageBounds.Y + (imageBounds.Height - di.Element.Image.Height) / 2);
		}
	}
}
