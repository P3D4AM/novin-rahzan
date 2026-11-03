using System;

namespace PacketParser.Records.Aghsat;

public record AghsatMSanadRecord
{
	public DateTime DateM { get; set; }

	public string Description { get; set; }
}
