using System.Drawing;

namespace Stimulsoft.Editor;

internal class CollapsedRange : OutRange
{
	public int DeltaX;

	public int DeltaY;

	public CollapsedRange(OutlineList owner, Point startPt, Point endPt, int deltaX, int deltaY, int level, string text)
		: base(owner, startPt, endPt, level, text)
	{
		DeltaX = deltaX;
		DeltaY = deltaY;
	}
}
