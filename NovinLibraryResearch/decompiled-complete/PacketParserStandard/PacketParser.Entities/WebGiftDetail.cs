using System;
using PacketParser.Entities.Interfaces.Accounting.Gift;

namespace PacketParser.Entities;

public class WebGiftDetail : IGiftDetail
{
	public Guid ProductGuid { get; set; }

	public decimal Count { get; set; }
}
