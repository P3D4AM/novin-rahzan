using System;

namespace Stimulsoft.Report.Dashboard;

[Flags]
internal enum StiAvailableInteractionOnClick
{
	ShowDashboard = 1,
	OpenHyperlink = 2,
	ApplyFilter = 4,
	DrillDown = 8,
	None = 0,
	All = 0xF
}
