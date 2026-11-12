using System;

namespace Stimulsoft.Base.Drawing;

[Flags]
public enum StiShadowSides
{
	Top = 1,
	Right = 2,
	Edge = 4,
	Bottom = 8,
	Left = 0x10,
	All = 0x1F
}
