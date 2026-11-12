using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Design.Editors.Progress;
using Stimulsoft.Report;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiProgressConditionsEditor : UITypeEditor
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
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		StiProgressElement stiProgressElement = context.Instance as StiProgressElement;
		StiReport stiReport = stiProgressElement?.Report;
		StiProgressElementConditionsEditorForm stiProgressElementConditionsEditorForm = new StiProgressElementConditionsEditorForm(stiProgressElement);
		try
		{
			stiReport?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiProgressElementConditionsEditorForm);
			((Form)stiProgressElementConditionsEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiProgressElementConditionsEditorForm)?.Dispose();
		}
		return stiProgressElement.Conditions;
	}
}
