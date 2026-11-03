using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ChequeShOperationVersion5
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public Guid CashierGuid { get; set; }

	public int SanadNo { get; set; }

	public Guid ChequeGuid { get; set; }

	public ENCheckSHStatus NewState { get; set; }

	public ENCheckSHStatus OldState { get; set; }

	public short Ordinal { get; set; }
}
