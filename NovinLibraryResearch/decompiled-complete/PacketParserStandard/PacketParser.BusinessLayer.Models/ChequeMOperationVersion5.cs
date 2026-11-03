using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ChequeMOperationVersion5
{
	private ENUMCheck newState;

	private ENUMCheck oldState;

	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public Guid CashierGuid { get; set; }

	public int SanadNo { get; set; }

	public bool? AvalDoreTransfer { get; set; }

	public Guid ChequeGuid { get; set; }

	public Guid? MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public ENUMCheck NewState
	{
		get
		{
			return newState;
		}
		set
		{
			newState = value;
			CheckValidation();
		}
	}

	public ENUMCheck OldState
	{
		get
		{
			return oldState;
		}
		set
		{
			oldState = value;
			CheckValidation();
		}
	}

	public short Ordinal { get; set; }

	public string Description { get; set; }

	public DateTime? TransferDate { get; set; }

	private void CheckValidation()
	{
		if (NewState == ENUMCheck.Mojod && OldState == ENUMCheck.Mojod)
		{
			throw new InvalidOperationException();
		}
	}
}
