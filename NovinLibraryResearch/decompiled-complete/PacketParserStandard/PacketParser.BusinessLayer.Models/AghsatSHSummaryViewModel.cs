using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class AghsatSHSummaryViewModel : ChequeSummaryViewModel
{
	public EnAghsatShStatus Status { get; set; }
}
