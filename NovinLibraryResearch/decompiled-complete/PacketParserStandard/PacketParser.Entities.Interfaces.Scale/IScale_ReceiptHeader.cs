using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_ReceiptHeader : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string WorkstationIp { get; set; }

	int SerialNumber { get; set; }

	int ReceiptNumber { get; set; }

	DateTime ReceiptTime { get; set; }

	string GhorfehName { get; set; }

	short WorkstationNumber { get; set; }

	string UserName { get; set; }

	short InvoiceType { get; set; }

	int DiscountSupercharge { get; set; }

	int TotalAmounts { get; set; }

	int TotalWeights { get; set; }

	int TotalItems { get; set; }

	int TotalTax { get; set; }

	short Tax { get; set; }

	int TotalItemFromPlu { get; set; }
}
