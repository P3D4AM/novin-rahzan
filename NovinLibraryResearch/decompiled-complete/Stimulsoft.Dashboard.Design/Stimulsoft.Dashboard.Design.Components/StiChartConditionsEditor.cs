using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Design.Editors.Chart;
using Stimulsoft.Report;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiChartConditionsEditor : UITypeEditor
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
		StiChartElement stiChartElement = context.Instance as StiChartElement;
		StiReport stiReport = stiChartElement?.Report;
		StiChartElementConditionsEditorForm stiChartElementConditionsEditorForm = new StiChartElementConditionsEditorForm(stiChartElement);
		try
		{
			stiReport?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiChartElementConditionsEditorForm);
			((Form)stiChartElementConditionsEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiChartElementConditionsEditorForm)?.Dispose();
		}
		return stiChartElement.Conditions;
	}
}
