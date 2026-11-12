using System.Text;

namespace Stimulsoft.Base;

public class StiDataNameValidator
{
	public static string Correct(string str)
	{
		StringBuilder stringBuilder = new StringBuilder(str);
		for (int i = 0; i < stringBuilder.Length; i++)
		{
			if (!char.IsLetterOrDigit(stringBuilder[i]) && stringBuilder[i] != '_')
			{
				stringBuilder[i] = '_';
			}
		}
		str = stringBuilder.ToString();
		if (str.Length > 0 && char.IsDigit(str[0]))
		{
			str = "n" + str;
		}
		return str;
	}
}
