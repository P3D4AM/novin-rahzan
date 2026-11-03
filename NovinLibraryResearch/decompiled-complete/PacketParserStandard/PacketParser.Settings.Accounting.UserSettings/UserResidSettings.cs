using System.Collections.Generic;
using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserResidSettings
{
	[DefaultValue(true)]
	public bool DisplayFilterDate { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayFilterStatus { get; set; } = true;


	public List<int> SelectedStatus { get; set; }
}
