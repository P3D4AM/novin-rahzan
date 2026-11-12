using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Data.Extensions;

public static class StiDataTableExt
{
	public static DataTable ToNetTable(this StiDataTable table, bool onlyColumns = false)
	{
		return table.Rows.ToNetTable(table.Meters, onlyColumns);
	}

	public static DataTable Remove(this DataTable table, IEnumerable<DataColumn> columns)
	{
		if (columns == null)
		{
			return table;
		}
		columns.ToList().ForEach(delegate(DataColumn column)
		{
			table.Columns.Remove(column);
		});
		return table;
	}

	public static DataTable Remove(this DataTable table, IEnumerable<string> columns)
	{
		if (columns == null)
		{
			return table;
		}
		columns.ToList().ForEach(delegate(string column)
		{
			table.Columns.Remove(column);
		});
		return table;
	}

	public static List<DataColumn> ColumnsList(this DataTable table)
	{
		return table.Columns.Cast<DataColumn>().ToList();
	}

	public static DataColumn AddColumn(this DataTable table, DataColumn column, int order = -1)
	{
		table.Columns.Add(column);
		if (order != -1)
		{
			column.SetOrdinal(order);
		}
		return column;
	}
}
