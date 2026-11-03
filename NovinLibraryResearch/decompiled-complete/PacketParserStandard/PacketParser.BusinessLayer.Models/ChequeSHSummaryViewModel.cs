using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ChequeSHSummaryViewModel : ChequeSummaryViewModel
{
	public ENCheckSHStatus Status { get; set; }
}
