using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Dictionary;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataColumnsEditor : UITypeEditor
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
		StiDataSource stiDataSource = context.Instance as StiDataSource;
		StiDesignerControl stiDesignerControl = stiDataSource?.Dictionary?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiDataColumnsEditorForm stiDataColumnsEditorForm = new StiDataColumnsEditorForm(stiDataSource);
		((Control)stiDataColumnsEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiDataColumnsEditorForm).Width) : rectangle.Right);
		((Control)stiDataColumnsEditorForm).Top = rectangle.Top;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiDataColumnsEditorForm);
		StiInplaceDesigner.AddEditor(stiDataColumnsEditorForm);
		((Control)stiDataColumnsEditorForm).Show();
		return stiDataSource.Columns;
	}
}
