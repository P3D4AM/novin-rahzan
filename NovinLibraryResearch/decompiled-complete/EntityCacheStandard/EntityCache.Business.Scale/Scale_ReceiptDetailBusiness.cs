using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_ReceiptDetailBusiness : IScale_ReceiptDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid HeaderRowGuid { get; set; }

	public short ItemNumber { get; set; }

	public Guid ProductGuid { get; set; }

	public string Name { get; set; }

	public int Price { get; set; }

	public int Discount { get; set; }

	public int TotalDiscount { get; set; }

	public int PluTax { get; set; }

	public int TotalDetailTax { get; set; }

	public int WeightQuantity { get; set; }

	public string Unit { get; set; }

	public int TotalPrice { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_ReceiptDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
