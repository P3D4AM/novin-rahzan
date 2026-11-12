using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Controls.Win.UI;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ColorTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService m_EditorService;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService editorService = ref m_EditorService;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			editorService = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			ColorScheme colorScheme = null;
			if (context.Instance != null)
			{
				MethodInfo methodInfo = null;
				if (context.Instance is object[])
				{
					methodInfo = ((object[])context.Instance)[0].GetType().GetMethod("GetColorScheme", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (methodInfo != null)
					{
						colorScheme = methodInfo.Invoke(((object[])context.Instance)[0], null) as ColorScheme;
					}
				}
				else
				{
					methodInfo = context.Instance.GetType().GetMethod("GetColorScheme", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (methodInfo != null)
					{
						colorScheme = methodInfo.Invoke(context.Instance, null) as ColorScheme;
					}
				}
			}
			Color selectedColor = (Color)value;
			if (m_EditorService != null)
			{
				ColorPicker colorPicker = new ColorPicker();
				colorPicker.EditorService = m_EditorService;
				((Control)colorPicker).BackColor = SystemColors.Control;
				colorPicker.ColorScheme = colorScheme;
				colorPicker.SelectedColor = selectedColor;
				string name = context.PropertyDescriptor.Name;
				PropertyInfo propertyInfo = null;
				propertyInfo = ((!(context.Instance is object[])) ? context.Instance.GetType().GetProperty(name + "SchemePart", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : ((object[])context.Instance)[0].GetType().GetProperty(name + "SchemePart", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				if (propertyInfo != null)
				{
					if (context.Instance is object[])
					{
						colorPicker.SelectedColorSchemeName = propertyInfo.GetValue(((object[])context.Instance)[0], null).ToString();
					}
					else
					{
						colorPicker.SelectedColorSchemeName = propertyInfo.GetValue(context.Instance, null).ToString();
					}
				}
				colorPicker.UpdateUIWithSelection();
				m_EditorService.DropDownControl((Control)(object)colorPicker);
				if (!colorPicker.Canceled)
				{
					Color selectedColor2 = colorPicker.SelectedColor;
					eColorSchemePart eColorSchemePart2 = eColorSchemePart.None;
					if (colorPicker.SelectedColorSchemeName != "")
					{
						eColorSchemePart2 = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), colorPicker.SelectedColorSchemeName);
					}
					if (propertyInfo != null)
					{
						if (context.Instance is object[])
						{
							object[] array = (object[])context.Instance;
							object[] array2 = array;
							foreach (object obj in array2)
							{
								propertyInfo = obj.GetType().GetProperty(name + "SchemePart", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								propertyInfo.SetValue(obj, eColorSchemePart2, null);
							}
						}
						else
						{
							propertyInfo.SetValue(context.Instance, eColorSchemePart2, null);
						}
					}
					return selectedColor2;
				}
			}
		}
		return value;
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
