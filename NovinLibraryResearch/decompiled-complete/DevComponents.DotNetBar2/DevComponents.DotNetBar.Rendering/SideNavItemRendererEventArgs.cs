using System;
using System.Drawing;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Rendering;

public class SideNavItemRendererEventArgs : EventArgs
{
	public SideNavItem Item;

	public Graphics Graphics;

	public bool Cancel;

	internal ItemPaintArgs က;

	public SideNavItemRendererEventArgs(SideNavItem item, Graphics g)
	{
		Item = item;
		Graphics = g;
	}
}
