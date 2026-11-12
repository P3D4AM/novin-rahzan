using System.Drawing;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class VS2008DocumentSuperTabStrip : SuperTabStripBaseDisplay
{
	private const int HvTabSpacing = 0;

	private const int HvTabOverLap = 20;

	internal override int TabOverlap => 20;

	internal override int TabSpacing => 0;

	internal override bool TabOverlapLeft => true;

	internal override Size TabLayoutOffset => new Size(3, 2);

	internal override Size MinTabSize
	{
		get
		{
			if (base.TabStripItem.IsVertical && base.TabStripItem.HorizontalText)
			{
				return new Size(33, 35);
			}
			return new Size(33, 16);
		}
	}

	public VS2008DocumentSuperTabStrip(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
	}

	protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
		int num = base.Tabs.IndexOf((BaseItem)e.Block);
		if (num >= 0 && num + 1 < base.Tabs.Count && base.Tabs[num] is SuperTabItem && base.Tabs[num + 1] is SuperTabItem)
		{
			e.NextPosition = e.CurrentPosition;
			if (base.TabStripItem.IsVertical)
			{
				e.NextPosition.Y += e.Block.Bounds.Height - 20;
			}
			else
			{
				e.NextPosition.X += e.Block.Bounds.Width - 20;
			}
			e.Cancel = true;
		}
	}

	internal override Rectangle NextBlockPosition(BaseItem item, BaseItem vItem)
	{
		Rectangle result = base.NextBlockPosition(item, vItem);
		if (item is SuperTabItem && vItem is SuperTabItem)
		{
			if (base.IsVertical)
			{
				result.Y -= 20;
			}
			else
			{
				result.X -= 20;
			}
		}
		return result;
	}
}
