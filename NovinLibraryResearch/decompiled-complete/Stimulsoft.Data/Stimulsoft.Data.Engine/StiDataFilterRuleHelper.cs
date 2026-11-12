using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Data.Engine;

public class StiDataFilterRuleHelper
{
	public static List<StiDataFilterRule> ToList(params StiDataFilterRule[] rules)
	{
		return rules.ToList();
	}

	internal static List<StiDataFilterRule> Validate(List<StiDataFilterRule> rules, List<string> columnKeys)
	{
		return rules.Where((StiDataFilterRule r) => StiKeyHelper.IsKey(r.Key) && columnKeys.Contains(r.Key)).ToList();
	}

	internal static string GetDataTableFilterQuery(List<StiDataFilterRule> rules, IEnumerable<IStiAppDataColumn> columns, IStiReport report)
	{
		return GetDataTableFilterQuery(rules, columns.Select((IStiAppDataColumn c) => c?.GetName()).ToList(), columns.Select((IStiAppDataColumn c) => c?.GetDataType()).ToList(), report);
	}

	internal static StiTableFiltersGroupsType GetTableFiltersGroupsType(List<StiDataFilterRule> rules)
	{
		if (rules != null && rules.Count > 1)
		{
			IEnumerable<IGrouping<string, StiDataFilterRule>> source = rules.Where((StiDataFilterRule r) => !string.IsNullOrWhiteSpace(r.Path)).GroupBy(GetFullPath);
			if (source.Count() > 1)
			{
				if (!source.Any((IGrouping<string, StiDataFilterRule> g) => g.Count() > 1))
				{
					return StiTableFiltersGroupsType.Simple;
				}
				return StiTableFiltersGroupsType.Complex;
			}
		}
		return StiTableFiltersGroupsType.None;
	}

	internal static string GetDataTableFilterQuery(List<StiDataFilterRule> rules, List<string> columnNames, List<Type> columnTypes, IStiReport report)
	{
		if (rules == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (rules.Any((StiDataFilterRule r) => r.Condition == StiDataFilterCondition.IsFalse))
		{
			rules = rules.FirstOrDefault((StiDataFilterRule r) => r.Condition == StiDataFilterCondition.IsFalse).ToList();
		}
		IEnumerable<IGrouping<string, StiDataFilterRule>> enumerable = (from r in rules
			where r.IsEnabled && !string.IsNullOrWhiteSpace(r.Path)
			orderby columnNames?.FindIndex((string k) => k == r.Path)
			select r).GroupBy(GetFullPath);
		string value = ((enumerable.Count() < 2 || rules.Any((StiDataFilterRule r) => r.Operation == StiDataFilterOperation.AND)) ? " AND " : " OR ");
		foreach (IGrouping<string, StiDataFilterRule> group in enumerable)
		{
			IEnumerable<StiDataFilterRule> enumerable2 = rules.Where((StiDataFilterRule r) => r.IsEnabled && GetFullPath(r) == group.Key);
			StringBuilder filterGroupQuery = GetFilterGroupQuery(enumerable2, columnNames, columnTypes, report);
			if (filterGroupQuery.Length > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder = stringBuilder.Append(value);
				}
				if (enumerable2.Count() > 1 && enumerable.Count() > 1)
				{
					stringBuilder = stringBuilder.Append("(");
				}
				stringBuilder = stringBuilder.Append((object?)filterGroupQuery);
				if (enumerable2.Count() > 1 && enumerable.Count() > 1)
				{
					stringBuilder = stringBuilder.Append(")");
				}
			}
		}
		return stringBuilder.ToString();
	}

	private static string GetFullPath(StiDataFilterRule rule)
	{
		if (rule.ElementKey == null)
		{
			return rule.Path;
		}
		return rule.Path + rule.ElementKey;
	}

	private static StringBuilder GetFilterGroupQuery(IEnumerable<StiDataFilterRule> groupRules, List<string> columnNames, List<Type> columnTypes, IStiReport report)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		StringBuilder stringBuilder4 = new StringBuilder();
		string text = null;
		foreach (StiDataFilterRule groupRule in groupRules)
		{
			if (groupRule.Condition == StiDataFilterCondition.IsNull || groupRule.Condition == StiDataFilterCondition.IsNotNull || groupRule.Condition == StiDataFilterCondition.IsBlankOrNull)
			{
				text = string.Empty;
			}
			int columnIndex = GetColumnIndex(columnNames, groupRule.Path);
			string columnName = GetColumnName(columnNames, columnIndex, groupRule.Path);
			Type columnType = GetColumnType(columnTypes, columnIndex);
			string text2 = (groupRule.IsExpression ? GetValue(columnType, StiExpressionHelper.ParseReportExpression(report, groupRule.Value, withBraces: false, allowReturnNull: true)) : GetValue(columnType, groupRule.Value));
			bool flag = columnType.IsNumericType() || columnType.IsStringType() || columnType.IsBooleanType();
			if (flag && groupRule.Condition == StiDataFilterCondition.EqualTo)
			{
				string queryValue = GetQueryValue(text2, columnType, stringQuotes: true);
				stringBuilder3.Append((stringBuilder3.Length == 0) ? (columnName + " IN (") : ",");
				stringBuilder3.Append(queryValue);
				continue;
			}
			if (flag && groupRule.Condition == StiDataFilterCondition.NotEqualTo)
			{
				string queryValue2 = GetQueryValue(text2, columnType, stringQuotes: true);
				stringBuilder4.Append((stringBuilder4.Length == 0) ? (columnName + " NOT IN (") : ",");
				stringBuilder4.Append(queryValue2);
				if (text == null)
				{
					text = columnName + " IS NULL";
				}
				continue;
			}
			string value = text2;
			string columnName2 = columnName;
			Type type = columnType;
			if (groupRule.Condition == StiDataFilterCondition.Between || groupRule.Condition == StiDataFilterCondition.NotBetween || groupRule.Condition == StiDataFilterCondition.PairEqualTo)
			{
				value = (groupRule.IsExpression ? GetValue(type, StiExpressionHelper.ParseReportExpression(report, groupRule.Value2, withBraces: false, allowReturnNull: true)) : GetValue(type, groupRule.Value2));
				if (groupRule.Condition == StiDataFilterCondition.PairEqualTo)
				{
					int columnIndex2 = GetColumnIndex(columnNames, groupRule.Path2);
					columnName2 = GetColumnName(columnNames, columnIndex2, groupRule.Path2);
					type = GetColumnType(columnTypes, columnIndex2);
				}
			}
			if (groupRule.Condition == StiDataFilterCondition.NotEqualTo && text == null)
			{
				text = columnName + " IS NULL";
			}
			string condition = GetCondition(columnName, columnName2, groupRule.Condition, text2, value, columnType, type);
			StiDataFilterOperation filterOperation = GetFilterOperation(groupRule);
			if (filterOperation == StiDataFilterOperation.OR)
			{
				stringBuilder.Append((stringBuilder.Length > 0) ? (" OR " + condition) : condition);
			}
			else
			{
				stringBuilder2.Append((stringBuilder2.Length > 0) ? (" AND " + condition) : condition);
			}
		}
		StringBuilder stringBuilder5 = new StringBuilder();
		if (stringBuilder3.Length > 0)
		{
			stringBuilder5.Append($"{stringBuilder3})");
		}
		if (stringBuilder.Length > 0)
		{
			if (stringBuilder3.Length > 0)
			{
				stringBuilder5.Append(" OR ");
			}
			stringBuilder5.Append((object?)stringBuilder);
			if (stringBuilder5.ToString().IndexOf(" OR ") > 0 && stringBuilder4.Length > 0 && stringBuilder2.Length > 0)
			{
				stringBuilder5.Insert(0, "(");
				stringBuilder5.Append(")");
			}
		}
		if (stringBuilder4.Length > 0)
		{
			if (stringBuilder5.Length > 0)
			{
				stringBuilder5.Append(" AND ");
			}
			stringBuilder5.Append((!string.IsNullOrEmpty(text)) ? $"({stringBuilder4}) OR {text})" : $"{stringBuilder4})");
		}
		if (stringBuilder2.Length > 0)
		{
			if (stringBuilder5.Length > 0)
			{
				stringBuilder5.Append(" AND ");
			}
			stringBuilder5.Append((!string.IsNullOrEmpty(text)) ? $"({stringBuilder2} OR {text})" : stringBuilder2.ToString());
		}
		return stringBuilder5;
	}

	private static int GetColumnIndex(List<string> columnNames, string path)
	{
		if (columnNames == null || string.IsNullOrEmpty(path))
		{
			return 0;
		}
		int num = ((columnNames.IndexOf(path) != -1) ? columnNames.IndexOf(path) : columnNames.IndexOf(GetCleanedPath(path)));
		if (num == -1)
		{
			columnNames = columnNames.Select((string c) => c?.Replace(" ", "")).ToList();
			num = ((columnNames.IndexOf(path) != -1) ? columnNames.IndexOf(path) : columnNames.IndexOf(GetCleanedPath(path)));
		}
		return num;
	}

	private static string GetCleanedPath(string path)
	{
		if (path.StartsWith("["))
		{
			path = path.Substring(1);
		}
		if (path.EndsWith("]"))
		{
			path = path.Substring(0, path.Length - 1);
		}
		return path;
	}

	private static string GetColumnName(List<string> columnNames, int columnIndex, string path)
	{
		return StiDataColumnRuleHelper.GetGoodColumnName((columnNames != null && columnIndex != -1) ? columnNames[columnIndex] : path);
	}

	private static Type GetColumnType(List<Type> columnTypes, int columnIndex)
	{
		if (columnTypes == null || columnIndex == -1)
		{
			return typeof(object);
		}
		return columnTypes[columnIndex];
	}

	private static string GetValue(Type type, string value)
	{
		if (type == typeof(string))
		{
			return value;
		}
		if (type != typeof(double) && type != typeof(float) && type != typeof(decimal))
		{
			if (value != null)
			{
				return value;
			}
			return string.Empty;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			if (type == typeof(double))
			{
				return StiValueHelper.TryToDouble(value).ToString();
			}
			if (type == typeof(float))
			{
				return StiValueHelper.TryToFloat(value).ToString();
			}
			if (type == typeof(decimal))
			{
				return StiValueHelper.TryToDecimal(value).ToString();
			}
			return value;
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	private static string GetCondition(string columnName, string columnName2, StiDataFilterCondition condition, string value, string value2, Type columnType, Type columnType2)
	{
		switch (condition)
		{
		case StiDataFilterCondition.EqualTo:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			if (!columnType.IsDateType())
			{
				return columnName + " = " + value;
			}
			return "(" + columnName + " >= #" + value + " 00:00:00# AND " + columnName + " <= #" + value + " 23:59:59#)";
		case StiDataFilterCondition.NotEqualTo:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			if (!columnType.IsDateType())
			{
				return columnName + " <> " + value;
			}
			return "(" + columnName + " < #" + value + " 00:00:00# OR " + columnName + " > #" + value + " 23:59:59#)";
		case StiDataFilterCondition.GreaterThan:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			if (!columnType.IsDateType())
			{
				return columnName + " > " + value;
			}
			return columnName + " > #" + value + " 23:59:59#";
		case StiDataFilterCondition.GreaterThanOrEqualTo:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			if (!columnType.IsDateType())
			{
				return columnName + " >= " + value;
			}
			return columnName + " >= #" + value + " 00:00:00#";
		case StiDataFilterCondition.LessThan:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			if (!columnType.IsDateType())
			{
				return columnName + " < " + value;
			}
			return columnName + " < #" + value + " 00:00:00#";
		case StiDataFilterCondition.LessThanOrEqualTo:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			if (!columnType.IsDateType())
			{
				return columnName + " <= " + value;
			}
			return columnName + " <= #" + value + " 23:59:59#";
		case StiDataFilterCondition.Between:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			value2 = GetQueryValue(value2, columnType, stringQuotes: true);
			if (!columnType.IsDateType())
			{
				return "(" + columnName + " >= " + value + " AND " + columnName + " <= " + value2 + ")";
			}
			return "(" + columnName + " >= #" + value + " 00:00:00# AND " + columnName + " <= #" + value2 + " 23:59:59#)";
		case StiDataFilterCondition.NotBetween:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			value2 = GetQueryValue(value2, columnType, stringQuotes: true);
			if (!columnType.IsDateType())
			{
				return "(" + columnName + " < " + value + " OR " + columnName + " > " + value2 + ")";
			}
			return "(" + columnName + " < #" + value + " 00:00:00# OR " + columnName + " > #" + value2 + " 23:59:59#)";
		case StiDataFilterCondition.Containing:
			value = GetQueryValue(value, columnType, stringQuotes: false);
			return columnName + " LIKE '*" + value + "*'";
		case StiDataFilterCondition.NotContaining:
			value = GetQueryValue(value, columnType, stringQuotes: false);
			return "NOT (" + columnName + " LIKE '*" + value + "*')";
		case StiDataFilterCondition.BeginningWith:
			value = GetQueryValue(value, columnType, stringQuotes: false);
			return columnName + " LIKE '" + value + "*'";
		case StiDataFilterCondition.EndingWith:
			value = GetQueryValue(value, columnType, stringQuotes: false);
			return columnName + " LIKE '*" + value + "'";
		case StiDataFilterCondition.IsNull:
			return columnName + " IS NULL";
		case StiDataFilterCondition.IsNotNull:
			return columnName + " IS NOT NULL";
		case StiDataFilterCondition.IsBlank:
			return "TRIM(" + columnName + ") = ''";
		case StiDataFilterCondition.IsNotBlank:
			return "TRIM(" + columnName + ") <> ''";
		case StiDataFilterCondition.IsBlankOrNull:
			return "(TRIM(" + columnName + ") = '' OR " + columnName + " IS NULL)";
		case StiDataFilterCondition.IsFalse:
			return "FALSE";
		case StiDataFilterCondition.PairEqualTo:
		{
			string text = ((value == null) ? "IS" : "=");
			string text2 = ((value2 == null) ? "IS" : "=");
			value = ((value != null || columnType.IsDateType()) ? GetQueryValue(value, columnType, stringQuotes: true) : "NULL");
			value2 = ((value2 != null || columnType2.IsDateType()) ? GetQueryValue(value2, columnType2, stringQuotes: true) : "NULL");
			string text3 = (columnType.IsDateType() ? (columnName + " >= #" + value + " 00:00:00# AND " + columnName + " <= #" + value + " 23:59:59# AND ") : (columnName + " " + text + " " + value + " AND "));
			if (!columnType2.IsDateType())
			{
				return text3 + columnName2 + " " + text2 + " " + value2;
			}
			return text3 + columnName2 + " >= #" + value2 + " 00:00:00# AND " + columnName2 + " <= #" + value2 + " 23:59:59#";
		}
		case StiDataFilterCondition.MapEqualTo:
			value = GetQueryValue(value, columnType, stringQuotes: true);
			return columnName + " = " + value;
		default:
			throw new StiTypeNotRecognizedException(condition);
		}
	}

	private static string GetQueryValue(string value, Type type, bool stringQuotes)
	{
		if (value == null)
		{
			value = string.Empty;
		}
		if (type == null)
		{
			return value;
		}
		if (type.IsNumericType())
		{
			if (string.IsNullOrEmpty(value))
			{
				return "0";
			}
			if (!decimal.TryParse(value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out var _))
			{
				return "0";
			}
			return value;
		}
		if (type == typeof(bool))
		{
			return (value != null && value.ToLower() == "true").ToString().ToUpper();
		}
		if (type.IsDateType())
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			if (!DateTime.TryParseExact(value, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result2))
			{
				DateTime? dateTime = StiValueHelper.TryToNullableDateTime(value);
				result2 = ((!dateTime.HasValue) ? new DateTime(1800, 1, 1) : dateTime.Value);
			}
			Thread.CurrentThread.CurrentCulture = currentCulture;
			return result2.ToString("MM-dd-yyyy");
		}
		if (type.IsEnum && Enum.IsDefined(type, value))
		{
			value = ParseEnum(value, type);
		}
		if (!string.IsNullOrEmpty(value))
		{
			value = value.Replace("'", "''");
		}
		if (stringQuotes)
		{
			return "'" + value + "'";
		}
		return value;
	}

	private static string ParseEnum(string value, Type type)
	{
		string valueStr = value;
		if (valueStr != null)
		{
			string[] names = Enum.GetNames(type);
			if (names.Any((string n) => n.ToLowerInvariant() == valueStr.ToLowerInvariant()))
			{
				return ((int)Enum.Parse(type, valueStr, ignoreCase: true)).ToString();
			}
		}
		if (value != null)
		{
			return Enum.GetName(type, value);
		}
		return value;
	}

	private static StiDataFilterOperation GetFilterOperation(StiDataFilterRule rule)
	{
		if (rule.Condition != 0 && rule.Condition != StiDataFilterCondition.BeginningWith && rule.Condition != StiDataFilterCondition.EndingWith && rule.Condition != StiDataFilterCondition.Between && rule.Condition != StiDataFilterCondition.Containing && rule.Condition != StiDataFilterCondition.GreaterThan && rule.Condition != StiDataFilterCondition.GreaterThanOrEqualTo && rule.Condition != StiDataFilterCondition.LessThan && rule.Condition != StiDataFilterCondition.LessThanOrEqualTo && rule.Condition != StiDataFilterCondition.IsNull && rule.Condition != StiDataFilterCondition.IsBlank && rule.Condition != StiDataFilterCondition.IsBlankOrNull && rule.Condition != StiDataFilterCondition.PairEqualTo && rule.Condition != StiDataFilterCondition.MapEqualTo)
		{
			return StiDataFilterOperation.AND;
		}
		return StiDataFilterOperation.OR;
	}

	internal static int GetFilterRulesHash(IStiApp app, IEnumerable<StiDataFilterRule> rules)
	{
		if (rules == null || !rules.Any())
		{
			return 0;
		}
		IEnumerable<IStiAppVariable> variables = app?.GetDictionary()?.FetchVariables();
		if (variables == null || !variables.Any())
		{
			return 0;
		}
		return rules.Select((StiDataFilterRule r) => GetFilterRuleHash(variables, r)).Aggregate(0, (int c1, int c2) => c1 + c2);
	}

	private static int GetFilterRuleHash(IEnumerable<IStiAppVariable> variables, StiDataFilterRule rule)
	{
		if (!rule.IsExpression)
		{
			return 0;
		}
		StiDataFilterCondition condition = rule.Condition;
		if ((uint)(condition - 6) <= 1u)
		{
			return GetFilterRuleHash(variables, rule.Value) + GetFilterRuleHash(variables, rule.Value2);
		}
		return GetFilterRuleHash(variables, rule.Value);
	}

	private static int GetFilterRuleHash(IEnumerable<IStiAppVariable> variables, string expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
		{
			return 0;
		}
		expression = expression.ToLowerInvariant();
		int num = 0;
		foreach (IStiAppVariable variable in variables)
		{
			if (expression.Contains(variable.GetName().ToLowerInvariant()))
			{
				int num2 = (variable.GetValue()?.ToString())?.GetHashCode() ?? 0;
				num += num2;
			}
		}
		return num;
	}
}
