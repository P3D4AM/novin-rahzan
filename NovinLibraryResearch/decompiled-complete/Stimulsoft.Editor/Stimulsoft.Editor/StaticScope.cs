using System;

namespace Stimulsoft.Editor;

[Flags]
public enum StaticScope
{
	None = 0,
	Static = 1,
	Instance = 2,
	Global = 4,
	Protected = 8,
	TypeName = 0x10,
	Delegate = 0x20,
	Method = 0x40,
	Property = 0x80,
	Field = 0x100,
	ShortType = 0x200,
	Private = 0x400
}
