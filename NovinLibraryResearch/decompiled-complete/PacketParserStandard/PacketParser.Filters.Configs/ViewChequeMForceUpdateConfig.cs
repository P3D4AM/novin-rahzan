using System;
using PacketParser.Entities;

namespace PacketParser.Filters.Configs;

public class ViewChequeMForceUpdateConfig
{
	public Guid ChequeGuid { get; set; }

	public ENUMCheck Status { get; set; }

	public string Place { get; set; } = "";


	public DateTime? DateToPlace { get; set; }

	public Guid? DebitTafsilGuid { get; set; }
}
