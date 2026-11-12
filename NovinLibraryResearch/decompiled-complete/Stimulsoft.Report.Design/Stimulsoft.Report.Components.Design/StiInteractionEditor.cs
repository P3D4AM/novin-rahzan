using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Design.Interaction;

namespace Stimulsoft.Report.Components.Design;

public class StiInteractionEditor : UITypeEditor
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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		StiComponent stiComponent;
		if (context.Instance is Array)
		{
			Array array = context.Instance as Array;
			stiComponent = array.GetValue(0) as StiComponent;
		}
		else
		{
			stiComponent = context.Instance as StiComponent;
		}
		if (stiComponent != null)
		{
			StiInteractionEditorForm stiInteractionEditorForm = new StiInteractionEditorForm(stiComponent);
			try
			{
				if (stiComponent.Report != null && stiComponent.Report.Designer != null)
				{
					stiComponent.Report.Designer.AddOwnedForm((Form)(object)stiInteractionEditorForm);
				}
				if ((int)((Form)stiInteractionEditorForm).ShowDialog() == 1)
				{
					return stiComponent.Interaction;
				}
			}
			finally
			{
				((IDisposable)stiInteractionEditorForm)?.Dispose();
			}
		}
		return value;
	}
}
