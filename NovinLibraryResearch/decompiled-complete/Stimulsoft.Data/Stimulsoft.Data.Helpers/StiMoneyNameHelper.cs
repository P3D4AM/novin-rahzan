namespace Stimulsoft.Data.Helpers;

public static class StiMoneyNameHelper
{
	public static bool IsMoneyName(string name)
	{
		if (!string.IsNullOrWhiteSpace(name))
		{
			if (!name.StartsWith("Price") && !name.StartsWith("Cost") && !name.StartsWith("Rate") && !name.EndsWith("Price") && !name.EndsWith("Cost"))
			{
				return name.EndsWith("Rate");
			}
			return true;
		}
		return false;
	}
}
