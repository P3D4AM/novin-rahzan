using System;

namespace Stimulsoft.Report.Dashboard;

[Flags]
internal enum StiAvailableInteractionOnHover
{
	ShowToolTip = 1,
	ShowHyperlink = 2,
	None = 0,
	All = 3
}
