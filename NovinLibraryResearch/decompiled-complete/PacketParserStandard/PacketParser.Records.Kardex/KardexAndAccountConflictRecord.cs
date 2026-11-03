using System;

namespace PacketParser.Records.Kardex;

public record KardexAndAccountConflictRecord
{
	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string TafsilName { get; set; }

	public decimal Account { get; set; }

	public decimal KardexAccount { get; set; }

	public decimal Conflict { get; set; }
}
