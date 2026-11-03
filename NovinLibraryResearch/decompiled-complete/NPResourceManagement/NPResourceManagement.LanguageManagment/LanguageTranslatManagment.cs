using System;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Entities;

namespace NPResourceManagement.LanguageManagment;

public class LanguageTranslatManagment
{
	public static void ChangeLanguage(ENSource source)
	{
		try
		{
			LanguageTypes language = Utilities.GetLanguage();
			if (source == ENSource.TimeAttendance)
			{
				if (language == LanguageTypes.Arbic)
				{
					LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguageArabicTimeAttendance, translateUi: true, ENSource.TimeAttendance);
				}
				else
				{
					LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguageFarsiTimeAttendance, translateUi: true, ENSource.TimeAttendance);
				}
			}
			if (source == ENSource.Accounting)
			{
				switch (language)
				{
				case LanguageTypes.Farsi:
					LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguagePersianAccounting, translateUi: true, ENSource.Accounting);
					break;
				case LanguageTypes.English:
					LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguageEnglishNewAccounting, translateUi: true, ENSource.Accounting);
					break;
				case LanguageTypes.Russian:
					LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguageRussianAccounting, translateUi: true, ENSource.Accounting);
					break;
				case LanguageTypes.Georgian:
					LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguageGeorgianAccounting, translateUi: true, ENSource.Accounting);
					break;
				default:
					LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguagePersianAccounting, translateUi: true, ENSource.Accounting);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeLanguage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\NPResourceManagement\\LanguageManagment\\LanguageTranslatManagment.cs", 51);
		}
	}
}
