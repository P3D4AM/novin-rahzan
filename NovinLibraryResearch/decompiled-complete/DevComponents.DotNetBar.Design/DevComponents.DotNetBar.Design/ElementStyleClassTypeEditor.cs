using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class ElementStyleClassTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService reference = ref edSvc;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			reference = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			if (edSvc != null)
			{
				ListBox val = new ListBox();
				val.SelectedIndexChanged += SelectedChanged;
				Type typeFromHandle = typeof(ElementStyleClassKeys);
				FieldInfo[] fields = typeFromHandle.GetFields(BindingFlags.Static | BindingFlags.Public);
				FieldInfo[] array = fields;
				foreach (FieldInfo fieldInfo in array)
				{
					string text = fieldInfo.GetValue(null).ToString();
					val.Items.Add((object)text);
					if (text == value.ToString())
					{
						val.SelectedItem = text;
					}
				}
				edSvc.DropDownControl((Control)(object)val);
				if (val.SelectedItem != null)
				{
					return val.SelectedItem.ToString();
				}
			}
		}
		return value;
	}

	private void SelectedChanged(object sender, EventArgs e)
	{
		if (edSvc != null)
		{
			edSvc.CloseDropDown();
		}
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}
}
