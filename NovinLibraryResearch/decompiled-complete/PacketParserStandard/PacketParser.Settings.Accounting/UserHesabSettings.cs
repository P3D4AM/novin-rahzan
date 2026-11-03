using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class UserHesabSettings
{
	[DefaultValue(true)]
	public bool KolDateSh { get; set; } = true;


	[DefaultValue(true)]
	public bool KolCode { get; set; } = true;


	[DefaultValue(true)]
	public bool MoeinCode { get; set; } = true;


	[DefaultValue(true)]
	public bool MoeinDateSh { get; set; } = true;

}
