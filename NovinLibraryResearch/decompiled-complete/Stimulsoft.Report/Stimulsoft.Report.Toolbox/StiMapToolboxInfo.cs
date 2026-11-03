using System.Collections.Generic;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;

namespace Stimulsoft.Report.Toolbox;

public sealed class StiMapToolboxInfo
{
	public StiMapCategory? Category { get; }

	public string MapID { get; }

	public string LocEnName { get; }

	public string LocRuName { get; }

	public string Icon { get; }

	public byte[] CustomIcon { get; }

	public string[] Language { get; set; }

	public List<StiMapToolboxInfo> Infos { get; set; }

	public override string ToString()
	{
		return MapID;
	}

	public string GetLangOriginalName(string id)
	{
		return id switch
		{
			"EN" => "English", 
			"DE" => "Deutsch", 
			"FR" => "Français", 
			"IT" => "Italiano", 
			"RU" => "Русский", 
			_ => null, 
		};
	}

	public StiMapToolboxInfo(StiMapID mapID)
		: this(mapID, null)
	{
	}

	public StiMapToolboxInfo(StiMapID mapID, string[] localization)
	{
		StiMapLoc.GetEnRu(mapID, out var resEn, out var resRu);
		MapID = mapID.ToString();
		LocEnName = resEn;
		LocRuName = resRu;
		Language = localization;
		Icon = MapID;
	}

	public StiMapToolboxInfo(string customMapID, byte[] icon)
	{
		MapID = customMapID;
		LocEnName = customMapID;
		LocRuName = customMapID;
		CustomIcon = icon;
	}

	public StiMapToolboxInfo(StiMapCategory category, string locEnName, string locRuName)
	{
		Category = category;
		LocEnName = locEnName;
		LocRuName = locRuName;
	}
}
