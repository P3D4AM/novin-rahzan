using PacketParser.Entities;

namespace PacketParser.Performance.Entities;

public class JaegerSamplingSettings
{
	public string Lk { get; set; }

	public string Version { get; set; }

	public string Username { get; set; }

	public ENSource Source { get; set; }
}
