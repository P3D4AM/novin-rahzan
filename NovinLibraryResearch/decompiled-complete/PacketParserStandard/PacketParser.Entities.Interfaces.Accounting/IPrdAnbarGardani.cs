using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IPrdAnbarGardani : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo
{
	DateTime DateM { get; set; }

	DateTime DateSabt { get; set; }

	SanadSt SanadSt { get; set; }

	Guid? KasrDebitTafsilGuid { get; set; }

	Guid KasrDebitMoeinGuid { get; set; }

	Guid? KasrCreditTafsilGuid { get; set; }

	Guid KasrCreditMoeinGuid { get; set; }

	Guid? EzafeDebitTafsilGuid { get; set; }

	Guid? EzafeCreditTafsilGuid { get; set; }

	Guid EzafeDebitMoeinGuid { get; set; }

	Guid EzafeCreditMoeinGuid { get; set; }

	decimal KasrSum { get; }

	decimal EzafeSum { get; }

	Guid UserGuid { get; set; }

	string Description { get; set; }

	short StatusId { get; set; }

	SanadType Type { get; set; }

	DateTime? DateUtc { get; set; }
}
