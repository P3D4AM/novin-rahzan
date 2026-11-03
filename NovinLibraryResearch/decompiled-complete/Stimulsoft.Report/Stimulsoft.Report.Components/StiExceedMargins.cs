using System;

namespace Stimulsoft.Report.Components;

[Flags]
public enum StiExceedMargins
{
	Left = 1,
	Right = 2,
	Top = 4,
	Bottom = 8,
	None = 0,
	All = 0xF
}
