using Newtonsoft.Json;

namespace PacketParser.Entities;

public class AndroidPrintStyle
{
	public string Name { get; set; }

	public int Delay { get; set; }

	public PrintType PrintType { get; set; }

	public PrintFormat PrintFormat { get; set; }

	public ConnectoinType ConnectoinType { get; set; }

	public string ConnectionValue { get; set; }

	public string Html { get; set; }

	[JsonIgnore]
	public int UserId { get; set; }
}
