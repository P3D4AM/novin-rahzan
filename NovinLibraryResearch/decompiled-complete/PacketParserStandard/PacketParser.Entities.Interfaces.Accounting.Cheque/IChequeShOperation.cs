using System;

namespace PacketParser.Entities.Interfaces.Accounting.Cheque;

public interface IChequeShOperation : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	SanadSt SanadSt { get; set; }

	int SanadNo { get; set; }

	DateTime DateM { get; set; }

	Guid CashierGuid { get; set; }

	Guid ChequeGuid { get; set; }

	Guid CreditMoeinGuid { get; set; }

	Guid? CreditTafsilGuid { get; set; }

	Guid DebitMoeinGuid { get; set; }

	Guid? DebitTafsilGuid { get; set; }

	short Ordinal { get; set; }

	ENCheckSHStatus? OldState { get; set; }

	ENCheckSHStatus NewState { get; set; }

	short StatusId { get; set; }

	DateTime? DateUtc { get; set; }

	decimal Price { get; set; }
}
