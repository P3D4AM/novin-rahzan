using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserPrintSettings
{
	[DefaultValue(360)]
	public short PrintWidth { get; set; } = 360;


	[DefaultValue(true)]
	public bool IsPrintEnable { get; set; } = true;


	[DefaultValue(false)]
	public bool IsBehpardakhtPrintEnable { get; set; }

	[DefaultValue(false)]
	public bool IsSepehrSaderatPrintEnable { get; set; }

	[DefaultValue(ENPrintType.AllA4Printers)]
	public ENPrintType PrinterType { get; set; }

	[DefaultValue(1)]
	public int OrderSellCountPrint { get; set; } = 1;


	[DefaultValue(1)]
	public int ReceptionCountPrint { get; set; } = 1;


	[DefaultValue(false)]
	public bool ShowPreview { get; set; }

	[DefaultValue(false)]
	public bool ShowDesign { get; set; }

	[DefaultValue(false)]
	public bool AutoPrintAfterAdd { get; set; }

	[DefaultValue(true)]
	public bool AskWouldYouLikeToPrint { get; set; } = true;


	[DefaultValue(ENFRMPrint.DisplayForAll)]
	public ENFRMPrint FRMPrint { get; set; } = ENFRMPrint.DisplayForAll;


	public bool AutoPrintAfterUpdate { get; set; }

	[DefaultValue(false)]
	public bool PrintCheckAghsat { get; set; }

	public string HeadFont { get; set; }

	public string BodyFont { get; set; }

	public string FactorType { get; set; }
}
