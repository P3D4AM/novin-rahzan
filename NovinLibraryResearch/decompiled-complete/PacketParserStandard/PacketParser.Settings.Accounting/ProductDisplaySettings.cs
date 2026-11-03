using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class ProductDisplaySettings
{
	[DefaultValue(true)]
	public bool DisplayProduct { get; set; }

	[DefaultValue(true)]
	public bool DisplayProductGroup { get; set; }

	[DefaultValue(true)]
	public bool DisplayHesab { get; set; }

	[DefaultValue(true)]
	public bool DisplayPrice { get; set; }

	[DefaultValue(true)]
	public bool DisplayUser { get; set; }

	[DefaultValue(true)]
	public bool DisplayRDescription { get; set; }

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

	[DefaultValue(true)]
	public bool DisplayVisitorID { get; set; }

	[DefaultValue(true)]
	public bool DisplayRowSum { get; set; }

	[DefaultValue(true)]
	public bool DisplayUnitPrice { get; set; }

	[DefaultValue(true)]
	public bool DisplayPrice_Visitor { get; set; }

	[DefaultValue(true)]
	public bool DisplaySelectedHesab { get; set; }

	[DefaultValue(true)]
	public bool DisplaySelectedPRD { get; set; }

	[DefaultValue(true)]
	public bool DisplayRowDiscount { get; set; }

	[DefaultValue(true)]
	public bool DisplayCustGRP { get; set; }

	[DefaultValue(true)]
	public bool DisplayMasir { get; set; }

	[DefaultValue(true)]
	public bool DisplayMaliat { get; set; }

	[DefaultValue(true)]
	public bool DisplayAvarez { get; set; }
}
