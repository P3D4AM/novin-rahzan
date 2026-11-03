using System;
using System.Collections;
using System.Data;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dictionary;

internal class StiBusinessObjectSort : IComparer
{
	private string[] sortColumns;

	private Hashtable rowToConditions;

	private object[,,] conditions;

	int IComparer.Compare(object x, object y)
	{
		try
		{
			if (x == y)
			{
				return 0;
			}
			if (rowToConditions != null && conditions != null)
			{
				int num = (int)rowToConditions[x];
				int num2 = (int)rowToConditions[y];
				int length = conditions.GetLength(1);
				for (int i = 0; i < length; i++)
				{
					object value = conditions[num, i, 0];
					object value2 = conditions[num2, i, 0];
					StiGroupSortDirection stiGroupSortDirection = (StiGroupSortDirection)conditions[num, i, 1];
					int num3 = CompareValues(value, value2, stiGroupSortDirection == StiGroupSortDirection.Ascending);
					if (num3 != 0)
					{
						return num3;
					}
				}
			}
			if (sortColumns == null || sortColumns.Length == 0)
			{
				return 0;
			}
			int j = 0;
			while (j < sortColumns.Length)
			{
				bool ascendary = sortColumns[j++] == "ASC";
				int num4 = j;
				for (; j < sortColumns.Length && sortColumns[j] != "ASC" && sortColumns[j] != "DESC"; j++)
				{
				}
				object obj = x;
				object obj2 = y;
				if (j - num4 > 1)
				{
					for (int k = num4; k < j - 1; k++)
					{
						obj = StiBusinessObjectHelper.GetValueFromObject(obj, sortColumns[k]);
						if (obj is DataRow[] array && array.Length != 0)
						{
							obj = array[0];
						}
						obj2 = StiBusinessObjectHelper.GetValueFromObject(obj2, sortColumns[k]);
						if (obj2 is DataRow[] array2 && array2.Length != 0)
						{
							obj2 = array2[0];
						}
					}
				}
				string name = sortColumns[j - 1];
				object valueFromObject = StiBusinessObjectHelper.GetValueFromObject(obj, name);
				object valueFromObject2 = StiBusinessObjectHelper.GetValueFromObject(obj2, name);
				int num5 = CompareValues(valueFromObject, valueFromObject2, ascendary);
				if (num5 != 0)
				{
					return num5;
				}
			}
			return 0;
		}
		catch
		{
			return 0;
		}
	}

	internal static int CompareValues(object value1, object value2)
	{
		return CompareValues(value1, value2, ascendary: true);
	}

	internal static int CompareValues(object value1, object value2, bool ascendary)
	{
		int num = 1;
		if (!ascendary)
		{
			num = -1;
		}
		if (value1 == DBNull.Value && value2 == DBNull.Value)
		{
			return 0;
		}
		if (value1 == DBNull.Value)
		{
			return -1 * num;
		}
		if (value2 == DBNull.Value)
		{
			return num;
		}
		IComparable comparable = value1 as IComparable;
		IComparable comparable2 = value2 as IComparable;
		if (comparable == null && comparable2 == null)
		{
			return 0;
		}
		if (comparable == null)
		{
			return -1 * num;
		}
		if (comparable2 == null)
		{
			return num;
		}
		if (comparable != comparable2)
		{
			int num2 = comparable.CompareTo(comparable2);
			if (!ascendary)
			{
				num2 = -num2;
			}
			if (num2 != 0)
			{
				return num2;
			}
		}
		return 0;
	}

	internal void Clear()
	{
		sortColumns = null;
	}

	public StiBusinessObjectSort(string[] sortColumns, Hashtable rowToConditions, object[,,] conditions)
	{
		this.sortColumns = sortColumns;
		this.conditions = conditions;
		this.rowToConditions = rowToConditions;
	}
}
