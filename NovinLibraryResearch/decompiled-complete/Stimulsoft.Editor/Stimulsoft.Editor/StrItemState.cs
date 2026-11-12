using System;

namespace Stimulsoft.Editor;

[Flags]
public enum StrItemState : byte
{
	None = 0,
	Parsed = 1,
	Readonly = 2
}
