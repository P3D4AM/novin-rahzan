using System;

namespace PacketParser.Records.TafsilRecords;

public record TafsilInstallmentsRecord
{
	public Guid TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public decimal Account { get; set; }

	public decimal AghsatMCount { get; set; }

	public decimal AghsatMSum { get; set; }

	public decimal AghsatShCount { get; set; }

	public decimal AghsatShSum { get; set; }

	public decimal SumAghsat => AghsatShSum - AghsatMSum;

	public decimal AccountWithAghsat => Account - SumAghsat;
}
