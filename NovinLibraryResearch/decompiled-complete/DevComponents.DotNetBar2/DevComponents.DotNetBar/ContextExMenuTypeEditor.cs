using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar;

public class ContextExMenuTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService ᮅ;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService reference = ref ᮅ;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			reference = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			ContextMenuBar contextMenuBar = null;
			GridItem val = (GridItem)((provider is GridItem) ? provider : null);
			string text = "";
			if (val != null)
			{
				text = val.Label;
			}
			foreach (IComponent component in context.Container.Components)
			{
				if (component is ContextMenuBar && (text.EndsWith(((ContextMenuBar)component).Name) || text == ""))
				{
					contextMenuBar = component as ContextMenuBar;
					break;
				}
			}
			BaseItem baseItem = null;
			if (value != null && value is BaseItem)
			{
				baseItem = (BaseItem)value;
			}
			if (contextMenuBar != null && ᮅ != null)
			{
				ListBox val2 = new ListBox();
				val2.SelectedIndexChanged += ᮆ;
				foreach (BaseItem item in contextMenuBar.Items)
				{
					if (item.Name != "")
					{
						int selectedIndex = val2.Items.Add((object)item.Name);
						if (item == baseItem)
						{
							((ListControl)val2).SelectedIndex = selectedIndex;
						}
					}
				}
				ᮅ.DropDownControl((Control)(object)val2);
				if (val2.SelectedItem != null)
				{
					return contextMenuBar.Items[val2.SelectedItem.ToString()];
				}
			}
		}
		return value;
	}

	private void ᮆ(object _٠, EventArgs ؿ)
	{
		if (ᮅ != null)
		{
			ᮅ.CloseDropDown();
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
