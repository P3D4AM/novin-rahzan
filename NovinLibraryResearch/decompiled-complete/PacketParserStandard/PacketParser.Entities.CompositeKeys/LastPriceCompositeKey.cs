using System;

namespace PacketParser.Entities.CompositeKeys;

public class LastPriceCompositeKey
{
	public Guid TafsilGuid { get; set; }

	public Guid ProductGuid { get; set; }
}
