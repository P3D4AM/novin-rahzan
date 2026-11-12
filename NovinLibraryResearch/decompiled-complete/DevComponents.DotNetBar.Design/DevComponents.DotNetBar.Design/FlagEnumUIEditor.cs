using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class FlagEnumUIEditor : UITypeEditor
{
	private FlagCheckedListBox flagEnumCB;

	public FlagEnumUIEditor()
	{
		flagEnumCB = new FlagCheckedListBox();
		((ListBox)flagEnumCB).BorderStyle = (BorderStyle)0;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			IWindowsFormsEditorService val = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (val != null)
			{
				Enum enumValue = (Enum)Convert.ChangeType(value, context.PropertyDescriptor.PropertyType);
				flagEnumCB.EnumValue = enumValue;
				val.DropDownControl((Control)(object)flagEnumCB);
				return flagEnumCB.EnumValue;
			}
		}
		return null;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		return (UITypeEditorEditStyle)3;
	}
}
