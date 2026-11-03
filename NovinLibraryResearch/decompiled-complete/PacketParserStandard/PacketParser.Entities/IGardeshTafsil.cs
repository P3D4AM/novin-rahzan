using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IGardeshTafsil : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int RowIndex { get; set; }

	DateTime Date { get; set; }

	Guid? DepotGuid { get; set; }

	string Description { get; set; }

	Guid? AsGuid { get; set; }

	short AsType { get; set; }

	Document EnAsType { get; set; }

	DocumentDet AsTypeEnum { get; set; }

	decimal Debit { get; set; }

	decimal Credit { get; set; }

	int SanadNO { get; set; }

	int SanadSt { get; set; }

	string Serial { get; set; }

	decimal Price { get; set; }

	decimal Discount { get; set; }

	int Id2 { get; set; }

	decimal QT { get; set; }

	decimal QT2 { get; set; }

	decimal Bazaryab { get; set; }

	Guid? VGuid { get; set; }

	string SanadDesc { get; set; }

	DateTime DateMoaser { get; set; }

	decimal Rem { get; set; }

	Guid? DecimalGuid { get; set; }

	string RowDesc { get; set; }

	Guid? TafsilGuid { get; set; }

	Guid MoeinGuid { get; set; }

	Guid KolGuid { get; set; }

	Guid UGuid { get; set; }

	decimal DecimalPrice { get; set; }

	decimal DecimalQt { get; set; }

	Guid DetailGuid { get; set; }
}
