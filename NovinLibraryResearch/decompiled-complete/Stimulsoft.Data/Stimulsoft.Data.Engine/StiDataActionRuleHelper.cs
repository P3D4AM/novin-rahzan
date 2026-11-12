using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Data.Comparers;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Data.Engine;

public class StiDataActionRuleHelper
{
	public static List<StiDataActionRule> ToList(params StiDataActionRule[] rules)
	{
		return rules.ToList();
	}

	internal static List<StiDataActionRule> Validate(List<StiDataActionRule> rules, List<string> columnKeys)
	{
		return rules.Where((StiDataActionRule r) => StiKeyHelper.IsKey(r.Key) && columnKeys.Contains(r.Key)).ToList();
	}

	private static int GetColumnIndex(StiDataActionRule action, List<string> columnKeys, List<string> columnNames)
	{
		int num = -1;
		if (columnNames != null && !string.IsNullOrEmpty(action.Path))
		{
			num = columnNames.IndexOf(action.Path);
		}
		if (num == -1 && columnKeys != null && !string.IsNullOrEmpty(action.Key))
		{
			num = columnKeys.IndexOf(action.Key);
		}
		return num;
	}

	public static void ApplyActions(DataTable table, List<StiDataActionRule> actions, List<string> columnKeys, List<string> columnNames, IStiReport report)
	{
		actions.Sort(new StiDataActionComparer());
		foreach (StiDataActionRule action in actions)
		{
			int columnIndex = GetColumnIndex(action, columnKeys, columnNames);
			switch (action.Type)
			{
			case StiDataActionType.Limit:
				ApplyLimitAction(table, action.StartIndex, action.RowsCount);
				break;
			case StiDataActionType.Replace:
				ApplyReplaceAction(table, columnIndex, action.ValueFrom, action.ValueTo, action.MatchCase, action.MatchWholeWord, report);
				break;
			case StiDataActionType.RunningTotal:
				ApplyRunningTotalAction(table, columnIndex, action.InitialValue, report);
				break;
			case StiDataActionType.Percentage:
				ApplyPercentageAction(table, columnIndex);
				break;
			}
		}
	}

	private static void ApplyLimitAction(DataTable table, int startIndex, int rowsCount)
	{
		if (rowsCount < 0)
		{
			rowsCount = table.Rows.Count;
		}
		IEnumerable<DataRow> second = table.AsEnumerable().Skip(startIndex).Take(Math.Max(0, rowsCount));
		table.AsEnumerable().Except(second).ToList()
			.ForEach(delegate(DataRow r)
			{
				r.Delete();
			});
		table.AcceptChanges();
	}

	private static void ApplyReplaceAction(DataTable table, int columnIndex, string valueFrom, string valueTo, bool matchCase, bool matchWholeWord, IStiReport report)
	{
		if (columnIndex == -1 || table.PrimaryKey.Any((DataColumn c) => c == table.Columns[columnIndex]))
		{
			return;
		}
		valueFrom = StiExpressionHelper.ParseReportExpression(report, valueFrom, withBraces: true);
		valueTo = StiExpressionHelper.ParseReportExpression(report, valueTo, withBraces: true);
		table.AsEnumerable().ToList().ForEach(delegate(DataRow r)
		{
			string text = StiValueHelper.TryToString(r[columnIndex]);
			if (!string.IsNullOrEmpty(text))
			{
				Regex regex = new Regex(matchWholeWord ? ("\\b" + valueFrom + "\\b") : valueFrom, matchCase ? RegexOptions.CultureInvariant : (RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
				r[columnIndex] = regex.Replace(text, valueTo);
			}
		});
	}

	private static void ApplyRunningTotalAction(DataTable table, int columnIndex, string initialValue, IStiReport report)
	{
		if (columnIndex != -1 && !table.PrimaryKey.Any((DataColumn c) => c == table.Columns[columnIndex]))
		{
			initialValue = StiExpressionHelper.ParseReportExpression(report, initialValue, withBraces: true);
			decimal.TryParse(initialValue.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out var currentValue);
			table.AsEnumerable().ToList().ForEach(delegate(DataRow r)
			{
				r[columnIndex] = (currentValue += StiValueHelper.TryToDecimal(r[columnIndex]));
			});
		}
	}

	private static void ApplyPercentageAction(DataTable table, int columnIndex)
	{
		if (columnIndex != -1 && !table.PrimaryKey.Any((DataColumn c) => c == table.Columns[columnIndex]))
		{
			decimal sum = Funcs.Sum(from r in table.AsEnumerable()
				select r[columnIndex]);
			table.AsEnumerable().ToList().ForEach(delegate(DataRow r)
			{
				r[columnIndex] = Math.Round(StiValueHelper.TryToDecimal(r[columnIndex]) / sum * 100m, 2);
			});
		}
	}
}
