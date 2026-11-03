using System;
using System.Drawing;
using System.Windows.Forms;

namespace BehComponents;

internal class clsCommonFunctions
{
	internal static string MonthOfYear(CalendarTypes type, int month)
	{
		string result = "";
		switch (type)
		{
		case CalendarTypes.Persian:
			switch (month)
			{
			case 1:
				result = "فروردین";
				break;
			case 2:
				result = "اردیبهشت";
				break;
			case 3:
				result = "خرداد";
				break;
			case 4:
				result = "تیر";
				break;
			case 5:
				result = "مرداد";
				break;
			case 6:
				result = "شهریور";
				break;
			case 7:
				result = "مهر";
				break;
			case 8:
				result = "آبان";
				break;
			case 9:
				result = "آذر";
				break;
			case 10:
				result = "دی";
				break;
			case 11:
				result = "بهمن";
				break;
			case 12:
				result = "اسفند";
				break;
			}
			break;
		case CalendarTypes.English:
			switch (month)
			{
			case 1:
				result = "January";
				break;
			case 2:
				result = "February";
				break;
			case 3:
				result = "March";
				break;
			case 4:
				result = "April";
				break;
			case 5:
				result = "May";
				break;
			case 6:
				result = "June";
				break;
			case 7:
				result = "July";
				break;
			case 8:
				result = "August";
				break;
			case 9:
				result = "September";
				break;
			case 10:
				result = "October";
				break;
			case 11:
				result = "November";
				break;
			case 12:
				result = "December";
				break;
			}
			break;
		}
		return result;
	}

	internal static int MonthOfYear(CalendarTypes type, string monthName)
	{
		int num = 0;
		if (type == CalendarTypes.Persian)
		{
			return monthName.Trim() switch
			{
				"فروردین" => 1, 
				"اردیبهشت" => 2, 
				"خرداد" => 3, 
				"تیر" => 4, 
				"مرداد" => 5, 
				"شهریور" => 6, 
				"مهر" => 7, 
				"آبان" => 8, 
				"آذر" => 9, 
				"دی" => 10, 
				"بهمن" => 11, 
				"اسفند" => 12, 
				_ => throw new Exception("Month name is not valid"), 
			};
		}
		return monthName.Trim().ToLower() switch
		{
			"january" => 1, 
			"february" => 2, 
			"march" => 3, 
			"april" => 4, 
			"may" => 5, 
			"june" => 6, 
			"july" => 7, 
			"august" => 8, 
			"september" => 9, 
			"october" => 10, 
			"november" => 11, 
			"december" => 12, 
			_ => throw new Exception("Month name is not valid"), 
		};
	}

	internal static string NameOfDayOfWeek(DayOfWeek dayOfWeek)
	{
		return dayOfWeek switch
		{
			DayOfWeek.Saturday => "شنبه", 
			DayOfWeek.Sunday => "یک شنبه", 
			DayOfWeek.Monday => "دو شنبه", 
			DayOfWeek.Tuesday => "سه شنبه", 
			DayOfWeek.Wednesday => "چهار شنبه", 
			DayOfWeek.Thursday => "پنج شنبه", 
			DayOfWeek.Friday => "جمعه", 
			_ => "", 
		};
	}

	internal static int CountChar(string custom, int startIndex, bool isCaseSensitive)
	{
		if (custom == "")
		{
			return 0;
		}
		if (!isCaseSensitive)
		{
			custom = custom.ToLower();
		}
		int num = 1;
		int num2 = startIndex;
		char c = custom[num2++];
		char c2 = ((custom.Length - 1 >= num2) ? custom[num2++] : '\0');
		while (c == c2)
		{
			num++;
			c2 = ((custom.Length - 1 >= num2) ? custom[num2++] : '\0');
		}
		return num;
	}

	internal static int GetStartIndexDayOfWeek(CalendarTypes type, DateTime date)
	{
		DayOfWeek dayOfWeek = DayOfWeek.Saturday;
		switch (type)
		{
		case CalendarTypes.Persian:
		{
			PersianDateTime persianDateTime = new PersianDateTime(date);
			dayOfWeek = new PersianDateTime(persianDateTime.Year, persianDateTime.Month, 1).DayOfWeek;
			break;
		}
		case CalendarTypes.English:
			dayOfWeek = new DateTime(date.Year, date.Month, 1).DayOfWeek;
			break;
		default:
			throw new Exception();
		}
		return dayOfWeek switch
		{
			DayOfWeek.Saturday => 0, 
			DayOfWeek.Sunday => 1, 
			DayOfWeek.Monday => 2, 
			DayOfWeek.Tuesday => 3, 
			DayOfWeek.Wednesday => 4, 
			DayOfWeek.Thursday => 5, 
			DayOfWeek.Friday => 6, 
			_ => 0, 
		};
	}

	internal static string[] NameOfWeekDays(CalendarTypes type)
	{
		string[] array = new string[7];
		if (type == CalendarTypes.Persian)
		{
			array[0] = "شنبه";
			array[1] = "یکشنبه";
			array[2] = "دوشنبه";
			array[3] = "سه شنبه";
			array[4] = "چهارشنبه";
			array[5] = "پنج شنبه";
			array[6] = "جمعه";
		}
		else
		{
			array[0] = "Saturday";
			array[1] = "Sunday";
			array[2] = "Monday";
			array[3] = "Tuesday";
			array[4] = "Wednesday";
			array[5] = "Thursday";
			array[6] = "Friday";
		}
		return array;
	}

	internal static string[] NameOfWeekDays(CalendarTypes type, float width, Font font)
	{
		string[] array = new string[7];
		if (type == CalendarTypes.Persian)
		{
			array[0] = "شنبه";
			array[1] = "یکشنبه";
			array[2] = "دوشنبه";
			array[3] = "سه شنبه";
			array[4] = "چهارشنبه";
			array[5] = "پنج شنبه";
			array[6] = "جمعه";
			int num = MaxSizeOfTexts(array, font);
			if ((float)num > width)
			{
				array[0] = "ش";
				array[1] = "ی";
				array[2] = "د";
				array[3] = "س";
				array[4] = "چ";
				array[5] = "پ";
				array[6] = "ج";
			}
		}
		else
		{
			array[0] = "Saturday";
			array[1] = "Sunday";
			array[2] = "Monday";
			array[3] = "Tuesday";
			array[4] = "Wednesday";
			array[5] = "Thursday";
			array[6] = "Friday";
			int num2 = MaxSizeOfTexts(array, font);
			if ((float)num2 > width)
			{
				array[0] = "Sa";
				array[1] = "Su";
				array[2] = "Mo";
				array[3] = "Tu";
				array[4] = "We";
				array[5] = "Th";
				array[6] = "Fr";
			}
		}
		return array;
	}

	internal static int MaxSizeOfTexts(string[] sWeeks, Font font)
	{
		int num = 0;
		foreach (string text in sWeeks)
		{
			Size size = TextRenderer.MeasureText(text, font);
			if (size.Width > num)
			{
				num = size.Width;
			}
		}
		return num;
	}
}
