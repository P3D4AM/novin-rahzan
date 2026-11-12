using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public class BlockDeletingEventArgs : EventArgs
{
	public Rectangle Rect;

	public BlockDeletingEventArgs(Rectangle rect)
	{
		Rect = rect;
	}
}
