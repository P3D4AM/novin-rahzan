using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Design;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Design.Editors.Chart;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Dashboard.Design.Components;

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
		StiChartElement stiChartElement = context.Instance as StiChartElement;
		StiDesignerControl stiDesignerControl = stiChartElement?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiChartConstantLinesEditorForm stiChartConstantLinesEditorForm = new StiChartConstantLinesEditorForm(stiChartElement);
		((Control)stiChartConstantLinesEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiChartConstantLinesEditorForm).Width) : rectangle.Right);
		((Control)stiChartConstantLinesEditorForm).Top = rectangle.Top;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiChartConstantLinesEditorForm);
		((Control)stiChartConstantLinesEditorForm).Show();
		return stiChartElement.ConstantLines;
	}
}
