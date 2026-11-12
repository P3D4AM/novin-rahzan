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
public class StiLabelWizardService : StiWizardService
{
	public override string ServiceName => StiLocalization.Get("Wizards", "LabelReport");

	public override string HelpUrl => "user-manual/reports_designer_creating_reports_in_designer_overview_label_report_wizard.htm";

	public override Bitmap Image => Resources.LabelReport;

	public override StiReport CreateReport(StiReport report)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		StiLabelReportWizardForm stiLabelReportWizardForm = new StiLabelReportWizardForm(report);
		try
		{
			report?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiLabelReportWizardForm);
			if ((int)((Form)stiLabelReportWizardForm).ShowDialog() == 1)
			{
				return stiLabelReportWizardForm.CreatedReport;
			}
			return null;
		}
		finally
		{
			((IDisposable)stiLabelReportWizardForm)?.Dispose();
		}
	}
}
