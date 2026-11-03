using System;

namespace PacketParser.Records.Order;

public record OrderDetailAggregatRecord
{
	public decimal Quantity { get; set; }

	public Guid ProductGuid { get; set; }

	public string ProductName { get; set; }

	public string ProductUnit { get; set; }
}
