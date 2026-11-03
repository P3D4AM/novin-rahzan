using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiCrossTabDataSourceConverter))]
public class StiCrossTabDataSource : StiDataStoreSource, IStiFilter
{
	private SortedDictionary<int, List<object>> sum;

	private List<string> summaryColumn;

	private Hashtable columns;

	private Hashtable summaryType;

	private StiCrossTab crossTab;

	[Browsable(false)]
	public StiFilterEventHandler FilterMethodHandler { get; set; }

	[Browsable(false)]
	public virtual bool FilterOn { get; set; } = true;


	[DefaultValue(StiFilterMode.And)]
	[StiSerializable]
	[Browsable(false)]
	public StiFilterMode FilterMode { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public virtual StiFiltersCollection Filters { get; set; } = new StiFiltersCollection();


	public override StiComponentId ComponentId => StiComponentId.StiCrossTabDataSource;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "FilterMode", (Enum)FilterMode, (Enum)StiFilterMode.And);
		JsonExtensions.AddPropertyJObject(val, "Filters", Filters.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "FilterMode"))
			{
				if (text == "Filters")
				{
					Filters.LoadFromJsonObject((JObject)item.Value);
				}
			}
			else
			{
				FilterMode = StiJsonReportObjectHelper.DeserializeEnum<StiFilterMode>(item);
			}
		}
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiCrossTabAdapterService);
	}

	public void ConnectToData(DataTable crossDataTable, StiCrossTab crossT)
	{
		if (crossT == null)
		{
			return;
		}
		DataTable dataTable = new DataTable();
		crossTab = crossT;
		columns = new Hashtable();
		summaryColumn = new List<string>();
		summaryType = new Hashtable();
		foreach (StiComponent component in crossTab.Components)
		{
			if (component is StiCrossCell || component is StiCrossTitle || component is StiCrossSummaryHeader)
			{
				StiCrossRow stiCrossRow = component as StiCrossRow;
				StiCrossColumn stiCrossColumn = component as StiCrossColumn;
				StiCrossSummary stiCrossSummary = component as StiCrossSummary;
				if (stiCrossRow != null)
				{
					columns.Add(stiCrossRow.Alias, stiCrossRow.Name);
					DataColumn column = new DataColumn(stiCrossRow.Alias);
					dataTable.Columns.Add(column);
				}
				else if (stiCrossColumn != null)
				{
					columns.Add(stiCrossColumn.Alias, stiCrossColumn.Name);
					DataColumn column2 = new DataColumn(stiCrossColumn.Alias);
					dataTable.Columns.Add(column2);
				}
				else if (stiCrossSummary != null)
				{
					columns.Add(stiCrossSummary.Alias, stiCrossSummary.Name);
					summaryColumn.Add(stiCrossSummary.Name);
					summaryType.Add(summaryType.Count, stiCrossSummary.Summary);
					StiDataColumn stiDataColumn = crossTab.DataSource.Columns[stiCrossSummary.Alias];
					DataColumn column3 = new DataColumn(stiCrossSummary.Alias, typeof(decimal));
					dataTable.Columns.Add(column3);
				}
			}
		}
		foreach (DataRow row in crossDataTable.Rows)
		{
			DataRow dataRow2 = dataTable.NewRow();
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				dataRow2[dataTable.Columns[i].Caption] = row[(string)columns[dataTable.Columns[i].Caption]];
			}
			dataTable.Rows.Add(dataRow2);
		}
		CalculationTable(dataTable);
	}

	public void BuildColumnsForDictionaryPanel(StiCrossTab cT)
	{
		Columns.Clear();
		if (cT == null || !(NameInSource != string.Empty))
		{
			return;
		}
		foreach (StiComponent component in cT.Components)
		{
			if (component is StiCrossCell || component is StiCrossTitle || component is StiCrossSummaryHeader)
			{
				StiCrossRow stiCrossRow = component as StiCrossRow;
				StiCrossColumn stiCrossColumn = component as StiCrossColumn;
				StiCrossSummary stiCrossSummary = component as StiCrossSummary;
				if (stiCrossRow != null)
				{
					StiDataColumn column = new StiDataColumn(stiCrossRow.Alias, FildType(cT.DataSource, cT.BusinessObject, stiCrossRow.Alias) ?? typeof(object));
					Columns.Add(column);
				}
				else if (stiCrossColumn != null)
				{
					StiDataColumn column2 = new StiDataColumn(stiCrossColumn.Alias, FildType(cT.DataSource, cT.BusinessObject, stiCrossColumn.Alias) ?? typeof(object));
					Columns.Add(column2);
				}
				else if (stiCrossSummary != null)
				{
					StiDataColumn stiDataColumn = new StiDataColumn(stiCrossSummary.Alias, FildType(cT.DataSource, cT.BusinessObject, stiCrossSummary.Alias) ?? typeof(string));
					stiDataColumn.Type = typeof(decimal);
					Columns.Add(stiDataColumn);
				}
			}
		}
	}

	private Type FildType(StiDataSource dataSource, StiBusinessObject businessObject, string colName)
	{
		Type type = null;
		if (dataSource != null)
		{
			type = dataSource.Columns[colName]?.Type;
		}
		if (businessObject != null)
		{
			type = businessObject.Columns[colName]?.Type;
		}
		if (type == null)
		{
			foreach (StiDataRelation relation in dataSource.Dictionary.Relations)
			{
				if (relation.ChildSource == dataSource && relation?.ParentSource.Columns[colName] != null)
				{
					type = relation?.ParentSource.Columns[colName].Type;
				}
			}
		}
		return type;
	}

	private void CalculationTable(DataTable table)
	{
		DataTable dataTable = table.Clone();
		sum = new SortedDictionary<int, List<object>>();
		int key = 0;
		int num = columns.Count - summaryColumn.Count;
		foreach (DataRow row in table.Rows)
		{
			bool flag = false;
			if (summaryColumn.Count > 0)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					bool flag2 = true;
					DataRow dataRow2 = dataTable.Rows[i];
					for (int j = 0; j < num; j++)
					{
						if (dataRow2[j].ToString() != row[j].ToString())
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						CalculationRows(dataRow2, row);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				dataTable.Rows.Add(CopyRow(row, dataTable, num, ref key));
			}
		}
		base.DataTable = CalculationSummaryColunms(dataTable);
	}

	private void CalculationRows(DataRow newRow, DataRow row)
	{
		int num = columns.Count - summaryColumn.Count;
		for (int i = num; i < columns.Count; i++)
		{
			int key = Convert.ToInt32(newRow[i]);
			sum[key].Add(row[i].ToString());
		}
	}

	private DataRow CopyRow(DataRow row, DataTable tempTable, int countColunmsNotSummary, ref int key)
	{
		DataRow dataRow = tempTable.NewRow();
		for (int i = 0; i < tempTable.Columns.Count; i++)
		{
			if (i >= countColunmsNotSummary)
			{
				dataRow[i] = key;
				sum.Add(key, new List<object>());
				sum[key].Add(row[i]);
				key++;
			}
			else
			{
				dataRow[i] = row[i];
			}
		}
		return dataRow;
	}

	private DataTable CalculationSummaryColunms(DataTable tempTable)
	{
		int num = columns.Count - summaryColumn.Count;
		foreach (DataRow row in tempTable.Rows)
		{
			int num2 = 0;
			for (int i = num; i < columns.Count; i++)
			{
				switch ((StiSummaryType)summaryType[num2])
				{
				case StiSummaryType.Sum:
					row[i] = Sum(Convert.ToInt32(row[i]));
					break;
				case StiSummaryType.Average:
					row[i] = Average(Convert.ToInt32(row[i]));
					break;
				case StiSummaryType.Min:
					row[i] = Min(Convert.ToInt32(row[i]));
					break;
				case StiSummaryType.Max:
					row[i] = Max(Convert.ToInt32(row[i]));
					break;
				case StiSummaryType.Count:
					row[i] = SummaryCount(Convert.ToInt32(row[i]));
					break;
				case StiSummaryType.CountDistinct:
					row[i] = CountDistinct(Convert.ToInt32(row[i]));
					break;
				case StiSummaryType.None:
					row[i] = DBNull.Value;
					break;
				case StiSummaryType.Image:
					row[i] = DBNull.Value;
					break;
				}
				num2++;
			}
		}
		return tempTable;
	}

	private decimal Sum(int nomer)
	{
		List<object> list = sum[nomer];
		decimal d = default(decimal);
		for (int i = 0; i < list.Count; i++)
		{
			d += Convert.ToDecimal(list[i]);
		}
		return Math.Round(d, 2);
	}

	private decimal Average(int nomer)
	{
		List<object> list = sum[nomer];
		if (list.Count == 0)
		{
			return 0m;
		}
		decimal num = default(decimal);
		for (int i = 0; i < list.Count; i++)
		{
			num += Convert.ToDecimal(list[i]);
		}
		return Math.Round(num / (decimal)list.Count, 2);
	}

	private decimal Min(int nomer)
	{
		List<object> list = sum[nomer];
		decimal num = default(decimal);
		if (list.Count > 0)
		{
			num = Convert.ToDecimal(list[0]);
			for (int i = 1; i < list.Count; i++)
			{
				if (num > Convert.ToDecimal(list[i]))
				{
					num = Convert.ToDecimal(list[i]);
				}
			}
			return Math.Round(num, 2);
		}
		return 0m;
	}

	private decimal Max(int nomer)
	{
		List<object> list = sum[nomer];
		decimal num = default(decimal);
		if (list.Count > 0)
		{
			num = Convert.ToDecimal(list[0]);
			for (int i = 1; i < list.Count; i++)
			{
				if (num < Convert.ToDecimal(list[i]))
				{
					num = Convert.ToDecimal(list[i]);
				}
			}
			return Math.Round(num, 2);
		}
		return 0m;
	}

	private decimal SummaryCount(int nomer)
	{
		return sum[nomer].Count;
	}

	private decimal CountDistinct(int nomer)
	{
		List<object> list = sum[nomer];
		List<object> list2 = new List<object>();
		for (int i = 0; i < list.Count; i++)
		{
			if (!list2.Contains(list[i]))
			{
				list2.Add(list[i]);
			}
		}
		return list2.Count;
	}

	public override StiDataSource CreateNew()
	{
		return new StiCrossTabDataSource();
	}

	public StiCrossTabDataSource()
	{
		ConnectionOrder = 0;
	}

	public StiCrossTabDataSource(string nameInSource, string name)
		: base(nameInSource, name)
	{
		ConnectionOrder = 0;
	}

	public StiCrossTabDataSource(string nameInSource, string name, string key)
		: base(nameInSource, name, name, key)
	{
		ConnectionOrder = 0;
	}
}
