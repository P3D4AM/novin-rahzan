using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SideBarPanelItemRendererEventArgs : EventArgs
{
	public readonly SideBarPanelItem SideBarPanelItem;

	public Graphics Graphics;

	internal ItemPaintArgs ItemPaintArgs;

	public SideBarPanelItemRendererEventArgs(SideBarPanelItem sideBarPanelItem, Graphics graphics)
	{
		SideBarPanelItem = sideBarPanelItem;
		Graphics = graphics;
	}
}
