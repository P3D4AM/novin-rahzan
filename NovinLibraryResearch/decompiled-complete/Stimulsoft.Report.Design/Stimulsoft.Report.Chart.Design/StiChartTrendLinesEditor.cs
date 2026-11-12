using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Chart.Design;

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
		if (context.Instance is StiSeries series)
		{
			return ProcessChart(series);
		}
		return ((UITypeEditor)this).EditValue(context, provider, value);
	}

	private static object ProcessChart(StiSeries series)
	{
		StiChart stiChart = series.Chart as StiChart;
		StiDesignerControl stiDesignerControl = stiChart?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiChartTrendLinesEditorForm stiChartTrendLinesEditorForm = new StiChartTrendLinesEditorForm(stiChart, series);
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		if (propertyGrid != null)
		{
			Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
			((Control)stiChartTrendLinesEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartTrendLinesEditorForm).Width) : rectangle.Right);
			((Control)stiChartTrendLinesEditorForm).Top = rectangle.Top;
			((Control)stiChartTrendLinesEditorForm).Height = rectangle.Height;
		}
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartTrendLinesEditorForm);
		StiInplaceDesigner.AddEditor(stiChartTrendLinesEditorForm, stiChart);
		((Control)stiChartTrendLinesEditorForm).Show();
		return series.TrendLines;
	}
}
