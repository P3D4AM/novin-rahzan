using PacketParser.Entities;

namespace PacketParser.Filters;

public class ErrorLogFilter
{
	public int DepId { get; set; }

	public ENSource Source { get; set; }

	public long LkSerial { get; set; }

	public string AndroidIme { get; set; }
}
