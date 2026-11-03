using System;
using System.Collections.Generic;

namespace Stimulsoft.Report.Toolbox;

public sealed class StiInfographicsToolboxInfo
{
	public object Tag { get; set; }

	public string LocName { get; set; }

	public string Icon { get; set; }

	public bool BeginGroup { get; set; }

	public bool SkipCategory { get; set; }

	public bool IsShowMore { get; set; }

	public StiInfographicsItemType ItemType { get; set; }

	public List<StiInfographicsToolboxInfo> Infos { get; set; }

	public byte[] CustomIcon { get; set; }

	public override string ToString()
	{
		return LocName;
	}

	public StiInfographicsToolboxInfo(object tag, string locName, string icon)
	{
		Tag = tag;
		LocName = locName;
		Icon = icon;
	}

	public StiInfographicsToolboxInfo(Type type, string locName, string icon, bool beginGroup)
	{
		Tag = type;
		LocName = locName;
		Icon = icon;
		BeginGroup = beginGroup;
	}

	public StiInfographicsToolboxInfo(StiInfographicsItemType itemType, object tag, string locName, string icon)
	{
		ItemType = itemType;
		Tag = tag;
		LocName = locName;
		Icon = icon;
	}

	public StiInfographicsToolboxInfo(StiInfographicsItemType itemType, Type type, string locName, string icon, bool beginGroup)
	{
		ItemType = itemType;
		Tag = type;
		LocName = locName;
		Icon = icon;
		BeginGroup = beginGroup;
	}
}
