using System;

namespace Stimulsoft.Editor;

[Flags]
public enum UndoFlags : byte
{
	None = 0,
	FirstTime = 1,
	Saved = 2
}
