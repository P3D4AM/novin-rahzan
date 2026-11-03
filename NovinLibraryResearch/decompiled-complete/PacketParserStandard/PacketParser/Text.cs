using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser;

public static class Text
{
	public static string DecimalPoint => TextManager.Instance.DecimalPoint;

	public static string decimalSeprator => TextManager.Instance.decimalSeprator;

	public static string fixNumberAndPutSharp(this string input)
	{
		return TextManager.Instance.fixNumberAndPutSharp(input);
	}

	public static string FixForTelegram(this string input, string replaceSpaceWith = "_")
	{
		return TextManager.Instance.FixForTelegram(input, replaceSpaceWith);
	}

	public static string fixForUrl(this string input)
	{
		return TextManager.Instance.fixForUrl(input);
	}

	public static string RemoveNoNumbers(this string input, string Replcment = "")
	{
		return TextManager.Instance.RemoveNoNumbers(input, Replcment);
	}

	public static string NpRemoveNoNumbers(this string input, string Replcment = "")
	{
		return TextManager.Instance.NpRemoveNoNumbers(input, Replcment);
	}

	public static bool IsValidEAN13Barcode(this string input)
	{
		if ((input?.Length ?? 0) == 13)
		{
			return input[12].ToString() == input.Checksum_EAN13Barcode().ToString();
		}
		return false;
	}

	public static string RemoveNumbers(this string input, string replacement)
	{
		return TextManager.Instance.RemoveNumbers(input, replacement);
	}

	public static string LimitLength(this string source, int maxLength)
	{
		try
		{
			if (source.Length <= maxLength)
			{
				return source;
			}
			return source.Substring(0, maxLength);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LimitLength", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\TextAndCalendar\\Text.cs", 28);
			return source;
		}
	}

	public static string FixSqlServerConnectionString(this string connectionStrung)
	{
		try
		{
			if (string.IsNullOrEmpty(connectionStrung))
			{
				return "";
			}
			if (!connectionStrung.ToLower().Contains("MultipleActiveResultSets=true".ToLower()))
			{
				connectionStrung += " ;MultipleActiveResultSets=true";
			}
			if (!connectionStrung.ToLower().Contains("TrustServerCertificate".ToLower()))
			{
				connectionStrung += " ;TrustServerCertificate=True";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixSqlServerConnectionString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\TextAndCalendar\\Text.cs", 42);
		}
		return connectionStrung;
	}

	public static List<string> SplitString(this string input, string spliterList = " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:")
	{
		return TextManager.Instance.SplitString(input, spliterList);
	}

	public static string IsCorrectName(this string Name, string title)
	{
		return TextManager.Instance.IsCorrectName(Name, title);
	}

	public static string FixString(this string input)
	{
		return TextManager.Instance.FixString(input);
	}

	public static string FixLength(this string input, int length)
	{
		return TextManager.Instance.FixLength(input, length);
	}

	public static string FixString(string input, bool sqlServer)
	{
		return TextManager.Instance.FixString(input, sqlServer);
	}

	public static bool IsPersianText(this string input)
	{
		return TextManager.Instance.IsPersianText(input);
	}

	public static string Negasht(this string input)
	{
		return TextManager.Instance.Negasht(input);
	}

	public static bool ISString(char Ch)
	{
		return TextManager.Instance.ISString(Ch);
	}

	public static bool ISNumber(char Ch)
	{
		return TextManager.Instance.ISNumber(Ch);
	}

	public static bool IsStringOrNumber(char ch)
	{
		return TextManager.Instance.IsStringOrNumber(ch);
	}

	public static string RemoveNoNumbers_Dot(string Input, string Replcment = "")
	{
		return TextManager.Instance.RemoveNoNumbers_Dot(Input, Replcment);
	}

	public static string Negasht(this char e)
	{
		return TextManager.Instance.Negasht(e);
	}

	public static bool StringComparisons(this string source, string searchString, StringComparison comparer = StringComparison.OrdinalIgnoreCase)
	{
		return TextManager.Instance.StringComparisons(source, searchString, comparer);
	}

	public static bool StringComparisons(this long source, string searchString, StringComparison comparer = StringComparison.OrdinalIgnoreCase)
	{
		return source.ToString().StringComparisons(searchString, comparer);
	}

	public static bool StringComparisons(this decimal source, string searchString, StringComparison comparer = StringComparison.OrdinalIgnoreCase)
	{
		return source.ToString().StringComparisons(searchString, comparer);
	}

	public static bool StringComparisons(this int source, string searchString, StringComparison comparer = StringComparison.OrdinalIgnoreCase)
	{
		return source.ToString().StringComparisons(searchString, comparer);
	}

	public static bool StringComparisons(this short source, string searchString, StringComparison comparer = StringComparison.OrdinalIgnoreCase)
	{
		return source.ToString().StringComparisons(searchString, comparer);
	}

	public static string WordPressFixName(this string name)
	{
		return TextManager.Instance.WordPressFixName(name);
	}

	public static string WordPressFixSlugOrSku(this string name, int? appendIndexToEndOfString = null)
	{
		return TextManager.Instance.WordPressFixSlugOrSku(name, appendIndexToEndOfString);
	}

	public static string UnicodeToCorrectString(this string data)
	{
		return TextManager.Instance.UnicodeToCorrectString(data);
	}

	public static decimal FixDecimal(this decimal value, short numberOfDigits = 18, short numberAfterPoint = 3)
	{
		return TextManager.Instance.FixDecimal(value, numberOfDigits, numberAfterPoint);
	}

	public static string GetClassName(this string sourceFilePath)
	{
		try
		{
			if (string.IsNullOrEmpty(sourceFilePath))
			{
				return "فاقد اسم فایل";
			}
			string text = sourceFilePath.Split(new char[1] { '\\' }).ToList().LastOrDefault();
			if (!string.IsNullOrEmpty(text))
			{
				return text.Split(new char[1] { '.' }).ToList().FirstOrDefault();
			}
			return "cannot find file path.sourceFilePath:" + sourceFilePath;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetClassName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\TextAndCalendar\\Text.cs", 81);
			return ex.Message;
		}
	}

	public static List<string> PrepareForIndexing(this string word)
	{
		return word?.ToLower()?.Replace("ـ", "")?.KeepPersianCharacters("1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZبپتثجچحخدذرزژسشصضطظعغفقکگلمنوهیءئ اآ /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:×აბგდევზთიკლმნოპრსტუფქღყშჩცძწჭხჯჰ")?.FixString()?.SplitString(" \\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:")?.Where((string p) => !string.IsNullOrEmpty(p))?.Distinct()?.ToList();
	}
}
