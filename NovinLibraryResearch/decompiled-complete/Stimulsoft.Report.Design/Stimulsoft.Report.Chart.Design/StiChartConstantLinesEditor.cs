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

public class StiChartConstantLinesEditor : UITypeEditor
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
		StiChart stiChart = context.Instance as StiChart;
		StiDesignerControl stiDesignerControl = stiChart?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiChartConstantLinesEditorForm stiChartConstantLinesEditorForm = new StiChartConstantLinesEditorForm(stiChart);
		((Control)stiChartConstantLinesEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartConstantLinesEditorForm).Width) : rectangle.Right);
		((Control)stiChartConstantLinesEditorForm).Top = rectangle.Top;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartConstantLinesEditorForm);
		StiInplaceDesigner.AddEditor(stiChartConstantLinesEditorForm, stiChart);
		((Control)stiChartConstantLinesEditorForm).Show();
		return stiChart.ConstantLines;
	}
}
