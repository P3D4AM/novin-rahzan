using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Controls.Helpers;

public static class StiZonePainter
{
	public static void DrawItemZone(Graphics g, string text, Font font, bool isHot, bool isHotClose, Color foreColor, Color backColor, Rectangle rect)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (isHot)
		{
			backColor = StiColorUtils.Dark(backColor, 30);
		}
		StiDrawing.FillRectangle(g, backColor, rect);
		TextFormatFlags val = (TextFormatFlags)32804;
		TextRenderer.DrawText((IDeviceContext)(object)g, text, font, rect, foreColor, val);
		DrawCross(g, rect, isHotClose, foreColor);
	}

	public static void DrawCross(Graphics g, Rectangle rect, bool isHot, Color foreColor)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			rect = GetCloseRect(rect);
			int num = (int)(8.0 * StiScale.Factor);
			Rectangle rectangle = new Rectangle(rect.X, rect.Y + (rect.Height - num) / 2 - StiScale.I1, num, num);
			if (isHot)
			{
				foreColor = StiColorUtils.Dark(foreColor, 35);
			}
			g.SmoothingMode = (SmoothingMode)2;
			Pen val = new Pen(foreColor, 2f);
			try
			{
				g.DrawLine(val, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
				g.DrawLine(val, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Y);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	public static Rectangle GetCloseRect(Rectangle rect)
	{
		return new Rectangle(rect.Right - (int)(13.0 * StiScale.Factor), rect.Y, (int)(8.0 * StiScale.Factor), rect.Height);
	}
}
