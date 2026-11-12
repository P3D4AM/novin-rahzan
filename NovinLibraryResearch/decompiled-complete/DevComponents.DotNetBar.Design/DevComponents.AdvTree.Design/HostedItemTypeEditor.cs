using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Design;

public class HostedItemTypeEditor : UITypeEditor
{
	private const string OPTION_REMOVE = "Delete Item";

	private IWindowsFormsEditorService m_EditorService;

	private Dictionary<string, Type> _Items = new Dictionary<string, Type>();

	public HostedItemTypeEditor()
	{
		_Items.Add("Button", typeof(ButtonItem));
		_Items.Add("Check-box", typeof(CheckBoxItem));
		_Items.Add("Micro-Chart", typeof(MicroChartItem));
		_Items.Add("Switch", typeof(SwitchButtonItem));
		_Items.Add("Slider", typeof(SliderItem));
		_Items.Add("Rating", typeof(RatingItem));
		_Items.Add("Progress bar", typeof(ProgressBarItem));
		_Items.Add("Container", typeof(ItemContainer));
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService editorService = ref m_EditorService;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			editorService = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			if (m_EditorService != null)
			{
				Cell cell = null;
				if (context.Instance is Node)
				{
					cell = ((Node)context.Instance).Cells[0];
				}
				else if (context.Instance is Cell)
				{
					cell = (Cell)context.Instance;
				}
				if (cell == null)
				{
					MessageBox.Show($"Invalid context.Instance. Instance: {context.Instance}");
					return value;
				}
				ListBox val = new ListBox();
				if (cell.HostedItem != null)
				{
					val.Items.Add((object)"Delete Item");
					val.Items.Add((object)"------------------");
				}
				foreach (KeyValuePair<string, Type> item in _Items)
				{
					val.Items.Add((object)item.Key);
				}
				val.SelectedIndexChanged += SelectedChanged;
				m_EditorService.DropDownControl((Control)(object)val);
				IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
				if (val.SelectedItem != null && designerHost != null)
				{
					if (val.SelectedItem == "Delete Item")
					{
						BaseItem hostedItem = cell.HostedItem;
						designerHost.DestroyComponent(hostedItem);
						value = null;
					}
					else if (_Items.ContainsKey(val.SelectedItem.ToString()))
					{
						if (cell.HostedItem != null)
						{
							BaseItem hostedItem2 = cell.HostedItem;
							designerHost.DestroyComponent(hostedItem2);
						}
						BaseItem baseItem = designerHost.CreateComponent(_Items[(string)val.SelectedItem]) as BaseItem;
						if (!string.IsNullOrEmpty(baseItem.Name) && string.IsNullOrEmpty(baseItem.Text))
						{
							TypeDescriptor.GetProperties(baseItem)["Text"].SetValue(baseItem, baseItem.Name);
						}
						value = baseItem;
					}
				}
			}
		}
		return value;
	}

	private void SelectedChanged(object sender, EventArgs e)
	{
		if (m_EditorService != null)
		{
			m_EditorService.CloseDropDown();
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
