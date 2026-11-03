using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class PardakhtChequeFilter
{
	public Guid? ChequeGuid { get; set; }

	public string Number { get; set; } = "";


	public ENCheckSHStatus? Status { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public DateTime? MasterDateFrom { get; set; }

	public DateTime? MasterDateTo { get; set; }

	public bool? IsSayadiRegistered { get; set; }
}
