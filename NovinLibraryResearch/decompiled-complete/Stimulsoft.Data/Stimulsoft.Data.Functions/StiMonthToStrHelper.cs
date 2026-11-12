using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Data.Functions;

public sealed class StiMonthToStrHelper
{
	private static List<string[]> months;

	private static List<bool> defaultUpperCaseList;

	private static Hashtable cultureIndexes;

	public static string MonthName(DateTime dateTime)
	{
		return dateTime.ToString("MMMM");
	}

	public static string MonthName(DateTime dateTime, bool localized)
	{
		if (!localized)
		{
			return MonthName(dateTime);
		}
		return dateTime.Month switch
		{
			1 => Loc.Get("A_WebViewer", "MonthJanuary"), 
			2 => Loc.Get("A_WebViewer", "MonthFebruary"), 
			3 => Loc.Get("A_WebViewer", "MonthMarch"), 
			4 => Loc.Get("A_WebViewer", "MonthApril"), 
			5 => Loc.Get("A_WebViewer", "MonthMay"), 
			6 => Loc.Get("A_WebViewer", "MonthJune"), 
			7 => Loc.Get("A_WebViewer", "MonthJuly"), 
			8 => Loc.Get("A_WebViewer", "MonthAugust"), 
			9 => Loc.Get("A_WebViewer", "MonthSeptember"), 
			10 => Loc.Get("A_WebViewer", "MonthOctober"), 
			11 => Loc.Get("A_WebViewer", "MonthNovember"), 
			12 => Loc.Get("A_WebViewer", "MonthDecember"), 
			_ => MonthName(dateTime), 
		};
	}

	public static string MonthName(DateTime dateTime, string culture)
	{
		return MonthName(dateTime.Month, culture);
	}

	public static string MonthName(int month, string culture)
	{
		string text = string.Empty;
		try
		{
			return new CultureInfo(culture).DateTimeFormat.GetMonthName(month);
		}
		catch
		{
			string key = culture.ToLowerInvariant();
			if (!cultureIndexes.ContainsKey(key))
			{
				return Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(month);
			}
			int index = (int)cultureIndexes[key];
			if (month < months[index].Length)
			{
				text = months[index][month - 1];
			}
			if (defaultUpperCaseList[index])
			{
				text = text.Substring(0, 1).ToUpperInvariant() + text.Substring(1);
			}
		}
		return text;
	}

	public static string MonthName(DateTime dateTime, string culture, bool upperCase)
	{
		string text = MonthName(dateTime, culture).ToLowerInvariant();
		if (!upperCase)
		{
			return text;
		}
		return text.Substring(0, 1).ToUpperInvariant() + text.Substring(1);
	}

	public static void AddCulture(string[] monthNames, string[] cultureNames, bool defaultUpperCase)
	{
		int count = months.Count;
		months.Add(monthNames);
		defaultUpperCaseList.Add(defaultUpperCase);
		foreach (string text in cultureNames)
		{
			cultureIndexes[text.ToLowerInvariant()] = count;
		}
	}

	public static StiMonth? Month(int value)
	{
		if (value >= 1 || value <= 12)
		{
			return (StiMonth)value;
		}
		return null;
	}

	public static StiMonth? Month(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		str = str.ToLowerInvariant().Trim();
		int result = 0;
		if (int.TryParse(str, out result))
		{
			return Month(result);
		}
		foreach (string[] month in months)
		{
			for (int i = 0; i < month.Length; i++)
			{
				string text = month[i].ToLowerInvariant();
				if (text == str)
				{
					return (StiMonth)(i + 1);
				}
				if (text.Length >= 3 && text.Substring(0, 3) == str)
				{
					return (StiMonth)(i + 1);
				}
			}
		}
		return null;
	}

	static StiMonthToStrHelper()
	{
		months = new List<string[]>();
		defaultUpperCaseList = new List<bool>();
		cultureIndexes = new Hashtable();
		AddCulture(new string[12]
		{
			"January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
			"November", "December"
		}, new string[1] { "en" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь",
			"Ноябрь", "Декабрь"
		}, new string[1] { "ru" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre",
			"noviembre", "diciembre"
		}, new string[1] { "es" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"gennaio", "febbraio", "marzo", "aprile", "maggio", "giugno", "luglio", "agosto", "settembre", "ottobre",
			"novembre", "dicembre"
		}, new string[1] { "it" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"ianuarie", "februarie", "martie", "aprilie", "mai", "iunie", "iulie", "august", "septembrie", "octombrie",
			"noiembrie", "decembrie"
		}, new string[1] { "ro" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro",
			"Novembro", "Dezembro"
		}, new string[1] { "pt" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"Jannar", "Frar", "Marzu", "April", "Mejju", "Ġunju", "Lulju", "Awissu", "Settembru", "Ottubru",
			"Novembru", "Diċembru"
		}, new string[1] { "mt" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"január", "február", "március", "április", "május", "június", "július", "augusztus", "szeptember", "október",
			"november", "december"
		}, new string[1] { "hu" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"január", "február", "marec", "apríl", "máj", "jún", "júl", "august", "september", "október",
			"november", "december"
		}, new string[1] { "sk" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"janúar", "febrúar", "mars", "apríl", "maí", "júní", "júlí", "ágúst", "september", "október",
			"nóvember", "desember"
		}, new string[1] { "is" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"januar", "februar", "mars", "apríl", "mai", "juni", "juli", "august", "september", "oktober",
			"november", "desember"
		}, new string[1] { "fo" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober",
			"November", "Dezember"
		}, new string[1] { "de" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober",
			"november", "december"
		}, new string[1] { "nl" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober",
			"Nopember", "Desember"
		}, new string[1] { "id" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"janvāris", "februāris", "marts", "aprīlis", "maijs", "jūnijs", "jūlijs", "augusts", "septembris", "oktobris",
			"novembris", "decembris"
		}, new string[1] { "lv" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"janvier", "février", "mars", "avril", "mai", "juin", "juillet", "août", "septembre", "octobre",
			"novembre", "décembre"
		}, new string[1] { "fr" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"leden", "únor", "březen", "duben", "květen", "červen", "červenec", "srpen", "září", "říjen",
			"listopad", "prosinec"
		}, new string[1] { "cs" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"Mutarama", "Gashyantare", "Werurwe", "Mata", "Gicurasi", "Kamena", "Nyakanga", "Kanama", "Nzeli", "Ukwakira",
			"Ugushyingo", "Ukuboza"
		}, new string[1] { "rw" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim",
			"Kasım", "Aralık"
		}, new string[1] { "tr" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"ođđajagemánnu", "guovvamánnu", "njukčamánnu", "cuoŋománnu", "miessemánnu", "geassemánnu", "suoidnemánnu", "borgemánnu", "čakčamánnu", "golggotmánnu",
			"skábmamánnu", "juovlamánnu"
		}, new string[1] { "se" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"sausis", "vasaris", "kovas", "balandis", "gegužė", "birželis", "liepa", "rugpjūtis", "rugsėjis", "spalis",
			"lapkritis", "gruodis"
		}, new string[1] { "lt" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"siječanj", "veljača", "ožujak", "travanj", "svibanj", "lipanj", "srpanj", "kolovoz", "rujan", "listopad",
			"studeni", "prosinac"
		}, new string[1] { "hr" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"styczeń", "luty", "marzec", "kwiecień", "maj", "czerwiec", "lipiec", "sierpień", "wrzesień", "październik",
			"listopad", "grudzień"
		}, new string[1] { "pl" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"tammikuu", "helmikuu", "maaliskuu", "huhtikuu", "toukokuu", "kesäkuu", "heinäkuu", "elokuu", "syyskuu", "lokakuu",
			"marraskuu", "joulukuu"
		}, new string[1] { "fi" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"јануари", "февруари", "март", "април", "мај", "јуни", "јули", "август", "септември", "октомври",
			"ноември", "декември"
		}, new string[1] { "mk" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"Януари", "Февруари", "Март", "Април", "Май", "Юни", "Юли", "Август", "Септември", "Октомври",
			"Ноември", "Декември"
		}, new string[1] { "bg" }, defaultUpperCase: false);
		AddCulture(new string[12]
		{
			"มกราคม", "ก\u0e38มภาพ\u0e31นธ\u0e4c", "ม\u0e35นาคม", "เมษายน", "พฤษภาคม", "ม\u0e34ถ\u0e38นายน", "กรกฎาคม", "ส\u0e34งหาคม", "ก\u0e31นยายน", "ต\u0e38ลาคม",
			"พฤศจ\u0e34กายน", "ธ\u0e31นวาคม"
		}, new string[1] { "th" }, defaultUpperCase: false);
	}
}
