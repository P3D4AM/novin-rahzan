using System;
using System.ComponentModel.Design;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design;

public class ComboItemsEditor : CollectionEditor
{
	public ComboItemsEditor(Type type)
		: base(type)
	{
	}

	protected override Type CreateCollectionItemType()
	{
		return typeof(ComboItem);
	}

	protected override object CreateInstance(Type itemType)
	{
		object obj = ((CollectionEditor)this).CreateInstance(itemType);
		if (obj is ComboItem comboItem)
		{
			if (comboItem.Site != null && comboItem.Site.Name != "")
			{
				comboItem.Text = comboItem.Site.Name;
			}
			else
			{
				comboItem.Text = "comboItem";
			}
		}
		return obj;
	}
}
