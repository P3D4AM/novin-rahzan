using System;

namespace PacketParser.Records.Aghsat;

public record AghsatMRecord
{
	public Guid? GroupGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public decimal ReceptionSum { get; set; }

	public bool CalculateInstallments { get; set; }
}
