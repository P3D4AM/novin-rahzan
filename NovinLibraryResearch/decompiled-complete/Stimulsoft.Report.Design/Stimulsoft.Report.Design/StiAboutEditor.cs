using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Report.Design;

internal class StiAboutEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	internal void OnDoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		StiAboutForm stiAboutForm = new StiAboutForm();
		try
		{
			((Form)stiAboutForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiAboutForm)?.Dispose();
		}
		return null;
	}
}
