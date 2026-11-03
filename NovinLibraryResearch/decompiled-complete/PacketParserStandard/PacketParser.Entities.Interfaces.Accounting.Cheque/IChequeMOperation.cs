using System;

namespace PacketParser.Entities.Interfaces.Accounting.Cheque;

public interface IChequeMOperation : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DateM { get; set; }

	Guid CashierGuid { get; set; }

	int SanadNO { get; set; }

	SanadSt SanadSt { get; set; }

	DateTime DateSabt { get; set; }

	bool? Avale2re_Transfer { get; set; }

	Guid GuidCheque { get; set; }

	Guid CreditMoeinGuid { get; set; }

	Guid? CreditTafsilGuid { get; set; }

	Guid DebitMoeinGuid { get; set; }

	Guid? DebitTafsilGuid { get; set; }

	short Ordinal { get; set; }

	ENUMCheck? OldState { get; set; }

	ENUMCheck NewState { get; set; }

	bool? IsDebitTafsilAffective { get; set; }

	bool? IsCreditTafsilAffective { get; set; }

	string Description { get; set; }

	Document? AsType { get; set; }

	Guid? AsGuid { get; set; }

	bool IsSanadAffective { get; set; }

	short StatusId { get; set; }

	DateTime? DateUtc { get; set; }

	decimal Price { get; set; }
}
