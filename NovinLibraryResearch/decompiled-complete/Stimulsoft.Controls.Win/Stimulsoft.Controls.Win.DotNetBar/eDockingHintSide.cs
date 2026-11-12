using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

[Flags]
internal enum eDockingHintSide
{
	Left = 1,
	Right = 2,
	Top = 4,
	Bottom = 8,
	DockTab = 0x10,
	All = 0x1F,
	Sides = 0xF
}
