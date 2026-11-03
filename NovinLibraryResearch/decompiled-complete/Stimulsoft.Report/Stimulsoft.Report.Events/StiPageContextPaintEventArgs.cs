using System.Drawing;

namespace Stimulsoft.Report.Events;

public class StiPageContextPaintEventArgs : StiPagePaintEventArgs
{
	public object Context { get; }

	public StiPageContextPaintEventArgs(object context, Rectangle clipRectangle, Rectangle clientRectangle, Rectangle fullRectangle, bool isPrinting, bool isDesigning)
		: base((Graphics)((context is Graphics) ? context : null), clipRectangle, clientRectangle, fullRectangle, isPrinting, isDesigning)
	{
		Context = context;
	}
}
