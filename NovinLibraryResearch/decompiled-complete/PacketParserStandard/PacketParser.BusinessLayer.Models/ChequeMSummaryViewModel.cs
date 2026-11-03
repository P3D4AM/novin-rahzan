using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ChequeMSummaryViewModel : ChequeSummaryViewModel
{
	public ENUMCheck Status { get; set; }
}
