using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Entities;

namespace PacketParser;

public static class Calendar
{
	public static readonly string[] PersianmonthNames = new string[12]
	{
		"فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی",
		"بهمن", "اسفند"
	};

	public static readonly string[] ArbicmonthNames = new string[12]
	{
		"محرم", "صفر", "ربیع الاول", "ربیع الثانی", "جمادی الاول", "جمادی الثانی", "رجب", "شعبان", "رمضان", "شوال",
		"ذوالقعده", "ذوالحجه"
	};

	public static readonly string[] InternationalmonthNames = new string[12]
	{
		"ژانویه", "فوریه", "مارس", "آپریل", "می", "ژوئن", "جولای", "آگوست", "سپتامبر", "اکتبر",
		"نوامبر", "دسامبر"
	};

	public static string FixScaleShamsiDate(this string dateSh)
	{
		try
		{
			if (string.IsNullOrEmpty(dateSh))
			{
				return "";
			}
			if (dateSh.SplitString("/").Count == 3)
			{
				return dateSh.ShamsiToMiladi().MiladiToCalture();
			}
			if (dateSh.Length == 6)
			{
				return DateTime.Now.MiladiToCalture().Substring(0, 2) + dateSh.Substring(0, 2) + "/" + dateSh.Substring(2, 2) + "/" + dateSh.Substring(4, 2);
			}
			if (dateSh.Length == 4)
			{
				return DateTime.Now.MiladiToCalture().Substring(0, 4) + dateSh.Substring(0, 2) + "/" + dateSh.Substring(2, 2);
			}
			if (dateSh.Length != 8)
			{
				throw new Exception("فرمت تاریخ اش رو نمیشناسم!\r\n" + dateSh);
			}
			return DateTime.Now.MiladiToCalture().Substring(0, 4) + dateSh.Substring(0, 4) + "/" + dateSh.Substring(2, 2) + "/" + dateSh.Substring(2, 2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, dateSh, "FixScaleShamsiDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 33);
			return dateSh;
		}
	}

	public static DateTime ShamsiToMiladi(this string dateParam, string time = "", string separator = "/", bool isMonthDay = false)
	{
		DateTime now = DateTime.Now;
		if (string.IsNullOrEmpty(dateParam))
		{
			return now;
		}
		try
		{
			if (isMonthDay)
			{
				int year = new PersianCalendar().GetYear(DateTime.Now);
				dateParam = ((dateParam.Length < 5) ? $"{year}{separator}{dateParam.Substring(0, 2)}{separator}{dateParam.Substring(2, 2)}" : $"{year}{separator}{dateParam}");
			}
			if (separator != "/")
			{
				dateParam = dateParam.Replace(separator, "/");
			}
			return dateParam.ShamsiToMiladi(time);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Date:" + dateParam, "ShamsiToMiladi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 57);
			return now;
		}
	}

	public static DateTime ShamsiToMiladi(this string dateParam)
	{
		return dateParam.ShamsiToMiladi("");
	}

	public static DateTime ShamsiToMiladi(this string dateParam, string time)
	{
		DateTime now = DateTime.Now;
		if (dateParam == null)
		{
			return now;
		}
		checked
		{
			try
			{
				if (LanguageAndCalendar.Instance.SelectedLangauge != 0)
				{
					return Convert.ToDateTime(dateParam);
				}
				time = time?.KeepCharacters("01234567890:");
				dateParam = TextManager.Instance.FixString(dateParam.Trim());
				if (dateParam.Length < 5 || !dateParam.Contains("/"))
				{
					return now;
				}
				if (dateParam.Contains(" "))
				{
					string[] array = dateParam.Split(new char[1] { ' ' });
					dateParam = array[0];
					if (!string.IsNullOrEmpty(time))
					{
						throw new ArgumentException("time exists on date(" + dateParam + ") and also passed as seprated parameter(" + time + ")");
					}
					time = array[1];
					time = time.KeepCharacters("01234567890:");
				}
				if (!dateParam.CheckShamsiDate())
				{
					return now;
				}
				string[] array2 = dateParam.Split(new char[1] { '/' });
				if (array2.Length != 3)
				{
					return now;
				}
				int.TryParse(array2[0], out var result);
				int.TryParse(array2[1], out var result2);
				int.TryParse(array2[2], out var result3);
				if (result3 <= 0 || result3 > 31)
				{
					return now;
				}
				if (result2 <= 0 || result2 > 12)
				{
					return now;
				}
				if (result < 100)
				{
					PersianCalendar persianCalendar = new PersianCalendar();
					result += unchecked(persianCalendar.GetYear(DateTime.Now) / 100) * 100;
				}
				if (result <= 1200 || result >= 1500)
				{
					return now;
				}
				short result4 = 0;
				short result5 = 0;
				short result6 = 0;
				if (!string.IsNullOrEmpty(time) && time.Contains(":"))
				{
					string[] array3 = time.Split(new char[1] { ':' });
					short.TryParse(array3[0], out result4);
					short.TryParse(array3[1], out result6);
					if (array3.Length >= 3)
					{
						short.TryParse(array3[2], out result5);
					}
				}
				else if ((time?.Length ?? 0) == 4)
				{
					result4 = time.Substring(0, 2).ParseToShort(0);
					result6 = time.Substring(2, 2).ParseToShort(0);
				}
				return new PersianCalendar().ToDateTime(result, result2, result3, result4, result6, result5, 0);
			}
			catch (ArgumentException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "Input:" + (dateParam ?? "Null!!"), "ShamsiToMiladi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 133);
				return now;
			}
		}
	}

	public static bool CheckShamsiDate(this string dateSh)
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
			string[] array = dateSh.Split(new char[1] { '/' });
			if (array.Length != 3)
			{
				return false;
			}
			int.TryParse(array[0], out var result);
			int.TryParse(array[1], out var result2);
			int.TryParse(array[2], out var result3);
			if (result3 <= 0 || result3 > 31)
			{
				return false;
			}
			if (result2 <= 0 || result2 > 12)
			{
				return false;
			}
			if (result < 100)
			{
				result = checked(result + 1300);
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
			return result2 != 12 || result3 != 30 || persianCalendar.IsLeapYear(result);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Date:" + dateSh, "CheckShamsiDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 171);
		}
		return false;
	}

	public static string ToShamsiDateTime(this DateTime dateM)
	{
		try
		{
			if (dateM.Year > 2500 || dateM.Year < 1500)
			{
				return "";
			}
			string text = "";
			PersianCalendar persianCalendar = new PersianCalendar();
			text = ((persianCalendar.GetMonth(dateM) >= 10) ? (persianCalendar.GetYear(dateM) + "/" + persianCalendar.GetMonth(dateM)) : (persianCalendar.GetYear(dateM) + "/0" + persianCalendar.GetMonth(dateM)));
			text = ((persianCalendar.GetDayOfMonth(dateM) < 10) ? (text + "/0" + persianCalendar.GetDayOfMonth(dateM)) : (text + "/" + persianCalendar.GetDayOfMonth(dateM)));
			return $"{text} {dateM:HH:mm}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToShamsiDateTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 196);
			return "";
		}
	}

	public static string ToShamsiDate(this DateTime dateM)
	{
		try
		{
			if (dateM.Year > 2500 || dateM.Year < 1500)
			{
				return "";
			}
			string text = "";
			PersianCalendar persianCalendar = new PersianCalendar();
			text = ((persianCalendar.GetMonth(dateM) >= 10) ? (persianCalendar.GetYear(dateM).ToString().Substring(2) + "/" + persianCalendar.GetMonth(dateM)) : (persianCalendar.GetYear(dateM).ToString().Substring(2) + "/0" + persianCalendar.GetMonth(dateM)));
			return (persianCalendar.GetDayOfMonth(dateM) < 10) ? (text + "/0" + persianCalendar.GetDayOfMonth(dateM)) : (text + "/" + persianCalendar.GetDayOfMonth(dateM));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToShamsiDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 225);
			return "";
		}
	}

	public static string MiladiToShamsi(this DateTime dateM)
	{
		try
		{
			if (dateM.Year > 2500 || dateM.Year < 1500)
			{
				return "";
			}
			string text = "";
			PersianCalendar persianCalendar = new PersianCalendar();
			text = ((persianCalendar.GetMonth(dateM) >= 10) ? (persianCalendar.GetYear(dateM) + "/" + persianCalendar.GetMonth(dateM)) : (persianCalendar.GetYear(dateM) + "/0" + persianCalendar.GetMonth(dateM)));
			return (persianCalendar.GetDayOfMonth(dateM) < 10) ? (text + "/0" + persianCalendar.GetDayOfMonth(dateM)) : (text + "/" + persianCalendar.GetDayOfMonth(dateM));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladiToShamsi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 255);
			return "";
		}
	}

	public static string MiladiToShamsi(this DateTime? dateM)
	{
		if (dateM.HasValue)
		{
			return dateM.Value.MiladiToShamsi();
		}
		return "";
	}

	public static string MiladiToShamsiDatePlusTime(this DateTime dateM)
	{
		try
		{
			if (dateM.Year > 2500 || dateM.Year < 1500)
			{
				return "";
			}
			string text = "";
			PersianCalendar persianCalendar = new PersianCalendar();
			text = ((persianCalendar.GetMonth(dateM) >= 10) ? (persianCalendar.GetYear(dateM) + "/" + persianCalendar.GetMonth(dateM)) : (persianCalendar.GetYear(dateM) + "/0" + persianCalendar.GetMonth(dateM)));
			text = ((persianCalendar.GetDayOfMonth(dateM) < 10) ? (text + "/0" + persianCalendar.GetDayOfMonth(dateM)) : (text + "/" + persianCalendar.GetDayOfMonth(dateM)));
			return text + $" {dateM.TimeOfDay.Hours:00}:{dateM.TimeOfDay.Minutes:00}:{dateM.TimeOfDay.Seconds:00}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladiToShamsiDatePlusTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 284);
			return "";
		}
	}

	public static EnSeason GetSeasonOfShamsi(DateTime dateM)
	{
		try
		{
			int monthOfDate = GetMonthOfDate(dateM.MiladiToCalture());
			if (monthOfDate >= 1 && monthOfDate <= 3)
			{
				return EnSeason.Spring;
			}
			if (monthOfDate >= 4 && monthOfDate <= 6)
			{
				return EnSeason.Summer;
			}
			return (monthOfDate >= 7 && monthOfDate <= 9) ? EnSeason.Fall : EnSeason.Winter;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSeasonOfShamsi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 299);
			throw;
		}
	}

	public static EnSeason? GetSeasonOfShamsi(this DateTime? dateM)
	{
		if (dateM.HasValue)
		{
			return GetSeasonOfShamsi(dateM.Value);
		}
		return null;
	}

	public static string ToShortDate(DateTime dateM)
	{
		try
		{
			new PersianCalendar();
			CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fa-IR");
			return dateM.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToShortDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 314);
			return "";
		}
	}

	public static int GetYearOfDateSh(string dateSh)
	{
		try
		{
			if (string.IsNullOrEmpty(dateSh))
			{
				return 0;
			}
			if (dateSh.Split(new char[1] { '/' }).Length <= 2)
			{
				return 0;
			}
			return int.Parse(dateSh.Split(new char[1] { '/' })[0]);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetYearOfDateSh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 331);
			return 0;
		}
	}

	public static int GetYearOfDateSh(this DateTime dateM)
	{
		return GetYearOfDateSh(dateM.MiladiToCalture());
	}

	public static int GetMonthOfDate(string dateSh)
	{
		try
		{
			if (string.IsNullOrEmpty(dateSh))
			{
				return 0;
			}
			if (dateSh.Split(new char[1] { '/' }).Length <= 2)
			{
				return 0;
			}
			return int.Parse(dateSh.Split(new char[1] { '/' })[1]);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMonthOfDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 347);
			return 0;
		}
	}

	public static int GetMonthOfDate(this DateTime dateM)
	{
		return GetMonthOfDate(dateM.MiladiToCalture());
	}

	public static string GetMonthName(this short month)
	{
		checked
		{
			try
			{
				string result = "";
				try
				{
					if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Farsi)
					{
						result = PersianmonthNames[month - 1];
					}
					if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Arbic)
					{
						result = ArbicmonthNames[month - 1];
					}
					if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.EnterNational || LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.English || LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Turkish)
					{
						result = InternationalmonthNames[month - 1];
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetMonthName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 367);
				}
				return result;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetMonthName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 372);
				return "";
			}
		}
	}

	public static string GetMonthName(DateTime date)
	{
		string result = "";
		try
		{
			if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Farsi)
			{
				result = GetMonthName(date.MiladiToShamsi());
			}
			if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Arbic)
			{
				result = GetMonthName(date.MiladiToGhamari());
			}
			if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.EnterNational)
			{
				result = GetMonthName(date.ToString("yyyy/MM/dd hh:mm:ss MMM"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMonthName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 391);
			result = "";
		}
		return result;
	}

	public static string GetMonthName(string dateSh)
	{
		string result = "";
		checked
		{
			try
			{
				if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Farsi)
				{
					result = PersianmonthNames[GetMonthOfDate(dateSh) - 1];
				}
				if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Arbic)
				{
					result = ArbicmonthNames[GetMonthOfDate(dateSh) - 1];
				}
				if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.EnterNational)
				{
					result = InternationalmonthNames[GetMonthOfDate(dateSh) - 1];
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetMonthName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 408);
			}
			return result;
		}
	}

	public static string ToLongDateWithoutYear(DateTime dateM)
	{
		string result = "";
		try
		{
			switch (LanguageAndCalendar.Instance.SelectedLangauge)
			{
			case LanguageTypes.Farsi:
			{
				PersianCalendar persianCalendar = new PersianCalendar();
				CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fa-IR");
				result = cultureInfo.DateTimeFormat.DayNames[(int)persianCalendar.GetDayOfWeek(dateM)] + " " + persianCalendar.GetDayOfMonth(dateM) + " " + GetMonthName(dateM);
				break;
			}
			case LanguageTypes.Arbic:
			{
				HijriCalendar hijriCalendar = new HijriCalendar();
				CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("ar-AR");
				result = cultureInfo.DateTimeFormat.DayNames[(int)hijriCalendar.GetDayOfWeek(dateM)] + " " + hijriCalendar.GetDayOfMonth(dateM) + " " + GetMonthName(dateM);
				break;
			}
			case LanguageTypes.English:
				result = "";
				break;
			case LanguageTypes.Spanish:
				result = "";
				break;
			case LanguageTypes.Turkish:
				result = "";
				break;
			case LanguageTypes.EnterNational:
			{
				GregorianCalendar gregorianCalendar = new GregorianCalendar();
				CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-En");
				result = cultureInfo.DateTimeFormat.DayNames[(int)gregorianCalendar.GetDayOfWeek(dateM)] + " " + gregorianCalendar.GetDayOfMonth(dateM) + " " + GetMonthName(dateM);
				break;
			}
			case LanguageTypes.Georgian:
			case LanguageTypes.Russian:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToLongDateWithoutYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 454);
			return "";
		}
		return result;
	}

	public static string ToLongTimeString(DateTime dateM)
	{
		try
		{
			string text = dateM.ToString("hh:mm:ss");
			return (dateM.Hour <= 12) ? (text + " ق.ظ") : (text + " ب.ظ");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToLongTimeString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 475);
			return "";
		}
	}

	public static string Minutes2TimeFormat(int min)
	{
		try
		{
			min = Math.Abs(min);
			int num = min / 60;
			int num2 = min % 60;
			string text = num.ToString();
			if (num < 10)
			{
				text = "0" + num;
			}
			text += ":";
			return (num2 <= 9) ? (text + "0" + num2) : (text + num2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Minutes2TimeFormat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 500);
			return "00:00";
		}
	}

	public static int EndDayOfMonth(DateTime? dateM = null)
	{
		using (new JaegerMonitor("EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs"))
		{
			try
			{
				if (!dateM.HasValue)
				{
					dateM = DateTime.Now;
				}
				string[] array = dateM.MiladiToCalture().Split(new char[1] { '/' });
				short civilMonth = short.Parse(array[1]);
				int civilyear = int.Parse(array[0]);
				return EndDayOfMonth(civilMonth, civilyear);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 518);
				return 0;
			}
		}
	}

	public static DateTime EndDayOfPersianMonth(this DateTime dateM, string time = "23:59:59")
	{
		using (new JaegerMonitor("EndDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs"))
		{
			try
			{
				string[] array = dateM.MiladiToShamsi().Split(new char[1] { '/' });
				array[2] = EndDayOfMonth(dateM).ToString();
				return (array[0] + "/" + array[1] + "/" + array[2]).ShamsiToMiladi(time);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EndDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 539);
				return DateTime.Now;
			}
		}
	}

	public static DateTime EndDayOfPersianMonth(this string solarDate)
	{
		using (new JaegerMonitor("EndDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs"))
		{
			try
			{
				string[] array = solarDate.Split(new char[1] { '/' });
				array[2] = EndDayOfMonth(solarDate).ToString();
				return (array[0] + "/" + array[1] + "/" + array[2]).ShamsiToMiladi();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EndDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 560);
				return DateTime.Now;
			}
		}
	}

	public static DateTime EndDayOfPersianMonth(short solarMonth, int solarYear)
	{
		using (new JaegerMonitor("EndDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs"))
		{
			try
			{
				string arg = EndDayOfMonth(solarMonth, solarYear).ToString();
				return $"{solarYear}/{solarMonth}/{arg}".ShamsiToMiladi();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EndDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 579);
				return DateTime.Now;
			}
		}
	}

	public static DateTime StartDayOfPersianMonth(this DateTime dateM, string time = "00:00:00")
	{
		using (new JaegerMonitor("StartDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs"))
		{
			try
			{
				string[] array = dateM.MiladiToShamsi().Split(new char[1] { '/' });
				return (array[0] + "/" + array[1] + "/1").ShamsiToMiladi(time);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StartDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 599);
				return DateTime.Now;
			}
		}
	}

	public static DateTime StartDayOfPersianMonth(short solarMonth, int solarYear)
	{
		using (new JaegerMonitor("StartDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs"))
		{
			try
			{
				return $"{solarYear}/{solarMonth}/1".ShamsiToMiladi();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StartDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 617);
				return DateTime.Now;
			}
		}
	}

	public static DateTime StartDayOfPersianMonth(this string solarDate)
	{
		using (new JaegerMonitor("StartDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs"))
		{
			try
			{
				string[] array = solarDate.Split(new char[1] { '/' });
				return (array[0] + "/" + array[1] + "/1").ShamsiToMiladi();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StartDayOfPersianMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 636);
				return DateTime.Now;
			}
		}
	}

	public static int EndDayOfMonth(string solarDate)
	{
		return EndDayOfMonth(checked((short)GetMonthOfDate(solarDate)), GetYearOfDateSh(solarDate));
	}

	public static int EndDayOfMonth(short civilMonth, int civilyear)
	{
		using (new JaegerMonitor("EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs"))
		{
			try
			{
				if ($"{civilyear}/{civilMonth}/31".CheckShamsiDate())
				{
					return 31;
				}
				return $"{civilyear}/{civilMonth}/30".CheckShamsiDate() ? 30 : 29;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EndDayOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 650);
			}
			return 30;
		}
	}

	public static int GetDayOfDateSh(string dateSh)
	{
		int num = 0;
		try
		{
			if (dateSh == "")
			{
				return 0;
			}
			num = 1;
			string[] array = dateSh.Split("/".ToCharArray());
			num = 2;
			return (array.Length > 2) ? int.Parse(array[2]) : 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"The Error Line IS: {num} ", "GetDayOfDateSh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 666);
			return 0;
		}
	}

	public static int GetDayOfDateSh(this DateTime dateM)
	{
		return GetDayOfDateSh(dateM.MiladiToCalture());
	}

	public static string Today()
	{
		try
		{
			return DateTime.Now.Date.MiladiToShamsi();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Today", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 679);
			return null;
		}
	}

	public static DateTime MiladiToday()
	{
		try
		{
			return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladiToday", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 691);
			throw;
		}
	}

	public static DateTime OneWeekAgo()
	{
		try
		{
			DateTime dateTime = DateTime.Now.AddDays(-7.0);
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OneWeekAgo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 704);
			throw;
		}
	}

	public static DateTime ThreeMonthsAgo()
	{
		try
		{
			DateTime dateTime = DateTime.Now.AddMonths(-3);
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ThreeMonthsAgo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 717);
			throw;
		}
	}

	public static string GetPartday(string date, EnPartDate type)
	{
		try
		{
			List<string> list = date.SplitString();
			if (list.Count != 3)
			{
				throw new ArgumentException("input data(" + date + ") is not valid.");
			}
			return type switch
			{
				EnPartDate.Year => list[0], 
				EnPartDate.Mounth => list[1], 
				EnPartDate.Day => list[2], 
				_ => "", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPartday", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 740);
			return null;
		}
	}

	public static int GetDaysInMonth(int y, int m)
	{
		try
		{
			switch (LanguageAndCalendar.Instance.SelectedLangauge)
			{
			case LanguageTypes.Farsi:
				if (m <= 0 || m > 12)
				{
					return 0;
				}
				return new PersianCalendar().GetDaysInMonth(y, m);
			case LanguageTypes.Arbic:
				if (m <= 0 || m > 12)
				{
					return 0;
				}
				if (y <= 1 || y > 2500)
				{
					return 0;
				}
				return CalcEndOfMonthGhamari(y, m);
			case LanguageTypes.English:
			case LanguageTypes.Spanish:
			case LanguageTypes.Turkish:
			case LanguageTypes.EnterNational:
				if (m <= 0 || m > 12)
				{
					return 0;
				}
				return new GregorianCalendar().GetDaysInMonth(y, m);
			default:
				if (m <= 0 || m > 12)
				{
					return 0;
				}
				return new GregorianCalendar().GetDaysInMonth(y, m);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDaysInMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 773);
			return 0;
		}
	}

	public static string AddToShamsiDate(int count, string dateSh, EnPartDate part)
	{
		int num = 1;
		checked
		{
			try
			{
				if (dateSh.Length < 7)
				{
					return DateTime.Now.MiladiToShamsi();
				}
				num = 2;
				string[] array = dateSh.Trim().Split(new char[1] { '/' });
				if (array.Count() != 3)
				{
					return DateTime.Now.MiladiToShamsi();
				}
				num = 3;
				int num2 = int.Parse(array[0]);
				num = 5;
				int num3 = int.Parse(array[1]);
				num = 6;
				int num4 = int.Parse(array[2]);
				num = 7;
				if (num4 <= 0 || num4 > 31)
				{
					return DateTime.Now.MiladiToShamsi();
				}
				num = 8;
				if (num3 <= 0 || num3 > 12)
				{
					return DateTime.Now.MiladiToShamsi();
				}
				num = 9;
				if (num2.ToString().Length == 2)
				{
					num2 = 1400 + num2;
				}
				num = 10;
				if (num2 <= 1200 || num2 >= 1500)
				{
					return DateTime.Now.MiladiToShamsi();
				}
				num = 11;
				DateTime dateTime = dateSh.ShamsiToMiladi();
				int num5 = 0;
				switch (part)
				{
				case EnPartDate.Day:
					num = 12;
					num5 = count;
					break;
				case EnPartDate.Mounth:
					num = 13;
					if (count < 0)
					{
						num3--;
					}
					num5 = count * EndDayOfMonth((short)num3, num2);
					break;
				case EnPartDate.Year:
					num = 14;
					return num2 + count + "/" + num3.ToString("00") + "/" + num4.ToString("00");
				}
				num = 15;
				dateTime = dateTime.AddDays(num5);
				num = 16;
				return dateTime.MiladiToShamsi();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Count:{count}//DateSh:{dateSh}//Part:{part}//Line:{num}", "AddToShamsiDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 826);
				return "";
			}
		}
	}

	public static string MiladitoShamsiFarsi(DateTime dateM)
	{
		string result = "";
		try
		{
			string dateSh = dateM.MiladiToShamsi();
			int dayOfDateSh = GetDayOfDateSh(dateSh);
			result = $"{GetDayNameOfWeek(dateM)} {GetYearOfDateSh(dateSh)}/{GetMonthOfDate(dateSh)}/{dayOfDateSh}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladitoShamsiFarsi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 839);
		}
		return result;
	}

	public static string GetDayNameOfWeek(DateTime dateM)
	{
		try
		{
			if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Farsi)
			{
				return dateM.DayOfWeek switch
				{
					DayOfWeek.Friday => "جمعه", 
					DayOfWeek.Monday => "دوشنبه", 
					DayOfWeek.Saturday => "شنبه", 
					DayOfWeek.Sunday => "یکشنبه", 
					DayOfWeek.Thursday => "پنج شنبه", 
					DayOfWeek.Tuesday => "سه شنبه", 
					DayOfWeek.Wednesday => "چهار شنبه", 
					_ => null, 
				};
			}
			if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.Arbic)
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
			if (LanguageAndCalendar.Instance.SelectedLangauge == LanguageTypes.EnterNational)
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
			return dateM.DayOfWeek switch
			{
				DayOfWeek.Friday => "Friday", 
				DayOfWeek.Monday => "Monday", 
				DayOfWeek.Saturday => "Saturday", 
				DayOfWeek.Sunday => "Sunday", 
				DayOfWeek.Thursday => "Tuesday", 
				DayOfWeek.Tuesday => "Tuesday", 
				DayOfWeek.Wednesday => "Wednesday", 
				_ => null, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDayNameOfWeek", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 910);
			return "";
		}
	}

	public static string TelegramTime(this DateTime date, bool forWeb = false)
	{
		try
		{
			if (date.Year < 1950)
			{
				return "نامعلوم";
			}
			if ((DateTime.Now - date).Minutes <= -2)
			{
				throw new Exception($"دوست عزیز تاریخ اینده قابل نمایش نیست//Now:{DateTime.Now}//date:{date}");
			}
			if ((DateTime.Now - date).TotalMilliseconds < 0.0)
			{
				return "هم اکنون";
			}
			if (date.Date == DateTime.Now.Date)
			{
				return date.ToString("t");
			}
			if ((DateTime.Now - date).TotalDays < 2.0)
			{
				return "دیروز";
			}
			if ((DateTime.Now - date).TotalDays < 8.0)
			{
				return GetShamsiDayShortName(date.DayOfWeek);
			}
			PersianCalendar persianCalendar = new PersianCalendar();
			int month = persianCalendar.GetMonth(date);
			int dayOfMonth = persianCalendar.GetDayOfMonth(date);
			int year = persianCalendar.GetYear(date);
			bool flag = persianCalendar.IsLeapYear(persianCalendar.GetYear(date));
			if (year == persianCalendar.GetYear(DateTime.Now) && ((month < 7 && dayOfMonth < 31) || (flag && dayOfMonth < 31) || dayOfMonth < 30))
			{
				return checked((!forWeb) ? $"{PersianmonthNames[month - 1]} {dayOfMonth}" : $"{dayOfMonth} {PersianmonthNames[month - 1]}");
			}
			return persianCalendar.GetYear(date).ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TelegramTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 938);
			return date.MiladiToCalture();
		}
	}

	public static string CallCenterDate(this DateTime date)
	{
		try
		{
			if (date.Year < 1950)
			{
				return "نامعلوم";
			}
			if ((DateTime.Now - date).Minutes <= -2)
			{
				throw new Exception($"دوست عزیز تاریخ اینده قابل نمایش نیست//Now:{DateTime.Now}//date:{date}");
			}
			if ((DateTime.Now - date).TotalMilliseconds < 0.0)
			{
				return "هم اکنون";
			}
			if (date.Date == DateTime.Now.Date)
			{
				return "امروز";
			}
			if ((DateTime.Now - date).TotalDays < 1.0)
			{
				return "دیروز";
			}
			if ((DateTime.Now - date).TotalDays < 8.0)
			{
				return GetShamsiDayShortName(date.DayOfWeek);
			}
			PersianCalendar persianCalendar = new PersianCalendar();
			int month = persianCalendar.GetMonth(date);
			int dayOfMonth = persianCalendar.GetDayOfMonth(date);
			int year = persianCalendar.GetYear(date);
			bool flag = persianCalendar.IsLeapYear(persianCalendar.GetYear(date));
			return (year == persianCalendar.GetYear(DateTime.Now) && ((month < 7 && dayOfMonth < 31) || (flag && dayOfMonth < 31) || dayOfMonth < 30)) ? $"{dayOfMonth} {PersianmonthNames[checked(month - 1)]}" : persianCalendar.GetYear(date).ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CallCenterDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 963);
			return date.MiladiToCalture();
		}
	}

	public static string CallCenterTime(this DateTime date)
	{
		try
		{
			if (date.Year < 1950)
			{
				return "نامعلوم";
			}
			if ((DateTime.Now - date).Minutes <= -2)
			{
				throw new Exception($"دوست عزیز تاریخ اینده قابل نمایش نیست//Now:{DateTime.Now}//date:{date}");
			}
			if ((DateTime.Now - date).TotalMilliseconds <= 0.0 && (DateTime.Now - date).TotalMilliseconds >= 1.0)
			{
				return "هم اکنون";
			}
			if (date.Date == DateTime.Now.Date)
			{
				TimeSpan timeSpan = DateTime.Now - date;
				if (timeSpan.TotalMinutes < 1.0 && timeSpan.Minutes >= 0)
				{
					return "هم اکنون";
				}
				if (timeSpan.TotalHours <= 1.0)
				{
					return $"{Math.Floor(timeSpan.TotalMinutes)} دقیقه قبل";
				}
			}
			return $"{date.Hour}:{date.Minute}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CallCenterTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 985);
			return date.MiladiToCalture();
		}
	}

	public static string MiladiToCalture(this DateTime dateM)
	{
		try
		{
			switch (LanguageAndCalendar.Instance.SelectedLangauge)
			{
			case LanguageTypes.Arbic:
				return dateM.MiladiToGhamari();
			case LanguageTypes.English:
				return dateM.ToString().ShamsiToMiladi().ToString();
			case LanguageTypes.Georgian:
			case LanguageTypes.Russian:
			case LanguageTypes.Spanish:
			case LanguageTypes.Turkish:
			case LanguageTypes.EnterNational:
				return dateM.ToString("yyyy/MM/dd");
			case LanguageTypes.Farsi:
				return dateM.MiladiToShamsi();
			default:
				return dateM.MiladiToShamsi();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladiToCalture", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1009);
			return "";
		}
	}

	public static string MiladiToCalture(this DateTime? dateM)
	{
		try
		{
			if (!dateM.HasValue)
			{
				return "";
			}
			switch (LanguageAndCalendar.Instance.SelectedLangauge)
			{
			case LanguageTypes.Arbic:
				return dateM.Value.MiladiToGhamari();
			case LanguageTypes.English:
			case LanguageTypes.Spanish:
			case LanguageTypes.Turkish:
			case LanguageTypes.EnterNational:
				return dateM.Value.ToString("yyyy/MM/dd");
			case LanguageTypes.Farsi:
				return dateM.Value.MiladiToShamsi();
			default:
				return dateM.Value.MiladiToShamsi();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladiToCalture", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1035);
			return "";
		}
	}

	private static string GetShamsiDayShortName(DayOfWeek dayOfWeek)
	{
		return dayOfWeek switch
		{
			DayOfWeek.Friday => "جمعه", 
			DayOfWeek.Monday => "دوشنبه", 
			DayOfWeek.Saturday => "شنبه", 
			DayOfWeek.Sunday => "یکشنبه", 
			DayOfWeek.Thursday => "پنج شنبه", 
			DayOfWeek.Tuesday => "سه شنبه", 
			DayOfWeek.Wednesday => "چهار شنبه", 
			_ => "", 
		};
	}

	public static string DateToFormatTime(this DateTime dateM)
	{
		checked
		{
			try
			{
				int num = dateM.Hour;
				int num2 = dateM.Minute;
				if (dateM.Second > 30)
				{
					num2++;
					if (num2 == 60)
					{
						num2 = 0;
						num++;
					}
				}
				return num.ToString("00") + ":" + num2.ToString("00");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DateToFormatTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\PacketParser\\Calendar.cs", 645);
				return "";
			}
		}
	}

	public static string AddYear(this string shamsiDate, int yearCount)
	{
		try
		{
			if (!shamsiDate.CheckShamsiDate())
			{
				return null;
			}
			string[] array = shamsiDate.Split(new char[1] { '/' });
			PersianCalendar persianCalendar = new PersianCalendar();
			return persianCalendar.AddYears(new DateTime(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), persianCalendar), yearCount).MiladiToShamsi();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1116);
			return "";
		}
	}

	public static string AddMonth(this string shamsiDate, int monthCount)
	{
		try
		{
			if (!shamsiDate.CheckShamsiDate())
			{
				return null;
			}
			string[] array = shamsiDate.Split(new char[1] { '/' });
			PersianCalendar persianCalendar = new PersianCalendar();
			return persianCalendar.AddMonths(new DateTime(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), persianCalendar), monthCount).MiladiToShamsi();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1133);
			return "";
		}
	}

	public static string AddDay(this string shamsiDate, int dayCount)
	{
		try
		{
			if (!shamsiDate.CheckShamsiDate())
			{
				return null;
			}
			string[] array = shamsiDate.Split(new char[1] { '/' });
			PersianCalendar persianCalendar = new PersianCalendar();
			return persianCalendar.AddDays(new DateTime(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), persianCalendar), dayCount).MiladiToShamsi();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1151);
			return "";
		}
	}

	public static string GhamariToShamsi(this string DateParam)
	{
		return DateParam.GhamariToMiladi().MiladiToShamsi();
	}

	public static string ShamsiToGhamari(this string DateParam)
	{
		return DateParam.ShamsiToMiladi().MiladiToGhamari();
	}

	public static DateTime GhamariToMiladi(this string dateParam, DateTime defDate = default(DateTime))
	{
		if (defDate == default(DateTime))
		{
			defDate = DateTime.Now;
		}
		try
		{
			dateParam = dateParam.Trim();
			if (dateParam.Length < 6)
			{
				return defDate;
			}
			string[] array = dateParam.Split(new char[1] { ' ' });
			if (array.Length == 0)
			{
				return defDate;
			}
			string input = "";
			if (array.Length != 0)
			{
				input = array[0];
			}
			input = TextManager.Instance.RemoveNoNumbers(input, "/").Replace("//", "/");
			if (input.Length < 6)
			{
				return defDate;
			}
			string[] array2 = input.Split(new char[1] { '/' });
			if (array2.Length != 3)
			{
				return defDate;
			}
			double num = double.Parse(array2[0]);
			double num2 = double.Parse(array2[1]);
			double num3 = double.Parse(array2[2]);
			if (num3 <= 0.0 || num3 > 30.0)
			{
				return defDate;
			}
			if (num2 <= 0.0 || num2 > 12.0)
			{
				return defDate;
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
			defDate = Convert.ToDateTime($"{num}/{num2}/{num3}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GhamariToMiladi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1219);
		}
		return defDate;
	}

	private static double getInt(double fNumber)
	{
		if (!(fNumber < -1E-07))
		{
			return Math.Floor(fNumber + 1E-07);
		}
		return Math.Ceiling(fNumber - 1E-07);
	}

	public static string MiladiToGhamari(this DateTime dateParam)
	{
		try
		{
			if (dateParam >= new DateTime(2012, 4, 22))
			{
				dateParam = dateParam.AddDays(-1.0);
			}
			return dateParam.ToString("yyyy/MM/dd", new CultureInfo("ar"));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladiToGhamari", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1260);
			return "";
		}
	}

	public static string MiladiToMiladi(this DateTime dateParam)
	{
		try
		{
			if (dateParam >= new DateTime(2012, 4, 22))
			{
				dateParam = dateParam.AddDays(-1.0);
			}
			return dateParam.ToString("yyyy/MM/dd hh:mm", new CultureInfo("en"));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MiladiToMiladi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1294);
			return "";
		}
	}

	private static string StandardGhamariFormat(int y, int m, int d, TimeSpan time)
	{
		try
		{
			return $"{y:0000}/{m:00}/{d:00} {time}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StandardGhamariFormat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1319);
		}
		return "";
	}

	public static string GetGhamariMonth(this int GhamariMonth)
	{
		try
		{
			return GhamariMonth switch
			{
				1 => "محرم", 
				2 => "صفر", 
				3 => "ربیع\u200cالاول", 
				4 => "ربیع\u200cالثانی", 
				5 => "جمادی\u200cالاول", 
				6 => "جمادی\u200cالثانی", 
				7 => "رجب", 
				8 => "شعبان", 
				9 => "رمضان", 
				10 => "شوال", 
				11 => "ذیقعده", 
				12 => "ذیحجه", 
				_ => "لا مفهوم", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGhamariMonth", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1346);
			return "لا مفهوم";
		}
	}

	private static int CalcEndOfMonthGhamari(int yh, int mh)
	{
		try
		{
			int[] array = new int[12]
			{
				30, 29, 30, 29, 30, 29, 30, 29, 30, 29,
				30, 29
			};
			int num = yh % 30;
			int num2 = ((mh == 12 && (num == 2 || num == 5 || num == 7 || num == 10 || num == 13 || num == 16 || num == 18 || num == 21 || num == 24 || num == 26 || num == 29)) ? 1 : 0);
			return (30 > checked(array[mh - 1] + num2)) ? 29 : 30;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcEndOfMonthGhamari", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Calendar.cs", 1363);
			return 0;
		}
	}

	public static int RemainingDays(DateTime expireDate)
	{
		TimeSpan timeSpan = expireDate - DateTime.Now;
		if (timeSpan.Days > 0)
		{
			return timeSpan.Days;
		}
		return 0;
	}

	public static int UsedDays(DateTime expireDate)
	{
		TimeSpan timeSpan = expireDate - DateTime.Now;
		if (timeSpan.Days <= 0)
		{
			return 365;
		}
		checked
		{
			if (timeSpan.Days <= 365)
			{
				return 365 - timeSpan.Days;
			}
			if (timeSpan.Days > 1825)
			{
				return 3650 - timeSpan.Days;
			}
			return 1825 - timeSpan.Days;
		}
	}

	public static int GetWeekNumber(this DateTime dateM, CalendarWeekRule role = CalendarWeekRule.FirstFullWeek)
	{
		return LanguageAndCalendar.Instance.CaltureCalender.GetWeekNumber(dateM, role);
	}

	public static DateTime GetFirstDayOfWeek(this DateTime dateM, string time = "00:00:00")
	{
		return LanguageAndCalendar.Instance.CaltureCalender.GetFirstDayOfWeek(dateM, time);
	}

	public static DateTime GetEndDayOfWeek(this DateTime dateM, string time = "23:59:59")
	{
		return LanguageAndCalendar.Instance.CaltureCalender.GetEndDayOfWeek(dateM, time);
	}
}
