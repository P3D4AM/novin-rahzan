using System;
using PacketParser.Entities;

namespace PacketParser;

public class Time
{
	public static string IntToTime(double second, string format = "mm:ss")
	{
		return IntToTime(checked((int)second), format);
	}

	public static string IntToTime(int second, string format = "mm:ss")
	{
		try
		{
			return new DateTime(1900, 1, 1).AddSeconds(second).ToString(format);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"second:{second}//format:{format}", "IntToTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Time.cs", 27);
			return "";
		}
	}

	public static int Diff(string d1, string d2)
	{
		try
		{
			if (string.IsNullOrEmpty(d1))
			{
				d1 = "00:00";
			}
			if (string.IsNullOrEmpty(d2))
			{
				d2 = "00:00";
			}
			DateTime dateTime = Convert.ToDateTime(d1);
			return (Convert.ToDateTime(d2) - dateTime).Minutes;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Diff", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Time.cs", 44);
			return 0;
		}
	}

	public static int Hour(int minutes)
	{
		try
		{
			return minutes / 60;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Hour", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Time.cs", 57);
			return 0;
		}
	}

	public static int Min(int minutes)
	{
		try
		{
			return minutes % 60;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Min", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Time.cs", 69);
			return 0;
		}
	}

	public static int IntToTime(string min)
	{
		checked
		{
			try
			{
				if (string.IsNullOrEmpty(min.Trim()))
				{
					return 0;
				}
				int num = 0;
				int num2 = min.IndexOf(":", StringComparison.Ordinal);
				int num3;
				if (num2 > -1)
				{
					num3 = int.Parse(min.Substring(0, num2));
					num = int.Parse(min.Substring(num2 + 1));
				}
				else
				{
					num3 = int.Parse(min);
				}
				return num3 * 60 + num;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "IntToTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Time.cs", 92);
				return 0;
			}
		}
	}

	public static string IntToTime(int min)
	{
		try
		{
			if (min == 0)
			{
				return "";
			}
			if (min < 0)
			{
				min = checked(min * -1);
			}
			string text = "";
			if (min > 32000)
			{
				int num = min / 1440;
				min %= 1440;
				if (min == 0)
				{
					return $"{num} روز";
				}
				text += $"{num} روز و ";
			}
			short num2 = short.Parse((min % 60).ToString());
			short num3 = short.Parse((checked(min - num2) / 60).ToString());
			return text + $"{num3:00}:{num2:00}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IntToTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Time.cs", 127);
		}
		return "نامعتبر";
	}

	public static int TimeToInt(string min)
	{
		checked
		{
			try
			{
				if (string.IsNullOrEmpty(min.Trim()))
				{
					return 0;
				}
				int num = 0;
				int num2 = 0;
				int num3 = min.IndexOf(":", StringComparison.Ordinal);
				if (num3 > -1)
				{
					num = min.Substring(0, num3).ParseToInt();
					num2 = min.Substring(num3 + 1).ParseToInt();
				}
				else
				{
					num = min.ParseToInt();
				}
				return num * 60 + num2;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TimeToInt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Time.cs", 150);
				return 0;
			}
		}
	}
}
