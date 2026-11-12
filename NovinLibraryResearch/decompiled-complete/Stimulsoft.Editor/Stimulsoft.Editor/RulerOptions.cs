using System;

namespace Stimulsoft.Editor;

[Flags]
public enum RulerOptions
{
	None = 0,
	Discrete = 1,
	AllowDrag = 2,
	DisplayDragLine = 4
}
