using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using PacketParser.Entities;

namespace PacketParser.Assistance.MultiLingual;

public class ArbicCalendar : BaseCalendar
{
	private SemaphoreSlim _fileReadSemaphore = new SemaphoreSlim(1, 1);

	private bool isCached;

	public static readonly string[] ArbicmonthNames = new string[12]
	{
		"محرم", "صفر", "ربیع الاول", "ربیع الثانی", "جمادی الاول", "جمادی الثانی", "رجب", "شعبان", "رمضان", "شوال",
		"ذوالقعده", "ذوالحجه"
	};

	public bool IsDisposed { get; set; }

	public override DayOfWeek StartDayOfWeek
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string GetDayNameOfWeek(DateTime dateM)
	{
		try
		{
			return dateM.DayOfWeek switch
			{
				DayOfWeek.Friday => "الجمعه", 
				DayOfWeek.Monday => "الأثنین", 
				DayOfWeek.Saturday => "السبت", 
				DayOfWeek.Sunday => "الأحد", 
				DayOfWeek.Thursday => "الخمیس", 
				DayOfWeek.Tuesday => "الثلاثاء", 
				DayOfWeek.Wednesday => "الأربعاء", 
				_ => null, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDayNameOfWeek", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 38);
			return "";
		}
	}

	public ArbicCalendar(string path)
		: base(path)
	{
	}

	public override int GetMonthOfDate(string date)
	{
		return GetMonthOfDate_(date);
	}

	public override string GetPartday(string date, EnPartDate type)
	{
		return GetPartday_(date, type);
	}

	public override int GetYearOfDate(string date)
	{
		return GetYearOfDate_(date);
	}

	public override string Minutes2TimeFormat(int min)
	{
		return Minutes2TimeFormat_(min);
	}

	public override int GetDayOfDate(string date)
	{
		return GetDayOfDate_(date);
	}

	public override string GetMonthName(string month)
	{
		return ArbicmonthNames[checked(GetMonthOfDate(month) - 1)];
	}

	public override string GetMonthName(DateTime date)
	{
		return GetMonthName(MiladiToCalendar(date));
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
				if (num3 <= 0 || num3 > 30)
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
				if (num <= 1400 || num >= 1600)
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
				WebErrorLog.Instance.StartLog(ex, "", "AddToDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 88);
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
			HijriCalendar calendar = new HijriCalendar();
			list[0] = checked(list[0].ParseToInt() + yearCount).ToString();
			return MiladiToCalendar(new DateTime(list[0].ParseToInt(), list[1].ParseToInt(), list[2].ParseToInt(), calendar));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 106);
			return "";
		}
	}

	public override DateTime CalendarToMiladi(string dateParam)
	{
		DateTime result = DateTime.Now;
		try
		{
			dateParam = dateParam.Trim();
			if (dateParam.Length < 6)
			{
				return result;
			}
			string[] array = dateParam.Split(new char[1] { ' ' });
			if (array.Length == 0)
			{
				return result;
			}
			string input = "";
			if (array.Length != 0)
			{
				input = array[0];
			}
			input = TextManager.Instance.RemoveNoNumbers(input, "/").Replace("//", "/");
			if (input.Length < 6)
			{
				return result;
			}
			List<string> list = input.SplitString();
			if (list.Count != 3)
			{
				return result;
			}
			double num = list[0].ParseToDouble(0.0, 3);
			double num2 = list[1].ParseToDouble(0.0, 3);
			double num3 = list[2].ParseToDouble(0.0, 3);
			if (num3 <= 0.0 || num3 > 30.0)
			{
				return result;
			}
			if (num2 <= 0.0 || num2 > 12.0)
			{
				return result;
			}
			double num4 = getInt((11.0 * num + 3.0) / 30.0) + 354.0 * num + 30.0 * num2 - getInt((num2 - 1.0) / 2.0) + num3 + 1948440.0 - 385.0;
			_ = num4 % 7.0;
			if (num4 > 2299160.0)
			{
				double num5 = num4 + 68569.0;
				double @int = getInt(4.0 * num5 / 146097.0);
				num5 -= getInt((146097.0 * @int + 3.0) / 4.0);
				double int2 = getInt(4000.0 * (num5 + 1.0) / 1461001.0);
				num5 = num5 - getInt(1461.0 * int2 / 4.0) + 31.0;
				double int3 = getInt(80.0 * num5 / 2447.0);
				num3 = num5 - getInt(2447.0 * int3 / 80.0);
				num5 = getInt(int3 / 11.0);
				num2 = int3 + 2.0 - 12.0 * num5;
				num = 100.0 * (@int - 49.0) + int2 + num5;
			}
			else
			{
				double int3 = num4 + 1402.0;
				double int4 = getInt((int3 - 1.0) / 1461.0);
				double num5 = int3 - 1461.0 * int4;
				double @int = getInt((num5 - 1.0) / 365.0) - getInt(num5 / 1461.0);
				double int2 = num5 - 365.0 * @int + 30.0;
				int3 = getInt(80.0 * int2 / 2447.0);
				num3 = int2 - getInt(2447.0 * int3 / 80.0);
				int2 = getInt(int3 / 11.0);
				num2 = int3 + 2.0 - 12.0 * int2;
				num = 4.0 * int4 + @int + int2 - 4716.0;
			}
			result = Convert.ToDateTime($"{num}/{num2}/{num3}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalendarToMiladi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 176);
		}
		return result;
	}

	private static double getInt(double fNumber)
	{
		if (fNumber < -1E-07)
		{
			return Math.Ceiling(fNumber - 1E-07);
		}
		return Math.Floor(fNumber + 1E-07);
	}

	public override bool CheckDate(string dateSh)
	{
		try
		{
			dateSh = dateSh.Trim();
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
			if (result3 <= 0 || result3 > 30)
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
			if (result <= 1400 || result >= 1500)
			{
				return false;
			}
			new HijriCalendar();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Date:" + dateSh, "CheckDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 228);
		}
		return false;
	}

	public override DateTime EndDayOfMonth(string Date, string time)
	{
		int[] array = new int[12]
		{
			30, 29, 30, 29, 30, 29, 30, 29, 30, 29,
			30, 29
		};
		int num = 0;
		int num2 = 0;
		try
		{
			if (!CheckDate(Date))
			{
				return DateTime.Now;
			}
			num = GetYearOfDate(Date);
			num2 = GetMonthOfDate(Date);
			GetDayOfDate(Date);
			return EndDayOfMonth(checked((short)num2), num, time);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 248);
			return CalendarToMiladi($"{num}/{num2}/{array[num2]}");
		}
	}

	public override DateTime EndDayOfMonth(short Month, int Year, string time)
	{
		int[] array = new int[12]
		{
			30, 29, 30, 29, 30, 29, 30, 29, 30, 29,
			30, 29
		};
		try
		{
			int num = Year % 30;
			int num2 = ((Month == 12 && (num == 2 || num == 5 || num == 7 || num == 10 || num == 13 || num == 16 || num == 18 || num == 21 || num == 24 || num == 26 || num == 29)) ? 1 : 0);
			if (30 > checked(array[Month - 1] + num2))
			{
				return CalendarToMiladi($"{Year}/{Month}/{29}");
			}
			return CalendarToMiladi($"{Year}/{Month}/{30}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 269);
			return CalendarToMiladi($"{Year}/{Month}/{array[Month]}");
		}
	}

	public override int EndDayOfMonthInt(short civilMonth, int civilyear)
	{
		try
		{
			int[] array = new int[12]
			{
				30, 29, 30, 29, 30, 29, 30, 29, 30, 29,
				30, 29
			};
			int num = civilyear % 30;
			int num2 = ((civilMonth == 12 && (num == 2 || num == 5 || num == 7 || num == 10 || num == 13 || num == 16 || num == 18 || num == 21 || num == 24 || num == 26 || num == 29)) ? 1 : 0);
			if (30 > checked(array[civilMonth - 1] + num2))
			{
				return 29;
			}
			return 30;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonthInt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 290);
			return 0;
		}
	}

	public override string MiladiToCalendar(DateTime dateM)
	{
		try
		{
			int day = dateM.Day;
			int month = dateM.Month;
			int year = dateM.Year;
			int num = 0;
			double num2 = 0.0;
			num2 = ((year <= 1582 && (year != 1582 || month <= 10) && (year != 1582 || month != 10 || day <= 14)) ? ((double)checked(367 * year) - intPart(7.0 * ((double)checked(year + 5001) + intPart(checked(month - 9) / 7)) / 4.0) + intPart(checked(275 * month) / 9) + (double)day + 1729777.0 + (double)num) : (intPart(1461.0 * ((double)checked(year + 4800) + intPart(checked(month - 14) / 12)) / 4.0) + intPart(367.0 * ((double)checked(month - 2) - 12.0 * intPart(checked(month - 14) / 12)) / 12.0) - intPart(3.0 * intPart(((double)checked(year + 4900) + intPart(checked(month - 14) / 12)) / 100.0) / 4.0) + (double)day - 32075.0 + (double)num));
			double num3 = num2 - 1948440.0 + 10632.0;
			double num4 = intPart((num3 - 1.0) / 10631.0);
			num3 = num3 - 10631.0 * num4 + 354.0;
			double num5 = intPart((10985.0 - num3) / 5316.0) * intPart(50.0 * num3 / 17719.0) + intPart(num3 / 5670.0) * intPart(43.0 * num3 / 15238.0);
			num3 = num3 - intPart((30.0 - num5) / 15.0) * intPart(17719.0 * num5 / 50.0) - intPart(num5 / 16.0) * intPart(15238.0 * num5 / 43.0) + 29.0;
			double num6 = intPart(24.0 * num3 / 709.0);
			double num7 = num3 - intPart(709.0 * num6 / 24.0);
			double num8 = 30.0 * num4 + num5 - 30.0;
			return checked($"{(int)num8}/{(int)num6:D2}/{(int)num7:D2}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladiToCalendar", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 325);
			return "";
		}
	}

	private double intPart(double floatNum)
	{
		if (floatNum < -1E-07)
		{
			return Math.Ceiling(floatNum - 1E-07);
		}
		return Math.Floor(floatNum + 1E-07);
	}

	public override DateTime StartDayOfMonth()
	{
		using (new JaegerMonitor("StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs"))
		{
			try
			{
				List<string> list = MiladiToCalendar(DateTime.Now).SplitString();
				return CalendarToMiladi(list[0] + "/" + list[1] + "/1");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 346);
				return DateTime.Now;
			}
		}
	}

	public override DateTime StartDayOfMonth(short Month, int Year, string time)
	{
		using (new JaegerMonitor("StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs"))
		{
			try
			{
				return CalendarToMiladi($"{Year}/{Month}/1");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 360);
				return DateTime.Now;
			}
		}
	}

	public override string Today()
	{
		try
		{
			return MiladiToCalendar(DateTime.Now.Date);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Today", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 373);
			return null;
		}
	}

	public override string ToLongDateWithoutYear(DateTime dateM)
	{
		string text = "";
		try
		{
			return GetDayNameOfWeek(dateM) + " " + GetDayOfDate(MiladiToCalendar(dateM).ToString()) + " " + GetMonthName(MiladiToCalendar(dateM).ToString()) + GetYearOfDate(MiladiToCalendar(dateM).ToString());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToLongDateWithoutYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 388);
			return "";
		}
	}

	public override string ToShortDate(DateTime dateM)
	{
		try
		{
			new HijriCalendar();
			CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("ar-Ar");
			return dateM.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToShortDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 403);
			return "";
		}
	}

	public override string ToLongTimeString(DateTime dateM)
	{
		return ToLongTimeString_(dateM);
	}

	public override DateTime EndDayOfMonth()
	{
		int[] array = new int[12]
		{
			30, 29, 30, 29, 30, 29, 30, 29, 30, 29,
			30, 29
		};
		int num = 0;
		int num2 = 0;
		string text = MiladiToCalendar(DateTime.Now);
		try
		{
			if (!CheckDate(text))
			{
				return DateTime.Now;
			}
			num = GetYearOfDate(text);
			num2 = GetMonthOfDate(text);
			GetDayOfDate(text);
			return EndDayOfMonth(checked((short)num2), num, "23:59:59");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 424);
			return CalendarToMiladi($"{num}/{num2}/{array[num2]}");
		}
	}

	public override DateTime StartDayOfMonth(string Date, string time)
	{
		using (new JaegerMonitor("StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs"))
		{
			try
			{
				int yearOfDate = GetYearOfDate(Date);
				return StartDayOfMonth(short.Parse(GetMonthOfDate(Date).ToString()), yearOfDate, time);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StartDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 439);
				return DateTime.Now;
			}
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
			list[1] = checked(list[1].ParseToInt() + Count).ToString();
			return MiladiToCalendar(CalendarToMiladi(list[0] + "/" + list[1] + "/" + list[2]));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 456);
			return "";
		}
	}

	public override string MiladitoCalenderText(DateTime dateM)
	{
		try
		{
			return GetDayNameOfWeek(dateM) + " " + MiladiToCalendar(dateM);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladitoCalenderText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 470);
		}
		return "";
	}

	public override string[] GetListOfMonth()
	{
		return ArbicmonthNames;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\ArbicCalendar.cs", 492);
		}
	}
}
