using System;

namespace Stimulsoft.Report.Components;

[Flags]
public enum StiConditionBorderSides
{
	None = 0,
	All = 0xF,
	Top = 1,
	Left = 2,
	Right = 4,
	Bottom = 8,
	NotAssigned = 0x10
}
