using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Components.Design;

public class StiSimpleImageEditor : UITypeEditor
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
		Image val = (Image)((value is Image) ? value : null);
		StiSimpleImageEditorForm stiSimpleImageEditorForm = new StiSimpleImageEditorForm(val);
		try
		{
			if ((int)((Form)stiSimpleImageEditorForm).ShowDialog() == 1)
			{
				return stiSimpleImageEditorForm.Image;
			}
			return val;
		}
		finally
		{
			((IDisposable)stiSimpleImageEditorForm)?.Dispose();
		}
	}
}
