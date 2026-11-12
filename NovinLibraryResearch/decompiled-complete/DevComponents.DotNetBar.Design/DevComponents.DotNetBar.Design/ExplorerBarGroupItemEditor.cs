using System;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class ExplorerBarGroupItemEditor : CollectionEditor
{
	public ExplorerBarGroupItemEditor(Type type)
		: base(type)
	{
	}

	protected override Type CreateCollectionItemType()
	{
		return typeof(ButtonItem);
	}

	protected override object CreateInstance(Type itemType)
	{
		object obj = ((CollectionEditor)this).CreateInstance(itemType);
		if (obj is ButtonItem)
		{
			ButtonItem button = obj as ButtonItem;
			ExplorerBarGroupItem.SetDesignTimeDefaults(button, eExplorerBarStockStyle.Custom);
		}
		return obj;
	}
}
