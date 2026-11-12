using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemContainerSeparatorRendererEventArgs : ItemContainerRendererEventArgs
{
	public BaseItem Item;

	public ItemContainerSeparatorRendererEventArgs(Graphics g, ItemContainer container, BaseItem item)
		: base(g, container)
	{
		Item = item;
	}
}
