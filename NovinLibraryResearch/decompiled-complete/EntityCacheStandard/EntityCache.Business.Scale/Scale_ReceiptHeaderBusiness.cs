using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_ReceiptHeaderBusiness : IScale_ReceiptHeader, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public string WorkstationIp { get; set; }

	public int SerialNumber { get; set; }

	public int ReceiptNumber { get; set; }

	public DateTime ReceiptTime { get; set; }

	public string GhorfehName { get; set; }

	public short WorkstationNumber { get; set; }

	public string UserName { get; set; }

	public short InvoiceType { get; set; }

	public int DiscountSupercharge { get; set; }

	public int TotalAmounts { get; set; }

	public int TotalWeights { get; set; }

	public int TotalItems { get; set; }

	public int TotalTax { get; set; }

	public short Tax { get; set; }

	public int TotalItemFromPlu { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_ReceiptHeaderBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
