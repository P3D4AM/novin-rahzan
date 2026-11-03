using System;
using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dictionary;

internal class StiHierarchicalBusinessObjectSort : IComparer
{
	private StiBusinessObject businessObject;

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
			object valueFromObject = StiBusinessObjectHelper.GetValueFromObject(x, keyColumn);
			object valueFromObject2 = StiBusinessObjectHelper.GetValueFromObject(y, keyColumn);
			return StiDataSort.CompareValues(valueFromObject, valueFromObject2);
		}
		catch
		{
			return 0;
		}
	}

	private string GetParentValue(object value)
	{
		if (value is Type)
		{
			value = null;
		}
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

	internal void Process()
	{
		Hashtable values = new Hashtable();
		CreateTree(values);
		businessObject.RowToLevel = new Hashtable();
		SetLevelAndSort(values, parentValue, 0);
		ArrayList arrayList = new ArrayList();
		CreateRowList(values, arrayList, parentValue);
		businessObject.enumerator = arrayList.GetEnumerator();
	}

	private void CreateTree(Hashtable values)
	{
		try
		{
			businessObject.enumerator.Reset();
		}
		catch
		{
		}
		while (businessObject.enumerator.MoveNext())
		{
			object valueFromObject = StiBusinessObjectHelper.GetValueFromObject(businessObject.enumerator.Current, masterKeyColumn);
			valueFromObject = GetParentValue(valueFromObject);
			ArrayList arrayList = values[valueFromObject] as ArrayList;
			if (arrayList == null)
			{
				arrayList = (ArrayList)(values[valueFromObject] = new ArrayList());
			}
			arrayList.Add(businessObject.enumerator.Current);
		}
	}

	private void SetLevelAndSort(Hashtable values, object masterKey, int level)
	{
		if (!(values[GetParentValue(masterKey)] is ArrayList arrayList))
		{
			return;
		}
		foreach (object item in arrayList)
		{
			businessObject.RowToLevel[item] = level;
			object valueFromObject = StiBusinessObjectHelper.GetValueFromObject(item, keyColumn);
			SetLevelAndSort(values, valueFromObject, level + 1);
		}
		StiBusinessObjectSort stiBusinessObjectSort = new StiBusinessObjectSort(businessObject.OwnerBand.Sort, null, null);
		arrayList.Sort(stiBusinessObjectSort);
		stiBusinessObjectSort.Clear();
	}

	private void CreateRowList(Hashtable values, ArrayList rows, object masterKey)
	{
		if (!(values[GetParentValue(masterKey)] is ArrayList arrayList))
		{
			return;
		}
		foreach (object item in arrayList)
		{
			rows.Add(item);
			object valueFromObject = StiBusinessObjectHelper.GetValueFromObject(item, keyColumn);
			CreateRowList(values, rows, valueFromObject);
		}
	}

	public StiHierarchicalBusinessObjectSort(StiBusinessObject businessObject, StiHierarchicalBand band, string[] sortColumns)
	{
		this.businessObject = businessObject;
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
			throw new ArgumentException("Property 'MasterKeyDataColumn' of '" + band.Name + "'is not filled!");
		}
		if (!businessObject.Columns.Contains(keyColumn))
		{
			throw new ArgumentException("Column '" + keyColumn + "' does not present in '" + businessObject.Name + "'");
		}
		if (!businessObject.Columns.Contains(masterKeyColumn))
		{
			throw new ArgumentException("Column '" + masterKeyColumn + "' does not present in '" + businessObject.Name + "'");
		}
		if (parentValue == null)
		{
			parentValue = string.Empty;
		}
	}
}
