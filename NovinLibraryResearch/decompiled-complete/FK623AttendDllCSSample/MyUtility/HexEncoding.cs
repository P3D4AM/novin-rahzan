using System;
using System.Globalization;

namespace MyUtility;

public class HexEncoding
{
	public static int GetByteCount(string hexString)
	{
		int num = 0;
		foreach (char c in hexString)
		{
			if (IsHexDigit(c))
			{
				num++;
			}
		}
		if (num % 2 != 0)
		{
			num--;
		}
		return num / 2;
	}

	public static byte[] GetBytes(string hexString, out int discarded)
	{
		discarded = 0;
		string text = "";
		for (int i = 0; i < hexString.Length; i++)
		{
			char c = hexString[i];
			if (IsHexDigit(c))
			{
				text += c;
			}
			else
			{
				discarded++;
			}
		}
		if (text.Length % 2 != 0)
		{
			discarded++;
			text = text.Substring(0, text.Length - 1);
		}
		int num = text.Length / 2;
		byte[] array = new byte[num];
		int num2 = 0;
		for (int j = 0; j < array.Length; j++)
		{
			string hex = new string(new char[2]
			{
				text[num2],
				text[num2 + 1]
			});
			array[j] = HexToByte(hex);
			num2 += 2;
		}
		return array;
	}

	public static string ToString(byte[] bytes)
	{
		string text = "";
		for (int i = 0; i < bytes.Length; i++)
		{
			text += bytes[i].ToString("X2");
		}
		return text;
	}

	public static bool InHexFormat(string hexString)
	{
		bool result = true;
		foreach (char c in hexString)
		{
			if (!IsHexDigit(c))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static bool IsHexDigit(char c)
	{
		int num = Convert.ToInt32('A');
		int num2 = Convert.ToInt32('0');
		c = char.ToUpper(c);
		int num3 = Convert.ToInt32(c);
		if (num3 >= num && num3 < num + 6)
		{
			return true;
		}
		if (num3 >= num2 && num3 < num2 + 10)
		{
			return true;
		}
		return false;
	}

	private static byte HexToByte(string hex)
	{
		if (hex.Length > 2 || hex.Length <= 0)
		{
			throw new ArgumentException("hex must be 1 or 2 characters in length");
		}
		return byte.Parse(hex, NumberStyles.HexNumber);
	}
}
