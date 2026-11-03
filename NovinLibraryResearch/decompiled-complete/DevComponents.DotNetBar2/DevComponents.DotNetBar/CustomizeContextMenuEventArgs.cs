using System;

namespace DevComponents.DotNetBar;

public class CustomizeContextMenuEventArgs : EventArgs
{
	private readonly BaseItem ԟ;

	public BaseItem Parent => ԟ;

	public CustomizeContextMenuEventArgs(BaseItem parentItem)
	{
		ԟ = parentItem;
	}
}
