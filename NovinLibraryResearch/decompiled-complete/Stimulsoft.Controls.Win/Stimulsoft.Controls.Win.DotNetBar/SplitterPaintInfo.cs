using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SplitterPaintInfo
{
	public Graphics Graphics;

	public Rectangle DisplayRectangle;

	public SplitterColors Colors;

	public bool Expandable;

	public bool Expanded;

	public DockStyle Dock;
}
