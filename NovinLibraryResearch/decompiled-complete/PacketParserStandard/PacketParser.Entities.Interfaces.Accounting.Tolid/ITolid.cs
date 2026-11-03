using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolid : IHasJustGuid, IHasModified, IHasSanadNo
{
	int Id2 { get; set; }

	DateTime DateM { get; set; }

	new int SanadNo { get; set; }

	SanadSt SanadSt { get; set; }

	Guid CashierGuid { get; set; }

	string Description { get; set; }

	decimal SumMavad { get; set; }

	decimal SumMahsol { get; set; }

	decimal SumHazine { get; set; }

	Guid MavadMoeinGuid { get; set; }

	Guid? MavadTafsilGuid { get; set; }

	Guid MahsolMoeinGuid { get; set; }

	Guid? MahsolTafsilGuid { get; set; }

	bool SanadAffective { get; set; }

	bool KardexAffective { get; set; }
}
