using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Design.Editors.Indicator;
using Stimulsoft.Report;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiIndicatorConditionsEditor : UITypeEditor
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
		StiIndicatorElement stiIndicatorElement = context.Instance as StiIndicatorElement;
		StiReport stiReport = stiIndicatorElement?.Report;
		StiIndicatorElementConditionsEditorForm stiIndicatorElementConditionsEditorForm = new StiIndicatorElementConditionsEditorForm(stiIndicatorElement);
		try
		{
			stiReport?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiIndicatorElementConditionsEditorForm);
			((Form)stiIndicatorElementConditionsEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiIndicatorElementConditionsEditorForm)?.Dispose();
		}
		return stiIndicatorElement.Conditions;
	}
}
