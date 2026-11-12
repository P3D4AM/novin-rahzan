using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Stimulsoft.Base;

namespace Stimulsoft.Data.Engine;

public class StiDataSortRuleHelper
{
	public static List<StiDataSortRule> ToList(params StiDataSortRule[] rules)
	{
		return rules.ToList();
	}

	internal static List<StiDataSortRule> Validate(List<StiDataSortRule> rules, List<string> columnKeys)
	{
		if (columnKeys == null || columnKeys.Count == 0)
		{
			return rules;
		}
		return rules.Where((StiDataSortRule r) => StiKeyHelper.IsKey(r.Key) && columnKeys.Contains(r.Key)).ToList();
	}

	internal static string GetDataTableSortQuery(List<StiDataSortRule> rules, IEnumerable<IStiAppDataColumn> columns)
	{
		List<string> columnKeys = (from c in columns
			select c?.GetKey() into c
			where c != null
			select c).ToList();
		List<string> columnNames = (from c in columns
			select c?.GetName() into c
			where c != null
			select c).ToList();
		return GetDataTableSortQuery(rules, columnKeys, columnNames);
	}

	internal static string GetDataTableSortQuery(List<StiDataSortRule> rules, List<string> columnKeys, List<string> columnNames, bool normalize = false)
	{
		if (rules == null || !rules.Any())
		{
			return string.Empty;
		}
		rules = Validate(rules, columnKeys);
		StringBuilder stringBuilder = new StringBuilder();
		foreach (StiDataSortRule item in rules.Where((StiDataSortRule r) => !string.IsNullOrWhiteSpace(r.Key)))
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder = stringBuilder.Append(", ");
			}
			int num = columnKeys.IndexOf(item.Key);
			if (num == -1)
			{
				num = columnNames.IndexOf(item.Key);
				if (num == -1)
				{
					continue;
				}
			}
			string goodColumnName = StiDataColumnRuleHelper.GetGoodColumnName(columnNames[num], normalize);
			stringBuilder = ((item.Direction != StiDataSortDirection.Descending) ? stringBuilder.Append(goodColumnName) : stringBuilder.Append(goodColumnName + " DESC"));
		}
		return stringBuilder.ToString();
	}

	internal static DataTable OrderDataTableWithCommainColumnNames(DataTable table, List<StiDataSortRule> rules, List<string> columnKeys, List<string> columnNames)
	{
		string[] oldNames = (from DataColumn c in table.Columns
			select c.ColumnName).ToArray();
		NormalizeCommas(table);
		table.DefaultView.Sort = GetDataTableSortQuery(rules, columnKeys, columnNames, normalize: true);
		table = table.DefaultView.ToTable();
		int columnIndex = 0;
		table.Columns.Cast<DataColumn>().ToList().ForEach(delegate(DataColumn c)
		{
			c.ColumnName = oldNames[columnIndex++];
		});
		return table;
	}

	internal static bool IsCommaInColumnNames(DataTable table)
	{
		return table.Columns.Cast<DataColumn>().Any((DataColumn c) => c.ColumnName.Contains(",") || c.ColumnName.Contains("'"));
	}

	internal static void NormalizeCommas(DataTable table)
	{
		(from DataColumn c in table.Columns
			where c.ColumnName.Contains(",") || c.ColumnName.Contains("'")
			select c).ToList().ForEach(delegate(DataColumn c)
		{
			c.ColumnName = c.ColumnName.Replace(",", "").Replace("'", "");
		});
	}

	public static StiDataSortDirection GetSortDirection(List<StiDataSortRule> rules, string columnKey)
	{
		if (rules == null)
		{
			return StiDataSortDirection.None;
		}
		return rules.FirstOrDefault((StiDataSortRule r) => string.Equals(r.Key, columnKey, StringComparison.InvariantCultureIgnoreCase))?.Direction ?? StiDataSortDirection.None;
	}

	public static List<StiDataSortRule> SetSortDirection(List<StiDataSortRule> rules, List<string> columnKeys, string columnKey, StiDataSortDirection direction)
	{
		rules = Validate(rules, columnKeys);
		StiDataSortRule stiDataSortRule = rules.FirstOrDefault((StiDataSortRule r) => string.Equals(r.Key, columnKey, StringComparison.InvariantCultureIgnoreCase));
		if (stiDataSortRule == null && direction != StiDataSortDirection.None)
		{
			stiDataSortRule = new StiDataSortRule(columnKey, direction);
			rules.Add(stiDataSortRule);
		}
		else if (direction == StiDataSortDirection.None)
		{
			rules.Remove(stiDataSortRule);
		}
		else
		{
			stiDataSortRule.Direction = direction;
		}
		return rules;
	}
}
