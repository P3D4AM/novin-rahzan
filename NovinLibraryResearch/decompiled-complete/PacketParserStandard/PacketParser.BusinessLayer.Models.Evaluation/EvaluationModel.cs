using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.BusinessLayer.Models.Evaluation;

public class EvaluationModel : IEvaluation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public string Description { get; set; }

	public Guid AnbarGardaniEzafeGuid { get; set; }

	public decimal AnbarGardaniEzafeSum { get; set; }

	public Guid AnbarGardaniKasrGuid { get; set; }

	public decimal AnbarGardaniKasrSum { get; set; }
}
