using System;

namespace PacketParser.Entities.Interfaces.Accounting.Sanad;

public interface IViewSanadDetailCover : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid? DebitMoeinGuid { get; set; }

	Guid? DebitTafsilGuid { get; set; }

	DateTime Date { get; set; }

	Guid AsGuid { get; set; }

	DocumentDet AsType { get; set; }

	decimal Debit { get; set; }

	decimal Credit { get; set; }

	Guid? CashierGuid { get; set; }

	int SanadNo { get; set; }

	SanadSt SanadSt { get; set; }

	int Id2 { get; set; }

	Guid? CreditMoeinGuid { get; set; }

	Guid? CreditTafsilGuid { get; set; }

	decimal Qt { get; set; }

	decimal Price { get; set; }

	decimal Qt2 { get; set; }

	decimal RDiscount { get; set; }

	decimal RBazaryab { get; set; }

	string Serial { get; set; }

	string SanadDesc { get; set; }

	string RDesc { get; set; }

	Guid? ProductGuid { get; set; }

	Guid? DepotGuid { get; set; }

	Guid? VahedGuid { get; set; }

	DateTime? DateMoaser { get; set; }

	Guid? DecimalGuid { get; set; }

	decimal DecimalQt { get; set; }

	decimal DecimalPrice { get; set; }

	Guid? DetailGuid { get; set; }

	Document AsTypeDocument { get; set; }

	Guid? TafsilGuid2 { get; set; }

	Guid? TafsilGuid3 { get; set; }

	Guid? TafsilGuid4 { get; set; }
}
