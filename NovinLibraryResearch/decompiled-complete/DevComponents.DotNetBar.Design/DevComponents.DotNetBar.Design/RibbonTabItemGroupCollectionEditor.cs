using System;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class RibbonTabItemGroupCollectionEditor : CollectionEditor
{
	public RibbonTabItemGroupCollectionEditor(Type type)
		: base(type)
	{
	}

	protected override Type CreateCollectionItemType()
	{
		return typeof(RibbonTabItemGroup);
	}

	protected override object CreateInstance(Type itemType)
	{
		object obj = ((CollectionEditor)this).CreateInstance(itemType);
		if (obj is RibbonTabItemGroup)
		{
			RibbonTabItemGroup defaults = obj as RibbonTabItemGroup;
			DesignerSupport.SetDefaults(defaults);
		}
		return obj;
	}
}
