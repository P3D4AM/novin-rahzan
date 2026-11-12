using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2010BackstageSuperTabStrip : SuperTabStripBaseDisplay
{
	private const int SelectedPadding = 5;

	private const int HvTabOverLap = -1;

	internal override bool PreDrawStripBorder => true;

	internal override int SelectedPaddingWidth => 5;

	public Office2010BackstageSuperTabStrip(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
		tabStripItem.ControlBox.MenuBox.Style = eDotNetBarStyle.Office2010;
	}

	protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
		BaseItem value = (BaseItem)e.Block;
		int num = base.Tabs.IndexOf(value);
		if (num >= 0 && num + 1 < base.Tabs.Count && base.Tabs[num] is SuperTabItem && base.Tabs[num + 1] is SuperTabItem)
		{
			e.NextPosition = e.CurrentPosition;
			if (base.TabStripItem.IsVertical)
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

	internal override Rectangle NextBlockPosition(BaseItem item, BaseItem vItem)
	{
		Rectangle result = base.NextBlockPosition(item, vItem);
		if (item is SuperTabItem && vItem is SuperTabItem)
		{
			if (base.TabStripItem.IsVertical)
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
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = p.Graphics;
		Rectangle displayRectangle = base.TabStripItem.DisplayRectangle;
		int num = ct.Background.GradientAngle;
		switch (base.TabStripItem.TabAlignment)
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
