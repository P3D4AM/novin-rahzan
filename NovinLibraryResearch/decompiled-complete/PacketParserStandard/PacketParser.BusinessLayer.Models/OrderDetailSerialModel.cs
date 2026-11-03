using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class OrderDetailSerialModel
{
	public Guid OrderDetailGuid { get; set; }

	public Document OrderType { get; set; }

	public DateTime OrderDate { get; set; }
}
