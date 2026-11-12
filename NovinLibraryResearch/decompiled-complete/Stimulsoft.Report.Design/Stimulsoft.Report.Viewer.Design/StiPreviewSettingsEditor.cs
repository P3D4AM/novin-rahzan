using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Viewer.Design;

public class StiPreviewSettingsEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		StiReport stiReport = context.Instance as StiReport;
		using (StiReport stiReport2 = new StiReport())
		{
			stiReport2.PreviewSettings = stiReport.PreviewSettings;
			stiReport2.DashboardViewerSettings = stiReport.DashboardViewerSettings;
			stiReport2.PreviewMode = stiReport.PreviewMode;
			if (stiReport.Designer != null && stiReport.Designer.DesignerControl != null)
			{
				stiReport.Designer.UndoRedoSave(StiLocalization.Get("FormViewer", "titlePageSettings"));
			}
			StiPreviewSettingsEditorForm.StartTab startTab = (stiReport.GetCurrentPage().IsDashboard ? StiPreviewSettingsEditorForm.StartTab.Dashboard : StiPreviewSettingsEditorForm.StartTab.Report);
			StiPreviewSettingsEditorForm stiPreviewSettingsEditorForm = new StiPreviewSettingsEditorForm(stiReport2, startTab);
			try
			{
				if (stiReport.Designer != null && stiReport.Designer.DesignerControl != null)
				{
					stiReport.Designer.DesignerControl.AddOwnedForm((Form)(object)stiPreviewSettingsEditorForm);
				}
				((Form)stiPreviewSettingsEditorForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiPreviewSettingsEditorForm)?.Dispose();
			}
			stiReport.PreviewSettings = stiReport2.PreviewSettings;
			stiReport.DashboardViewerSettings = stiReport2.DashboardViewerSettings;
			stiReport.PreviewMode = stiReport2.PreviewMode;
		}
		return stiReport.PreviewSettings;
	}
}
