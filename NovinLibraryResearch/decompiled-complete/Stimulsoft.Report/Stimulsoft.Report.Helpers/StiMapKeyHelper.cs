using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Map;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;

namespace Stimulsoft.Report.Helpers;

public class StiMapKeyHelper : IStiMapKeyHelper
{
	public List<string> GetMapIdents(string key)
	{
		key = Simplify(key);
		List<string> list = new List<string>();
		List<string> mapIdents = GetMapIdents(key, StiIsoElementHelper.Countries);
		if (mapIdents != null)
		{
			list.AddRange(mapIdents);
		}
		string isoAlpha2FromName = GetIsoAlpha2FromName(key, null, null);
		if (!string.IsNullOrWhiteSpace(isoAlpha2FromName))
		{
			list.Add(isoAlpha2FromName);
		}
		string isoAlpha3FromName = GetIsoAlpha3FromName(key, null, null);
		if (!string.IsNullOrWhiteSpace(isoAlpha3FromName))
		{
			list.Add(isoAlpha3FromName);
		}
		if (list.Count == 0)
		{
			mapIdents = GetMapIdents(key, StiIsoElementHelper.UsStates);
			if (mapIdents != null)
			{
				list.AddRange(mapIdents);
			}
		}
		if (list.Count == 0)
		{
			mapIdents = GetMapIdents(key, StiIsoElementHelper.CanadaProvinces);
			if (mapIdents != null)
			{
				list.AddRange(mapIdents);
			}
		}
		if (list.Count == 0)
		{
			mapIdents = GetMapIdents(key, StiIsoElementHelper.BrazilProvinces);
			if (mapIdents != null)
			{
				list.AddRange(mapIdents);
			}
		}
		list.Add(key);
		return list.Distinct().ToList();
	}

	private static List<string> GetMapIdents(string key, Collection<StiIsoCountry> countries)
	{
		foreach (StiIsoCountry country in countries)
		{
			if (Simplify(country.Alpha2) == key || Simplify(country.Alpha3) == key || (country.Names != null && country.Names.Any((string n) => Simplify(n) == key)) || (country.FrNames != null && country.FrNames.Any((string n) => Simplify(n) == key)) || (country.RuNames != null && country.RuNames.Any((string n) => Simplify(n) == key)))
			{
				List<string> list = new List<string>();
				if (country.Names != null)
				{
					list.AddRange(country.Names);
				}
				if (country.RuNames != null)
				{
					list.AddRange(country.RuNames);
				}
				if (country.FrNames != null)
				{
					list.AddRange(country.FrNames);
				}
				return list;
			}
		}
		return null;
	}

	public string GetNameFromIsoAlpha2(string alpha2, string mapId, string lang, IStiReport report = null)
	{
		StiMapSvg svgBlockFromName = StiMapResourceHelper.GetSvgBlockFromName(alpha2, mapId, lang, report as StiReport);
		if (svgBlockFromName != null)
		{
			return svgBlockFromName.EnglishName;
		}
		StiIsoCountry countryFromAlpha = StiIsoElementHelper.GetCountryFromAlpha2(alpha2, mapId);
		if (countryFromAlpha != null)
		{
			return countryFromAlpha.Names.FirstOrDefault();
		}
		string nameFromIsoAlpha = StiRegionInfoHelper.GetNameFromIsoAlpha2(alpha2);
		if (nameFromIsoAlpha != null)
		{
			return nameFromIsoAlpha;
		}
		return null;
	}

	public string GetNameFromIsoAlpha3(string alpha3, string mapId, string lang, IStiReport report = null)
	{
		StiMapSvg svgBlockFromName = StiMapResourceHelper.GetSvgBlockFromName(alpha3, mapId, lang, report as StiReport);
		if (svgBlockFromName != null)
		{
			return svgBlockFromName.EnglishName;
		}
		StiIsoCountry countryFromAlpha = StiIsoElementHelper.GetCountryFromAlpha3(alpha3, mapId);
		if (countryFromAlpha != null)
		{
			return countryFromAlpha.Names.FirstOrDefault();
		}
		string nameFromIsoAlpha = StiRegionInfoHelper.GetNameFromIsoAlpha3(alpha3);
		if (nameFromIsoAlpha != null)
		{
			return nameFromIsoAlpha;
		}
		return null;
	}

	public string NormalizeName(string name, string mapId, string lang, IStiReport report = null)
	{
		string isoAlpha2FromName = GetIsoAlpha2FromName(name, mapId, lang, report);
		string nameFromIsoAlpha = GetNameFromIsoAlpha2(isoAlpha2FromName, mapId, lang, report);
		if (!string.IsNullOrWhiteSpace(nameFromIsoAlpha))
		{
			return nameFromIsoAlpha;
		}
		return name;
	}

	public string GetIsoAlpha2FromName(string name, string mapId, string lang, IStiReport report = null)
	{
		if (StiGssMapHelper.IsGssValue(name))
		{
			return name;
		}
		string isoAlpha2FromName = StiMapResourceHelper.GetIsoAlpha2FromName(name, mapId, lang, report as StiReport);
		if (isoAlpha2FromName != null)
		{
			return isoAlpha2FromName;
		}
		isoAlpha2FromName = StiIsoElementHelper.GetIsoAlpha2FromName(name, mapId);
		if (isoAlpha2FromName != null)
		{
			return isoAlpha2FromName;
		}
		isoAlpha2FromName = StiRegionInfoHelper.GetIsoAlpha2FromName(name);
		if (isoAlpha2FromName != null)
		{
			return isoAlpha2FromName;
		}
		return null;
	}

	public string GetIsoAlpha3FromName(string name, string mapId, string lang, IStiReport report = null)
	{
		string isoAlpha3FromName = StiMapResourceHelper.GetIsoAlpha3FromName(name, mapId, lang, report as StiReport);
		if (isoAlpha3FromName != null)
		{
			return isoAlpha3FromName;
		}
		isoAlpha3FromName = StiIsoElementHelper.GetIsoAlpha3FromName(name, mapId);
		if (isoAlpha3FromName != null)
		{
			return isoAlpha3FromName;
		}
		isoAlpha3FromName = StiRegionInfoHelper.GetIsoAlpha3FromName(name);
		if (isoAlpha3FromName != null)
		{
			return isoAlpha3FromName;
		}
		return null;
	}

	public List<string> ConvertMapKeysToIsoAlpha2(List<string> mapKeys, string mapId, string lang, IStiReport report = null)
	{
		if (mapKeys == null || mapKeys.Count == 0)
		{
			return null;
		}
		return (from k in mapKeys
			select StiMapResourceHelper.GetIsoAlpha2FromName(k, mapId, lang, report as StiReport) into k
			where !string.IsNullOrWhiteSpace(k)
			select k).Distinct().ToList();
	}

	public List<string> GetMapKeysFromNames(List<object> values, string mapId, string lang, IStiReport report = null)
	{
		return (from n in values?.Select((object v) => v?.ToString())
			select GetMapKeyFromName(n, mapId, lang, report)).ToList();
	}

	private string GetMapKeyFromName(string name, string mapId, string lang, IStiReport report = null)
	{
		if (StiGssMapHelper.IsGssValue(name))
		{
			return name;
		}
		string isoAlpha2FromName = new StiMapKeyHelper().GetIsoAlpha2FromName(name, mapId, lang, report);
		if (isoAlpha2FromName == null)
		{
			return name;
		}
		return StiMapResourceHelper.GetSvgBlockFromIsoAlpha2(isoAlpha2FromName, mapId, lang, report as StiReport)?.Key;
	}

	public static string Simplify(string key)
	{
		return key?.ToLowerInvariant()?.Replace(" ", "")?.Replace("-", "");
	}
}
