using System.Linq;

namespace Stimulsoft.Base;

public static class StiUrl
{
	public static string Combine(params string[] uriParts)
	{
		string text = string.Empty;
		if (uriParts != null && uriParts.Any())
		{
			char[] trimChars = new char[2] { '\\', '/' };
			text = (uriParts[0] ?? string.Empty).TrimEnd(trimChars);
			for (int i = 1; i < uriParts.Length; i++)
			{
				text = text.TrimEnd(trimChars) + "/" + (uriParts[i] ?? string.Empty).TrimStart(trimChars);
			}
		}
		return text;
	}
}
