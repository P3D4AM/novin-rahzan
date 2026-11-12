using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Data.Extensions;

public static class IEnumerableTableExt
{
	public static DataTable ToNetTable(this IEnumerable<object[]> source, IEnumerable<IStiMeter> meters, bool onlyColumns = false)
	{
		DataTable table = new DataTable();
		List<object[]> rows = source.ToList();
		int meterIndex = 0;
		meters.ToList().ForEach(delegate(IStiMeter f)
		{
			Type dataType = FindType(rows, meterIndex);
			table.Columns.Add(new DataColumn
			{
				ColumnName = table.GetUniqueName(f, StiLabelHelper.GetLabel(f)),
				DataType = dataType
			});
			meterIndex++;
		});
		if (!onlyColumns)
		{
			table.BeginLoadData();
			source.ToList().ForEach(delegate(object[] a)
			{
				LoadDataRow(table, a);
			});
			table.EndLoadData();
		}
		return table;
	}

	private static void LoadDataRow(DataTable table, object[] a)
	{
		try
		{
			table.LoadDataRow(a, fAcceptChanges: true);
		}
		catch
		{
			DataRow dataRow = table.NewRow();
			for (int i = 0; i < a.Length; i++)
			{
				try
				{
					dataRow[i] = a[i];
				}
				catch
				{
				}
			}
			table.Rows.Add(dataRow);
		}
	}

	private static Type FindType(List<object[]> rows, int meterIndex)
	{
		Type type = FindTypeInRows(rows, meterIndex);
		if (type != null && type != typeof(DBNull))
		{
			return type;
		}
		return typeof(object);
	}

	private static Type FindTypeInRows(List<object[]> rows, int columnIndex)
	{
		IEnumerable<Type> enumerable = (from r in rows
			select r[columnIndex] into v
			where v != null && v != DBNull.Value
			select v.GetType() into t
			where t != null && t != typeof(object)
			select t).Distinct();
		if (enumerable == null || !enumerable.Any() || enumerable.Count() > 1)
		{
			return typeof(object);
		}
		return enumerable.FirstOrDefault();
	}
}
