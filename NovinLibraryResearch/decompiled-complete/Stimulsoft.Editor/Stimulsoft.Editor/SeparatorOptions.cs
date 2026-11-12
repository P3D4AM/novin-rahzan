using System;

namespace Stimulsoft.Editor;

[Flags]
public enum SeparatorOptions
{
	None = 0,
	HighlightCurrentLine = 1,
	HideHighlighting = 2,
	SeparateLines = 4,
	SeparateWrapLines = 8,
	SeparateContent = 0x10,
	SeparateBeyondEof = 0x20
}
