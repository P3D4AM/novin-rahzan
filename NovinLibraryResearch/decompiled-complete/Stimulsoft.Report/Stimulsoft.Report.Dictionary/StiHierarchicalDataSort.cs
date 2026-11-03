using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dictionary;

internal class StiHierarchicalDataSort : IComparer
{
	private StiDataSource dataSource;

	private string keyColumn;

	private string masterKeyColumn;

	private string parentValue;

	private string[] sortColumns;

	public int Compare(object x, object y)
	{
		try
		{
			if (x == y)
			{
				return 0;
			}
			DataRow dataRow = x as DataRow;
			DataRow dataRow2 = y as DataRow;
			object value = dataRow[keyColumn];
			object value2 = dataRow2[keyColumn];
			return StiDataSort.CompareValues(value, value2);
		}
		catch
		{
			return 0;
		}
	}

	private string GetParentValue(object value)
	{
		if (value == null)
		{
			return string.Empty;
		}
		if (value == DBNull.Value)
		{
			return string.Empty;
		}
		string text = value.ToString();
		if (text.Trim().Length == 0)
		{
			return string.Empty;
		}
		return text;
	}

	internal void Process(Hashtable rowToConditions)
	{
		if (dataSource.DetailRows != null)
		{
			Hashtable values = new Hashtable();
			CreateTree(values);
			dataSource.RowToLevel = new Hashtable();
			SetLevelAndSort(rowToConditions, values, parentValue, 0);
			ArrayList arrayList = new ArrayList();
			CreateRowList(values, arrayList, parentValue);
			dataSource.DetailRows = new DataRow[arrayList.Count];
			arrayList.CopyTo(dataSource.DetailRows);
		}
	}

	private void CreateTree(Hashtable values)
	{
		DataRow[] detailRows = dataSource.DetailRows;
		foreach (DataRow dataRow in detailRows)
		{
			object value = dataRow[masterKeyColumn];
			value = GetParentValue(value);
			ArrayList arrayList = values[value] as ArrayList;
			if (arrayList == null)
			{
				arrayList = (ArrayList)(values[value] = new ArrayList());
			}
			arrayList.Add(dataRow);
		}
	}

	private void SetLevelAndSort(Hashtable rowToConditions, Hashtable values, object masterKey, int level)
	{
		if (!(values[GetParentValue(masterKey)] is ArrayList arrayList))
		{
			return;
		}
		foreach (DataRow item2 in arrayList)
		{
			dataSource.RowToLevel[item2] = level;
			object masterKey2 = item2[keyColumn];
			SetLevelAndSort(rowToConditions.Clone() as Hashtable, values, masterKey2, level + 1);
		}
		StiDataSort stiDataSort = new StiDataSort(rowToConditions, null, sortColumns, dataSource);
		List<DataRow> list = new List<DataRow>();
		foreach (DataRow item3 in arrayList)
		{
			list.Add(item3);
		}
		list.Sort(stiDataSort);
		arrayList.Clear();
		foreach (DataRow item4 in list)
		{
			arrayList.Add(item4);
		}
		stiDataSort.Clear();
	}

	private void CreateRowList(Hashtable values, ArrayList rows, object masterKey)
	{
		if (!(values[GetParentValue(masterKey)] is ArrayList arrayList))
		{
			return;
		}
		foreach (DataRow item in arrayList)
		{
			rows.Add(item);
			object masterKey2 = item[keyColumn];
			CreateRowList(values, rows, masterKey2);
		}
	}

	public StiHierarchicalDataSort(StiDataSource dataSource, StiHierarchicalBand band, string[] sortColumns)
	{
		this.dataSource = dataSource;
		keyColumn = band.KeyDataColumn;
		masterKeyColumn = band.MasterKeyDataColumn;
		parentValue = band.ParentValue;
		this.sortColumns = sortColumns;
		if (keyColumn == null || keyColumn.Trim().Length == 0)
		{
			throw new ArgumentException("Property 'KeyDataColumn' of '" + band.Name + "'is not filled!");
		}
		if (masterKeyColumn == null || masterKeyColumn.Trim().Length == 0)
		{
			throw new ArgumentException("Property 'MasterKeyDataColumn' of '" + band.Name + "' is not filled!");
		}
		if (!dataSource.Columns.Contains(keyColumn))
		{
			throw new ArgumentException("Column '" + keyColumn + "' does not present in '" + dataSource.Name + "'!");
		}
		if (!dataSource.Columns.Contains(masterKeyColumn))
		{
			throw new ArgumentException("Column '" + masterKeyColumn + "' does not present in '" + dataSource.Name + "'!");
		}
		if (parentValue == null)
		{
			parentValue = string.Empty;
		}
	}
}
