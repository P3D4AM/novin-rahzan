using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class ChequeShOperationFilter
{
	public Guid? ChequeGuid { get; set; }

	public ENCheckSHStatus? LastStatus { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public int? StatusId { get; set; }
}
