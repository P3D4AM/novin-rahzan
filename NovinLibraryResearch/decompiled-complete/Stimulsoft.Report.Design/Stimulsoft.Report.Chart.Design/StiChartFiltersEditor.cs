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

public class StiChartFiltersEditor : UITypeEditor
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
			return ProcessSeries(series);
		}
		return ((UITypeEditor)this).EditValue(context, provider, value);
	}

	private static object ProcessSeries(StiSeries series)
	{
		StiChart stiChart = series.Chart as StiChart;
		StiDesignerControl stiDesignerControl = stiChart?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiChartFiltersEditorForm stiChartFiltersEditorForm = new StiChartFiltersEditorForm(stiChart, series);
		((Control)stiChartFiltersEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartFiltersEditorForm).Width) : rectangle.Right);
		((Control)stiChartFiltersEditorForm).Top = rectangle.Top;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartFiltersEditorForm);
		StiInplaceDesigner.AddEditor(stiChartFiltersEditorForm, stiChart);
		((Control)stiChartFiltersEditorForm).Show();
		return series.Filters;
	}
}
