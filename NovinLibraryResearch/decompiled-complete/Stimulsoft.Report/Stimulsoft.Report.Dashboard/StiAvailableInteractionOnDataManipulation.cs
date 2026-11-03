using System;

namespace Stimulsoft.Report.Dashboard;

[Flags]
internal enum StiAvailableInteractionOnDataManipulation
{
	AllowSorting = 1,
	AllowFiltering = 2,
	AllowDrillDown = 4,
	AllowColumnSelection = 8,
	All = 0xF,
	None = 0
}
