using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Maps.Helpers;

public static class StiCustomMapFinder
{
	private static StiReport lastReport;

	private static List<string> defaultMaps;

	private static Dictionary<string, StiMapSvgContainer> customMaps;

	public static int Version { get; private set; }

	static StiCustomMapFinder()
	{
		defaultMaps = new List<string>();
		customMaps = new Dictionary<string, StiMapSvgContainer>();
		Version = 1;
		Array values = Enum.GetValues(typeof(StiMapID));
		foreach (StiMapID item in values)
		{
			defaultMaps.Add(item.ToString().ToLowerInvariant());
		}
		defaultMaps.Add("Germany_DE".ToLowerInvariant());
		defaultMaps.Add("France_FR".ToLowerInvariant());
		defaultMaps.Add("Italy_IT".ToLowerInvariant());
		defaultMaps.Add("Russia_RU".ToLowerInvariant());
	}

	internal static Dictionary<string, StiMapSvgContainer> GetMaps()
	{
		return customMaps;
	}

	internal static void Clear()
	{
		lastReport = null;
		Version++;
		customMaps.Clear();
	}

	internal static void Init(StiReport report)
	{
		bool flag = lastReport != report;
		if (report == null)
		{
			if (flag)
			{
				Version++;
			}
			lastReport = null;
			customMaps.Clear();
			return;
		}
		lastReport = report;
		Dictionary<string, StiMapSvgContainer> dictionary = new Dictionary<string, StiMapSvgContainer>(customMaps);
		List<StiResource> list = (from StiResource x in report.Dictionary.Resources
			where x.Type == StiResourceType.Map && x.Content != null
			select x).ToList();
		foreach (StiResource item in list)
		{
			if (defaultMaps.Contains(item.Name.ToLowerInvariant()))
			{
				continue;
			}
			if (dictionary.ContainsKey(item.Name))
			{
				dictionary.Remove(item.Name);
				continue;
			}
			try
			{
				string text = StiGZipHelper.ConvertByteArrayToString(item.Content);
				StiMapSvgContainer stiMapSvgContainer = new StiMapSvgContainer();
				JsonConvert.PopulateObject(text, (object)stiMapSvgContainer, StiJsonHelper.DefaultSerializerSettings);
				if (stiMapSvgContainer.Paths == null)
				{
					stiMapSvgContainer.Paths = new List<StiMapSvg>();
				}
				if (stiMapSvgContainer.Paths.Count == 0)
				{
					stiMapSvgContainer.IsNotCorrect = true;
				}
				stiMapSvgContainer.Prepare();
				stiMapSvgContainer.IsCustom = true;
				customMaps.Add(item.Name, stiMapSvgContainer);
			}
			catch
			{
				StiMapSvgContainer stiMapSvgContainer2 = new StiMapSvgContainer();
				stiMapSvgContainer2.Prepare();
				stiMapSvgContainer2.IsNotCorrect = true;
				stiMapSvgContainer2.IsCustom = true;
				customMaps.Add(item.Name, stiMapSvgContainer2);
			}
			flag = true;
		}
		if (dictionary.Count > 0)
		{
			flag = true;
			foreach (KeyValuePair<string, StiMapSvgContainer> item2 in dictionary)
			{
				customMaps.Remove(item2.Key);
			}
		}
		if (flag)
		{
			Version++;
		}
	}

	internal static byte[] GetDefaultIcon()
	{
		return StiImageUtils.GetByteArray(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiMap64.png");
	}

	internal static string GetIcon(StiReport report, string mapIdent)
	{
		Init(report);
		customMaps.TryGetValue(mapIdent, out var value);
		return value?.Icon;
	}

	public static bool IsCustom(string mapIdent)
	{
		return !defaultMaps.Contains(mapIdent.ToLowerInvariant());
	}

	internal static bool IsCorrentMap(StiReport report, string mapIdent)
	{
		Init(report);
		customMaps.TryGetValue(mapIdent, out var value);
		if (value == null)
		{
			return false;
		}
		return !value.IsNotCorrect;
	}

	internal static StiMapSvgContainer GetContainer(StiReport report, string mapIdent)
	{
		Init(report);
		customMaps.TryGetValue(mapIdent, out var value);
		return value;
	}
}
