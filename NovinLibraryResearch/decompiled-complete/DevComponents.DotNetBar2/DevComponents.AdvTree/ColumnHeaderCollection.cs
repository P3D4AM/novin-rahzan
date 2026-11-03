using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.AdvTree;

public class ColumnHeaderCollection : CollectionBase
{
	private class _17B7
	{
		public int _17B8 = -1;

		public int _17B9 = -1;

		public _17B7(int index, int displayIndex)
		{
			_17B8 = index;
			_17B9 = displayIndex;
		}
	}

	private class _17BA : IComparer<_17B7>
	{
		public int Compare(_17B7 x, _17B7 y)
		{
			if (x._17B9 == y._17B9)
			{
				return x._17B8 - y._17B8;
			}
			return x._17B9 - y._17B9;
		}
	}

	private Node _ᜯ;

	private AdvTree _ᗚ;

	private SortCellsEventHandler ᜎ;

	private bool _ᜰ;

	private bool _ᜱ;

	private List<int> __1732 = new List<int>();

	private bool __1733;

	private Rectangle __1734 = Rectangle.Empty;

	internal bool _1735;

	private bool __1736;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Node ParentNode => _ᜯ;

	internal AdvTree ކ
	{
		get
		{
			return _ᗚ;
		}
		set
		{
			_ᗚ = value;
		}
	}

	public ColumnHeader this[int index]
	{
		get
		{
			return (ColumnHeader)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public ColumnHeader this[string name]
	{
		get
		{
			int num = IndexOf(name);
			if (num == -1)
			{
				return null;
			}
			return this[num];
		}
		set
		{
			this[IndexOf(name)] = value;
		}
	}

	public bool IsSorted
	{
		get
		{
			return _ᜱ;
		}
		internal set
		{
			_ᜱ = value;
		}
	}

	internal List<int> ᜭ
	{
		get
		{
			if (!__1733)
			{
				ᝀ();
			}
			return __1732;
		}
	}

	public ColumnHeader LastVisibleColumn
	{
		get
		{
			List<int> list = ᜭ;
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (this[list[num]].Visible)
				{
					return this[list[num]];
				}
			}
			return null;
		}
	}

	public ColumnHeader FirstVisibleColumn
	{
		get
		{
			List<int> list = ᜭ;
			for (int i = 0; i < list.Count; i++)
			{
				if (this[list[i]].Visible)
				{
					return this[list[i]];
				}
			}
			return null;
		}
	}

	[Browsable(false)]
	public Rectangle Bounds => __1734;

	internal bool ᜮ
	{
		get
		{
			return __1736;
		}
		set
		{
			__1736 = value;
		}
	}

	[Description("Occurs before the cells are sorted.")]
	public event SortCellsEventHandler SortCells
	{
		add
		{
			SortCellsEventHandler sortCellsEventHandler = ᜎ;
			SortCellsEventHandler sortCellsEventHandler2;
			do
			{
				sortCellsEventHandler2 = sortCellsEventHandler;
				SortCellsEventHandler value2 = (SortCellsEventHandler)Delegate.Combine(sortCellsEventHandler2, value);
				sortCellsEventHandler = Interlocked.CompareExchange(ref ᜎ, value2, sortCellsEventHandler2);
			}
			while ((object)sortCellsEventHandler != sortCellsEventHandler2);
		}
		remove
		{
			SortCellsEventHandler sortCellsEventHandler = ᜎ;
			SortCellsEventHandler sortCellsEventHandler2;
			do
			{
				sortCellsEventHandler2 = sortCellsEventHandler;
				SortCellsEventHandler value2 = (SortCellsEventHandler)Delegate.Remove(sortCellsEventHandler2, value);
				sortCellsEventHandler = Interlocked.CompareExchange(ref ᜎ, value2, sortCellsEventHandler2);
			}
			while ((object)sortCellsEventHandler != sortCellsEventHandler2);
		}
	}

	internal void Ԟ(Node ԟ)
	{
		_ᜯ = ԟ;
	}

	public int Add(ColumnHeader ch)
	{
		return base.List.Add(ch);
	}

	public void Insert(int index, ColumnHeader value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(ColumnHeader value)
	{
		return base.List.IndexOf(value);
	}

	public int IndexOf(string name)
	{
		for (int i = 0; i < base.List.Count; i++)
		{
			if (this[i].Name == name)
			{
				return i;
			}
		}
		return -1;
	}

	public int IndexOfDataField(string dataFieldName)
	{
		dataFieldName = dataFieldName.ToLower();
		for (int i = 0; i < base.List.Count; i++)
		{
			if (this[i].DataFieldName.ToLower() == dataFieldName)
			{
				return i;
			}
		}
		return -1;
	}

	internal int _1737(string _06E3)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		_06E3 = _06E3.ToLower();
		for (int i = 0; i < base.List.Count; i++)
		{
			ColumnHeader columnHeader = this[i];
			if (columnHeader.Tag is BindingMemberInfo)
			{
				BindingMemberInfo val = (BindingMemberInfo)columnHeader.Tag;
				if (((BindingMemberInfo)(ref val)).BindingField.ToLower() == _06E3)
				{
					return i;
				}
			}
			if (columnHeader.DataFieldName.ToLower() == _06E3)
			{
				return i;
			}
			if (columnHeader.Name == _06E3)
			{
				return i;
			}
		}
		return -1;
	}

	public bool Contains(ColumnHeader value)
	{
		return base.List.Contains(value);
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		if (oldValue is ColumnHeader)
		{
			ColumnHeader columnHeader = (ColumnHeader)oldValue;
			if (columnHeader.SortDirection != 0)
			{
				IsSorted = false;
			}
		}
		if (newValue is ColumnHeader)
		{
			ColumnHeader columnHeader2 = (ColumnHeader)newValue;
			if (columnHeader2.SortDirection != 0)
			{
				IsSorted = true;
			}
		}
		base.OnSet(index, oldValue, newValue);
	}

	public void Remove(ColumnHeader value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		if (value is ColumnHeader)
		{
			ColumnHeader columnHeader = (ColumnHeader)value;
			if (columnHeader.SortDirection != 0)
			{
				IsSorted = false;
			}
			columnHeader.HeaderSizeChanged -= _170D;
			columnHeader.SortCells -= _173B;
			columnHeader.MouseDown -= new MouseEventHandler(_1739);
			columnHeader.MouseUp -= new MouseEventHandler(_1738);
			columnHeader.Parent = null;
		}
		InvalidateDisplayIndexes();
		_173F();
		base.OnRemoveComplete(index, value);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		if (value is ColumnHeader)
		{
			((ColumnHeader)value).HeaderSizeChanged += _170D;
			((ColumnHeader)value).SortCells += _173B;
			((ColumnHeader)value).MouseDown += new MouseEventHandler(_1739);
			((ColumnHeader)value).MouseUp += new MouseEventHandler(_1738);
			((ColumnHeader)value).Parent = this;
		}
		InvalidateDisplayIndexes();
		_173F();
		base.OnInsertComplete(index, value);
	}

	[Obfuscation(Exclude = true)]
	private void _1738(object _٠, MouseEventArgs ؿ)
	{
		_173A()?.ڱ(_٠, ؿ);
	}

	[Obfuscation(Exclude = true)]
	private void _1739(object _٠, MouseEventArgs ؿ)
	{
		_173A()?.ڲ(_٠, ؿ);
	}

	private AdvTree _173A()
	{
		AdvTree advTree = _ᗚ;
		if (advTree == null && _ᜯ != null)
		{
			advTree = _ᜯ.TreeControl;
		}
		return advTree;
	}

	protected virtual void OnSortCells(SortEventArgs e)
	{
		if (ᜎ != null)
		{
			ᜎ(this, e);
		}
	}

	private void _173B(object _٠, SortEventArgs ؿ)
	{
		ColumnHeader columnHeader = (ColumnHeader)_٠;
		int columnIndex = IndexOf(columnHeader);
		IComparer comparer = null;
		comparer = (ؿ.ReverseSort ? ((columnHeader.SortComparerReverse == null) ? new NodeComparerReverse(columnIndex) : columnHeader.SortComparerReverse) : ((columnHeader.SortComparer == null) ? new NodeComparer(columnIndex) : columnHeader.SortComparer));
		if (ؿ.Comparer == null)
		{
			ؿ.Comparer = comparer;
		}
		else
		{
			comparer = ؿ.Comparer;
		}
		OnSortCells(ؿ);
		if (!ؿ.Cancel)
		{
			if (_ᗚ != null)
			{
				_ᗚ.Nodes.Sort(comparer);
			}
			else if (_ᜯ != null)
			{
				_ᜯ.Nodes.Sort(comparer);
			}
		}
	}

	internal void _173C(ColumnHeader _173D)
	{
		if (_ᜰ)
		{
			return;
		}
		_ᜰ = true;
		try
		{
			if (_173D.SortDirection == eSortDirection.None)
			{
				IsSorted = false;
				return;
			}
			IsSorted = true;
			foreach (ColumnHeader item in base.List)
			{
				if (item != _173D && item.SortDirection != 0)
				{
					item.SortDirection = eSortDirection.None;
				}
			}
		}
		finally
		{
			_ᜰ = false;
		}
	}

	internal void _173E()
	{
		if (!_ᜱ)
		{
			return;
		}
		foreach (ColumnHeader item in base.List)
		{
			if (item.SortDirection != 0)
			{
				item.Sort(item.SortDirection == eSortDirection.Descending);
				break;
			}
		}
	}

	private void _173F()
	{
		if (_ᗚ != null)
		{
			_ᗚ.InvalidateNodesSize();
			((Control)_ᗚ).Invalidate();
			if (_ᗚ.Nodes.Count == 0)
			{
				_ᗚ.RecalcLayout();
			}
			else
			{
				_ᗚ._0654();
			}
		}
		else if (_ᜯ != null)
		{
			AdvTree treeControl = _ᜯ.TreeControl;
			if (treeControl != null)
			{
				treeControl.InvalidateNodeSize(_ᜯ);
				((Control)treeControl).Invalidate();
			}
		}
	}

	private void _170D(object _٠, EventArgs ؿ)
	{
		_173F();
	}

	public void CopyTo(ColumnHeader[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(ColumnHeader[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ColumnHeader columnHeader = (ColumnHeader)enumerator.Current;
				columnHeader.HeaderSizeChanged -= _170D;
				columnHeader.SortCells -= _173B;
				columnHeader.MouseDown -= new MouseEventHandler(_1739);
				columnHeader.MouseUp -= new MouseEventHandler(_1738);
				columnHeader.Parent = null;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		IsSorted = false;
		base.OnClear();
	}

	public int GetDisplayIndex(ColumnHeader column)
	{
		int num = IndexOf(column);
		ᝀ();
		for (int i = 0; i < __1732.Count; i++)
		{
			if (__1732[i] == num)
			{
				return i;
			}
		}
		return -1;
	}

	public ColumnHeader ColumnAtDisplayIndex(int displayIndex)
	{
		ᝀ();
		return this[__1732[displayIndex]];
	}

	private void ᝀ()
	{
		if (__1733)
		{
			return;
		}
		__1733 = true;
		__1732.Clear();
		List<_17B7> list = new List<_17B7>();
		bool flag = true;
		for (int i = 0; i < base.Count; i++)
		{
			int displayIndex = this[i].DisplayIndex;
			if (displayIndex != -1)
			{
				flag = false;
			}
			list.Add(new _17B7(i, displayIndex));
		}
		if (!flag)
		{
			list.Sort(new _17BA());
		}
		foreach (_17B7 item in list)
		{
			__1732.Add(item._17B8);
		}
	}

	internal void ᝁ(ColumnHeader ᝂ, int ᝃ, int ᝄ)
	{
		InvalidateDisplayIndexes();
		if (!__1736)
		{
			if (_ᗚ != null && !_ᗚ.IsUpdateSuspended)
			{
				_173F();
			}
			else if (_ᜯ != null && _ᜯ.TreeControl != null && !_ᜯ.TreeControl.IsUpdateSuspended)
			{
				_173F();
			}
		}
	}

	public void InvalidateDisplayIndexes()
	{
		__1733 = false;
	}

	internal void ᝅ(Rectangle ᝆ)
	{
		__1734 = ᝆ;
	}
}
