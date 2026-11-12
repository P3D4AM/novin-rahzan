using System;
using System.ComponentModel.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class ToolboxControlGroupsEditor : CollectionEditor
{
	public ToolboxControlGroupsEditor(Type type)
		: base(type)
	{
	}

	protected override Type CreateCollectionItemType()
	{
		return typeof(ToolboxGroup);
	}

	protected override Type[] CreateNewItemTypes()
	{
		return new Type[1] { typeof(ToolboxGroup) };
	}

	protected override object CreateInstance(Type itemType)
	{
		object obj = ((CollectionEditor)this).CreateInstance(itemType);
		if (obj is ToolboxGroup)
		{
			ToolboxGroup toolboxGroup = obj as ToolboxGroup;
			toolboxGroup.Text = "Group";
		}
		return obj;
	}
}
