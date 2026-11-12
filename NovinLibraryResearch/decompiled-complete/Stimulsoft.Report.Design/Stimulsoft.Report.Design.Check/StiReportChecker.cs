using System;
using System.Data;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Check;

public class StiReportChecker
{
	internal static event EventHandler GoToCode;

	public static void RunReportChecker()
	{
		StiOptions.Localization.LoadDefault();
		Type typeFromHandle = typeof(DataSet);
		typeFromHandle.ToString();
		RunReportChecker(null, checkKeyEscape: false);
	}

	public static void RunReportChecker(StiReport report, bool checkKeyEscape)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		StiChecksViewerForm stiChecksViewerForm = new StiChecksViewerForm(report, checkKeyEscape);
		try
		{
			if (report != null && report.Designer != null && report.Designer.DesignerControl != null)
			{
				report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiChecksViewerForm);
			}
			((Form)stiChecksViewerForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiChecksViewerForm)?.Dispose();
		}
	}

	internal static void InvokeGoToCode()
	{
		if (StiReportChecker.GoToCode != null)
		{
			StiReportChecker.GoToCode(null, EventArgs.Empty);
		}
	}
}
