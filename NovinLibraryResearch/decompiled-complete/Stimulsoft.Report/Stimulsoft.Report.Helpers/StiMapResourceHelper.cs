using System.Collections.Generic;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Helpers;

public class StiMapResourceHelper
{
	public static StiMapSvg GetSvgBlockFromIsoAlpha2(string alpha2, string mapId, string lang, StiReport report = null)
	{
		if (string.IsNullOrWhiteSpace(alpha2))
		{
			return null;
		}
		StiMapSvgContainer resource = GetResource(mapId, lang, report);
		if (resource == null)
		{
			return null;
		}
		string text = StiMapKeyHelper.Simplify(alpha2);
		foreach (KeyValuePair<string, StiMapSvg> hashPath in resource.HashPaths)
		{
			if (StiMapKeyHelper.Simplify(hashPath.Value.ISOCode) == text)
			{
				return hashPath.Value;
			}
		}
		return null;
	}

	public static StiMapSvg GetSvgBlockFromName(string name, string mapId, string lang, StiReport report = null)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}
		StiMapSvgContainer resource = GetResource(mapId, lang, report);
		if (resource == null)
		{
			return null;
		}
		string text = StiMapKeyHelper.Simplify(name);
		string text2 = StiMapKeyHelper.Simplify(DecodeAlpha(name));
		foreach (KeyValuePair<string, StiMapSvg> hashPath in resource.HashPaths)
		{
			if (StiMapKeyHelper.Simplify(hashPath.Value.Key) == text || StiMapKeyHelper.Simplify(hashPath.Value.EnglishName) == text || StiMapKeyHelper.Simplify(hashPath.Value.ISOCode) == text || StiMapKeyHelper.Simplify(hashPath.Value.ISOCode) == text2)
			{
				return hashPath.Value;
			}
		}
		return null;
	}

	public static string GetIsoAlpha2FromName(string name, string mapId, string lang, StiReport report = null)
	{
		return GetSvgBlockFromName(name, mapId, lang, report)?.ISOCode;
	}

	public static string GetIsoAlpha3FromName(string name, string mapId, string lang, StiReport report = null)
	{
		string isoAlpha2FromName = GetIsoAlpha2FromName(name, mapId, lang, report);
		if (isoAlpha2FromName == null)
		{
			return null;
		}
		return StiIsoElementHelper.GetCountryFromAlpha2(isoAlpha2FromName, mapId)?.Alpha3;
	}

	private static StiMapSvgContainer GetResource(string mapId, string lang, StiReport report = null)
	{
		if (!string.IsNullOrWhiteSpace(mapId))
		{
			return StiMapLoader.LoadResource(report, mapId, lang);
		}
		return StiMapLoader.LoadResource(report, StiMapID.World.ToString(), lang);
	}

	private static string DecodeAlpha(string alpha2)
	{
		if (alpha2 == null)
		{
			return alpha2;
		}
		if (alpha2.Length != 5 || !char.IsLetter(alpha2[0]) || !char.IsLetter(alpha2[1]) || alpha2[2] != '-' || !char.IsLetter(alpha2[3]) || !char.IsLetter(alpha2[4]))
		{
			return alpha2;
		}
		string[] array = alpha2.Split(new char[1] { '-' });
		if (array.Length < 2)
		{
			return null;
		}
		return array[1];
	}
}
