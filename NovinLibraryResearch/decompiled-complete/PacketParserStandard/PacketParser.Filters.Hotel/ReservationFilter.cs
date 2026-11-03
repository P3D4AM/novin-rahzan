using System;

namespace PacketParser.Filters.Hotel;

public class ReservationFilter
{
	public bool? Status { get; set; } = true;


	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public int NumberFrom { get; set; }

	public int NumberTo { get; set; }
}
