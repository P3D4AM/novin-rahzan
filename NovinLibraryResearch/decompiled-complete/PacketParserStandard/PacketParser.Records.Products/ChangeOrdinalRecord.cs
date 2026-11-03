using System;

namespace PacketParser.Records.Products;

public record ChangeOrdinalRecord
{
	public Guid Guid { get; private set; }

	public short Ordinal { get; private set; }

	public ChangeOrdinalRecord(Guid guid, short ordinal)
	{
		Guid = guid;
		Ordinal = ordinal;
	}
}
