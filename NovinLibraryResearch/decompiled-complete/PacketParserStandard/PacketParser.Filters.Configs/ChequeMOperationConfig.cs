using System;
using PacketParser.Entities;

namespace PacketParser.Filters.Configs;

public class ChequeMOperationConfig
{
	public Guid? Guid { get; set; }

	public Guid ChequeGuid { get; set; }

	public EnSystem System { get; set; } = EnSystem.UserInterface;


	public Guid? DebitTafsilGuid { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public int SanadNo { get; set; }

	public bool? IsAvalDoreTransfered { get; set; }

	public Guid? CashierGuid { get; set; }

	public string Description { get; set; } = "";


	public bool IsSanadAffective { get; set; } = true;


	public string SanadDesc { get; set; } = "";


	public short SanadStatusId { get; set; }

	public ENUMCheck? OldState { get; set; }

	public int? Ordinal { get; set; }
}
