using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_SaleDetailBusiness : IScale_SaleDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid ScaleGuid { get; set; }

	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public int PluNumber { get; set; }

	public string UnitName { get; set; }

	public decimal UnitPrice { get; set; }

	public decimal Discount { get; set; }

	public decimal Tax { get; set; }

	public decimal TotalPrice { get; set; }

	public decimal QT { get; set; }

	public int IM { get; set; }

	public short Order { get; set; }

	public decimal AccountingUnitPrice { get; set; }

	public decimal AccountingDiscount { get; set; }

	public int MyProperty { get; set; }

	public decimal AccountingTax { get; set; }

	public decimal AccountingTotalPrice { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_SaleDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
