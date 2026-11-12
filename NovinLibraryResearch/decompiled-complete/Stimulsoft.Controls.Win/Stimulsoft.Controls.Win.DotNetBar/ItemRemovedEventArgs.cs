using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemRemovedEventArgs : EventArgs
{
	private readonly BaseItem parent;

	public BaseItem Parent => parent;

	public ItemRemovedEventArgs(BaseItem parentItem)
	{
		parent = parentItem;
	}
}
