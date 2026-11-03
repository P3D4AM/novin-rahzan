using System;
using System.Collections.Generic;

namespace Stimulsoft.Report.Chart;

internal class StiDataItemComparer : IComparer<StiDataItem>
{
	private int directionFactor;

	private StiSeriesSortType sortType;

	public int Compare(StiDataItem x, StiDataItem y)
	{
		IComparable comparable;
		IComparable comparable2;
		if (sortType == StiSeriesSortType.Value)
		{
			comparable = x.Value as IComparable;
			comparable2 = y.Value as IComparable;
		}
		else
		{
			comparable = x.Argument as IComparable;
			comparable2 = y.Argument as IComparable;
		}
		if (comparable == null && comparable2 == null)
		{
			return 0;
		}
		if (comparable == null)
		{
			return 1;
		}
		if (comparable2 == null)
		{
			return -1;
		}
		return comparable.CompareTo(comparable2) * directionFactor;
	}

	internal StiDataItemComparer(StiSeriesSortType sortType, StiSeriesSortDirection sortDirection)
	{
		directionFactor = 1;
		if (sortDirection == StiSeriesSortDirection.Descending)
		{
			directionFactor = -1;
		}
		this.sortType = sortType;
	}
}
