using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class ChequeMOperationFilter
{
	public Guid? ChequeGuid { get; set; }

	public ENUMCheck? LastStatus { get; set; }

	public int SanadNO { get; set; }

	public Guid? AsGuid { get; set; }

	public Document? AsType { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public int? StatusId { get; set; }
}
