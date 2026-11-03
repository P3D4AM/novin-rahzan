using System;
using System.Drawing;

namespace DevComponents.DotNetBar.Controls;

public class PaintTabFormItemEventArgs : EventArgs
{
	public readonly TabFormItem Tab;

	public readonly Graphics Graphics;

	public bool PaintDefault = true;

	public PaintTabFormItemEventArgs(TabFormItem tab, Graphics g)
	{
		Tab = tab;
		Graphics = g;
	}
}
