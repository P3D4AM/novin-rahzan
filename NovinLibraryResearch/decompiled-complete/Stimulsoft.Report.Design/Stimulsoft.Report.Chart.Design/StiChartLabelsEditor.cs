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

public class StiChartLabelsEditor : UITypeEditor
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
		if (context.Instance is StiChart chart)
		{
			return ProcessChart(chart);
		}
		if (context.Instance is StiSeries series)
		{
			return ProcessChart(series);
		}
		return ((UITypeEditor)this).EditValue(context, provider, value);
	}

	private static object ProcessChart(StiChart chart)
	{
		StiDesignerControl stiDesignerControl = chart?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiChartLabelsEditorForm stiChartLabelsEditorForm = new StiChartLabelsEditorForm(chart, null);
		((Control)stiChartLabelsEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartLabelsEditorForm).Width) : rectangle.Right);
		((Control)stiChartLabelsEditorForm).Top = rectangle.Top;
		((Control)stiChartLabelsEditorForm).Height = rectangle.Height;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartLabelsEditorForm);
		StiInplaceDesigner.AddEditor(stiChartLabelsEditorForm, chart);
		((Control)stiChartLabelsEditorForm).Show();
		return chart.Labels;
	}

	private static object ProcessChart(StiSeries series)
	{
		StiChart stiChart = series.Chart as StiChart;
		StiDesignerControl stiDesignerControl = stiChart?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiChartLabelsEditorForm stiChartLabelsEditorForm = new StiChartLabelsEditorForm(stiChart, series);
		((Control)stiChartLabelsEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartLabelsEditorForm).Width) : rectangle.Right);
		((Control)stiChartLabelsEditorForm).Top = rectangle.Top;
		((Control)stiChartLabelsEditorForm).Height = rectangle.Height;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartLabelsEditorForm);
		StiInplaceDesigner.AddEditor(stiChartLabelsEditorForm, stiChart);
		((Control)stiChartLabelsEditorForm).Show();
		return series.SeriesLabels;
	}
}
