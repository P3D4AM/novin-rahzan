using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class KardexSummaryFilter
{
	public Document FactorType { get; set; }

	public Guid? MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? ProductGuid { get; set; }

	public Guid? DepotGuid { get; set; }

	public bool? KardexAffective { get; set; } = true;


	public KardexType? Type { get; set; }
}
