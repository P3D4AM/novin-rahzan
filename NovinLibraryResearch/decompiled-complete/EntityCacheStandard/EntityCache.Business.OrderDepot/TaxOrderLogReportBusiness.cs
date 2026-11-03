using System;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.OrderDepot;

public class TaxOrderLogReportBusiness
{
	public Guid Guid { get; set; }

	public Guid OrderGuid { get; set; }

	public EnTaxResult OldState { get; set; }

	public string ActionForDisplay => OldState.GetDisplay().GetTranslate();

	public EnTaxResult? NewState { get; set; }

	public string TaxResultForDisplay
	{
		get
		{
			if (NewState.GetValueOrDefault() != 0)
			{
				return NewState.Value.GetDisplay();
			}
			return "";
		}
	}

	public EnTaxInvoiceSubject Subject { get; set; }

	public string SubjectForDisplay => Subject.GetDisplay().GetTranslate();

	public DateTime? TaxSendDate { get; set; }

	public string TaxSendDateSh => TaxSendDate.MiladiToCalture();

	public string TaxSendDateTime
	{
		get
		{
			if (TaxSendDate.HasValue)
			{
				return $"{TaxSendDate.Value.Hour}:{TaxSendDate.Value.Minute}";
			}
			return "";
		}
	}

	public string TaxRefrenceNumber { get; set; }

	public DateTime? TaxDeliveryDate { get; set; }

	public string DeliveryDateSh => TaxDeliveryDate.MiladiToCalture();

	public string DeliveryDateTime
	{
		get
		{
			if (TaxDeliveryDate.HasValue)
			{
				return $"{TaxDeliveryDate.Value.Hour}:{TaxDeliveryDate.Value.Minute}";
			}
			return "";
		}
	}

	public string TaxResultErrorCode { get; set; }

	public string TaxResultMessage { get; set; }

	public string TaxUid { get; set; }

	public string TaxOrderId { get; set; }

	public DateTime InsertDate { get; set; }

	public string InsertDateSh => InsertDate.MiladiToCalture();

	public decimal TotalSum { get; set; }

	public decimal Discount { get; set; }

	public decimal Tax { get; set; }

	public DateTime OrderDate { get; set; }

	public string OrderDateSh => OrderDate.MiladiToCalture();

	public Guid TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public int Id2 { get; set; }

	public string BaseTaxOrderId { get; set; }

	public short Ordinal { get; set; }
}
