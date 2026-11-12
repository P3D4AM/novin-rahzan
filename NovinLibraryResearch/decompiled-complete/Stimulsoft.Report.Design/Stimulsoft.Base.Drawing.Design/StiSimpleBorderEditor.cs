using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Base.Drawing.Design;

public class StiSimpleBorderEditor : UITypeEditor
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
		StiSimpleBorderEditorForm stiSimpleBorderEditorForm = new StiSimpleBorderEditorForm(value as StiSimpleBorder);
		try
		{
			stiReport?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiSimpleBorderEditorForm);
			if ((int)((Form)stiSimpleBorderEditorForm).ShowDialog() == 1)
			{
				if (context.Instance is IStiSimpleBorder stiSimpleBorder)
				{
					stiSimpleBorder.Border = stiSimpleBorderEditorForm.Border;
				}
				return stiSimpleBorderEditorForm.Border;
			}
			return value as StiSimpleBorder;
		}
		finally
		{
			((IDisposable)stiSimpleBorderEditorForm)?.Dispose();
		}
	}
}
