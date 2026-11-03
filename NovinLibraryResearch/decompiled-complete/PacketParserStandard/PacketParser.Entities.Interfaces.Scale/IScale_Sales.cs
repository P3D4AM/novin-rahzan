using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Sales : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string TotalBarcode { get; set; }

	Guid MemoryGuid { get; set; }

	DateTime DateM { get; set; }

	string DateSH { get; set; }

	DateTime ReadDateM { get; set; }

	string MyProperty { get; set; }

	string ReadDateSH { get; set; }

	string PortIP { get; set; }

	short Count { get; set; }

	decimal Sum { get; set; }

	decimal Tax { get; set; }

	decimal Discount { get; set; }

	decimal TotalSum { get; set; }

	string ScaleNumber { get; set; }

	Guid ScaleGuid { get; set; }

	int FactorNumber { get; set; }

	int ClerkNumber { get; set; }

	string ScaleName { get; set; }

	decimal AccountingSum { get; set; }

	decimal AccountingDiscount { get; set; }

	decimal AccountingTax { get; set; }

	decimal AccountingTotalSum { get; set; }

	Guid AccountingRegisterGuid { get; set; }

	DateTime AccountingRegisterTime { get; set; }

	string CustomerFullName { get; set; }

	int AccountingFactorNumber { get; set; }

	bool AccountingRegisterStatus { get; set; }
}
