using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class AghsatMSummaryViewModel : ChequeSummaryViewModel
{
	public EnAghsatMStatus Status { get; set; }
}
