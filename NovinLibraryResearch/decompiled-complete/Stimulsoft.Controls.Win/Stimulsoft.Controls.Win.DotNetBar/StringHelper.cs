using System.Text;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal static class StringHelper
{
	internal static string Capitalize(string s)
	{
		if (s.Length > 1)
		{
			return s.Substring(0, 1).ToUpper() + s.Substring(1);
		}
		return s;
	}

	internal static string GetFriendlyName(string fieldName)
	{
		fieldName = fieldName.Replace('_', ' ');
		if (fieldName.Contains(" "))
		{
			string[] array = fieldName.Split(new char[1] { ' ' });
			StringBuilder stringBuilder = new StringBuilder(fieldName.Length);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(Capitalize(array[i]));
				if (i < array.Length - 1)
				{
					stringBuilder.Append(' ');
				}
			}
			return stringBuilder.ToString();
		}
		if (fieldName.Length > 2)
		{
			bool flag = false;
			int num = -1;
			StringBuilder stringBuilder2 = new StringBuilder(fieldName.Length + 10);
			for (int j = 0; j < fieldName.Length; j++)
			{
				if (IsUpperCase(fieldName[j]))
				{
					if (flag && j - num > 1)
					{
						stringBuilder2.Append(' ');
						stringBuilder2.Append(fieldName[j]);
						num = j;
					}
					else if (!flag)
					{
						num = j;
						flag = true;
						if (j > 0)
						{
							stringBuilder2.Append(' ');
						}
						stringBuilder2.Append(fieldName[j]);
					}
					else
					{
						num = j;
						stringBuilder2.Append(fieldName[j].ToString().ToLower());
						flag = true;
					}
				}
				else if (flag)
				{
					stringBuilder2.Append(fieldName[j]);
				}
				else
				{
					stringBuilder2.Append(fieldName[j].ToString().ToUpper());
					flag = true;
					num = j;
				}
			}
			return stringBuilder2.ToString();
		}
		return fieldName;
	}

	internal static bool IsUpperCase(char c)
	{
		return IsUpperCase(c.ToString());
	}

	internal static bool IsUpperCase(string s)
	{
		return s.ToUpper() == s;
	}

	internal static string Repeat(char repeatCharacter, int count)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(repeatCharacter, count);
		return stringBuilder.ToString();
	}
}
