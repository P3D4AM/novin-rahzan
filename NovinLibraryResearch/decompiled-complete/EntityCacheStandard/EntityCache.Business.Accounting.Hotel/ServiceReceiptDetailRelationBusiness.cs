using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;

namespace EntityCache.Business.Accounting.Hotel;

public class ServiceReceiptDetailRelationBusiness : IServiceReceiptDetailRelation, IHasContext
{
	public string ContextGuid { get; set; }

	public Guid ReceiptDetailGuid { get; set; }

	public Guid ServiceGuid { get; set; }

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal DiscountPrice { get; set; }

	public decimal DiscountPercent { get; set; }

	public decimal AvarezPrice { get; set; }

	public decimal AvarezPercent { get; set; }

	public decimal TaxPrice { get; set; }

	public decimal TaxPercent { get; set; }

	public ServiceReceiptDetailRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
