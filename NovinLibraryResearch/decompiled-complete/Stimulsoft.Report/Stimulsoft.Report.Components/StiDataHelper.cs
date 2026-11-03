using System.Text;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public sealed class StiDataHelper
{
	public static void SetData(StiComponent component, bool reinit)
	{
		SetData(component, reinit, null);
	}

	public static void SetData(StiComponent component, bool reinit, StiComponent masterComponent)
	{
		if (component is IStiDataSource { DataSource: not null } stiDataSource)
		{
			IStiDataRelation stiDataRelation = component as IStiDataRelation;
			string relationName = null;
			if (stiDataRelation != null && stiDataRelation.DataRelation != null)
			{
				relationName = stiDataRelation.DataRelationName;
			}
			if (masterComponent == null && !(component is IStiMasterComponent { MasterComponent: not null }))
			{
				relationName = null;
			}
			object filterEventHandler = GetFilterEventHandler(component, stiDataSource);
			IStiSort stiSort = component as IStiSort;
			string[] sortColumns = null;
			if (stiSort != null && stiSort.Sort != null && stiSort.Sort.Length != 0)
			{
				sortColumns = stiSort.Sort;
			}
			stiDataSource.DataSource.SetData(component as StiDataBand, relationName, filterEventHandler, sortColumns, reinit, component);
		}
		if (component is IStiEnumerator stiEnumerator)
		{
			stiEnumerator.First();
		}
		StiBusinessObject stiBusinessObject2 = ((component is IStiBusinessObject stiBusinessObject) ? stiBusinessObject.BusinessObject : null);
		if (stiBusinessObject2 == null)
		{
			return;
		}
		StiDataBand stiDataBand = component as StiDataBand;
		if (stiDataBand != null && ((stiDataBand.Sort != null && stiDataBand.Sort.Length != 0) || (stiDataBand.FilterOn && stiDataBand.FilterMethodHandler != null) || (component.Report != null && component.Report.CalculationMode == StiCalculationMode.Interpretation && stiDataBand.FilterOn && stiDataBand.Filters.Count > 0) || stiDataBand is StiHierarchicalBand || NeedGroupSort(stiDataBand)))
		{
			if (stiDataBand.BusinessObject.OwnerBand != stiDataBand)
			{
				stiDataBand.BusinessObject.OwnerBand = stiDataBand;
			}
		}
		else if (stiDataBand != null)
		{
			stiDataBand.BusinessObject.OwnerBand = null;
		}
		stiBusinessObject2.SetDetails();
	}

	internal static bool NeedGroupSort(StiDataBand band)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		if (band == null || band.Report == null)
		{
			return false;
		}
		StiComponentsCollection stiComponentsCollection = (((int)band.Report.EngineVersion == 1) ? band.DataBandInfoV1.GroupHeaderComponents : band.DataBandInfoV2.GroupHeaders);
		if (stiComponentsCollection == null || stiComponentsCollection.Count == 0)
		{
			return false;
		}
		int num = 0;
		foreach (StiGroupHeaderBand item in stiComponentsCollection)
		{
			if (item.SortDirection != StiGroupSortDirection.None)
			{
				num++;
			}
		}
		return num > 0;
	}

	public static object GetFilterEventHandler(StiComponent component, object dataSource)
	{
		IStiFilter stiFilter = component as IStiFilter;
		object result = null;
		if (stiFilter != null && stiFilter.FilterMethodHandler != null)
		{
			result = stiFilter.FilterMethodHandler;
		}
		StiReport stiReport = null;
		if (component != null && component.Report != null)
		{
			stiReport = component.Report;
		}
		if (stiReport == null && dataSource != null && dataSource is StiBusinessObject && (dataSource as StiBusinessObject).Dictionary != null)
		{
			stiReport = (dataSource as StiBusinessObject).Dictionary.Report;
		}
		if (stiReport != null && (stiReport.CalculationMode == StiCalculationMode.Interpretation || stiReport.IsDesigning) && stiFilter != null && stiFilter.FilterOn && stiFilter.Filters.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder("{");
			for (int i = 0; i < stiFilter.Filters.Count; i++)
			{
				StiFilter stiFilter2 = stiFilter.Filters[i];
				stringBuilder.Append("(");
				if (dataSource is IStiDataSource)
				{
					stringBuilder.Append(GetFilterExpression(stiFilter2, StiNameValidator.CorrectName(((IStiDataSource)dataSource).DataSourceName, stiReport) + "." + stiFilter2.Column, stiReport));
				}
				else
				{
					stringBuilder.Append(GetFilterExpression(stiFilter2, stiFilter2.Column, stiReport));
				}
				stringBuilder.Append(")");
				if (i < stiFilter.Filters.Count - 1)
				{
					if (stiReport != null && stiReport.ScriptLanguage == StiReportLanguageType.VB)
					{
						stringBuilder.Append((stiFilter.FilterMode == StiFilterMode.And) ? " And " : " Or ");
					}
					else
					{
						stringBuilder.Append((stiFilter.FilterMode == StiFilterMode.And) ? " && " : " || ");
					}
				}
			}
			stringBuilder.Append("}");
			result = new StiParser.StiFilterParserData(component, stringBuilder.ToString());
			StiParser.StiParserException ex = StiParser.CheckExpression(stringBuilder.ToString(), component);
			if (ex != null)
			{
				StiLogService.Write("StiDataSource Filters ...ERROR");
				StiLogService.Write(ex);
				stiReport?.WriteToReportRenderingMessages(component.Name + ".Filters " + ex.Message);
			}
		}
		return result;
	}

	public static string GetFilterExpression(StiFilter filter, string fullColumnName, StiReport report)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (filter.Item == StiFilterItem.Expression)
		{
			string value = filter.Expression.Value;
			if (value != null)
			{
				if (value.StartsWith("{") && value.EndsWith("}"))
				{
					stringBuilder.Append(value.Substring(1, value.Length - 2));
				}
				else
				{
					stringBuilder.Append(value);
				}
			}
		}
		else if (filter.Item == StiFilterItem.Value)
		{
			if (report != null && report.ScriptLanguage == StiReportLanguageType.VB)
			{
				if (filter.Condition == StiFilterCondition.IsNull || filter.Condition == StiFilterCondition.IsNotNull)
				{
					int num = fullColumnName.LastIndexOf('.');
					string text;
					if (num < 0)
					{
						text = fullColumnName;
					}
					else
					{
						string text2 = fullColumnName.Substring(0, num);
						string text3 = fullColumnName.Substring(num + 1);
						text = text2 + "[\"" + text3 + "\"]";
					}
					StiFilterCondition condition = filter.Condition;
					if (condition == StiFilterCondition.IsNull)
					{
						stringBuilder.Append(text + " = null Or " + text + " = DBNull.Value");
					}
					else
					{
						stringBuilder.Append(text + " <> null And " + text + " <> DBNull.Value");
					}
				}
				else if (filter.DataType == StiFilterDataType.String)
				{
					switch (filter.Condition)
					{
					case StiFilterCondition.EqualTo:
						stringBuilder.Append(fullColumnName + ".ToString().ToLower() = \"" + report.ToString(filter.Value1).ToLower() + "\"");
						break;
					case StiFilterCondition.NotEqualTo:
						stringBuilder.Append(fullColumnName + ".ToString().ToLower() <> \"" + report.ToString(filter.Value1).ToLower() + "\"");
						break;
					case StiFilterCondition.Containing:
						stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().IndexOf(\"" + report.ToString(filter.Value1).ToLower() + "\") <> -1");
						break;
					case StiFilterCondition.NotContaining:
						stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().IndexOf(\"" + report.ToString(filter.Value1).ToLower() + "\") = -1");
						break;
					case StiFilterCondition.BeginningWith:
						stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().StartsWith(\"" + report.ToString(filter.Value1).ToLower() + "\")");
						break;
					case StiFilterCondition.EndingWith:
						stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().EndsWith(\"" + report.ToString(filter.Value1).ToLower() + "\")");
						break;
					}
				}
				else if (filter.DataType == StiFilterDataType.Numeric)
				{
					switch (filter.Condition)
					{
					case StiFilterCondition.EqualTo:
						stringBuilder.Append("(decimal)" + fullColumnName + " = (decimal)" + filter.Value1);
						break;
					case StiFilterCondition.NotEqualTo:
						stringBuilder.Append("(decimal)" + fullColumnName + " <> (decimal)" + filter.Value1);
						break;
					case StiFilterCondition.Between:
						stringBuilder.Append("(decimal)" + fullColumnName + " >= (decimal)" + filter.Value1 + " And (decimal)" + fullColumnName + " <= (decimal)" + filter.Value2);
						break;
					case StiFilterCondition.NotBetween:
						stringBuilder.Append("(decimal)" + fullColumnName + " < (decimal)" + filter.Value1 + " Or (decimal)" + fullColumnName + " > (decimal)" + filter.Value2);
						break;
					case StiFilterCondition.GreaterThan:
						stringBuilder.Append("(decimal)" + fullColumnName + " > (decimal)" + filter.Value1);
						break;
					case StiFilterCondition.GreaterThanOrEqualTo:
						stringBuilder.Append("(decimal)" + fullColumnName + " >= (decimal)" + filter.Value1);
						break;
					case StiFilterCondition.LessThan:
						stringBuilder.Append("(decimal)" + fullColumnName + " < (decimal)" + filter.Value1);
						break;
					case StiFilterCondition.LessThanOrEqualTo:
						stringBuilder.Append("(decimal)" + fullColumnName + " <= (decimal)" + filter.Value1);
						break;
					}
				}
				else if (filter.DataType == StiFilterDataType.DateTime)
				{
					string text4 = string.Empty;
					if (!string.IsNullOrEmpty(filter.Value1))
					{
						string[] array = filter.Value1.Split(new char[1] { '/' });
						text4 = "DateSerial(" + array[2] + "," + array[0] + "," + array[1] + ")";
					}
					string text5 = string.Empty;
					if (!string.IsNullOrEmpty(filter.Value2))
					{
						string[] array2 = filter.Value2.Split(new char[1] { '/' });
						text5 = "DateSerial(" + array2[2] + "," + array2[0] + "," + array2[1] + ")";
					}
					switch (filter.Condition)
					{
					case StiFilterCondition.EqualTo:
						stringBuilder.Append("(DateTime)" + fullColumnName + " = " + text4);
						break;
					case StiFilterCondition.NotEqualTo:
						stringBuilder.Append("(DateTime)" + fullColumnName + " <> " + text4);
						break;
					case StiFilterCondition.Between:
						stringBuilder.Append("(DateTime)" + fullColumnName + " >= " + text4 + " And (DateTime)" + fullColumnName + " <= " + text5);
						break;
					case StiFilterCondition.NotBetween:
						stringBuilder.Append("(DateTime)" + fullColumnName + " < " + text4 + " Or (DateTime)" + fullColumnName + " > " + text5);
						break;
					case StiFilterCondition.GreaterThan:
						stringBuilder.Append("(DateTime)" + fullColumnName + " > " + text4);
						break;
					case StiFilterCondition.GreaterThanOrEqualTo:
						stringBuilder.Append("(DateTime)" + fullColumnName + " >= " + text4);
						break;
					case StiFilterCondition.LessThan:
						stringBuilder.Append("(DateTime)" + fullColumnName + " < " + text4);
						break;
					case StiFilterCondition.LessThanOrEqualTo:
						stringBuilder.Append("(DateTime)" + fullColumnName + " <= " + text4);
						break;
					}
				}
				else if (filter.DataType == StiFilterDataType.Boolean)
				{
					switch (filter.Condition)
					{
					case StiFilterCondition.EqualTo:
						stringBuilder.Append("(bool)" + fullColumnName + " = " + filter.Value1);
						break;
					case StiFilterCondition.NotEqualTo:
						stringBuilder.Append("(bool)" + fullColumnName + " <> " + filter.Value1);
						break;
					}
				}
				else if (filter.DataType == StiFilterDataType.Expression)
				{
					switch (filter.Condition)
					{
					case StiFilterCondition.EqualTo:
						stringBuilder.Append(fullColumnName + " = " + filter.Value1);
						break;
					case StiFilterCondition.NotEqualTo:
						stringBuilder.Append(fullColumnName + " <> )" + filter.Value1);
						break;
					case StiFilterCondition.Between:
						stringBuilder.Append(fullColumnName + " >= " + filter.Value1 + " And " + fullColumnName + " <= " + filter.Value2);
						break;
					case StiFilterCondition.NotBetween:
						stringBuilder.Append(fullColumnName + " < " + filter.Value1 + " Or " + fullColumnName + " > " + filter.Value2);
						break;
					case StiFilterCondition.GreaterThan:
						stringBuilder.Append(fullColumnName + " > " + filter.Value1);
						break;
					case StiFilterCondition.GreaterThanOrEqualTo:
						stringBuilder.Append(fullColumnName + " >= " + filter.Value1);
						break;
					case StiFilterCondition.LessThan:
						stringBuilder.Append(fullColumnName + " < " + filter.Value1);
						break;
					case StiFilterCondition.LessThanOrEqualTo:
						stringBuilder.Append(fullColumnName + " <= " + filter.Value1);
						break;
					}
				}
			}
			else if (filter.Condition == StiFilterCondition.IsNull || filter.Condition == StiFilterCondition.IsNotNull)
			{
				int num2 = fullColumnName.LastIndexOf('.');
				string text6;
				if (num2 < 0)
				{
					text6 = fullColumnName;
				}
				else
				{
					string text7 = fullColumnName.Substring(0, num2);
					string text8 = fullColumnName.Substring(num2 + 1);
					text6 = text7 + "[\"" + text8 + "\"]";
				}
				StiFilterCondition condition2 = filter.Condition;
				if (condition2 == StiFilterCondition.IsNull)
				{
					stringBuilder.Append(text6 + " == null || " + text6 + " == DBNull.Value");
				}
				else
				{
					stringBuilder.Append(text6 + " != null && " + text6 + " != DBNull.Value");
				}
			}
			else if (filter.DataType == StiFilterDataType.String)
			{
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append(fullColumnName + ".ToString().ToLower() == \"" + report.ToString(filter.Value1).ToLower() + "\"");
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append(fullColumnName + ".ToString().ToLower() != \"" + report.ToString(filter.Value1).ToLower() + "\"");
					break;
				case StiFilterCondition.Containing:
					stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().IndexOf(\"" + report.ToString(filter.Value1).ToLower() + "\") != -1");
					break;
				case StiFilterCondition.NotContaining:
					stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().IndexOf(\"" + report.ToString(filter.Value1).ToLower() + "\") == -1");
					break;
				case StiFilterCondition.BeginningWith:
					stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().StartsWith(\"" + report.ToString(filter.Value1).ToLower() + "\")");
					break;
				case StiFilterCondition.EndingWith:
					stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().EndsWith(\"" + report.ToString(filter.Value1).ToLower() + "\")");
					break;
				}
			}
			else if (filter.DataType == StiFilterDataType.Numeric)
			{
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append("(decimal)" + fullColumnName + " == (decimal)" + filter.Value1);
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append("(decimal)" + fullColumnName + " != (decimal)" + filter.Value1);
					break;
				case StiFilterCondition.Between:
					stringBuilder.Append("(decimal)" + fullColumnName + " >= (decimal)" + filter.Value1 + " && (decimal)" + fullColumnName + " <= (decimal)" + filter.Value2);
					break;
				case StiFilterCondition.NotBetween:
					stringBuilder.Append("(decimal)" + fullColumnName + " < (decimal)" + filter.Value1 + " || (decimal)" + fullColumnName + " > (decimal)" + filter.Value2);
					break;
				case StiFilterCondition.GreaterThan:
					stringBuilder.Append("(decimal)" + fullColumnName + " > (decimal)" + filter.Value1);
					break;
				case StiFilterCondition.GreaterThanOrEqualTo:
					stringBuilder.Append("(decimal)" + fullColumnName + " >= (decimal)" + filter.Value1);
					break;
				case StiFilterCondition.LessThan:
					stringBuilder.Append("(decimal)" + fullColumnName + " < (decimal)" + filter.Value1);
					break;
				case StiFilterCondition.LessThanOrEqualTo:
					stringBuilder.Append("(decimal)" + fullColumnName + " <= (decimal)" + filter.Value1);
					break;
				}
			}
			else if (filter.DataType == StiFilterDataType.DateTime)
			{
				string text9 = string.Empty;
				if (!string.IsNullOrEmpty(filter.Value1))
				{
					string[] array3 = filter.Value1.Split(new char[1] { '/' });
					text9 = "DateSerial(" + array3[2] + "," + array3[0] + "," + array3[1] + ")";
				}
				string text10 = string.Empty;
				if (!string.IsNullOrEmpty(filter.Value2))
				{
					string[] array4 = filter.Value2.Split(new char[1] { '/' });
					text10 = "DateSerial(" + array4[2] + "," + array4[0] + "," + array4[1] + ")";
				}
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append("(DateTime)" + fullColumnName + " == " + text9);
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append("(DateTime)" + fullColumnName + " != " + text9);
					break;
				case StiFilterCondition.Between:
					stringBuilder.Append("(DateTime)" + fullColumnName + " >= " + text9 + " && (DateTime)" + fullColumnName + " <= " + text10);
					break;
				case StiFilterCondition.NotBetween:
					stringBuilder.Append("(DateTime)" + fullColumnName + " < " + text9 + " || (DateTime)" + fullColumnName + " > " + text10);
					break;
				case StiFilterCondition.GreaterThan:
					stringBuilder.Append("(DateTime)" + fullColumnName + " > " + text9);
					break;
				case StiFilterCondition.GreaterThanOrEqualTo:
					stringBuilder.Append("(DateTime)" + fullColumnName + " >= " + text9);
					break;
				case StiFilterCondition.LessThan:
					stringBuilder.Append("(DateTime)" + fullColumnName + " < " + text9);
					break;
				case StiFilterCondition.LessThanOrEqualTo:
					stringBuilder.Append("(DateTime)" + fullColumnName + " <= " + text9);
					break;
				}
			}
			else if (filter.DataType == StiFilterDataType.Boolean)
			{
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append("(bool)" + fullColumnName + " == " + filter.Value1);
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append("(bool)" + fullColumnName + " != " + filter.Value1);
					break;
				}
			}
			else if (filter.DataType == StiFilterDataType.Expression)
			{
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append(fullColumnName + " == " + filter.Value1);
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append(fullColumnName + " != " + filter.Value1);
					break;
				case StiFilterCondition.Between:
					stringBuilder.Append(fullColumnName + " >= " + filter.Value1 + " && " + fullColumnName + " <= " + filter.Value2);
					break;
				case StiFilterCondition.NotBetween:
					stringBuilder.Append(fullColumnName + " < " + filter.Value1 + " || " + fullColumnName + " > " + filter.Value2);
					break;
				case StiFilterCondition.GreaterThan:
					stringBuilder.Append(fullColumnName + " > " + filter.Value1);
					break;
				case StiFilterCondition.GreaterThanOrEqualTo:
					stringBuilder.Append(fullColumnName + " >= " + filter.Value1);
					break;
				case StiFilterCondition.LessThan:
					stringBuilder.Append(fullColumnName + " < " + filter.Value1);
					break;
				case StiFilterCondition.LessThanOrEqualTo:
					stringBuilder.Append(fullColumnName + " <= " + filter.Value1);
					break;
				case StiFilterCondition.Containing:
					stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().Contains(\"" + report.ToString(filter.Value1).ToLower() + "\")");
					break;
				case StiFilterCondition.NotContaining:
					stringBuilder.Append("!ToString(" + fullColumnName + ").ToLower().Contains(\"" + report.ToString(filter.Value1).ToLower() + "\")");
					break;
				case StiFilterCondition.BeginningWith:
					stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().StartsWith(\"" + report.ToString(filter.Value1).ToLower() + "\")");
					break;
				case StiFilterCondition.EndingWith:
					stringBuilder.Append("ToString(" + fullColumnName + ").ToLower().EndsWith(\"" + report.ToString(filter.Value1).ToLower() + "\")");
					break;
				}
			}
		}
		return stringBuilder.ToString();
	}
}
