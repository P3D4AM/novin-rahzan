using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiSubReportEditor : UITypeEditor
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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		if (!(context.Instance is StiSubReport stiSubReport))
		{
			return null;
		}
		StiSubReportPageSelectForm stiSubReportPageSelectForm = new StiSubReportPageSelectForm(stiSubReport);
		try
		{
			if (stiSubReport.Report != null && stiSubReport.Report.Designer != null)
			{
				stiSubReport.Report.Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "SubReportPage"), onlyCurrentPage: true);
				stiSubReport.Report.Designer.AddOwnedForm((Form)(object)stiSubReportPageSelectForm);
			}
			((Form)stiSubReportPageSelectForm).ShowDialog();
			return stiSubReport.SubReportPage;
		}
		finally
		{
			((IDisposable)stiSubReportPageSelectForm)?.Dispose();
		}
	}
}
