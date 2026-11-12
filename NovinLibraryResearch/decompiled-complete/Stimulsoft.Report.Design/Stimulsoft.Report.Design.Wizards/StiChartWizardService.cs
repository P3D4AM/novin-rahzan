using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Properties;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Wizards;

[StiServiceCategoryBitmap(typeof(StiWizardService), "Stimulsoft.Report.Design.Bmp.Wizards.WizardService.gif")]
[StiToolbox(true)]
public class StiChartWizardService : StiWizardService
{
	public override string ServiceName => StiLocalization.Get("Components", "StiChart");

	public override string HelpUrl => "user-manual/reports_designer_creating_reports_in_designer_overview_chart_wizard.htm";

	public override Bitmap Image => Resources.ChartReport;

	public override StiReport CreateReport(StiReport report)
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Invalid comparison between Unknown and I4
		if (report == null)
		{
			report = new StiReport();
		}
		StiReport report2 = StiReport.GetReport();
		StiPage currentPage = report2.GetCurrentPage();
		report2.Dictionary.DataSources = new StiDataSourcesCollection(report2.Dictionary);
		report2.Dictionary.DataSources.AddRange(report.Dictionary.DataSources);
		report2.Designer = report.Designer;
		StiOptions.Engine.GlobalEvents.InvokeWizardStart(report2, EventArgs.Empty);
		RectangleD rect = new RectangleD(0.0, 0.0, currentPage.Width, StiAlignValue.AlignToGrid(currentPage.Height / 14.0, report.Info.GridSize, report.Info.AlignToGrid));
		StiReportTitleBand stiReportTitleBand = new StiReportTitleBand(rect);
		stiReportTitleBand.Name = StiNameCreation.GenerateName(report2, StiLocalization.Get("Components", "StiReportTitleBand"), "ReportTitleBand");
		report2.Pages[0].Components.Add(stiReportTitleBand);
		StiText component = new StiText(rect, StiNameCreation.GenerateName(report2, StiLocalization.Get("Components", "StiChart"), "Chart"))
		{
			Name = StiNameCreation.GenerateName(report2, StiLocalization.Get("Components", "StiText"), "Text"),
			HorAlignment = StiTextHorAlignment.Center,
			VertAlignment = StiVertAlignment.Center,
			Font = new Font("Arial", 36f, (FontStyle)1),
			TextBrush = new StiSolidBrush(Color.SaddleBrown)
		};
		stiReportTitleBand.Components.Add(component);
		rect = new RectangleD(currentPage.Width / 12.0, currentPage.Height / 4.0, currentPage.Width - currentPage.Width / 6.0, currentPage.Height / 2.0);
		rect = StiAlignHelper.AlignToGrid(rect, report.Info.GridSize, report.Info.AlignToGrid);
		StiChart stiChart = new StiChart(rect)
		{
			Name = StiNameCreation.GenerateName(report2, StiLocalization.Get("Components", "StiChart"), "Chart")
		};
		report2.Pages[0].Components.Add(stiChart);
		StiChartEditorForm stiChartEditorForm = new StiChartEditorForm(stiChart);
		try
		{
			stiChart?.Report?.Designer?.DesignerControl.AddOwnedForm((Form)(object)stiChartEditorForm);
			if ((int)((Form)stiChartEditorForm).ShowDialog() == 1)
			{
				StiOptions.Engine.GlobalEvents.InvokeWizardFinish(report2, EventArgs.Empty);
				return report2;
			}
			return null;
		}
		finally
		{
			((IDisposable)stiChartEditorForm)?.Dispose();
		}
	}
}
