using System.Text;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components;

public static class StiDataBandSQLFilterHelper
{
	public static string GetFilter(StiDataBand band, bool useDataSourceNames)
	{
		StiSqlSource stiSqlSource = band.DataSource as StiSqlSource;
		if (!band.FilterOn || band.Filters.Count <= 0 || stiSqlSource == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(" ");
		for (int i = 0; i < band.Filters.Count; i++)
		{
			StiFilter stiFilter = band.Filters[i];
			stringBuilder.Append("(");
			if (useDataSourceNames)
			{
				stringBuilder.Append(GetFilterExpression(stiFilter, stiSqlSource.Name + "." + stiFilter.Column));
			}
			else
			{
				stringBuilder.Append(GetFilterExpression(stiFilter, (stiFilter.Item == StiFilterItem.Value) ? stiSqlSource.Columns[stiFilter.Column].NameInSource : ""));
			}
			stringBuilder.Append(")");
			if (i < band.Filters.Count - 1)
			{
				stringBuilder.Append((band.FilterMode == StiFilterMode.And) ? " AND " : " OR ");
			}
		}
		return stringBuilder.ToString();
	}

	private static string GetFilterExpression(StiFilter filter, string fullColumnName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (filter.Item == StiFilterItem.Expression)
		{
			string value = filter.Expression.Value;
			if (value != null && value.Length >= 2)
			{
				stringBuilder.Append(value.Substring(1, value.Length - 2).Replace("==", "="));
			}
		}
		else if (filter.Item == StiFilterItem.Value)
		{
			if (filter.DataType == StiFilterDataType.String)
			{
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append(fullColumnName + " = '" + filter.Value1 + "'");
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append(fullColumnName + " <> '" + filter.Value1 + "'");
					break;
				case StiFilterCondition.Containing:
					stringBuilder.Append(fullColumnName + " LIKE '%" + filter.Value1 + "%'");
					break;
				case StiFilterCondition.NotContaining:
					stringBuilder.Append(fullColumnName + " NOT LIKE '%" + filter.Value1 + "%'");
					break;
				case StiFilterCondition.BeginningWith:
					stringBuilder.Append(fullColumnName + " LIKE '" + filter.Value1 + "%'");
					break;
				case StiFilterCondition.EndingWith:
					stringBuilder.Append(fullColumnName + " LIKE '%" + filter.Value1 + "'");
					break;
				}
			}
			else if (filter.DataType == StiFilterDataType.Numeric)
			{
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append(fullColumnName + " = " + filter.Value1);
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append(fullColumnName + " <> " + filter.Value1);
					break;
				case StiFilterCondition.Between:
					stringBuilder.Append(fullColumnName + " >= " + filter.Value1 + " AND " + fullColumnName + " <= " + filter.Value2);
					break;
				case StiFilterCondition.NotBetween:
					stringBuilder.Append(fullColumnName + " < " + filter.Value1 + " OR " + fullColumnName + " > " + filter.Value2);
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
			else if (filter.DataType == StiFilterDataType.DateTime)
			{
				string text = string.Empty;
				if (!string.IsNullOrEmpty(filter.Value1))
				{
					string[] array = filter.Value1.Split(new char[1] { '/' });
					text = "'" + array[2] + "-" + array[0] + "-" + array[1] + "'";
				}
				string text2 = string.Empty;
				if (!string.IsNullOrEmpty(filter.Value2))
				{
					string[] array2 = filter.Value2.Split(new char[1] { '/' });
					text2 = "'" + array2[2] + "-" + array2[0] + "-" + array2[1] + "'";
				}
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append(fullColumnName + " = " + text);
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append(fullColumnName + " <> " + text);
					break;
				case StiFilterCondition.Between:
					stringBuilder.Append(fullColumnName + " >= " + text + " AND " + fullColumnName + " <= " + text2);
					break;
				case StiFilterCondition.NotBetween:
					stringBuilder.Append(fullColumnName + " < " + text + " OR " + fullColumnName + " > " + text2);
					break;
				case StiFilterCondition.GreaterThan:
					stringBuilder.Append(fullColumnName + " > " + text);
					break;
				case StiFilterCondition.GreaterThanOrEqualTo:
					stringBuilder.Append(fullColumnName + " >= " + text);
					break;
				case StiFilterCondition.LessThan:
					stringBuilder.Append(fullColumnName + " < " + text);
					break;
				case StiFilterCondition.LessThanOrEqualTo:
					stringBuilder.Append(fullColumnName + " <= " + text);
					break;
				}
			}
			else if (filter.DataType == StiFilterDataType.Boolean)
			{
				switch (filter.Condition)
				{
				case StiFilterCondition.EqualTo:
					stringBuilder.Append(fullColumnName + " = " + filter.Value1);
					break;
				case StiFilterCondition.NotEqualTo:
					stringBuilder.Append(fullColumnName + " <> " + filter.Value1);
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
					stringBuilder.Append(fullColumnName + " >= " + filter.Value1 + " AND " + fullColumnName + " <= " + filter.Value2);
					break;
				case StiFilterCondition.NotBetween:
					stringBuilder.Append(fullColumnName + " < " + filter.Value1 + " OR " + fullColumnName + " > " + filter.Value2);
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
		return stringBuilder.ToString();
	}
}
