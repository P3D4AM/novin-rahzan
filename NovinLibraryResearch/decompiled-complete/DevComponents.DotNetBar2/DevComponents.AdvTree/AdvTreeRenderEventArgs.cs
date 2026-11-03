using System;
using System.Drawing;

namespace DevComponents.AdvTree;

public class AdvTreeRenderEventArgs : EventArgs
{
	public readonly eAdvTreeRenderType RenderType;

	public Graphics Graphics;

	public AdvTreeRenderEventArgs(eAdvTreeRenderType renderType, Graphics g)
	{
		RenderType = renderType;
		Graphics = g;
	}
}
