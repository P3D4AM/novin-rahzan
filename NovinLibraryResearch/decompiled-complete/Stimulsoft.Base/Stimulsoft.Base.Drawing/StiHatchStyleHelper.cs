using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing;

public sealed class StiHatchStyleHelper
{
	public static int GetWidthOfHatchStyle(Graphics g, Font font, int index)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		HatchStyle val = StiBrushes.HatchStyles[index];
		string text = StiLocalization.Get("PropertyHatchStyle", ((object)(HatchStyle)(ref val)).ToString());
		int num = (int)g.MeasureString(text, font).Width;
		return num + (int)Math.Ceiling(40.0 * StiScale.X);
	}

	public static void DrawHatchStyle(DrawItemEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index < 0)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		if ((e.State & 1) > 0)
		{
			bounds.Width--;
		}
		Rectangle rectangle = new Rectangle(bounds.X + (int)Math.Ceiling(2.0 * StiScale.X), bounds.Y + (int)Math.Ceiling(2.0 * StiScale.Y), (int)Math.Ceiling(19.0 * StiScale.X), bounds.Height - (int)Math.Ceiling(5.0 * StiScale.Y));
		StiControlPaint.DrawItemBackground(graphics, bounds, e.State);
		HatchBrush val = new HatchBrush(StiBrushes.HatchStyles[e.Index], Color.Black, Color.White);
		try
		{
			graphics.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		graphics.DrawRectangle(Pens.Black, rectangle);
		Font val2 = new Font("Arial", 8f);
		try
		{
			StringFormat val3 = new StringFormat();
			try
			{
				val3.LineAlignment = (StringAlignment)1;
				val3.FormatFlags = (StringFormatFlags)4096;
				val3.Trimming = (StringTrimming)3;
				HatchStyle val4 = StiBrushes.HatchStyles[e.Index];
				string text = StiLocalization.Get("PropertyHatchStyle", ((object)(HatchStyle)(ref val4)).ToString());
				e.Graphics.DrawString(text, val2, Brushes.Black, (RectangleF)new Rectangle((int)Math.Ceiling(25.0 * StiScale.X), bounds.Top, bounds.Width - (int)Math.Ceiling(18.0 * StiScale.X), (int)Math.Ceiling(16.0 * StiScale.Y)), val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
