using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Maps.Helpers;

public static class StiMapHelper
{
	private static StiReport globalReport;

	private static StiMap globalMap;

	private static Bitmap mapIcon;

	public static Bitmap MapIcon => mapIcon ?? (mapIcon = StiImageUtils.GetImage(typeof(StiMapHelper).Module.Assembly, "Stimulsoft.Report.Images.Office2013.ToolBox.StiMap32.png", true));

	public static bool IsWorld(StiMapID id)
	{
		return id == StiMapID.World;
	}

	public static bool IsAfrica(StiMapID id)
	{
		return id == StiMapID.SouthAfrica;
	}

	public static bool IsNorthAmerica(StiMapID id)
	{
		if (id == StiMapID.Canada || id == StiMapID.USA || id == StiMapID.Mexico)
		{
			return true;
		}
		return false;
	}

	public static bool IsSouthAmerica(StiMapID id)
	{
		switch (id)
		{
		case StiMapID.Brazil:
		case StiMapID.Argentina:
		case StiMapID.Bolivia:
		case StiMapID.Chile:
		case StiMapID.Colombia:
		case StiMapID.Ecuador:
		case StiMapID.FalklandIslands:
		case StiMapID.Guyana:
		case StiMapID.Paraguay:
		case StiMapID.Peru:
		case StiMapID.Suriname:
		case StiMapID.Uruguay:
		case StiMapID.Venezuela:
			return true;
		default:
			return false;
		}
	}

	public static bool IsEU(StiMapID id)
	{
		switch (id)
		{
		case StiMapID.Austria:
		case StiMapID.EU:
		case StiMapID.France:
		case StiMapID.Germany:
		case StiMapID.Italy:
		case StiMapID.Netherlands:
		case StiMapID.Russia:
		case StiMapID.UK:
		case StiMapID.UKCountries:
		case StiMapID.Albania:
		case StiMapID.Andorra:
		case StiMapID.Belarus:
		case StiMapID.Belgium:
		case StiMapID.BosniaAndHerzegovina:
		case StiMapID.Bulgaria:
		case StiMapID.Croatia:
		case StiMapID.CzechRepublic:
		case StiMapID.Denmark:
		case StiMapID.Estonia:
		case StiMapID.Finland:
		case StiMapID.Georgia:
		case StiMapID.Greece:
		case StiMapID.Hungary:
		case StiMapID.Iceland:
		case StiMapID.Ireland:
		case StiMapID.Latvia:
		case StiMapID.Liechtenstein:
		case StiMapID.Lithuania:
		case StiMapID.Luxembourg:
		case StiMapID.Macedonia:
		case StiMapID.Malta:
		case StiMapID.Moldova:
		case StiMapID.Monaco:
		case StiMapID.Montenegro:
		case StiMapID.Norway:
		case StiMapID.Poland:
		case StiMapID.Portugal:
		case StiMapID.Romania:
		case StiMapID.SanMarino:
		case StiMapID.Serbia:
		case StiMapID.Slovakia:
		case StiMapID.Slovenia:
		case StiMapID.Spain:
		case StiMapID.Sweden:
		case StiMapID.Switzerland:
		case StiMapID.Turkey:
		case StiMapID.Ukraine:
		case StiMapID.Vatican:
			return true;
		default:
			return false;
		}
	}

	public static bool IsOceania(StiMapID id)
	{
		switch (id)
		{
		case StiMapID.Australia:
		case StiMapID.Indonesia:
		case StiMapID.NewZealand:
		case StiMapID.Oceania:
			return true;
		default:
			return false;
		}
	}

	public static bool IsAsia(StiMapID id)
	{
		switch (id)
		{
		case StiMapID.China:
		case StiMapID.Afghanistan:
		case StiMapID.Armenia:
		case StiMapID.Azerbaijan:
		case StiMapID.Cyprus:
		case StiMapID.India:
		case StiMapID.Israel:
		case StiMapID.Japan:
		case StiMapID.Kazakhstan:
		case StiMapID.Malaysia:
		case StiMapID.Philippines:
		case StiMapID.SaudiArabia:
		case StiMapID.SouthKorea:
		case StiMapID.Thailand:
		case StiMapID.Vietnam:
		case StiMapID.MiddleEast:
		case StiMapID.Oman:
		case StiMapID.Qatar:
			return true;
		default:
			return false;
		}
	}

	public static string[] GetStates(StiReport report, string id, string lang)
	{
		StiMapSvgContainer stiMapSvgContainer = StiMapLoader.LoadResource(report, id, lang);
		if (stiMapSvgContainer == null)
		{
			return new string[0];
		}
		return stiMapSvgContainer.HashPaths.Keys.ToArray();
	}

	public static StiMap GetMapSample(StiReport baseReport, string mapID = "USA")
	{
		if (globalReport == null)
		{
			globalReport = new StiReport();
		}
		globalReport.Styles.Clear();
		if (baseReport != null && baseReport.Styles.Count > 0)
		{
			globalReport.Styles.AddRange(baseReport.Styles);
		}
		if (globalMap == null)
		{
			globalMap = new StiMap();
			globalMap.MapData = "[{\"Key\":\"Alabama\",\"Group\":\"3\"},{\"Key\":\"Alaska\",\"Group\":\"1\"},{\"Key\":\"Arizona\",\"Group\":\"1\"},{\"Key\":\"Arkansas\",\"Group\":\"2\"},{\"Key\":\"California\",\"Group\":\"1\"},{\"Key\":\"Colorado\",\"Group\":\"1\"},{\"Key\":\"Connecticut\",\"Group\":\"3\"},{\"Key\":\"Delaware\",\"Group\":\"3\"},{\"Key\":\"Florida\",\"Group\":\"3\"},{\"Key\":\"Georgia\",\"Group\":\"3\"},{\"Key\":\"Hawaii\",\"Group\":\"2\"},{\"Key\":\"Idaho\",\"Group\":\"1\"},{\"Key\":\"Illinois\",\"Group\":\"3\"},{\"Key\":\"Indiana\",\"Group\":\"3\"},{\"Key\":\"Iowa\",\"Group\":\"2\"},{\"Key\":\"Kansas\",\"Group\":\"2\"},{\"Key\":\"Kentucky\",\"Group\":\"3\"},{\"Key\":\"Louisiana\",\"Group\":\"2\"},{\"Key\":\"Maine\",\"Group\":\"3\"},{\"Key\":\"Maryland\",\"Group\":\"3\"},{\"Key\":\"Massachusetts\",\"Group\":\"3\"},{\"Key\":\"Michigan\",\"Group\":\"3\"},{\"Key\":\"Minnesota\",\"Group\":\"2\"},{\"Key\":\"Mississippi\",\"Group\":\"3\"},{\"Key\":\"Missouri\",\"Group\":\"2\"},{\"Key\":\"Montana\",\"Group\":\"1\"},{\"Key\":\"Nebraska\",\"Group\":\"2\"},{\"Key\":\"Nevada\",\"Group\":\"1\"},{\"Key\":\"NewHampshire\",\"Group\":\"3\"},{\"Key\":\"NewJersey\",\"Group\":\"3\"},{\"Key\":\"NewMexico\",\"Group\":\"1\"},{\"Key\":\"NewYork\",\"Group\":\"3\"},{\"Key\":\"NorthCarolina\",\"Group\":\"3\"},{\"Key\":\"NorthDakota\",\"Group\":\"2\"},{\"Key\":\"Ohio\",\"Group\":\"3\"},{\"Key\":\"Oklahoma\",\"Group\":\"2\"},{\"Key\":\"Oregon\",\"Group\":\"1\"},{\"Key\":\"Pennsylvania\",\"Group\":\"3\"},{\"Key\":\"RhodeIsland\",\"Group\":\"3\"},{\"Key\":\"SouthCarolina\",\"Group\":\"3\"},{\"Key\":\"SouthDakota\",\"Group\":\"2\"},{\"Key\":\"Tennessee\",\"Group\":\"3\"},{\"Key\":\"Texas\",\"Group\":\"2\"},{\"Key\":\"Utah\",\"Group\":\"1\"},{\"Key\":\"Vermont\",\"Group\":\"3\"},{\"Key\":\"Virginia\",\"Group\":\"3\"},{\"Key\":\"Washington\",\"Group\":\"1\"},{\"Key\":\"WestVirginia\",\"Group\":\"3\"},{\"Key\":\"Wisconsin\",\"Group\":\"3\"},{\"Key\":\"Wyoming\",\"Group\":\"1\"}]";
			globalReport.Pages[0].Components.Add(globalMap);
		}
		globalMap.Stretch = true;
		globalMap.MapType = StiMapType.Individual;
		globalMap.DisplayNameType = StiDisplayNameType.None;
		globalMap.ShowLegend = false;
		globalMap.MapIdent = mapID;
		globalMap.ShowValue = false;
		globalMap.Stretch = true;
		return globalMap;
	}

	public static Color[] GetColors()
	{
		return new Color[8]
		{
			ColorTranslator.FromHtml("#90c2a8"),
			ColorTranslator.FromHtml("#da5459"),
			ColorTranslator.FromHtml("#efca70"),
			ColorTranslator.FromHtml("#63b8e3"),
			ColorTranslator.FromHtml("#ab92c4"),
			ColorTranslator.FromHtml("#6d58d9"),
			ColorTranslator.FromHtml("#fb6b40"),
			ColorTranslator.FromHtml("#e34e2e")
		};
	}

	public static string PrepareIsoCode(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		int num = text.IndexOf("-");
		if (num != -1)
		{
			return text.Substring(num + 1);
		}
		return text;
	}
}
