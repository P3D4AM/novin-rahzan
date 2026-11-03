using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_SaleDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ScaleGuid { get; set; }

	Guid ProductGuid { get; set; }

	string ProductCode { get; set; }

	string ProductName { get; set; }

	int PluNumber { get; set; }

	string UnitName { get; set; }

	decimal UnitPrice { get; set; }

	decimal Discount { get; set; }

	decimal Tax { get; set; }

	decimal TotalPrice { get; set; }

	decimal QT { get; set; }

	int IM { get; set; }

	short Order { get; set; }

	decimal AccountingUnitPrice { get; set; }

	decimal AccountingDiscount { get; set; }

	int MyProperty { get; set; }

	decimal AccountingTax { get; set; }

	decimal AccountingTotalPrice { get; set; }
}
