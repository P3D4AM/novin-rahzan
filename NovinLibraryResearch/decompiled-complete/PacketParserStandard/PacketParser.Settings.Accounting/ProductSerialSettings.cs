using System.ComponentModel;
using PacketParser.Records.Products;

namespace PacketParser.Settings.Accounting;

public class ProductSerialSettings
{
	[DefaultValue(true)]
	public bool DisplaySerial { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayGNCode { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayDate { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayProduct { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayProductGroup { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayHesab { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayUser { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayDepot { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayDescription { get; set; } = true;


	public ProductSerialSettingRecord TechnicalInfo1 { get; set; } = new ProductSerialSettingRecord();


	public ProductSerialSettingRecord TechnicalInfo2 { get; set; } = new ProductSerialSettingRecord();


	public ProductSerialSettingRecord TechnicalInfo3 { get; set; } = new ProductSerialSettingRecord();

}
