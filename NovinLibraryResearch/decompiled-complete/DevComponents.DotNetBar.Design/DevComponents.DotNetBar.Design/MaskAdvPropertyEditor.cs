using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class MaskAdvPropertyEditor : UITypeEditor
{
	internal static string EditMask(ITypeDiscoveryService discoverySvc, IUIService uiSvc, MaskedTextBoxAdv instance, IHelpService helpService)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		string result = null;
		Type type = Type.GetType("System.Windows.Forms.Design.MaskDesignerDialog, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
		ConstructorInfo constructor = type.GetConstructor(new Type[2]
		{
			typeof(MaskedTextBox),
			typeof(IHelpService)
		});
		object obj = constructor.Invoke(new object[2] { instance.MaskedTextBox, helpService });
		Form val = (Form)((obj is Form) ? obj : null);
		try
		{
			MethodInfo method = type.GetMethod("DiscoverMaskDescriptors");
			method.Invoke(val, new object[1] { discoverySvc });
			DialogResult val2 = ((uiSvc != null) ? uiSvc.ShowDialog(val) : val.ShowDialog());
			if ((int)val2 == 1)
			{
				PropertyInfo property = type.GetProperty("Mask");
				result = (string)property.GetValue(val, null);
				property = type.GetProperty("ValidatingType");
				Type type2 = property.GetValue(val, null) as Type;
				if ((object)type2 == instance.ValidatingType)
				{
					return result;
				}
				instance.ValidatingType = type2;
			}
		}
		finally
		{
			((Component)(object)val).Dispose();
		}
		return result;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		if (context != null && provider != null)
		{
			ITypeDiscoveryService discoverySvc = (ITypeDiscoveryService)provider.GetService(typeof(ITypeDiscoveryService));
			IUIService uiSvc = (IUIService)provider.GetService(typeof(IUIService));
			IHelpService helpService = (IHelpService)provider.GetService(typeof(IHelpService));
			string text = EditMask(discoverySvc, uiSvc, context.Instance as MaskedTextBoxAdv, helpService);
			if (text != null)
			{
				return text;
			}
		}
		return value;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		return (UITypeEditorEditStyle)2;
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return false;
	}
}
