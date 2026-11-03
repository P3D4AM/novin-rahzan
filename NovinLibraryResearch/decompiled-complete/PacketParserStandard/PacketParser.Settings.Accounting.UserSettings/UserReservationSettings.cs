using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserReservationSettings
{
	[DefaultValue(true)]
	public bool DisplayFilterDate { get; set; } = true;


	[DefaultValue(false)]
	public bool DisplayFilterNumber { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterTafsil { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterCashier { get; set; }
}
