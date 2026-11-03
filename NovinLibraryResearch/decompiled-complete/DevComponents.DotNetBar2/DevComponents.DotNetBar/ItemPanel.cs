using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.ItemPanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(ItemPanel), "Ribbon.ItemPanel.ico")]
public class ItemPanel : ItemPanelBase, IScrollableItemControl, IBindingSupport
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Browsable(true)]
	[Category("Layout")]
	[DefaultValue(eOrientation.Horizontal)]
	[DevCoBrowsable(true)]
	public virtual eOrientation LayoutOrientation
	{
		get
		{
			return ItemContainer.LayoutOrientation;
		}
		set
		{
			ItemContainer.LayoutOrientation = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Category("Layout")]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public virtual bool ResizeItemsToFit
	{
		get
		{
			return ItemContainer.ResizeItemsToFit;
		}
		set
		{
			ItemContainer.ResizeItemsToFit = value;
		}
	}

	[Description("Indicates whether ButtonItem buttons when in vertical layout are fit into the available width so any text inside of them is wrapped if needed.")]
	[DefaultValue(false)]
	[Category("Layout")]
	public bool FitButtonsToContainerWidth
	{
		get
		{
			return ItemContainer.ᅚ;
		}
		set
		{
			ItemContainer.ᅚ = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Category("Layout")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates item alignment when container is in horizontal layout.")]
	[DefaultValue(eHorizontalItemsAlignment.Left)]
	public eHorizontalItemsAlignment HorizontalItemAlignment
	{
		get
		{
			return ItemContainer.HorizontalItemAlignment;
		}
		set
		{
			ItemContainer.HorizontalItemAlignment = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
	[Category("Layout")]
	[DefaultValue(false)]
	public virtual bool MultiLine
	{
		get
		{
			return ItemContainer.MultiLine;
		}
		set
		{
			ItemContainer.MultiLine = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => m_ItemContainer.SubItems;

	[DefaultValue(true)]
	[Category("Indicates whether block elements inside of container (affects span or div for example) when aligned center or right will reserve the space to the left.")]
	public bool ReserveLeftSpace
	{
		get
		{
			return ItemContainer.ReserveLeftSpace;
		}
		set
		{
			ItemContainer.ReserveLeftSpace = value;
		}
	}

	[Description("Gets or sets the index specifying the currently selected item.")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int SelectedIndex
	{
		get
		{
			return _SelectedIndex;
		}
		set
		{
			if (value == _SelectedIndex)
			{
				return;
			}
			if (value == -1)
			{
				_SelectedIndex = -1;
				_᪆(SelectedItem, _᪇: false);
			}
			else
			{
				BaseItem baseItem = null;
				if (_SelectedIndex >= Items.Count)
				{
					_SelectedIndex = -1;
					return;
				}
				if (_SelectedIndex > -1)
				{
					baseItem = Items[_SelectedIndex];
				}
				_᪆(SelectedItem, _᪇: false);
				_᪆(baseItem, _᪇: true);
				_SelectedIndex = value;
			}
			OnSelectedIndexChanged(EventArgs.Empty);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<BaseItem> SelectedItems
	{
		get
		{
			List<BaseItem> list = new List<BaseItem>();
			SubItemsCollection items = Items;
			_᪅(list, items);
			return list;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public BaseItem SelectedItem
	{
		get
		{
			int selectedIndex = SelectedIndex;
			if (selectedIndex == -1)
			{
				return null;
			}
			return Items[selectedIndex];
		}
		set
		{
			_᪆(SelectedItem, _᪇: false);
			_᪆(value, _᪇: true);
			if (value != null)
			{
				_SelectedIndex = Items.IndexOf(value);
			}
			else
			{
				_SelectedIndex = -1;
			}
		}
	}

	public ButtonItem GetChecked()
	{
		foreach (BaseItem item in Items)
		{
			if (item.Visible && item is ButtonItem && ((ButtonItem)item).Checked)
			{
				return item as ButtonItem;
			}
		}
		return null;
	}

	protected override void OnButtonCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (item.Checked)
		{
			_SelectedIndex = Items.IndexOf(item);
			OnSelectedIndexChanged(e);
		}
		base.OnButtonCheckedChanged(item, e);
	}

	public BaseItem AddItem(string text)
	{
		BaseItem itemTemplate = GetItemTemplate();
		if (itemTemplate == null)
		{
			throw new NullReferenceException("ItemTemplate property not set.");
		}
		BaseItem baseItem = itemTemplate.Copy();
		baseItem.Text = text;
		Items.Add(baseItem);
		return baseItem;
	}

	private static void _᪅(List<BaseItem> ڴ, SubItemsCollection ڵ)
	{
		foreach (BaseItem item in ڵ)
		{
			if (item is ItemContainer)
			{
				_᪅(ڴ, item.SubItems);
			}
			else if (item is ButtonItem { Checked: not false } buttonItem)
			{
				ڴ.Add(buttonItem);
			}
			else if (item is CheckBoxItem { Checked: not false } checkBoxItem)
			{
				ڴ.Add(checkBoxItem);
			}
			else if (item is MetroTileItem && ((MetroTileItem)item).Checked)
			{
				ڴ.Add(item);
			}
		}
	}

	private bool _᪆(BaseItem _06EC, bool _᪇)
	{
		if (_06EC == null)
		{
			return false;
		}
		bool result = true;
		if (_06EC is CheckBoxItem)
		{
			((CheckBoxItem)_06EC).Checked = _᪇;
		}
		else if (_06EC is ButtonItem)
		{
			((ButtonItem)_06EC).Checked = _᪇;
		}
		else if (_06EC is MetroTileItem)
		{
			((MetroTileItem)_06EC).Checked = _᪇;
		}
		else
		{
			result = false;
		}
		return result;
	}
}
