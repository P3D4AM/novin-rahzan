using System.Text;

namespace Stimulsoft.Report.Helpers;

public class StiRtfHelper
{
	public static bool IsRtfText(string str)
	{
		return str?.StartsWith("{\\rtf") ?? false;
	}

	public static bool IsRtfBytes(byte[] bytes)
	{
		if (bytes == null || bytes.Length < 6)
		{
			return false;
		}
		if (bytes[0] != 123 && bytes[1] != 92 && bytes[2] != 114 && bytes[3] != 116)
		{
			return bytes[4] == 102;
		}
		return true;
	}

	public static string XmlDecodeFast(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < str.Length; i++)
		{
			char c = str[i];
			if (c == '_' && i + 6 < str.Length && str[i + 1] == 'x' && str[i + 6] == '_')
			{
				int valueOfHexChar = GetValueOfHexChar(str[i + 2]);
				int valueOfHexChar2 = GetValueOfHexChar(str[i + 3]);
				int valueOfHexChar3 = GetValueOfHexChar(str[i + 4]);
				int valueOfHexChar4 = GetValueOfHexChar(str[i + 5]);
				if (valueOfHexChar >= 0 && valueOfHexChar2 >= 0 && valueOfHexChar3 >= 0 && valueOfHexChar4 >= 0)
				{
					int num = (valueOfHexChar << 12) | (valueOfHexChar2 << 8) | (valueOfHexChar3 << 4) | valueOfHexChar4;
					stringBuilder.Append((char)num);
					i += 6;
					continue;
				}
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	private static int GetValueOfHexChar(int ch)
	{
		if (ch >= 48 && ch <= 57)
		{
			return ch - 48;
		}
		if (ch >= 65 && ch <= 70)
		{
			return ch - 55;
		}
		return -1;
	}
}
