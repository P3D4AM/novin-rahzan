using System.Text;

namespace Stimulsoft.Data.Helpers;

public static class StiHumanReadableHelper
{
	public static string GetSize(long size)
	{
		string[] array = new string[4] { "B", "KB", "MB", "GB" };
		int num = 0;
		while (size >= 1024 && ++num < array.Length)
		{
			size /= 1024;
		}
		return $"{size:0.##} {array[num]}";
	}

	public static string GetHumanReadableName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder(name.Trim());
		stringBuilder.Replace('_', ' ');
		for (int i = 1; i < stringBuilder.Length; i++)
		{
			char c = stringBuilder[i - 1];
			char c2 = stringBuilder[i];
			if (char.IsLetter(c) && char.IsLetter(c2) && char.IsLower(c) && char.IsUpper(c2))
			{
				stringBuilder.Insert(i, ' ');
				i++;
			}
		}
		for (int j = 1; j < stringBuilder.Length; j++)
		{
			char c3 = stringBuilder[j - 1];
			char c4 = stringBuilder[j];
			if ((char.IsLetter(c3) && char.IsDigit(c4)) || (char.IsDigit(c3) && char.IsLetter(c4)))
			{
				stringBuilder.Insert(j, ' ');
				j++;
			}
		}
		for (int k = 0; k < stringBuilder.Length; k++)
		{
			char c5 = stringBuilder[k];
			if (!char.IsLetterOrDigit(c5) && c5 != ' ')
			{
				stringBuilder.Remove(k, 1);
			}
		}
		if (stringBuilder.Length > 0 && !char.IsUpper(stringBuilder[0]))
		{
			stringBuilder[0] = stringBuilder[0].ToString().ToUpperInvariant()[0];
		}
		return stringBuilder.ToString();
	}
}
