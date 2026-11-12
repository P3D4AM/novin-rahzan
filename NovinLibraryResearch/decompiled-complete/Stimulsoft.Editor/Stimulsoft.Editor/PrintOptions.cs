using System;

namespace Stimulsoft.Editor;

[Flags]
public enum PrintOptions
{
	None = 0,
	LineNumbers = 1,
	PageNumbers = 2,
	WordWrap = 4,
	PrintSelection = 8,
	UseColors = 0x10,
	UseSyntax = 0x20,
	UseHeader = 0x40,
	UseFooter = 0x80,
	DisplayProgress = 0x100
}
