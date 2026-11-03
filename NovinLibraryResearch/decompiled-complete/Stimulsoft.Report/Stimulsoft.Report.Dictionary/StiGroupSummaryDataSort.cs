using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dictionary;

internal class StiGroupSummaryDataSort : IComparer<DataRow>
{
	private Hashtable groupSummaries;

	private Hashtable groupLines;

	private Hashtable baseRowOrder;

	private StiComponentsCollection groupHeaders;

	public int Compare(DataRow row1, DataRow row2)
	{
		try
		{
			if (row1 == row2)
			{
				return 0;
			}
			foreach (StiGroupHeaderBand groupHeader in groupHeaders)
			{
				if (groupHeader.SummarySortDirection != StiGroupSortDirection.None)
				{
					int num = ((groupHeader.SummarySortDirection == StiGroupSortDirection.Ascending) ? 1 : (-1));
					Hashtable hashtable = groupSummaries[groupHeader] as Hashtable;
					Hashtable hashtable2 = groupLines[groupHeader] as Hashtable;
					object value = hashtable[row1];
					object value2 = hashtable[row2];
					int num2 = CompareValues(value, value2);
					if (num2 != 0)
					{
						return num2 * num;
					}
					int num3 = (int)hashtable2[row1];
					int value3 = (int)hashtable2[row2];
					num2 = num3.CompareTo(value3);
					if (num2 != 0)
					{
						return num2 * num;
					}
				}
			}
			int num4 = (int)baseRowOrder[row1];
			int value4 = (int)baseRowOrder[row2];
			return num4.CompareTo(value4);
		}
		catch
		{
			return 0;
		}
	}

	private int CompareValues(object value1, object value2)
	{
		if (value1 is long || value2 is long)
		{
			return ((long)value1).CompareTo((long)value2);
		}
		if (value1 is decimal || value2 is decimal)
		{
			return ((decimal)value1).CompareTo((decimal)value2);
		}
		if (value1 is DateTime || value2 is DateTime)
		{
			return ((DateTime)value1).CompareTo((DateTime)value2);
		}
		if (value1 is TimeSpan || value2 is TimeSpan)
		{
			return ((TimeSpan)value1).CompareTo((TimeSpan)value2);
		}
		return 0;
	}

	public void Clear()
	{
		groupSummaries.Clear();
		groupSummaries = null;
		groupLines.Clear();
		groupLines = null;
		groupHeaders = null;
		baseRowOrder.Clear();
		baseRowOrder = null;
	}

	public StiGroupSummaryDataSort(Hashtable groupSummaries, Hashtable groupLines, StiComponentsCollection groupHeaders, Hashtable baseRowOrder)
	{
		this.groupSummaries = groupSummaries;
		this.groupLines = groupLines;
		this.groupHeaders = groupHeaders;
		this.baseRowOrder = baseRowOrder;
	}
}
