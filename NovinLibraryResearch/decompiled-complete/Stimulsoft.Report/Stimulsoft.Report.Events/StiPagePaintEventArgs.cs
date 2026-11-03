using System;
using System.Drawing;

namespace Stimulsoft.Report.Events;

public class StiPagePaintEventArgs : EventArgs
{
	public Graphics Graphics { get; }

	public Rectangle ClipRectangle { get; }

	public Rectangle ClientRectangle { get; }

	public Rectangle FullRectangle { get; }

	public bool IsPrinting { get; }

	public bool IsDesigning { get; }

	public StiPagePaintEventArgs(Graphics graphics, Rectangle clipRectangle, Rectangle clientRectangle, Rectangle fullRectangle, bool isPrinting, bool isDesigning)
	{
		Graphics = graphics;
		ClipRectangle = clipRectangle;
		ClientRectangle = clientRectangle;
		FullRectangle = fullRectangle;
		IsPrinting = isPrinting;
		IsDesigning = isDesigning;
	}
}
