using System;
using System.Globalization;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.Assistance;

public static class NumberManager
{
	private static string[] yakan = new string[10] { "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };

	private static string[] dahgan = new string[10] { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };

	private static string[] dahyek = new string[10] { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };

	private static string[] sadgan = new string[10] { "", "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };

	private static string[] basex = new string[10] { "", "هزار", "میلیون", "میلیارد", "تریلیون", "کادریلیون", "کنتلیون", "سیکستیلیون", "سپتیلیون", "اکتیلیون" };

	public static string NumberGroupingFormat => NumberFormatInfo.CurrentInfo.NumberGroupSeparator;

	public static string DecimalSeprator => NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;

	private static string Adad3RaghamiToStr(int num3Digit)
	{
		string text = "";
		int num = 0;
		try
		{
			int num2 = num3Digit % 100;
			num = 1;
			int num3 = num3Digit / 100;
			num = 1;
			if (num3 != 0)
			{
				text = sadgan[num3] + " و ";
			}
			num = 2;
			if (num2 >= 10 && num2 <= 19)
			{
				num = 3;
				text += dahyek[checked(num2 - 10)];
			}
			else
			{
				num = 4;
				int num4 = num2 / 10;
				num = 5;
				if (num4 != 0)
				{
					text = text + dahgan[num4] + " و ";
				}
				num = 6;
				int num5 = num2 % 10;
				num = 7;
				if (num5 != 0)
				{
					text = text + yakan[num5] + " و ";
				}
				num = 8;
				text = text.Substring(0, checked(text.Length - 3));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"line={num}, num3digit={num3Digit}", "Adad3RaghamiToStr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 54);
		}
		return text;
	}

	private static string AdadToStr(long num)
	{
		string text = "";
		int num2 = 0;
		checked
		{
			try
			{
				if (num == 0L)
				{
					return yakan[0];
				}
				string text2 = num.ToString();
				num2 = 1;
				text2 = text2.PadLeft((unchecked(checked(text2.Length - 1) / 3) + 1) * 3, '0');
				num2 = 2;
				int num3 = unchecked(text2.Length / 3) - 1;
				num2 = 3;
				for (int i = 0; i <= num3; i++)
				{
					int num4 = int.Parse(text2.Substring(i * 3, 3));
					num2 = 4;
					if (num4 != 0)
					{
						num2 = 5;
						text = text + Adad3RaghamiToStr(num4) + " " + basex[num3 - i] + " و ";
					}
				}
				num2 = 6;
				text = text.Substring(0, text.Length - 3);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line={num2}, num={num}", "AdadToStr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 83);
			}
			return text;
		}
	}

	public static string AdadToHoroof(decimal number)
	{
		try
		{
			return number.ToString().AdadToHoroof();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AdadToHoroof", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 92);
		}
		return "";
	}

	public static string AdadToHoroof(this int number)
	{
		try
		{
			return number.ToString().AdadToHoroof();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AdadToHoroof", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 101);
		}
		return "";
	}

	public static string AdadToHoroof(this long number)
	{
		try
		{
			return number.ToString().AdadToHoroof();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AdadToHoroof", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 110);
		}
		return "";
	}

	public static string AdadToHoroof(this double number)
	{
		try
		{
			return number.ToString().AdadToHoroof();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AdadToHoroof", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 119);
		}
		return "";
	}

	private static string AdadToHoroof(this string numberStr)
	{
		int num = 0;
		try
		{
			string text = "";
			string text2 = numberStr.Trim();
			if (string.IsNullOrEmpty(text2))
			{
				return "";
			}
			num = 1;
			if (text2.Length > 0 && text2.Substring(0, 1) == "-")
			{
				num = 2;
				text = "منفی ";
				text2 = text2.Substring(1, checked(text2.Length - 1));
			}
			num = 3;
			if (!text2.Contains(Text.DecimalPoint))
			{
				return text + AdadToStr(long.Parse(text2));
			}
			num = 4;
			string[] array = text2.Split(Text.DecimalPoint.ToCharArray());
			num = 5;
			string text3 = text + AdadToStr(long.Parse(array[0]));
			num = 6;
			string text4 = array[1].TrimEnd(new char[1] { '0' });
			num = 7;
			if (text4.Length > 0)
			{
				text3 += " ممیز ";
			}
			num = 8;
			if (!string.IsNullOrEmpty(text4))
			{
				text3 += AdadToStr(long.Parse(text4));
			}
			num = 9;
			switch (text4.Length)
			{
			case 1:
				text3 += " دهم ";
				break;
			case 2:
				text3 += " صدم ";
				break;
			case 3:
				text3 += " هزارم ";
				break;
			case 4:
				text3 += " ده هزارم ";
				break;
			case 5:
				text3 += " صد هزارم ";
				break;
			}
			return text3;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"line={num},numberstr={numberStr}", "AdadToHoroof", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 174);
		}
		return "";
	}

	public static string Mid(string str, int start, int Length)
	{
		if (Length == 0 || str == null)
		{
			return "";
		}
		int length = str.Length;
		return (start > length) ? "" : checked((start + Length <= length) ? str.Substring(start - 1, Length) : str.Substring(start - 1));
	}

	public static string Adad2(this decimal number, short digitForRound)
	{
		string text = "";
		checked
		{
			try
			{
				number = Math.Round(number, digitForRound);
				bool flag = false;
				double[] array = new double[4];
				if (number == 0m)
				{
					return "صفر";
				}
				if (number < 0m)
				{
					number = -number;
				}
				text = number.ToString().Trim();
				byte b = (byte)text.Length;
				if (b > 15)
				{
					return "بسیار بزرگ";
				}
				for (int i = 1; i < 15 - b; i++)
				{
					text = "0" + text;
				}
				for (int j = 1; (double)j < (double)unchecked(b / 3) + 0.99; j++)
				{
					array[5 - j] = Mid(text, 3 * (5 - j) + 1, 3).ParseToDouble(0.0, 3);
				}
				text = "";
				for (int k = 0; k < 5; k++)
				{
					if (array[k - 1] != 0.0)
					{
						switch (k)
						{
						case 1:
							text = text + Three(array[k - 1]) + " تریلیون";
							flag = true;
							break;
						case 2:
							text = text + (flag ? " و " : "") + Three(array[k - 1]) + " میلیارد";
							flag = true;
							break;
						case 3:
							text = text + (flag ? " و " : "") + Three(array[k - 1]) + " میلیون";
							flag = true;
							break;
						case 4:
							text = text + (flag ? " و " : "") + Three(array[k - 1]) + " هزار";
							flag = true;
							break;
						case 5:
							text = text + (flag ? " و " : "") + Three(array[k - 1]);
							break;
						}
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = "صفر";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Adad2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 243);
			}
			return text;
		}
	}

	private static string Three(double number)
	{
		string text = "";
		checked
		{
			try
			{
				byte[] array = new byte[2];
				long num = number.ToString().Trim().Length;
				if (number == 0.0)
				{
					text = "";
				}
				if (number == 100.0)
				{
					text = "یکصد";
				}
				if (num == 2)
				{
					array[1] = 0;
				}
				if (num == 1)
				{
					array[0] = 0;
					array[1] = 0;
				}
				for (long num2 = 1L; num2 < num; num2++)
				{
					array[2 - num2 + 1] = Convert.ToByte(Mid(number.ToString().Trim(), (int)(num - num2 + 1), 1));
				}
				switch (array[0])
				{
				case 1:
					text = "یکصد";
					break;
				case 2:
					text = "دویست";
					break;
				case 3:
					text = "سیصد";
					break;
				case 4:
					text = "چهارصد";
					break;
				case 5:
					text = "پانصد";
					break;
				case 6:
					text = "ششصد";
					break;
				case 7:
					text = "هفتصد";
					break;
				case 8:
					text = "هشتصد";
					break;
				case 9:
					text = "نهصد";
					break;
				}
				switch (array[1])
				{
				case 1:
					switch (array[2])
					{
					case 0:
						text += " و ده";
						break;
					case 1:
						text += " و یازده";
						break;
					case 2:
						text += " و دوازده";
						break;
					case 3:
						text += " و سیزده";
						break;
					case 4:
						text += " و چهارده";
						break;
					case 5:
						text += " و پانزده";
						break;
					case 6:
						text += " و شانزده";
						break;
					case 7:
						text += " و هفده";
						break;
					case 8:
						text += " و هجده";
						break;
					case 9:
						text += " و نوزده";
						break;
					}
					break;
				case 2:
					text += " و بیست";
					break;
				case 3:
					text += " و سی";
					break;
				case 4:
					text += " و چهل";
					break;
				case 5:
					text += " و پنجاه";
					break;
				case 6:
					text += " و شصت";
					break;
				case 7:
					text += " و هفتاد";
					break;
				case 8:
					text += " و هشتاد";
					break;
				case 9:
					text += " و نود";
					break;
				}
				if (array[1] != 1)
				{
					switch (array[2])
					{
					case 1:
						text += " و یک";
						break;
					case 2:
						text += " و دو";
						break;
					case 3:
						text += " و سه";
						break;
					case 4:
						text += " و چهار";
						break;
					case 5:
						text += " و پنج";
						break;
					case 6:
						text += " و شش";
						break;
					case 7:
						text += " و هفت";
						break;
					case 8:
						text += " و هشت";
						break;
					case 9:
						text += " و نه";
						break;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Three", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 321);
			}
			return text;
		}
	}

	public static string DoubleToString(double value)
	{
		try
		{
			string text = value.ToString().Replace(".", "/");
			if (text.EndsWith("/0"))
			{
				return text.Replace("/0", "");
			}
			if (text.EndsWith("/00"))
			{
				return text.Replace("/00", "");
			}
			if (text.EndsWith("/000"))
			{
				return text.Replace("/000", "");
			}
			if (text.EndsWith("/0000"))
			{
				return text.Replace("/0000", "");
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DoubleToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 341);
			return "";
		}
	}

	public static double StringToDouble(string value)
	{
		try
		{
			if (string.IsNullOrEmpty(value))
			{
				return 0.0;
			}
			value = value.FixString().RemoveNoNumbers(".");
			string[] source = value.Split(".".ToArray());
			if (!source.Any())
			{
				return 0.0;
			}
			if (source.Count() > 2)
			{
				return 0.0;
			}
			if (double.TryParse(value, out var result))
			{
				return result;
			}
			return 0.0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StringToDouble", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\NumberManager.cs", 360);
			return 0.0;
		}
	}
}
