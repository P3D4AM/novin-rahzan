using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class SideBarPanelItemRendererEventArgs : EventArgs
{
	public readonly SideBarPanelItem SideBarPanelItem;

	public Graphics Graphics;

	internal ItemPaintArgs က;

	public SideBarPanelItemRendererEventArgs(SideBarPanelItem sideBarPanelItem, Graphics graphics)
	{
		SideBarPanelItem = sideBarPanelItem;
		Graphics = graphics;
	}
}
