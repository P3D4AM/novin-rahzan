using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Report.CrossTab.Core;

public class StiColumnCollection : CollectionBase, IComparer
{
	private int directionFactor = 1;

	private StiSortType sortType;

	private Hashtable items = new Hashtable();

	public StiColumn this[object name] => (StiColumn)items[name];

	public StiColumn this[int index] => (StiColumn)base.List[index];

	int IComparer.Compare(object x, object y)
	{
		StiColumn stiColumn = x as StiColumn;
		StiColumn stiColumn2 = y as StiColumn;
		if (sortType == StiSortType.ByDisplayValue)
		{
			if (stiColumn.DisplayValue != null && stiColumn2.DisplayValue != DBNull.Value)
			{
				if (!(stiColumn.DisplayValue is IComparable comparable))
				{
					return 1;
				}
				if (stiColumn2.OthersText != null)
				{
					return -1;
				}
				if (stiColumn.OthersText != null)
				{
					return 1;
				}
				return comparable.CompareTo(stiColumn2.DisplayValue) * directionFactor;
			}
			return 1;
		}
		if (stiColumn.Value != null)
		{
			if (stiColumn2.OthersText != null)
			{
				return -1;
			}
			if (stiColumn.OthersText != null)
			{
				return 1;
			}
			IComparable comparable2 = stiColumn.Value as IComparable;
			return comparable2.CompareTo(stiColumn2.Value) * directionFactor;
		}
		return 1;
	}

	public List<StiColumn> ToList()
	{
		return this.Cast<StiColumn>().ToList();
	}

	public void Insert(int position, object value)
	{
		base.List.Insert(position, value);
	}

	public void Add(object value, object displayValue)
	{
		Add(new StiColumn(value, displayValue));
	}

	public void Add(StiColumn col)
	{
		base.List.Add(col);
		if (!col.IsTotal)
		{
			items.Add(col.Value, col);
		}
	}

	public void Sort(StiSortDirection direction, StiSortType sortType)
	{
		this.sortType = sortType;
		directionFactor = ((direction == StiSortDirection.Asc) ? 1 : (-1));
		base.InnerList.Sort(this);
	}

	public new void Clear()
	{
		base.List.Clear();
		items.Clear();
	}
}
