using System;

namespace PacketParser.Filters.Configs;

public class AghsatMDelayedConfig
{
	public int MaxDelayDayCount { get; set; }

	public DateTime DateM { get; set; }

	public decimal PenaltyPercent { get; set; }
}
