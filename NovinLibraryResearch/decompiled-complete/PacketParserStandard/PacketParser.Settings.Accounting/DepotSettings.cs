using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class DepotSettings
{
	[DefaultValue(true)]
	public bool Enable { get; set; } = true;


	[DefaultValue(true)]
	public bool FactorVisible { get; set; } = true;


	public short DefaultDepot1 { get; set; }

	public short DefaultDepot2 { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState SanadWithoutSerial { get; set; }
}
