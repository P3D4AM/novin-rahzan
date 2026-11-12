using System;

namespace Stimulsoft.Editor;

[Flags]
public enum DrawState
{
	None = 0,
	Control = 1,
	Text = 2,
	Selection = 4,
	WhiteSpace = 8,
	LineHighlight = 0x10,
	LineSeparator = 0x20,
	LineBookMark = 0x40,
	Brace = 0x80,
	BeyondEol = 0x100,
	BeyondEof = 0x200,
	Gutter = 0x400,
	GutterImage = 0x800,
	BookMark = 0x1000,
	LineNumber = 0x2000,
	OutlineArea = 0x4000,
	OutlineImage = 0x8000,
	OutlineButton = 0x10000,
	LineModificator = 0x20000,
	Spelling = 0x40000,
	SyntaxError = 0x80000,
	Page = 0x100000,
	PageHeader = 0x200000,
	PageBorder = 0x400000,
	LineStyle = 0x800000,
	CodeSnippet = 0x1000000,
	UserMargin = 0x2000000
}
