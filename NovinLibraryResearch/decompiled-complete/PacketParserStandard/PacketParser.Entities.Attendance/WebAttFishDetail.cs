using System;
using PacketParser.Entities.Interfaces.Attendance;

namespace PacketParser.Entities.Attendance;

public class WebAttFishDetail : IAttFishDetail
{
	public int HesabId { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public decimal TmpValue { get; set; }

	public string Description { get; set; }

	public short TypeAccount { get; set; }

	public short Type { get; set; }

	public string Name { get; set; }

	public bool IsDecrease { get; set; }

	public string Formula { get; set; }

	public int Privilege { get; set; }

	public decimal TotalPayable { get; set; }

	public decimal RemainingBeforeBill { get; set; }

	public DateTime StartMounthPayM { get; set; }

	public long CodeMoein { get; set; }

	public long CodeTafsil { get; set; }

	public bool Taxable { get; set; }

	public bool Insurance { get; set; }

	public bool DisableCompute { get; set; }
}
