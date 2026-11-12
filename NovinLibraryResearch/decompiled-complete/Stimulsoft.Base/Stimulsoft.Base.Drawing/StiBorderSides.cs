using System;

namespace Stimulsoft.Base.Drawing;

[Flags]
public enum StiBorderSides
{
	None = 0,
	All = 0xF,
	Top = 1,
	Left = 2,
	Right = 4,
	Bottom = 8
}
