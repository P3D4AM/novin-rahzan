using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Dialogs.Design;

public class StiGridColumnsEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)2;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		StiGridControl stiGridControl = context.Instance as StiGridControl;
		StiGridColumnsForm stiGridColumnsForm = new StiGridColumnsForm(stiGridControl);
		try
		{
			((Form)stiGridColumnsForm).ShowDialog();
			return stiGridControl.Columns;
		}
		finally
		{
			((IDisposable)stiGridColumnsForm)?.Dispose();
		}
	}
}
