using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserTarazHesabSettings
{
	[DefaultValue(false)]
	public bool DisplayFilterKol { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterMoein { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterTafsil { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterSanadNo { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterSanadType { get; set; }
}
