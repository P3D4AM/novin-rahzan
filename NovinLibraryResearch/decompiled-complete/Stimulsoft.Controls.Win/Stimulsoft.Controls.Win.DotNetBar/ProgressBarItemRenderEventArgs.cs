using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ProgressBarItemRenderEventArgs : EventArgs
{
	public Graphics Graphics;

	public ProgressBarItem ProgressBarItem;

	public bool RightToLeft;

	public Font Font;

	public ProgressBarItemRenderEventArgs(Graphics g, ProgressBarItem item, Font f, bool rtl)
	{
		Graphics = g;
		ProgressBarItem = item;
		RightToLeft = rtl;
		Font = f;
	}
}
