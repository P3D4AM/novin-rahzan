using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class ShapeTypeEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		return (UITypeEditorEditStyle)2;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (context != null && provider != null)
		{
			IUIService uiService = (IUIService)provider.GetService(typeof(IUIService));
			IHelpService helpService = (IHelpService)provider.GetService(typeof(IHelpService));
			return EditShape(uiService, context.Instance, helpService, value);
		}
		return value;
	}

	internal static IShapeDescriptor EditShape(IUIService uiService, object instance, IHelpService helpService, object currentValue)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		ShapeEditorForm shapeEditorForm = new ShapeEditorForm();
		shapeEditorForm.Value = currentValue as IShapeDescriptor;
		if (uiService != null)
		{
			uiService.ShowDialog((Form)(object)shapeEditorForm);
		}
		else
		{
			((Form)shapeEditorForm).ShowDialog();
		}
		if ((int)((Form)shapeEditorForm).DialogResult == 1)
		{
			currentValue = shapeEditorForm.Value;
		}
		((Component)(object)shapeEditorForm).Dispose();
		return currentValue as IShapeDescriptor;
	}
}
