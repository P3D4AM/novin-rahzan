using System;

namespace PacketParser;

public class DateManagment
{
	private static DateTime _dateM = DateTime.Now;

	private static bool _systemDate = true;

	public static DateTime DateM
	{
		get
		{
			if (_systemDate)
			{
				return DateTime.Now;
			}
			_dateM = DateTime.Parse(_dateM.Year + "/" + _dateM.Month + "/" + _dateM.Day + " " + DateTime.Now.TimeOfDay.Hours + ":" + DateTime.Now.TimeOfDay.Minutes + ":" + DateTime.Now.TimeOfDay.Seconds);
			return _dateM;
		}
		set
		{
			_dateM = value;
		}
	}

	public static string DateSh => DateM.MiladiToCalture();

	public static int Year => DateM.Year;

	public static int Month => DateM.Month;

	public static int Day => DateM.Day;

	public static string Time => $"{Hour}:{Minute}:{Second}";

	public static string ShortTime => $"{Hour}:{Minute}";

	public static int Hour => DateM.Hour;

	public static int Minute => DateM.Minute;

	public static int Second => DateM.Second;

	public static bool SystemDate
	{
		get
		{
			return _systemDate;
		}
		set
		{
			_systemDate = value;
		}
	}
}
