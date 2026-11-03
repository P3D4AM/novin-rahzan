using System;

namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface IServiceReceiptDetailRelation
{
	Guid ReceiptDetailGuid { get; set; }

	Guid ServiceGuid { get; set; }

	decimal Quantity { get; set; }

	decimal Price { get; set; }

	decimal DiscountPrice { get; set; }

	decimal DiscountPercent { get; set; }

	decimal AvarezPrice { get; set; }

	decimal AvarezPercent { get; set; }

	decimal TaxPrice { get; set; }

	decimal TaxPercent { get; set; }
}
