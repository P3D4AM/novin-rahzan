using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserBarcodeSettings
{
	public bool Barcode1 { get; set; }

	public bool Barcode2 { get; set; }

	public bool Barcode3 { get; set; }

	public string BarcodeVoiceSTR { get; set; } = "";


	public string Format1 { get; set; } = "HHFFFFFDDDDDI";


	public string Format2 { get; set; } = "";


	public string Format3 { get; set; } = "";


	public string Vazn1 { get; set; } = "";


	public string Vazn2 { get; set; } = "";


	public string Vazn3 { get; set; } = "";


	public int Vahed1 { get; set; }

	public int Vahed2 { get; set; }

	public int Vahed3 { get; set; }

	public string Ctrl1 { get; set; } = "";


	public string Ctrl2 { get; set; } = "";


	public string Ctrl3 { get; set; } = "";


	public bool BarcodeT1 { get; set; }

	public string FormatT1 { get; set; } = "AAJJJJBBBBTTTTTTCCGGGGGGGG";


	public string ContorolT1 { get; set; } = "";


	[DefaultValue(1)]
	public int MablaghT1 { get; set; } = 1;


	public bool BarcodeT2 { get; set; }

	public string FormatT2 { get; set; } = "";


	public string ContorolT2 { get; set; } = "";


	[DefaultValue(1)]
	public int MablaghT2 { get; set; } = 1;


	public bool BarcodeT3 { get; set; }

	public string FormatT3 { get; set; } = "";


	public string ContorolT3 { get; set; } = "";


	[DefaultValue(1)]
	public int MablaghT3 { get; set; } = 1;


	public bool BarcodeT_Sell { get; set; }

	public string FormatT_Sell { get; set; } = "KKBBBBBBGGGGGGGGGGGG";


	public EnOrderBarcodeDetect OrderBarcodeDetect { get; set; }

	public string ContorolT_Sell { get; set; } = "";


	[DefaultValue(1)]
	public int MablaghT_Sell { get; set; } = 1;

}
