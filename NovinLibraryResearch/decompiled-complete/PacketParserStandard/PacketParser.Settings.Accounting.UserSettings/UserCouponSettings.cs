using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserCouponSettings
{
	[DefaultValue(true)]
	public bool DisplayFilterDate { get; set; } = true;


	[DefaultValue(false)]
	public bool DisplayFilterUsageStatus { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterServerStatus { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterCashier { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterMode { get; set; }
}
