using System.Drawing;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiRangeBand
{
	public int Top { get; set; }

	public int Bottom { get; set; }

	public int Height => Bottom - Top;

	public int OriginalTop { get; }

	public int OriginalBottom { get; }

	public int OriginalHeight => OriginalBottom - OriginalTop;

	public bool IsFixed { get; set; }

	public override string ToString()
	{
		string text = (IsFixed ? ", Fixed" : "");
		return $"Top-{Top}, Bottom-{Bottom}, Height-{Height}{text}";
	}

	public bool Intersect(Rectangle rect)
	{
		if (rect.Bottom > Top)
		{
			return rect.Top < Bottom;
		}
		return false;
	}

	public StiRangeBand(int top, int bottom)
	{
		OriginalTop = (Top = top);
		OriginalBottom = (Bottom = bottom);
	}
}
