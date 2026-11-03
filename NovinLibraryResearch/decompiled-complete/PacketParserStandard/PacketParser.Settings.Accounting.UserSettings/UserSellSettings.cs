using System.ComponentModel;
using PacketParser.Scale;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserSellSettings
{
	[DefaultValue(false)]
	public bool ScaleListenEnable { get; set; }

	public MicroScaleInfo? ScaleInfoForReadWeight { get; set; }

	[DefaultValue(true)]
	public bool WaitForUserAccept { get; set; } = true;

}
