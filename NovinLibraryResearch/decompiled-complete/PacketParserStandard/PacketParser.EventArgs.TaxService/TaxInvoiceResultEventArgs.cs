using System;
using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.EventArgs.TaxService;

public class TaxInvoiceResultEventArgs
{
	public string InternaleSerial { get; set; }

	public Guid OrderGuid { get; set; }

	public Document OrderType { get; set; }

	public EnTaxResult TaxResult { get; set; }

	public string TaxRefrenceNumber { get; set; }

	public string TaxResultErrorCode { get; set; }

	public string TaxResultMessage { get; set; }

	public DateTime? TaxSendDate { get; set; }

	public string Uid { get; set; }

	[Obsolete("", true)]
	public ENAction Action
	{
		get
		{
			return (ENAction)checked((short)Subject);
		}
		set
		{
			Subject = (EnTaxInvoiceSubject)value;
		}
	}

	public EnTaxInvoiceSubject Subject { get; set; }

	[DefaultValue("")]
	public string BaseTaxOrderId { get; set; } = "";


	[DefaultValue("")]
	public string InvoiceSeries { get; set; } = "";

}
