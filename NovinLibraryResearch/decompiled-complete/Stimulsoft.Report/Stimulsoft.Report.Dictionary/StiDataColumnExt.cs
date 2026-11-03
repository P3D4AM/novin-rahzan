using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Dictionary;

public static class StiDataColumnExt
{
	public static StiDataSource GetDataSourceByColumnKey(StiReport report, string columnKey)
	{
		if (StiKeyHelper.IsEmptyKey(columnKey))
		{
			return null;
		}
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			StiDataColumn stiDataColumn = dataSource.Columns.ToList().FirstOrDefault((StiDataColumn c) => c.Key == columnKey);
			if (stiDataColumn != null)
			{
				return dataSource;
			}
		}
		return null;
	}

	public static StiDataColumn GetColumnByKey(StiReport report, string columnKey)
	{
		if (StiKeyHelper.IsEmptyKey(columnKey))
		{
			return null;
		}
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			StiDataColumn stiDataColumn = dataSource.Columns.ToList().FirstOrDefault((StiDataColumn c) => c.Key == columnKey);
			if (stiDataColumn != null)
			{
				return stiDataColumn;
			}
		}
		return null;
	}

	public static bool IsNumericType(this StiDataColumn column)
	{
		if (column != null && column.Type != null)
		{
			return TypeExt.IsNumericType(column.Type);
		}
		return false;
	}

	public static bool IsDateType(this StiDataColumn column)
	{
		if (column != null && column.Type != null)
		{
			return TypeExt.IsDateType(column.Type);
		}
		return false;
	}

	public static bool IsIntegerType(this StiDataColumn column)
	{
		if (column != null && column.Type != null)
		{
			return TypeExt.IsIntegerType(column.Type);
		}
		return false;
	}

	public static bool IsArray(this StiDataColumn column)
	{
		if (column != null && column.Type != null)
		{
			return column.Type.IsArray;
		}
		return false;
	}
}
