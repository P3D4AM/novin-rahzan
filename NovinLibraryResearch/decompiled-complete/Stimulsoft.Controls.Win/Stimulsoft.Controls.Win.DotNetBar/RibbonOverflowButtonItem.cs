using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class RibbonOverflowButtonItem : ButtonItem
{
	public RibbonBar RibbonBar;

	protected override void OnCommandChanged()
	{
	}

	protected override void OnExternalSizeChange()
	{
		base.OnExternalSizeChange();
		AdjustSubItemsRect();
	}

	public override void RecalcSize()
	{
		base.RecalcSize();
		AdjustSubItemsRect();
	}

	private void AdjustSubItemsRect()
	{
		Rectangle subItemsRect = base.SubItemsRect;
		if (!subItemsRect.IsEmpty)
		{
			subItemsRect.Y += 8;
			subItemsRect.Height -= 4;
			base.SubItemsRect = subItemsRect;
		}
	}
}
