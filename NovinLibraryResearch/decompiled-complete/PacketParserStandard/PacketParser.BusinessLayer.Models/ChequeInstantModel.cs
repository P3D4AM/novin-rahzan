using System;

namespace PacketParser.BusinessLayer.Models;

public class ChequeInstantModel
{
	public Guid ChequeGuid { get; set; }

	public Guid? OperationGuid { get; set; }

	public DateTime AvalDoreDate { get; set; }
}
