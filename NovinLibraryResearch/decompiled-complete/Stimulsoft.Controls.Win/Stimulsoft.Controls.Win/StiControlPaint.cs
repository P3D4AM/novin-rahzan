using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win;

public static class StiControlPaint
{
	public static void DrawBorder(Graphics graphics, Rectangle bounds, bool isFocused, bool isEnabled, bool flat)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		if (flat)
		{
			Color color = (isEnabled ? StiColorUtils.Dark(SystemColors.ControlDark, 10) : StiColorUtils.Dark(SystemColors.Control, 30));
			if (isFocused)
			{
				color = StiColors.SelectedText;
			}
			Pen val = new Pen(color);
			try
			{
				graphics.DrawRectangle(val, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		bounds.Width++;
		bounds.Height++;
		ControlPaint.DrawBorder3D(graphics, bounds, (Border3DStyle)10);
	}
}
