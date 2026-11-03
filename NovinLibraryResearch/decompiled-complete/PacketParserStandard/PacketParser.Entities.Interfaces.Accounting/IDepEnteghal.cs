using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IDepEnteghal : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid UserGuid { get; set; }

	int No { get; set; }

	string Description { get; set; }

	DateTime DateM { get; set; }

	DateTime ModifiedM { get; set; }

	string MashinNo { get; set; }

	string GirandeName { get; set; }

	string RanandehName { get; set; }

	short StatusId { get; set; }

	Guid? DebitMoeinGuid { set; }

	Guid? DebitTafsilGuid { set; }

	Guid? CreditMoeinGuid { set; }

	Guid? CreditTafsilGuid { set; }

	int SanadNo { get; set; }
}
