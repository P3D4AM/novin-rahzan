using System;
using System.Drawing;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Rendering;

public class TabFormPainterArgs : EventArgs
{
	public Graphics Graphics;

	public TabParentForm TabParentForm;

	public bool Cancel;

	public TabFormPainterArgs(TabParentForm form, Graphics graphics)
	{
		Graphics = graphics;
		TabParentForm = form;
	}
}
