using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartSeriesEditor : UITypeEditor
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
		return ((UITypeEditor)this).EditValue(context, provider, value);
	}

	public static object ProcessChart(StiChart chart)
	{
		StiDesignerControl stiDesignerControl = chart?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		stiDesignerControl.UndoRedoSave(Loc.GetMain("Series"), onlyCurrentPage: true);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiChartSeriesEditorForm stiChartSeriesEditorForm = new StiChartSeriesEditorForm(chart);
		((Control)stiChartSeriesEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartSeriesEditorForm).Width) : rectangle.Right);
		((Control)stiChartSeriesEditorForm).Top = rectangle.Top;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartSeriesEditorForm);
		StiInplaceDesigner.AddEditor(stiChartSeriesEditorForm, chart);
		((Control)stiChartSeriesEditorForm).Show();
		return chart.Series;
	}
}
