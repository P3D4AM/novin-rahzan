using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SideBarRendererEventArgs : EventArgs
{
	public readonly SideBar SideBar;

	public Graphics Graphics;

	internal ItemPaintArgs ItemPaintArgs;

	public SideBarRendererEventArgs(SideBar sideBar, Graphics graphics)
	{
		SideBar = sideBar;
		Graphics = graphics;
	}
}
