using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;

[Table("Acc_TaxOrderLog")]
public class TaxOrderLog : ITaxOrderLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid OrderGuid { get; set; }

	public EnTaxResult OldState { get; set; }

	public EnTaxResult? NewState { get; set; }

	public EnTaxInvoiceSubject Subject { get; set; }

	public DateTime? TaxSendDate { get; set; }

	public string? TaxRefrenceNumber { get; set; }

	public DateTime? TaxDeliveryDate { get; set; }

	public string? TaxResultErrorCode { get; set; }

	public string? TaxResultMessage { get; set; }

	public string? TaxUid { get; set; }

	public string? TaxOrderId { get; set; }

	public DateTime InsertDate { get; set; }

	public bool IsSystem { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public short Ordinal { get; set; }

	public string? BaseTaxOrderId { get; set; }

	public string? InvoiceSeries { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	[NotMapped]
	public bool Status { get; set; }

	public Order OrderItem { get; set; }
}
