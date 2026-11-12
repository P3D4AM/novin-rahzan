using System;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class SuperTabStripTabsEditor : CollectionEditor
{
	public SuperTabStripTabsEditor(Type type)
		: base(type)
	{
	}

	protected override Type CreateCollectionItemType()
	{
		return typeof(SuperTabItem);
	}

	protected override Type[] CreateNewItemTypes()
	{
		return new Type[8]
		{
			typeof(SuperTabItem),
			typeof(ButtonItem),
			typeof(TextBoxItem),
			typeof(ComboBoxItem),
			typeof(LabelItem),
			typeof(ColorPickerDropDown),
			typeof(ProgressBarItem),
			typeof(CheckBoxItem)
		};
	}

	protected override object CreateInstance(Type itemType)
	{
		object obj = ((CollectionEditor)this).CreateInstance(itemType);
		if (obj is SuperTabItem)
		{
			SuperTabItem superTabItem = obj as SuperTabItem;
			superTabItem.Text = (string.IsNullOrEmpty(superTabItem.Name) ? "My Tab" : superTabItem.Name);
		}
		else if (obj is ButtonItem)
		{
			ButtonItem buttonItem = obj as ButtonItem;
			buttonItem.Text = (string.IsNullOrEmpty(buttonItem.Name) ? "My Button" : buttonItem.Name);
		}
		else if (obj is TextBoxItem)
		{
			TextBoxItem textBoxItem = obj as TextBoxItem;
			textBoxItem.Text = (string.IsNullOrEmpty(textBoxItem.Name) ? "My TextBox" : textBoxItem.Name);
		}
		else if (obj is ComboBoxItem)
		{
			ComboBoxItem comboBoxItem = obj as ComboBoxItem;
			comboBoxItem.Text = (string.IsNullOrEmpty(comboBoxItem.Name) ? "My ComboBox" : comboBoxItem.Name);
		}
		else if (obj is LabelItem)
		{
			LabelItem labelItem = obj as LabelItem;
			labelItem.Text = (string.IsNullOrEmpty(labelItem.Name) ? "My Label" : labelItem.Name);
		}
		else if (obj is ColorItem)
		{
			ColorItem colorItem = obj as ColorItem;
			colorItem.Text = (string.IsNullOrEmpty(colorItem.Name) ? "My Color" : colorItem.Name);
		}
		else if (obj is ProgressBarItem)
		{
			ProgressBarItem progressBarItem = obj as ProgressBarItem;
			progressBarItem.Text = (string.IsNullOrEmpty(progressBarItem.Name) ? "My ProgressBar" : progressBarItem.Name);
		}
		else if (obj is CheckBoxItem)
		{
			CheckBoxItem checkBoxItem = obj as CheckBoxItem;
			checkBoxItem.Text = (string.IsNullOrEmpty(checkBoxItem.Name) ? "My CheckBox" : checkBoxItem.Name);
		}
		return obj;
	}
}
