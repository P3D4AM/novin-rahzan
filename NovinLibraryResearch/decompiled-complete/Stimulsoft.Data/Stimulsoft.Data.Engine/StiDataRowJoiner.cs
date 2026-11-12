using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Data.Engine;

public class StiDataRowJoiner
{
	private DataTable resultTable;

	private DataTable table1;

	private DataTable table2;

	private Dictionary<string, int> resultColumnIndexes = new Dictionary<string, int>();

	private Dictionary<string, int> column1Indexes = new Dictionary<string, int>();

	private Dictionary<string, int> column2Indexes = new Dictionary<string, int>();

	public IEnumerable<DataRow> Join(StiDataJoinType type, StiDataLink link, IEnumerable<IStiMeter> meters)
	{
		return type switch
		{
			StiDataJoinType.Inner => InnerJoinRows(link), 
			StiDataJoinType.Left => LeftJoinRows(link, meters), 
			StiDataJoinType.Cross => CrossJoinRows(), 
			StiDataJoinType.Full => FullJoinRows(link), 
			_ => throw new NotSupportedException(), 
		};
	}

	private IEnumerable<DataRow> LeftJoinRows(StiDataLink link, IEnumerable<IStiMeter> meters)
	{
		if (StiDataJoiner.JoinEngine == StiDataJoiner.StiDataJoinEngine.V1 || StiDataJoiner.JoinEngine == StiDataJoiner.StiDataJoinEngine.V4)
		{
			return LeftJoinRowsV1(link);
		}
		return LeftJoinRowsV2V3(link, meters);
	}

	private IEnumerable<DataRow> InnerJoinRows(StiDataLink link)
	{
		int fieldIndex1 = GetFieldIndex(table1, link);
		int fieldIndex2 = GetFieldIndex(table2, link);
		List<DataRow> outer = table1.AsEnumerable().ToList();
		List<DataRow> inner = table2.AsEnumerable().ToList();
		return outer.Join(inner, (DataRow r1) => GetHashCode(r1, fieldIndex1), (DataRow r2) => GetHashCode(r2, fieldIndex2), SplitRows);
	}

	private IEnumerable<DataRow> LeftJoinRowsV1(StiDataLink link)
	{
		int fieldIndex1 = GetFieldIndex(table1, link);
		int fieldIndex2 = GetFieldIndex(table2, link);
		List<DataRow> outer = table1.AsEnumerable().ToList();
		List<DataRow> inner = table2.AsEnumerable().ToList();
		return (from r1 in outer
			join r2 in inner on GetHashCode(r1, fieldIndex1) equals GetHashCode(r2, fieldIndex2) into r2
			select new
			{
				key = r1,
				rows = r2
			}).SelectMany(a => a.rows.DefaultIfEmpty(), (r1, DataRow r2) => SplitRows(r1.key, r2));
	}

	private IEnumerable<DataRow> LeftJoinRowsV2V3(StiDataLink link, IEnumerable<IStiMeter> meters)
	{
		int fieldIndex1 = GetFieldIndex(table1, link);
		int fieldIndex2 = GetFieldIndex(table2, link);
		List<DataRow> outer = table1.AsEnumerable().ToList();
		List<DataRow> inner = table2.AsEnumerable().ToList();
		var enumerable = from r1 in outer
			join r2 in inner on GetHashCode(r1, fieldIndex1) equals GetHashCode(r2, fieldIndex2) into r2
			select new
			{
				key = r1,
				rows = r2
			};
		Hashtable hashtable = CalculateIndexes(meters);
		List<DataRow> list = new List<DataRow>();
		foreach (var item in enumerable)
		{
			bool flag = true;
			foreach (DataRow item2 in item.rows.DefaultIfEmpty())
			{
				if (flag)
				{
					list.Add(SplitRows(item.key, item2));
				}
				else
				{
					object[] itemArray = item.key.ItemArray;
					object[] array = new object[itemArray.Length];
					for (int i = 0; i < itemArray.Length; i++)
					{
						if (StiDataJoiner.JoinEngine == StiDataJoiner.StiDataJoinEngine.V2)
						{
							if (!IsNumericType(itemArray[i]))
							{
								array[i] = itemArray[i];
							}
						}
						else if (StiDataJoiner.JoinEngine == StiDataJoiner.StiDataJoinEngine.V3 && (!hashtable.ContainsKey(i) || !IsNumericType(itemArray[i])))
						{
							array[i] = itemArray[i];
						}
					}
					DataRow row = resultTable.LoadDataRow(array, fAcceptChanges: false);
					list.Add(SplitRows(row, item2));
				}
				flag = false;
			}
		}
		return list;
	}

	private Hashtable CalculateIndexes(IEnumerable<IStiMeter> meters)
	{
		List<string> list = (from m in meters.Where((IStiMeter m) => m is IStiMeasureMeter).SelectMany((IStiMeter m) => StiExpressionHelper.GetArguments(m.Expression))
			where !string.IsNullOrWhiteSpace(m)
			select m.Trim().ToLowerInvariant()).ToList();
		Hashtable hashtable = new Hashtable();
		foreach (string measure in list)
		{
			DataColumn dataColumn = table1.Columns.Cast<DataColumn>().ToList().FirstOrDefault((DataColumn c) => c.ColumnName.ToLowerInvariant() == measure);
			if (dataColumn != null)
			{
				int num = table1.Columns.IndexOf(dataColumn);
				if (num != -1)
				{
					hashtable[num] = num;
				}
			}
		}
		return hashtable;
	}

	private bool IsNumericType(object value)
	{
		return value?.GetType().IsNumericType() ?? false;
	}

	private IEnumerable<DataRow> CrossJoinRows()
	{
		List<DataRow> source = table1.AsEnumerable().ToList();
		List<DataRow> rows2 = table2.AsEnumerable().ToList();
		return source.SelectMany((DataRow r1) => rows2.Select((DataRow r2) => SplitRows(r1, r2)));
	}

	private IEnumerable<DataRow> FullJoinRows(StiDataLink link)
	{
		int fieldIndex1 = GetFieldIndex(table1, link);
		int fieldIndex2 = GetFieldIndex(table2, link);
		List<DataRow> outer = table1.AsEnumerable().ToList();
		List<DataRow> inner = table2.AsEnumerable().ToList();
		return outer.FullOuterJoin(inner, (DataRow r1) => GetHashCode(r1, fieldIndex1), (DataRow r2) => GetHashCode(r2, fieldIndex2), SplitRows);
	}

	private int GetHashCode(DataRow row, int fieldIndex)
	{
		if (row[fieldIndex] == null)
		{
			return 0;
		}
		return row[fieldIndex].GetHashCode();
	}

	private DataRow SplitRows(DataRow row1, DataRow row2)
	{
		object[] array = new object[resultTable.Columns.Count];
		foreach (DataColumn column in resultTable.Columns)
		{
			int num = (resultColumnIndexes.ContainsKey(column.ColumnName) ? resultColumnIndexes[column.ColumnName] : (-1));
			if (num != -1)
			{
				int num2 = (column1Indexes.ContainsKey(column.ColumnName) ? column1Indexes[column.ColumnName] : (-1));
				int num3 = (column2Indexes.ContainsKey(column.ColumnName) ? column2Indexes[column.ColumnName] : (-1));
				if (num2 != -1 && row1 != null)
				{
					array[num] = row1[num2];
				}
				if (num3 != -1 && row2 != null)
				{
					array[num] = row2[num3];
				}
			}
		}
		return resultTable.LoadDataRow(array.ToArray(), fAcceptChanges: false);
	}

	private int GetFieldIndex(DataTable table, StiDataLink link)
	{
		DataColumn dataColumn = table.Columns.Cast<DataColumn>().FirstOrDefault((DataColumn c) => c.ColumnName == link.ParentKey || c.ColumnName == link.ChildKey || c.Table.TableName + "." + c.ColumnName == link.ParentKey || c.Table.TableName + "." + c.ColumnName == link.ChildKey);
		if (dataColumn == null)
		{
			return -1;
		}
		return table.Columns.IndexOf(dataColumn);
	}

	public StiDataRowJoiner(DataTable resultTable, DataTable table1, DataTable table2)
	{
		this.resultTable = resultTable;
		this.table1 = table1;
		this.table2 = table2;
		foreach (DataColumn column in resultTable.Columns)
		{
			resultColumnIndexes.Add(column.ColumnName, resultTable.Columns.IndexOf(column.ColumnName));
		}
		foreach (DataColumn column2 in table1.Columns)
		{
			column1Indexes.Add(column2.ColumnName, table1.Columns.IndexOf(column2.ColumnName));
		}
		foreach (DataColumn column3 in table2.Columns)
		{
			column2Indexes.Add(column3.ColumnName, table2.Columns.IndexOf(column3.ColumnName));
		}
	}
}
