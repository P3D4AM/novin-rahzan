using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class PcPossSettings : Serializable<PcPossSettings>
{
	[DefaultValue(EnCurreny.Rial)]
	public EnCurreny CurrencyMode { get; set; }
}
