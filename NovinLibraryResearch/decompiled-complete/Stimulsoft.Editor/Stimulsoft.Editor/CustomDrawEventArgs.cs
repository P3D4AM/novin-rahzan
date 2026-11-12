using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public class CustomDrawEventArgs : EventArgs
{
	public IPainter Painter;

	public DrawStage DrawStage;

	public DrawState DrawState;

	public Rectangle Rect;

	public DrawInfo DrawInfo;

	public bool Handled;
}
