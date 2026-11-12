using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Design;

public class ItemSelectorTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private Type[] _AvailableTypes = new Type[4]
	{
		typeof(ButtonItem),
		typeof(CheckBoxItem),
		typeof(MetroTileItem),
		typeof(LabelItem)
	};

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
				for (int i = 0; i < _AvailableTypes.Length; i++)
				{
					Type type = _AvailableTypes[i];
					val.Items.Add((object)type.Name);
					if (value != null && (object)value.GetType() == type)
					{
						((ListControl)val).SelectedIndex = i;
					}
				}
				edSvc.DropDownControl((Control)(object)val);
				if (val.SelectedItem != null)
				{
					int selectedIndex = ((ListControl)val).SelectedIndex;
					if ((value == null || (object)_AvailableTypes[selectedIndex] != value.GetType()) && provider.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
					{
						DesignerTransaction designerTransaction = designerHost.CreateTransaction("Creating item");
						if (value is IComponent)
						{
							designerHost.DestroyComponent((IComponent)value);
						}
						value = designerHost.CreateComponent(_AvailableTypes[selectedIndex]);
						designerTransaction.Commit();
					}
				}
				return value;
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
