using System;

namespace PacketParser.Entities;

public class RunScriptInformation
{
	public long Crc { get; set; }

	public long LastExcecutionDuration { get; set; }

	public DateTime? LastSuccessExcecutionTime { get; set; }

	public string LastExcecutionError { get; set; } = "";


	public bool? LastSuccessExcecutionWasSuccess { get; set; }
}
