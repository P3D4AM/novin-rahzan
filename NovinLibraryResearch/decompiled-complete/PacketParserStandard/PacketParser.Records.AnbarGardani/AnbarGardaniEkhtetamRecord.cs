using System;

namespace PacketParser.Records.AnbarGardani;

public record AnbarGardaniEkhtetamRecord
{
	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public decimal Amount { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public string DepotName { get; set; }

	public string UnitName { get; set; }

	public Guid GroupGuid { get; set; }

	public decimal UnitPrice { get; set; }
}
