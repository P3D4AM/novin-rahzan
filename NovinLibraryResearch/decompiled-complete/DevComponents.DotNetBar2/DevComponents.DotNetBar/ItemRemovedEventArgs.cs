using System;

namespace DevComponents.DotNetBar;

public class ItemRemovedEventArgs : EventArgs
{
	private readonly BaseItem ԟ;

	public readonly int ItemIndex;

	public BaseItem Parent => ԟ;

	public ItemRemovedEventArgs(BaseItem parentItem, int itemIndex)
	{
		ItemIndex = itemIndex;
		ԟ = parentItem;
	}
}
