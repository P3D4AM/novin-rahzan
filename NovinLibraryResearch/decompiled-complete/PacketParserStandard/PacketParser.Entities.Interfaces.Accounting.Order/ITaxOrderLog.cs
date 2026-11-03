using System;

namespace PacketParser.Entities.Interfaces.Accounting.Order;

public interface ITaxOrderLog : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid OrderGuid { get; set; }

	EnTaxResult OldState { get; set; }

	EnTaxResult? NewState { get; set; }

	EnTaxInvoiceSubject Subject { get; set; }

	DateTime? TaxSendDate { get; set; }

	string TaxRefrenceNumber { get; set; }

	DateTime? TaxDeliveryDate { get; set; }

	string TaxResultErrorCode { get; set; }

	string TaxResultMessage { get; set; }

	string TaxUid { get; set; }

	string TaxOrderId { get; set; }

	DateTime InsertDate { get; set; }

	bool IsSystem { get; set; }

	short Ordinal { get; set; }

	string BaseTaxOrderId { get; set; }

	string InvoiceSeries { get; set; }
}
