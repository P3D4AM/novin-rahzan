using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ChequeMInstantModel : ChequeInstantModel
{
	public ENUMCheck? Status { get; set; }
}
