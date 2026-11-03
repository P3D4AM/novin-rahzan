using System;

namespace Stimulsoft.Report;

[Flags]
public enum StiDashboardViewerSettings
{
	All = 0xFF,
	None = 0,
	ShowToolBar = 1,
	ShowRefreshButton = 2,
	ShowOpenButton = 4,
	ShowFullScreenButton = 8,
	ShowMenuButton = 0x10,
	ShowEditButton = 0x20,
	ShowReportSnapshots = 0x40,
	ShowExports = 0x80
}
