using System;

namespace Stimulsoft.Base.Wizards;

public interface IStiWizardStartScreenControl
{
	IStiReport Report { get; }

	IntPtr IntPtrParentControl { get; set; }

	IntPtr IntPtrParentForm { get; set; }

	object RecentFile { get; set; }

	event EventHandler CloudReportOpened;

	event EventHandler RefreshRecentFiles;

	event EventHandler RecentFileChanged;

	event EventHandler ReportChanged;

	event EventHandler ReportPathChanged;

	event EventHandler ButtonMinimizeClick;

	event EventHandler ButtonMaximizeClick;

	event EventHandler ButtonCloseClick;
}
