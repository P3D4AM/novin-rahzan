using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IViewSanadDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DateM { get; set; }

	string Description { get; set; }

	Guid? AsGuid { get; set; }

	Document AsType { get; set; }

	decimal Debit { get; set; }

	decimal Credit { get; set; }

	int SanadNo { get; set; }

	SanadSt SanadSt { get; set; }

	decimal Qt { get; set; }

	Guid? DecimalGuid { get; set; }

	string RowDesc { get; set; }

	Guid KolGuid { get; set; }

	Guid MoeinGuid { get; set; }

	Guid? TafsilGuid { get; set; }

	Guid? TafsilGuid2 { get; set; }

	Guid? TafsilGuid3 { get; set; }

	Guid? TafsilGuid4 { get; set; }

	Guid CashierGuid { get; set; }

	string Serial { get; set; }
}
