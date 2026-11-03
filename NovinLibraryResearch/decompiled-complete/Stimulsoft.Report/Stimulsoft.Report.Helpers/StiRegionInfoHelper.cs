using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Stimulsoft.Report.Helpers;

public class StiRegionInfoHelper
{
	public static string GetIsoAlpha2FromName(string name)
	{
		return GetRegionInfoFromName(name)?.TwoLetterISORegionName;
	}

	public static string GetIsoAlpha3FromName(string name)
	{
		return GetRegionInfoFromName(name)?.ThreeLetterISORegionName;
	}

	public static string GetNameFromIsoAlpha2(string alpha2)
	{
		return GetRegionInfoFromName(alpha2)?.Name;
	}

	public static string GetNameFromIsoAlpha3(string alpha3)
	{
		return GetRegionInfoFromName(alpha3)?.Name;
	}

	public static string GetLocalizedNameFromIsoAlpha2(string alpha2)
	{
		return GetRegionInfoFromName(alpha2)?.NativeName;
	}

	public static string GetLocalizedNameFromIsoAlpha3(string alpha3)
	{
		return GetRegionInfoFromName(alpha3)?.NativeName;
	}

	public static RegionInfo GetRegionInfoFromName(string name)
	{
		string simpliedName = StiMapKeyHelper.Simplify(name);
		return GetAllRegions().FirstOrDefault((RegionInfo c) => StiMapKeyHelper.Simplify(c.Name) == simpliedName || StiMapKeyHelper.Simplify(c.NativeName) == simpliedName || StiMapKeyHelper.Simplify(c.ThreeLetterISORegionName) == simpliedName || StiMapKeyHelper.Simplify(c.ThreeLetterWindowsRegionName) == simpliedName || StiMapKeyHelper.Simplify(c.TwoLetterISORegionName) == simpliedName);
	}

	private static IEnumerable<RegionInfo> GetAllRegions()
	{
		return from c in CultureInfo.GetCultures(CultureTypes.AllCultures)
			where c.LCID != 127
			select new RegionInfo(c.TextInfo.CultureName);
	}
}
