using System;

namespace PacketParser.Filters.Global;

public class SarNakhFilter
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public bool? Status { get; set; }

	public Guid? CashierGuid { get; set; }

	public int? StatusId { get; set; }

	public Guid? IntroductionGuid { get; set; }
}
