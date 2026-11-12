using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Render;

internal abstract class StiElementBuilder
{
	protected int? GetSeriesKeysLimitCount(StiElement element)
	{
		int? result = null;
		if (element.IsDesigning)
		{
			result = 100;
		}
		return result;
	}

	protected string GetSeries(StiDataTable table, int seriesIndex, object seriesKey, StiReport report)
	{
		if (table == null || seriesIndex == -1)
		{
			return null;
		}
		object[] array = table.Rows.WhereFirstOrDefaultArrayItemStringEqualsTo(seriesIndex, ToString(seriesKey));
		object obj = ((array != null && array.Length != 0) ? array[seriesIndex] : null);
		if (obj is DateTime dateTime)
		{
			return dateTime.ToShortDateString();
		}
		return ToReadableString(obj, report);
	}

	protected string GetSeries(StiDataTable table, int seriesIndex, object seriesKey, bool shortValue, StiReport report)
	{
		if (table == null || seriesIndex == -1)
		{
			return null;
		}
		object[] array = table.Rows.WhereFirstOrDefaultArrayItemStringEqualsTo(seriesIndex, ToString(seriesKey));
		object obj = ((array != null && array.Length != 0) ? array[seriesIndex] : null);
		if (obj is DateTime dateTime)
		{
			return dateTime.ToShortDateString();
		}
		if (shortValue && obj != null)
		{
			if (obj is decimal || obj is double || obj is int)
			{
				return StiAbbreviationNumberFormatHelper.Format(Convert.ToDouble(obj));
			}
			if (obj is string s && double.TryParse(s, out var result))
			{
				return StiAbbreviationNumberFormatHelper.Format(result);
			}
		}
		return ToReadableString(obj, report);
	}

	protected decimal GetValue(StiDataTable table, int valueIndex, int seriesIndex, int seriesCount, string seriesKey)
	{
		if (table == null || valueIndex == -1)
		{
			return 0m;
		}
		if (seriesCount < 2)
		{
			return table.Rows.GetArrayItem(valueIndex).FirstOrDefaultAsDecimal();
		}
		object[] array = table.Rows.WhereFirstOrDefaultArrayItemStringEqualsTo(seriesIndex, seriesKey);
		if (array == null || array.Length == 0)
		{
			return 0m;
		}
		return StiValueHelper.TryToDecimal(array[valueIndex]);
	}

	protected decimal? GetTarget(StiDataTable table, int targetIndex, int seriesIndex, int seriesCount, string seriesKey)
	{
		if (table == null || targetIndex == -1)
		{
			return null;
		}
		if (seriesCount < 2)
		{
			return table.Rows.GetArrayItem(targetIndex).FirstOrDefaultAsDecimal();
		}
		object[] array = table.Rows.WhereFirstOrDefaultArrayItemStringEqualsTo(seriesIndex, seriesKey);
		return (array != null && array.Length != 0) ? StiValueHelper.TryToDecimal(array[targetIndex]) : 0m;
	}

	protected decimal? GetNullableValue(StiDataTable table, int valueIndex, int seriesIndex, int seriesCount, string seriesKey)
	{
		if (table == null || valueIndex == -1)
		{
			return null;
		}
		if (seriesCount < 2)
		{
			return table.Rows.GetArrayItem(valueIndex).FirstOrDefaultAsNullableDecimal();
		}
		object[] array = table.Rows.WhereFirstOrDefaultArrayItemStringEqualsTo(seriesIndex, seriesKey);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		return StiValueHelper.TryToNullableDecimal(array[valueIndex]);
	}

	protected decimal? GetNullableTarget(StiDataTable table, int targetIndex, int seriesIndex, int seriesCount, string seriesKey)
	{
		if (table == null || targetIndex == -1)
		{
			return null;
		}
		if (seriesCount < 2)
		{
			return table.Rows.GetArrayItem(targetIndex).FirstOrDefaultAsNullableDecimal();
		}
		object[] array = table.Rows.WhereFirstOrDefaultArrayItemStringEqualsTo(seriesIndex, seriesKey);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		return StiValueHelper.TryToNullableDecimal(array[targetIndex]);
	}

	protected IEnumerable<object> GetSeriesKeyValues(StiDataTable table, int seriesIndex, int? count)
	{
		if (table?.Rows == null || seriesIndex == -1)
		{
			return new List<string> { string.Empty };
		}
		IEnumerable<object> enumerable = ((!count.HasValue) ? (from r in table.Rows.GetArrayItem(seriesIndex).Distinct().Select(ReplaceDbNull)
			where r != null
			select r) : (from r in table.Rows.GetArrayItem(seriesIndex).Distinct().Select(ReplaceDbNull)
			where r != null
			select r).Take(count.GetValueOrDefault()));
		if (!enumerable.Any())
		{
			return new List<string> { string.Empty };
		}
		return enumerable;
	}

	protected IEnumerable<string> GetSeriesKeys(StiDataTable table, int seriesIndex, int? count)
	{
		IEnumerable<object> seriesKeyValues = GetSeriesKeyValues(table, seriesIndex, count);
		if (seriesKeyValues == null)
		{
			return new List<string> { string.Empty };
		}
		return seriesKeyValues.Select(ToString);
	}

	private object ReplaceDbNull(object value)
	{
		if (value != DBNull.Value)
		{
			return value;
		}
		return null;
	}

	protected string ToString(object value)
	{
		if (value != null)
		{
			return value.ToString();
		}
		return string.Empty;
	}

	protected string ToReadableString(object value, StiReport report)
	{
		if (StiExtValueConverter.IsEligable(value, report?.Culture))
		{
			return StiExtValueConverter.Convert(value, report?.Culture);
		}
		return ToString(value);
	}
}
