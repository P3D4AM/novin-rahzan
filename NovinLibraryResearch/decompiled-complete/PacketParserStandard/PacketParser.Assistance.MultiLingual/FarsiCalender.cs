using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using PacketParser.Entities;

namespace PacketParser.Assistance.MultiLingual;

public class FarsiCalender : BaseCalendar
{
	private SemaphoreSlim _fileReadSemaphore = new SemaphoreSlim(1, 1);

	public static readonly string[] PersianmonthNames = new string[12]
	{
		"فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی",
		"بهمن", "اسفند"
	};

	private bool IsDisposed { get; set; }

	public override DayOfWeek StartDayOfWeek => DayOfWeek.Saturday;

	public FarsiCalender(string content)
		: base(content)
	{
	}

	public override string Minutes2TimeFormat(int min)
	{
		return Minutes2TimeFormat_(min);
	}

	public override string ToLongTimeString(DateTime dateM)
	{
		return ToLongTimeString_(dateM);
	}

	public override int GetDayOfDate(string date)
	{
		return GetDayOfDate_(date);
	}

	public override int GetMonthOfDate(string date)
	{
		return GetMonthOfDate_(date);
	}

	public override string GetPartday(string date, EnPartDate type)
	{
		return GetPartday_(date, type);
	}

	public override string GetMonthName(string dateSh)
	{
		if (!string.IsNullOrEmpty(dateSh))
		{
			return PersianmonthNames[checked(GetMonthOfDate(dateSh) - 1)];
		}
		return "";
	}

	public override int GetYearOfDate(string date)
	{
		return GetYearOfDate_(date);
	}

	public override string AddToDate(int count, string date, EnPartDate part)
	{
		checked
		{
			try
			{
				if (!CheckDate(date))
				{
					return date;
				}
				if (date.Length < 7)
				{
					return MiladiToCalendar(DateTime.Now);
				}
				List<string> list = date.Trim().SplitString("/");
				if (list.Count != 3)
				{
					return MiladiToCalendar(DateTime.Now);
				}
				int num = int.Parse(list[0]);
				int num2 = int.Parse(list[1]);
				int num3 = int.Parse(list[2]);
				if (num3 <= 0 || num3 > 31)
				{
					return MiladiToCalendar(DateTime.Now);
				}
				if (num2 <= 0 || num2 > 12)
				{
					return MiladiToCalendar(DateTime.Now);
				}
				if (num.ToString().Length == 2)
				{
					num = 1400 + num;
				}
				if (num <= 1200 || num >= 1500)
				{
					return MiladiToCalendar(DateTime.Now);
				}
				DateTime dateTime = CalendarToMiladi(date);
				int num4 = 0;
				switch (part)
				{
				case EnPartDate.Day:
					num4 = count;
					break;
				case EnPartDate.Mounth:
					if (count < 0)
					{
						num2--;
					}
					num4 = count * EndDayOfMonthInt((short)num2, num);
					break;
				case EnPartDate.Year:
					return num + count + "/" + num2.ToString("00") + "/" + num3.ToString("00");
				}
				dateTime = dateTime.AddDays(num4);
				return MiladiToCalendar(dateTime);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 58);
				return "";
			}
		}
	}

	public override string AddYear(string Date, int yearCount)
	{
		try
		{
			if (!CheckDate(Date))
			{
				return null;
			}
			List<string> list = Date.SplitString();
			PersianCalendar persianCalendar = new PersianCalendar();
			DateTime dateM = persianCalendar.AddYears(new DateTime(Convert.ToInt32(list[0]), Convert.ToInt32(list[1]), Convert.ToInt32(list[2]), persianCalendar), yearCount);
			return MiladiToCalendar(dateM);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 74);
			return "";
		}
	}

	public override DateTime CalendarToMiladi(string dateParam)
	{
		return dateParam.ParsToDateFromShOrMiDate();
	}

	public override bool CheckDate(string dateSh)
	{
		try
		{
			dateSh = dateSh?.Trim();
			if (string.IsNullOrEmpty(dateSh))
			{
				return false;
			}
			if (dateSh.Contains("_"))
			{
				return false;
			}
			List<string> list = dateSh.SplitString();
			if (list.Count < 3)
			{
				return false;
			}
			int.TryParse(list[0], out var result);
			int.TryParse(list[1], out var result2);
			int.TryParse(list[2], out var result3);
			if (result3 <= 0 || result3 > 31)
			{
				return false;
			}
			if (result2 <= 0 || result2 > 12)
			{
				return false;
			}
			if (result <= 0 || result > 1500)
			{
				return false;
			}
			if (result < 100)
			{
				result = checked(result + 1400);
			}
			if (result <= 1200 || result >= 1500)
			{
				return false;
			}
			if (result2 > 6 && result3 == 31)
			{
				return false;
			}
			PersianCalendar persianCalendar = new PersianCalendar();
			if (result2 == 12 && result3 == 30 && !persianCalendar.IsLeapYear(result))
			{
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Date:" + dateSh, "CheckDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 104);
		}
		return false;
	}

	public override DateTime EndDayOfMonth(string solarDate, string time)
	{
		try
		{
			if (!CheckDate(solarDate))
			{
				return DateTime.Now;
			}
			List<string> list = solarDate.SplitString();
			int civilyear = list[0].ParseToInt();
			short civilMonth = list[1].ParseToShort(0);
			int num = EndDayOfMonthInt(civilMonth, civilyear);
			return CalendarToMiladi($"{list[0]}/{list[1]}/{num}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "solarDate:" + solarDate, "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 121);
			return DateTime.Now;
		}
	}

	public override DateTime EndDayOfMonth()
	{
		try
		{
			int year = DateTime.Now.Year;
			int month = DateTime.Now.Month;
			int num = EndDayOfMonthInt(short.Parse(month.ToString()), year);
			return CalendarToMiladi($"{year}/{month}/{num}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 137);
			return DateTime.Now;
		}
	}

	public override DateTime EndDayOfMonth(short solarMonth, int solarYear, string time)
	{
		try
		{
			string arg = EndDayOfMonthInt(solarMonth, solarYear).ToString();
			return CalendarToMiladi($"{solarYear}/{solarMonth}/{arg}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 151);
			return DateTime.Now;
		}
	}

	public override int EndDayOfMonthInt(short civilMonth, int civilyear)
	{
		try
		{
			if (CheckDate($"{civilyear}/{civilMonth}/31"))
			{
				return 31;
			}
			if (CheckDate($"{civilyear}/{civilMonth}/30"))
			{
				return 30;
			}
			return 29;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonthInt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 169);
			return 0;
		}
	}

	public override string GetMonthName(DateTime date)
	{
		return GetMonthName(MiladiToCalendar(date));
	}

	public override string MiladiToCalendar(DateTime dateM)
	{
		StringBuilder stringBuilder = new StringBuilder($"DateM={dateM}");
		try
		{
			PersianCalendar persianCalendar = new PersianCalendar();
			stringBuilder.Append($" year={persianCalendar.GetYear(dateM)}");
			stringBuilder.Append($" month={persianCalendar.GetMonth(dateM):D2}");
			stringBuilder.Append($" day={persianCalendar.GetDayOfMonth(dateM):D2}");
			return $"{persianCalendar.GetYear(dateM)}/{persianCalendar.GetMonth(dateM):D2}/{persianCalendar.GetDayOfMonth(dateM):D2}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, stringBuilder.ToString(), "MiladiToCalendar", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 185);
		}
		return "";
	}

	public override DateTime StartDayOfMonth()
	{
		try
		{
			List<string> list = MiladiToCalendar(DateTime.Now).SplitString();
			return CalendarToMiladi(list[0] + "/" + list[1] + "/01");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 199);
			return DateTime.Now;
		}
	}

	public override DateTime StartDayOfMonth(short Month, int Year, string time)
	{
		try
		{
			return CalendarToMiladi($"{Year}/{Month:D2}/01");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 212);
			return DateTime.Now;
		}
	}

	public override string Today()
	{
		return MiladiToCalendar(DateTime.Now.Date);
	}

	public override string ToLongDateWithoutYear(DateTime dateM)
	{
		string text = "";
		try
		{
			PersianCalendar persianCalendar = new PersianCalendar();
			CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fa-IR");
			return cultureInfo.DateTimeFormat.DayNames[(int)persianCalendar.GetDayOfWeek(dateM)] + " " + persianCalendar.GetDayOfMonth(dateM) + " " + GetMonthName(dateM);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToLongDateWithoutYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 230);
			return "";
		}
	}

	public override string ToShortDate(DateTime dateM)
	{
		try
		{
			new PersianCalendar();
			CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fa-IR");
			return dateM.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToShortDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 247);
			return "";
		}
	}

	public override DateTime StartDayOfMonth(string date, string time)
	{
		try
		{
			int yearOfDate = GetYearOfDate(date);
			return StartDayOfMonth(short.Parse(GetMonthOfDate(date).ToString()), yearOfDate, time);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 261);
			return DateTime.Now;
		}
	}

	public override string AddMonth(string Date, int Count)
	{
		try
		{
			if (!CheckDate(Date))
			{
				return null;
			}
			List<string> list = Date.SplitString();
			PersianCalendar persianCalendar = new PersianCalendar();
			DateTime dateM = persianCalendar.AddMonths(new DateTime(Convert.ToInt32(list[0]), Convert.ToInt32(list[1]), Convert.ToInt32(list[2]), persianCalendar), Count);
			return MiladiToCalendar(dateM);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 277);
			return "";
		}
	}

	public override string MiladitoCalenderText(DateTime dateM)
	{
		return GetDayNameOfWeek(dateM) + " " + MiladiToCalendar(dateM);
	}

	public override string[] GetListOfMonth()
	{
		return PersianmonthNames;
	}

	public override string DateToFormatTime(DateTime dt)
	{
		return DateToFormatTime_(dt);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_fileReadSemaphore?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 292);
		}
	}

	public override string GetDayNameOfWeek(DateTime dateM)
	{
		try
		{
			return dateM.DayOfWeek switch
			{
				DayOfWeek.Saturday => "شنبه", 
				DayOfWeek.Sunday => "یکشنبه", 
				DayOfWeek.Monday => "دوشنبه", 
				DayOfWeek.Tuesday => "سه شنبه", 
				DayOfWeek.Wednesday => "چهار شنبه", 
				DayOfWeek.Thursday => "پنج شنبه", 
				DayOfWeek.Friday => "جمعه", 
				_ => throw new Exception($"dateM.DayOfWeek({dateM.DayOfWeek}) is not valid."), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDayNameOfWeek", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\FarsiCalender.cs", 312);
			return "";
		}
	}
}
