using System;
using PacketParser;

namespace EntityCache.Models.Accounting.Prd;

public class ProductOrderPointModel
{
	public Guid ProductGuid { get; set; }

	public string IdProduct { get; set; }

	public string ProductName { get; set; }

	public string ProductDescription { get; set; }

	public string Pic { get; set; }

	public DateTime Modified { get; set; }

	public decimal Height { get; set; }

	public decimal Width { get; set; }

	public string GroupName { get; set; }

	public Guid VahedGuid { get; set; }

	public string VahedName { get; set; }

	public Guid DepotGuid { get; set; }

	public string DepotName { get; set; }

	public string DepotDescription { get; set; }

	public decimal OrderPoint { get; set; }

	public string OrderPointFroDisplay => OrderPoint.ToString(3);

	public bool ProductStatus { get; set; }

	public decimal Amount { get; set; }

	public string AmountForDisplay => Amount.ToString(3);

	public Guid ProductGroupGuid { get; set; }
}
