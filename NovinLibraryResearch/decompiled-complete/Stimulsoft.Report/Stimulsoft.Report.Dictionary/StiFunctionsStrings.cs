using System;
using System.Globalization;
using System.Text;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiFunctionsStrings
{
	private const string tC_0 = "ศ\u0e39นย\u0e4c";

	private const string tC_1 = "หน\u0e36\u0e48ง";

	private const string tC_2 = "สอง";

	private const string tC_3 = "สาม";

	private const string tC_4 = "ส\u0e35\u0e48";

	private const string tC_5 = "ห\u0e49า";

	private const string tC_6 = "หก";

	private const string tC_7 = "เจ\u0e47ด";

	private const string tC_8 = "แปด";

	private const string tC_9 = "เก\u0e49า";

	private const string tC_01 = "เอ\u0e47ด";

	private const string tC_10 = "ส\u0e34บ";

	private const string tC_20 = "ย\u0e35\u0e48";

	private const string tC_100 = "ร\u0e49อย";

	private const string tC_1000 = "พ\u0e31น";

	private const string tC_10000 = "หม\u0e37\u0e48น";

	private const string tC_100000 = "แสน";

	private const string tC_1000000 = "ล\u0e49าน";

	private const string tC_Baht = "บาท";

	private const string tC_Satang = "สตางค\u0e4c";

	private const string tC_Complete = "ถ\u0e49วน";

	public static int Length(object str)
	{
		if (str == null || str == DBNull.Value)
		{
			return 0;
		}
		return str.ToString().Length;
	}

	public static string Trim(object str)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		return str.ToString().Trim();
	}

	public static string TrimStart(object str)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		return str.ToString().TrimStart(new char[0]);
	}

	public static string TrimEnd(object str)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		return str.ToString().TrimEnd(new char[0]);
	}

	public static string ToLowerCase(object str)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		return str.ToString().ToLower(CultureInfo.InvariantCulture);
	}

	public static string ToUpperCase(object str)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		return str.ToString().ToUpper(CultureInfo.InvariantCulture);
	}

	public static string ToProperCase(object str)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str.ToString().ToLowerInvariant());
	}

	public static string Substring(object str, int startIndex, int length)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		string text = str.ToString();
		if (startIndex + length >= text.Length)
		{
			return text.Substring(startIndex);
		}
		return text.Substring(startIndex, length);
	}

	public static string Left(object str, int length)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		string text = str.ToString();
		if (length >= text.Length)
		{
			return text;
		}
		return text.Substring(0, length);
	}

	public static string Right(object str, int length)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		string text = str.ToString();
		if (length >= text.Length)
		{
			return text;
		}
		return text.Substring(text.Length - length, length);
	}

	public static string Mid(object str, int startIndex, int length)
	{
		return Substring(str, startIndex, length);
	}

	public static string Roman(int value)
	{
		return Func.Convert.ToRoman(value);
	}

	public static string ABC(int value)
	{
		return Func.Convert.ToABC(value);
	}

	public static string Arabic(int value)
	{
		return Func.Convert.ToArabic(value, useEasternDigits: false);
	}

	public static string Arabic(string value)
	{
		return Func.Convert.ToArabic(value, useEasternDigits: false);
	}

	public static string Persian(int value)
	{
		return Func.Convert.ToArabic(value, useEasternDigits: true);
	}

	public static string Persian(string value)
	{
		return Func.Convert.ToArabic(value, useEasternDigits: true);
	}

	public static bool IsNumeric(object str)
	{
		if (str == null || str == DBNull.Value)
		{
			return false;
		}
		double result;
		return double.TryParse(str.ToString(), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result);
	}

	public static string Replace(object str, object oldValue, object newValue)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		if (oldValue == null || oldValue == DBNull.Value)
		{
			return str.ToString();
		}
		if (newValue == null || newValue == DBNull.Value)
		{
			return str.ToString();
		}
		return str.ToString().Replace(oldValue.ToString(), newValue.ToString());
	}

	public static string Remove(object str, int startIndex, int count)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		return str.ToString().Remove(startIndex, count);
	}

	public static string Remove(object str, int startIndex)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		return str.ToString().Substring(0, startIndex);
	}

	public static string Insert(object str, int startIndex, object value)
	{
		if (str == null || str == DBNull.Value)
		{
			return string.Empty;
		}
		if (value == null || value == DBNull.Value)
		{
			return str.ToString();
		}
		return str.ToString().Insert(startIndex, value.ToString());
	}

	public static bool TryParseDateTime(string value)
	{
		DateTime dateTime = default(DateTime);
		return StringExt.TryParseDateTime(value, ref dateTime);
	}

	public static bool TryParseDecimal(string value)
	{
		decimal result;
		return decimal.TryParse(value, out result);
	}

	public static bool TryParseDouble(string value)
	{
		double result;
		return double.TryParse(value, out result);
	}

	public static bool TryParseLong(string value)
	{
		long result;
		return long.TryParse(value, out result);
	}

	public static string ToWords(long value)
	{
		return Func.En.NumToStr(value);
	}

	public static string ToWords(decimal value)
	{
		return Func.En.NumToStr(value);
	}

	public static string ToWords(double value)
	{
		return Func.En.NumToStr(value);
	}

	public static string ToWords(long value, bool upperCase)
	{
		return Func.En.NumToStr(value, upperCase);
	}

	public static string ToWords(decimal value, bool upperCase)
	{
		return Func.En.NumToStr(value, upperCase);
	}

	public static string ToWords(double value, bool upperCase)
	{
		return Func.En.NumToStr(value, upperCase);
	}

	public static string ToWords(long? value)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return Func.En.NumToStr(value.Value);
	}

	public static string ToWords(decimal? value)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return Func.En.NumToStr(value.Value);
	}

	public static string ToWords(double? value)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return Func.En.NumToStr(value.Value);
	}

	public static string ToWords(long? value, bool? upperCase)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		return Func.En.NumToStr(value.Value, upperCase.Value);
	}

	public static string ToWords(decimal? value, bool? upperCase)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		return Func.En.NumToStr(value.Value, upperCase.Value);
	}

	public static string ToWords(double? value, bool? upperCase)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		return Func.En.NumToStr(value.Value, upperCase.Value);
	}

	public static string DateToStr(DateTime value)
	{
		return Func.En.DateToStr(value);
	}

	public static string DateToStr(DateTime value, bool upperCase)
	{
		return Func.En.DateToStr(value, upperCase);
	}

	public static string DateToStr(DateTime? value)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return Func.En.DateToStr(value.Value);
	}

	public static string DateToStr(DateTime? value, bool upperCase)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return Func.En.DateToStr(value.Value, upperCase);
	}

	public static string ToCurrencyWords(long value)
	{
		return Func.En.CurrToStr(value);
	}

	public static string ToCurrencyWords(decimal value)
	{
		return Func.En.CurrToStr(value);
	}

	public static string ToCurrencyWords(double value)
	{
		return Func.En.CurrToStr(value);
	}

	public static string ToCurrencyWords(long value, bool upperCase)
	{
		return Func.En.CurrToStr(value, upperCase);
	}

	public static string ToCurrencyWords(decimal value, bool upperCase)
	{
		return Func.En.CurrToStr(value, upperCase);
	}

	public static string ToCurrencyWords(double value, bool upperCase)
	{
		return Func.En.CurrToStr(value, upperCase);
	}

	public static string ToCurrencyWords(long value, bool upperCase, bool showCents)
	{
		return Func.En.CurrToStr(value, upperCase, showCents);
	}

	public static string ToCurrencyWords(decimal value, bool upperCase, bool showCents)
	{
		return Func.En.CurrToStr(value, upperCase, showCents);
	}

	public static string ToCurrencyWords(double value, bool upperCase, bool showCents)
	{
		return Func.En.CurrToStr(value, upperCase, showCents);
	}

	public static string ToCurrencyWords(double value, bool upperCase, bool showCents, string dollars, string cents)
	{
		return Func.En.CurrToStr(value, upperCase, showCents, dollars, cents);
	}

	public static string ToCurrencyWords(long value, bool upperCase, bool showCents, string dollars, string cents)
	{
		return Func.En.CurrToStr(value, upperCase, showCents, dollars, cents);
	}

	public static string ToCurrencyWords(decimal value, bool upperCase, bool showCents, string dollars, string cents)
	{
		return Func.En.CurrToStr(value, upperCase, showCents, dollars, cents);
	}

	public static string ToCurrencyWords(long? value)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return Func.En.CurrToStr(value.Value);
	}

	public static string ToCurrencyWords(decimal? value)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return Func.En.CurrToStr(value.Value);
	}

	public static string ToCurrencyWords(double? value)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		return Func.En.CurrToStr(value.Value);
	}

	public static string ToCurrencyWords(long? value, bool? upperCase)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value);
	}

	public static string ToCurrencyWords(decimal? value, bool? upperCase)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value);
	}

	public static string ToCurrencyWords(double? value, bool? upperCase)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value);
	}

	public static string ToCurrencyWords(long? value, bool? upperCase, bool? showCents)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		if (!showCents.HasValue)
		{
			showCents = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value, showCents.Value);
	}

	public static string ToCurrencyWords(decimal? value, bool? upperCase, bool? showCents)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		if (!showCents.HasValue)
		{
			showCents = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value, showCents.Value);
	}

	public static string ToCurrencyWords(double? value, bool? upperCase, bool? showCents)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		if (!showCents.HasValue)
		{
			showCents = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value, showCents.Value);
	}

	public static string ToCurrencyWords(double? value, bool? upperCase, bool? showCents, string dollars, string cents)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		if (!showCents.HasValue)
		{
			showCents = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value, showCents.Value, dollars, cents);
	}

	public static string ToCurrencyWords(long? value, bool? upperCase, bool? showCents, string dollars, string cents)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		if (!showCents.HasValue)
		{
			showCents = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value, showCents.Value, dollars, cents);
	}

	public static string ToCurrencyWords(decimal? value, bool? upperCase, bool? showCents, string dollars, string cents)
	{
		if (!value.HasValue)
		{
			return string.Empty;
		}
		if (!upperCase.HasValue)
		{
			upperCase = true;
		}
		if (!showCents.HasValue)
		{
			showCents = true;
		}
		return Func.En.CurrToStr(value.Value, upperCase.Value, showCents.Value, dollars, cents);
	}

	public static string ToWordsRu(long value)
	{
		return Func.Ru.NumToStr(value);
	}

	public static string ToWordsRu(decimal value)
	{
		return Func.Ru.NumToStr(value);
	}

	public static string ToWordsRu(double value)
	{
		return Func.Ru.NumToStr(value);
	}

	public static string ToWordsRu(long value, bool upperCase)
	{
		return Func.Ru.NumToStr(value, upperCase);
	}

	public static string ToWordsRu(decimal value, bool upperCase)
	{
		return Func.Ru.NumToStr(value, upperCase);
	}

	public static string ToWordsRu(double value, bool upperCase)
	{
		return Func.Ru.NumToStr(value, upperCase);
	}

	public static DateTime StrToDateTime(string value)
	{
		DateTime result = default(DateTime);
		if (!StringExt.TryParseDateTime(value, ref result))
		{
			return DateTime.Now;
		}
		return result;
	}

	public static DateTime? StrToNullableDateTime(string value)
	{
		DateTime value2 = default(DateTime);
		if (!StringExt.TryParseDateTime(value, ref value2))
		{
			return null;
		}
		return value2;
	}

	public static string DateToStrRu(DateTime value)
	{
		return Func.Ru.DateToStr(value);
	}

	public static string DateToStrRu(DateTime value, bool upperCase)
	{
		return Func.Ru.DateToStr(value, upperCase);
	}

	public static string ToCurrencyWordsRu(long value)
	{
		return Func.Ru.CurrToStr((decimal)value);
	}

	public static string ToCurrencyWordsRu(decimal value)
	{
		return Func.Ru.CurrToStr(value);
	}

	public static string ToCurrencyWordsRu(double value)
	{
		return Func.Ru.CurrToStr(value);
	}

	public static string ToCurrencyWordsRu(long value, bool upperCase)
	{
		return Func.Ru.CurrToStr(value, upperCase);
	}

	public static string ToCurrencyWordsRu(decimal value, bool upperCase)
	{
		return Func.Ru.CurrToStr(value, upperCase);
	}

	public static string ToCurrencyWordsRu(double value, bool upperCase)
	{
		return Func.Ru.CurrToStr(value, upperCase);
	}

	public static string ToCurrencyWordsRu(decimal value, bool showCents, string currency)
	{
		return Func.Ru.CurrToStr(value, currency, showCents);
	}

	public static string ToCurrencyWordsRu(double value, bool showCents, string currency)
	{
		return Func.Ru.CurrToStr(value, currency, showCents);
	}

	public static string ToCurrencyWordsRu(long value, bool showCents, string currency)
	{
		return Func.Ru.CurrToStr(value, currency, showCents);
	}

	public static string ToCurrencyWordsRu(decimal value, string currency, bool upperCase)
	{
		return Func.Ru.CurrToStr(value, upperCase, currency);
	}

	public static string ToCurrencyWordsRu(double value, string currency, bool upperCase)
	{
		return Func.Ru.CurrToStr(value, upperCase, currency);
	}

	public static string ToCurrencyWordsRu(long value, string currency, bool upperCase)
	{
		return Func.Ru.CurrToStr(value, upperCase, currency);
	}

	public static string ToCurrencyWordsThai(long value)
	{
		return SP_STRtNumToMny(value);
	}

	public static string ToCurrencyWordsThai(double value)
	{
		return SP_STRtNumToMny((decimal)value);
	}

	public static string ToCurrencyWordsThai(decimal value)
	{
		return SP_STRtNumToMny(value);
	}

	private static string SP_STRtNumToMny(decimal value)
	{
		string text = string.Empty;
		string empty = string.Empty;
		string[] array = null;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string result = string.Empty;
		string empty4 = string.Empty;
		empty4 = Math.Round(value, 2).ToString();
		array = empty4.Split(new char[1] { '.' });
		if (array.GetUpperBound(0) > 1)
		{
			return "";
		}
		empty = array[0];
		if (array.GetUpperBound(0) > 0)
		{
			text = array[1];
			if (text.Length > 2)
			{
				return "";
			}
		}
		empty2 = (string.IsNullOrEmpty(empty) ? "ศ\u0e39นย\u0e4c" : SP_XCGtNumToMny(empty));
		if (!string.IsNullOrEmpty(text))
		{
			switch (text.Length)
			{
			case 1:
				text += "0";
				break;
			default:
				text = text.Remove(2);
				break;
			case 2:
				break;
			}
			empty3 = SP_XCGtNumToMny(text);
		}
		else
		{
			empty3 = "ศ\u0e39นย\u0e4c";
		}
		if (empty2 == "ศ\u0e39นย\u0e4c" && empty3 == "ศ\u0e39นย\u0e4c")
		{
			result = empty2 + "บาทถ\u0e49วน";
		}
		else if (empty2 == "ศ\u0e39นย\u0e4c" && empty3 != "ศ\u0e39นย\u0e4c")
		{
			result = empty3 + "สตางค\u0e4c";
		}
		else if (empty2 != "ศ\u0e39นย\u0e4c" && empty3 == "ศ\u0e39นย\u0e4c")
		{
			result = empty2 + "บาทถ\u0e49วน";
		}
		else if (empty2 != "ศ\u0e39นย\u0e4c" && empty3 != "ศ\u0e39นย\u0e4c")
		{
			result = empty2 + "บาท" + empty3 + "สตางค\u0e4c";
		}
		return result;
	}

	private static string SP_XCGtNumToMny(string ptMoneyNum)
	{
		string text = ptMoneyNum;
		if (string.IsNullOrEmpty(ptMoneyNum))
		{
			return "";
		}
		while (text[0] == '0' && text.Length > 1)
		{
			text = text.Remove(1, 1);
		}
		text = ReverseString(text);
		int num = 0;
		string text2 = "";
		char[] array = text.ToCharArray();
		for (num = array.GetLowerBound(0); num <= array.GetUpperBound(0); num++)
		{
			string text3 = "";
			string text4 = "";
			int num2 = 0;
			num2 = num % 6;
			switch (array[num])
			{
			case '0':
				text4 = "";
				break;
			case '1':
				text4 = num2 switch
				{
					0 => (array.GetUpperBound(0) % 6 != 0 || num != array.GetUpperBound(0)) ? "เอ\u0e47ด" : "หน\u0e36\u0e48ง", 
					1 => "ส\u0e34บ", 
					_ => "หน\u0e36\u0e48ง", 
				};
				break;
			case '2':
				text4 = ((num2 != 1) ? "สอง" : "ย\u0e35\u0e48");
				break;
			case '3':
				text4 = "สาม";
				break;
			case '4':
				text4 = "ส\u0e35\u0e48";
				break;
			case '5':
				text4 = "ห\u0e49า";
				break;
			case '6':
				text4 = "หก";
				break;
			case '7':
				text4 = "เจ\u0e47ด";
				break;
			case '8':
				text4 = "แปด";
				break;
			case '9':
				text4 = "เก\u0e49า";
				break;
			}
			if (!string.IsNullOrEmpty(text4) || num % 6 == 0)
			{
				switch (num2)
				{
				case 0:
					text3 = ((num / 6 > 0) ? "ล\u0e49าน" : "");
					break;
				case 1:
					text3 = ((!(text4 == "ส\u0e34บ")) ? "ส\u0e34บ" : "");
					break;
				case 2:
					text3 = "ร\u0e49อย";
					break;
				case 3:
					text3 = "พ\u0e31น";
					break;
				case 4:
					text3 = "หม\u0e37\u0e48น";
					break;
				case 5:
					text3 = "แสน";
					break;
				}
			}
			text2 = text4 + text3 + text2;
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "ศ\u0e39นย\u0e4c";
		}
		return text2;
	}

	private static string ReverseString(string s)
	{
		char[] array = s.ToCharArray();
		Array.Reverse((Array)array);
		return new string(array);
	}

	public static string ToWordsUa(long value)
	{
		return Func.Ua.NumToStr(value);
	}

	public static string ToWordsUa(decimal value)
	{
		return Func.Ua.NumToStr(value);
	}

	public static string ToWordsUa(double value)
	{
		return Func.Ua.NumToStr(value);
	}

	public static string ToWordsUa(long value, bool upperCase)
	{
		return Func.Ua.NumToStr(value, upperCase);
	}

	public static string ToWordsUa(decimal value, bool upperCase)
	{
		return Func.Ua.NumToStr(value, upperCase);
	}

	public static string ToWordsUa(double value, bool upperCase)
	{
		return Func.Ua.NumToStr(value, upperCase);
	}

	public static string DateToStrUa(DateTime value)
	{
		return Func.Ua.DateToStr(value);
	}

	public static string DateToStrUa(DateTime value, bool upperCase)
	{
		return Func.Ua.DateToStr(value, upperCase);
	}

	public static string ToCurrencyWordsUa(long value)
	{
		return Func.Ua.CurrToStr((decimal)value);
	}

	public static string ToCurrencyWordsUa(decimal value)
	{
		return Func.Ua.CurrToStr(value);
	}

	public static string ToCurrencyWordsUa(double value)
	{
		return Func.Ua.CurrToStr(value);
	}

	public static string ToCurrencyWordsUa(long value, bool cents)
	{
		return Func.Ua.CurrToStr(value, cents);
	}

	public static string ToCurrencyWordsUa(decimal value, bool cents)
	{
		return Func.Ua.CurrToStr(value, cents);
	}

	public static string ToCurrencyWordsUa(double value, bool cents)
	{
		return Func.Ua.CurrToStr(value, cents);
	}

	public static string ToCurrencyWordsUa(decimal value, bool cents, string currency)
	{
		return Func.Ua.CurrToStr(value, currency, cents);
	}

	public static string ToCurrencyWordsUa(double value, bool cents, string currency)
	{
		return Func.Ua.CurrToStr(value, currency, cents);
	}

	public static string ToCurrencyWordsUa(long value, bool cents, string currency)
	{
		return Func.Ua.CurrToStr(value, currency, cents);
	}

	public static string ToWordsPt(long value, bool upperCase)
	{
		return Func.Pt.NumToStr(value, upperCase);
	}

	public static string ToCurrencyWordsPt(decimal value, bool upperCase, bool showCents)
	{
		return Func.Pt.CurrToStr(value, upperCase, showCents);
	}

	public static string ToCurrencyWordsPt(decimal value, bool upperCase, bool showCents, string dollars, string cents)
	{
		return Func.Pt.CurrToStr(value, upperCase, showCents, dollars, cents);
	}

	public static string ToCurrencyWordsPt(decimal value, bool upperCase, bool showCents, string iso, int decimals)
	{
		return Func.Pt.CurrToStr(value, upperCase, showCents, iso, decimals);
	}

	public static string ToCurrencyWordsPtBr(decimal value)
	{
		return Func.PtBr.NumToStr(value);
	}

	public static string ToCurrencyWordsPtBr(decimal value, bool uppercase, string dollars, string cents)
	{
		return Func.PtBr.NumToStr(value, uppercase, dollars, cents);
	}

	public static string DateToStrPt(DateTime value)
	{
		return Func.Pt.DateToStr(value);
	}

	public static string DateToStrPtBr(DateTime value)
	{
		return Func.Pt.DateToStr(value).ToLowerInvariant();
	}

	public static string ToCurrencyWordsFr(decimal number, string currencyISO, int decimals)
	{
		return Func.Fr.ConvertToWord(number, currencyISO, decimals);
	}

	public static string ToCurrencyWordsEs(decimal number, string currencyISO, int decimals)
	{
		return Func.Es.ConvertToWord(number, currencyISO, decimals);
	}

	public static string ToCurrencyWordsEs(decimal number, string currencyISO, int decimals, bool female)
	{
		return Func.Es.ConvertToWord(number, currencyISO, decimals, female);
	}

	public static string ToWordsEs(long value, bool upperCase)
	{
		return Func.Es.NumToStr(value, upperCase);
	}

	public static string ToWordsEs(long value, bool upperCase, bool female)
	{
		return Func.Es.NumToStr(value, upperCase, female);
	}

	public static string ToCurrencyWordsNl(decimal number, string currencyISO, int decimals)
	{
		return Func.Nl.ConvertToWord(number, currencyISO, decimals);
	}

	public static string ToCurrencyWordsEnGb(decimal number, string currencyISO, int decimals)
	{
		return Func.EnGb.ConvertToWord(number, currencyISO, decimals);
	}

	public static string ToWordsPl(long value, bool upperCase)
	{
		return Func.Pl.NumToStr(value, upperCase);
	}

	public static string DateToStrPl(DateTime value, bool upperCase)
	{
		return Func.Pl.DateToStr(value, upperCase);
	}

	public static string ToCurrencyWordsPl(decimal value, string currencyISO, bool showCents, bool upperCase)
	{
		return Func.Pl.CurrToStr(value, currencyISO, showCents, upperCase);
	}

	public static string ToWordsEnIn(long value, bool blankIfZero)
	{
		return Func.EnIn.NumberToStr(value, blankIfZero);
	}

	public static string ToCurrencyWordsEnIn(string currencyBasicUnit, string currencyFractionalUnit, decimal value, int decimalPlaces, bool blankIfZero = false)
	{
		return Func.EnIn.CurrencyToStr(currencyBasicUnit, currencyFractionalUnit, value, decimalPlaces, blankIfZero);
	}

	public static string ToWordsFa(long value)
	{
		return Func.Fa.ConvertToWord(value);
	}

	public static string ToWordsAr(double value)
	{
		return Func.Ar.NumToStr(value, "", "").Trim();
	}

	public static string ToCurrencyWordsAr(double value, string currencyBasicUnit, string currencyFractionalUnit)
	{
		return Func.Ar.NumToStr(value, currencyBasicUnit, currencyFractionalUnit);
	}

	public static string ToWordsZh(decimal value)
	{
		return Func.Zh.ToWordsZh(value);
	}

	public static string ToCurrencyWordsZh(decimal value)
	{
		return Func.Zh.ToCurrencyWordsZh(value);
	}

	public static string ToWordsTr(decimal value)
	{
		return Func.Tr.NumToStr(value);
	}

	public static string ToCurrencyWordsTr(decimal value)
	{
		return Func.Tr.CurrToStr(value);
	}

	public static string ToCurrencyWordsTr(decimal value, string currencyName, bool showZeroCents)
	{
		return Func.Tr.CurrToStr(value, currencyName, showZeroCents);
	}

	public static string ToOrdinal(long value)
	{
		string text = value.ToString();
		if (value < 1)
		{
			return text;
		}
		value %= 100;
		if (value >= 11 && value <= 13)
		{
			return text + "th";
		}
		long num = value % 10;
		long num2 = num - 1;
		if ((ulong)num2 <= 2uL)
		{
			switch (num2)
			{
			case 0L:
				return text + "st";
			case 1L:
				return text + "nd";
			case 2L:
				return text + "rd";
			}
		}
		return text + "th";
	}

	public static string ConvertToBase64String(string value)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
	}
}
