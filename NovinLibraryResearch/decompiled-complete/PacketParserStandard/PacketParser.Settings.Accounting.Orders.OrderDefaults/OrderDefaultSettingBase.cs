using System.ComponentModel;

namespace PacketParser.Settings.Accounting.Orders.OrderDefaults;

public class OrderDefaultSettingBase
{
	[DefaultValue(true)]
	public bool SanadAffective { get; set; } = true;


	[DefaultValue(true)]
	public bool KardexAffective { get; set; } = true;

}
