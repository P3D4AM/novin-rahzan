using System;

namespace Stimulsoft.Report.Components;

[Flags]
public enum StiShiftMode
{
	None = 0,
	IncreasingSize = 1,
	DecreasingSize = 2,
	OnlyInWidthOfComponent = 4
}
