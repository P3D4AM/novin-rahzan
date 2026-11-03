using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report;

internal static class StiSystemVariableLocHelper
{
	private static Dictionary<string, string> locs = new Dictionary<string, string>
	{
		{ "ar", "صفحة {0} من {1}" },
		{ "en", "Page {0} of {1}" },
		{ "be", "Старонка {0} з {1}" },
		{ "bg", "Страница {0} от {1}" },
		{ "cz", "{0} z {1}" },
		{ "de", "Seite {0} von {1}" },
		{ "el-gr", "Σελίδα {0} από {1}" },
		{ "es", "Pág.{0} de {1}" },
		{ "ca-es", "P.{0} de {1}" },
		{ "eu-es", "{0}. or. {1}-tik" },
		{ "gl-es", "Páx.{0} de {1}" },
		{ "fa", "صفحه {0} از {1}" },
		{ "fr", "{0} sur {1}" },
		{ "hr", "Strana {0} od {1}" },
		{ "hu", "Oldal {0} a {1}-ból" },
		{ "id", "Halaman {0} dari {1}" },
		{ "it", "{0} di {1}" },
		{ "ka", "{0}, {1}-დან" },
		{ "lt", "{0} iš {1}" },
		{ "lt-sr-sp", "Strana {0} od {1}" },
		{ "nb-no", "Side {0} av {1}" },
		{ "nl", "{0} op {1}" },
		{ "pl", "Strona {0} z {1}" },
		{ "pt", "{0} de {1}" },
		{ "pt-br", "{0} de {1}" },
		{ "ro", "{0} din {1}" },
		{ "ru", "Страница {0} из {1}" },
		{ "sk", "{0} z {1}" },
		{ "sv", "{0} av {1}" },
		{ "tr", "Sayfa {0}/{1}" },
		{ "ua", "Сторінка {0} із {1}" }
	};

	public static string GetPageNofM(StiReport report)
	{
		return string.Format(GetPageNofMIdent(report), report.PageNumber, report.TotalPageCount);
	}

	public static string GetPageNofMThrough(StiReport report)
	{
		return string.Format(GetPageNofMIdent(report), report.PageNumberThrough, report.TotalPageCountThrough);
	}

	private static string GetPageNofMIdent(StiReport report)
	{
		if (report.PageNofMLocalizationString != null)
		{
			return report.PageNofMLocalizationString;
		}
		string text = null;
		if (!string.IsNullOrWhiteSpace(report.Culture))
		{
			text = GetIdent(report.Culture);
		}
		if (text != null)
		{
			return text;
		}
		text = GetIdent(StiLocalization.CultureName);
		if (text != null)
		{
			return text;
		}
		return StiLocalization.Get("Report", "PageNofM");
	}

	private static string GetIdent(string culture)
	{
		culture = culture.ToLowerInvariant();
		if (locs.ContainsKey(culture))
		{
			return locs[culture];
		}
		if (culture.Contains("-"))
		{
			culture = culture.Substring(0, culture.IndexOf("-"));
			return (from p in locs
				where culture.StartsWith(p.Key)
				select p.Value).FirstOrDefault();
		}
		return null;
	}
}
