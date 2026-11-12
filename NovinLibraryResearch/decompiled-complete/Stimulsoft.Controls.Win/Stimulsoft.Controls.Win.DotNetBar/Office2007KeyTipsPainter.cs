using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007KeyTipsPainter : KeyTipsPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	public override void PaintKeyTips(KeyTipsRendererEventArgs e)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		Color keyTipBorder = m_ColorTable.KeyTips.KeyTipBorder;
		Color color = m_ColorTable.KeyTips.KeyTipText;
		Color color2 = m_ColorTable.KeyTips.KeyTipBackground;
		if (e.ReferenceObject is BaseItem && !((BaseItem)e.ReferenceObject).Enabled)
		{
			int alpha = 128;
			color2 = Color.FromArgb(alpha, color2);
			color = Color.FromArgb(alpha, color);
			keyTipBorder = Color.FromArgb(alpha, keyTipBorder);
		}
		Graphics graphics = e.Graphics;
		string keyTip = e.KeyTip;
		Font font = e.Font;
		SolidBrush val = new SolidBrush(color2);
		try
		{
			DisplayHelp.FillRoundedRectangle(graphics, (Brush)(object)val, bounds, 2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(color, 1f);
		try
		{
			DisplayHelp.DrawRoundedRectangle(graphics, val2, bounds, 2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		TextDrawing.DrawString(graphics, keyTip, font, color, bounds, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
	}
}
