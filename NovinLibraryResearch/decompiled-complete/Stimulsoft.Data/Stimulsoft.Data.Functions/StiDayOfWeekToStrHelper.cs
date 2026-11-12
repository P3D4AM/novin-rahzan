using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Data.Functions;

public sealed class StiDayOfWeekToStrHelper
{
	private static List<string[]> days;

	private static List<bool> defaultUpperCaseList;

	private static Hashtable cultureIndexes;

	public static string DayOfWeek(DateTime date)
	{
		return DateTimeFormatInfo.CurrentInfo.GetDayName(date.DayOfWeek);
	}

	public static string DayOfWeek(DateTime date, bool localized)
	{
		if (!localized)
		{
			return DayOfWeek(date);
		}
		return date.DayOfWeek switch
		{
			System.DayOfWeek.Sunday => Loc.Get("A_WebViewer", "DaySunday"), 
			System.DayOfWeek.Monday => Loc.Get("A_WebViewer", "DayMonday"), 
			System.DayOfWeek.Tuesday => Loc.Get("A_WebViewer", "DayTuesday"), 
			System.DayOfWeek.Wednesday => Loc.Get("A_WebViewer", "DayWednesday"), 
			System.DayOfWeek.Thursday => Loc.Get("A_WebViewer", "DayThursday"), 
			System.DayOfWeek.Friday => Loc.Get("A_WebViewer", "DayFriday"), 
			System.DayOfWeek.Saturday => Loc.Get("A_WebViewer", "DaySaturday"), 
			_ => DayOfWeek(date), 
		};
	}

	public static string DayOfWeek(DateTime dateTime, string culture)
	{
		return DayOfWeek((int)dateTime.DayOfWeek, culture);
	}

	public static string DayOfWeek(int dayOfWeek, string culture)
	{
		string text = string.Empty;
		try
		{
			return new CultureInfo(culture).DateTimeFormat.GetDayName((DayOfWeek)dayOfWeek);
		}
		catch
		{
			string key = culture.ToLowerInvariant();
			if (!cultureIndexes.ContainsKey(key))
			{
				return Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)dayOfWeek);
			}
			int index = (int)cultureIndexes[key];
			if (dayOfWeek < days[index].Length)
			{
				text = days[index][dayOfWeek];
			}
			if (defaultUpperCaseList[index])
			{
				text = text.Substring(0, 1).ToUpperInvariant() + text.Substring(1);
			}
		}
		return text;
	}

	public static string DayOfWeek(DateTime dateTime, string culture, bool upperCase)
	{
		string text = DayOfWeek(dateTime, culture).ToLowerInvariant();
		if (!upperCase)
		{
			return text;
		}
		return text.Substring(0, 1).ToUpperInvariant() + text.Substring(1);
	}

	public static void AddCulture(string[] monthNames, string[] cultureNames, bool defaultUpperCase)
	{
		int count = days.Count;
		days.Add(monthNames);
		defaultUpperCaseList.Add(defaultUpperCase);
		foreach (string text in cultureNames)
		{
			cultureIndexes[text.ToLowerInvariant()] = count;
		}
	}

	public static DayOfWeek? DayOfWeek(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		str = str.ToLowerInvariant().Trim();
		foreach (string[] day in days)
		{
			for (int i = 0; i < day.Length; i++)
			{
				string text = day[i].ToLowerInvariant();
				if (text == str)
				{
					return (DayOfWeek)i;
				}
			}
		}
		return null;
	}

	static StiDayOfWeekToStrHelper()
	{
		days = new List<string[]>();
		defaultUpperCaseList = new List<bool>();
		cultureIndexes = new Hashtable();
		AddCulture(new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" }, new string[1] { "en" }, defaultUpperCase: false);
		AddCulture(new string[7] { "dimanche", "lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi" }, new string[1] { "fr" }, defaultUpperCase: false);
		AddCulture(new string[7] { "domenica", "lunedì", "martedì", "mercoledì", "giovedì", "venerdì", "sabato" }, new string[1] { "it" }, defaultUpperCase: false);
		AddCulture(new string[7] { "domingo", "lunes", "martes", "miércoles", "jueves", "viernes", "sábado" }, new string[1] { "es" }, defaultUpperCase: false);
		AddCulture(new string[7] { "domingo", "segunda-feira", "terça-feira", "quarta-feira", "quinta-feira", "sexta-feira", "sábado" }, new string[1] { "pt" }, defaultUpperCase: false);
		AddCulture(new string[7] { "duminică", "luni", "marţi", "miercuri", "joi", "vineri", "sâmbătă" }, new string[1] { "ro" }, defaultUpperCase: false);
		AddCulture(new string[7] { "Il-Ħadd", "It-Tnejn", "It-Tlieta", "L-Erbgħa", "Il-Ħamis", "Il-Ġimgħa", "Is-Sibt" }, new string[1] { "mt" }, defaultUpperCase: false);
		AddCulture(new string[7] { "Ku wa mbere", "Ku wa kabiri", "Ku wa gatatu", "Ku wa kane", "Ku wa gatanu", "Ku wa gatandatu", "Ku cyumweru" }, new string[1] { "rw" }, defaultUpperCase: false);
		AddCulture(new string[7] { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" }, new string[1] { "id" }, defaultUpperCase: false);
		AddCulture(new string[7] { "nedeľa", "pondelok", "utorok", "streda", "štvrtok", "piatok", "sobota" }, new string[1] { "sk" }, defaultUpperCase: false);
		AddCulture(new string[7] { "nedjelja", "ponedjeljak", "utorak", "srijeda", "četvrtak", "petak", "subota" }, new string[1] { "hr" }, defaultUpperCase: false);
		AddCulture(new string[7] { "niedziela", "poniedziałek", "wtorek", "środa", "czwartek", "piątek", "sobota" }, new string[1] { "pl" }, defaultUpperCase: false);
		AddCulture(new string[7] { "Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi" }, new string[1] { "tr" }, defaultUpperCase: false);
		AddCulture(new string[7] { "sekmadienis", "pirmadienis", "antradienis", "trečiadienis", "ketvirtadienis", "penktadienis", "šeštadienis" }, new string[1] { "lt" }, defaultUpperCase: false);
		AddCulture(new string[7] { "Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag" }, new string[1] { "de" }, defaultUpperCase: false);
		AddCulture(new string[7] { "sotnabeaivi", "mánnodat", "disdat", "gaskavahkku", "duorastat", "bearjadat", "lávvardat" }, new string[1] { "se" }, defaultUpperCase: false);
		AddCulture(new string[7] { "sunnudagur", "mánadagur", "týsdagur", "mikudagur", "hósdagur", "fríggjadagur", "leygardagur" }, new string[1] { "fo" }, defaultUpperCase: false);
		AddCulture(new string[7] { "sunnudagur", "mánudagur", "þriðjudagur", "miðvikudagur", "fimmtudagur", "föstudagur", "laugardagur" }, new string[1] { "is" }, defaultUpperCase: false);
		AddCulture(new string[7] { "sunnuntai", "maanantai", "tiistai", "keskiviikko", "torstai", "perjantai", "lauantai" }, new string[1] { "fi" }, defaultUpperCase: false);
		AddCulture(new string[7] { "svētdiena", "pirmdiena", "otrdiena", "trešdiena", "ceturtdiena", "piektdiena", "sestdiena" }, new string[1] { "lv" }, defaultUpperCase: false);
		AddCulture(new string[7] { "vasárnap", "hétfő", "kedd", "szerda", "csütörtök", "péntek", "szombat" }, new string[1] { "hu" }, defaultUpperCase: false);
		AddCulture(new string[7] { "zondag", "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag" }, new string[1] { "nl" }, defaultUpperCase: false);
		AddCulture(new string[7] { "воскресенье", "понедельник", "вторник", "среда", "четверг", "пятница", "суббота" }, new string[1] { "ru" }, defaultUpperCase: false);
		AddCulture(new string[7] { "недела", "понеделник", "вторник", "среда", "четврток", "петок", "сабота" }, new string[1] { "mk" }, defaultUpperCase: false);
		AddCulture(new string[7] { "неделя", "понеделник", "вторник", "сряда", "четвъртък", "петък", "събота" }, new string[1] { "bg" }, defaultUpperCase: false);
		AddCulture(new string[7] { "Ням", "Даваа", "Мягмар", "Лхагва", "Пүрэв", "Баасан", "Бямба" }, new string[1] { "mn" }, defaultUpperCase: false);
		AddCulture(new string[7] { "อาท\u0e34ตย\u0e4c", "จ\u0e31นทร\u0e4c", "อ\u0e31งคาร", "พ\u0e38ธ", "พฤห\u0e31สบด\u0e35", "ศ\u0e38กร\u0e4c", "เสาร\u0e4c" }, new string[1] { "th" }, defaultUpperCase: false);
	}
}
