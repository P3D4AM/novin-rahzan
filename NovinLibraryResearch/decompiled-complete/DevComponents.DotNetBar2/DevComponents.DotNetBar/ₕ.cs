using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Rendering;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

internal class ₕ : ῲ
{
	private const int ₖ = 5;

	private const int ₗ = -1;

	internal override bool _1FFD => true;

	internal override int _1FFE => 5;

	public ₕ(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
		tabStripItem.ControlBox.MenuBox.Style = eDotNetBarStyle.Office2010;
	}

	protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
		BaseItem value = (BaseItem)e.Block;
		int num = base.ᇃ.IndexOf(value);
		if (num >= 0 && num + 1 < base.ᇃ.Count && base.ᇃ[num] is SuperTabItem && base.ᇃ[num + 1] is SuperTabItem)
		{
			e.NextPosition = e.CurrentPosition;
			if (base._1FD4.ᓙ)
			{
				e.NextPosition.Y += e.Block.Bounds.Height - -1;
			}
			else
			{
				e.NextPosition.X += e.Block.Bounds.Width - -1;
			}
			e.Cancel = true;
		}
	}

	internal override Rectangle _2013(BaseItem _06EC, BaseItem _2014)
	{
		Rectangle result = base._2013(_06EC, _2014);
		if (_06EC is SuperTabItem && _2014 is SuperTabItem)
		{
			if (base._1FD4.ᓙ)
			{
				result.Y -= -1;
			}
			else
			{
				result.X -= -1;
			}
		}
		return result;
	}

	protected override void DrawBackground(ItemPaintArgs p, SuperTabColorTable ct)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = p.Graphics;
		Rectangle displayRectangle = base._1FD4.DisplayRectangle;
		int num = ct.Background.GradientAngle;
		switch (base._1FD4.TabAlignment)
		{
		case eTabStripAlignment.Bottom:
			displayRectangle.Y--;
			break;
		case eTabStripAlignment.Right:
			displayRectangle.X--;
			break;
		case eTabStripAlignment.Top:
			num += 180;
			break;
		}
		Brush brush = ct.Background.GetBrush(displayRectangle, num);
		try
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)0;
			if (brush != null)
			{
				graphics.FillRectangle(brush, displayRectangle);
			}
			graphics.SmoothingMode = smoothingMode;
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
	}
}
