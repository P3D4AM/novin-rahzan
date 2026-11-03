using System;
using System.Drawing;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors;

public class VisualCustomButton : VisualButton
{
	protected override void PaintButtonBackground(PaintInfo p, Office2007ButtonItemStateColorTable ct)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		base.PaintButtonBackground(p, ct);
		if (base.Text.Length != 0 || base.Image != null || !string.IsNullOrEmpty(base.Symbol))
		{
			return;
		}
		Point location = new Point(base.RenderBounds.X + (base.RenderBounds.Width - Dpi.Width7) / 2, base.RenderBounds.Bottom - Dpi.Height6);
		SolidBrush val = new SolidBrush(ct.Text);
		try
		{
			Size size = new Size(Dpi.Width2, Dpi.Height2);
			for (int i = 0; i < 3; i++)
			{
				p.Graphics.FillEllipse((Brush)(object)val, new Rectangle(location, size));
				location.X += size.Width + Dpi.Width1;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
