using System.Drawing;

namespace Stimulsoft.Base.Wizards;

public interface IStiWizardService
{
	StiDesignerWizardReportType ReportType { get; }

	string ServiceName { get; }

	object Run(object designer, object owner, object baseReport);

	object GetWpfImage();

	Image GetWinFormsImage();
}
