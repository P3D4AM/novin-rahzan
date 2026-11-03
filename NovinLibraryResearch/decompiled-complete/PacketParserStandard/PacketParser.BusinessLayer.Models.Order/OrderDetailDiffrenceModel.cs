using System;

namespace PacketParser.BusinessLayer.Models.Order;

public class OrderDetailDiffrenceModel
{
	public Guid ProductGuid { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public string ProductVahedName { get; set; }

	public decimal OldQuantity { get; set; }

	public decimal NewQuantity { get; set; }

	public decimal Diffrence => OldQuantity - NewQuantity;
}
