using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ISanad_Det : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid MoeinGuid { get; }

	Guid? TafsilGuid { get; }

	Guid? TafsilGuid2 { get; }

	Guid? TafsilGuid3 { get; }

	Guid? TafsilGuid4 { get; }

	decimal Debit { get; set; }

	decimal Credit { get; set; }

	Guid? DecimalGuid { get; set; }

	decimal Qt { get; set; }

	string DescriptionForUi { get; }

	string RowDesc { get; set; }

	string Serial { get; set; }

	Document AsType { get; set; }

	Guid AsGuid { get; set; }

	DateTime? DateSarresid { get; set; }

	int Ordinal { get; }
}
