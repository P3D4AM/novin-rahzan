using System;
using System.ComponentModel;
using System.Globalization;

namespace BehComponents;

[Description("نمونه ای از تاریخ شمسی و زمان را ارائه میکند")]
public struct PersianDateTime
{
	private static string _defLongDate = "dddd d MMMM y";

	private static string _defShortDate = "y/M/d";

	private static string _defLongTime = "hh:mm:ss tt";

	private static string _defShortTime = "hh:mm tt";

	private static string _LongDateFormat = "dddd d MMMM y";

	private static string _ShortDateFormat = "y/M/d";

	private static string _LongTimeFormat = "hh:mm:ss tt";

	private static string _ShortTimeFormat = "hh:mm tt";

	private bool _blnManualInit;

	private int _Year;

	private int _Month;

	private int _Day;

	private DayOfWeek _DayOfWeek;

	private int _DayOfYear;

	private int _Hour;

	private int _Minute;

	private int _Second;

	private int _Millisecond;

	private TimeSpan _TimeOfDay;

	public static string LongDateFormat
	{
		get
		{
			return _LongDateFormat;
		}
		set
		{
			_LongDateFormat = value;
		}
	}

	public static string ShortDateFormat
	{
		get
		{
			return _ShortDateFormat;
		}
		set
		{
			_ShortDateFormat = value;
		}
	}

	public static string LongTimeFormat
	{
		get
		{
			return _LongTimeFormat;
		}
		set
		{
			_LongTimeFormat = value;
		}
	}

	public static string ShortTimeFormat
	{
		get
		{
			return _ShortTimeFormat;
		}
		set
		{
			_ShortTimeFormat = value;
		}
	}

	[Browsable(false)]
	public string Programmer => "Saeed Mohammadi";

	[Browsable(false)]
	public string ProgrammerWebSite => "http://BehSoft.blogfa.com/";

	public int Year
	{
		get
		{
			if (!_blnManualInit)
			{
				PersianCalendar persianCalendar = new PersianCalendar();
				DateTime minSupportedDateTime = persianCalendar.MinSupportedDateTime;
				_Year = persianCalendar.GetYear(minSupportedDateTime);
			}
			return _Year;
		}
	}

	public int Month
	{
		get
		{
			if (!_blnManualInit)
			{
				PersianCalendar persianCalendar = new PersianCalendar();
				DateTime minSupportedDateTime = persianCalendar.MinSupportedDateTime;
				_Month = persianCalendar.GetMonth(minSupportedDateTime);
			}
			return _Month;
		}
	}

	public int Day
	{
		get
		{
			if (!_blnManualInit)
			{
				PersianCalendar persianCalendar = new PersianCalendar();
				DateTime minSupportedDateTime = persianCalendar.MinSupportedDateTime;
				_Day = persianCalendar.GetDayOfMonth(minSupportedDateTime);
			}
			return _Day;
		}
	}

	public PersianDateTime Date => new PersianDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);

	public DayOfWeek DayOfWeek
	{
		get
		{
			if (!_blnManualInit)
			{
				PersianCalendar persianCalendar = new PersianCalendar();
				DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
				DayOfWeek dayOfWeek = persianCalendar.GetDayOfWeek(time);
				_DayOfWeek = dayOfWeek;
			}
			return _DayOfWeek;
		}
	}

	public int DayOfYear
	{
		get
		{
			if (!_blnManualInit)
			{
				PersianCalendar persianCalendar = new PersianCalendar();
				DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
				int dayOfYear = persianCalendar.GetDayOfYear(time);
				_DayOfYear = dayOfYear;
			}
			return _DayOfYear;
		}
	}

	public int Hour
	{
		get
		{
			if (!_blnManualInit)
			{
				_Hour = 0;
			}
			return _Hour;
		}
	}

	public int Minute
	{
		get
		{
			if (!_blnManualInit)
			{
				_Minute = 0;
			}
			return _Minute;
		}
	}

	public int Second
	{
		get
		{
			if (!_blnManualInit)
			{
				_Second = 0;
			}
			return _Second;
		}
	}

	public int Millisecond
	{
		get
		{
			if (!_blnManualInit)
			{
				_Millisecond = 0;
			}
			return _Millisecond;
		}
	}

	public TimeSpan TimeOfDay
	{
		get
		{
			if (!_blnManualInit)
			{
				_TimeOfDay = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
			}
			return _TimeOfDay;
		}
	}

	public static PersianDateTime Now
	{
		get
		{
			PersianCalendar persianCalendar = new PersianCalendar();
			int year = persianCalendar.GetYear(DateTime.Now);
			int month = persianCalendar.GetMonth(DateTime.Now);
			int dayOfMonth = persianCalendar.GetDayOfMonth(DateTime.Now);
			int hour = DateTime.Now.Hour;
			int minute = DateTime.Now.Minute;
			int second = DateTime.Now.Second;
			int millisecond = DateTime.Now.Millisecond;
			return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
		}
	}

	public static PersianDateTime Today
	{
		get
		{
			PersianCalendar persianCalendar = new PersianCalendar();
			int year = persianCalendar.GetYear(DateTime.Now);
			int month = persianCalendar.GetMonth(DateTime.Now);
			int dayOfMonth = persianCalendar.GetDayOfMonth(DateTime.Now);
			return new PersianDateTime(year, month, dayOfMonth, 0, 0, 0, 0);
		}
	}

	public static void SetDefaultFormats()
	{
		_LongDateFormat = _defLongDate;
		_LongTimeFormat = _defLongTime;
		_ShortDateFormat = _defShortDate;
		_ShortTimeFormat = _defShortTime;
	}

	public PersianDateTime(int year, int month, int day)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
		_blnManualInit = true;
		_Year = year;
		_Month = month;
		_Day = day;
		_Hour = 0;
		_Minute = 0;
		_Second = 0;
		_Millisecond = 0;
		DayOfWeek dayOfWeek = persianCalendar.GetDayOfWeek(time);
		_DayOfWeek = dayOfWeek;
		int dayOfYear = persianCalendar.GetDayOfYear(time);
		_DayOfYear = dayOfYear;
		_TimeOfDay = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
	}

	public PersianDateTime(int year, int month, int day, int hour, int minute, int second)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);
		_blnManualInit = true;
		_Year = year;
		_Month = month;
		_Day = day;
		_Hour = hour;
		_Minute = minute;
		_Second = second;
		_Millisecond = 0;
		DayOfWeek dayOfWeek = persianCalendar.GetDayOfWeek(time);
		_DayOfWeek = dayOfWeek;
		int dayOfYear = persianCalendar.GetDayOfYear(time);
		_DayOfYear = dayOfYear;
		_TimeOfDay = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
	}

	public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(year, month, day, hour, minute, second, millisecond);
		_blnManualInit = true;
		_Year = year;
		_Month = month;
		_Day = day;
		_Hour = hour;
		_Minute = minute;
		_Second = second;
		_Millisecond = millisecond;
		DayOfWeek dayOfWeek = persianCalendar.GetDayOfWeek(time);
		_DayOfWeek = dayOfWeek;
		int dayOfYear = persianCalendar.GetDayOfYear(time);
		_DayOfYear = dayOfYear;
		_TimeOfDay = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
	}

	public PersianDateTime(DateTime value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		_blnManualInit = true;
		_Year = persianCalendar.GetYear(value);
		_Month = persianCalendar.GetMonth(value);
		_Day = persianCalendar.GetDayOfMonth(value);
		_Hour = value.Hour;
		_Minute = value.Minute;
		_Second = value.Second;
		_Millisecond = value.Millisecond;
		DayOfWeek dayOfWeek = persianCalendar.GetDayOfWeek(value);
		_DayOfWeek = dayOfWeek;
		int dayOfYear = persianCalendar.GetDayOfYear(value);
		_DayOfYear = dayOfYear;
		_TimeOfDay = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
	}

	public PersianDateTime AddDays(double value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond).AddDays(value);
		int year = persianCalendar.GetYear(time);
		int month = persianCalendar.GetMonth(time);
		int dayOfMonth = persianCalendar.GetDayOfMonth(time);
		int hour = time.Hour;
		int minute = time.Minute;
		int second = time.Second;
		int millisecond = time.Millisecond;
		return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
	}

	public PersianDateTime AddHours(double value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond).AddHours(value);
		int year = persianCalendar.GetYear(time);
		int month = persianCalendar.GetMonth(time);
		int dayOfMonth = persianCalendar.GetDayOfMonth(time);
		int hour = time.Hour;
		int minute = time.Minute;
		int second = time.Second;
		int millisecond = time.Millisecond;
		return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
	}

	public PersianDateTime AddMillisecond(double value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond).AddMilliseconds(value);
		int year = persianCalendar.GetYear(time);
		int month = persianCalendar.GetMonth(time);
		int dayOfMonth = persianCalendar.GetDayOfMonth(time);
		int hour = time.Hour;
		int minute = time.Minute;
		int second = time.Second;
		int millisecond = time.Millisecond;
		return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
	}

	public PersianDateTime AddMinutes(double value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond).AddMinutes(value);
		int year = persianCalendar.GetYear(time);
		int month = persianCalendar.GetMonth(time);
		int dayOfMonth = persianCalendar.GetDayOfMonth(time);
		int hour = time.Hour;
		int minute = time.Minute;
		int second = time.Second;
		int millisecond = time.Millisecond;
		return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
	}

	public PersianDateTime AddMonths(int months)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
		time = persianCalendar.AddMonths(time, months);
		int year = persianCalendar.GetYear(time);
		int month = persianCalendar.GetMonth(time);
		int dayOfMonth = persianCalendar.GetDayOfMonth(time);
		int hour = time.Hour;
		int minute = time.Minute;
		int second = time.Second;
		int millisecond = time.Millisecond;
		return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
	}

	public PersianDateTime AddSeconds(double value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond).AddSeconds(value);
		int year = persianCalendar.GetYear(time);
		int month = persianCalendar.GetMonth(time);
		int dayOfMonth = persianCalendar.GetDayOfMonth(time);
		int hour = time.Hour;
		int minute = time.Minute;
		int second = time.Second;
		int millisecond = time.Millisecond;
		return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
	}

	public PersianDateTime AddYears(int value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
		time = persianCalendar.AddYears(time, value);
		int year = persianCalendar.GetYear(time);
		int month = persianCalendar.GetMonth(time);
		int dayOfMonth = persianCalendar.GetDayOfMonth(time);
		int hour = time.Hour;
		int minute = time.Minute;
		int second = time.Second;
		int millisecond = time.Millisecond;
		return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
	}

	public int CompareTo(PersianDateTime value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime dateTime = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
		DateTime value2 = persianCalendar.ToDateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
		return dateTime.CompareTo(value2);
	}

	public bool Equals(PersianDateTime value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime dateTime = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
		DateTime value2 = persianCalendar.ToDateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
		return dateTime.Equals(value2);
	}

	public TimeSpan Subtract(PersianDateTime value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime dateTime = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
		DateTime value2 = persianCalendar.ToDateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
		return dateTime.Subtract(value2);
	}

	public PersianDateTime Subtract(TimeSpan value)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime time = persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond).Subtract(value);
		int year = persianCalendar.GetYear(time);
		int month = persianCalendar.GetMonth(time);
		int dayOfMonth = persianCalendar.GetDayOfMonth(time);
		int hour = time.Hour;
		int minute = time.Minute;
		int second = time.Second;
		int millisecond = time.Millisecond;
		return new PersianDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
	}

	public string ToLongDateString()
	{
		return ToFormat(_LongDateFormat);
	}

	public string ToLongTimeString()
	{
		return ToFormat(_LongTimeFormat);
	}

	public string ToShortDateString()
	{
		return ToFormat(_ShortDateFormat);
	}

	public string ToShortTimeString()
	{
		return ToFormat(_ShortTimeFormat);
	}

	private static char[] FormatChars()
	{
		return new char[13]
		{
			'd', 'M', 'm', 'y', 'h', 'H', 's', 't', 'f', 'F',
			'"', '\'', '\\'
		};
	}

	private string ToFormat(string format)
	{
		string text = "";
		string text2 = "";
		char[] anyOf = FormatChars();
		for (int i = 0; i < format.Length; i++)
		{
			int num = format[i].ToString().IndexOfAny(anyOf);
			if (num == -1)
			{
				text2 += format[i];
				continue;
			}
			bool isCaseSensitive = true;
			int num2 = clsCommonFunctions.CountChar(format, i, isCaseSensitive);
			if (format[i] == 'd')
			{
				if (num2 <= 1)
				{
					text = Day.ToString();
				}
				else if (num2 > 1 && num2 < 4)
				{
					text = ((Day < 10) ? ("0" + Day) : Day.ToString());
					while (text.Length < num2)
					{
						text = " " + text;
					}
				}
				else
				{
					text = clsCommonFunctions.NameOfDayOfWeek(DayOfWeek);
				}
				text2 += text;
			}
			else if (format[i] == 'M')
			{
				if (num2 <= 1)
				{
					text = Month.ToString();
				}
				else if (num2 > 1 && num2 < 4)
				{
					text = ((Month < 10) ? ("0" + Month) : Month.ToString());
					while (text.Length < num2)
					{
						text = " " + text;
					}
				}
				else
				{
					text = clsCommonFunctions.MonthOfYear(CalendarTypes.Persian, Month);
				}
				text2 += text;
			}
			else if (format[i] == 'y')
			{
				text = Year.ToString();
				if (num2 > text.Length)
				{
					text = Year.ToString();
					while (text.Length < num2)
					{
						text = " " + text;
					}
				}
				text2 += text;
			}
			else if (format[i] == 'h' || format[i] == 'H')
			{
				int num3 = Hour;
				if (num3 > 12 && format[i] == 'h')
				{
					num3 -= 12;
				}
				if (num3 == 0 && format[i] == 'h')
				{
					num3 = 12;
				}
				if (num2 <= 1)
				{
					text = num3.ToString();
				}
				else
				{
					text = ((num3 < 10) ? ("0" + num3) : num3.ToString());
					while (text.Length < num2)
					{
						text = " " + text;
					}
				}
				text2 += text;
			}
			else if (format[i] == 'm')
			{
				if (num2 <= 1)
				{
					text = Minute.ToString();
				}
				else
				{
					text = ((Minute < 10) ? ("0" + Minute) : Minute.ToString());
					while (text.Length < num2)
					{
						text = " " + text;
					}
				}
				text2 += text;
			}
			else if (format[i] == 's')
			{
				if (num2 <= 1)
				{
					text = Second.ToString();
				}
				else
				{
					text = ((Second < 10) ? ("0" + Second) : Second.ToString());
					while (text.Length < num2)
					{
						text = " " + text;
					}
				}
				text2 += text;
			}
			else if (format[i] == 'f' || format[i] == 'F')
			{
				text = Millisecond.ToString();
				if (num2 <= text.Length)
				{
					text = text.Substring(0, num2);
				}
				text2 += text;
			}
			else if (format[i] == 't')
			{
				int hour = Hour;
				text = ((hour < 12) ? "ق" : "ب");
				if (num2 > 1)
				{
					text += " ظ";
				}
				text2 += text;
			}
			else if (format[i] == '\'' || format[i] == '"')
			{
				int num4 = 0;
				bool flag = false;
				while (!flag)
				{
					num4 = format.IndexOf(format[i], i + num4 + 1);
					if (num4 == -1)
					{
						throw new FormatException("Cannot find a matching quote character for the character \" or '");
					}
					if (format[num4 - 1] == '\\')
					{
						num4++;
					}
					else
					{
						flag = true;
					}
				}
				text = format.Substring(i + 1, num4 - i - 1);
				for (int j = 0; j < text.Length; j++)
				{
					if (text[j] == '\\')
					{
						text = text.Remove(j, 1);
					}
				}
				text2 += text;
				i = num4;
				num2 = 1;
			}
			else if (format[i] == '\\')
			{
				text = format[i + 1].ToString();
				text2 += text;
				i++;
				num2 = 1;
			}
			else
			{
				text2 += format[i];
			}
			i += num2 - 1;
		}
		return text2;
	}

	public string ToStringFormat(string format)
	{
		string text = "";
		return ToFormat(format);
	}

	public override string ToString()
	{
		string format = _ShortDateFormat + " " + _ShortTimeFormat;
		string text = "";
		return ToFormat(format);
	}

	public static int Compare(PersianDateTime t1, PersianDateTime t2)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime t3 = persianCalendar.ToDateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second, t1.Millisecond);
		DateTime t4 = persianCalendar.ToDateTime(t2.Year, t2.Month, t2.Day, t2.Hour, t2.Minute, t2.Second, t2.Millisecond);
		return DateTime.Compare(t3, t4);
	}

	public static int DaysInMonth(int year, int month)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		return persianCalendar.GetDaysInMonth(year, month);
	}

	public static bool Equals(PersianDateTime t1, PersianDateTime t2)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime t3 = persianCalendar.ToDateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second, t1.Millisecond);
		DateTime t4 = persianCalendar.ToDateTime(t2.Year, t2.Month, t2.Day, t2.Hour, t2.Minute, t2.Second, t2.Millisecond);
		return DateTime.Equals(t3, t4);
	}

	public static bool IsLeapYear(int year)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		return persianCalendar.IsLeapYear(year);
	}

	private static bool DoParse2(string s, string format, bool blnTryParse, out PersianDateTime dateParsed)
	{
		int year = 1;
		int month = 1;
		int num = 1;
		int num2 = 0;
		int minute = 0;
		int second = 0;
		int millisecond = 0;
		int num3 = 0;
		string text = "";
		char[] anyOf = FormatChars();
		char c = ' ';
		int num4 = 0;
		int num5 = 0;
		bool isCaseSensitive = true;
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < format.Length; i++)
		{
			num5 = 0;
			int num6 = clsCommonFunctions.CountChar(format, i, isCaseSensitive);
			char c2 = format[i];
			if (c2.ToString().IndexOfAny(anyOf) == -1)
			{
				num4++;
				continue;
			}
			if (c2 != '\'' && flag2)
			{
				num4++;
				continue;
			}
			if (c2 != '"' && flag3)
			{
				num4++;
				continue;
			}
			bool flag4;
			if (i + num6 < format.Length)
			{
				c = format[i + num6];
				flag4 = false;
			}
			else
			{
				flag4 = true;
			}
			int num7 = 0;
			if (flag4)
			{
				text = s.Substring(num4);
			}
			else
			{
				num7 = s.IndexOf(c, num4);
				if (num7 == -1)
				{
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				else
				{
					text = s.Substring(num4, num7 - num4);
				}
			}
			i += num6 - 1;
			num4 = num7;
			switch (c2)
			{
			case 'y':
				if (!int.TryParse(text, out num5))
				{
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				if (num5 < 1 || num5 > 9999)
				{
					num5 = 1;
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				year = num5;
				continue;
			case 'M':
				if (num6 >= 4)
				{
					num5 = clsCommonFunctions.MonthOfYear(CalendarTypes.Persian, text);
				}
				else if (!int.TryParse(text, out num5))
				{
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				if (num5 < 1 || num5 > 12)
				{
					num5 = 1;
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				month = num5;
				continue;
			case 'd':
				if (num6 >= 1 && num6 <= 3)
				{
					if (!int.TryParse(text, out num5))
					{
						flag = false;
						if (!blnTryParse)
						{
							break;
						}
					}
					if (num5 < 1 || num5 > 31)
					{
						num5 = 1;
						flag = false;
						if (!blnTryParse)
						{
							break;
						}
					}
					num = num5;
				}
				else
				{
					string text2 = "شنبه";
					num7 = s.IndexOf(text2, num4);
					if (num7 != -1)
					{
						num4 = num7 + text2.Length;
					}
				}
				continue;
			case 'H':
			case 'h':
				if (!int.TryParse(text, out num5))
				{
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				if ((c2 == 'H' && (num5 < 0 || num5 > 23)) || (c2 == 'h' && (num5 < 0 || num5 > 12)))
				{
					num5 = 0;
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				num2 = num5;
				continue;
			case 'm':
				if (!int.TryParse(text, out num5))
				{
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				if (num5 < 0 || num5 > 59)
				{
					num5 = 0;
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				minute = num5;
				continue;
			case 's':
				if (!int.TryParse(text, out num5))
				{
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				if (num5 < 0 || num5 > 59)
				{
					num5 = 0;
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				second = num5;
				continue;
			case 'F':
			case 'f':
				if (!int.TryParse(text, out num5))
				{
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				if (num5 < 0 || num5 > 999)
				{
					num5 = 0;
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				millisecond = num5;
				continue;
			case 't':
				if (text.IndexOf('ب') != -1)
				{
					num3 = 12;
				}
				else if (text.IndexOf('ق') != -1)
				{
					num3 = -12;
				}
				else
				{
					num3 = 0;
					flag = false;
					if (!blnTryParse)
					{
						break;
					}
				}
				if (num6 > 1 && c == ' ')
				{
					num4 += 2;
				}
				continue;
			case '\'':
				flag2 = !flag2;
				continue;
			case '"':
				flag3 = !flag3;
				continue;
			case '\\':
				i++;
				num4++;
				continue;
			default:
				continue;
			}
			break;
		}
		num2 += num3;
		if (num2 > 23)
		{
			num2 = 0;
		}
		if (num2 < 0)
		{
			num2 -= num3;
		}
		int num8 = DaysInMonth(year, month);
		if (num > num8)
		{
			flag = false;
			num = num8;
		}
		PersianDateTime persianDateTime = (flag ? new PersianDateTime(year, month, num, num2, minute, second, millisecond) : ((!blnTryParse) ? new PersianDateTime(year, month, num, num2, minute, second, millisecond) : default(PersianDateTime)));
		dateParsed = persianDateTime;
		return flag;
	}

	private static bool DoParse(string s, string format, bool blnTryParse, out PersianDateTime dateParsed)
	{
		int year = 1;
		int month = 1;
		int day = 1;
		int num = 0;
		int minute = 0;
		int second = 0;
		int millisecond = 0;
		int num2 = 0;
		bool flag = true;
		string text = "";
		int result = 0;
		int num3 = 0;
		int length = s.Length;
		char[] anyOf = FormatChars();
		for (int i = 0; i < format.Length; i++)
		{
			_ = format[i];
			int num4 = format[i].ToString().IndexOfAny(anyOf);
			if (num4 > -1)
			{
				bool isCaseSensitive = true;
				int num5 = clsCommonFunctions.CountChar(format, i, isCaseSensitive);
				if (format[i] == 'd')
				{
					if (num5 >= 1 && num5 <= 3)
					{
						int j = num3;
						int num6 = num3;
						for (; j < length && (s[j] < '0' || s[j] > '9'); j++)
						{
						}
						for (num6 = j; num6 < length && s[num6] >= '0' && s[num6] <= '9'; num6++)
						{
						}
						if (num6 > j)
						{
							text = s.Substring(j, num6 - j);
							if (!int.TryParse(text, out result))
							{
								flag = false;
								if (!blnTryParse)
								{
									break;
								}
							}
							day = result;
						}
						num3 = num6;
					}
					i += num5 - 1;
				}
				else if (format[i] == 'M')
				{
					if (num5 >= 4)
					{
						int num7 = num3;
						int k;
						for (k = num3; k < length && (s[k] < '0' || s[k] > '9'); k++)
						{
						}
						if (k > num7)
						{
							text = s.Substring(num7, k - num7);
							result = clsCommonFunctions.MonthOfYear(CalendarTypes.Persian, text);
							if (result < 1 || result > 12)
							{
								flag = false;
								if (!blnTryParse)
								{
									break;
								}
							}
							month = result;
						}
						num3 = k;
					}
					else
					{
						int l = num3;
						int num8 = num3;
						for (; l < length && (s[l] < '0' || s[l] > '9'); l++)
						{
						}
						for (num8 = l; num8 < length && s[num8] >= '0' && s[num8] <= '9'; num8++)
						{
						}
						if (num8 > l)
						{
							text = s.Substring(l, num8 - l);
							if (!int.TryParse(text, out result))
							{
								flag = false;
								if (!blnTryParse)
								{
									break;
								}
							}
							month = result;
						}
						num3 = num8;
					}
					i += num5 - 1;
				}
				else if (format[i] == 'y')
				{
					int m = num3;
					int num9 = num3;
					for (; m < length && (s[m] < '0' || s[m] > '9'); m++)
					{
					}
					for (num9 = m; num9 < length && s[num9] >= '0' && s[num9] <= '9'; num9++)
					{
					}
					if (num9 > m)
					{
						text = s.Substring(m, num9 - m);
						if (!int.TryParse(text, out result))
						{
							flag = false;
							if (!blnTryParse)
							{
								break;
							}
						}
						year = result;
					}
					num3 = num9;
					i += num5 - 1;
				}
				else if (format[i] == 'h')
				{
					int n = num3;
					int num10 = num3;
					for (; n < length && (s[n] < '0' || s[n] > '9'); n++)
					{
					}
					for (num10 = n; num10 < length && s[num10] >= '0' && s[num10] <= '9'; num10++)
					{
					}
					if (num10 > n)
					{
						text = s.Substring(n, num10 - n);
						if (!int.TryParse(text, out result))
						{
							flag = false;
							if (!blnTryParse)
							{
								break;
							}
						}
						num = result;
					}
					num3 = num10;
					i += num5 - 1;
				}
				else if (format[i] == 'm')
				{
					int num11 = num3;
					int num12 = num3;
					for (; num11 < length && (s[num11] < '0' || s[num11] > '9'); num11++)
					{
					}
					for (num12 = num11; num12 < length && s[num12] >= '0' && s[num12] <= '9'; num12++)
					{
					}
					if (num12 > num11)
					{
						text = s.Substring(num11, num12 - num11);
						if (!int.TryParse(text, out result))
						{
							flag = false;
							if (!blnTryParse)
							{
								break;
							}
						}
						minute = result;
					}
					num3 = num12;
					i += num5 - 1;
				}
				else if (format[i] == 's')
				{
					int num13 = num3;
					int num14 = num3;
					for (; num13 < length && (s[num13] < '0' || s[num13] > '9'); num13++)
					{
					}
					for (num14 = num13; num14 < length && s[num14] >= '0' && s[num14] <= '9'; num14++)
					{
					}
					if (num14 > num13)
					{
						text = s.Substring(num13, num14 - num13);
						if (!int.TryParse(text, out result))
						{
							flag = false;
							if (!blnTryParse)
							{
								break;
							}
						}
						second = result;
					}
					num3 = num14;
					i += num5 - 1;
				}
				else if (format[i] == 'f' || format[i] == 'F')
				{
					int num15 = num3;
					int num16 = num3;
					for (; num15 < length && (s[num15] < '0' || s[num15] > '9'); num15++)
					{
					}
					for (num16 = num15; num16 < length && s[num16] >= '0' && s[num16] <= '9'; num16++)
					{
					}
					if (num16 > num15)
					{
						text = s.Substring(num15, num16 - num15);
						if (!int.TryParse(text, out result))
						{
							flag = false;
							if (!blnTryParse)
							{
								break;
							}
						}
						millisecond = result;
					}
					num3 = num16;
					i += num5 - 1;
				}
				else if (format[i] == 't')
				{
					int num17 = num3;
					int num18 = num3;
					for (; s[num17] == ' '; num17++)
					{
					}
					switch (s[num17])
					{
					case 'ب':
						num2 = 12;
						break;
					case 'ق':
						num2 = -12;
						break;
					}
					num18 = num17;
					if (num5 == 1)
					{
						num18++;
					}
					else
					{
						num18++;
						if (num18 < length && s[num18] == ' ')
						{
							num18++;
						}
						else
						{
							flag = false;
							if (!blnTryParse)
							{
								break;
							}
						}
						if (num18 < length)
						{
							num18++;
						}
						else
						{
							flag = false;
							if (!blnTryParse)
							{
								break;
							}
						}
					}
					num3 = num18;
					i += num5 - 1;
				}
				else if (format[i] == '\'' || format[i] == '"')
				{
					char c = format[i];
					int num19 = num3;
					for (i++; s[num19] == format[i]; i++)
					{
						num19++;
					}
					if (format[i] != c)
					{
						flag = false;
						if (!blnTryParse)
						{
							break;
						}
					}
					i++;
					num3 = num19;
				}
				else if (format[i] == '\\')
				{
					i += 2;
					num3++;
				}
			}
			else
			{
				num3++;
			}
		}
		num += num2;
		if (num > 23)
		{
			num = 0;
		}
		if (num < 0)
		{
			num -= num2;
		}
		PersianDateTime persianDateTime = (flag ? new PersianDateTime(year, month, day, num, minute, second, millisecond) : ((!blnTryParse) ? new PersianDateTime(year, month, day, num, minute, second, millisecond) : default(PersianDateTime)));
		dateParsed = persianDateTime;
		return flag;
	}

	public static PersianDateTime Parse(string s, string format)
	{
		PersianDateTime dateParsed = default(PersianDateTime);
		if (!DoParse2(s, format, blnTryParse: false, out dateParsed))
		{
			throw new Exception("Format is not valid");
		}
		return dateParsed;
	}

	public static bool TryParse(string s, string format, out PersianDateTime result)
	{
		PersianDateTime dateParsed = default(PersianDateTime);
		bool result2 = DoParse2(s, format, blnTryParse: true, out dateParsed);
		result = dateParsed;
		return result2;
	}

	public static DateTime ToDateTime(PersianDateTime date)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		return persianCalendar.ToDateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
	}

	public static DateTime ToDateTime(int year, int month, int day)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		return persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
	}

	public static DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		return persianCalendar.ToDateTime(year, month, day, hour, minute, second, millisecond);
	}
}
