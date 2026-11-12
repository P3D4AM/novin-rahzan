using System;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiTableElementHelper
{
	internal static decimal MinDataColumnValue(StiTableColumn column, StiDataTable dataTable, int columnIndex)
	{
		if (!(column is StiDataBarsColumn) && !(column is StiColorScaleColumn) && !(column is StiBubbleColumn))
		{
			return 0m;
		}
		if (!dataTable.Rows.Any())
		{
			return 0m;
		}
		return dataTable.Rows.Min((object[] r) => StiValueHelper.TryToDecimal(r[columnIndex]));
	}

	internal static decimal MaxDataColumnValue(StiTableColumn column, StiDataTable dataTable, int columnIndex)
	{
		if (!(column is StiDataBarsColumn) && !(column is StiColorScaleColumn) && !(column is StiBubbleColumn))
		{
			return 0m;
		}
		if (!dataTable.Rows.Any())
		{
			return 0m;
		}
		return dataTable.Rows.Max((object[] r) => StiValueHelper.TryToDecimal(r[columnIndex]));
	}

	internal static decimal CalculateTotal(StiTableColumn column, int columnIndex, StiDataTable dataTable)
	{
		switch (column.SummaryType)
		{
		case StiSummaryColumnType.Sum:
			if (StiExpressionHelper.IsPercentOfGrandTotal(column.Expression))
			{
				return 1m;
			}
			if (!dataTable.Rows.Any())
			{
				return 0m;
			}
			return dataTable.Rows.Sum((object[] r) => (columnIndex >= r.Length) ? 0m : StiValueHelper.TryToDecimal(r[columnIndex]));
		case StiSummaryColumnType.Count:
			if (!dataTable.Rows.Any())
			{
				return 0m;
			}
			return dataTable.Rows.Count((object[] r) => columnIndex < r.Length && r[columnIndex] != null && r[columnIndex] != DBNull.Value);
		case StiSummaryColumnType.Min:
			if (!dataTable.Rows.Any())
			{
				return 0m;
			}
			return dataTable.Rows.Min((object[] r) => (columnIndex >= r.Length) ? 0m : StiValueHelper.TryToDecimal(r[columnIndex]));
		case StiSummaryColumnType.Max:
			if (!dataTable.Rows.Any())
			{
				return 0m;
			}
			return dataTable.Rows.Max((object[] r) => (columnIndex >= r.Length) ? 0m : StiValueHelper.TryToDecimal(r[columnIndex]));
		case StiSummaryColumnType.Average:
		{
			decimal num = (dataTable.Rows.Any() ? dataTable.Rows.Sum((object[] r) => (columnIndex >= r.Length) ? 0m : StiValueHelper.TryToDecimal(r[columnIndex])) : 0m);
			int num2 = (dataTable.Rows.Any() ? dataTable.Rows.Count((object[] r) => columnIndex < r.Length && r[columnIndex] != null && r[columnIndex] != DBNull.Value) : 0);
			if (num2 != 0)
			{
				return num / (decimal)num2;
			}
			return 0m;
		}
		default:
			return 0m;
		}
	}
}
