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

public class StiDataParametersEditor : UITypeEditor
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
		StiDataStoreSource stiDataStoreSource = context.Instance as StiDataStoreSource;
		StiDesignerControl stiDesignerControl = stiDataStoreSource?.Dictionary?.Report?.Designer?.DesignerControl as StiDesignerControl;
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(stiDesignerControl);
		Rectangle rectangle = ((Control)propertyGrid).RectangleToScreen(((Control)propertyGrid).ClientRectangle);
		StiDataParametersEditorForm stiDataParametersEditorForm = new StiDataParametersEditorForm(stiDataStoreSource);
		((Control)stiDataParametersEditorForm).Left = (StiPropertiesPanelHelper.IsRightDocked(stiDesignerControl) ? (rectangle.Left - ((Control)stiDataParametersEditorForm).Width) : rectangle.Right);
		((Control)stiDataParametersEditorForm).Top = rectangle.Top;
		stiDesignerControl?.AddOwnedForm((Form)(object)stiDataParametersEditorForm);
		StiInplaceDesigner.AddEditor(stiDataParametersEditorForm);
		((Control)stiDataParametersEditorForm).Show();
		return stiDataStoreSource.Parameters;
	}
}
