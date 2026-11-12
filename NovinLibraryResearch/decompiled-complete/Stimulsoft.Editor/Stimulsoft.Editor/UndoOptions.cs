using System;

namespace Stimulsoft.Editor;

[Flags]
public enum UndoOptions
{
	None = 0,
	AllowUndo = 1,
	GroupUndo = 2,
	UndoNavigations = 4,
	UndoAfterSave = 8,
	SingleBreakUndo = 0x10
}
