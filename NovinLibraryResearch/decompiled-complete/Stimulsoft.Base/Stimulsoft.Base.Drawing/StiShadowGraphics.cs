using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Stimulsoft.Base.Drawing;

public class StiShadowGraphics : IDisposable
{
	private Bitmap shadowBitmap;

	private float factor = 3f;

	public Graphics Graphics { get; }

	public void Dispose()
	{
		Graphics.Dispose();
		((Image)shadowBitmap).Dispose();
	}

	public void DrawShadow(Graphics g, Rectangle rect, int shadowSize)
	{
		g.CompositingQuality = (CompositingQuality)2;
		g.SmoothingMode = (SmoothingMode)4;
		g.InterpolationMode = (InterpolationMode)6;
		g.DrawImage((Image)(object)shadowBitmap, (float)rect.X - 5f * factor + (float)shadowSize, (float)rect.Y - 5f * factor + (float)shadowSize, (float)rect.Width + 10f * factor, (float)rect.Height + 10f * factor);
	}

	public void DrawShadow(Graphics g, RectangleF rect, float shadowSize)
	{
		g.CompositingQuality = (CompositingQuality)2;
		g.SmoothingMode = (SmoothingMode)4;
		g.InterpolationMode = (InterpolationMode)6;
		g.DrawImage((Image)(object)shadowBitmap, rect.X - 5f * factor + shadowSize, rect.Y - 5f * factor + shadowSize, rect.Width + 10f * factor, rect.Height + 10f * factor);
	}

	public StiShadowGraphics(RectangleF rect)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		shadowBitmap = new Bitmap((int)Math.Round(rect.Width / factor) + 10, (int)Math.Round(rect.Height / factor) + 10, (PixelFormat)2498570);
		Graphics = Graphics.FromImage((Image)(object)shadowBitmap);
		Graphics.ScaleTransform(1f / factor, 1f / factor);
		Graphics.TranslateTransform(5f * factor, 5f * factor);
		Graphics.SmoothingMode = (SmoothingMode)4;
		Graphics.CompositingQuality = (CompositingQuality)2;
		Graphics.InterpolationMode = (InterpolationMode)6;
	}
}
