using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class CustomizeContextMenuEventArgs : EventArgs
{
	private readonly BaseItem parent;

	public BaseItem Parent => parent;

	public CustomizeContextMenuEventArgs(BaseItem parentItem)
	{
		parent = parentItem;
	}
}
