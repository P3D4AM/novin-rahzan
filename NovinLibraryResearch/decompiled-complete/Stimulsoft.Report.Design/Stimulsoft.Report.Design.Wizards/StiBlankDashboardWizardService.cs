using System.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Properties;

namespace Stimulsoft.Report.Design.Wizards;

[StiServiceCategoryBitmap(typeof(StiWizardService), "Stimulsoft.Report.Design.Bmp.Wizards.WizardService.gif")]
[StiToolbox(true)]
public class StiBlankDashboardWizardService : StiWizardService
{
	public override string ServiceName => StiLocalization.Get("Wizards", "BlankDashboard");

	public override string HelpUrl => "user-manual/reports_designer_creating_reports_in_designer_overview_standard_report_wizard.htm";

	public override Bitmap Image => Resources.BlankDashboard;

	public override StiReport CreateReport(StiReport report)
	{
		return StiReport.CreateNewDashboard();
	}
}
