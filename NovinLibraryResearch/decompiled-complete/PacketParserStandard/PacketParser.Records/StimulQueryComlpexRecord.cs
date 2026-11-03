using System;

namespace PacketParser.Records;

public record StimulQueryComlpexRecord
{
	public Guid Guid { get; set; }

	public int Id { get; set; }

	public string SecondKey { get; set; }
}
