using System;

namespace PacketParser.Filters.Hotel;

public class PlacementKardexFilter
{
	public Guid? PlacementGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public int NumberFrom { get; set; }

	public int NumberTo { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public int PeopleCountFrom { get; set; }

	public int PeopleCountTo { get; set; }

	public decimal SumFrom { get; set; }

	public decimal SumTo { get; set; }
}
