using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Report.CrossTab.Core;

public class StiRowCollection : CollectionBase, IComparer
{
	private int directionFactor = 1;

	private StiSortType sortType;

	private Hashtable items = new Hashtable();

	public StiRow this[object name] => (StiRow)items[name];

	public StiRow this[int index] => (StiRow)base.List[index];

	int IComparer.Compare(object x, object y)
	{
		StiRow stiRow = x as StiRow;
		StiRow stiRow2 = y as StiRow;
		if (sortType == StiSortType.ByDisplayValue)
		{
			if (stiRow.DisplayValue != null && stiRow.DisplayValue != DBNull.Value)
			{
				if (stiRow2.DisplayValue == DBNull.Value)
				{
					return -1;
				}
				if (stiRow2.OthersText != null)
				{
					return -1;
				}
				if (stiRow.OthersText != null)
				{
					return 1;
				}
				if (!(stiRow.DisplayValue is IComparable comparable))
				{
					return 1;
				}
				try
				{
					return comparable.CompareTo(stiRow2.DisplayValue) * directionFactor;
				}
				catch
				{
					try
					{
						return comparable.ToString().CompareTo(stiRow2.DisplayValue.ToString()) * directionFactor;
					}
					catch
					{
						return -1;
					}
				}
			}
			return 1;
		}
		if (stiRow.Value != null && stiRow.Value != DBNull.Value)
		{
			if (stiRow2.Value == DBNull.Value)
			{
				return -1;
			}
			if (stiRow2.OthersText != null)
			{
				return -1;
			}
			if (stiRow.OthersText != null)
			{
				return 1;
			}
			if (!(stiRow.Value is IComparable comparable2))
			{
				return 1;
			}
			return comparable2.CompareTo(stiRow2.Value) * directionFactor;
		}
		return 1;
	}

	public List<StiRow> ToList()
	{
		return this.Cast<StiRow>().ToList();
	}

	public void Insert(int position, object value)
	{
		base.List.Insert(position, value);
	}

	public void Add(object value, object displayValue)
	{
		StiRow row = new StiRow(value, displayValue);
		Add(row);
	}

	public void Add(StiRow row)
	{
		base.List.Add(row);
		if (!row.IsTotal)
		{
			items.Add(row.Value, row);
		}
	}

	public new void Clear()
	{
		base.List.Clear();
		items.Clear();
	}

	public void Sort(StiSortDirection direction, StiSortType sortType)
	{
		this.sortType = sortType;
		directionFactor = ((direction == StiSortDirection.Asc) ? 1 : (-1));
		base.InnerList.Sort(this);
	}
}
