using System;
using PacketParser.Entities;

namespace PacketParser.Records.Aghsat;

public record AghsatMGroupRecord
{
	public Guid TafsilGuid { get; set; }

	public HesabType HesabType { get; set; }

	public decimal Price { get; set; }

	public int Count { get; set; } = 1;


	public string Description { get; set; }

	public DateTime FirstDate { get; set; } = DateTime.Now;


	public EnRasGiriType RasGiriType { get; set; }

	public Guid? GroupGuid { get; set; }

	public decimal ProfitPercent { get; set; }

	public Guid? DebitMoeinGuid { get; set; }
}
