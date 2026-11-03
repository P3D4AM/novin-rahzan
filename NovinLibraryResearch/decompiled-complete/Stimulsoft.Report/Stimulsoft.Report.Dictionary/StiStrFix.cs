namespace Stimulsoft.Report.Dictionary;

public static class StiStrFix
{
	public static string Del_(string str)
	{
		if (!string.IsNullOrWhiteSpace(str))
		{
			return str.Replace("_", string.Empty);
		}
		return str;
	}
}
