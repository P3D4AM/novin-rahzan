using System;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Data.Helpers;

public static class StiDataTableConverter
{
	public static DataTable ToNetTable(StiDataTable dataTable, Type[] types = null)
	{
		if (dataTable == null)
		{
			return null;
		}
		DataTable table = new DataTable();
		object[] row = dataTable.Rows.FirstOrDefault();
		int meterIndex = 0;
		dataTable.Meters.ToList().ForEach(delegate(IStiMeter f)
		{
			table.Columns.Add(new DataColumn
			{
				ColumnName = table.GetUniqueName(f),
				DataType = GetDataType(row, meterIndex, types)
			});
			meterIndex++;
		});
		table.BeginLoadData();
		dataTable.Rows.ToList().ForEach(delegate(object[] a)
		{
			DataRow dataRow = table.NewRow();
			try
			{
				for (int i = 0; i < a.Length; i++)
				{
					if (i < types.Length)
					{
						try
						{
							if (a[i] is DBNull || a[i] == null)
							{
								dataRow[i] = DBNull.Value;
							}
							else
							{
								dataRow[i] = StiConvert.ChangeType(a[i], table.Columns[i].DataType);
							}
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
			table.Rows.Add(dataRow);
		});
		table.EndLoadData();
		return table;
	}

	private static Type GetDataType(object[] row, int meterIndex, Type[] types = null)
	{
		if (types != null && meterIndex < types.Length)
		{
			Type type = types[meterIndex];
			if (StiTypeWrapper.SimpleNullableTypes.Contains(type))
			{
				type = Nullable.GetUnderlyingType(type);
			}
			return type;
		}
		Type type2 = ((row != null) ? row[meterIndex].GetType() : typeof(object));
		if (!(type2 == typeof(DBNull)))
		{
			return type2;
		}
		return typeof(object);
	}
}
