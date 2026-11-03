using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.AdvTree;

public class CellCollection : CollectionBase
{
	private Node ԙ;

	private Cell _ម;

	public Cell this[int index]
	{
		get
		{
			return (Cell)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public Cell this[string name]
	{
		get
		{
			foreach (Cell item in base.List)
			{
				if (item.Name == name)
				{
					return item;
				}
			}
			return null;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Node ParentNode => ԙ;

	public int Add(Cell cell)
	{
		return base.List.Add(cell);
	}

	public Cell GetByColumnName(string columnName)
	{
		if (string.IsNullOrEmpty(columnName))
		{
			throw new ArgumentException("columnName argument must be non-empty non-null string with column name");
		}
		Node parentNode = ParentNode;
		AdvTree treeControl = parentNode.TreeControl;
		if (treeControl == null)
		{
			throw new NullReferenceException("AdvTree control cannot be reached. Node is not added to a tree.");
		}
		Cell result = null;
		if (parentNode.Parent != null && parentNode.Parent.NodesColumns.Count > 0)
		{
			int num = parentNode.Parent.NodesColumns.IndexOf(columnName);
			if (num >= 0)
			{
				result = this[num];
			}
		}
		else
		{
			int num2 = treeControl.Columns.IndexOf(columnName);
			if (num2 >= 0)
			{
				result = this[num2];
			}
		}
		return result;
	}

	public void Insert(int index, Cell value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(Cell value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(Cell value)
	{
		return base.List.Contains(value);
	}

	public void Remove(Cell value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		Cell cell = value as Cell;
		cell.SetParent(null);
		if (ԙ != null)
		{
			ԙ.OnCellRemoved(cell);
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		Cell cell = value as Cell;
		if (cell.Parent != null && cell.Parent != ԙ)
		{
			cell.Parent.Cells.Remove(cell);
		}
		cell.SetParent(ԙ);
		if (ԙ != null)
		{
			ԙ.OnCellInserted(cell);
		}
	}

	protected override void OnInsert(int index, object value)
	{
		if (ԙ != null && ԙ.Site != null && ԙ.Site.DesignMode && base.List.Count > 0)
		{
			Cell cell = value as Cell;
			if (cell.Site == null && base.List.Contains(cell))
			{
				base.List.Remove(cell);
			}
		}
		base.OnInsert(index, value);
	}

	public void CopyTo(Cell[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(Cell[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		if (ԙ != null && ԙ.Site != null && ԙ.Site.DesignMode && base.List.Count > 0 && this[0].Site == null)
		{
			_ម = this[0];
		}
		base.OnClear();
	}

	protected override void OnClearComplete()
	{
		base.OnClearComplete();
		if (_ម != null)
		{
			Add(_ម);
			_ម = null;
		}
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		base.OnSet(index, oldValue, newValue);
	}

	internal void Ԟ(Node ԟ)
	{
		ԙ = ԟ;
	}
}
