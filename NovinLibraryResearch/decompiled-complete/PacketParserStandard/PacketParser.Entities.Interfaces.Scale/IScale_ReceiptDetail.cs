using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_ReceiptDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid HeaderRowGuid { get; set; }

	short ItemNumber { get; set; }

	Guid ProductGuid { get; set; }

	string Name { get; set; }

	int Price { get; set; }

	int Discount { get; set; }

	int TotalDiscount { get; set; }

	int PluTax { get; set; }

	int TotalDetailTax { get; set; }

	int WeightQuantity { get; set; }

	string Unit { get; set; }

	int TotalPrice { get; set; }
}
