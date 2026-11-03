using System.ComponentModel;

namespace PacketParser.Entities.Accounting;

public class ClientEntityUpdateSetting
{
	[DefaultValue(false)]
	public bool Reception { get; set; }

	[DefaultValue(false)]
	public bool Payment { get; set; }

	[DefaultValue(false)]
	public bool OrderSell { get; set; }

	[DefaultValue(false)]
	public bool OrderBuy { get; set; }

	[DefaultValue(false)]
	public bool OrderBackSell { get; set; }

	[DefaultValue(false)]
	public bool OrderBackBuy { get; set; }

	[DefaultValue(false)]
	public bool OrderPreSell { get; set; }

	[DefaultValue(false)]
	public bool OrderPreBuy { get; set; }

	[DefaultValue(false)]
	public bool OrderWaste { get; set; }

	[DefaultValue(false)]
	public bool OrderDepositEnter { get; set; }

	[DefaultValue(false)]
	public bool OrderDepositOut { get; set; }
}
