using System.Drawing;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class WinMediaPlayer12SuperTabStrip : SuperTabStripBaseDisplay
{
	private const int HvTabOverLap = -3;

	public WinMediaPlayer12SuperTabStrip(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
		tabStripItem.ControlBox.MenuBox.Style = eDotNetBarStyle.Office2010;
	}

	protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
		int num = base.Tabs.IndexOf((BaseItem)e.Block);
		if (num >= 0 && num + 1 < base.Tabs.Count && base.Tabs[num] is SuperTabItem && base.Tabs[num + 1] is SuperTabItem)
		{
			e.NextPosition = e.CurrentPosition;
			if (base.TabStripItem.IsVertical)
			{
				e.NextPosition.Y += e.Block.Bounds.Height - -3;
			}
			else
			{
				e.NextPosition.X += e.Block.Bounds.Width - -3;
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
				result.Y -= -3;
			}
			else
			{
				result.X -= -3;
			}
		}
		return result;
	}
}
