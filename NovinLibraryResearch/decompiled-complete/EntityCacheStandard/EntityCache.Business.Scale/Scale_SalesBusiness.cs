using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_SalesBusiness : IScale_Sales, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public string TotalBarcode { get; set; }

	public Guid MemoryGuid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSH { get; set; }

	public DateTime ReadDateM { get; set; }

	public string MyProperty { get; set; }

	public string ReadDateSH { get; set; }

	public string PortIP { get; set; }

	public short Count { get; set; }

	public decimal Sum { get; set; }

	public decimal Tax { get; set; }

	public decimal Discount { get; set; }

	public decimal TotalSum { get; set; }

	public string ScaleNumber { get; set; }

	public Guid ScaleGuid { get; set; }

	public int FactorNumber { get; set; }

	public int ClerkNumber { get; set; }

	public string ScaleName { get; set; }

	public decimal AccountingSum { get; set; }

	public decimal AccountingDiscount { get; set; }

	public decimal AccountingTax { get; set; }

	public decimal AccountingTotalSum { get; set; }

	public Guid AccountingRegisterGuid { get; set; }

	public DateTime AccountingRegisterTime { get; set; }

	public string CustomerFullName { get; set; }

	public int AccountingFactorNumber { get; set; }

	public bool AccountingRegisterStatus { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_SalesBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
