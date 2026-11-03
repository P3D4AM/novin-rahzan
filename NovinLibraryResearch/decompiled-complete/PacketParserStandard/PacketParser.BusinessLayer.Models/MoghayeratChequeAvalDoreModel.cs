using System;

namespace PacketParser.BusinessLayer.Models;

public class MoghayeratChequeAvalDoreModel
{
	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public string MoeinName { get; set; }

	public string TafsilName { get; set; }

	public decimal SumAvalDoreFromCheque { get; set; }

	public decimal SumAvalDoreFromSanad { get; set; }
}
