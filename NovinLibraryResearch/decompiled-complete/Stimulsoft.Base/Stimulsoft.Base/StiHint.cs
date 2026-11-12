namespace Stimulsoft.Base;

public class StiHint
{
	public static string CreateHint(string hint)
	{
		return hint.Replace("...", string.Empty).Replace("&", string.Empty).Replace(":", string.Empty);
	}
}
