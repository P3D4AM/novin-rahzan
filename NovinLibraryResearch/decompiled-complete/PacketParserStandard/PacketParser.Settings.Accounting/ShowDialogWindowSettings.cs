using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class ShowDialogWindowSettings
{
	[DefaultValue(true)]
	public bool ShowDialogWindows { get; set; } = true;

}
