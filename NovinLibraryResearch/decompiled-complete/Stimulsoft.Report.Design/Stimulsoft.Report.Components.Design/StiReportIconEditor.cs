using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Components.Design;

public class StiReportIconEditor : UITypeEditor
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
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		byte[] array = value as byte[];
		StiReportIconEditorForm stiReportIconEditorForm = new StiReportIconEditorForm(array);
		try
		{
			if ((int)((Form)stiReportIconEditorForm).ShowDialog() == 1)
			{
				return stiReportIconEditorForm.ReportIcon;
			}
			return array;
		}
		finally
		{
			((IDisposable)stiReportIconEditorForm)?.Dispose();
		}
	}
}
