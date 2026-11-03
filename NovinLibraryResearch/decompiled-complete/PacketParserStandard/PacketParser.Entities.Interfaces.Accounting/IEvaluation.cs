using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IEvaluation : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Id2 { get; set; }

	DateTime DateM { get; set; }

	int SanadNo { get; set; }

	SanadSt SanadSt { get; set; }

	Guid CashierGuid { get; set; }

	string Description { get; set; }

	Guid AnbarGardaniEzafeGuid { get; set; }

	Guid AnbarGardaniKasrGuid { get; set; }
}
