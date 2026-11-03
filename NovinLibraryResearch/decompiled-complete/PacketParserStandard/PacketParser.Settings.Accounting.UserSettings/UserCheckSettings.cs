using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserCheckSettings
{
	public bool CanPardakhtOutOfDasteCheck { get; set; }

	[DefaultValue(true)]
	public bool DisplayONLoad { get; set; } = true;


	public bool CheckBedehiTalabStartEnable { get; set; }

	[DefaultValue(30)]
	public int DaysCount { get; set; } = 30;


	[DefaultValue(true)]
	public bool DontDisplayIfEmpty { get; set; } = true;


	[DefaultValue(true)]
	public bool PrintVagozari { get; set; } = true;


	[DefaultValue(true)]
	public bool AghsatM_Round { get; set; } = true;


	[DefaultValue(true)]
	public bool AghsatM_Reception { get; set; } = true;


	public bool AghsatM_Sanad { get; set; }

	public string DarVajhe { get; set; } = "";

}
