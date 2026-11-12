using System.Data;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Data.Extensions;

public static class DataColumnExt
{
	public static bool IsNumericType(this DataColumn column)
	{
		if (column?.DataType != null)
		{
			return column.DataType.IsNumericType();
		}
		return false;
	}

	public static bool IsDateType(this DataColumn column)
	{
		if (column?.DataType != null)
		{
			return column.DataType.IsDateType();
		}
		return false;
	}

	public static bool IsIntegerType(this DataColumn column)
	{
		if (column?.DataType != null)
		{
			return column.DataType.IsIntegerType();
		}
		return false;
	}

	public static bool IsMoneyName(this DataColumn column)
	{
		return StiMoneyNameHelper.IsMoneyName(column?.ColumnName);
	}

	public static string GetHumanReadableName(this DataColumn column)
	{
		return StiHumanReadableHelper.GetHumanReadableName(column.ColumnName);
	}
}
