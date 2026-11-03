using System;

namespace Stimulsoft.Report;

[Flags]
public enum StiResizeReportOptions
{
	ProcessAllPages = 1,
	RebuildReport = 2,
	RescaleContent = 4,
	PageOrientationChanged = 8,
	ShowProgressOnRebuildReport = 0x10,
	AllowPageMarginsRescaling = 0x20
}
