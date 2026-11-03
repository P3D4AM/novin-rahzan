using System;
using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserCustomerSettings
{
	[DefaultValue(true)]
	public bool SrchByName { get; set; } = true;


	[DefaultValue(true)]
	public bool SrchByFamily { get; set; } = true;


	[DefaultValue(true)]
	public bool SrchByCode { get; set; } = true;


	[DefaultValue(CustomerSort.NameAsc)]
	public CustomerSort CustomerSort { get; set; }

	[DefaultValue(true)]
	public bool SrchByTelMob { get; set; } = true;


	[DefaultValue(true)]
	public bool SrchByAdd1 { get; set; } = true;


	[DefaultValue(true)]
	public bool SrchByAdd2 { get; set; } = true;


	[DefaultValue(true)]
	[Obsolete("حذف شده", true)]
	public bool SrchByFax { get; set; } = true;


	[DefaultValue(true)]
	public bool SrchByZip { get; set; } = true;


	[DefaultValue(true)]
	public bool SrchByDecription { get; set; } = true;


	[DefaultValue(CustomerSearchOption.All)]
	public CustomerSearchOption CusSearchOption { get; set; }

	[DefaultValue(SearchLocation.All)]
	public SearchLocation CusSearchLocation { get; set; }
}
