using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiVirtualSourceConverter))]
public class StiVirtualSource : StiDataStoreSource, IStiFilter
{
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


	public override StiComponentId ComponentId => StiComponentId.StiVirtualSource;

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public string[] GroupColumns { get; set; } = new string[0];


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public string[] Results { get; set; } = new string[0];


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public virtual string[] Sort { get; set; } = new string[0];


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "FilterMode", (Enum)FilterMode, (Enum)StiFilterMode.And);
		JsonExtensions.AddPropertyJObject(val, "Filters", Filters.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringArray(val, "GroupColumns", GroupColumns);
		JsonExtensions.AddPropertyStringArray(val, "Results", Results);
		JsonExtensions.AddPropertyStringArray(val, "Sort", Sort);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "FilterMode":
				FilterMode = StiJsonReportObjectHelper.DeserializeEnum<StiFilterMode>(item);
				break;
			case "Filters":
				Filters.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GroupColumns":
				GroupColumns = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "Results":
				Results = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "Sort":
				Sort = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			}
		}
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiVirtualAdapterService);
	}

	internal void ConnectToData(bool allowConnect = false)
	{
		if (allowConnect)
		{
			StiDataSource stiDataSource = base.Dictionary.DataSources[NameInSource];
			if (stiDataSource.DataTable == null)
			{
				if (StiDataPicker.ExistsInCache((IStiAppDataSource)(object)stiDataSource))
				{
					stiDataSource.DataTable = StiDataPicker.GetFromCache((IStiAppDataSource)(object)stiDataSource)?.Copy();
				}
				else
				{
					stiDataSource.DataTable = StiDataPicker.GetDataTable((IStiApp)(object)base.Dictionary.Report, (IStiAppDataSource)(object)stiDataSource, (StiDataRequestOption)3)?.Copy();
				}
			}
		}
		ConnectToDataInternal();
	}

	private void ConnectToDataInternal()
	{
		object obj = null;
		string text = StiNameValidator.CorrectName(base.Name, base.Dictionary.Report);
		Type type = base.Dictionary.Report.GetType();
		MethodInfo method = type.GetMethod(text + "__GetFilter", new Type[2]
		{
			typeof(object),
			typeof(StiFilterEventArgs)
		});
		if (method != null)
		{
			obj = Delegate.CreateDelegate(typeof(StiFilterEventHandler), base.Dictionary.Report, text + "__GetFilter") as StiFilterEventHandler;
		}
		if ((obj == null && base.Dictionary.Report.CalculationMode == StiCalculationMode.Interpretation) || base.Dictionary.Report.IsDesigning)
		{
			StiDataBand stiDataBand = new StiDataBand
			{
				Name = "VirtualSourceBand",
				Page = base.Dictionary.Report.Pages[0],
				DataSourceName = NameInSource,
				Filters = Filters,
				FilterMode = FilterMode,
				FilterOn = FilterOn
			};
			obj = StiDataHelper.GetFilterEventHandler(stiDataBand, stiDataBand);
		}
		StiDataSource stiDataSource = base.Dictionary.DataSources[NameInSource];
		if (stiDataSource == null)
		{
			throw new ArgumentException("'" + base.Name + "' filter. Datasource '" + NameInSource + "' is not found.");
		}
		List<string> list = new List<string>();
		string[] groupColumns = GroupColumns;
		foreach (string text2 in groupColumns)
		{
			string text3 = text2;
			if (StringExt.StartsWithInvariant(text3, "DESC"))
			{
				if (stiDataSource.Columns[text3] != null && stiDataSource.Columns[text3.Substring(4)] == null)
				{
					list.Add("ASC");
				}
				else
				{
					list.Add("DESC");
					text3 = text3.Substring(4);
				}
			}
			else
			{
				if (StringExt.StartsWithInvariant(text3, "NONE"))
				{
					continue;
				}
				list.Add("ASC");
			}
			string[] array = text3.Split(new char[1] { '.' });
			int num = 0;
			string[] array2 = array;
			foreach (string text4 in array2)
			{
				string item = text4;
				if ((num == 0 && array.Length > 1) || num < array.Length - 1)
				{
					item = StiDataColumn.GetRelationName(base.Dictionary, stiDataSource, text4);
				}
				list.Add(item);
			}
		}
		string[] sort = Sort;
		foreach (string item2 in sort)
		{
			list.Add(item2);
		}
		string[] array3 = list.ToArray();
		if (array3.Length == 0)
		{
			array3 = null;
		}
		stiDataSource.ResetDetailsRows();
		stiDataSource.First();
		stiDataSource.SetSort(null, array3, null, null, null);
		stiDataSource.SetFilter(obj);
		DataTable dataTable = new DataTable();
		foreach (StiDataColumn column2 in Columns)
		{
			DataColumn column = new DataColumn(column2.Name, column2.Type);
			dataTable.Columns.Add(column);
		}
		bool flag = false;
		Hashtable hashtable = new Hashtable();
		List<StiAggregateFunctionService> list2 = new List<StiAggregateFunctionService>();
		string[] groupColumns2 = GroupColumns;
		foreach (string text5 in groupColumns2)
		{
			string text6 = text5;
			if (StringExt.StartsWithInvariant(text6, "DESC"))
			{
				if (stiDataSource.Columns[text6] == null || stiDataSource.Columns[text6.Substring(4)] != null)
				{
					text6 = text6.Substring(4);
				}
			}
			else if (StringExt.StartsWithInvariant(text6, "NONE"))
			{
				text6 = text6.Substring(4);
			}
			StiFirstFunctionService item3 = (StiFirstFunctionService)(hashtable[text6] = new StiFirstFunctionService());
			list2.Add(item3);
		}
		int num2 = 0;
		while (num2 < Results.Length)
		{
			string text7 = Results[num2++];
			string text8 = Results[num2++];
			string key = Results[num2++];
			switch (text8)
			{
			case "Sum":
			{
				StiAggregateFunctionService item21 = (StiAggregateFunctionService)(hashtable[key] = new StiSumDecimalFunctionService());
				list2.Add(item21);
				flag = true;
				break;
			}
			case "SumDistinct":
			{
				StiAggregateFunctionService item20 = (StiAggregateFunctionService)(hashtable[key] = new StiSumDistinctDecimalFunctionService());
				list2.Add(item20);
				flag = true;
				break;
			}
			case "Count":
			{
				StiAggregateFunctionService item19 = (StiAggregateFunctionService)(hashtable[key] = new StiCountFunctionService());
				list2.Add(item19);
				flag = true;
				break;
			}
			case "CountDistinct":
			{
				StiAggregateFunctionService item18 = (StiAggregateFunctionService)(hashtable[key] = new StiCountDistinctFunctionService());
				list2.Add(item18);
				flag = true;
				break;
			}
			case "Min":
			{
				StiAggregateFunctionService item17 = (StiAggregateFunctionService)(hashtable[key] = new StiMinDecimalFunctionService());
				list2.Add(item17);
				flag = true;
				break;
			}
			case "Max":
			{
				StiAggregateFunctionService item16 = (StiAggregateFunctionService)(hashtable[key] = new StiMaxDecimalFunctionService());
				list2.Add(item16);
				flag = true;
				break;
			}
			case "Avg":
			{
				StiAggregateFunctionService item15 = (StiAggregateFunctionService)(hashtable[key] = new StiAvgDecimalFunctionService());
				list2.Add(item15);
				flag = true;
				break;
			}
			case "First":
			{
				StiAggregateFunctionService item14 = (StiAggregateFunctionService)(hashtable[key] = new StiFirstFunctionService());
				list2.Add(item14);
				flag = true;
				break;
			}
			case "Last":
			{
				StiAggregateFunctionService item13 = (StiAggregateFunctionService)(hashtable[key] = new StiLastFunctionService());
				list2.Add(item13);
				flag = true;
				break;
			}
			case "MinDate":
			{
				StiAggregateFunctionService item12 = (StiAggregateFunctionService)(hashtable[key] = new StiMinDateFunctionService());
				list2.Add(item12);
				flag = true;
				break;
			}
			case "MaxDate":
			{
				StiAggregateFunctionService item11 = (StiAggregateFunctionService)(hashtable[key] = new StiMaxDateFunctionService());
				list2.Add(item11);
				flag = true;
				break;
			}
			case "MinTime":
			{
				StiAggregateFunctionService item10 = (StiAggregateFunctionService)(hashtable[key] = new StiMinTimeFunctionService());
				list2.Add(item10);
				flag = true;
				break;
			}
			case "MaxTime":
			{
				StiAggregateFunctionService item9 = (StiAggregateFunctionService)(hashtable[key] = new StiMaxTimeFunctionService());
				list2.Add(item9);
				flag = true;
				break;
			}
			case "MinStr":
			{
				StiAggregateFunctionService item8 = (StiAggregateFunctionService)(hashtable[key] = new StiMinStrFunctionService());
				list2.Add(item8);
				flag = true;
				break;
			}
			case "MaxStr":
			{
				StiAggregateFunctionService item7 = (StiAggregateFunctionService)(hashtable[key] = new StiMaxStrFunctionService());
				list2.Add(item7);
				flag = true;
				break;
			}
			case "Mode":
			{
				StiAggregateFunctionService item6 = (StiAggregateFunctionService)(hashtable[key] = new StiModeDecimalFunctionService());
				list2.Add(item6);
				flag = true;
				break;
			}
			case "Median":
			{
				StiAggregateFunctionService item5 = (StiAggregateFunctionService)(hashtable[key] = new StiMedianDecimalFunctionService());
				list2.Add(item5);
				flag = true;
				break;
			}
			default:
			{
				StiAggregateFunctionService item4 = (StiAggregateFunctionService)(hashtable[key] = new StiFirstFunctionService());
				list2.Add(item4);
				break;
			}
			}
		}
		bool flag2 = GroupColumns.Length != 0;
		object[] values = new object[GroupColumns.Length];
		bool flag3 = true;
		int num3 = 0;
		if (!flag2 && flag)
		{
			InitTotals(list2);
		}
		bool flag4 = !flag && !flag2;
		string[] array4 = new string[Results.Length];
		Results.CopyTo(array4, 0);
		for (int m = 0; m < array4.Length; m += 3)
		{
			string text9 = array4[m];
			array4[m] = StiNameValidator.CorrectName(text9);
			string[] array5 = text9.Split(new char[1] { '.' });
			if (array5.Length <= 1)
			{
				continue;
			}
			string text10 = array5[0];
			int num4 = 0;
			while (num4 < array5.Length - 1)
			{
				if (stiDataSource.GetParentRelations().Contains(text10))
				{
					string str = text9.Substring(text10.Length + 1);
					array4[m] = StiNameValidator.CorrectName(text10) + "." + StiNameValidator.CorrectName(str);
					break;
				}
				num4++;
				text10 = text10 + "." + array5[num4];
			}
		}
		stiDataSource.First();
		base.Dictionary.Report.Line = 1;
		base.Dictionary.Report.LineThrough = 1;
		while (!stiDataSource.IsEof)
		{
			if (flag2)
			{
				object[] array6 = new object[GroupColumns.Length];
				int num5 = 0;
				string[] groupColumns3 = GroupColumns;
				foreach (string text11 in groupColumns3)
				{
					string text12 = text11;
					if (StringExt.StartsWithInvariant(text12, "DESC"))
					{
						if (stiDataSource.Columns[text12] == null || stiDataSource.Columns[text12.Substring(4)] != null)
						{
							text12 = text12.Substring(4);
						}
					}
					else if (StringExt.StartsWithInvariant(text12, "NONE"))
					{
						text12 = text12.Substring(4);
					}
					array6[num5++] = StiDataColumn.GetDataFromDataColumn(base.Dictionary, StiNameValidator.CorrectName(stiDataSource.Name, base.Dictionary.Report) + "." + text12, useRelationName: false);
				}
				bool flag5 = false;
				if (flag3)
				{
					InitTotals(list2);
				}
				else
				{
					flag5 = Compare(values, array6);
				}
				if (flag5)
				{
					AddRow(dataTable, hashtable);
					InitTotals(list2);
					num3 = 0;
				}
				values = array6;
				flag3 = false;
			}
			DataRow dataRow = null;
			if (flag4)
			{
				dataRow = dataTable.NewRow();
			}
			try
			{
				string[] groupColumns4 = GroupColumns;
				foreach (string text13 in groupColumns4)
				{
					string text14 = text13;
					if (StringExt.StartsWithInvariant(text14, "DESC"))
					{
						if (stiDataSource.Columns[text14] == null || stiDataSource.Columns[text14.Substring(4)] != null)
						{
							text14 = text14.Substring(4);
						}
					}
					else if (StringExt.StartsWithInvariant(text14, "NONE"))
					{
						text14 = text14.Substring(4);
					}
					object obj2 = StiDataColumn.GetDataFromDataColumn(base.Dictionary, StiNameValidator.CorrectName(stiDataSource.Name, base.Dictionary.Report) + "." + text14, useRelationName: false);
					if (obj2 == null)
					{
						obj2 = DBNull.Value;
					}
					if (flag4)
					{
						dataRow[text13] = obj2;
					}
					else if (hashtable[text14] is StiAggregateFunctionService stiAggregateFunctionService)
					{
						stiAggregateFunctionService.CalcItem(obj2);
					}
				}
			}
			catch
			{
			}
			int num7 = 0;
			while (num7 < array4.Length)
			{
				try
				{
					string text15 = array4[num7++];
					string text16 = array4[num7++];
					string text17 = array4[num7++];
					object obj4 = DBNull.Value;
					if (!string.IsNullOrEmpty(text15))
					{
						obj4 = StiDataColumn.GetDataFromDataColumn(base.Dictionary, StiNameValidator.CorrectName(stiDataSource.Name, base.Dictionary.Report) + "." + text15, useRelationName: false);
						if (obj4 == null)
						{
							obj4 = DBNull.Value;
						}
					}
					if (flag4)
					{
						dataRow[text17] = obj4;
					}
					else if (hashtable[text17] is StiAggregateFunctionService stiAggregateFunctionService2)
					{
						stiAggregateFunctionService2.CalcItem(obj4);
					}
				}
				catch
				{
				}
			}
			num3++;
			if (flag4)
			{
				dataTable.Rows.Add(dataRow);
			}
			stiDataSource.Next();
			base.Dictionary.Report.Line++;
			base.Dictionary.Report.LineThrough++;
		}
		if (num3 > 0 && (flag || flag2))
		{
			AddRow(dataTable, hashtable);
		}
		base.DataTable = dataTable;
		stiDataSource.ResetDetailsRows();
		stiDataSource.ResetData();
		stiDataSource.First();
	}

	private bool Compare(object[] values1, object[] values2)
	{
		int num = 0;
		foreach (object obj in values1)
		{
			if (obj == null)
			{
				if (values2[num] != null)
				{
					return true;
				}
				num++;
			}
			else if (!obj.Equals(values2[num++]))
			{
				return true;
			}
		}
		return false;
	}

	private void InitTotals(List<StiAggregateFunctionService> totalsList)
	{
		foreach (StiAggregateFunctionService totals in totalsList)
		{
			totals.Init();
		}
	}

	private void AddRow(DataTable table, Hashtable totalsHash)
	{
		DataRow dataRow = table.NewRow();
		foreach (StiDataColumn column in Columns)
		{
			if (totalsHash[column.Name] is StiAggregateFunctionService stiAggregateFunctionService)
			{
				object value = stiAggregateFunctionService.GetValue();
				if (value == null)
				{
					value = DBNull.Value;
				}
				dataRow[column.Name] = value;
			}
		}
		table.Rows.Add(dataRow);
	}

	public override StiDataSource CreateNew()
	{
		return new StiVirtualSource();
	}

	public StiVirtualSource()
	{
	}

	public StiVirtualSource(string nameInSource, string name)
		: base(nameInSource, name)
	{
		ConnectionOrder = 0;
	}

	public StiVirtualSource(string nameInSource, string name, string key)
		: base(nameInSource, name, name, key)
	{
		ConnectionOrder = 0;
	}
}
