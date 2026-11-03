using System;
using System.Collections.Generic;
using PacketParser.EventArgs.TaxService;

namespace PacketParser.Entities.MetaData;

public class OrderTaxMetaData
{
	public EnTaxBillType TaxBillType { get; set; }

	public EnTaxInvoiceTemplate Template { get; set; }

	public string ContractUniqueId { get; set; }

	public string CustomsLicenseNumber { get; set; }

	public string CustomsCode { get; set; }

	public DateTime? CottageDate { get; set; }

	public string CottageNumber { get; set; }

	public EnTaxTasvieMethod? TaxTasvieMethod { get; set; }

	public List<TaxInvoiceTrackingEventArgs> ResultList { get; set; }
}
