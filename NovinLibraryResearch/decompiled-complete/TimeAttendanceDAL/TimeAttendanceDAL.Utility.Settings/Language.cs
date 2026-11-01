using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using PacketParser.Entities;

namespace TimeAttendanceDAL.Utility.Settings;

public static class Language
{
	private static bool _changedLanguage;

	public static void ChangeLanguage(string language)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		try
		{
			if (_changedLanguage)
			{
				return;
			}
			_changedLanguage = true;
			foreach (InputLanguage item in (ReadOnlyCollectionBase)(object)InputLanguage.InstalledInputLanguages)
			{
				InputLanguage val = item;
				if (!(val.Culture.Name != language))
				{
					Application.CurrentInputLanguage = val;
					break;
				}
			}
			CultureInfo cultureInfo = new CultureInfo(language, useUserOverride: true);
			cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();
			cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
			cultureInfo.DateTimeFormat.LongDatePattern = "yyyy/MM/dd";
			cultureInfo.DateTimeFormat.FullDateTimePattern = "yyyy/MM/dd HH:mm:ss";
			cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm";
			cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
			cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
			cultureInfo.NumberFormat.NumberGroupSeparator = ",";
			cultureInfo.NumberFormat.NegativeSign = "-";
			cultureInfo.NumberFormat.NativeDigits = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChangeLanguage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\Language.cs", 43);
		}
	}
}
