namespace Stimulsoft.Report.Components;

public static class StiTextInCellsHelper
{
	public static string TrimEndWhiteSpace(string inputString)
	{
		if (StiOptions.Engine.MeasureTrailingSpaces)
		{
			return inputString;
		}
		string result = string.Empty;
		int num = inputString.Length;
		while (num > 0 && char.IsWhiteSpace(inputString[num - 1]))
		{
			num--;
		}
		if (num == inputString.Length)
		{
			result = inputString;
		}
		else if (num > 0)
		{
			result = inputString.Substring(0, num);
		}
		return result;
	}
}
