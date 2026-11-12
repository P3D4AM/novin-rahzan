using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class ColumnNamesEditor : UITypeEditor
{
	private TextBoxX _TextBox;

	public ColumnNamesEditor()
	{
		_TextBox = new TextBoxX();
		((TextBoxBase)_TextBox).Multiline = true;
		((TextBox)_TextBox).ScrollBars = (ScrollBars)2;
		_TextBox.WatermarkBehavior = eWatermarkBehavior.HideNonEmpty;
		_TextBox.WatermarkText = "Use Enter key to separate custom column names";
		_TextBox.Border.Class = ElementStyleClassKeys.TextBoxBorderKey;
		((TextBox)_TextBox).AcceptsReturn = true;
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
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(context.Instance)["DataSource"];
				if (propertyDescriptor == null)
				{
					return value;
				}
				((Control)_TextBox).Text = (string)value;
				((Control)_TextBox).MinimumSize = new Size(24, 120);
				val.DropDownControl((Control)(object)_TextBox);
				return ((Control)_TextBox).Text;
			}
		}
		return value;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		return (UITypeEditorEditStyle)3;
	}
}
