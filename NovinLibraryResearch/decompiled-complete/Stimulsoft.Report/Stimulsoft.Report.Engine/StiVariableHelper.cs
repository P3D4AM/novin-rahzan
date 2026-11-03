using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Engine;

public class StiVariableHelper
{
	private static object lockObject = new object();

	private static Hashtable ReportToLabels = new Hashtable();

	public static bool FillItemsOfVariables(StiReport compiledReport)
	{
		bool modified = false;
		foreach (StiVariable variable in compiledReport.Dictionary.Variables)
		{
			if (FillItemsOfVariable(variable, compiledReport, ref modified))
			{
				break;
			}
		}
		return modified;
	}

	public static bool FillItemsOfVariable(StiVariable variable, StiReport compiledReport, ref bool modified)
	{
		if (variable.DialogInfo == null)
		{
			return false;
		}
		if (variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Items)
		{
			if (compiledReport.IsReportRenderingAfterSubmit)
			{
				return false;
			}
			List<StiDialogInfoItem> dialogInfoItems = variable.DialogInfo.GetDialogInfoItems(variable.Type);
			dialogInfoItems = variable.DialogInfo.OrderBy(dialogInfoItems);
			if (dialogInfoItems != null && dialogInfoItems.Count > 0)
			{
				if (!(compiledReport[variable.Name] is IStiList stiList))
				{
					return false;
				}
				if (stiList.Count > 0)
				{
					return true;
				}
				stiList.Clear();
				modified = true;
				foreach (StiDialogInfoItem item4 in dialogInfoItems)
				{
					try
					{
						stiList.AddElement(item4.KeyObject);
					}
					catch
					{
					}
				}
			}
		}
		else
		{
			if ((variable.DialogInfo.Keys != null && variable.DialogInfo.Keys.Length != 0) || (variable.DialogInfo.Values != null && variable.DialogInfo.Values.Length != 0) || (variable.DialogInfo.CheckedStates != null && variable.DialogInfo.CheckedStates.Length != 0))
			{
				return false;
			}
			object[] array = ((!string.IsNullOrEmpty(variable.DialogInfo.KeysColumn)) ? StiDataColumn.GetDatasFromDataColumn(compiledReport.Dictionary, variable.DialogInfo.KeysColumn, null, firstPositionInDataSource: false, useRelationName: false) : null);
			object[] array2 = ((!string.IsNullOrEmpty(variable.DialogInfo.ValuesColumn)) ? StiDataColumn.GetDatasFromDataColumn(compiledReport.Dictionary, variable.DialogInfo.ValuesColumn, null, firstPositionInDataSource: false, useRelationName: false) : null);
			bool[] array3 = ((!string.IsNullOrEmpty(variable.DialogInfo.CheckedColumn)) ? GetChecked(compiledReport.Dictionary, variable.DialogInfo.CheckedColumn) : null);
			object[] array4 = ((!string.IsNullOrEmpty(variable.DialogInfo.BindingValuesColumn)) ? StiDataColumn.GetDatasFromDataColumn(compiledReport.Dictionary, variable.DialogInfo.BindingValuesColumn, null, firstPositionInDataSource: false, useRelationName: false) : null);
			if (array == null)
			{
				array = new object[0];
			}
			if (array2 == null)
			{
				array2 = new object[0];
			}
			if (array3 == null)
			{
				array3 = new bool[0];
			}
			if (array4 == null)
			{
				array4 = new object[0];
			}
			int val = Math.Max(array.Length, array2.Length);
			val = Math.Max(val, array3.Length);
			Hashtable hashtable = new Hashtable();
			if (array.Length != 0)
			{
				List<object> list = new List<object>();
				List<object> list2 = new List<object>();
				List<object> list3 = new List<object>();
				List<bool> list4 = new List<bool>();
				for (int i = 0; i < val; i++)
				{
					object obj2 = ((i < array.Length) ? array[i] : null);
					string item = ((i < array2.Length) ? array2[i].ToString() : string.Empty);
					object obj3 = ((i < array2.Length) ? array2[i] : null);
					object item2 = ((i < array4.Length) ? array4[i] : null);
					bool item3 = array3 == null || i >= array3.Length || StiValueHelper.TryToBool((object)array3[i]);
					if (obj2 is DateTime dateTime)
					{
						switch (variable.DialogInfo.DateTimeType)
						{
						case StiDateTimeType.Date:
							obj2 = dateTime.Date;
							break;
						case StiDateTimeType.Time:
							obj2 = dateTime.ToShortTimeString();
							break;
						}
					}
					if (obj3 is DateTime)
					{
						DateTime dateTime2 = (DateTime)array2[i];
						switch (variable.DialogInfo.DateTimeType)
						{
						case StiDateTimeType.DateAndTime:
							item = dateTime2.ToString();
							break;
						case StiDateTimeType.Date:
							item = dateTime2.ToShortDateString();
							break;
						case StiDateTimeType.Time:
							item = dateTime2.ToShortTimeString();
							break;
						}
					}
					if (!hashtable.Contains(obj2))
					{
						list.Add(obj2);
						list2.Add(item);
						list3.Add(item2);
						list4.Add(item3);
						hashtable[obj2] = new List<object> { item2 };
					}
					else
					{
						List<object> list5 = hashtable[obj2] as List<object>;
						list5.Add(item2);
					}
				}
				array = list.ToArray();
				array2 = list2.ToArray();
				array4 = list3.ToArray();
				array3 = list4.ToArray();
				val = Math.Max(array.Length, array2.Length);
				val = Math.Max(val, array3.Length);
			}
			List<StiDialogInfoItem> list6 = new List<StiDialogInfoItem>();
			Type type = variable.Type;
			for (int j = 0; j < val; j++)
			{
				object obj4 = ((array.Length <= j) ? null : array[j]);
				string value = ((array2.Length <= j) ? string.Empty : array2[j].ToString());
				bool @checked = array3 == null || j >= array3.Length || array3[j];
				StiDialogInfoItem stiDialogInfoItem = null;
				if (type == typeof(ByteList) || type == typeof(ShortList) || type == typeof(IntList) || type == typeof(LongList) || type == typeof(byte) || type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(byte?) || type == typeof(short?) || type == typeof(int?) || type == typeof(long?))
				{
					stiDialogInfoItem = new StiLongDialogInfoItem();
				}
				else if (type == typeof(StringList) || type == typeof(string))
				{
					stiDialogInfoItem = new StiStringDialogInfoItem();
				}
				else if (type == typeof(DoubleList) || type == typeof(FloatList) || type == typeof(double) || type == typeof(float) || type == typeof(double?) || type == typeof(float?))
				{
					stiDialogInfoItem = new StiDoubleDialogInfoItem();
				}
				else if (type == typeof(DecimalList) || type == typeof(decimal) || type == typeof(decimal?))
				{
					stiDialogInfoItem = new StiDecimalDialogInfoItem();
				}
				else if (type == typeof(DateTimeList) || type == typeof(DateTime) || type == typeof(DateTime?))
				{
					stiDialogInfoItem = new StiDateTimeDialogInfoItem();
				}
				else if (type == typeof(TimeSpanList) || type == typeof(TimeSpan) || type == typeof(TimeSpan?))
				{
					stiDialogInfoItem = new StiTimeSpanDialogInfoItem();
				}
				else if (type == typeof(BoolList) || type == typeof(bool) || type == typeof(bool?))
				{
					stiDialogInfoItem = new StiBoolDialogInfoItem();
				}
				else if (type == typeof(CharList) || type == typeof(char) || type == typeof(char?))
				{
					stiDialogInfoItem = new StiCharDialogInfoItem();
				}
				else if (type == typeof(GuidList) || type == typeof(Guid))
				{
					stiDialogInfoItem = new StiGuidDialogInfoItem();
				}
				else if (type == typeof(Image))
				{
					stiDialogInfoItem = new StiImageDialogInfoItem();
				}
				if (obj4 != null)
				{
					stiDialogInfoItem.KeyObject = obj4;
				}
				stiDialogInfoItem.Value = value;
				stiDialogInfoItem.ValueBinding = hashtable[obj4] as List<object>;
				stiDialogInfoItem.Checked = @checked;
				list6.Add(stiDialogInfoItem);
			}
			List<StiDialogInfoItem> list7 = new List<StiDialogInfoItem>();
			Hashtable hashtable2 = new Hashtable();
			foreach (StiDialogInfoItem item5 in list6)
			{
				if (hashtable2[item5.KeyObject] == null || item5 is StiRangeDialogInfoItem)
				{
					hashtable2[item5.KeyObject] = item5.KeyObject;
					list7.Add(item5);
				}
			}
			List<StiDialogInfoItem> items = (variable.DialogInfo.BindingValue ? list6 : list7);
			items = variable.DialogInfo.OrderBy(items);
			variable.DialogInfo.SetDialogInfoItems(items, variable.Type);
			array = items.Select((StiDialogInfoItem r) => r.KeyObject).ToArray();
			if (StiTypeFinder.FindInterface(variable.Type, typeof(IStiList)) && array != null && array.Length != 0)
			{
				FillVariableList(variable, compiledReport, array);
				modified = true;
			}
		}
		return false;
	}

	private static bool[] GetChecked(StiDictionary dictionary, string dataColumn)
	{
		if (string.IsNullOrEmpty(dataColumn))
		{
			return new bool[0];
		}
		int num = dataColumn.IndexOf(";");
		if (num == -1)
		{
			return new bool[0];
		}
		string name = dataColumn.Substring(0, num);
		string expression = "{" + dataColumn.Substring(num + 1) + "}";
		StiDataSource stiDataSource = dictionary.DataSources[name];
		if (stiDataSource == null)
		{
			return new bool[0];
		}
		return StiDataColumn.GetDatasFromDataSourceWithExpression(stiDataSource, expression, null, firstPositionInDataSource: true)?.Select((object c) => StiValueHelper.TryToBool(c)).ToArray();
	}

	private static void FillVariableList(StiVariable variable, StiReport compiledReport, object[] keys)
	{
		IStiList stiList = compiledReport[variable.Name] as IStiList;
		if (stiList == null)
		{
			Type typeFromTypeMode = StiType.GetTypeFromTypeMode(variable.Type, StiTypeMode.List);
			stiList = StiActivator.CreateObject(typeFromTypeMode) as IStiList;
		}
		stiList.Clear();
		foreach (object value in keys)
		{
			try
			{
				stiList.AddElement(value);
			}
			catch
			{
			}
		}
	}

	public static void SetDefaultValueForRequestFromUserVariables(StiReport report, bool haveVars, bool allowParseQuery = false)
	{
		if (report == null)
		{
			return;
		}
		StiText stiText = new StiText
		{
			Name = "**VariableRequestFromUser**"
		};
		if (report.Pages.Count > 0)
		{
			stiText.Page = report.Pages[0];
		}
		InitDateTimeVariables(report);
		if (!report.IsReportRenderingAfterSubmit)
		{
			foreach (StiVariable variable in report.Dictionary.Variables)
			{
				if (variable.RequestFromUser && typeof(IStiList).IsAssignableFrom(variable.Type) && !report.ModifiedVariables.ContainsKey(variable.Name) && variable.DialogInfo.Values != null)
				{
					string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
					string label = string.Join(listSeparator, variable.DialogInfo.Values.ToArray());
					SetVariableLabel(report, variable, label);
				}
			}
		}
		foreach (StiVariable variable2 in report.Dictionary.Variables)
		{
			if (!variable2.RequestFromUser || typeof(IStiList).IsAssignableFrom(variable2.Type) || variable2.Type.IsSubclassOf(typeof(Range)) || report.ModifiedVariables.ContainsKey(variable2.Name))
			{
				continue;
			}
			try
			{
				if (variable2.Selection == StiSelectionMode.FromVariable && variable2.InitBy == StiVariableInitBy.Expression)
				{
					object obj = StiParser.ParseTextValue("{" + variable2.Value + "}", stiText);
					if (variable2.DialogInfo.DateTimeType == StiDateTimeType.Date && obj is DateTime dateTime)
					{
						obj = dateTime.Date;
					}
					if (!report.IsReportRenderingAfterSubmit)
					{
						int num = ((obj != null && variable2.DialogInfo.Keys != null) ? variable2.DialogInfo.Keys.ToList().IndexOf(obj?.ToString()) : (-1));
						if (num != -1)
						{
							SetVariableLabel(report, variable2, variable2.DialogInfo.Values.ElementAtOrDefault(num));
						}
					}
					report[variable2.Name] = obj;
					haveVars = true;
				}
				if (variable2.Selection == StiSelectionMode.FromVariable && variable2.InitBy == StiVariableInitBy.Value && !report.IsReportRenderingAfterSubmit)
				{
					int num2 = ((variable2.ValueObject != null && variable2.DialogInfo.Keys != null) ? variable2.DialogInfo.Keys.ToList().IndexOf(variable2.ValueObject?.ToString()) : (-1));
					if (num2 != -1)
					{
						SetVariableLabel(report, variable2, variable2.DialogInfo.Values.ElementAtOrDefault(num2));
					}
				}
				if (variable2.Selection == StiSelectionMode.First && variable2.DialogInfo.Keys != null && variable2.DialogInfo.Keys.Length != 0)
				{
					object value = StiReport.ChangeType(variable2.DialogInfo.Keys.FirstOrDefault(), variable2.Type);
					report[variable2.Name] = value;
					haveVars = true;
					if (!report.IsReportRenderingAfterSubmit)
					{
						SetVariableLabel(report, variable2, variable2.DialogInfo.Values.FirstOrDefault());
					}
				}
			}
			catch
			{
			}
		}
		if (!haveVars)
		{
			return;
		}
		StiDictionary dictionary = report.Dictionary;
		string[] array = dictionary.ReconnectListForRequestFromUserVariables;
		if (array == null)
		{
			array = GetDataSourcesWithRequestFromUserVariablesInCommand(report).ToArray();
		}
		if (array.Length == 0)
		{
			return;
		}
		string[] array2 = array;
		foreach (string name in array2)
		{
			StiDataSource stiDataSource = dictionary.DataSources[name];
			object obj3 = ((dictionary != null) ? ((object)dictionary) : ((object)stiDataSource));
			lock (obj3)
			{
				StiSqlSource stiSqlSource = stiDataSource as StiSqlSource;
				string text = stiSqlSource?.SqlCommand;
				try
				{
					if (allowParseQuery)
					{
						StiDataSourceParserHelper.ConnectSqlSource(stiSqlSource);
					}
					StiDataLeader.Disconnect(stiDataSource);
					StiDataLeader.Connect(stiDataSource);
				}
				finally
				{
					if (text != null)
					{
						stiSqlSource.SqlCommand = text;
					}
				}
			}
		}
		dictionary.RegRelations();
		dictionary.RegRelations(virtualSources: true);
	}

	private static void InitDateTimeVariables(StiReport report)
	{
		if (report.IsReportRenderingAfterSubmit)
		{
			return;
		}
		List<StiVariable> list = (from v in report.Dictionary.Variables.ToList()
			where v.DialogInfo.DateTimeType == StiDateTimeType.Date && v.RequestFromUser && v.InitBy == StiVariableInitBy.Value && (v.Type == typeof(DateTimeRange) || v.Type == typeof(DateTime))
			select v).ToList();
		list.ForEach(delegate(StiVariable v)
		{
			if (v.Type == typeof(DateTime))
			{
				report[v.Name] = StiVariable.GetDateTimeFromValue(v.GetNativeValue()).Date;
			}
			else if (v.ValueObject is DateTimeRange dateTimeRange)
			{
				report[v.Name] = new DateTimeRange(dateTimeRange.FromDate.Date, (dateTimeRange.ToDate != DateTime.MaxValue) ? dateTimeRange.ToDate.Date.AddDays(1.0).AddTicks(-1L) : dateTimeRange.ToDate.Date);
			}
		});
	}

	internal static List<string> GetDataSourcesWithRequestFromUserVariablesInCommand(StiReport report)
	{
		List<string> list = new List<string>();
		Hashtable hashtable = new Hashtable();
		StiText component = new StiText
		{
			Name = "**VariableRequestFromUser**",
			Page = report.Pages[0]
		};
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (variable.RequestFromUser)
			{
				hashtable[variable.Name] = null;
			}
		}
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			if (!(dataSource is StiSqlSource stiSqlSource))
			{
				continue;
			}
			try
			{
				string text = null;
				if (report.CalculationMode == StiCalculationMode.Interpretation && report.Variables != null)
				{
					object obj = report.Variables["**StoredDataSourceSqlCommandForInterpretationMode**" + dataSource.Name];
					if (obj != null && obj is string)
					{
						text = obj as string;
					}
				}
				if (string.IsNullOrWhiteSpace(text))
				{
					text = stiSqlSource.SqlCommand;
				}
				bool flag = CheckExpressionForVariables(text, component, hashtable);
				if (!flag)
				{
					foreach (StiDataParameter parameter in stiSqlSource.Parameters)
					{
						if (!string.IsNullOrWhiteSpace(parameter.Expression) && CheckExpressionForVariables(parameter.Expression, component, hashtable))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					list.Add(dataSource.Name);
				}
			}
			catch
			{
			}
		}
		return list;
	}

	private static bool CheckExpressionForVariables(string expression, StiComponent component, Hashtable vars)
	{
		try
		{
			bool storeToPrint = false;
			object obj = StiParser.ParseTextValue(expression, component, ref storeToPrint, executeIfStoreToPrint: false, returnAsmList: true);
			if (obj != null && obj is List<StiParser.StiAsmCommand>)
			{
				foreach (StiParser.StiAsmCommand item in obj as List<StiParser.StiAsmCommand>)
				{
					if (item.Type == StiParser.StiAsmCommandType.PushVariable)
					{
						string key = item.Parameter1.ToString();
						if (vars.ContainsKey(key))
						{
							return true;
						}
					}
				}
			}
		}
		catch
		{
		}
		return false;
	}

	public static bool ApplyVariableValue(StiReport report, StiVariable variable, object variableValue)
	{
		if (!StiOptions.Engine.ReconnectDataSourcesIfRequestFromUserVariableChanged)
		{
			return false;
		}
		StiComponent comp = null;
		List<StiDataSource> dataSourcesToReconnectList = GetDataSourcesToReconnectList(report, variable, ref comp);
		if (dataSourcesToReconnectList.Count == 0)
		{
			return false;
		}
		SetVariableValue(report, variable, variableValue);
		report.Dictionary.ConnectToDatabases(loadData: false);
		foreach (StiDataSource item in dataSourcesToReconnectList)
		{
			StiDataLeader.Disconnect(item);
			StiDataLeader.Connect(item);
		}
		report.Dictionary.DataStore.ClearReportDatabase();
		return true;
	}

	public static void SetVariableValue(StiReport report, StiVariable variable, object variableValue)
	{
		FieldInfo field = report.GetType().GetField(variable.Name);
		if (field != null)
		{
			field.SetValue(report, variableValue);
		}
		else
		{
			report[variable.Name] = variableValue;
		}
	}

	public static void RefreshDialogInfo(StiReport report, StiVariable var)
	{
		if (StiOptions.Engine.ReconnectDataSourcesIfRequestFromUserVariableChanged)
		{
			var.DialogInfo.Keys = null;
			var.DialogInfo.Values = null;
			var.DialogInfo.CheckedStates = null;
			bool modified = false;
			FillItemsOfVariable(var, report, ref modified);
		}
	}

	public static List<string> GetRelatedVariablesList(StiReport report, StiVariable var)
	{
		List<string> list = new List<string>();
		Hashtable hashtable = new Hashtable();
		StiComponent comp = null;
		List<StiDataSource> dataSourcesToReconnectList = GetDataSourcesToReconnectList(report, var, ref comp);
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (!variable.RequestFromUser || variable.DialogInfo == null)
			{
				continue;
			}
			hashtable.Clear();
			if (variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns)
			{
				StiDataSourceHelper.CheckExpression("{" + variable.DialogInfo.KeysColumn + "}", comp, hashtable);
			}
			if (variable.InitBy == StiVariableInitBy.Expression)
			{
				StiDataSourceHelper.CheckExpression("{" + variable.Value + "}", comp, hashtable);
				if (variable.Type == typeof(Range))
				{
					StiDataSourceHelper.CheckExpression("{" + variable.InitByExpressionFrom + "}", comp, hashtable);
					StiDataSourceHelper.CheckExpression("{" + variable.InitByExpressionTo + "}", comp, hashtable);
				}
			}
			foreach (StiDataSource item in dataSourcesToReconnectList)
			{
				if (hashtable.ContainsKey(item.Name))
				{
					list.Add(variable.Name);
					break;
				}
			}
		}
		return list;
	}

	private static List<StiDataSource> GetDataSourcesToReconnectList(StiReport report, StiVariable var, ref StiComponent comp)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[var.Name] = null;
		List<StiDataSource> list = new List<StiDataSource>();
		comp = new StiText
		{
			Name = "**DataSourceParameter**",
			Page = report.Pages[0]
		};
		Hashtable dataSourcesUsedInRequestFromUsersVariables = StiDataSourceHelper.GetDataSourcesUsedInRequestFromUsersVariables(report);
		foreach (string key in dataSourcesUsedInRequestFromUsersVariables.Keys)
		{
			StiDataSource stiDataSource = report.Dictionary.DataSources[key];
			foreach (StiDataParameter parameter in stiDataSource.Parameters)
			{
				if (CheckExpressionForVariables("{" + parameter.Expression + "}", comp, hashtable))
				{
					list.Add(stiDataSource);
					break;
				}
			}
		}
		return list;
	}

	public static void SetVariableLabel(StiReport report, StiVariable variable, string label)
	{
		if (report == null || label == null)
		{
			return;
		}
		lock (lockObject)
		{
			string text = variable?.Name;
			if (!string.IsNullOrWhiteSpace(text))
			{
				report.Key = StiKeyHelper.GetOrGeneratedKey(report.Key);
				Hashtable hashtable = ReportToLabels[report.Key] as Hashtable;
				if (hashtable == null)
				{
					hashtable = new Hashtable();
					ReportToLabels[report.Key] = hashtable;
				}
				hashtable[text] = label;
			}
		}
	}

	public static string GetVariableLabel(StiReport report, string variableName)
	{
		if (report == null)
		{
			return string.Empty;
		}
		if (string.IsNullOrWhiteSpace(variableName))
		{
			return string.Empty;
		}
		lock (lockObject)
		{
			report.Key = StiKeyHelper.GetOrGeneratedKey(report.Key);
			if (!(ReportToLabels[report.Key] is Hashtable hashtable))
			{
				return string.Empty;
			}
			if (!(hashtable[variableName] is string result))
			{
				return string.Empty;
			}
			return result;
		}
	}
}
