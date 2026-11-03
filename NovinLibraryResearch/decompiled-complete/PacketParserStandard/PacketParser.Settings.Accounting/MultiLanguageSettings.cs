using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class MultiLanguageSettings : Serializable<MultiLanguageSettings>
{
	[DefaultValue(0)]
	public short DigitAfterPoint { get; set; }

	[DefaultValue(3)]
	public short DigitForRound { get; set; } = 3;


	[DefaultValue("ریال")]
	public string CurrencyUnit { get; set; } = "ریال";

}
