namespace Stimulsoft.Base;

public static class StiTableName
{
	public static string GetName(string schema, string table)
	{
		if (!string.IsNullOrWhiteSpace(schema))
		{
			return schema + "." + table;
		}
		return table;
	}
}
