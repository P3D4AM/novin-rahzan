using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Report;
using Stimulsoft.Report.Design.Dashboard.Chart;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiChartTrendLinesEditor : UITypeEditor
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
		//IL_0047: Invalid comparison between Unknown and I4
		StiChartElement stiChartElement = context.Instance as StiChartElement;
		StiReport stiReport = stiChartElement?.Report;
		StiChartTrendLinesEditorForm stiChartTrendLinesEditorForm = new StiChartTrendLinesEditorForm(stiChartElement);
		try
		{
			stiReport?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiChartTrendLinesEditorForm);
			if ((int)((Form)stiChartTrendLinesEditorForm).ShowDialog() == 1)
			{
				return stiChartTrendLinesEditorForm.TrendLines;
			}
		}
		finally
		{
			((IDisposable)stiChartTrendLinesEditorForm)?.Dispose();
		}
		return stiChartElement.TrendLines;
	}
}
