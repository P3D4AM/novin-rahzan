using System;

namespace Stimulsoft.Report;

[Flags]
public enum StiStyleLocation
{
	None = 0,
	TopLeft = 1,
	TopCenter = 2,
	TopRight = 4,
	MiddleLeft = 8,
	MiddleCenter = 0x10,
	MiddleRight = 0x20,
	BottomLeft = 0x40,
	BottomCenter = 0x80,
	BottomRight = 0x100,
	Left = 0x200,
	Right = 0x400,
	Top = 0x800,
	Bottom = 0x1000,
	CenterHorizontal = 0x2000,
	CenterVertical = 0x4000
}
