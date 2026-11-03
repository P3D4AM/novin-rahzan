using System;

namespace Stimulsoft.Report.Components;

public sealed class StiSortHelper
{
	public static int GetColumnIndexInSorting(string[] sorts, string columnName)
	{
		int num = 0;
		string text = "";
		int num2 = 0;
		foreach (string text2 in sorts)
		{
			if (text2 != "ASC" && text2 != "DESC")
			{
				text = ((text.Length != 0) ? (text + "." + text2) : text2);
			}
			if ((text2 == "ASC" || text2 == "DESC" || num2 == sorts.Length - 1) && text.Length > 0)
			{
				if (columnName == text)
				{
					return num;
				}
				text = "";
				num++;
			}
			num2++;
		}
		return -1;
	}

	public static StiInteractionSortDirection GetColumnSortDirection(string[] sorts, string columnName)
	{
		int num = GetColumnIndexInSorting(sorts, columnName);
		if (num == -1)
		{
			return StiInteractionSortDirection.None;
		}
		foreach (string text in sorts)
		{
			if (!(text == "ASC") && !(text == "DESC"))
			{
				continue;
			}
			if (num == 0)
			{
				if (text == "ASC")
				{
					return StiInteractionSortDirection.Ascending;
				}
				if (text == "DESC")
				{
					return StiInteractionSortDirection.Descending;
				}
			}
			num--;
		}
		return StiInteractionSortDirection.None;
	}

	public static string[] ChangeColumnSortDirection(string[] sorts, string columnName)
	{
		int num = GetColumnIndexInSorting(sorts, columnName);
		if (num == -1)
		{
			return sorts;
		}
		int num2 = 0;
		foreach (string text in sorts)
		{
			if (text == "ASC" || text == "DESC")
			{
				if (num == 0)
				{
					if (text == "ASC")
					{
						sorts[num2] = "DESC";
					}
					if (text == "DESC")
					{
						sorts[num2] = "ASC";
					}
					return sorts;
				}
				num--;
			}
			num2++;
		}
		return sorts;
	}

	public static bool IsColumnExistInSorting(string[] sorts, string columnName)
	{
		return GetColumnIndexInSorting(sorts, columnName) != -1;
	}

	public static string[] AddColumnToSorting(string[] sorts, string columnName, bool isAscending)
	{
		string[] array = columnName.Split(new char[1] { '.' });
		if (columnName.StartsWith("{", StringComparison.InvariantCulture) && columnName.EndsWith("}", StringComparison.InvariantCulture))
		{
			array = new string[1] { columnName };
		}
		string[] array2 = new string[sorts.Length + array.Length + 1];
		for (int i = 0; i < sorts.Length; i++)
		{
			array2[i] = sorts[i];
		}
		array2[sorts.Length] = (isAscending ? "ASC" : "DESC");
		for (int j = 0; j < array.Length; j++)
		{
			array2[sorts.Length + j + 1] = array[j];
		}
		return array2;
	}
}
