using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiBarCodeTypeServiceEditor : UITypeEditor
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
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		StiBarCode stiBarCode;
		if (context.Instance is Array)
		{
			Array array = context.Instance as Array;
			stiBarCode = array.GetValue(0) as StiBarCode;
		}
		else
		{
			stiBarCode = context.Instance as StiBarCode;
		}
		if (stiBarCode == null)
		{
			return value;
		}
		IStiDesignerBase stiDesignerBase = null;
		if (stiBarCode?.Report?.Designer != null)
		{
			stiDesignerBase = stiBarCode.Report.Designer;
		}
		StiBarCodeEditorForm stiBarCodeEditorForm = new StiBarCodeEditorForm(stiBarCode);
		try
		{
			stiDesignerBase?.AddOwnedForm((Form)(object)stiBarCodeEditorForm);
			((Form)stiBarCodeEditorForm).ShowDialog();
			return value;
		}
		finally
		{
			((IDisposable)stiBarCodeEditorForm)?.Dispose();
		}
	}
}
