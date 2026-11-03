using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Win.Parameters;

public static class StiDateTimeFormatHelper
{
	public static bool IsUSADateTimeFormat => CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLowerInvariant().StartsWith("mm");

	public static void SetDateTimePickerFormat(DateTimePicker picker, StiDateTimeType type, CultureInfo culture = null)
	{
		picker.CustomFormat = GetDateTimeFormat(type, culture);
	}

	public static string GetDateTimeFormat(StiDateTimeType type, CultureInfo culture = null)
	{
		switch (type)
		{
		case StiDateTimeType.DateAndTime:
			if (culture != null)
			{
				return culture.DateTimeFormat.ShortDatePattern + ", " + culture.DateTimeFormat.LongTimePattern;
			}
			if (IsUSADateTimeFormat)
			{
				return "MM.dd.yyyy; hh:mm:ss";
			}
			return "dd.MM.yyyy, HH:mm:ss";
		case StiDateTimeType.Date:
			if (culture != null)
			{
				return culture.DateTimeFormat.ShortDatePattern;
			}
			if (IsUSADateTimeFormat)
			{
				return "MM.dd.yyyy";
			}
			return "dd.MM.yyyy";
		case StiDateTimeType.Time:
			if (culture != null)
			{
				return culture.DateTimeFormat.LongTimePattern;
			}
			if (IsUSADateTimeFormat)
			{
				return "HH:mm:ss";
			}
			return "hh:mm:ss";
		default:
			if (IsUSADateTimeFormat)
			{
				return "HH:mm:ss";
			}
			return "hh:mm:ss";
		}
	}
}
