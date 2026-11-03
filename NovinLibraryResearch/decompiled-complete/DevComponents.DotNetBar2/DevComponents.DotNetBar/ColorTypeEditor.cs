using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.UI;

namespace DevComponents.DotNetBar;

public class ColorTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService _28E4;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService reference = ref _28E4;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			reference = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			ColorScheme ԫ = null;
			if (context.Instance != null)
			{
				MethodInfo methodInfo = null;
				if (context.Instance is object[])
				{
					methodInfo = ((object[])context.Instance)[0].GetType().GetMethod("GetColorScheme", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if ((object)methodInfo != null)
					{
						ԫ = methodInfo.Invoke(((object[])context.Instance)[0], null) as ColorScheme;
					}
				}
				else
				{
					methodInfo = context.Instance.GetType().GetMethod("GetColorScheme", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if ((object)methodInfo != null)
					{
						ԫ = methodInfo.Invoke(context.Instance, null) as ColorScheme;
					}
				}
			}
			Color _2358 = (Color)value;
			if (_28E4 != null)
			{
				ӓ ӓ = new ӓ();
				ӓ._2990 = _28E4;
				((Control)ӓ).BackColor = SystemColors.Control;
				ӓ.ԫ = ԫ;
				ӓ._2358 = _2358;
				string name = context.PropertyDescriptor.Name;
				PropertyInfo propertyInfo = null;
				propertyInfo = ((!(context.Instance is object[])) ? context.Instance.GetType().GetProperty(name + "SchemePart", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : ((object[])context.Instance)[0].GetType().GetProperty(name + "SchemePart", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				if ((object)propertyInfo != null)
				{
					if (context.Instance is object[])
					{
						ӓ._2991 = propertyInfo.GetValue(((object[])context.Instance)[0], null).ToString();
					}
					else
					{
						ӓ._2991 = propertyInfo.GetValue(context.Instance, null).ToString();
					}
				}
				ӓ._29AA();
				_28E4.DropDownControl((Control)(object)ӓ);
				if (!ӓ._2992)
				{
					Color _23582 = ӓ._2358;
					eColorSchemePart eColorSchemePart2 = eColorSchemePart.None;
					if (ӓ._2991 != "")
					{
						eColorSchemePart2 = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), ӓ._2991);
					}
					if ((object)propertyInfo != null)
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
					return _23582;
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
