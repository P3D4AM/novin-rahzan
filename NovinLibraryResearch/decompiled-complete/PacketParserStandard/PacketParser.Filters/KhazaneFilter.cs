using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class KhazaneFilter
{
	public DependencyType? Type { get; set; }

	public decimal SumFrom { get; set; }

	public decimal SumTo { get; set; }

	public int Id2From { get; set; }

	public int Id2To { get; set; }

	public List<int> Id2 { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public string Description { get; set; } = "";


	public Guid? CashierGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? CustomerGroupGuid { get; set; }

	public bool? Isdelivered { get; set; }

	public bool? IsSent { get; set; }

	public bool? IsTemp { get; set; }

	public string CustomerFullName { get; set; }

	public List<ENSource> Sources { get; set; }

	public string HostRefrences { get; set; } = "";


	public bool? IsDelivered { get; set; }

	public bool? Status { get; set; }

	public HesabType? HesabType { get; set; }

	public Guid? ChequeMGuid { get; set; }

	public bool? HasSumNaghd { get; set; }

	public decimal? NaghdFrom { get; set; }

	public decimal? NaghdTo { get; set; }

	public decimal? HavaleFrom { get; set; }

	public decimal? HavaleTo { get; set; }

	public decimal? ChequeFrom { get; set; }

	public decimal? ChequeTo { get; set; }

	public decimal? ChequeMFrom { get; set; }

	public decimal? ChequeMTo { get; set; }

	public int? StatusId { get; set; }

	public List<ServerStatusForClient> ServerStatuses { get; set; }
}
