using System.Linq;

namespace Stimulsoft.Data.Engine;

public class StiDataColumnRuleHelper
{
	private static string[] keywords = new string[12]
	{
		"and", "between", "child", "false", "in", "is", "like", "not", "null", "or",
		"parent", "true"
	};

	public static bool IsGoodColumnName(string columnName)
	{
		if (string.IsNullOrWhiteSpace(columnName))
		{
			return false;
		}
		if (columnName.StartsWith("["))
		{
			return true;
		}
		if (columnName.Any((char c) => !char.IsLetterOrDigit(c)))
		{
			return false;
		}
		if (char.IsDigit(columnName[0]))
		{
			return false;
		}
		return !keywords.Contains(columnName.ToLowerInvariant());
	}

	public static string GetGoodColumnName(string columnName, bool normalize = false)
	{
		if ((columnName.Contains(",") || columnName.Contains("'")) && normalize)
		{
			columnName = columnName.Replace(",", "").Replace("'", "");
		}
		if (!IsGoodColumnName(columnName))
		{
			return "[" + columnName + "]";
		}
		return columnName;
	}
}
