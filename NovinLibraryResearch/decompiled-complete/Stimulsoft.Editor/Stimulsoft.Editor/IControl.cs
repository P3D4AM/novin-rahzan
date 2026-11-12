using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IControl
{
	bool CanFocus { get; }

	bool Visible { get; set; }

	Point Location { get; set; }

	int Left { get; set; }

	int Top { get; set; }

	int Width { get; set; }

	int Height { get; set; }

	Rectangle Bounds { get; set; }

	Rectangle ClientRectangle { get; }

	Color ForeColor { get; set; }

	Color BackColor { get; set; }

	Image BackgroundImage { get; set; }

	Font Font { get; set; }

	Control Parent { get; set; }

	bool Focused { get; }

	bool Enabled { get; }

	IntPtr Handle { get; }

	bool IsHandleCreated { get; }

	event EventHandler Click;

	void Invalidate();

	void Invalidate(Rectangle rect);

	void Update();

	Point PointToClient(Point p);

	Point PointToScreen(Point p);

	bool Focus();
}
