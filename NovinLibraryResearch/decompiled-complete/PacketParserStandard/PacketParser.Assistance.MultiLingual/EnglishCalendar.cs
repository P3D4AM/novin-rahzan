using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Assistance.MultiLingual;

public class EnglishCalendar : BaseCalendar
{
	private SemaphoreSlim _fileReadSemaphore = new SemaphoreSlim(1, 1);

	private bool isCached;

	public static readonly string[] InternationalmonthNames = new string[12]
	{
		"ژانویه", "فوریه", "مارس", "آپریل", "می", "ژوئن", "جولای", "آگوست", "سپتامبر", "اکتبر",
		"نوامبر", "دسامبر"
	};

	private bool IsDisposed { get; set; }

	public override DayOfWeek StartDayOfWeek
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public EnglishCalendar(string content)
		: base(content)
	{
	}

	public override string AddToDate(int count, string date, EnPartDate part)
	{
		checked
		{
			try
			{
				if (date.Length < 7)
				{
					return MiladiToCalendar(DateTime.Now);
				}
				date = date.Trim();
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
				if (num.ToString().Length <= 2)
				{
					num = 2000 + num;
				}
				if (num <= 1900 || num >= 2200)
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
				WebErrorLog.Instance.StartLog(ex, "", "AddToDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 88);
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
				return Date.ToString();
			}
			List<string> list = Date.SplitString();
			GregorianCalendar gregorianCalendar = new GregorianCalendar();
			return gregorianCalendar.AddYears(new DateTime(Convert.ToInt32(list[0]), Convert.ToInt32(list[1]), Convert.ToInt32(list[2]), gregorianCalendar), yearCount).ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 106);
			return "";
		}
	}

	public override DateTime CalendarToMiladi(string dateParam)
	{
		return dateParam.ParseToDate();
	}

	public override bool CheckDate(string dateSh)
	{
		try
		{
			DateTime.Parse(dateSh);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public override DateTime EndDayOfMonth(string Date, string time)
	{
		using (new JaegerMonitor("EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs"))
		{
			try
			{
				if (!CheckDate(Date))
				{
					return DateTime.Now;
				}
				List<string> list = Date.SplitString();
				int civilyear = list[0].ParseToInt();
				short civilMonth = list[1].ParseToShort(0);
				int num = EndDayOfMonthInt(civilMonth, civilyear);
				return CalendarToMiladi(list[0] + "/" + list[1] + "/" + num);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "solarDate:" + Date, "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 145);
				return DateTime.Now;
			}
		}
	}

	public override DateTime EndDayOfMonth(short Month, int Year, string time)
	{
		try
		{
			string arg = EndDayOfMonthInt(Month, Year).ToString();
			return CalendarToMiladi($"{Year} /{Month}/{arg}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 163);
			return DateTime.Now;
		}
	}

	public override int EndDayOfMonthInt(short civilMonth, int civilyear)
	{
		using (new JaegerMonitor("EndDayOfMonthInt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs"))
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
				WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonthInt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 181);
			}
			return 30;
		}
	}

	public override string GetDayNameOfWeek(DateTime dateM)
	{
		return dateM.DayOfWeek switch
		{
			DayOfWeek.Friday => "Friday", 
			DayOfWeek.Monday => "Monday", 
			DayOfWeek.Saturday => "Saturday", 
			DayOfWeek.Sunday => "Sunday", 
			DayOfWeek.Thursday => "Thursday", 
			DayOfWeek.Tuesday => "Tuesday", 
			DayOfWeek.Wednesday => "Wednesday", 
			_ => null, 
		};
	}

	public override string GetMonthName(string month)
	{
		return InternationalmonthNames[checked(month.ParseToInt() - 1)];
	}

	public override string GetMonthName(DateTime date)
	{
		return InternationalmonthNames[checked(date.Month - 1)];
	}

	public override string MiladiToCalendar(DateTime dateM)
	{
		return dateM.ToString();
	}

	public override DateTime StartDayOfMonth()
	{
		return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
	}

	public override DateTime StartDayOfMonth(short Month, int Year, string time)
	{
		return new DateTime(Year, Month, 1);
	}

	public override string Today()
	{
		return DateTime.Now.ToString();
	}

	public override string ToLongDateWithoutYear(DateTime dateM)
	{
		string text = "";
		try
		{
			GregorianCalendar gregorianCalendar = new GregorianCalendar();
			CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-En");
			return cultureInfo.DateTimeFormat.DayNames[(int)gregorianCalendar.GetDayOfWeek(dateM)] + " " + gregorianCalendar.GetDayOfMonth(dateM) + " " + GetMonthName(dateM);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToLongDateWithoutYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 238);
			return "";
		}
	}

	public override string ToShortDate(DateTime dateM)
	{
		return dateM.ToString("yyyy/mm/dd");
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

	public override string ToLongTimeString(DateTime dateM)
	{
		return ToLongTimeString_(dateM);
	}

	public override DateTime EndDayOfMonth()
	{
		DateTime result;
		using (new JaegerMonitor("EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs"))
		{
			try
			{
				result = DateTime.Now;
				int year = result.Year;
				result = DateTime.Now;
				short num = short.Parse(result.Month.ToString());
				int num2 = EndDayOfMonthInt(num, year);
				result = CalendarToMiladi($"{year}/{num}/{num2.ToString()}");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"solarDate:{DateTime.Now}", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 278);
				result = DateTime.Now;
			}
		}
		return result;
	}

	public override DateTime StartDayOfMonth(string Date, string time)
	{
		return new DateTime(GetYearOfDate(Date), GetMonthOfDate(Date), 1);
	}

	public override string AddMonth(string Date, int Count)
	{
		try
		{
			if (!CheckDate(Date))
			{
				return "0";
			}
			List<string> list = Date.SplitString();
			GregorianCalendar gregorianCalendar = new GregorianCalendar();
			return gregorianCalendar.AddMonths(new DateTime(Convert.ToInt32(list[0]), Convert.ToInt32(list[1]), Convert.ToInt32(list[2]), gregorianCalendar), Count).ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 303);
			return "";
		}
	}

	public override string MiladitoCalenderText(DateTime dateM)
	{
		string result = "";
		try
		{
			string text = MiladiToCalendar(dateM);
			string dayNameOfWeek = GetDayNameOfWeek(text.CalendarToMiladi());
			result = $"{GetDayNameOfWeek(dateM)} {GetYearOfDate(text)}/{GetMonthOfDate(text)}/{dayNameOfWeek}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladitoCalenderText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 316);
		}
		return result;
	}

	public override string[] GetListOfMonth()
	{
		return InternationalmonthNames;
	}

	public override string DateToFormatTime(DateTime dt)
	{
		return DateToFormatTime_(dt);
	}

	public async Task<ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>>> GetAll()
	{
		ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>> returnedSaveFuncInfoWithValue = await GetAll(base.Content);
		if (!returnedSaveFuncInfoWithValue.HasError)
		{
			base.LstLangauge = returnedSaveFuncInfoWithValue.value;
		}
		return returnedSaveFuncInfoWithValue;
	}

	public async Task<string> GetValueAsync(string key)
	{
		await _fileReadSemaphore.WaitAsync();
		try
		{
			if (string.IsNullOrEmpty(key))
			{
				return "";
			}
			if (base.LstLangauge == null)
			{
				return key;
			}
			if (base.LstLangauge.TryGetValue(key, out var value))
			{
				return value;
			}
			return key;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 354);
			return key;
		}
		finally
		{
			_fileReadSemaphore.Release();
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\EnglishCalendar.cs", 370);
		}
	}
}
