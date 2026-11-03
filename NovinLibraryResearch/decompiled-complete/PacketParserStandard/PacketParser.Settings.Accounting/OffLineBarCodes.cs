using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class OffLineBarCodes
{
	public string Format { get; set; }

	public string ControlChar { get; set; }

	[DefaultValue(1)]
	public double ZaribRial { get; set; } = 1.0;


	[DefaultValue(1)]
	public double ZaribVazn { get; set; } = 1.0;


	[DefaultValue(false)]
	public bool Status { get; set; }
}
