using System;

namespace Stimulsoft.Editor;

[Flags]
public enum NotifyState
{
	None = 0,
	PositionChanged = 1,
	CountChanged = 2,
	OverWriteChanged = 4,
	ModifiedChanged = 8,
	SyntaxChanged = 0x10,
	ReadonlyChanged = 0x20,
	BlockChanged = 0x40,
	BookMarkChanged = 0x80,
	IncrementalSearchChanged = 0x100,
	SearcRectChanged = 0x200,
	Undo = 0x400,
	Edit = 0x800,
	Modified = 0x1000,
	Outline = 0x2000,
	WordWrap = 0x4000,
	GotoBookMark = 0x8000,
	SelectBlock = 0x10000,
	FirstSearchChanged = 0x20000,
	CenterLine = 0x40000,
	TextParsed = 0x80000,
	SmartFormat = 0x100000,
	StringsChanged = 0x200000
}
