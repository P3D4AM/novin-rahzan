using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class DesignTime
{
	public static void DrawDesignTimeSelection(Graphics g, Rectangle r, Color c)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		bool flag = false;
		if ((int)g.SmoothingMode == 4)
		{
			flag = true;
			g.SmoothingMode = (SmoothingMode)0;
		}
		g.DrawRectangle(Pens.White, r);
		Pen val = new Pen(c, 1f);
		try
		{
			val.DashStyle = (DashStyle)2;
			g.DrawRectangle(val, r);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (flag)
		{
			g.SmoothingMode = (SmoothingMode)4;
		}
	}
}
