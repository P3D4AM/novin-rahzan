using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.CrossTab;

public static class TreeViewExtension
{
	private const int HSCROLL = 0;

	private const int VSCROLL = 1;

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int GetScrollPos(int hWnd, int nBar);

	[DllImport("user32.dll")]
	public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

	public static Point ScrollPosition(this TreeView treeView)
	{
		return new Point(GetScrollPos((int)((Control)treeView).Handle, 0), GetScrollPos((int)((Control)treeView).Handle, 1));
	}

	public static void ScrollTo(this TreeView treeView, Point scrollPosition)
	{
		SetScrollPos(((Control)treeView).Handle, 0, scrollPosition.X, bRedraw: true);
		SetScrollPos(((Control)treeView).Handle, 1, scrollPosition.Y, bRedraw: true);
	}
}
