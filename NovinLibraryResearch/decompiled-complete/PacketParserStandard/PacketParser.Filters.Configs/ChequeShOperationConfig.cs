using System;
using PacketParser.Entities;

namespace PacketParser.Filters.Configs;

public class ChequeShOperationConfig
{
	public Guid ChequeGuid { get; set; }

	public Guid? Guid { get; set; }

	public EnSystem System { get; set; } = EnSystem.UserInterface;


	public DateTime DateM { get; set; } = DateTime.Now;


	public int SanadNo { get; set; }

	public Guid? CashierGuid { get; set; }

	public string SanadDesc { get; set; } = "";


	public short SanadStatusId { get; set; }
}
