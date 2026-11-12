using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2010RibbonTabGroupPainter : Office2007RibbonTabGroupPainter
{
	protected override void PaintTabGroupBackground(Graphics g, Office2007RibbonTabGroupColorTable colorTable, Rectangle bounds, Rectangle groupBounds, bool glassEnabled)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		if (colorTable == null)
		{
			return;
		}
		Rectangle rectangle = bounds;
		rectangle.Height -= 2;
		Rectangle r = rectangle;
		r.Width--;
		r.Height--;
		DisplayHelp.FillRectangle(g, r, colorTable.Background);
		rectangle = bounds;
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Rectangle rectangle2 = groupBounds;
		if (!colorTable.Border.IsEmpty)
		{
			rectangle.Height += 18;
			SolidBrush val = new SolidBrush(colorTable.Border.Start);
			try
			{
				g.FillRectangle((Brush)(object)val, rectangle2.X + 1, rectangle2.Y, rectangle2.Width - 2, 4);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			DisplayHelp.FillRectangle(g, new Rectangle(rectangle2.X + 1, rectangle.Y, 1, rectangle.Height), colorTable.Border);
			DisplayHelp.FillRectangle(g, new Rectangle(rectangle2.Right - 1, rectangle.Y, 1, rectangle.Height), colorTable.Border);
			Pen val2 = new Pen(Color.FromArgb(92, Color.White));
			try
			{
				g.DrawLine(val2, rectangle2.X, rectangle.Y, rectangle2.X, rectangle.Height - 2);
				g.DrawLine(val2, rectangle2.Right, rectangle.Y, rectangle2.Right, rectangle.Height - 2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
	}

	protected override ThemeTextFormat GetThemeFormat()
	{
		return (ThemeTextFormat)32801;
	}

	protected override eTextFormat GetTextFormat()
	{
		return eTextFormat.Default;
	}

	protected override Themes.RECT GetThemeTextBounds(Rectangle rect)
	{
		return new Themes.RECT(new Rectangle(0, 6, rect.Width, rect.Height - 6));
	}

	protected override Rectangle GetTextBounds(RibbonTabGroupRendererEventArgs e)
	{
		Rectangle bounds = e.Bounds;
		bounds.Y += 6;
		bounds.Height -= 6;
		return bounds;
	}
}
