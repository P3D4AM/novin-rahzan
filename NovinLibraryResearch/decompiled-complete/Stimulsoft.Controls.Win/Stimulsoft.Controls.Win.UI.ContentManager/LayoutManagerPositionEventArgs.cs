using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.UI.ContentManager;

public class LayoutManagerPositionEventArgs : EventArgs
{
	public IBlock Block;

	public Point CurrentPosition = Point.Empty;

	public Point NextPosition = Point.Empty;

	public bool Cancel;
}
