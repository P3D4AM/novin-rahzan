using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroStatusBarPainter : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Invalid comparison between Unknown and I4
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		MetroStatusBar metroStatusBar = (MetroStatusBar)renderingInfo.Control;
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		MetroStatusBarColorTable metroStatusBar2 = renderingInfo.ColorTable.MetroStatusBar;
		Rectangle clientRectangle = ((Control)metroStatusBar).ClientRectangle;
		if (metroStatusBar2.BackgroundStyle != null)
		{
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(metroStatusBar2.BackgroundStyle, graphics, clientRectangle);
			ElementStyleDisplay.PaintBackground(e);
		}
		if (metroStatusBar2.TopBorders != null && metroStatusBar2.TopBorders.Length != 0)
		{
			for (int i = 0; i < metroStatusBar2.TopBorders.Length; i++)
			{
				Pen val = new Pen(metroStatusBar2.TopBorders[i]);
				try
				{
					graphics.DrawLine(val, clientRectangle.X, clientRectangle.Y + i, clientRectangle.Right, clientRectangle.Y + i);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		if (metroStatusBar2.BottomBorders != null && metroStatusBar2.BottomBorders.Length != 0)
		{
			for (int j = 0; j < metroStatusBar2.BottomBorders.Length; j++)
			{
				Pen val2 = new Pen(metroStatusBar2.BottomBorders[j]);
				try
				{
					graphics.DrawLine(val2, clientRectangle.X, clientRectangle.Bottom - j - 1, clientRectangle.Right, clientRectangle.Bottom - j - 1);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		if (metroStatusBar.ResizeHandleVisible)
		{
			Form val3 = ((Control)metroStatusBar).FindForm();
			if (val3 != null && (int)val3.WindowState == 0)
			{
				DrawResizeHandle(graphics, clientRectangle, metroStatusBar2.ResizeMarkerLightColor, metroStatusBar2.ResizeMarkerColor, (int)((Control)metroStatusBar).RightToLeft == 1);
			}
		}
	}

	private void DrawResizeHandle(Graphics g, Rectangle statusBarBounds, Color lightColor, Color color, bool rightToLeft)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		int num = 1;
		Point point = new Point(statusBarBounds.Right, statusBarBounds.Bottom);
		if (rightToLeft)
		{
			num = -1;
			point = new Point(0, statusBarBounds.Bottom - 2);
		}
		Pen val = new Pen(lightColor, 1f);
		try
		{
			g.DrawLine(val, point.X - 2 * num, point.Y - 2, point.X - 3 * num, point.Y - 2);
			g.DrawLine(val, point.X - 6 * num, point.Y - 2, point.X - 7 * num, point.Y - 2);
			g.DrawLine(val, point.X - 10 * num, point.Y - 2, point.X - 11 * num, point.Y - 2);
			g.DrawLine(val, point.X - 2 * num, point.Y - 2, point.X - 2 * num, point.Y - 3);
			g.DrawLine(val, point.X - 6 * num, point.Y - 2, point.X - 6 * num, point.Y - 3);
			g.DrawLine(val, point.X - 10 * num, point.Y - 2, point.X - 10 * num, point.Y - 3);
			g.DrawLine(val, point.X - 2 * num, point.Y - 6, point.X - 3 * num, point.Y - 6);
			g.DrawLine(val, point.X - 6 * num, point.Y - 6, point.X - 7 * num, point.Y - 6);
			g.DrawLine(val, point.X - 2 * num, point.Y - 6, point.X - 2 * num, point.Y - 7);
			g.DrawLine(val, point.X - 6 * num, point.Y - 6, point.X - 6 * num, point.Y - 7);
			g.DrawLine(val, point.X - 2 * num, point.Y - 10, point.X - 3 * num, point.Y - 10);
			g.DrawLine(val, point.X - 2 * num, point.Y - 10, point.X - 2 * num, point.Y - 11);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(color, 1f);
		try
		{
			g.DrawRectangle(val2, point.X - 4 * num, point.Y - 4, 1, 1);
			g.DrawRectangle(val2, point.X - 8 * num, point.Y - 4, 1, 1);
			g.DrawRectangle(val2, point.X - 12 * num, point.Y - 4, 1, 1);
			g.DrawRectangle(val2, point.X - 4 * num, point.Y - 8, 1, 1);
			g.DrawRectangle(val2, point.X - 8 * num, point.Y - 8, 1, 1);
			g.DrawRectangle(val2, point.X - 4 * num, point.Y - 12, 1, 1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
