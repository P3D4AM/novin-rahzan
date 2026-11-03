using System;
using System.ComponentModel;
using System.Drawing;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar;

public class PropertyOptionListNode : PropertyNode
{
	public PropertyOptionListNode(PropertyDescriptor property)
		: base(property)
	{
	}

	internal override void _2E74()
	{
		base._2E74();
		Cell editCell = EditCell;
		ItemContainer itemContainer = new ItemContainer();
		itemContainer.LayoutOrientation = eOrientation.Vertical;
		string[] popupListContent = GetPopupListContent();
		string[] array = popupListContent;
		foreach (string text in array)
		{
			CheckBoxItem checkBoxItem = new CheckBoxItem();
			checkBoxItem.CheckBoxStyle = eCheckBoxStyle.RadioButton;
			checkBoxItem.Text = text;
			checkBoxItem.TextColor = Color.Black;
			checkBoxItem._0E62 = 0;
			checkBoxItem.CheckSignSize = new Size(11, 11);
			checkBoxItem.CheckedChanging += _2EBC;
			itemContainer.SubItems.Add(checkBoxItem);
		}
		itemContainer.Style = eDotNetBarStyle.Office2007;
		editCell.HostedItem = itemContainer;
	}

	protected override void Dispose(bool disposing)
	{
		BaseItem hostedItem = EditCell.HostedItem;
		if (hostedItem != null)
		{
			EditCell.HostedItem = null;
			foreach (BaseItem subItem in hostedItem.SubItems)
			{
				if (subItem is CheckBoxItem checkBoxItem)
				{
					checkBoxItem.CheckedChanging -= _2EBC;
				}
			}
			hostedItem.Dispose();
		}
		base.Dispose(disposing);
	}

	private void _2EBC(object _٠, CheckBoxChangeEventArgs ؿ)
	{
		if (ؿ.EventSource != eEventSource.Code)
		{
			Exception exception = null;
			object valueFromString = GetValueFromString(((CheckBoxItem)_٠).Text, out exception);
			ؿ.Cancel = !ApplyValue(valueFromString, exception);
		}
	}

	protected override void UpdateDisplayedValue(object propertyValue, bool refreshSubProperties)
	{
		base.UpdateDisplayedValue(propertyValue, refreshSubProperties);
		string propertyTextValue = GetPropertyTextValue(propertyValue);
		Cell editCell = EditCell;
		if (editCell.HostedItem == null)
		{
			return;
		}
		foreach (BaseItem subItem in editCell.HostedItem.SubItems)
		{
			if (subItem is CheckBoxItem checkBoxItem)
			{
				checkBoxItem.Checked = checkBoxItem.Text.Equals(propertyTextValue);
			}
		}
	}

	public override void EnterEditorMode(eTreeAction action, bool focusEditor)
	{
	}
}
