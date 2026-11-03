using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class AnbarGardaniFilter
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public string Description { get; set; } = "";


	public bool? Status { get; set; }

	public Guid? DepotGuid { get; set; }

	public bool LoadAllProductsInEditMod { get; set; }

	public SanadType? Type { get; set; }

	public int? StatusId { get; set; }
}
