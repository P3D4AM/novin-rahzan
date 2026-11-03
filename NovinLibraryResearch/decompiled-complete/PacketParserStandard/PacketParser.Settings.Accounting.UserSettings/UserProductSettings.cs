using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserProductSettings
{
	public bool SrchByName { get; set; } = true;


	public bool SrchByCode { get; set; } = true;


	public bool SrchByDescription { get; set; }

	public bool SrchByTechnical1 { get; set; }

	public bool SrchByTechnical2 { get; set; }

	public bool SrchByTechnical3 { get; set; }

	public bool SrchByTechnical4 { get; set; }

	public bool SrchByTechnical5 { get; set; }

	public bool SrchByTechnical6 { get; set; }

	public bool SrchByTechnical7 { get; set; }

	public bool SrchByTechnical8 { get; set; }

	public ProductSort ProductSort { get; set; }

	public bool PriceLevel1 { get; set; }

	public bool PriceLevel2 { get; set; }

	public bool PriceLevel3 { get; set; }

	public bool PriceLevel4 { get; set; }

	public bool PriceLevel5 { get; set; }

	public bool PriceLevel6 { get; set; }

	public bool PriceLevel7 { get; set; }

	public bool PriceLevel8 { get; set; }

	public ProductSearchOption ProductSearchOption { get; set; }

	public SearchLocation ProductSearchLocation { get; set; }

	[DefaultValue(true)]
	public bool ShowNameForDisplayAgainstName { get; set; } = true;

}
