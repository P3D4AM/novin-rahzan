using System;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

[Table("Hotel_ServiceReceiptDetailRelation")]
public class ServiceReceiptDetailRelation : IServiceReceiptDetailRelation
{
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

	public ReceiptDetail ReceiptDetail { get; set; }

	public Product Product { get; set; }
}
