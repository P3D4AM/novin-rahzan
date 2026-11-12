using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Data.Extensions;

public static class DataTableExt
{
	public static DataTable NullTable = new DataTable();

	public static string GetUniqueName(this DataTable table, IStiMeter meter)
	{
		if (!table.Columns.Contains(meter.Label))
		{
			return meter.Label;
		}
		string function = StiExpressionHelper.GetFunction(meter.Expression);
		if (!string.IsNullOrWhiteSpace(function))
		{
			string text = meter.Label + "-" + function;
			if (!table.Columns.Contains(text))
			{
				return text;
			}
		}
		return table.GetUniqueName(meter.Label);
	}

	public static string GetUniqueName(this DataTable table, IStiMeter meter, string baseName)
	{
		if (!table.Columns.Contains(baseName))
		{
			return baseName;
		}
		string function = StiExpressionHelper.GetFunction(meter.Expression);
		if (!string.IsNullOrWhiteSpace(function))
		{
			string text = baseName + "-" + function;
			if (!table.Columns.Contains(text))
			{
				return text;
			}
		}
		return table.GetUniqueName(meter.Label);
	}

	public static string GetUniqueName(this DataTable table, string baseName)
	{
		if (baseName == null)
		{
			return null;
		}
		string text = baseName;
		int num = 2;
		while (table.Columns.Contains(text))
		{
			text = baseName + num++;
		}
		return text;
	}

	public static IEnumerable<DataRelation> ParentRelationList(this DataTable table)
	{
		return table.ParentRelations.Cast<DataRelation>().ToList();
	}

	public static IEnumerable<DataRelation> ChildRelationList(this DataTable table)
	{
		return table.ChildRelations.Cast<DataRelation>().ToList();
	}

	public static IEnumerable<object[]> AsEnumerableArray(this DataTable joinedTable)
	{
		List<object[]> list = (from r in joinedTable.AsEnumerable()
			select r.ItemArray).ToList();
		int num = 0;
		foreach (DataColumn column in joinedTable.Columns)
		{
			Type dataType = column.DataType;
			if (column.DataType.IsEnum)
			{
				foreach (object[] item in list)
				{
					object obj = item[num];
					if (obj != null && obj != DBNull.Value)
					{
						item[num] = Enum.ToObject(dataType, obj);
					}
				}
			}
			num++;
		}
		return list;
	}
}
