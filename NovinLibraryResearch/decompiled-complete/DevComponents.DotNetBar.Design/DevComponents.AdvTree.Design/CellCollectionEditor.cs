using System;
using System.ComponentModel.Design;

namespace DevComponents.AdvTree.Design;

public class CellCollectionEditor : CollectionEditor
{
	public CellCollectionEditor(Type type)
		: base(type)
	{
	}

	protected override Type CreateCollectionItemType()
	{
		return typeof(Cell);
	}

	protected override Type[] CreateNewItemTypes()
	{
		return new Type[1] { typeof(Cell) };
	}

	protected override object CreateInstance(Type itemType)
	{
		object obj = ((CollectionEditor)this).CreateInstance(itemType);
		if (obj is Cell)
		{
			Cell cell = obj as Cell;
			cell.Text = cell.Name;
		}
		return obj;
	}
}
