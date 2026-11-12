using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;

namespace Stimulsoft.Dashboard.Viewer.Controls.Helpers;

public static class StiButtonPainter
{
	public static void DrawButton(Graphics g, Rectangle rect, bool isHot, Color normalColor, Color hotColor)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		int num = rect.X + rect.Width / 2;
		int num2 = rect.Y + rect.Height / 2;
		GraphicsPath val = new GraphicsPath();
		try
		{
			int i = StiScale.I1;
			int i2 = StiScale.I2;
			int i3 = StiScale.I3;
			val.AddLine(num - i3, num2 - i2, num - i3, num2 - i);
			val.AddLine(num - i3, num2 - i, num, num2 + i2);
			val.AddLine(num, num2 + i2, num + i3, num2 - i);
			val.AddLine(num + i3, num2 - i, num + i3, num2 - i2);
			Pen val2 = new Pen(isHot ? hotColor : normalColor, 2f);
			try
			{
				val2.StartCap = (LineCap)0;
				val2.EndCap = (LineCap)0;
				g.SmoothingMode = (SmoothingMode)4;
				g.SetClip(new Rectangle(num - i3, num2 - i2, StiScale.I7, StiScale.I6));
				g.DrawPath(val2, val);
				g.ResetClip();
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
