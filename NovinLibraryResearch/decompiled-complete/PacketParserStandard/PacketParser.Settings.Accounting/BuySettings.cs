using System;
using System.ComponentModel;
using PacketParser.Settings.Accounting.Orders.OrderDefaults;

namespace PacketParser.Settings.Accounting;

public class BuySettings : OrderDefaultSettingBase
{
	[DefaultValue(true)]
	public bool ShowKholaseGardeshCustomer { get; set; } = true;


	public Guid? DefaultPriceLevelGuid { get; set; }
}
