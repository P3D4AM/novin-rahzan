using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class ProductReportSettings
{
	[DefaultValue(true)]
	public bool DisplayProduct { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayProductGroup { get; set; }

	[DefaultValue(true)]
	public bool DisplayHesab { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayPrice { get; set; }

	[DefaultValue(true)]
	public bool DisplayUser { get; set; }

	[DefaultValue(true)]
	public bool DisplayDate { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayDepot { get; set; }

	[DefaultValue(true)]
	public bool DisplaySerial { get; set; }

	[DefaultValue(true)]
	public bool DisplayDescription { get; set; }

	[DefaultValue(true)]
	public bool DisplayNumber { get; set; }
}
