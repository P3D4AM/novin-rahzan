using System;
using PacketParser.Entities;

namespace PacketParser.Filters.Configs;

public class OrderJoinConfig
{
	public WebOrder Order { get; set; }

	public bool ClearCurrentOrderItems { get; set; }

	public Guid PriceTypeGuid { get; set; }

	public bool ChangeDiscount { get; set; } = true;


	public bool ChangeVisitor { get; set; } = true;

}
