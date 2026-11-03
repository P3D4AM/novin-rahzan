using System;

namespace PacketParser.Entities.Interfaces.Accounting.Aghsat;

public interface IAghsatM_Group : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	decimal Sum { get; set; }

	int Count { get; set; }

	Guid TafsilGuid { get; set; }

	Guid MoeinGuid { set; }

	Guid CashierGuid { get; set; }

	int? SanadNo { get; set; }

	decimal ProfitSum { get; set; }

	Guid? CreditMoeinGuid { set; }

	Guid? CreditTafsilGuid { set; }
}
