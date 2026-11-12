using System.Drawing;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class OneNote2007SuperTabStrip : SuperTabStripBaseDisplay
{
	private const int HTabSpacing = 5;

	private const int HTabOverLap = 10;

	private const int VTabSpacing = 7;

	private const int VTabOverLap = 10;

	internal override Size MinTabSize
	{
		get
		{
			if (base.TabStripItem.IsVertical && base.TabStripItem.HorizontalText)
			{
				return new Size(28, 30);
			}
			return new Size(28, 16);
		}
	}

	internal override int TabOverlap
	{
		get
		{
			if (!base.TabStripItem.IsVertical)
			{
				return 10;
			}
			return 10;
		}
	}

	internal override int TabSpacing
	{
		get
		{
			switch (base.TabStripItem.TabAlignment)
			{
			case eTabStripAlignment.Top:
			case eTabStripAlignment.Bottom:
				return 5;
			case eTabStripAlignment.Right:
				if (base.TabStripItem.HorizontalText)
				{
					return 7;
				}
				return 5;
			default:
				return 7;
			}
		}
	}

	public OneNote2007SuperTabStrip(SuperTabStripItem tabStripItem)
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
				e.NextPosition.Y += e.Block.Bounds.Height - 10;
			}
			else
			{
				e.NextPosition.X += e.Block.Bounds.Width - 10;
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
				result.Y -= 10;
			}
			else
			{
				result.X -= 10;
			}
		}
		return result;
	}
}
