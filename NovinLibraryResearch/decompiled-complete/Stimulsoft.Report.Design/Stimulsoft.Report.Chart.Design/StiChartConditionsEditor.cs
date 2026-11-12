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
		if (context.Instance is StiSeriesLabels labels)
		{
			return ProcessLabels(labels);
		}
		if (context.Instance is StiSeries series)
		{
			return ProcessSeries(series);
		}
		return ((UITypeEditor)this).EditValue(context, provider, value);
	}

	private static object ProcessLabels(StiSeriesLabels labels)
	{
		StiChart stiChart = labels.Chart as StiChart;
		StiDesignerControl stiDesignerControl = stiChart?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiChartConditionsEditorForm stiChartConditionsEditorForm = new StiChartConditionsEditorForm(stiChart, null, labels);
		((Control)stiChartConditionsEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartConditionsEditorForm).Width) : rectangle.Right);
		((Control)stiChartConditionsEditorForm).Top = rectangle.Top;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartConditionsEditorForm);
		StiInplaceDesigner.AddEditor(stiChartConditionsEditorForm, stiChart);
		((Control)stiChartConditionsEditorForm).Show();
		return labels.Conditions;
	}

	private static object ProcessSeries(StiSeries series)
	{
		StiChart stiChart = series.Chart as StiChart;
		StiDesignerControl stiDesignerControl = stiChart?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiChartConditionsEditorForm stiChartConditionsEditorForm = new StiChartConditionsEditorForm(stiChart, series, null);
		((Control)stiChartConditionsEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartConditionsEditorForm).Width) : rectangle.Right);
		((Control)stiChartConditionsEditorForm).Top = rectangle.Top;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartConditionsEditorForm);
		StiInplaceDesigner.AddEditor(stiChartConditionsEditorForm, stiChart);
		((Control)stiChartConditionsEditorForm).Show();
		return series.Conditions;
	}
}
