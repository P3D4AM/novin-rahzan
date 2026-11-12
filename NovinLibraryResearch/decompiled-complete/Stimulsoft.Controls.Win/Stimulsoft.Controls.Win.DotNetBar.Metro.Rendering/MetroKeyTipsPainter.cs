using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroKeyTipsPainter : Office2007KeyTipsPainter
{
	public override void PaintKeyTips(KeyTipsRendererEventArgs e)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bounds.Inflate(1, 1);
		Color color = base.ColorTable.KeyTips.KeyTipText;
		Color color2 = base.ColorTable.KeyTips.KeyTipBackground;
		if (e.ReferenceObject is BaseItem && !((BaseItem)e.ReferenceObject).Enabled)
		{
			int alpha = 128;
			color2 = Color.FromArgb(alpha, color2);
			color = Color.FromArgb(alpha, color);
		}
		Graphics graphics = e.Graphics;
		string keyTip = e.KeyTip;
		Font font = e.Font;
		SolidBrush val = new SolidBrush(color2);
		try
		{
			DisplayHelp.FillRoundedRectangle(graphics, (Brush)(object)val, bounds, 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(color, 1f);
		try
		{
			DisplayHelp.DrawRoundedRectangle(graphics, val2, bounds, 1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		TextDrawing.DrawString(graphics, keyTip, font, color, bounds, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
	}
}
