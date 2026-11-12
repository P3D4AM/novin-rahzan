using System;

namespace Stimulsoft.Editor;

[Flags]
public enum BracesOptions
{
	None = 0,
	Highlight = 1,
	HighlightBounds = 2,
	TempHighlight = 4
}
