using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserDepotHavaleSettings
{
	[DefaultValue(true)]
	public bool DisplayFilterDate { get; set; } = true;


	[DefaultValue(false)]
	public bool DisplayFilterKardexAffective { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterSanadAffective { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterCustomer { get; set; }

	[DefaultValue(false)]
	public bool DisplayFilterCashier { get; set; }
}
