using System;
using PacketParser.Entities;

namespace PacketParser.Records;

public record DocumentRoundMismatchRecord
{
	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public string AsTypeForDisplay => AsType.GetDisplay();

	public int Id2 { get; set; }
}
