using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiWatermarkEditor : UITypeEditor
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
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		StiPage stiPage;
		if (context.Instance is Array)
		{
			Array array = context.Instance as Array;
			stiPage = array.GetValue(0) as StiPage;
		}
		else
		{
			stiPage = context.Instance as StiPage;
		}
		if (stiPage == null)
		{
			return value;
		}
		IStiDesignerBase stiDesignerBase = null;
		if (stiPage.Report != null && stiPage.Report.Designer != null)
		{
			stiDesignerBase = stiPage.Report.Designer;
		}
		StiPageSetupForm stiPageSetupForm = new StiPageSetupForm(stiPage, stiDesignerBase, 2);
		try
		{
			stiDesignerBase?.AddOwnedForm((Form)(object)stiPageSetupForm);
			if ((int)((Form)stiPageSetupForm).ShowDialog() == 1)
			{
				return stiPage.Interaction;
			}
			return value;
		}
		finally
		{
			((IDisposable)stiPageSetupForm)?.Dispose();
		}
	}
}
