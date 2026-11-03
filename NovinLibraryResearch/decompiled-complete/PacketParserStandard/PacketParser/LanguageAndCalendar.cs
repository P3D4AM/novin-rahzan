using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser;

public class LanguageAndCalendar
{
	private bool _translateUi;

	public static LanguageAndCalendar Instance = new LanguageAndCalendar(LanguageTypes.Farsi, changeCalture: false, translateUi: false);

	public LanguageTypes SelectedLangauge { get; private set; }

	public ICalendar CaltureCalender { get; set; }

	public ENSource Source { get; private set; } = ENSource.TimeAttendance;


	private void ChangeSchemaLanguage()
	{
		using (new JaegerMonitor("ChangeSchemaLanguage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\LanguageAndCalendar.cs"))
		{
			try
			{
				CultureInfo cultureInfo = new CultureInfo("en-US", useUserOverride: true);
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
				WebErrorLog.Instance.StartLog(ex, "", "ChangeSchemaLanguage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\LanguageAndCalendar.cs", 43);
			}
		}
	}

	public ReturnedSaveFuncInfo ChangeLanguage(LanguageTypes languages, string path, bool translateUi, ENSource source)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_translateUi = translateUi;
			SelectedLangauge = languages;
			Source = source;
			ChangeSchemaLanguage();
			switch (SelectedLangauge)
			{
			case LanguageTypes.Farsi:
				CaltureCalender = new FarsiCalender(path);
				break;
			case LanguageTypes.Arbic:
				CaltureCalender = new ArbicCalendar(path);
				break;
			case LanguageTypes.English:
			case LanguageTypes.Spanish:
			case LanguageTypes.Turkish:
			case LanguageTypes.EnterNational:
				CaltureCalender = new EnglishCalendar(path);
				break;
			default:
				CaltureCalender = new FarsiCalender(path);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeLanguage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\LanguageAndCalendar.cs", 75);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "ChangeLanguage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\LanguageAndCalendar.cs", 76);
		}
		return returnedSaveFuncInfo;
	}

	public string GetValue(string key)
	{
		try
		{
			if (string.IsNullOrEmpty(key))
			{
				return key;
			}
			if (CaltureCalender == null)
			{
				return key;
			}
			string text = key;
			List<string> list = Language.ChecKParamters(key);
			if (list != null && list.Any())
			{
				foreach (string item in list)
				{
					if (!string.IsNullOrEmpty(item))
					{
						string value = CaltureCalender.GetValue(item.Replace("ـ", "").Trim());
						if (!(value == item.Replace("ـ", "")))
						{
							key = key.Replace("[" + item + "]", value);
						}
					}
				}
				if (text == key)
				{
					key = key.Replace("[", "").Replace("]", "");
				}
				return key;
			}
			if (text == key)
			{
				key = key.Replace("[", "").Replace("]", "");
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\LanguageAndCalendar.cs", 108);
		}
		return key;
	}

	public bool TranslateUi()
	{
		return _translateUi;
	}

	private LanguageAndCalendar(LanguageTypes languges, bool changeCalture, bool translateUi)
	{
		_translateUi = translateUi;
		SelectedLangauge = languges;
	}
}
