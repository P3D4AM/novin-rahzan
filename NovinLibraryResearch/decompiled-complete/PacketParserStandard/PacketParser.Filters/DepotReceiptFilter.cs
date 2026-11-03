using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class DepotReceiptFilter
{
	public EnDepotReceiptType Type { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public bool? IsTemp { get; set; }

	public bool? SanadAffective { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? CashierGuid { get; set; }
}
