using System;

namespace Stimulsoft.Editor;

[Flags]
public enum ScrollingOptions
{
	None = 0,
	SmoothScroll = 1,
	ShowScrollHint = 2,
	UseScrollDelta = 4,
	SystemScrollbars = 8,
	FlatScrollbars = 0x10,
	AllowSplitHorz = 0x20,
	AllowSplitVert = 0x40,
	HorzButtons = 0x80,
	VertButtons = 0x100
}
