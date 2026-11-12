using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Base.Drawing.Design;

public class StiBorderEditor : UITypeEditor
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
		return ChangeBorder(context, value);
	}

	public static object ChangeBorder(ITypeDescriptorContext context, object value)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		StiReport stiReport = StiFontEditor.FindReportInContext(context);
		StiBorderEditorForm stiBorderEditorForm = new StiBorderEditorForm(value as StiBorder);
		try
		{
			stiReport?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiBorderEditorForm);
			if ((int)((Form)stiBorderEditorForm).ShowDialog() == 1)
			{
				if (context.Instance is IStiBorder stiBorder)
				{
					stiBorder.Border = stiBorderEditorForm.Border;
				}
				if (context.Instance is StiStyle stiStyle)
				{
					stiStyle.Border = stiBorderEditorForm.Border;
				}
				return stiBorderEditorForm.Border;
			}
			return value as StiBorder;
		}
		finally
		{
			((IDisposable)stiBorderEditorForm)?.Dispose();
		}
	}
}
