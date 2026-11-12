using System;

namespace Stimulsoft.Editor;

[Flags]
public enum HitTest
{
	None = 0,
	Above = 1,
	Below = 2,
	Left = 4,
	Right = 8,
	Text = 0x10,
	Selection = 0x20,
	BeyondEol = 0x40,
	BeyondEof = 0x80,
	Gutter = 0x100,
	Margin = 0x200,
	GutterImage = 0x400,
	BookMark = 0x800,
	OutlineArea = 0x1000,
	OutlineImage = 0x2000,
	OutlineButton = 0x4000,
	LineModificator = 0x8000,
	LineNumber = 0x10000,
	HyperText = 0x20000,
	Page = 0x40000,
	PageWhiteSpace = 0x80000
}
