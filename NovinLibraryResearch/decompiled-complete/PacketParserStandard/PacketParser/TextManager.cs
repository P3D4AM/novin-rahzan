using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser;

public class TextManager
{
	private string _DecimalPoint = "";

	private string _decimalSeprator = "";

	public static TextManager Instance { get; private set; } = new TextManager();


	public string DecimalPoint
	{
		get
		{
			try
			{
				decimal result = default(decimal);
				if (!decimal.TryParse("0.1", out result))
				{
					_DecimalPoint = 0.1.ToString().Substring(1, 1);
				}
				else if (result.ToString() == "0.1")
				{
					_DecimalPoint = ".";
				}
				else
				{
					_DecimalPoint = 0.1.ToString().Substring(1, 1);
				}
				return _DecimalPoint;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DecimalPoint", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 415);
				return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			}
		}
	}

	public string decimalSeprator
	{
		get
		{
			try
			{
				if (!string.IsNullOrEmpty(_decimalSeprator))
				{
					return _decimalSeprator;
				}
				string text = 1000m.NpToString("N0", 0);
				_decimalSeprator = text.Substring(1, 1);
				return _decimalSeprator;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "decimalSeprator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 438);
				return CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
			}
		}
	}

	public string FixLength(string str, int length)
	{
		try
		{
			if (string.IsNullOrEmpty(str))
			{
				return "";
			}
			if (str.Length > length)
			{
				str = str.Remove(length, checked(str.Length - length));
			}
			return str;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixLength", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 26);
			return "";
		}
	}

	public string fixNumberAndPutSharp(string input)
	{
		try
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			input = RemoveNoNumbers(FixString(input));
			while (input.StartsWith("0"))
			{
				input = input.Substring(1, checked(input.Length - 1));
			}
			if (input.Length == 8)
			{
				input = "51" + input;
			}
			if (input.Length == 10)
			{
				return "#T" + input;
			}
			return "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "fixNumberAndPutSharp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 47);
			return "";
		}
	}

	public string WordPressFixName(string value)
	{
		return value?.FixString()?.Replace("'", " ")?.Replace("\\", " ")?.Replace(",", " ")?.Replace("+", " ")?.Replace("\r", " ")?.Replace("\n", " ")?.Replace("<", " ")?.Replace(">", " ")?.Replace("]", " ")?.Replace("[", " ")?.Replace("{", " ")?.Replace("#", " ")?.Replace("}", " ");
	}

	public string WordPressFixSlugOrSku(string value, int? appendIndexToEndOfString = null)
	{
		checked
		{
			try
			{
				if (string.IsNullOrEmpty(value))
				{
					return "";
				}
				value = value.TrimStart(Array.Empty<char>()).TrimEnd(Array.Empty<char>()).Replace("'", "-")
					.Replace("\\", "-")
					.Replace(" ", "-")
					.Replace(",", "-")
					.Replace("+", "-")
					.Replace("\r", "-")
					.Replace("\n", "-")
					.Replace("<", "-")
					.Replace(">", "-")
					.Replace("]", "-")
					.Replace("[", "-")
					.Replace("{", "-")
					.Replace("}", "-")
					.Replace("،", "-")
					.Replace(".", "-")
					.Replace("(", "-")
					.Replace(")", "-")
					.Replace("ـ", "")
					.KeepCharacters("1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_آابپتثجچحخدذرزسشصضطظعغفقکگلمنوهیءئ");
				while (value.StartsWith("-"))
				{
					if (value.Length > 1)
					{
						value = value.Substring(1, value.Length - 1);
					}
					else if (value.Length == 1)
					{
						return "";
					}
				}
				while (value.StartsWith("-"))
				{
					if (value.Length > 1)
					{
						value = value.Substring(1, value.Length - 1);
					}
				}
				while (value.Contains("__"))
				{
					value = value.Replace("__", "_");
				}
				while (value.Contains("--"))
				{
					value = value.Replace("--", "-");
				}
				if (value.EndsWith("_"))
				{
					value = value.Substring(0, value.Length - 1);
				}
				if (value.EndsWith("-"))
				{
					value = value.Substring(0, value.Length - 1);
				}
				if (value.StartsWith("_"))
				{
					value = value.Substring(1, value.Length - 1);
				}
				if (value.StartsWith("-"))
				{
					value = value.Substring(1, value.Length - 1);
				}
				if (value.Length > 40)
				{
					foreach (char item in "ارزژدذو0123456789".ToList())
					{
						if (value.Length > 40)
						{
							value = value.Replace($"{item}-", item.ToString());
						}
					}
				}
				if (value.Length > 40)
				{
					value = value.Replace("-و-", "");
				}
				if (value.Length > 40)
				{
					value = value.Substring(0, 40);
				}
				if (value.EndsWith("_"))
				{
					value = value.Substring(0, value.Length - 1);
				}
				if (appendIndexToEndOfString.GetValueOrDefault() > 0)
				{
					if (value.Length + appendIndexToEndOfString.ToString().Length + 1 > 40)
					{
						value = value.Substring(0, 40 - (appendIndexToEndOfString.ToString().Length + 1));
					}
					if (!value.EndsWith("-"))
					{
						value += "-";
					}
					value = $"{value}{appendIndexToEndOfString}";
				}
				return value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, value, "WordPressFixSlugOrSku", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 140);
				return value;
			}
		}
	}

	public string UnicodeToCorrectString(string data)
	{
		int num = 0;
		try
		{
			if (string.IsNullOrEmpty(data))
			{
				return "";
			}
			num = 1;
			IEnumerable<string> enumerable = (from m in Regex.Matches(data, "\\\\u([a-fA-F\\d]{4})").OfType<Match>()
				select m.Groups[0].Value).Distinct();
			num = 2;
			string text = data;
			num = 3;
			foreach (string item in enumerable)
			{
				try
				{
					num = 5;
					string newValue = ((char)Convert.ToUInt16(item.Replace("\\u", ""), 16)).ToString();
					num = 6;
					text = text.Replace(item, newValue);
					num = 7;
				}
				catch (Exception ex)
				{
					IEnumerable<string> values = from p in item.Split()
						select "(" + string.Join("-", Encoding.ASCII.GetBytes(p)) + ")\r\n";
					WebErrorLog.Instance.StartLog(ex, string.Format("Line:{0},Part:{1},partBytes:{2},data:{3}", num, item, string.Join(",", values), data), "UnicodeToCorrectString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 175);
				}
			}
			return text;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{num},data:{data}", "UnicodeToCorrectString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 182);
			return data;
		}
	}

	public string FindKeyFromJsonText(string JsonText, string FindKey)
	{
		string text = "";
		try
		{
			text = JsonConvert.DeserializeObject<Dictionary<string, string>>(UnicodeToCorrectString(JsonText)).FirstOrDefault((KeyValuePair<string, string> p) => p.Key == FindKey).Value ?? "";
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FindKeyFromJsonText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 197);
		}
		return text;
	}

	public string FixForTelegram(string input, string replaceSpaceWith = "_")
	{
		int num = 0;
		try
		{
			if (string.IsNullOrEmpty(input) || (input?.Length ?? (-1)) <= 0)
			{
				return "";
			}
			num = 1;
			input = input.Trim().TrimStart(new char[1] { '#' }).Replace("#", replaceSpaceWith)
				.Replace(" ", replaceSpaceWith)
				.Replace(replaceSpaceWith + replaceSpaceWith, replaceSpaceWith);
			num = 2;
			input = input.Replace("-", "_").Replace(".", "_");
			num = 300;
			if (Enumerable.Contains("@#$%^&*-=+/\\.,~()_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZابپتثجچحخدذرزسشصضطظعغفقکگلمنوهیآ", input.FirstOrDefault()))
			{
				return "#" + input;
			}
			return "#A_" + input;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}//input:{input}//InputLen:{input?.Length ?? (-1)}//replaceSpaceWith:{replaceSpaceWith}", "FixForTelegram", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 219);
			return "";
		}
	}

	public string fixForUrl(string input)
	{
		try
		{
			if (string.IsNullOrEmpty(input))
			{
				return "";
			}
			input = input.Replace("!", "%21");
			input = input.Replace("#", "%23");
			input = input.Replace("$", "%24");
			input = input.Replace("'", "%27");
			input = input.Replace("(", "%28");
			input = input.Replace(")", "%29");
			input = input.Replace("*", "%2A");
			input = input.Replace("+", "%2B");
			input = input.Replace(",", "%2c");
			input = input.Replace("/", "%2F");
			input = input.Replace(":", "%3A");
			input = input.Replace(";", "%3B");
			input = input.Replace("=", "%3D");
			input = input.Replace("?", "%3F");
			input = input.Replace("@", "%40");
			input = input.Replace("[", "%5B");
			input = input.Replace("]", "%5D");
			input = input.Replace("\r\n", "\n");
			input = input.Replace("\r", "\n");
			input = input.Replace("\n", "%0D%0A");
			input = input.Replace(" ", "%20");
			input = input.Replace("\"", "%22");
			input = input.Replace("-", "%2D");
			input = input.Replace(".", "%2E");
			input = input.Replace("<", "%3C");
			input = input.Replace(">", "%3E");
			input = input.Replace("\\", "%5C");
			input = input.Replace("^", "%5E");
			input = input.Replace("_", "%5F");
			input = input.Replace("`", "%60");
			input = input.Replace("{", "%7B");
			input = input.Replace("|", "%7C");
			input = input.Replace("}", "%7D");
			input = input.Replace("~", " %7E");
			return input;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "fixForUrl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 269);
			return "";
		}
	}

	public string RemoveNoNumbers(string input, string replcment = "")
	{
		try
		{
			string text = "";
			input = FixString(input);
			if (string.IsNullOrEmpty(input))
			{
				return "";
			}
			input = input.Replace("\r", replcment);
			input = input.Replace("\n", replcment);
			input = input.Replace("\t", replcment);
			char[] array = input.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				text = ((!"0123456789".Contains(c.ToString())) ? (text + replcment) : (text + c));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveNoNumbers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 292);
			return null;
		}
	}

	public string NpRemoveNoNumbers(string Input, string Replcment = "")
	{
		try
		{
			string text = "";
			Input = FixString(Input);
			if (string.IsNullOrEmpty(Input))
			{
				return "";
			}
			Input = Input.Replace("\r", Replcment);
			Input = Input.Replace("\n", Replcment);
			Input = Input.Replace("\t", Replcment);
			char[] array = Input.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				text = ((!"0123456789./".Contains(c.ToString())) ? (text + Replcment) : (text + c));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NpRemoveNoNumbers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 316);
			return null;
		}
	}

	public string RemoveNumbers(string input, string replacement)
	{
		try
		{
			return Regex.Replace(input, "[\\d-]", replacement);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveNumbers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 329);
			return null;
		}
	}

	public string IsCorrectName(string name, string title = "کالا")
	{
		string result = "";
		try
		{
			if (string.IsNullOrEmpty(name))
			{
				result = "نام " + title + " خالی می باشد";
			}
			if (name.Length < 2)
			{
				result = "نام " + title + " باید بیش از یک کاراکتر باشد";
			}
			if (name.SplitString("/\\\r\n\t=+$#@*|><,`~'\".?؟،;:").Count > 1)
			{
				result = "در نام " + title + " از کاراکترهای غیر مجاز استفاده شده است /\\\r\n\t=+$#@*|><,`~'.?؟،;:";
			}
		}
		catch (Exception ex)
		{
			result = ex.Message;
		}
		return result;
	}

	public List<string> SplitString(string input, string SpliterList = " /\\\r\n\t)(-_=+%$#@*|><,`~'\":.?؟،;")
	{
		if (string.IsNullOrEmpty(input))
		{
			return new List<string>();
		}
		input = FixString(input.ToLower());
		return input.Split(SpliterList.ToCharArray())?.ToList();
	}

	public string FixString(string input)
	{
		try
		{
			if (string.IsNullOrEmpty(input))
			{
				return "";
			}
			while (input.Contains("  "))
			{
				input = input.Replace("  ", " ");
			}
			input = input.Replace("*", "×");
			input = input.Replace("'", "");
			input = input.Replace("ھ", "ه");
			input = input.Replace("ك", "ک");
			input = input.Replace("ي", "ی");
			input = input.Replace("۰", "0").Replace("0", "0").Replace("٠", "0");
			input = input.Replace("۱", "1").Replace("١", "1");
			input = input.Replace("۲", "2").Replace("٢", "2");
			input = input.Replace("٣", "3").Replace("۳", "3");
			input = input.Replace("۴", "4").Replace("٤", "4");
			input = input.Replace("٥", "5").Replace("۵", "5");
			input = input.Replace("۶", "6").Replace("٦", "6");
			input = input.Replace("٧", "7").Replace("۷", "7");
			input = input.Replace("۸", "8").Replace("٨", "8");
			input = input.Replace("۹", "9").Replace("٩", "9");
			return input;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 388);
			return null;
		}
	}

	public string FixString(string input, bool SQLServer)
	{
		try
		{
			if (SQLServer && input.Contains("FUNCTION [dbo].[FixString]"))
			{
				return input;
			}
			if (input == null)
			{
				return "";
			}
			while (input.Contains("  "))
			{
				input = input.Replace("  ", " ");
			}
			input = input.Replace("ك", "ک");
			if (!input.Contains("@IN,N'ك',"))
			{
				input = input.Replace("ي", "ی");
			}
			if (!input.Contains("@IN,N'ك',"))
			{
				input = input.Replace("ي", "ی");
			}
			input = input.Replace("ھ", "ه");
			input = input.Replace("\u200d", "");
			input = input.Replace("۰", "0").Replace("0", "0");
			input = input.Replace("۱", "1");
			input = input.Replace("۲", "2").Replace("٢", "2");
			input = input.Replace("٣", "3").Replace("۳", "3");
			input = input.Replace("۴", "4").Replace("٤", "4");
			input = input.Replace("٥", "5").Replace("۵", "5");
			input = input.Replace("۶", "6").Replace("٦", "6");
			input = input.Replace("٧", "7").Replace("۷", "7");
			input = input.Replace("۸", "8").Replace("٨", "8");
			input = input.Replace("۹", "9").Replace("٩", "9");
			return input;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 484);
			return "";
		}
	}

	public bool ISString(char Ch)
	{
		try
		{
			Ch = char.ToUpper(Ch);
			return Ch switch
			{
				'A' => true, 
				'C' => true, 
				'D' => true, 
				'E' => true, 
				'F' => true, 
				'G' => true, 
				'H' => true, 
				'I' => true, 
				'J' => true, 
				'K' => true, 
				'L' => true, 
				'M' => true, 
				'N' => true, 
				'O' => true, 
				'P' => true, 
				'Q' => true, 
				'R' => true, 
				'S' => true, 
				'T' => true, 
				'U' => true, 
				'V' => true, 
				'W' => true, 
				'X' => true, 
				'Y' => true, 
				'Z' => true, 
				'ا' => true, 
				'آ' => true, 
				'ب' => true, 
				'پ' => true, 
				'ت' => true, 
				'ث' => true, 
				'ج' => true, 
				'چ' => true, 
				'ح' => true, 
				'خ' => true, 
				'د' => true, 
				'ذ' => true, 
				'ر' => true, 
				'ز' => true, 
				'س' => true, 
				'ش' => true, 
				'ص' => true, 
				'ض' => true, 
				'ط' => true, 
				'ظ' => true, 
				'ع' => true, 
				'غ' => true, 
				'ف' => true, 
				'ق' => true, 
				'ك' => true, 
				'ک' => true, 
				'گ' => true, 
				'ل' => true, 
				'م' => true, 
				'ن' => true, 
				'و' => true, 
				'ه' => true, 
				'ی' => true, 
				'ي' => true, 
				'-' => true, 
				'.' => true, 
				'_' => true, 
				'/' => true, 
				'(' => true, 
				')' => true, 
				'+' => true, 
				'\\' => true, 
				'*' => true, 
				'×' => true, 
				_ => false, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ISString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 572);
			return false;
		}
	}

	public bool ISNumber(char Ch)
	{
		try
		{
			return Ch switch
			{
				'0' => true, 
				'1' => true, 
				'2' => true, 
				'3' => true, 
				'4' => true, 
				'5' => true, 
				'6' => true, 
				'7' => true, 
				'8' => true, 
				'9' => true, 
				_ => false, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ISNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 611);
			return false;
		}
	}

	public bool IsStringOrNumber(char ch)
	{
		try
		{
			return ISNumber(ch) || ISString(ch);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsStringOrNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 625);
			return false;
		}
	}

	public string RemoveNoNumbers_Dot(string Input, string Replcment = "")
	{
		try
		{
			Input = FixString(Input).Trim();
			if (Input == "")
			{
				return Input;
			}
			Input = Input.Replace("\r\n", Replcment);
			char[] array = Input.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char value = array[i];
				if (!Enumerable.Contains("0123456789.", value) && value.ToString() != "" && value.ToString() != Replcment)
				{
					Input = Input.Replace(value.ToString(), Replcment);
				}
			}
			if (Replcment != "")
			{
				while (Input.Contains(Replcment + Replcment))
				{
					Input = Input.Replace(Replcment + Replcment, Replcment);
				}
			}
			return Input;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveNoNumbers_Dot", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 670);
			return "";
		}
	}

	public string Negasht(char e)
	{
		try
		{
			return e.ToString().FixString() switch
			{
				"ا" => "H", 
				"آ" => "H", 
				"ب" => "F", 
				"پ" => "\\", 
				"ت" => "J", 
				"ث" => "E", 
				"ج" => "[", 
				"چ" => "]", 
				"ح" => "P", 
				"خ" => "O", 
				"د" => "N", 
				"ذ" => "B", 
				"ر" => "V", 
				"ز" => "C", 
				"س" => "S", 
				"ش" => "A", 
				"ص" => "W", 
				"ض" => "Q", 
				"ط" => "X", 
				"ظ" => "Z", 
				"ع" => "U", 
				"غ" => "Y", 
				"ف" => "T", 
				"ق" => "R", 
				"ک" => ";", 
				"گ" => ";", 
				"ل" => "G", 
				"م" => "L", 
				"ن" => "K", 
				"و" => ",", 
				"ه" => "I", 
				"ی" => "D", 
				"ئ" => "M", 
				_ => Enumerable.Contains("ABCDEFGHIJKLMNOPQRSTUVWXYZ-+=/\\1234567890", e) ? e.ToString().FixString() : "", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Negasht", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 721);
			return "";
		}
	}

	public bool StringComparisons(string source, string searchString, StringComparison comparer = StringComparison.OrdinalIgnoreCase)
	{
		try
		{
			if (string.IsNullOrEmpty(source))
			{
				return false;
			}
			return source.FixString().IndexOf(searchString.FixString(), comparer) >= 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StringComparisons", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 734);
			return false;
		}
	}

	public decimal FixDecimal(decimal value, short numberOfDigits, short numberAfterPoint)
	{
		try
		{
			decimal num = value % 1m;
			decimal num2 = value - num;
			num = Math.Round(num, numberAfterPoint);
			decimal num3 = (decimal)Math.Pow(10.0, checked(numberOfDigits - numberAfterPoint));
			if (value >= num3 || value <= -num3)
			{
				return 0m;
			}
			decimal num4 = num2 + num;
			if ((decimal)Math.Round((double)num4, 6) == num4)
			{
				return (decimal)Math.Round((double)num4, 6);
			}
			return num4;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDecimal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 755);
			return value;
		}
	}

	public bool IsPersianText(string value)
	{
		try
		{
			string source = "abcdefghijklmnopqrstuvwxyz_1234567890-";
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			char[] array = value.FixString().ToLower().ToCharArray();
			if (array == null || !array.Any())
			{
				return false;
			}
			char[] array2 = array;
			foreach (char value2 in array2)
			{
				if (!Enumerable.Contains(source, value2))
				{
					return true;
				}
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsPersianText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 776);
			return false;
		}
	}

	public string Negasht(string value)
	{
		try
		{
			string text = "";
			if (string.IsNullOrEmpty(value))
			{
				return text;
			}
			char[] array = value.FixString().ToLower().ToCharArray();
			if (array == null || !array.Any())
			{
				return text;
			}
			char[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				char c = array2[i];
				switch (c.ToString())
				{
				case "ا":
					text += "A";
					break;
				case "آ":
					text += "A";
					break;
				case "ب":
					text += "B";
					break;
				case "پ":
					text += "P";
					break;
				case "ت":
					text += "T";
					break;
				case "ث":
					text += "S";
					break;
				case "ج":
					text += "J";
					break;
				case "چ":
					text += "CH";
					break;
				case "ح":
					text += "H";
					break;
				case "خ":
					text += "KH";
					break;
				case "د":
					text += "D";
					break;
				case "ذ":
					text += "Z";
					break;
				case "ر":
					text += "R";
					break;
				case "ز":
					text += "Z";
					break;
				case "س":
					text += "S";
					break;
				case "ش":
					text += "Sh";
					break;
				case "ص":
					text += "S";
					break;
				case "ض":
					text += "Z";
					break;
				case "ط":
					text += "T";
					break;
				case "ظ":
					text += "Z";
					break;
				case "ع":
					text += "A";
					break;
				case "غ":
					text += "Gh";
					break;
				case "ف":
					text += "F";
					break;
				case "ق":
					text += "Gh";
					break;
				case "ک":
					text += "K";
					break;
				case "گ":
					text += "G";
					break;
				case "ل":
					text += "L";
					break;
				case "م":
					text += "M";
					break;
				case "ن":
					text += "N";
					break;
				case "و":
					text += "V";
					break;
				case "ه":
					text += "H";
					break;
				case " ":
					text += "-";
					break;
				}
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Negasht", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\TextAndCalendar\\TextManager.cs", 828);
			return "";
		}
	}
}
