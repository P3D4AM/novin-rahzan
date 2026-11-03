using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserPlacementSettings
{
	[DefaultValue(true)]
	public bool DisplayFilterDate { get; set; } = true;


	[DefaultValue(false)]
	public bool DisplayFilterNumber { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterTafsil { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterPeopleCount { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterSum { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterCashier { get; set; }
}
