using System.Collections;
using System.Drawing;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemBlockLayoutManager : BlockLayoutManager
{
	public override void Layout(IBlock block, Size availableSize)
	{
		if (block is BaseItem)
		{
			BaseItem baseItem = block as BaseItem;
			if (baseItem is ItemContainer || baseItem.IsContainer)
			{
				baseItem.SuspendLayout = true;
				baseItem.WidthInternal = availableSize.Width;
				baseItem.HeightInternal = availableSize.Height;
				baseItem.SuspendLayout = false;
			}
			baseItem.RecalcSize();
			baseItem.Displayed = baseItem.Visible;
		}
	}

	public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
	{
		return blocksBounds;
	}
}
