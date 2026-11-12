using System;

namespace Stimulsoft.Editor;

[Flags]
public enum NavigateOptions
{
	None = 0,
	BeyondEol = 1,
	BeyondEof = 2,
	UpAtLineBegin = 4,
	DownAtLineEnd = 8,
	MoveOnRightButton = 0x10,
	LeaveCaretOnStringsChanged = 0x20
}
