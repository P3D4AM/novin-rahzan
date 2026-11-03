using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IAghsatAshkhas : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Id2 { get; set; }

	string Description { get; set; }

	DateTime DateSarResid { get; set; }

	DateTime Date { get; set; }

	Guid? ReceptionGuid { get; set; }

	DateTime? ReceptionDateM { get; set; }

	decimal ReceptionSum { get; set; }

	decimal Price { set; }

	int TypePardakht { get; set; }

	string Mounth { get; set; }

	Guid GuidGroup { get; set; }

	EnRasGiriType RasGiriType { get; set; }

	decimal VajhEltezam { get; set; }

	DateTime? DateUtc { get; set; }

	int? SanadNo { get; set; }

	DateTime? VajhEltezamModified { get; set; }

	Guid? CreditMoeinGuid { set; }

	Guid? CreditTafsilGuid { set; }
}
