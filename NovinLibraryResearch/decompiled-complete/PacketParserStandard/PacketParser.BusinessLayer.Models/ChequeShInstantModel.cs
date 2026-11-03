using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ChequeShInstantModel : ChequeInstantModel
{
	public ENCheckSHStatus? Status { get; set; }
}
