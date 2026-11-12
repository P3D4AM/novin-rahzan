using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Properties;

namespace Stimulsoft.Report.Design.Wizards;

[StiServiceCategoryBitmap(typeof(StiWizardService), "Stimulsoft.Report.Design.Bmp.Wizards.WizardService.gif")]
[StiToolbox(true)]
public class StiStandardWizardService : StiWizardService
{
	public override string ServiceName => StiLocalization.Get("Wizards", "StandardReport");

	public override string HelpUrl => "user-manual/reports_designer_creating_reports_in_designer_overview_standard_report_wizard.htm";

	public override Bitmap Image => Resources.StandardReport;

	public override StiReport CreateReport(StiReport report)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		StiStandardReportWizardForm stiStandardReportWizardForm = new StiStandardReportWizardForm(report, isMasterDetailWizard: false);
		try
		{
			report?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiStandardReportWizardForm);
			if ((int)((Form)stiStandardReportWizardForm).ShowDialog() == 1)
			{
				return stiStandardReportWizardForm.CreatedReport;
			}
			return null;
		}
		finally
		{
			((IDisposable)stiStandardReportWizardForm)?.Dispose();
		}
	}
}
