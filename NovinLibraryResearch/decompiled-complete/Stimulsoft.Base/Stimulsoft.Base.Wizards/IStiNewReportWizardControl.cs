using System;

namespace Stimulsoft.Base.Wizards;

public interface IStiNewReportWizardControl
{
	IntPtr IntPtr { get; set; }

	bool IsCreatedReportFromDataWizard { get; set; }

	bool IsBlankReport { get; set; }

	event EventHandler ReportCreated;

	event EventHandler ReportPathChanged;

	void InitShow();
}
