using System;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttFishDetail
{
	int HesabId { get; set; }

	decimal Debit { get; set; }

	decimal Credit { get; set; }

	decimal TmpValue { get; set; }

	string Description { get; set; }

	short TypeAccount { get; set; }

	short Type { get; set; }

	string Name { get; set; }

	bool IsDecrease { get; set; }

	string Formula { get; set; }

	int Privilege { get; set; }

	decimal TotalPayable { get; set; }

	decimal RemainingBeforeBill { get; set; }

	DateTime StartMounthPayM { get; set; }

	bool Taxable { get; set; }

	bool Insurance { get; set; }

	bool DisableCompute { get; set; }
}
